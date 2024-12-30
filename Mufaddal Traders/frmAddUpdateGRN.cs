using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics; // <-- For Debug.WriteLine
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmAddUpdateGRN : Form
    {

        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        // Connection string for SQL Server
        private string connectionString = DatabaseConfig.ConnectionString;
        // somewhere at the class level:
        private bool isProgrammaticRadioChange = false;

        public frmAddUpdateGRN()
        {
            Debug.WriteLine("frmAddUpdateGRN constructor called 🐞");
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("btnClose_Click method called 🐞");
            this.Close();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("btnMinimize_Click method called 🐞");
            this.WindowState = FormWindowState.Minimized;
        }

        private void picHeader_MouseDown(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("picHeader_MouseDown method called 🐞");
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("btnBack_Click method called 🐞");
            this.Close();
        }

        private void frmAddUpdateGRN_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("frmAddUpdateGRN_Load method called 🐞");
            LoadNextGRNID();
            LoadWarehouseIDs(); // Load warehouse information
        }

        private void LoadNextGRNID()
        {
            Debug.WriteLine("LoadNextGRNID method called 🐞");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Query to find the smallest available GRN_ID starting from 1
                    string query = @"
            SELECT TOP 1 Number
            FROM (SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Number
                  FROM master.dbo.spt_values) AS Numbers
            WHERE Number NOT IN (SELECT GRN_ID FROM tblGRN)
            ORDER BY Number";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    object result = cmd.ExecuteScalar();
                    txtGRN_ID.Text = result != null ? result.ToString() : "1";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading GRN ID: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void rbPurchaseOrder_CheckedChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("rbPurchaseOrder_CheckedChanged method called 🐞");
            if (isProgrammaticRadioChange) return;

            if (rbPurchaseOrder.Checked)
            {
                ClearSupplierAndItemInformation();
                txtItemQtys.ReadOnly = true;
                txtItemQtys.BackColor = Color.BurlyWood; // Set background color
                LoadPurchaseIDs("O"); // 'O' for Purchase Order
                cmbPurchaseID.SelectedIndex = -1; // Ensure dropdown is reset
            }
        }


        private void rbPurchaseContract_CheckedChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("rbPurchaseContract_CheckedChanged method called 🐞");
            if (isProgrammaticRadioChange) return;

            if (rbPurchaseContract.Checked)
            {
                ClearSupplierAndItemInformation();
                txtItemQtys.ReadOnly = false;
                txtItemQtys.BackColor = SystemColors.Control; // Default background color
                LoadPurchaseIDs("C"); // 'C' for Purchase Contract
                cmbPurchaseID.SelectedIndex = -1; // Ensure dropdown is reset
            }
        }


        private void ClearSupplierAndItemInformation()
        {
            Debug.WriteLine("ClearSupplierAndItemInformation method called 🐞");
            txtSupplierID.Clear();
            txtSupplierName.Clear();
            txtItemIDs.Clear();
            txtItemNames.Clear();
            txtItemQtys.Clear();
            cmbPurchaseID.SelectedIndex = -1; // Clear selected value in the combo box
            cmbPurchaseID.Text = string.Empty; // Clear text in the combo box
        }

        private void LoadPurchaseIDs(string purchaseType)
        {
            Debug.WriteLine("LoadPurchaseIDs method called 🐞");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query;
                    if (purchaseType == "O")
                    {
                        query = @"
                    SELECT DISTINCT PurchaseOrderID 
                    FROM Purchase_Orders 
                    WHERE Status = 'N'";
                    }
                    else
                    {
                        query = "SELECT PurchaseContractID FROM Purchase_Contract";
                    }

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    cmbPurchaseID.Items.Clear();
                    while (reader.Read())
                    {
                        cmbPurchaseID.Items.Add(reader[0].ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Purchase IDs: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cmbPurchaseID_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("cmbPurchaseID_SelectedIndexChanged method called 🐞");
            if (cmbPurchaseID.SelectedItem == null) return; // Prevent null reference exception

            if (rbPurchaseOrder.Checked)
            {
                LoadPurchaseOrderDetails(cmbPurchaseID.SelectedItem.ToString());
            }
            else if (rbPurchaseContract.Checked)
            {
                LoadPurchaseContractDetails(cmbPurchaseID.SelectedItem.ToString());
            }
        }

        private void LoadPurchaseOrderDetails(string purchaseID)
        {
            Debug.WriteLine("LoadPurchaseOrderDetails method called 🐞");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                SELECT po.SupplierID, s.Name AS SupplierName, i.ItemID, i.Item_Name, po.ItemQty
                FROM Purchase_Orders po
                JOIN Items i ON po.ItemID = i.ItemID
                JOIN tblManageSuppliers s ON po.SupplierID = s.SupplierID
                WHERE po.PurchaseOrderID = @PurchaseID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@PurchaseID", purchaseID);
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Clear text fields
                    txtSupplierID.Clear();
                    txtSupplierName.Clear();
                    txtItemIDs.Clear();
                    txtItemNames.Clear();
                    txtItemQtys.Clear();

                    while (reader.Read())
                    {
                        txtSupplierID.Text = reader["SupplierID"].ToString();
                        txtSupplierName.Text = reader["SupplierName"].ToString();
                        txtItemIDs.AppendText(reader["ItemID"].ToString() + Environment.NewLine);
                        txtItemNames.AppendText(reader["Item_Name"].ToString() + Environment.NewLine);
                        txtItemQtys.AppendText(reader["ItemQty"].ToString() + Environment.NewLine);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Purchase Order details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadPurchaseContractDetails(string purchaseID)
        {
            Debug.WriteLine("LoadPurchaseContractDetails method called 🐞");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // We'll select columns that do exist in Purchase_Contract 
                    // but there's no quantity column in that table
                    string query = @"
                SELECT pc.SupplierID,
                       s.Name AS SupplierName,
                       i.ItemID,
                       i.Item_Name
                FROM Purchase_Contract pc
                JOIN Items i ON pc.ItemID = i.ItemID
                JOIN tblManageSuppliers s ON pc.SupplierID = s.SupplierID
                WHERE pc.PurchaseContractID = @PurchaseID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@PurchaseID", purchaseID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // We'll "temporarily" store what's in txtItemQtys
                        // because we only want to overwrite if the table actually
                        // has a quantity (which it does not).
                        string existingQtyText = txtItemQtys.Text;

                        // Clear text fields for IDs, Names, etc.
                        txtSupplierID.Clear();
                        txtSupplierName.Clear();
                        txtItemIDs.Clear();
                        txtItemNames.Clear();
                        // DO NOT clear txtItemQtys here. We'll keep existing text.

                        while (reader.Read())
                        {
                            txtSupplierID.Text = reader["SupplierID"].ToString();
                            txtSupplierName.Text = reader["SupplierName"].ToString();
                            txtItemIDs.AppendText(reader["ItemID"].ToString() + Environment.NewLine);
                            txtItemNames.AppendText(reader["Item_Name"].ToString() + Environment.NewLine);
                        }

                        // Because the Purchase_Contract table has no quantity column,
                        // we leave txtItemQtys as is (which presumably got loaded from the GRN record).
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Purchase Contract details: " + ex.Message,
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadWarehouseIDs()
        {
            Debug.WriteLine("LoadWarehouseIDs method called 🐞");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT StoreID, Store_Name FROM Warehouse";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    cmbWarehouseID.Items.Clear();
                    while (reader.Read())
                    {
                        cmbWarehouseID.Items.Add(reader["StoreID"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Warehouse IDs: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cmbWarehouseID_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("cmbWarehouseID_SelectedIndexChanged method called 🐞");
            if (cmbWarehouseID.SelectedItem == null) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Store_Name FROM Warehouse WHERE StoreID = @StoreID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StoreID", cmbWarehouseID.SelectedItem.ToString());
                    object result = cmd.ExecuteScalar();
                    txtWarehouseName.Text = result?.ToString() ?? string.Empty;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Warehouse Name: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("btnSave_Click method called 🐞");
            try
            {
                // Validate inputs
                if (!ValidateInputs())
                {
                    MessageBox.Show("Please ensure all fields are correctly filled.",
                                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // **Add monthly validation for Purchase Contracts here**
                if (!ValidatePurchaseContractMonthly())
                {
                    Debug.WriteLine("❗ Monthly validation for Purchase Contract failed. Aborting save operation.");
                    return; // Exit if validation fails
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Combine item information into arrays
                    string[] itemIDs = txtItemIDs.Lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
                    string[] itemQuantities = txtItemQtys.Lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

                    if (itemIDs.Length != itemQuantities.Length)
                    {
                        MessageBox.Show("Mismatch between items and quantities.",
                                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Start a transaction
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Insert GRN record
                            string insertGRNQuery = @"
                    INSERT INTO tblGRN (GRN_ID, PurchaseID, PurchaseType, SupplierID, ItemID, ItemQuantity, WarehouseID, GRN_Date, GRN_Type)
                    VALUES (@GRN_ID, @PurchaseID, @PurchaseType, @SupplierID, @ItemIDs, @ItemQuantities, @WarehouseID, GETDATE(), @GRN_Type)";

                            using (SqlCommand cmd = new SqlCommand(insertGRNQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@GRN_ID", txtGRN_ID.Text);
                                cmd.Parameters.AddWithValue(
                                    "@PurchaseID",
                                    cmbPurchaseID.SelectedItem != null ? cmbPurchaseID.SelectedItem.ToString() : cmbPurchaseID.Text
                                );
                                cmd.Parameters.AddWithValue("@PurchaseType", rbPurchaseOrder.Checked ? "O" : "C");
                                cmd.Parameters.AddWithValue("@SupplierID", txtSupplierID.Text);
                                cmd.Parameters.AddWithValue("@ItemIDs", string.Join(",", itemIDs));
                                cmd.Parameters.AddWithValue("@ItemQuantities", string.Join(",", itemQuantities));
                                cmd.Parameters.AddWithValue("@WarehouseID", cmbWarehouseID.SelectedItem.ToString());
                                cmd.Parameters.AddWithValue("@GRN_Type", cmbGRN_Type.SelectedItem?.ToString() ?? string.Empty);

                                cmd.ExecuteNonQuery();
                            }

                            // Update stock balance for each item
                            for (int i = 0; i < itemIDs.Length; i++)
                            {
                                string updateStockBalanceQuery = @"
    MERGE INTO tblStockBalance AS Target
    USING (SELECT @ItemID AS ItemID, @WarehouseID AS WarehouseID, @ItemQty AS ItemQty, @ItemName AS ItemName) AS Source
    ON Target.ItemID = Source.ItemID AND Target.WarehouseID = Source.WarehouseID
    WHEN MATCHED THEN
        UPDATE SET Target.ItemQty = Target.ItemQty + Source.ItemQty
    WHEN NOT MATCHED THEN
        INSERT (ItemID, ItemName, WarehouseID, WarehouseName, ItemQty)
        VALUES (Source.ItemID, Source.ItemName, Source.WarehouseID, 
                (SELECT Store_Name FROM Warehouse WHERE StoreID = Source.WarehouseID), Source.ItemQty);";

                                using (SqlCommand cmd = new SqlCommand(updateStockBalanceQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@ItemID", itemIDs[i]);
                                    cmd.Parameters.AddWithValue("@ItemName", txtItemNames.Lines[i].Trim());
                                    cmd.Parameters.AddWithValue("@WarehouseID", cmbWarehouseID.SelectedItem.ToString());
                                    cmd.Parameters.AddWithValue("@ItemQty", int.Parse(itemQuantities[i]));

                                    cmd.ExecuteNonQuery();
                                }
                            }

                            // If this is a Purchase Order, update its status to 'Y'
                            if (rbPurchaseOrder.Checked)
                            {
                                string updatePurchaseOrderStatusQuery = @"
        UPDATE Purchase_Orders
        SET Status = 'Y'
        WHERE PurchaseOrderID = @PurchaseID";

                                using (SqlCommand cmd = new SqlCommand(updatePurchaseOrderStatusQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@PurchaseID", cmbPurchaseID.SelectedItem.ToString());
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            // Commit transaction
                            transaction.Commit();
                            MessageBox.Show("GRN record saved!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                            LoadNextGRNID();

                        }
                        catch (Exception ex)
                        {
                            // Rollback transaction in case of an error
                            transaction.Rollback();
                            MessageBox.Show("An error occurred: " + ex.Message,
                                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidatePurchaseContractMonthly()
        {
            Debug.WriteLine("ValidatePurchaseContractMonthly method called 🐞");

            if (rbPurchaseContract.Checked)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT COUNT(*) 
                FROM tblGRN 
                WHERE PurchaseType = 'C' AND PurchaseID = @PurchaseID 
                AND MONTH(GRN_Date) = MONTH(GETDATE()) AND YEAR(GRN_Date) = YEAR(GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Check for SelectedItem or use Text as fallback
                        string purchaseID = cmbPurchaseID.SelectedItem != null
                                            ? cmbPurchaseID.SelectedItem.ToString()
                                            : cmbPurchaseID.Text;

                        if (string.IsNullOrWhiteSpace(purchaseID))
                        {
                            MessageBox.Show("Purchase ID is required for Purchase Contract validation.",
                                            "Validation Error",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);
                            return false;
                        }

                        cmd.Parameters.AddWithValue("@PurchaseID", purchaseID);
                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("A GRN for this Purchase Contract has already been created this month.",
                                            "Validation Error",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);
                            return false;
                        }
                    }
                }
            }

            return true;
        }


        private bool ValidateInputs()
        {
            Debug.WriteLine("ValidateInputs method called 🐞");

            if (string.IsNullOrWhiteSpace(txtGRN_ID.Text))
            {
                MessageBox.Show("GRN ID cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Check if Purchase ID is either selected or has valid text
            if ((cmbPurchaseID.SelectedIndex == -1 && string.IsNullOrWhiteSpace(cmbPurchaseID.Text)) ||
                (cmbPurchaseID.SelectedIndex == -1 && !IsPurchaseIDValid(cmbPurchaseID.Text)))
            {
                MessageBox.Show("Please select or enter a valid Purchase ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSupplierID.Text) || string.IsNullOrWhiteSpace(txtSupplierName.Text))
            {
                MessageBox.Show("Supplier information is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtItemIDs.Text) || string.IsNullOrWhiteSpace(txtItemNames.Text))
            {
                MessageBox.Show("Item information is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbWarehouseID.SelectedIndex == -1 || string.IsNullOrWhiteSpace(cmbWarehouseID.Text))
            {
                MessageBox.Show("Please select a valid Warehouse ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }


        private bool IsPurchaseIDValid(string purchaseID)
        {
            Debug.WriteLine($"Validating PurchaseID: {purchaseID}");
            if (string.IsNullOrWhiteSpace(purchaseID)) return false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                SELECT COUNT(1) 
                FROM (
                    SELECT PurchaseOrderID AS PurchaseID FROM Purchase_Orders
                    UNION
                    SELECT PurchaseContractID AS PurchaseID FROM Purchase_Contract
                ) AS AllPurchases
                WHERE PurchaseID = @PurchaseID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@PurchaseID", purchaseID);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error validating Purchase ID: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }



        private void ClearFields()
        {
            Debug.WriteLine("ClearFields method called 🐞");
            txtGRN_ID.Clear();
            cmbPurchaseID.SelectedIndex = -1;
            txtSupplierID.Clear();
            txtSupplierName.Clear();
            txtItemIDs.Clear();
            txtItemNames.Clear();
            txtItemQtys.Clear();
            cmbWarehouseID.SelectedIndex = -1;
            txtWarehouseName.Clear();
            cmbGRN_Type.SelectedIndex = -1; // Reset the GRN Type combo box

            // Reset radio buttons
            rbPurchaseOrder.Checked = false;
            rbPurchaseContract.Checked = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("btnUpdate_Click method invoked 🕾");

            try
            {
                // Validate inputs
                if (!ValidateInputs())
                {
                    MessageBox.Show("Please ensure all fields are correctly filled.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string newPurchaseType = rbPurchaseOrder.Checked ? "O" : "C";
                string newPurchaseID = cmbPurchaseID.SelectedItem != null ? cmbPurchaseID.SelectedItem.ToString() : cmbPurchaseID.Text;
                string newWarehouseID = cmbWarehouseID.SelectedItem.ToString();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Fetch old GRN details (Item IDs, Quantities, and Warehouse)
                            string fetchOldDetailsQuery = @"
                        SELECT ItemID, ItemQuantity, WarehouseID
                        FROM tblGRN
                        WHERE GRN_ID = @GRN_ID";

                            Dictionary<string, int> oldQuantities = new Dictionary<string, int>();
                            string oldWarehouseID = "";

                            using (SqlCommand fetchCmd = new SqlCommand(fetchOldDetailsQuery, conn, transaction))
                            {
                                fetchCmd.Parameters.AddWithValue("@GRN_ID", txtGRN_ID.Text);
                                using (SqlDataReader reader = fetchCmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        oldQuantities[reader["ItemID"].ToString()] = Convert.ToInt32(reader["ItemQuantity"]);
                                        oldWarehouseID = reader["WarehouseID"].ToString();
                                    }
                                }
                            }

                            // Reverse stock quantities in the old warehouse
                            foreach (var item in oldQuantities)
                            {
                                string reverseStockQuery = @"
                            UPDATE tblStockBalance
                            SET ItemQty = ItemQty - @Qty
                            WHERE ItemID = @ItemID AND WarehouseID = @WarehouseID";

                                using (SqlCommand cmd = new SqlCommand(reverseStockQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@ItemID", item.Key);
                                    cmd.Parameters.AddWithValue("@WarehouseID", oldWarehouseID);
                                    cmd.Parameters.AddWithValue("@Qty", item.Value);

                                    cmd.ExecuteNonQuery();
                                }
                            }

                            // Update GRN details
                            string[] itemIDs = txtItemIDs.Lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
                            string[] itemQuantities = txtItemQtys.Lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

                            string updateGRNQuery = @"
                        UPDATE tblGRN
                        SET PurchaseID = @PurchaseID,
                            PurchaseType = @PurchaseType,
                            SupplierID = @SupplierID,
                            ItemID = @ItemIDs,
                            ItemQuantity = @ItemQuantities,
                            WarehouseID = @WarehouseID,
                            GRN_Date = GETDATE(),
                            GRN_Type = @GRN_Type
                        WHERE GRN_ID = @GRN_ID";

                            using (SqlCommand updateCmd = new SqlCommand(updateGRNQuery, conn, transaction))
                            {
                                updateCmd.Parameters.AddWithValue("@GRN_ID", txtGRN_ID.Text);
                                updateCmd.Parameters.AddWithValue("@PurchaseID", newPurchaseID);
                                updateCmd.Parameters.AddWithValue("@PurchaseType", newPurchaseType);
                                updateCmd.Parameters.AddWithValue("@SupplierID", txtSupplierID.Text);
                                updateCmd.Parameters.AddWithValue("@ItemIDs", string.Join(",", itemIDs));
                                updateCmd.Parameters.AddWithValue("@ItemQuantities", string.Join(",", itemQuantities));
                                updateCmd.Parameters.AddWithValue("@WarehouseID", newWarehouseID);
                                updateCmd.Parameters.AddWithValue("@GRN_Type", cmbGRN_Type.SelectedItem?.ToString() ?? string.Empty);

                                updateCmd.ExecuteNonQuery();
                            }

                            // Add new stock quantities to the new warehouse
                            for (int i = 0; i < itemIDs.Length; i++)
                            {
                                string adjustStockQuery = @"
MERGE INTO tblStockBalance AS Target
USING (SELECT @ItemID AS ItemID, @WarehouseID AS WarehouseID, @Qty AS Qty, @ItemName AS ItemName) AS Source
ON Target.ItemID = Source.ItemID AND Target.WarehouseID = Source.WarehouseID
WHEN MATCHED THEN
    UPDATE SET Target.ItemQty = Target.ItemQty + Source.Qty
WHEN NOT MATCHED THEN
    INSERT (ItemID, ItemName, WarehouseID, WarehouseName, ItemQty)
    VALUES (Source.ItemID, Source.ItemName, Source.WarehouseID, 
            (SELECT Store_Name FROM Warehouse WHERE StoreID = Source.WarehouseID), Source.Qty);";

                                using (SqlCommand cmd = new SqlCommand(adjustStockQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@ItemID", itemIDs[i]);
                                    cmd.Parameters.AddWithValue("@ItemName", txtItemNames.Lines[i].Trim()); // Ensure no null ItemName
                                    cmd.Parameters.AddWithValue("@WarehouseID", newWarehouseID);
                                    cmd.Parameters.AddWithValue("@Qty", int.Parse(itemQuantities[i]));

                                    cmd.ExecuteNonQuery();
                                }

                            }

                            // Commit transaction
                            transaction.Commit();

                            // Clear fields
                            ClearFields();
                            LoadNextGRNID();

                            MessageBox.Show("GRN updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"An error occurred during the update: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }






        private void btnClear_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("btnClear_Click method called 🐞");
            // Call the ClearFields method to reset all fields and radio buttons
            ClearFields();

            // Reload the next GRN ID to ensure it's ready for new entries
            LoadNextGRNID();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine("txtSearch_KeyDown method called 🐞");
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    MessageBox.Show("Please enter a GRN ID to search.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "SELECT * FROM tblGRN WHERE GRN_ID = @GRN_ID";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@GRN_ID", txtSearch.Text.Trim());
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            // Populate fields from tblGRN
                            txtGRN_ID.Text = reader["GRN_ID"].ToString();
                            string purchaseId = reader["PurchaseID"].ToString();
                            txtSupplierID.Text = reader["SupplierID"].ToString();

                            // Convert comma-separated IDs/Qtys to newline-separated
                            txtItemIDs.Text = reader["ItemID"].ToString().Replace(",", Environment.NewLine);
                            txtItemQtys.Text = reader["ItemQuantity"].ToString().Replace(",", Environment.NewLine);

                            cmbWarehouseID.Text = reader["WarehouseID"].ToString();
                            cmbGRN_Type.Text = reader["GRN_Type"].ToString();

                            // Temporarily mark that we're changing the radio in code
                            isProgrammaticRadioChange = true;
                            try
                            {
                                string purchaseType = reader["PurchaseType"].ToString();
                                if (purchaseType == "O")
                                {
                                    rbPurchaseOrder.Checked = true;

                                    // Set Item Quantity as read-only and change background
                                    txtItemQtys.ReadOnly = true;
                                    txtItemQtys.BackColor = Color.BurlyWood;
                                }
                                else if (purchaseType == "C")
                                {
                                    rbPurchaseContract.Checked = true;

                                    // Set Item Quantity as editable and change background to default
                                    txtItemQtys.ReadOnly = false;
                                    txtItemQtys.BackColor = SystemColors.Control;
                                }
                            }
                            finally
                            {
                                // Revert this after setting the radio button
                                isProgrammaticRadioChange = false;
                            }

                            // Also set the PurchaseID combo text now
                            cmbPurchaseID.Text = purchaseId;

                            // Finally, call the relevant loading method to get SupplierName & ItemNames
                            if (rbPurchaseOrder.Checked)
                            {
                                LoadPurchaseOrderDetails(purchaseId);
                            }
                            else if (rbPurchaseContract.Checked)
                            {
                                LoadPurchaseContractDetails(purchaseId);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No record found for the entered GRN ID.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

    }
}
