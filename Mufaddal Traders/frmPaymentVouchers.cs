using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmPaymentVouchers : Form
    {
        private readonly string connectionString = DatabaseConfig.ConnectionString;

        // DLL imports for dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public frmPaymentVouchers()
        {
            InitializeComponent();
        }

        private void frmPaymentVouchers_Load(object sender, EventArgs e)
        {
            frmLogin.userType = "Storekeeper";
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

            LoadPaymentVouchers(); // Load data on form load
        }

        private void LoadPaymentVouchers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                    SELECT 
                        PaymentID, 
                        PurchaseType, 
                        PurchaseID, 
                        PurchaseDetails, 
                        PaymentAmount, 
                        Status
                    FROM tblPaymentVoucher";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable paymentVouchersTable = new DataTable();
                    adapter.Fill(paymentVouchersTable);

                    dgvDisplay.DataSource = paymentVouchersTable;
                    ApplyDataGridViewFormatting(); // Apply styling
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading payment vouchers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchText = txtSearch.Text.Trim();

                // If searchText is empty, reload the full list
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    Debug.WriteLine("Search text is empty. Reloading full payment vouchers list.");
                    LoadPaymentVouchers(); // Load all payment vouchers
                    return;
                }

                // Validate numeric input
                if (!int.TryParse(searchText, out int paymentID))
                {
                    MessageBox.Show("Please enter a valid numeric Payment ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string query = @"
        SELECT 
            PaymentID, 
            PurchaseType, 
            PurchaseID, 
            PurchaseDetails, 
            PaymentAmount, 
            Status
        FROM tblPaymentVoucher
        WHERE PaymentID = @PaymentID";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@PaymentID", paymentID);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable searchResult = new DataTable();
                        adapter.Fill(searchResult);

                        if (searchResult.Rows.Count == 0)
                        {
                            MessageBox.Show("No payment voucher found with the entered Payment ID.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadPaymentVouchers(); // Reload all data if no matching record is found
                        }
                        else
                        {
                            dgvDisplay.DataSource = searchResult; // Display search results
                            ApplyDataGridViewFormatting(); // Ensure consistent styling
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during search: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ApplyDataGridViewFormatting()
        {
            dgvDisplay.RowTemplate.Height = 40;
            foreach (DataGridViewRow row in dgvDisplay.Rows)
            {
                row.Height = 40;
            }

            dgvDisplay.DefaultCellStyle.Font = new Font("Arial", 12);
            dgvDisplay.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
            dgvDisplay.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSeaGreen;
            dgvDisplay.EnableHeadersVisualStyles = false;

            // Adjust column widths proportionally
            dgvDisplay.Columns["PaymentID"].Width = (int)(1281 * 0.15);
            dgvDisplay.Columns["PurchaseType"].Width = (int)(1281 * 0.15);
            dgvDisplay.Columns["PurchaseID"].Width = (int)(1281 * 0.15);
            dgvDisplay.Columns["PurchaseDetails"].Width = (int)(1281 * 0.3);
            dgvDisplay.Columns["PaymentAmount"].Width = (int)(1281 * 0.15);
            dgvDisplay.Columns["Status"].Width = (int)(1281 * 0.15);

            // Conditional formatting
            foreach (DataGridViewRow row in dgvDisplay.Rows)
            {
                // Formatting for "Status" column
                if (row.Cells["Status"].Value != null)
                {
                    string status = row.Cells["Status"].Value.ToString();
                    row.Cells["Status"].Style.ForeColor = status == "Y" ? Color.Green : Color.Red; // Only color, no bold
                }

                // Formatting for "PurchaseType" column
                if (row.Cells["PurchaseType"].Value != null)
                {
                    string purchaseType = row.Cells["PurchaseType"].Value.ToString();
                    if (purchaseType == "C")
                    {
                        row.Cells["PurchaseType"].Style.ForeColor = Color.CadetBlue; // Cadet Blue for "C"
                        row.Cells["PurchaseType"].Style.Font = new Font("Arial", 12, FontStyle.Bold); // Bold and colored
                    }
                    else if (purchaseType == "O")
                    {
                        row.Cells["PurchaseType"].Style.ForeColor = Color.YellowGreen; // Yellow Green for "O"
                        row.Cells["PurchaseType"].Style.Font = new Font("Arial", 12, FontStyle.Bold); // Bold and colored
                    }
                }
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadPaymentVouchers(); // Reload data
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            frmAddUpdatePaymentVoucher managePaymentVoucherForm = new frmAddUpdatePaymentVoucher();
            managePaymentVoucherForm.ShowDialog();
            LoadPaymentVouchers(); // Reload after management
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Check if a row is selected in the DataGridView
            if (dgvDisplay.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a payment voucher to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the PaymentID of the selected row
            int selectedPaymentID = Convert.ToInt32(dgvDisplay.SelectedRows[0].Cells["PaymentID"].Value);

            // Confirm deletion
            DialogResult confirmResult = MessageBox.Show($"Are you sure you want to delete Payment ID {selectedPaymentID}?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult != DialogResult.Yes)
            {
                return; // Cancel deletion if the user selects "No"
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM tblPaymentVoucher WHERE PaymentID = @PaymentID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PaymentID", selectedPaymentID);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Payment voucher deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadPaymentVouchers(); // Reload data after deletion
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete the selected payment voucher. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during deletion: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            frmStorekeeperMenu menuForm = new frmStorekeeperMenu();
            menuForm.Show();
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

        private void btnHistory_Click(object sender, EventArgs e)
        {

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
    }
}
