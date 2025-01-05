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
    public partial class frmStockBalance : Form
    {
        private string connectionString = DatabaseConfig.ConnectionString;

        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public frmStockBalance()
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
            // Add history-related functionality
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

        private void btnLogout_Click_1(object sender, EventArgs e)
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

        private void frmStockBalance_Load(object sender, EventArgs e)
        {
            LoadStockBalance();
        }

        private void LoadStockBalance()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
        SELECT 
            I.ItemID, 
            I.Item_Name AS ItemName, 
            I.Item_Description AS ItemDescription, 
            SB.WarehouseID, 
            SB.WarehouseName, 
            ISNULL(SB.ItemQty, 0) AS ItemQty
        FROM 
            Items I
        LEFT JOIN 
            tblStockBalance SB 
        ON 
            I.ItemID = SB.ItemID";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable stockTable = new DataTable();

                    adapter.Fill(stockTable);

                    // Bind data to DataGridView
                    dgvDisplay.DataSource = stockTable;

                    // Ensure row height is applied consistently
                    dgvDisplay.RowTemplate.Height = 40; // Set the desired row height
                    foreach (DataGridViewRow row in dgvDisplay.Rows)
                    {
                        row.Height = 40; // Apply the height explicitly to all rows
                    }

                    // Adjust DataGridView formatting
                    dgvDisplay.DefaultCellStyle.Font = new Font("Arial", 12); // Keep font size for rows

                    // Set column header font size and background color
                    dgvDisplay.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold); // Increase header font size
                    dgvDisplay.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSeaGreen; // Set header background color
                    dgvDisplay.EnableHeadersVisualStyles = false; // Apply custom header style

                    // Adjust column widths proportionally
                    dgvDisplay.Columns["ItemID"].Width = 115; // Slightly reduce width for ID
                    dgvDisplay.Columns["ItemName"].Width = 230; // Increase for better visibility
                    dgvDisplay.Columns["ItemDescription"].Width = 500; // Largest column for descriptions
                    dgvDisplay.Columns["WarehouseID"].Width = 115; // Compact for ID
                    dgvDisplay.Columns["WarehouseName"].Width = 170; // Medium width for warehouse names
                    dgvDisplay.Columns["ItemQty"].Width = 107; // Compact for numeric data

                    // Conditional formatting for the Quantity column
                    foreach (DataGridViewRow row in dgvDisplay.Rows)
                    {
                        if (row.Cells["ItemQty"].Value != null)
                        {
                            int quantity = Convert.ToInt32(row.Cells["ItemQty"].Value);
                            if (quantity > 0)
                            {
                                row.Cells["ItemQty"].Style.ForeColor = Color.Green;
                            }
                            else if (quantity == 0)
                            {
                                row.Cells["ItemQty"].Style.ForeColor = Color.Red;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading stock balance: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchText = txtSearch.Text.Trim();

                // SQL query to search by ItemID or ItemName
                string query = @"
        SELECT 
            I.ItemID, 
            I.Item_Name AS ItemName, 
            I.Item_Description AS ItemDescription, 
            SB.WarehouseID, 
            SB.WarehouseName, 
            ISNULL(SB.ItemQty, 0) AS ItemQty
        FROM 
            Items I
        LEFT JOIN 
            tblStockBalance SB 
        ON 
            I.ItemID = SB.ItemID
        WHERE 
            CAST(I.ItemID AS NVARCHAR) = @Search OR
            I.Item_Name LIKE '%' + @Search + '%'";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Search", searchText);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable searchResult = new DataTable();
                        adapter.Fill(searchResult);

                        // Bind search results to the DataGridView
                        dgvDisplay.DataSource = searchResult;

                        // Ensure consistent row height
                        dgvDisplay.RowTemplate.Height = 40; // Set template height
                        foreach (DataGridViewRow row in dgvDisplay.Rows)
                        {
                            row.Height = 40; // Ensure all rows are the same height
                        }

                        // Apply consistent styles
                        dgvDisplay.DefaultCellStyle.Font = new Font("Arial", 12);
                        dgvDisplay.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
                        dgvDisplay.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGreen;
                        dgvDisplay.EnableHeadersVisualStyles = false;

                        // Adjust column widths proportionally
                        dgvDisplay.Columns["ItemID"].Width = 115;
                        dgvDisplay.Columns["ItemName"].Width = 230;
                        dgvDisplay.Columns["ItemDescription"].Width = 500;
                        dgvDisplay.Columns["WarehouseID"].Width = 115;
                        dgvDisplay.Columns["WarehouseName"].Width = 170;
                        dgvDisplay.Columns["ItemQty"].Width = 107;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during search: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadStockBalance();
        }

    }
}
