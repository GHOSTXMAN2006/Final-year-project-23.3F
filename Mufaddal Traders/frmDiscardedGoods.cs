using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Mufaddal_Traders
{
    public partial class frmDiscardedGoods : Form
    {
        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private string connectionString = DatabaseConfig.ConnectionString;

        public frmDiscardedGoods()
        {
            InitializeComponent();
        }

        private void frmDiscardedGoods_Load(object sender, EventArgs e)
        {

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
            LoadDiscardedGoods();
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddDiscardedGoods addDiscardedGoods = new frmAddDiscardedGoods();

            addDiscardedGoods.Show();
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

        private void LoadDiscardedGoods()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT DiscardedGoodsID, ItemID, ItemName, WarehouseID, WarehouseName, ItemQty, Status
                FROM tblDiscardedGoods";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable table = new DataTable();

                try
                {
                    conn.Open();
                    adapter.Fill(table);
                    dgvDisplay.DataSource = table;

                    // Apply DataGridView formatting
                    ApplyDataGridViewFormatting();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading discarded goods: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ApplyDataGridViewFormatting()
        {
            // Set column headers
            dgvDisplay.Columns["DiscardedGoodsID"].HeaderText = "Discarded Goods ID";
            dgvDisplay.Columns["ItemID"].HeaderText = "Item ID";
            dgvDisplay.Columns["ItemName"].HeaderText = "Item Name";
            dgvDisplay.Columns["WarehouseID"].HeaderText = "Warehouse ID";
            dgvDisplay.Columns["WarehouseName"].HeaderText = "Warehouse Name";
            dgvDisplay.Columns["ItemQty"].HeaderText = "Quantity";
            dgvDisplay.Columns["Status"].HeaderText = "Status";

            // Adjust column widths proportionally
            dgvDisplay.Columns["DiscardedGoodsID"].Width = (int)(1281 * 0.12);
            dgvDisplay.Columns["ItemID"].Width = (int)(1281 * 0.10);
            dgvDisplay.Columns["ItemName"].Width = (int)(1281 * 0.20);
            dgvDisplay.Columns["WarehouseID"].Width = (int)(1281 * 0.10);
            dgvDisplay.Columns["WarehouseName"].Width = (int)(1281 * 0.22);
            dgvDisplay.Columns["ItemQty"].Width = (int)(1281 * 0.10);
            dgvDisplay.Columns["Status"].Width = (int)(1281 * 0.11);

            dgvDisplay.RowTemplate.Height = 40;
            dgvDisplay.DefaultCellStyle.Font = new Font("Arial", 12);
            dgvDisplay.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
            dgvDisplay.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSeaGreen;
            dgvDisplay.EnableHeadersVisualStyles = false;

            foreach (DataGridViewRow row in dgvDisplay.Rows)
            {
                // Apply bold, color, center alignment, and increased text size to the "Status" column
                DataGridViewCell statusCell = row.Cells["Status"];
                statusCell.Style.Font = new Font("Arial", 14, FontStyle.Bold);  // Increased font size to 16

                if (statusCell.Value.ToString() == "-")
                    statusCell.Style.ForeColor = Color.Red;  // Discarded
                else if (statusCell.Value.ToString() == "R")
                    statusCell.Style.ForeColor = Color.Green;  // Returned

                row.Height = 40;
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                    SELECT DiscardedGoodsID, ItemID, ItemName, WarehouseID, WarehouseName, ItemQty, Status
                    FROM tblDiscardedGoods
                    WHERE DiscardedGoodsID = @DiscardedGoodsID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@DiscardedGoodsID", txtSearch.Text.Trim());

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable searchResult = new DataTable();

                    try
                    {
                        conn.Open();
                        adapter.Fill(searchResult);

                        if (searchResult.Rows.Count > 0)
                        {
                            dgvDisplay.DataSource = searchResult;
                            ApplyDataGridViewFormatting();
                        }
                        else
                        {
                            MessageBox.Show("No records found for the entered Discarded Goods ID.", "No Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDiscardedGoods();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error during search: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDisplay.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedDiscardedGoodsID = Convert.ToInt32(dgvDisplay.SelectedRows[0].Cells["DiscardedGoodsID"].Value);
            int itemQty = Convert.ToInt32(dgvDisplay.SelectedRows[0].Cells["ItemQty"].Value);
            int itemID = Convert.ToInt32(dgvDisplay.SelectedRows[0].Cells["ItemID"].Value);
            int warehouseID = Convert.ToInt32(dgvDisplay.SelectedRows[0].Cells["WarehouseID"].Value);

            DialogResult confirmation = MessageBox.Show($"Are you sure you want to delete Discarded Goods ID {selectedDiscardedGoodsID}?",
                                                         "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmation == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        SqlTransaction transaction = conn.BeginTransaction();

                        string deleteQuery = "DELETE FROM tblDiscardedGoods WHERE DiscardedGoodsID = @DiscardedGoodsID";
                        SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn, transaction);
                        deleteCmd.Parameters.AddWithValue("@DiscardedGoodsID", selectedDiscardedGoodsID);
                        deleteCmd.ExecuteNonQuery();

                        string updateStockQuery = @"
                        UPDATE tblStockBalance
                        SET ItemQty = ItemQty + @ItemQty
                        WHERE ItemID = @ItemID AND WarehouseID = @WarehouseID";
                        SqlCommand updateCmd = new SqlCommand(updateStockQuery, conn, transaction);
                        updateCmd.Parameters.AddWithValue("@ItemQty", itemQty);
                        updateCmd.Parameters.AddWithValue("@ItemID", itemID);
                        updateCmd.Parameters.AddWithValue("@WarehouseID", warehouseID);
                        updateCmd.ExecuteNonQuery();

                        transaction.Commit();

                        MessageBox.Show("Record deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDiscardedGoods();
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
            LoadDiscardedGoods();
        }
    }
}
