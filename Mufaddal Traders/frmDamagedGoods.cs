using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Mufaddal_Traders
{
    public partial class frmDamagedGoods : Form
    {

        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private string connectionString = DatabaseConfig.ConnectionString;

        public frmDamagedGoods()
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
            // To be implemented later
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

        private void frmStorekeeperStockDamage_Load(object sender, EventArgs e)
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

            LoadDamagedGoods();
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

            this.Close(); // Close the current form
            frmLogin loginForm = new frmLogin(); // Show login form
            loginForm.Show();
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            frmAddUpdateDamagedGoods addUpdateStockDamage = new frmAddUpdateDamagedGoods();
            addUpdateStockDamage.Show();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // If search box is empty, reload all data
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    LoadDamagedGoods(); // Load all damaged goods records
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
            SELECT StockDamageID, ItemID, ItemName, WarehouseID, WarehouseName, ItemQty
            FROM tblDamagedGoods
            WHERE StockDamageID = @StockDamageID OR ItemName LIKE @ItemName OR WarehouseName LIKE @WarehouseName";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StockDamageID", txtSearch.Text.Trim());
                    cmd.Parameters.AddWithValue("@ItemName", "%" + txtSearch.Text.Trim() + "%");
                    cmd.Parameters.AddWithValue("@WarehouseName", "%" + txtSearch.Text.Trim() + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable searchResult = new DataTable();

                    try
                    {
                        conn.Open();
                        adapter.Fill(searchResult);

                        if (searchResult.Rows.Count > 0)
                        {
                            dgvDisplay.DataSource = searchResult;

                            // Apply consistent DataGridView formatting
                            ApplyDataGridViewFormatting();
                        }
                        else
                        {
                            MessageBox.Show("No records found for the entered search term.", "No Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDamagedGoods(); // Reload all data if no records are found
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error during search: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void LoadDamagedGoods()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT StockDamageID, ItemID, ItemName, WarehouseID, WarehouseName, ItemQty
                FROM tblDamagedGoods";

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
                    MessageBox.Show("Error loading damaged goods: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ApplyDataGridViewFormatting()
        {
            // Set column headers
            dgvDisplay.Columns["StockDamageID"].HeaderText = "Stock Damage ID";
            dgvDisplay.Columns["ItemID"].HeaderText = "Item ID";
            dgvDisplay.Columns["ItemName"].HeaderText = "Item Name";
            dgvDisplay.Columns["WarehouseID"].HeaderText = "Warehouse ID";
            dgvDisplay.Columns["WarehouseName"].HeaderText = "Warehouse Name";
            dgvDisplay.Columns["ItemQty"].HeaderText = "Quantity";

            // Adjust column widths proportionally
            dgvDisplay.Columns["StockDamageID"].Width = (int)(1281 * 0.13);  // ~10%
            dgvDisplay.Columns["ItemID"].Width = (int)(1281 * 0.15);         // ~10%
            dgvDisplay.Columns["ItemName"].Width = (int)(1281 * 0.20);       // ~20%
            dgvDisplay.Columns["WarehouseID"].Width = (int)(1281 * 0.15);    // ~10%
            dgvDisplay.Columns["WarehouseName"].Width = (int)(1281 * 0.195);  // ~25%
            dgvDisplay.Columns["ItemQty"].Width = (int)(1281 * 0.13);        // ~15%

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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDisplay.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Retrieve the selected `StockDamageID`
            int selectedStockDamageID = Convert.ToInt32(dgvDisplay.SelectedRows[0].Cells["StockDamageID"].Value);

            // Ask for confirmation before deleting
            DialogResult confirmation = MessageBox.Show($"Are you sure you want to delete Stock Damage ID {selectedStockDamageID}?",
                                                         "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmation == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string deleteQuery = "DELETE FROM tblDamagedGoods WHERE StockDamageID = @StockDamageID";

                        SqlCommand cmd = new SqlCommand(deleteQuery, conn);
                        cmd.Parameters.AddWithValue("@StockDamageID", selectedStockDamageID);

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Record deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDamagedGoods(); // Reload the DataGridView after deletion
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete the record. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while deleting the record: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadDamagedGoods();
        }
    }
}
