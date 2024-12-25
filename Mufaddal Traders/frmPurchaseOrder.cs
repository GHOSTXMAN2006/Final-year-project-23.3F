using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmPurchaseOrder : Form
    {
        private readonly string connectionString = @"Data source=DESKTOP-O0Q3714\SQLEXPRESS ; Initial Catalog=Mufaddal_Traders_db ; Integrated Security=True";

        // DLL imports for dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public frmPurchaseOrder()
        {
            InitializeComponent();
        }

        private void frmPurchaseOrder_Load(object sender, EventArgs e)
        {
            frmLogin.userType = "Storekeeper";
            // Check the userType and show/hide buttons accordingly
            if (frmLogin.userType != "Storekeeper")
            {
                btnAdd.Visible = false;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
            }
            else
            {
                btnAdd.Visible = true;
                btnUpdate.Visible = true;
                btnDelete.Visible = true;
            }


            LoadPurchaseOrders();
        }

        private void LoadPurchaseOrders()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
            SELECT 
                PurchaseOrderID, 
                ItemID, 
                ItemName, 
                ItemQty, 
                SupplierID, 
                Status
            FROM Purchase_Orders";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable purchaseOrdersTable = new DataTable();
                    adapter.Fill(purchaseOrdersTable);

                    // Bind data to DataGridView
                    dgvDisplay.DataSource = purchaseOrdersTable;

                    // Set DataGridView dimensions
                    dgvDisplay.Width = 1281;
                    dgvDisplay.Height = 664;

                    // Set row formatting
                    dgvDisplay.RowTemplate.Height = 40;
                    foreach (DataGridViewRow row in dgvDisplay.Rows)
                    {
                        row.Height = 40;
                    }

                    dgvDisplay.DefaultCellStyle.Font = new Font("Arial", 12);

                    // Set column header style (matching frmStockBalance)
                    dgvDisplay.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
                    dgvDisplay.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSeaGreen; // Set header background color
                    dgvDisplay.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvDisplay.EnableHeadersVisualStyles = false;

                    // Adjust column widths proportionally
                    dgvDisplay.Columns["PurchaseOrderID"].Width = (int)(1281 * 0.15); // ~15%
                    dgvDisplay.Columns["ItemID"].Width = (int)(1281 * 0.1);          // ~10%
                    dgvDisplay.Columns["ItemName"].Width = (int)(1281 * 0.3);       // ~30%
                    dgvDisplay.Columns["ItemQty"].Width = (int)(1281 * 0.1);        // ~10%
                    dgvDisplay.Columns["SupplierID"].Width = (int)(1281 * 0.15);    // ~15%
                    dgvDisplay.Columns["Status"].Width = (int)(1281 * 0.2);         // ~20%

                    // Conditional formatting for the Status column
                    foreach (DataGridViewRow row in dgvDisplay.Rows)
                    {
                        if (row.Cells["Status"].Value != null)
                        {
                            string status = row.Cells["Status"].Value.ToString();
                            if (status == "N")
                            {
                                row.Cells["Status"].Style.ForeColor = Color.Red;
                            }
                            else if (status == "Y")
                            {
                                row.Cells["Status"].Style.ForeColor = Color.Green;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading purchase orders: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchText = txtSearch.Text.Trim();

                string query = @"
                SELECT 
                    PurchaseOrderID, 
                    ItemID, 
                    ItemName, 
                    ItemQty, 
                    SupplierID, 
                    Status
                FROM Purchase_Orders
                WHERE 
                    CAST(PurchaseOrderID AS NVARCHAR) = @Search OR
                    ItemName LIKE '%' + @Search + '%' OR
                    CAST(SupplierID AS NVARCHAR) = @Search";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Search", searchText);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable searchResult = new DataTable();
                        adapter.Fill(searchResult);

                        dgvDisplay.DataSource = searchResult;

                        // Apply consistent row formatting
                        dgvDisplay.RowTemplate.Height = 40;
                        foreach (DataGridViewRow row in dgvDisplay.Rows)
                        {
                            row.Height = 40;
                        }
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
            LoadPurchaseOrders();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddUpdatePurchaseOrders addPurchaseOrderForm = new frmAddUpdatePurchaseOrders
            {
                OperationMode = "Add"
            };
            addPurchaseOrderForm.ShowDialog();
            LoadPurchaseOrders();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvDisplay.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedPurchaseOrderID = Convert.ToInt32(dgvDisplay.SelectedRows[0].Cells["PurchaseOrderID"].Value);

            frmAddUpdatePurchaseOrders updatePurchaseOrderForm = new frmAddUpdatePurchaseOrders
            {
                OperationMode = "Update",
                PurchaseOrderID = selectedPurchaseOrderID
            };
            updatePurchaseOrderForm.ShowDialog();
            LoadPurchaseOrders();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDisplay.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedPurchaseOrderID = Convert.ToInt32(dgvDisplay.SelectedRows[0].Cells["PurchaseOrderID"].Value);

            DialogResult result = MessageBox.Show($"Are you sure you want to delete Purchase Order ID {selectedPurchaseOrderID}?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM Purchase_Orders WHERE PurchaseOrderID = @PurchaseOrderID";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@PurchaseOrderID", selectedPurchaseOrderID);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Purchase order deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadPurchaseOrders();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while deleting the purchase order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            frmLogin.profilePicture = null;

            this.Close();

            frmLogin loginForm = new frmLogin();
            loginForm.Show();
        }
    }
}
