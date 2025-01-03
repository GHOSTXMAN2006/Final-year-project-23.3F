using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmAddUpdateDamagedGoods : Form
    {
        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private string connectionString = DatabaseConfig.ConnectionString;

        public frmAddUpdateDamagedGoods()
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

        private void frmAddUpdateDamagedGoods_Load(object sender, EventArgs e)
        {
            LoadNextStockDamageID();
            LoadWarehouseIDs();
        }

        private void LoadNextStockDamageID()
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
                    WHERE Number NOT IN (SELECT StockDamageID FROM tblDamagedGoods)
                    ORDER BY Number";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    object result = cmd.ExecuteScalar();
                    txtStockDamageID.Text = result != null ? result.ToString() : "1";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Stock Damage ID: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("Error loading Items: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                        // Turn on identity insert
                        string identityInsertOn = "SET IDENTITY_INSERT tblDamagedGoods ON;";
                        SqlCommand identityOnCmd = new SqlCommand(identityInsertOn, conn, transaction);
                        identityOnCmd.ExecuteNonQuery();

                        // Insert the damaged goods record
                        string insertQuery = @"
                INSERT INTO tblDamagedGoods (StockDamageID, ItemID, ItemName, WarehouseID, WarehouseName, ItemQty)
                VALUES (@StockDamageID, @ItemID, @ItemName, @WarehouseID, @WarehouseName, @ItemQty)";

                        SqlCommand cmd = new SqlCommand(insertQuery, conn, transaction);
                        cmd.Parameters.AddWithValue("@StockDamageID", int.Parse(txtStockDamageID.Text));
                        cmd.Parameters.AddWithValue("@ItemID", cmbItemID.SelectedValue);
                        cmd.Parameters.AddWithValue("@ItemName", txtItemName.Text);
                        cmd.Parameters.AddWithValue("@WarehouseID", cmbWarehouseID.SelectedValue);
                        cmd.Parameters.AddWithValue("@WarehouseName", txtWarehouseName.Text);
                        cmd.Parameters.AddWithValue("@ItemQty", int.Parse(txtItemQty.Text));

                        cmd.ExecuteNonQuery();

                        // Turn off identity insert
                        string identityInsertOff = "SET IDENTITY_INSERT tblDamagedGoods OFF;";
                        SqlCommand identityOffCmd = new SqlCommand(identityInsertOff, conn, transaction);
                        identityOffCmd.ExecuteNonQuery();

                        transaction.Commit();

                        MessageBox.Show("Record saved successfully in tblDamagedGoods!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFields(); // Clear form after saving
                    }
                    catch (Exception ex)
                    {
                        transaction?.Rollback();
                        MessageBox.Show("Error saving damaged goods record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtStockDamageID.Text))
            {
                MessageBox.Show("Stock Damage ID cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            if (string.IsNullOrWhiteSpace(txtItemQty.Text) || !int.TryParse(txtItemQty.Text, out int itemQty) || itemQty <= 0)
            {
                MessageBox.Show("Please enter a valid Item Quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (int.TryParse(QtyDisplay.Text, out int availableQty) && itemQty > availableQty)
            {
                MessageBox.Show("The entered quantity exceeds the available stock balance.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ClearFields()
        {
            // Reset Stock Damage ID by reloading the next available ID
            LoadNextStockDamageID();

            // Clear text fields
            txtWarehouseName.Clear();
            txtItemName.Clear();
            txtItemQty.Clear();
            QtyDisplay.Text = "000000"; // Reset to default display

            // Reset combo boxes
            cmbWarehouseID.SelectedIndex = -1;
            cmbItemID.DataSource = null;

            // Reset focus to the first field
            cmbWarehouseID.Focus();
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

                        // Fetch the current record to compare with new data
                        string fetchQuery = @"
                SELECT ItemID, ItemName, WarehouseID, WarehouseName, ItemQty
                FROM tblDamagedGoods
                WHERE StockDamageID = @StockDamageID";

                        SqlCommand fetchCmd = new SqlCommand(fetchQuery, conn, transaction);
                        fetchCmd.Parameters.AddWithValue("@StockDamageID", int.Parse(txtStockDamageID.Text));

                        SqlDataReader reader = fetchCmd.ExecuteReader();

                        if (!reader.Read())
                        {
                            MessageBox.Show("No record found for the entered Stock Damage ID.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            reader.Close();
                            return;
                        }

                        // Original record values
                        int originalItemID = (int)reader["ItemID"];
                        string originalItemName = reader["ItemName"].ToString();
                        int originalWarehouseID = (int)reader["WarehouseID"];
                        string originalWarehouseName = reader["WarehouseName"].ToString();
                        int originalItemQty = (int)reader["ItemQty"];
                        reader.Close();

                        // Check if the new values are the same as the old values
                        if (originalItemID == (int)cmbItemID.SelectedValue &&
                            originalItemName == txtItemName.Text &&
                            originalWarehouseID == (int)cmbWarehouseID.SelectedValue &&
                            originalWarehouseName == txtWarehouseName.Text &&
                            originalItemQty == int.Parse(txtItemQty.Text))
                        {
                            MessageBox.Show("No changes detected. The data is already up-to-date.", "No Changes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        // Update the record if changes exist
                        string updateQuery = @"
                UPDATE tblDamagedGoods
                SET ItemID = @ItemID, 
                    ItemName = @ItemName, 
                    WarehouseID = @WarehouseID, 
                    WarehouseName = @WarehouseName, 
                    ItemQty = @ItemQty
                WHERE StockDamageID = @StockDamageID";

                        SqlCommand cmd = new SqlCommand(updateQuery, conn, transaction);
                        cmd.Parameters.AddWithValue("@StockDamageID", int.Parse(txtStockDamageID.Text));
                        cmd.Parameters.AddWithValue("@ItemID", cmbItemID.SelectedValue);
                        cmd.Parameters.AddWithValue("@ItemName", txtItemName.Text);
                        cmd.Parameters.AddWithValue("@WarehouseID", cmbWarehouseID.SelectedValue);
                        cmd.Parameters.AddWithValue("@WarehouseName", txtWarehouseName.Text);
                        cmd.Parameters.AddWithValue("@ItemQty", int.Parse(txtItemQty.Text));

                        cmd.ExecuteNonQuery();
                        transaction.Commit();

                        MessageBox.Show("Record updated successfully in tblDamagedGoods!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFields(); // Clear form after updating
                    }
                    catch (Exception ex)
                    {
                        transaction?.Rollback();
                        MessageBox.Show("Error updating damaged goods record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                // Reset Stock Damage ID by reloading the next available ID
                LoadNextStockDamageID();

                // Clear text fields
                txtWarehouseName.Clear();
                txtItemName.Clear();
                txtItemQty.Clear();
                txtSearch.Clear();
                QtyDisplay.Text = "000000"; // Reset to default display

                // Reset combo boxes
                cmbWarehouseID.SelectedIndex = -1;
                cmbItemID.DataSource = null; // Clear item dropdown

                // Reset focus to the first field
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
                SELECT StockDamageID, ItemID, ItemName, WarehouseID, WarehouseName, ItemQty
                FROM tblDamagedGoods
                WHERE StockDamageID = @StockDamageID";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@StockDamageID", int.Parse(txtSearch.Text));

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            // Load data into form fields
                            txtStockDamageID.Text = reader["StockDamageID"].ToString();
                            cmbWarehouseID.SelectedValue = reader["WarehouseID"];
                            txtWarehouseName.Text = reader["WarehouseName"].ToString();
                            cmbItemID.SelectedValue = reader["ItemID"];
                            txtItemName.Text = reader["ItemName"].ToString();
                            txtItemQty.Text = reader["ItemQty"].ToString();

                            // Load items for the selected warehouse to refresh the dropdown
                            LoadItemsForWarehouse((int)reader["WarehouseID"]);

                            // Update QtyDisplay for the selected item in the selected warehouse
                            UpdateQtyDisplay((int)reader["ItemID"], (int)reader["WarehouseID"]);
                        }
                        else
                        {
                            MessageBox.Show("No record found for the entered Stock Damage ID.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            ClearFields();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading Stock Damage record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
