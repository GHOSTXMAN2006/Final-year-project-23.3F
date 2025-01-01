using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmAddUpdateGIN : Form
    {
        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private string connectionString = DatabaseConfig.ConnectionString;

        public frmAddUpdateGIN()
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

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddUpdateGIN_Load(object sender, EventArgs e)
        {
            LoadNextGINID();
            LoadWarehouseIDs();
            LoadSupplierIDs();

            // Add tooltips for QtyDisplay
            ToolTip toolTip = new ToolTip
            {
                AutoPopDelay = 5000,
                InitialDelay = 1000,
                ReshowDelay = 500,
                ShowAlways = true
            };
            toolTip.SetToolTip(QtyDisplay, "Displays the quantity of the selected item in the selected warehouse.");
        }

        private void LoadNextGINID()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = @"
                    SELECT TOP 1 Number
                    FROM (
                        SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Number
                        FROM master.dbo.spt_values
                    ) AS Numbers
                    WHERE Number NOT IN (SELECT GIN_ID FROM tblGIN)
                    ORDER BY Number";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    object result = cmd.ExecuteScalar();

                    txtGIN_ID.Text = result != null ? result.ToString() : "1";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading GIN ID: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    string query = @"
                    SELECT DISTINCT sb.WarehouseID, sb.WarehouseName
                    FROM tblStockBalance sb
                    WHERE sb.ItemQty > 0";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbWarehouseID.DisplayMember = "WarehouseID";
                    cmbWarehouseID.ValueMember = "WarehouseID";
                    cmbWarehouseID.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Warehouse IDs: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadSupplierIDs()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT SupplierID, Name FROM tblManageSuppliers";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbSupplierID.DisplayMember = "SupplierID";
                    cmbSupplierID.ValueMember = "SupplierID";
                    cmbSupplierID.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Supplier IDs: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cmbWarehouseID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbWarehouseID.SelectedValue != null)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        string query = "SELECT Store_Name FROM Warehouse WHERE StoreID = @StoreID";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@StoreID", cmbWarehouseID.SelectedValue);

                        object result = cmd.ExecuteScalar();
                        txtWarehouseName.Text = result?.ToString() ?? string.Empty;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading Warehouse Name: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void cmbSupplierID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSupplierID.SelectedValue != null)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        string query = "SELECT Name FROM tblManageSuppliers WHERE SupplierID = @SupplierID";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@SupplierID", cmbSupplierID.SelectedValue);

                        object result = cmd.ExecuteScalar();
                        txtSupplierName.Text = result?.ToString() ?? string.Empty;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading Supplier Name: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void cmbWarehouseID_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmbWarehouseID.SelectedValue != null)
            {
                LoadItemsForWarehouse((int)cmbWarehouseID.SelectedValue);
            }
        }

        private void LoadItemsForWarehouse(int warehouseID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = @"
                    SELECT ItemID, ItemName 
                    FROM tblStockBalance 
                    WHERE WarehouseID = @WarehouseID AND ItemQty > 0";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@WarehouseID", warehouseID);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbItemID.DisplayMember = "ItemID";
                    cmbItemID.ValueMember = "ItemID";
                    cmbItemID.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading items for Warehouse: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cmbItemID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbItemID.SelectedValue != null)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        string query = @"
                SELECT ItemName, ItemQty 
                FROM tblStockBalance 
                WHERE ItemID = @ItemID AND WarehouseID = @WarehouseID";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ItemID", cmbItemID.SelectedValue);
                        cmd.Parameters.AddWithValue("@WarehouseID", cmbWarehouseID.SelectedValue);

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            txtItemName.Text = reader["ItemName"].ToString();
                            QtyDisplay.Text = Convert.ToInt32(reader["ItemQty"]).ToString("D6");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading Item details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtGIN_ID.Text))
            {
                MessageBox.Show("GIN ID cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbWarehouseID.SelectedValue == null)
            {
                MessageBox.Show("Please select a Warehouse ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbItemID.SelectedValue == null)
            {
                MessageBox.Show("Please select an Item ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtGIN_Qty.Text) || !int.TryParse(txtGIN_Qty.Text, out int ginQty) || ginQty <= 0)
            {
                MessageBox.Show("Please enter a valid GIN Quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (int.TryParse(QtyDisplay.Text, out int availableQty) && ginQty > availableQty)
            {
                MessageBox.Show("GIN Quantity cannot exceed the available quantity in the selected warehouse.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ClearFields()
        {
            // Reset GIN ID by reloading the next available ID
            LoadNextGINID();

            // Clear text fields
            txtWarehouseName.Clear();
            txtSupplierName.Clear();
            txtItemName.Clear();
            QtyDisplay.Text = "000000"; // Reset to default display
            txtGIN_Qty.Clear();

            // Reset combo boxes
            cmbWarehouseID.SelectedIndex = -1;
            cmbSupplierID.SelectedIndex = -1;
            cmbItemID.SelectedIndex = -1;

            // Reset focus to the first field
            cmbWarehouseID.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlTransaction transaction = null;

                    try
                    {
                        conn.Open();
                        transaction = conn.BeginTransaction();

                        // Temporarily turn on identity insert
                        string identityInsertOn = "SET IDENTITY_INSERT tblGIN ON;";
                        SqlCommand identityOnCmd = new SqlCommand(identityInsertOn, conn, transaction);
                        identityOnCmd.ExecuteNonQuery();

                        // Insert the GIN record
                        string insertQuery = @"
                INSERT INTO tblGIN (GIN_ID, WarehouseID, Warehouse_Name, ItemID, Item_Name, SupplierID, Supplier_Name, GIN_Quantity, Status)
                VALUES (@GIN_ID, @WarehouseID, @Warehouse_Name, @ItemID, @Item_Name, @SupplierID, @Supplier_Name, @GIN_Quantity, @Status);";

                        SqlCommand insertCmd = new SqlCommand(insertQuery, conn, transaction);
                        insertCmd.Parameters.AddWithValue("@GIN_ID", int.Parse(txtGIN_ID.Text));
                        insertCmd.Parameters.AddWithValue("@WarehouseID", cmbWarehouseID.SelectedValue);
                        insertCmd.Parameters.AddWithValue("@Warehouse_Name", txtWarehouseName.Text);
                        insertCmd.Parameters.AddWithValue("@ItemID", cmbItemID.SelectedValue);
                        insertCmd.Parameters.AddWithValue("@Item_Name", txtItemName.Text);
                        insertCmd.Parameters.AddWithValue("@SupplierID", cmbSupplierID.SelectedValue);
                        insertCmd.Parameters.AddWithValue("@Supplier_Name", txtSupplierName.Text);
                        insertCmd.Parameters.AddWithValue("@GIN_Quantity", int.Parse(txtGIN_Qty.Text));
                        insertCmd.Parameters.AddWithValue("@Status", "N"); // Default status

                        insertCmd.ExecuteNonQuery();

                        // Reduce the quantity from tblStockBalance
                        string updateStockQuery = @"
                UPDATE tblStockBalance
                SET ItemQty = ItemQty - @GIN_Quantity
                WHERE ItemID = @ItemID AND WarehouseID = @WarehouseID;";

                        SqlCommand updateStockCmd = new SqlCommand(updateStockQuery, conn, transaction);
                        updateStockCmd.Parameters.AddWithValue("@GIN_Quantity", int.Parse(txtGIN_Qty.Text));
                        updateStockCmd.Parameters.AddWithValue("@ItemID", cmbItemID.SelectedValue);
                        updateStockCmd.Parameters.AddWithValue("@WarehouseID", cmbWarehouseID.SelectedValue);

                        updateStockCmd.ExecuteNonQuery();

                        // Turn off identity insert
                        string identityInsertOff = "SET IDENTITY_INSERT tblGIN OFF;";
                        SqlCommand identityOffCmd = new SqlCommand(identityInsertOff, conn, transaction);
                        identityOffCmd.ExecuteNonQuery();

                        transaction.Commit();

                        MessageBox.Show("GIN record saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFields();
                    }
                    catch (Exception ex)
                    {
                        transaction?.Rollback();
                        MessageBox.Show("Error saving GIN record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlTransaction transaction = null;

                    try
                    {
                        conn.Open();
                        transaction = conn.BeginTransaction();

                        // Fetch the original GIN record
                        string fetchQuery = @"
                SELECT WarehouseID, ItemID, GIN_Quantity
                FROM tblGIN
                WHERE GIN_ID = @GIN_ID";

                        SqlCommand fetchCmd = new SqlCommand(fetchQuery, conn, transaction);
                        fetchCmd.Parameters.AddWithValue("@GIN_ID", int.Parse(txtGIN_ID.Text));

                        SqlDataReader reader = fetchCmd.ExecuteReader();
                        if (!reader.Read())
                        {
                            MessageBox.Show("GIN record not found for update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Original values
                        int originalWarehouseID = (int)reader["WarehouseID"];
                        int originalItemID = (int)reader["ItemID"];
                        int originalQuantity = (int)reader["GIN_Quantity"];
                        reader.Close();

                        // Determine changes
                        int newWarehouseID = (int)cmbWarehouseID.SelectedValue;
                        int newItemID = (int)cmbItemID.SelectedValue;
                        int newQuantity = int.Parse(txtGIN_Qty.Text);

                        // Revert original changes to stock balance
                        string revertStockQuery = @"
                UPDATE tblStockBalance
                SET ItemQty = ItemQty + @OriginalQuantity
                WHERE ItemID = @OriginalItemID AND WarehouseID = @OriginalWarehouseID";

                        SqlCommand revertStockCmd = new SqlCommand(revertStockQuery, conn, transaction);
                        revertStockCmd.Parameters.AddWithValue("@OriginalQuantity", originalQuantity);
                        revertStockCmd.Parameters.AddWithValue("@OriginalItemID", originalItemID);
                        revertStockCmd.Parameters.AddWithValue("@OriginalWarehouseID", originalWarehouseID);
                        revertStockCmd.ExecuteNonQuery();

                        // Apply new changes to stock balance
                        string updateStockQuery = @"
                UPDATE tblStockBalance
                SET ItemQty = ItemQty - @NewQuantity
                WHERE ItemID = @NewItemID AND WarehouseID = @NewWarehouseID";

                        SqlCommand updateStockCmd = new SqlCommand(updateStockQuery, conn, transaction);
                        updateStockCmd.Parameters.AddWithValue("@NewQuantity", newQuantity);
                        updateStockCmd.Parameters.AddWithValue("@NewItemID", newItemID);
                        updateStockCmd.Parameters.AddWithValue("@NewWarehouseID", newWarehouseID);
                        updateStockCmd.ExecuteNonQuery();

                        // Update GIN record
                        string updateGINQuery = @"
                UPDATE tblGIN
                SET WarehouseID = @NewWarehouseID, Warehouse_Name = @WarehouseName,
                    ItemID = @NewItemID, Item_Name = @ItemName,
                    GIN_Quantity = @NewQuantity
                WHERE GIN_ID = @GIN_ID";

                        SqlCommand updateGINCmd = new SqlCommand(updateGINQuery, conn, transaction);
                        updateGINCmd.Parameters.AddWithValue("@GIN_ID", int.Parse(txtGIN_ID.Text));
                        updateGINCmd.Parameters.AddWithValue("@NewWarehouseID", newWarehouseID);
                        updateGINCmd.Parameters.AddWithValue("@WarehouseName", txtWarehouseName.Text);
                        updateGINCmd.Parameters.AddWithValue("@NewItemID", newItemID);
                        updateGINCmd.Parameters.AddWithValue("@ItemName", txtItemName.Text);
                        updateGINCmd.Parameters.AddWithValue("@NewQuantity", newQuantity);
                        updateGINCmd.ExecuteNonQuery();

                        transaction.Commit();
                        MessageBox.Show("GIN record updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFields();
                    }
                    catch (Exception ex)
                    {
                        transaction?.Rollback();
                        MessageBox.Show("Error updating GIN record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                // Reset GIN ID by reloading the next available ID
                LoadNextGINID();

                // Clear text fields
                txtWarehouseName.Clear();
                txtSupplierName.Clear();
                txtItemName.Clear();
                QtyDisplay.Text = "000000"; // Reset to default display
                txtGIN_Qty.Clear();
                txtSearch.Clear();

                // Reset combo boxes
                cmbWarehouseID.SelectedIndex = -1;
                cmbSupplierID.SelectedIndex = -1;
                cmbItemID.DataSource = null;

                // Reload warehouse and supplier IDs
                LoadWarehouseIDs();
                LoadSupplierIDs();

                // Reset focus to the first input field
                cmbWarehouseID.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while clearing the form: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        string query = @"
                SELECT GIN_ID, WarehouseID, Warehouse_Name, ItemID, Item_Name, SupplierID, Supplier_Name, GIN_Quantity, Status
                FROM tblGIN
                WHERE GIN_ID = @GIN_ID";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@GIN_ID", int.Parse(txtSearch.Text));

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            // Load data into form fields
                            txtGIN_ID.Text = reader["GIN_ID"].ToString();
                            cmbWarehouseID.SelectedValue = reader["WarehouseID"];
                            txtWarehouseName.Text = reader["Warehouse_Name"].ToString();
                            cmbSupplierID.SelectedValue = reader["SupplierID"];
                            txtSupplierName.Text = reader["Supplier_Name"].ToString();
                            txtGIN_Qty.Text = reader["GIN_Quantity"].ToString();

                            // Load items for the selected warehouse
                            LoadItemsForWarehouse((int)reader["WarehouseID"]);

                            // Select the appropriate item
                            cmbItemID.SelectedValue = reader["ItemID"];
                            txtItemName.Text = reader["Item_Name"].ToString();

                            // Update QtyDisplay for the selected item and warehouse
                            UpdateQtyDisplay((int)reader["ItemID"], (int)reader["WarehouseID"]);
                        }
                        else
                        {
                            MessageBox.Show("No record found with the provided GIN ID.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            ClearFields();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void UpdateQtyDisplay(int itemID, int warehouseID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = @"
            SELECT ItemQty
            FROM tblStockBalance
            WHERE ItemID = @ItemID AND WarehouseID = @WarehouseID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ItemID", itemID);
                    cmd.Parameters.AddWithValue("@WarehouseID", warehouseID);

                    object result = cmd.ExecuteScalar();
                    QtyDisplay.Text = result != null ? Convert.ToInt32(result).ToString("D6") : "000000";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading quantity: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
