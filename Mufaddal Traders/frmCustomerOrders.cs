using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmCustomerOrders : Form
    {
        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private readonly string connectionString = DatabaseConfig.ConnectionString;

        public frmCustomerOrders()
        {
            InitializeComponent();
        }

        private void frmCustomerOrders_Load(object sender, EventArgs e)
        {
            // Check the userType and show/hide buttons accordingly
            if (frmLogin.userType != "Marketing and Sales Department")
            {
                btnManage.Visible = false;
                btnDelete.Visible = false;
            }
            else
            {
                btnManage.Visible = true;
                btnDelete.Visible = true;
            }
            LoadCustomerOrdersData(); // Load all data on form load
        }

        private void LoadCustomerOrdersData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                    SELECT 
                        CustomerOrderID,
                        ItemID,
                        ItemName,
                        ItemQty,
                        CustomerID,
                        OrderDate,
                        Status,
                        LocalOrExport
                    FROM tblCustomerOrders";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable customerOrdersTable = new DataTable();
                    adapter.Fill(customerOrdersTable);

                    // Bind data to DataGridView
                    dgvDisplay.DataSource = customerOrdersTable;

                    ApplyStyling(); // Apply styling to DataGridView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading customer orders: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyStyling()
        {
            // Set row height
            dgvDisplay.RowTemplate.Height = 40;
            foreach (DataGridViewRow row in dgvDisplay.Rows)
            {
                row.Height = 40;
            }

            // Default cell style
            dgvDisplay.DefaultCellStyle.Font = new Font("Arial", 12);
            dgvDisplay.DefaultCellStyle.BackColor = Color.White;

            // Header styling
            dgvDisplay.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
            dgvDisplay.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSeaGreen;
            dgvDisplay.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvDisplay.EnableHeadersVisualStyles = false;

            // Adjust column widths proportionally
            dgvDisplay.Columns["CustomerOrderID"].Width = (int)(dgvDisplay.Width * 0.14);
            dgvDisplay.Columns["ItemID"].Width = (int)(dgvDisplay.Width * 0.10);
            dgvDisplay.Columns["ItemName"].Width = (int)(dgvDisplay.Width * 0.15);
            dgvDisplay.Columns["ItemQty"].Width = (int)(dgvDisplay.Width * 0.10);
            dgvDisplay.Columns["CustomerID"].Width = (int)(dgvDisplay.Width * 0.10);
            dgvDisplay.Columns["OrderDate"].Width = (int)(dgvDisplay.Width * 0.15);
            dgvDisplay.Columns["Status"].Width = (int)(dgvDisplay.Width * 0.10);
            dgvDisplay.Columns["LocalOrExport"].Width = (int)(dgvDisplay.Width * 0.12);

            // Custom styling for Status and LocalOrExport columns
            foreach (DataGridViewRow row in dgvDisplay.Rows)
            {
                if (row.Cells["Status"].Value != null)
                {
                    string status = row.Cells["Status"].Value.ToString();
                    row.Cells["Status"].Style.ForeColor = status == "N" ? Color.Red : Color.Green;
                    row.Cells["Status"].Style.Font = new Font("Arial", 12, FontStyle.Regular); // Keep font consistent
                }

                if (row.Cells["LocalOrExport"].Value != null)
                {
                    string localOrExport = row.Cells["LocalOrExport"].Value.ToString();
                    row.Cells["LocalOrExport"].Style.ForeColor = localOrExport == "L" ? Color.CadetBlue : Color.YellowGreen;
                    row.Cells["LocalOrExport"].Style.Font = new Font("Arial", 12, FontStyle.Bold);
                }
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchValue = txtSearch.Text.Trim();
                if (string.IsNullOrEmpty(searchValue))
                {
                    LoadCustomerOrdersData(); // Load all data if search is empty
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = @"
                        SELECT 
                            CustomerOrderID,
                            ItemID,
                            ItemName,
                            ItemQty,
                            CustomerID,
                            OrderDate,
                            Status,
                            LocalOrExport
                        FROM tblCustomerOrders
                        WHERE CAST(CustomerOrderID AS NVARCHAR) LIKE @SearchID";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@SearchID", $"%{searchValue}%");
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable searchResult = new DataTable();
                        adapter.Fill(searchResult);

                        // Bind search results to DataGridView
                        dgvDisplay.DataSource = searchResult;

                        ApplyStyling(); // Apply styling after search results

                        if (searchResult.Rows.Count == 0)
                        {
                            MessageBox.Show("No records found for the given search criteria.", "No Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during search: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

        private void btnManage_Click(object sender, EventArgs e)
        {
            frmAddUpdateCustomerOrder addUpdateCustomerOrder = new frmAddUpdateCustomerOrder();
            addUpdateCustomerOrder.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            frmLogin.userType = string.Empty;
            frmLogin.userName = string.Empty;
            frmLogin.userPassword = string.Empty;
            frmLogin.userEmail = string.Empty;
            frmLogin.userTelephone = string.Empty;
            frmLogin.userAddress = string.Empty;
            frmLogin.userDescription = string.Empty;
            frmLogin.profilePicture = null;
            this.Close();
            frmLogin loginForm = new frmLogin();
            loginForm.Show();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadCustomerOrdersData(); // Reload all data
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDisplay.SelectedRows.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show(
                    "Are you sure you want to delete the selected customer order?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (dialogResult == DialogResult.Yes)
                {
                    int selectedOrderID = Convert.ToInt32(dgvDisplay.SelectedRows[0].Cells["CustomerOrderID"].Value);

                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            SqlTransaction transaction = conn.BeginTransaction();

                            try
                            {
                                // Delete the customer order record
                                string deleteQuery = @"
                        DELETE FROM tblCustomerOrders
                        WHERE CustomerOrderID = @CustomerOrderID";

                                SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn, transaction);
                                deleteCmd.Parameters.AddWithValue("@CustomerOrderID", selectedOrderID);
                                deleteCmd.ExecuteNonQuery();

                                transaction.Commit();

                                MessageBox.Show("Customer order deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Reload the data grid view
                                LoadCustomerOrdersData();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                MessageBox.Show($"Error deleting customer order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a customer order to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
