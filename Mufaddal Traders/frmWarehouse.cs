using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmWarehouse : Form
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

        public frmWarehouse()
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
            // keep this event as it is..
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

        private void btnBack_Click(object sender, EventArgs e)
        {
            frmStorekeeperMenu menuForm = new frmStorekeeperMenu();
            menuForm.Show();
            this.Hide();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedStoreID = Convert.ToInt32(gridView.SelectedRows[0].Cells["StoreID"].Value);

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if the Warehouse is related to tblStockBalance
                    string checkStockBalanceQuery = @"
            SELECT ItemID, ItemName 
            FROM tblStockBalance 
            WHERE WarehouseID = @StoreID";
                    SqlCommand checkStockBalanceCmd = new SqlCommand(checkStockBalanceQuery, conn);
                    checkStockBalanceCmd.Parameters.AddWithValue("@StoreID", selectedStoreID);

                    SqlDataAdapter stockBalanceAdapter = new SqlDataAdapter(checkStockBalanceCmd);
                    DataTable stockBalanceTable = new DataTable();
                    stockBalanceAdapter.Fill(stockBalanceTable);

                    // Check if the Warehouse is related to tblGRN
                    string checkGRNQuery = @"
            SELECT GRN_ID, PurchaseID 
            FROM tblGRN 
            WHERE WarehouseID = @StoreID";
                    SqlCommand checkGRNCmd = new SqlCommand(checkGRNQuery, conn);
                    checkGRNCmd.Parameters.AddWithValue("@StoreID", selectedStoreID);

                    SqlDataAdapter grnAdapter = new SqlDataAdapter(checkGRNCmd);
                    DataTable grnTable = new DataTable();
                    grnAdapter.Fill(grnTable);

                    // If there are related records in either table, show a detailed message
                    if (stockBalanceTable.Rows.Count > 0 || grnTable.Rows.Count > 0)
                    {
                        StringBuilder messageBuilder = new StringBuilder();
                        messageBuilder.AppendLine("This warehouse cannot be deleted because it is related to the following records:");

                        if (stockBalanceTable.Rows.Count > 0)
                        {
                            messageBuilder.AppendLine("\nStock Balance Records:");
                            foreach (DataRow row in stockBalanceTable.Rows)
                            {
                                messageBuilder.AppendLine($"- Item ID: {row["ItemID"]}, Item Name: {row["ItemName"]}");
                            }
                        }

                        if (grnTable.Rows.Count > 0)
                        {
                            messageBuilder.AppendLine("\nGRN Records:");
                            foreach (DataRow row in grnTable.Rows)
                            {
                                messageBuilder.AppendLine($"- GRN ID: {row["GRN_ID"]}, Purchase ID: {row["PurchaseID"]}");
                            }
                        }

                        MessageBox.Show(messageBuilder.ToString(), "Deletion Restricted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // If no related records exist, ask for confirmation before deletion
                    DialogResult result = MessageBox.Show(
                        $"Are you sure you want to delete Warehouse ID = {selectedStoreID}?",
                        "Delete Confirmation",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        string deleteQuery = "DELETE FROM Warehouse WHERE StoreID = @StoreID";
                        SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn);
                        deleteCmd.Parameters.AddWithValue("@StoreID", selectedStoreID);

                        int rowsAffected = deleteCmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Warehouse deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataIntoGridView(); // Refresh the grid
                        }
                        else
                        {
                            MessageBox.Show("Deletion failed. No rows affected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting the warehouse: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void btnManage_Click(object sender, EventArgs e)
        {
            frmAddUpdateWarehouse addUpdateWarehouse = new frmAddUpdateWarehouse();
            addUpdateWarehouse.Show();
        }

        private void frmStorekeeperWarehouse_Load(object sender, EventArgs e)
        {
            LoadDataIntoGridView(); // Call the method to load data into the grid
        }


        /// <summary>
        /// Loads data from the Warehouse table into the gridView and sets proper column headers.
        /// </summary>
        private void LoadDataIntoGridView()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT StoreID, Store_Name, Store_Location FROM Warehouse";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Set data source of the gridView to display the warehouse data
                    gridView.DataSource = dataTable;

                    // Set row height
                    gridView.RowTemplate.Height = 40;
                    foreach (DataGridViewRow row in gridView.Rows)
                    {
                        row.Height = 40;
                    }

                    // Basic font and styling
                    gridView.DefaultCellStyle.Font = new Font("Arial", 12);

                    // Header styling
                    gridView.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
                    gridView.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSeaGreen;
                    gridView.EnableHeadersVisualStyles = false;

                    // Adjust column widths proportionally
                    gridView.Columns["StoreID"].Width = (int)(gridView.Width * 0.30);
                    gridView.Columns["Store_Name"].Width = (int)(gridView.Width * 0.35);
                    gridView.Columns["Store_Location"].Width = (int)(gridView.Width * 0.35);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading warehouse details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadDataIntoGridView(); // Call the method to load data into the grid
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchText = txtSearch.Text.Trim();
                string query = @"
            SELECT StoreID, Store_Name, Store_Location
            FROM Warehouse
            WHERE CAST(StoreID AS NVARCHAR) LIKE @Search
               OR Store_Name LIKE @Search
               OR Store_Location LIKE @Search";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Search", "%" + searchText + "%");

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable searchResult = new DataTable();
                        adapter.Fill(searchResult);

                        // Bind search results to gridView
                        gridView.DataSource = searchResult;

                        // Reapply the same formatting as in LoadDataIntoGridView()
                        gridView.RowTemplate.Height = 40;
                        foreach (DataGridViewRow row in gridView.Rows)
                        {
                            row.Height = 40;
                        }

                        gridView.DefaultCellStyle.Font = new Font("Arial", 12);
                        gridView.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
                        gridView.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSeaGreen;
                        gridView.EnableHeadersVisualStyles = false;

                        gridView.Columns["StoreID"].Width = (int)(gridView.Width * 0.30);
                        gridView.Columns["Store_Name"].Width = (int)(gridView.Width * 0.35);
                        gridView.Columns["Store_Location"].Width = (int)(gridView.Width * 0.35);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during search: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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
    }
}
