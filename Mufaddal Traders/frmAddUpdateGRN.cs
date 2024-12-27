using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void picHeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddUpdateGRN_Load(object sender, EventArgs e)
        {
            LoadNextGRNID();
            LoadWarehouseIDs(); // Load warehouse information
        }


        private void LoadNextGRNID()
        {
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
            // only run if the user truly clicked, not if we changed the radio in code
            if (isProgrammaticRadioChange) return;

            if (rbPurchaseOrder.Checked)
            {
                // Clear supplier and item information
                ClearSupplierAndItemInformation();


                txtItemQtys.ReadOnly = true;
                txtItemQtys.BackColor = Color.BurlyWood; // Set the background color to Burlywood
                LoadPurchaseIDs("O"); // 'O' for Purchase Order
            }
        }

        private void rbPurchaseContract_CheckedChanged(object sender, EventArgs e)
        {
            // only run if the user truly clicked, not if we changed the radio in code
            if (isProgrammaticRadioChange) return;

            if (rbPurchaseContract.Checked)
            {
                // Clear supplier and item information
                ClearSupplierAndItemInformation();

                txtItemQtys.ReadOnly = false;
                txtItemQtys.BackColor = SystemColors.Control; // Default control background color
                LoadPurchaseIDs("C"); // 'C' for Purchase Contract
            }
        }

        private void ClearSupplierAndItemInformation()
        {
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
                        // We'll "temporarily" store what’s in txtItemQtys
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
                        // If you want to forcibly ensure the user must enter a quantity,
                        // you can either do so on the form or in your code’s validation.
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
            try
            {
                // Validate inputs
                if (!ValidateInputs())
                {
                    MessageBox.Show("Please ensure all fields are correctly filled.",
                                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Add validation for monthly Purchase Contract GRN
                if (!ValidatePurchaseContractMonthly())
                {
                    // If validation fails, return early
                    return;
                }

                if (rbPurchaseOrder.Checked)
                {
                    // Validate that cmbPurchaseID.SelectedItem (the order ID) exists in Purchase_Orders
                    if (!DoesPurchaseOrderExist(cmbPurchaseID.SelectedItem.ToString()))
                    {
                        MessageBox.Show("This Purchase Order ID does not exist. Please select a valid one.",
                                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else if (rbPurchaseContract.Checked)
                {
                    // Validate that the purchase ID exists in Purchase_Contract
                    if (!DoesPurchaseContractExist(cmbPurchaseID.SelectedItem.ToString()))
                    {
                        MessageBox.Show("This Purchase Contract ID does not exist. Please select a valid one.",
                                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Combine item information into comma-separated strings
                    string itemIDs = string.Join(",", txtItemIDs.Lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray());
                    string itemQuantities = string.Join(",", txtItemQtys.Lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray());

                    // Insert GRN record
                    string insertQuery = @"
                INSERT INTO tblGRN (GRN_ID, PurchaseID, PurchaseType, SupplierID, ItemID, ItemQuantity, WarehouseID, GRN_Date, GRN_Type)
                VALUES (@GRN_ID, @PurchaseID, @PurchaseType, @SupplierID, @ItemIDs, @ItemQuantities, @WarehouseID, GETDATE(), @GRN_Type)";

                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@GRN_ID", txtGRN_ID.Text);
                        insertCmd.Parameters.AddWithValue("@PurchaseID", cmbPurchaseID.SelectedItem.ToString());
                        insertCmd.Parameters.AddWithValue("@PurchaseType", rbPurchaseOrder.Checked ? "O" : "C");
                        insertCmd.Parameters.AddWithValue("@SupplierID", txtSupplierID.Text);
                        insertCmd.Parameters.AddWithValue("@ItemIDs", itemIDs);
                        insertCmd.Parameters.AddWithValue("@ItemQuantities", itemQuantities);
                        insertCmd.Parameters.AddWithValue("@WarehouseID", cmbWarehouseID.SelectedItem.ToString());
                        insertCmd.Parameters.AddWithValue("@GRN_Type", cmbGRN_Type.SelectedItem?.ToString() ?? string.Empty);

                        int rowsAffected = insertCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // **Update the Purchase Order status to 'Y'** if radio is Purchase Order
                            if (rbPurchaseOrder.Checked)
                            {
                                string poUpdateQuery = @"
                            UPDATE Purchase_Orders
                            SET Status = 'Y'
                            WHERE PurchaseOrderID = @POID";

                                using (SqlCommand poStatusCmd = new SqlCommand(poUpdateQuery, conn))
                                {
                                    poStatusCmd.Parameters.AddWithValue("@POID", cmbPurchaseID.SelectedItem.ToString());
                                    poStatusCmd.ExecuteNonQuery();
                                }
                            }

                            MessageBox.Show("GRN record saved successfully!",
                                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                            LoadNextGRNID();
                        }
                        else
                        {
                            MessageBox.Show("Failed to save GRN record.",
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


        private bool DoesPurchaseOrderExist(string purchaseOrderID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Purchase_Orders WHERE PurchaseOrderID = @POID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@POID", purchaseOrderID);
                    int count = (int)cmd.ExecuteScalar();
                    return (count > 0);
                }
            }
        }

        private bool DoesPurchaseContractExist(string contractID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Purchase_Contract WHERE PurchaseContractID = @ContractID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ContractID", contractID);
                    int count = (int)cmd.ExecuteScalar();
                    return (count > 0);
                }
            }
        }

        private bool ValidatePurchaseContractMonthly()
        {
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

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@PurchaseID", cmbPurchaseID.SelectedItem.ToString());
                    int count = (int)cmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("A GRN for this Purchase Contract has already been created this month.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
            }
            return true;
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtGRN_ID.Text))
            {
                MessageBox.Show("GRN ID cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (cmbPurchaseID.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Purchase ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            if (cmbWarehouseID.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Warehouse ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void ClearFields()
        {
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
            try
            {
                // 1) Validate the form fields:
                if (!ValidateInputs())
                {
                    MessageBox.Show("Please ensure all fields are correctly filled.",
                                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2) Depending on the new radio button, confirm the PurchaseID really exists:
                if (rbPurchaseOrder.Checked)
                {
                    if (!DoesPurchaseOrderExist(cmbPurchaseID.SelectedItem.ToString()))
                    {
                        MessageBox.Show("This Purchase Order ID does not exist. Please select a valid one.",
                                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else // rbPurchaseContract.Checked
                {
                    if (!DoesPurchaseContractExist(cmbPurchaseID.SelectedItem.ToString()))
                    {
                        MessageBox.Show("This Purchase Contract ID does not exist. Please select a valid one.",
                                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // 3) Perform the UPDATE inside a single SqlConnection:
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    //-------------------------------------------------------------
                    // STEP A: Fetch the old PurchaseType & old PurchaseID 
                    //         for the *existing* GRN record 
                    //-------------------------------------------------------------
                    string oldPurchaseType = "";
                    string oldPurchaseID = "";

                    string selectOld = @"
                SELECT PurchaseType, PurchaseID 
                FROM tblGRN 
                WHERE GRN_ID = @GRN_ID;
            ";
                    using (SqlCommand oldCmd = new SqlCommand(selectOld, conn))
                    {
                        oldCmd.Parameters.AddWithValue("@GRN_ID", txtGRN_ID.Text);
                        using (SqlDataReader rdr = oldCmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                oldPurchaseType = rdr["PurchaseType"].ToString();
                                oldPurchaseID = rdr["PurchaseID"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Could not find an existing GRN with that ID.",
                                                "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }

                    // Determine what the *new* type and *new* purchase ID are:
                    string newPurchaseType = rbPurchaseOrder.Checked ? "O" : "C";
                    string newPurchaseID = cmbPurchaseID.SelectedItem.ToString();

                    //-------------------------------------------------------------
                    // STEP B: If the user is switching from O->C, revert old PO status to 'N'
                    //         If the user is switching from C->O, set *new* PO status to 'Y'
                    //-------------------------------------------------------------
                    if (oldPurchaseType == "O" && newPurchaseType == "C")
                    {
                        // Revert the *old* PO's status to 'N':
                        string revertOldPO =
                            "UPDATE Purchase_Orders SET Status = 'N' WHERE PurchaseOrderID = @OldPOID";
                        using (SqlCommand revertCmd = new SqlCommand(revertOldPO, conn))
                        {
                            revertCmd.Parameters.AddWithValue("@OldPOID", oldPurchaseID);
                            revertCmd.ExecuteNonQuery();
                        }
                    }
                    else if (oldPurchaseType == "C" && newPurchaseType == "O")
                    {
                        // Mark the *new* PO's status as 'Y':
                        string setNewPO =
                            "UPDATE Purchase_Orders SET Status = 'Y' WHERE PurchaseOrderID = @NewPOID";
                        using (SqlCommand setPOCmd = new SqlCommand(setNewPO, conn))
                        {
                            setPOCmd.Parameters.AddWithValue("@NewPOID", newPurchaseID);
                            setPOCmd.ExecuteNonQuery();
                        }
                    }
                    // (Optionally, if you want to *always* set 'Y' if new type is 'O',
                    // you could put that logic here as well.)

                    //-------------------------------------------------------------
                    // STEP C: Proceed with the normal GRN update
                    //-------------------------------------------------------------
                    // Combine item information into comma-separated strings
                    string itemIDs = string.Join(",",
                        txtItemIDs.Lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray()
                    );
                    string itemQuantities = string.Join(",",
                        txtItemQtys.Lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray()
                    );

                    string updateQuery = @"
                UPDATE tblGRN
                SET PurchaseID   = @PurchaseID,
                    PurchaseType = @PurchaseType,
                    SupplierID   = @SupplierID,
                    ItemID       = @ItemIDs,
                    ItemQuantity = @ItemQuantities,
                    WarehouseID  = @WarehouseID,
                    GRN_Date     = GETDATE(),
                    GRN_Type     = @GRN_Type
                WHERE GRN_ID = @GRN_ID;
            ";

                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@GRN_ID", txtGRN_ID.Text);
                        updateCmd.Parameters.AddWithValue("@PurchaseID", newPurchaseID);
                        updateCmd.Parameters.AddWithValue("@PurchaseType", newPurchaseType);
                        updateCmd.Parameters.AddWithValue("@SupplierID", txtSupplierID.Text);
                        updateCmd.Parameters.AddWithValue("@ItemIDs", itemIDs);
                        updateCmd.Parameters.AddWithValue("@ItemQuantities", itemQuantities);
                        updateCmd.Parameters.AddWithValue("@WarehouseID", cmbWarehouseID.SelectedItem.ToString());
                        updateCmd.Parameters.AddWithValue("@GRN_Type", cmbGRN_Type.SelectedItem?.ToString() ?? "");

                        int rowsAffected = updateCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // If the new type is 'O', also set that PO to 'Y' if needed:
                            // e.g. in case old type was also O, but different PO ID
                            if (newPurchaseType == "O")
                            {
                                string setPOtoY = @"
                            UPDATE Purchase_Orders
                            SET Status = 'Y'
                            WHERE PurchaseOrderID = @POID
                        ";
                                using (SqlCommand poCmd = new SqlCommand(setPOtoY, conn))
                                {
                                    poCmd.Parameters.AddWithValue("@POID", newPurchaseID);
                                    poCmd.ExecuteNonQuery();
                                }
                            }

                            MessageBox.Show("GRN record updated successfully!",
                                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                            LoadNextGRNID();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update GRN record. Please ensure the GRN ID exists.",
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


        private void btnClear_Click(object sender, EventArgs e)
        {
            // Call the ClearFields method to reset all fields and radio buttons
            ClearFields();

            // Reload the next GRN ID to ensure it's ready for new entries
            LoadNextGRNID();
        }


        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
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
                                }
                                else if (purchaseType == "C")
                                {
                                    rbPurchaseContract.Checked = true;
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
                            // so that item names and supplier names are re-loaded from the source tables
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
