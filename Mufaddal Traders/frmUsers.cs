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
    public partial class frmUsers : Form
    {
        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private string connectionString = DatabaseConfig.ConnectionString;

        public frmUsers()
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

        private void btnMenu_Click(object sender, EventArgs e)
        {
            frmITadminMenu frmITadminMenu = new frmITadminMenu();
            frmITadminMenu.Show();
            this.Hide();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            frmHome frmHome = new frmHome();
            frmHome.Show();
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

            this.Close(); // Close the current form
            frmLogin loginForm = new frmLogin(); // Show login form
            loginForm.Show();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            frmITadminMenu iTadminMenu = new frmITadminMenu();
            iTadminMenu.Show();
            this.Hide();
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            frmAddUpdateUsers addUpdateUsers = new frmAddUpdateUsers();
            addUpdateUsers.Show();
        }

        private void frmUsers_Load(object sender, EventArgs e)
        {
            LoadUsers(); // Load all users when the form loads
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)  // Trigger search on Enter key press
            {
                string searchTerm = txtSearch.Text.Trim();
                if (string.IsNullOrEmpty(searchTerm))
                {
                    LoadUsers();  // Reload all users if the search box is empty
                }
                else
                {
                    SearchUsers(searchTerm);
                }
            }
        }

        private void LoadUsers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                SELECT 
                    UserID,
                    UserName,
                    UserEmail,
                    UserTelephone,
                    UserAddress,
                    Description,
                    UserType
                FROM Users";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable usersTable = new DataTable();
                    adapter.Fill(usersTable);

                    // Bind data to DataGridView
                    dgvDisplay.DataSource = usersTable;

                    ApplyDgvStyle();  // Apply DataGridView styling
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred while loading user data: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void SearchUsers(string searchTerm)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                SELECT 
                    UserID,
                    UserName,
                    UserEmail,
                    UserTelephone,
                    UserAddress,
                    Description,
                    UserType
                FROM Users
                WHERE CAST(UserID AS NVARCHAR) LIKE '%' + @SearchTerm + '%' 
                    OR UserName LIKE '%' + @SearchTerm + '%'
                    OR UserEmail LIKE '%' + @SearchTerm + '%'
                    OR UserTelephone LIKE '%' + @SearchTerm + '%'";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable searchResult = new DataTable();
                    adapter.Fill(searchResult);

                    dgvDisplay.DataSource = searchResult;

                    ApplyDgvStyle();  // Apply DataGridView styling after binding search results

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

        private void ApplyDgvStyle()
        {
            // Set row height
            dgvDisplay.RowTemplate.Height = 40;
            foreach (DataGridViewRow row in dgvDisplay.Rows)
            {
                row.Height = 40;
            }

            // Font and styling for cells
            dgvDisplay.DefaultCellStyle.Font = new Font("Arial", 12);

            // Header styling
            dgvDisplay.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
            dgvDisplay.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSeaGreen;
            dgvDisplay.EnableHeadersVisualStyles = false;

            // Adjust column widths proportionally
            dgvDisplay.Columns["UserID"].Width = (int)(dgvDisplay.Width * 0.08);
            dgvDisplay.Columns["UserName"].Width = (int)(dgvDisplay.Width * 0.15);
            dgvDisplay.Columns["UserEmail"].Width = (int)(dgvDisplay.Width * 0.20);
            dgvDisplay.Columns["UserTelephone"].Width = (int)(dgvDisplay.Width * 0.15);
            dgvDisplay.Columns["UserAddress"].Width = (int)(dgvDisplay.Width * 0.20);
            dgvDisplay.Columns["Description"].Width = (int)(dgvDisplay.Width * 0.15);
            dgvDisplay.Columns["UserType"].Width = (int)(dgvDisplay.Width * 0.15);

            Console.WriteLine("DEBUG: DataGridView styling applied.");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDisplay.SelectedRows.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show(
                    "Are you sure you want to delete the selected user?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (dialogResult == DialogResult.Yes)
                {
                    int selectedUserID = Convert.ToInt32(dgvDisplay.SelectedRows[0].Cells["UserID"].Value);
                    string selectedUserName = dgvDisplay.SelectedRows[0].Cells["UserName"].Value.ToString();

                    // Prevent deleting the admin user
                    if (selectedUserName.Equals("admin", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Cannot delete the admin user.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            SqlTransaction transaction = conn.BeginTransaction();

                            try
                            {
                                // Delete user
                                string deleteQuery = "DELETE FROM Users WHERE UserID = @UserID";
                                SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn, transaction);
                                deleteCmd.Parameters.AddWithValue("@UserID", selectedUserID);
                                deleteCmd.ExecuteNonQuery();

                                transaction.Commit();

                                MessageBox.Show("User deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Reload the DataGridView after deletion
                                LoadUsers();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                MessageBox.Show($"Error deleting user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Please select a user to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }
    }
}
