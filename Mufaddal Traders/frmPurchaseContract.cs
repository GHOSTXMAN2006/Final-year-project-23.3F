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
    public partial class frmPurchaseContract : Form
    {
        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private string connectionString = DatabaseConfig.ConnectionString;


        public frmPurchaseContract()
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDisplay.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Retrieve the selected PurchaseContractID
            int selectedContractID = Convert.ToInt32(dgvDisplay.SelectedRows[0].Cells["PurchaseContractID"].Value);

            // Ask for confirmation
            DialogResult confirmation = MessageBox.Show($"Are you sure you want to delete Purchase Contract ID {selectedContractID}?",
                                                         "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmation == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string deleteQuery = "DELETE FROM Purchase_Contract WHERE PurchaseContractID = @PurchaseContractID";
                        SqlCommand cmd = new SqlCommand(deleteQuery, conn);
                        cmd.Parameters.AddWithValue("@PurchaseContractID", selectedContractID);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Purchase contract deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadPurchaseContracts(); // Refresh the DataGridView
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete the purchase contract. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while deleting the purchase contract: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void btnManage_Click(object sender, EventArgs e)
        {
            frmAddUpdatePurchaseContract addUpdatePurchaseContract = new frmAddUpdatePurchaseContract();

            addUpdatePurchaseContract.Show();
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
            LoadPurchaseContracts();
        }

        private void LoadPurchaseContracts()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT PurchaseContractID, SupplierID, SupplierName, StartDate, EndDate, ItemID, Item_Name, Description 
                         FROM Purchase_Contract";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable table = new DataTable();

                try
                {
                    conn.Open();
                    adapter.Fill(table);
                    dgvDisplay.DataSource = table;

                    // Apply consistent DataGridView formatting
                    ApplyDataGridViewFormatting();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading purchase contracts: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void ApplyDataGridViewFormatting()
        {
            // Set column headers
            dgvDisplay.Columns["PurchaseContractID"].HeaderText = "Contract ID";
            dgvDisplay.Columns["SupplierID"].HeaderText = "Supplier ID";
            dgvDisplay.Columns["SupplierName"].HeaderText = "Supplier Name";
            dgvDisplay.Columns["StartDate"].HeaderText = "Start Date";
            dgvDisplay.Columns["EndDate"].HeaderText = "End Date";
            dgvDisplay.Columns["ItemID"].HeaderText = "Item ID";
            dgvDisplay.Columns["Item_Name"].HeaderText = "Item Name";
            dgvDisplay.Columns["Description"].HeaderText = "Description";

            // Adjust column widths proportionally
            dgvDisplay.Columns["PurchaseContractID"].Width = (int)(1281 * 0.10); // ~15%
            dgvDisplay.Columns["SupplierID"].Width = (int)(1281 * 0.10);        // ~15%
            dgvDisplay.Columns["SupplierName"].Width = (int)(1281 * 0.15);       // ~20%
            dgvDisplay.Columns["StartDate"].Width = (int)(1281 * 0.15);         // ~15%
            dgvDisplay.Columns["EndDate"].Width = (int)(1281 * 0.15);           // ~15%
            dgvDisplay.Columns["ItemID"].Width = (int)(1281 * 0.1);             // ~10%
            dgvDisplay.Columns["Item_Name"].Width = (int)(1281 * 0.15);          // ~20%
            dgvDisplay.Columns["Description"].Width = (int)(1281 * 0.25);        // ~30%

            // Set row formatting
            dgvDisplay.RowTemplate.Height = 40;
            foreach (DataGridViewRow row in dgvDisplay.Rows)
            {
                row.Height = 40;
            }

            // Set font styles
            dgvDisplay.DefaultCellStyle.Font = new Font("Arial", 12);
            dgvDisplay.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
            dgvDisplay.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSeaGreen;
            dgvDisplay.EnableHeadersVisualStyles = false;
        }



        private void frmPurchaseContract_Load(object sender, EventArgs e)
        {
            frmLogin.userType = "Storekeeper";
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


            LoadPurchaseContracts();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchText = txtSearch.Text.Trim();

                // Ensure the search text is numeric for PurchaseContractID
                if (!int.TryParse(searchText, out int contractID))
                {
                    MessageBox.Show("Please enter a valid numeric Purchase Contract ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string query = @"
            SELECT PurchaseContractID, SupplierID, SupplierName, StartDate, EndDate, ItemID, Item_Name, Description 
            FROM Purchase_Contract
            WHERE PurchaseContractID = @PurchaseContractID";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@PurchaseContractID", contractID);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable searchResult = new DataTable();

                        conn.Open();
                        adapter.Fill(searchResult);

                        // Bind the search results to the DataGridView
                        dgvDisplay.DataSource = searchResult;

                        // Apply consistent formatting
                        ApplyDataGridViewFormatting();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error during search: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
