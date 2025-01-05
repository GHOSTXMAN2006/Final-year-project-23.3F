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
    public partial class frmSRN : Form
    {

        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private string connectionString = DatabaseConfig.ConnectionString;


        public frmSRN()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
            // Check the userType to open the corresponding menu form
            switch (frmLogin.userType)  // Accessing userType from frmLogin
            {
                case "Storekeeper":
                    new frmStorekeeperMenu().Show();
                    break;
                case "Shipping Manager":
                    new frmShippingManagerMenu().Show();
                    break;
                case "Accountant":
                    new frmAccountantsMenu().Show();
                    break;
                case "Marketing and Sales Department":
                    new frmMSD_Menu().Show();
                    break;
                default:
                    MessageBox.Show("Invalid User Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

            // Hide the current dashboard form (optional, to switch to the menu form)
            this.Hide();
        }

        private void btnAccount_Click(object sender, EventArgs e)
        {
            frmAccount accountForm = new frmAccount();

            accountForm.Show();

            this.Hide();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            frmDashboard dashboardForm = new frmDashboard();

            dashboardForm.Show();

            this.Hide();
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {

        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            // Check the userType to open the corresponding menu form
            switch (frmLogin.userType)  // Accessing userType from frmLogin
            {
                case "Storekeeper":
                    new frmStorekeeperMenu().Show();
                    break;
                case "Shipping Manager":
                    new frmShippingManagerMenu().Show();
                    break;
                case "Accountant":
                    new frmAccountantsMenu().Show();
                    break;
                case "Marketing and Sales Department":
                    new frmMSD_Menu().Show();
                    break;
                default:
                    MessageBox.Show("Invalid User Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

            // Hide the current dashboard form (optional, to switch to the menu form)
            this.Hide();
        }
        private void btnHome_Click(object sender, EventArgs e)
        {
            frmHome homeForm = new frmHome();

            homeForm.Show();

            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear the session or global variables
            frmLogin.userType = string.Empty;
            frmLogin.userName = string.Empty;
            frmLogin.userPassword = string.Empty;
            frmLogin.userEmail = string.Empty;
            frmLogin.userTelephone = string.Empty;
            frmLogin.userAddress = string.Empty;
            frmLogin.userDescription = string.Empty;
            frmLogin.profilePicture = null; // Clear profile picture if any

            // Optionally, you can also clear other session variables if needed

            // Close the current form (Dashboard)
            this.Close();

            // Show the login form again
            frmLogin loginForm = new frmLogin();
            loginForm.Show();
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            frmAddUpdateSRN addUpdateSRN = new frmAddUpdateSRN();

            addUpdateSRN.Show();
        }

        // Method to load SRN data
        private void LoadSRNData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                SELECT 
                    SRN_ID,
                    SupplierID,
                    (SELECT Name FROM tblManageSuppliers WHERE SupplierID = tblSRN.SupplierID) AS SupplierName,
                    DiscardedGoodID,
                    (SELECT ItemID FROM tblDiscardedGoods WHERE DiscardedGoodsID = tblSRN.DiscardedGoodID) AS ItemID,
                    (SELECT WarehouseID FROM tblDiscardedGoods WHERE DiscardedGoodsID = tblSRN.DiscardedGoodID) AS WarehouseID,
                    ItemQty,
                    Status
                FROM tblSRN";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable srnTable = new DataTable();
                    adapter.Fill(srnTable);

                    // Bind data to DataGridView
                    dgvDisplay.DataSource = srnTable;

                    // Apply formatting after loading data
                    ApplyDataGridViewFormatting(dgvDisplay);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading SRN data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyDataGridViewFormatting(DataGridView dgvDisplay)
        {
            // Set column headers
            dgvDisplay.Columns["SRN_ID"].HeaderText = "SRN ID";
            dgvDisplay.Columns["SupplierID"].HeaderText = "Supplier ID";
            dgvDisplay.Columns["SupplierName"].HeaderText = "Supplier Name";
            dgvDisplay.Columns["DiscardedGoodID"].HeaderText = "Discarded Goods ID";
            dgvDisplay.Columns["ItemID"].HeaderText = "Item ID";
            dgvDisplay.Columns["WarehouseID"].HeaderText = "Warehouse ID";  // Added WarehouseID column
            dgvDisplay.Columns["ItemQty"].HeaderText = "Quantity";
            dgvDisplay.Columns["Status"].HeaderText = "Status";

            // Adjust column widths proportionally
            dgvDisplay.Columns["SRN_ID"].Width = (int)(1281 * 0.10);
            dgvDisplay.Columns["SupplierID"].Width = (int)(1281 * 0.10);
            dgvDisplay.Columns["SupplierName"].Width = (int)(1281 * 0.20);
            dgvDisplay.Columns["DiscardedGoodID"].Width = (int)(1281 * 0.12);
            dgvDisplay.Columns["ItemID"].Width = (int)(1281 * 0.10);
            dgvDisplay.Columns["WarehouseID"].Width = (int)(1281 * 0.12);  // Set width for WarehouseID column
            dgvDisplay.Columns["ItemQty"].Width = (int)(1281 * 0.10);
            dgvDisplay.Columns["Status"].Width = (int)(1281 * 0.11);

            dgvDisplay.RowTemplate.Height = 40;
            dgvDisplay.DefaultCellStyle.Font = new Font("Arial", 12);
            dgvDisplay.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
            dgvDisplay.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSeaGreen;
            dgvDisplay.EnableHeadersVisualStyles = false;

            // Apply status-specific formatting
            foreach (DataGridViewRow row in dgvDisplay.Rows)
            {
                if (row.Cells["Status"].Value != null)
                {
                    DataGridViewCell statusCell = row.Cells["Status"];
                    statusCell.Style.Font = new Font("Arial", 14, FontStyle.Regular);

                    if (statusCell.Value.ToString() == "N")
                    {
                        statusCell.Style.ForeColor = Color.Red;
                    }
                    else if (statusCell.Value.ToString() == "Y")
                    {
                        statusCell.Style.ForeColor = Color.Green;
                    }

                    row.Height = 40;
                }
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchText = txtSearch.Text.Trim();

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    // If the search field is empty, load all data
                    LoadSRNData();
                }
                else
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            string searchQuery = @"
                    SELECT 
                        SRN_ID,
                        SupplierID,
                        (SELECT Name FROM tblManageSuppliers WHERE SupplierID = tblSRN.SupplierID) AS SupplierName,
                        DiscardedGoodID,
                        (SELECT ItemID FROM tblDiscardedGoods WHERE DiscardedGoodsID = tblSRN.DiscardedGoodID) AS ItemID,
                        (SELECT WarehouseID FROM tblDiscardedGoods WHERE DiscardedGoodsID = tblSRN.DiscardedGoodID) AS WarehouseID,
                        ItemQty,
                        Status
                    FROM tblSRN
                    WHERE CAST(SRN_ID AS NVARCHAR) LIKE @SearchText";

                            SqlDataAdapter adapter = new SqlDataAdapter(searchQuery, conn);
                            adapter.SelectCommand.Parameters.AddWithValue("@SearchText", $"%{searchText}%");

                            DataTable searchResults = new DataTable();
                            adapter.Fill(searchResults);

                            if (searchResults.Rows.Count > 0)
                            {
                                dgvDisplay.DataSource = searchResults;
                                ApplyDataGridViewFormatting(dgvDisplay);
                            }
                            else
                            {
                                MessageBox.Show("No matching SRN records found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadSRNData();  // Reload full data if no match found
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while searching for SRN: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void frmSRN_Load(object sender, EventArgs e)
        {
            // Check the userType and show/hide buttons accordingly
            if (frmLogin.userType != "Storekeeper")
            {
                btnManage.Visible = false;
                btnDelete.Visible = false;
            }
            else
            {
                btnManage.Visible = true;
                btnDelete.Visible = true;
            }

            LoadSRNData();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadSRNData();
        }

        private void btnSRN_ID_Status_Click(object sender, EventArgs e)
        {
            string srnIDText = txtSRN_ID_Status.Text.Trim();

            if (string.IsNullOrWhiteSpace(srnIDText) || !int.TryParse(srnIDText, out int srnID))
            {
                MessageBox.Show("Please enter a valid SRN ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Check if SRN exists and get details
                    string checkSRNQuery = @"
                SELECT SRN_ID, DiscardedGoodID, ItemID, ItemQty, Status, 
                       (SELECT WarehouseID FROM tblDiscardedGoods WHERE DiscardedGoodsID = tblSRN.DiscardedGoodID) AS WarehouseID
                FROM tblSRN 
                WHERE SRN_ID = @SRN_ID";

                    SqlCommand checkSRNCmd = new SqlCommand(checkSRNQuery, conn);
                    checkSRNCmd.Parameters.AddWithValue("@SRN_ID", srnID);

                    SqlDataReader reader = checkSRNCmd.ExecuteReader();
                    if (!reader.Read())
                    {
                        MessageBox.Show("SRN record not found. Please enter a valid SRN ID.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        reader.Close();
                        return;
                    }

                    // Extract SRN details
                    string currentStatus = reader["Status"].ToString();
                    int itemID = Convert.ToInt32(reader["ItemID"]);
                    int itemQty = Convert.ToInt32(reader["ItemQty"]);
                    int warehouseID = Convert.ToInt32(reader["WarehouseID"]);
                    reader.Close();

                    // Validate status
                    if (currentStatus == "Y")
                    {
                        MessageBox.Show("The status of this SRN is already 'Y'.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Get ItemName and WarehouseName
                            string itemNameQuery = "SELECT Item_Name FROM Items WHERE ItemID = @ItemID";
                            string warehouseNameQuery = "SELECT Store_Name FROM Warehouse WHERE StoreID = @WarehouseID";

                            SqlCommand getItemNameCmd = new SqlCommand(itemNameQuery, conn, transaction);
                            getItemNameCmd.Parameters.AddWithValue("@ItemID", itemID);
                            string itemName = getItemNameCmd.ExecuteScalar()?.ToString() ?? "Unknown Item";

                            SqlCommand getWarehouseNameCmd = new SqlCommand(warehouseNameQuery, conn, transaction);
                            getWarehouseNameCmd.Parameters.AddWithValue("@WarehouseID", warehouseID);
                            string warehouseName = getWarehouseNameCmd.ExecuteScalar()?.ToString() ?? "Unknown Warehouse";

                            // Update SRN status to 'Y'
                            string updateSRNStatusQuery = "UPDATE tblSRN SET Status = 'Y' WHERE SRN_ID = @SRN_ID";
                            SqlCommand updateSRNCmd = new SqlCommand(updateSRNStatusQuery, conn, transaction);
                            updateSRNCmd.Parameters.AddWithValue("@SRN_ID", srnID);
                            updateSRNCmd.ExecuteNonQuery();

                            // Check if the item already exists in the stock balance
                            string checkStockBalanceQuery = @"
                        SELECT ItemQty 
                        FROM tblStockBalance 
                        WHERE ItemID = @ItemID AND WarehouseID = @WarehouseID";

                            SqlCommand checkStockBalanceCmd = new SqlCommand(checkStockBalanceQuery, conn, transaction);
                            checkStockBalanceCmd.Parameters.AddWithValue("@ItemID", itemID);
                            checkStockBalanceCmd.Parameters.AddWithValue("@WarehouseID", warehouseID);

                            object stockBalanceResult = checkStockBalanceCmd.ExecuteScalar();

                            if (stockBalanceResult != null)
                            {
                                // Item exists in stock balance, update the quantity
                                int currentQty = Convert.ToInt32(stockBalanceResult);
                                int newQty = currentQty + itemQty;

                                string updateStockBalanceQuery = @"
                            UPDATE tblStockBalance 
                            SET ItemQty = @NewQty 
                            WHERE ItemID = @ItemID AND WarehouseID = @WarehouseID";

                                SqlCommand updateStockCmd = new SqlCommand(updateStockBalanceQuery, conn, transaction);
                                updateStockCmd.Parameters.AddWithValue("@NewQty", newQty);
                                updateStockCmd.Parameters.AddWithValue("@ItemID", itemID);
                                updateStockCmd.Parameters.AddWithValue("@WarehouseID", warehouseID);
                                updateStockCmd.ExecuteNonQuery();
                            }
                            else
                            {
                                // Item does not exist in stock balance, insert new record
                                string insertStockBalanceQuery = @"
                            INSERT INTO tblStockBalance (ItemID, ItemName, WarehouseID, WarehouseName, ItemQty)
                            VALUES (@ItemID, @ItemName, @WarehouseID, @WarehouseName, @ItemQty)";

                                SqlCommand insertStockCmd = new SqlCommand(insertStockBalanceQuery, conn, transaction);
                                insertStockCmd.Parameters.AddWithValue("@ItemID", itemID);
                                insertStockCmd.Parameters.AddWithValue("@ItemName", itemName);
                                insertStockCmd.Parameters.AddWithValue("@WarehouseID", warehouseID);
                                insertStockCmd.Parameters.AddWithValue("@WarehouseName", warehouseName);
                                insertStockCmd.Parameters.AddWithValue("@ItemQty", itemQty);
                                insertStockCmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            MessageBox.Show("SRN status updated to 'Y' and stock balance updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            LoadSRNData();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"An error occurred during the update: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while connecting to the database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDisplay.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an SRN record to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int srnID = Convert.ToInt32(dgvDisplay.SelectedRows[0].Cells["SRN_ID"].Value);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string checkSRNQuery = @"
            SELECT SRN_ID, DiscardedGoodID, ItemID, ItemQty, Status, 
                   (SELECT WarehouseID FROM tblDiscardedGoods WHERE DiscardedGoodsID = tblSRN.DiscardedGoodID) AS WarehouseID
            FROM tblSRN 
            WHERE SRN_ID = @SRN_ID";

                    SqlCommand checkSRNCmd = new SqlCommand(checkSRNQuery, conn);
                    checkSRNCmd.Parameters.AddWithValue("@SRN_ID", srnID);

                    SqlDataReader reader = checkSRNCmd.ExecuteReader();
                    if (!reader.Read())
                    {
                        MessageBox.Show("SRN record not found. Please select a valid SRN.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        reader.Close();
                        return;
                    }

                    string status = reader["Status"].ToString();
                    int itemID = Convert.ToInt32(reader["ItemID"]);
                    int itemQty = Convert.ToInt32(reader["ItemQty"]);
                    int warehouseID = Convert.ToInt32(reader["WarehouseID"]);
                    int discardedGoodID = Convert.ToInt32(reader["DiscardedGoodID"]);
                    reader.Close();

                    // Declare currentStockQty for later use
                    int currentStockQty = 0;
                    int newStockQty = 0;

                    if (status == "Y")
                    {
                        // Check current stock balance
                        string checkStockQuery = "SELECT ItemQty FROM tblStockBalance WHERE ItemID = @ItemID AND WarehouseID = @WarehouseID";
                        SqlCommand checkStockCmd = new SqlCommand(checkStockQuery, conn);
                        checkStockCmd.Parameters.AddWithValue("@ItemID", itemID);
                        checkStockCmd.Parameters.AddWithValue("@WarehouseID", warehouseID);

                        object stockResult = checkStockCmd.ExecuteScalar();
                        if (stockResult == null)
                        {
                            MessageBox.Show("Stock balance record not found for this item and warehouse.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        currentStockQty = Convert.ToInt32(stockResult);  // Assign stock quantity
                        newStockQty = currentStockQty - itemQty;

                        if (newStockQty < 0)
                        {
                            MessageBox.Show($"This deletion will cause the stock to go negative.\n\n" +
                                            $"Current Stock Quantity: {currentStockQty}\n" +
                                            $"Quantity to Deduct: {itemQty}\n" +
                                            $"Resulting Quantity: {newStockQty}",
                                            "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Show confirmation for all cases (whether status is "N" or "Y")
                    var confirmationResult = MessageBox.Show($"Do you really want to delete this SRN?\n\n" +
                                                              (status == "Y" ?
                                                               $"Current Stock Quantity: {currentStockQty}\n" +
                                                               $"Quantity to Deduct: {itemQty}\n" +
                                                               $"Resulting Quantity: {newStockQty}" : "Status: 'N' (No stock deduction)"),
                                                              "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (confirmationResult == DialogResult.No)
                    {
                        return;
                    }

                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            if (status == "Y")
                            {
                                if (newStockQty == 0)
                                {
                                    // Delete stock balance record if new quantity is zero
                                    string deleteStockQuery = "DELETE FROM tblStockBalance WHERE ItemID = @ItemID AND WarehouseID = @WarehouseID";
                                    SqlCommand deleteStockCmd = new SqlCommand(deleteStockQuery, conn, transaction);
                                    deleteStockCmd.Parameters.AddWithValue("@ItemID", itemID);
                                    deleteStockCmd.Parameters.AddWithValue("@WarehouseID", warehouseID);
                                    deleteStockCmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    // Update stock balance quantity
                                    string updateStockQuery = "UPDATE tblStockBalance SET ItemQty = @NewQty WHERE ItemID = @ItemID AND WarehouseID = @WarehouseID";
                                    SqlCommand updateStockCmd = new SqlCommand(updateStockQuery, conn, transaction);
                                    updateStockCmd.Parameters.AddWithValue("@NewQty", newStockQty);
                                    updateStockCmd.Parameters.AddWithValue("@ItemID", itemID);
                                    updateStockCmd.Parameters.AddWithValue("@WarehouseID", warehouseID);
                                    updateStockCmd.ExecuteNonQuery();
                                }
                            }

                            // Update DiscardedGoodID status to "-"
                            string updateDiscardedGoodStatusQuery = "UPDATE tblDiscardedGoods SET Status = '-' WHERE DiscardedGoodsID = @DiscardedGoodID";
                            SqlCommand updateDiscardedGoodStatusCmd = new SqlCommand(updateDiscardedGoodStatusQuery, conn, transaction);
                            updateDiscardedGoodStatusCmd.Parameters.AddWithValue("@DiscardedGoodID", discardedGoodID);
                            updateDiscardedGoodStatusCmd.ExecuteNonQuery();

                            // Delete SRN record
                            string deleteSRNQuery = "DELETE FROM tblSRN WHERE SRN_ID = @SRN_ID";
                            SqlCommand deleteSRNCmd = new SqlCommand(deleteSRNQuery, conn, transaction);
                            deleteSRNCmd.Parameters.AddWithValue("@SRN_ID", srnID);
                            deleteSRNCmd.ExecuteNonQuery();

                            transaction.Commit();
                            MessageBox.Show("SRN deleted successfully and relevant records updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadSRNData();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"An error occurred during deletion: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while connecting to the database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
