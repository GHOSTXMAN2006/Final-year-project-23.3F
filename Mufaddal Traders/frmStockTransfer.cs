using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics; // <-- For Debug.WriteLine

namespace Mufaddal_Traders
{
    public partial class frmStockTransfer : Form
    {

        private string connectionString = DatabaseConfig.ConnectionString;

        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);


        public frmStockTransfer()
        {
            InitializeComponent();
        }

        private void frmStorekeeperStockTransfer_Load(object sender, EventArgs e)
        {

            LoadDataIntoGridView(); // Call the method to load data into the grid

            frmLogin.userType = "Storekeeper";
            // Check the userType and show/hide buttons accordingly
            if (frmLogin.userType != "Storekeeper")
            {
                btnAdd.Visible = false;
                btnDelete.Visible = false;
            }
            else
            {
                btnAdd.Visible = true;
                btnDelete.Visible = true;
            }
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
            frmStorekeeperMenu storekeeperMenu = new frmStorekeeperMenu();

            storekeeperMenu.Show();

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
            frmStorekeeperMenu menuForm = new frmStorekeeperMenu();

            menuForm.Show();

            this.Hide();

        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            frmHome homeForm = new frmHome();

            homeForm.Show();

            this.Hide();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddStockTransfer addStockTransfer = new frmAddStockTransfer();

            addStockTransfer.Show();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure a row is selected
                Debug.WriteLine("Checking if any row is selected in dgvST...");
                if (dgvST.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a record to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Debug.WriteLine("No rows selected. Returning.");
                    return;
                }
                Debug.WriteLine("Row selected. Proceeding with deletion.");

                // Get the selected record's ST_ID (stock transfer ID) and other details
                int stId = Convert.ToInt32(dgvST.SelectedRows[0].Cells["ST_ID"].Value);
                Debug.WriteLine($"Selected ST_ID: {stId}");

                int itemId = Convert.ToInt32(dgvST.SelectedRows[0].Cells["ItemID"].Value);  // Ensure this column exists
                Debug.WriteLine($"Selected ItemID: {itemId}");

                int transferQty = Convert.ToInt32(dgvST.SelectedRows[0].Cells["ST_Qty"].Value);
                Debug.WriteLine($"Transfer Quantity: {transferQty}");

                string startingWarehouseName = dgvST.SelectedRows[0].Cells["StartingLocation"].Value.ToString();  // Ensure this column exists
                Debug.WriteLine($"Starting Warehouse: {startingWarehouseName}");

                string endingWarehouseName = dgvST.SelectedRows[0].Cells["EndingLocation"].Value.ToString();  // Ensure this column exists
                Debug.WriteLine($"Ending Warehouse: {endingWarehouseName}");

                // Confirm deletion action
                var result = MessageBox.Show("Are you sure you want to delete this stock transfer record?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    Debug.WriteLine("Deletion cancelled by user.");
                    return;
                }

                // Create a connection to the database
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    Debug.WriteLine("Opening database connection...");
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();
                    Debug.WriteLine("Transaction started.");

                    try
                    {
                        // Reverse the quantity update for both warehouses
                        string updateStartingWarehouseQuery = @"
                    UPDATE tblStockBalance
                    SET ItemQty = ItemQty + @Qty
                    WHERE ItemID = @ItemID AND WarehouseName = @StartingWarehouseName";

                        string updateEndingWarehouseQuery = @"
                    UPDATE tblStockBalance
                    SET ItemQty = ItemQty - @Qty
                    WHERE ItemID = @ItemID AND WarehouseName = @EndingWarehouseName";

                        Debug.WriteLine("Executing query to update starting warehouse.");
                        Debug.WriteLine(updateStartingWarehouseQuery);
                        Debug.WriteLine("Executing query to update ending warehouse.");
                        Debug.WriteLine(updateEndingWarehouseQuery);

                        // Update starting warehouse (restore quantity)
                        SqlCommand updateStartingCmd = new SqlCommand(updateStartingWarehouseQuery, conn, transaction);
                        updateStartingCmd.Parameters.AddWithValue("@Qty", transferQty);
                        updateStartingCmd.Parameters.AddWithValue("@ItemID", itemId);
                        updateStartingCmd.Parameters.AddWithValue("@StartingWarehouseName", startingWarehouseName);
                        updateStartingCmd.ExecuteNonQuery();
                        Debug.WriteLine("Starting warehouse quantity updated.");

                        // Update ending warehouse (remove transferred quantity)
                        SqlCommand updateEndingCmd = new SqlCommand(updateEndingWarehouseQuery, conn, transaction);
                        updateEndingCmd.Parameters.AddWithValue("@Qty", transferQty);
                        updateEndingCmd.Parameters.AddWithValue("@ItemID", itemId);
                        updateEndingCmd.Parameters.AddWithValue("@EndingWarehouseName", endingWarehouseName);
                        updateEndingCmd.ExecuteNonQuery();
                        Debug.WriteLine("Ending warehouse quantity updated.");

                        // Delete the stock transfer record from Stock_Transfer table
                        string deleteTransferQuery = "DELETE FROM Stock_Transfer WHERE ST_ID = @ST_ID";
                        SqlCommand deleteCmd = new SqlCommand(deleteTransferQuery, conn, transaction);
                        deleteCmd.Parameters.AddWithValue("@ST_ID", stId);
                        deleteCmd.ExecuteNonQuery();
                        Debug.WriteLine("Stock transfer record deleted.");

                        // Commit transaction
                        transaction.Commit();
                        Debug.WriteLine("Transaction committed successfully.");
                        MessageBox.Show("Stock transfer record deleted and changes reverted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Refresh the data grid view after deletion
                        LoadDataIntoGridView();
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction in case of an error
                        transaction.Rollback();
                        Debug.WriteLine($"Error occurred during transaction: {ex.Message}");
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occurred in outer catch block: {ex.Message}");
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadDataIntoGridView(); // Call the method to load data into the grid
        }


        private void LoadDataIntoGridView()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    Debug.WriteLine("Opening database connection...");
                    conn.Open();

                    string query = @"
                SELECT 
                    ST.ST_ID, 
                    ST.ST_Date, 
                    ST.ST_Qty, 
                    ST.ItemID,  -- Explicitly select ItemID here
                    I.Item_Name, 
                    W1.Store_Name AS StartingLocation, 
                    W2.Store_Name AS EndingLocation
                FROM 
                    Stock_Transfer ST
                JOIN 
                    Items I ON ST.ItemID = I.ItemID
                JOIN 
                    Warehouse W1 ON ST.Starting_Warehouse = W1.StoreID
                JOIN 
                    Warehouse W2 ON ST.Ending_Warehouse = W2.StoreID";

                    Debug.WriteLine($"Executing query: {query}");

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    Debug.WriteLine("Data loaded into DataTable successfully.");

                    dgvST.DataSource = dt;
                    Debug.WriteLine("DataTable bound to DataGridView.");

                    // Log the columns in the DataGridView
                    Debug.WriteLine("Columns in dgvST:");
                    foreach (DataGridViewColumn column in dgvST.Columns)
                    {
                        Debug.WriteLine($"Column Name: {column.Name}");
                    }

                    dgvST.Columns["ST_ID"].HeaderText = "Stock Transfer ID";
                    dgvST.Columns["ST_Date"].HeaderText = "Transfer Date";
                    dgvST.Columns["ST_Qty"].HeaderText = "Transfer Quantity";
                    dgvST.Columns["Item_Name"].HeaderText = "Item Name";
                    dgvST.Columns["StartingLocation"].HeaderText = "Starting Location";
                    dgvST.Columns["EndingLocation"].HeaderText = "Ending Location";
                    Debug.WriteLine("Column headers set.");

                    dgvST.RowTemplate.Height = 40;
                    foreach (DataGridViewRow row in dgvST.Rows)
                    {
                        row.Height = 40;
                    }
                    Debug.WriteLine("Row heights set.");

                    dgvST.DefaultCellStyle.Font = new Font("Arial", 12);
                    dgvST.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
                    dgvST.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSeaGreen;
                    dgvST.EnableHeadersVisualStyles = false;

                    dgvST.Columns["ST_ID"].Width = (int)(dgvST.Width * 0.15);
                    dgvST.Columns["ST_Date"].Width = (int)(dgvST.Width * 0.15);
                    dgvST.Columns["ST_Qty"].Width = (int)(dgvST.Width * 0.10);
                    dgvST.Columns["Item_Name"].Width = (int)(dgvST.Width * 0.20);
                    dgvST.Columns["StartingLocation"].Width = (int)(dgvST.Width * 0.175);
                    dgvST.Columns["EndingLocation"].Width = (int)(dgvST.Width * 0.175);

                    Debug.WriteLine("Column widths adjusted.");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error loading stock transfer data: {ex.Message}");
                    MessageBox.Show("Error loading stock transfer data: " + ex.Message);
                }
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // Check if Enter key is pressed
            {
                string searchText = txtSearch.Text.Trim();

                // If search text is empty, reload all data
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    LoadDataIntoGridView();
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = @"
                SELECT 
                    ST.ST_ID, 
                    ST.ST_Date, 
                    ST.ST_Qty, 
                    ST.ItemID,  -- Explicitly select ItemID here
                    I.Item_Name, 
                    W1.Store_Name AS StartingLocation, 
                    W2.Store_Name AS EndingLocation
                FROM 
                    Stock_Transfer ST
                JOIN 
                    Items I ON ST.ItemID = I.ItemID
                JOIN 
                    Warehouse W1 ON ST.Starting_Warehouse = W1.StoreID
                JOIN 
                    Warehouse W2 ON ST.Ending_Warehouse = W2.StoreID
                WHERE 
                    CAST(ST.ST_ID AS NVARCHAR) LIKE @Search OR 
                    I.Item_Name LIKE @Search OR 
                    W1.Store_Name LIKE @Search OR 
                    W2.Store_Name LIKE @Search";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Search", "%" + searchText + "%");

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        dgvST.DataSource = dt;

                        // Apply the same formatting as in LoadDataIntoGridView
                        dgvST.Columns["ST_ID"].HeaderText = "Stock Transfer ID";
                        dgvST.Columns["ST_Date"].HeaderText = "Transfer Date";
                        dgvST.Columns["ST_Qty"].HeaderText = "Transfer Quantity";
                        dgvST.Columns["Item_Name"].HeaderText = "Item Name";
                        dgvST.Columns["StartingLocation"].HeaderText = "Starting Location";
                        dgvST.Columns["EndingLocation"].HeaderText = "Ending Location";

                        dgvST.RowTemplate.Height = 40;
                        foreach (DataGridViewRow row in dgvST.Rows)
                        {
                            row.Height = 40;
                        }

                        dgvST.DefaultCellStyle.Font = new Font("Arial", 12);
                        dgvST.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
                        dgvST.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSeaGreen;
                        dgvST.EnableHeadersVisualStyles = false;

                        dgvST.Columns["ST_ID"].Width = (int)(dgvST.Width * 0.15);
                        dgvST.Columns["ST_Date"].Width = (int)(dgvST.Width * 0.15);
                        dgvST.Columns["ST_Qty"].Width = (int)(dgvST.Width * 0.10);
                        dgvST.Columns["Item_Name"].Width = (int)(dgvST.Width * 0.20);
                        dgvST.Columns["StartingLocation"].Width = (int)(dgvST.Width * 0.175);
                        dgvST.Columns["EndingLocation"].Width = (int)(dgvST.Width * 0.175);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during search: {ex.Message}",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
