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
    public partial class frmCustomers : Form
    {
        private string connectionString = DatabaseConfig.ConnectionString;

        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public frmCustomers()
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
            // Add functionality as needed
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDisplay.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select one or more rows to delete.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    bool recordDeleted = false;

                    foreach (DataGridViewRow row in dgvDisplay.SelectedRows)
                    {
                        int customerID = Convert.ToInt32(row.Cells["CustomerID"].Value);

                        DialogResult result = MessageBox.Show(
                            $"Are you sure you want to delete Customer ID = {customerID}?",
                            "Delete Confirmation",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (result == DialogResult.Yes)
                        {
                            string deleteQuery = "DELETE FROM tblCustomers WHERE CustomerID = @CustomerID";
                            using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                            {
                                deleteCmd.Parameters.AddWithValue("@CustomerID", customerID);
                                deleteCmd.ExecuteNonQuery();
                                recordDeleted = true;
                            }
                        }
                    }

                    if (recordDeleted)
                    {
                        MessageBox.Show("Record(s) deleted successfully!",
                                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCustomerData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void frmCustomers_Load(object sender, EventArgs e)
        {
            LoadCustomerData();

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
        }

        private void LoadCustomerData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                    SELECT 
                        CustomerID,
                        Name,
                        Telephone,
                        Email,
                        Address,
                        [Description]
                    FROM tblCustomers";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvDisplay.DataSource = dt;

                    dgvDisplay.RowTemplate.Height = 40;
                    foreach (DataGridViewRow row in dgvDisplay.Rows)
                    {
                        row.Height = 40;
                    }

                    dgvDisplay.DefaultCellStyle.Font = new Font("Arial", 12);
                    dgvDisplay.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
                    dgvDisplay.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSeaGreen;
                    dgvDisplay.EnableHeadersVisualStyles = false;

                    dgvDisplay.Columns["CustomerID"].Width = 115;
                    dgvDisplay.Columns["Name"].Width = 160;
                    dgvDisplay.Columns["Telephone"].Width = 125;
                    dgvDisplay.Columns["Email"].Width = 200;
                    dgvDisplay.Columns["Address"].Width = 300;
                    dgvDisplay.Columns["Description"].Width = 336;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customers: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadCustomerData();
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            frmAddUpdateCustomers addUpdateCustomers = new frmAddUpdateCustomers();
            addUpdateCustomers.Show();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchText = txtSearch.Text.Trim();

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    LoadCustomerData();
                    return;
                }

                string query = @"
                SELECT 
                    CustomerID,
                    Name,
                    Telephone,
                    Email,
                    Address,
                    [Description]
                FROM tblCustomers
                WHERE 
                    CustomerID = @CustomerID OR 
                    Name LIKE '%' + @Name + '%'";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(query, conn);

                        if (int.TryParse(searchText, out int customerID))
                        {
                            cmd.Parameters.AddWithValue("@CustomerID", customerID);
                            cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                            cmd.Parameters.AddWithValue("@Name", searchText);
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable searchResult = new DataTable();
                        adapter.Fill(searchResult);

                        dgvDisplay.DataSource = searchResult;

                        dgvDisplay.RowTemplate.Height = 40;
                        foreach (DataGridViewRow row in dgvDisplay.Rows)
                        {
                            row.Height = 40;
                        }

                        dgvDisplay.DefaultCellStyle.Font = new Font("Arial", 12);
                        dgvDisplay.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
                        dgvDisplay.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSeaGreen;
                        dgvDisplay.EnableHeadersVisualStyles = false;

                        dgvDisplay.Columns["CustomerID"].Width = 115;
                        dgvDisplay.Columns["Name"].Width = 160;
                        dgvDisplay.Columns["Telephone"].Width = 125;
                        dgvDisplay.Columns["Email"].Width = 200;
                        dgvDisplay.Columns["Address"].Width = 300;
                        dgvDisplay.Columns["Description"].Width = 336;
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
