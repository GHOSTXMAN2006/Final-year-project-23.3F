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
            LoadPurchaseOrders();
        }

        // Load data into dgvDisplay
        private void LoadPurchaseOrders()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Purchase_Orders";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvDisplay.DataSource = dt;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddUpdatePurchaseOrders addForm = new frmAddUpdatePurchaseOrders();
            addForm.OperationMode = "Add";
            addForm.ShowDialog();

            // Refresh DataGridView after adding
            LoadPurchaseOrders();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvDisplay.SelectedRows.Count != 1)
            {
                MessageBox.Show("Please select a single row to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dgvDisplay.SelectedRows[0];
            frmAddUpdatePurchaseOrders updateForm = new frmAddUpdatePurchaseOrders
            {
                OperationMode = "Update",
                PurchaseOrderID = Convert.ToInt32(selectedRow.Cells["PurchaseOrderID"].Value),
                ItemID = Convert.ToInt32(selectedRow.Cells["ItemID"].Value),
                ItemName = selectedRow.Cells["ItemName"].Value.ToString(),
                ItemQty = Convert.ToInt32(selectedRow.Cells["ItemQty"].Value),
                SupplierID = Convert.ToInt32(selectedRow.Cells["SupplierID"].Value)
            };

            updateForm.ShowDialog();

            // Refresh DataGridView after updating
            LoadPurchaseOrders();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDisplay.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select rows to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete the selected rows?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    foreach (DataGridViewRow row in dgvDisplay.SelectedRows)
                    {
                        int purchaseOrderID = Convert.ToInt32(row.Cells["PurchaseOrderID"].Value);
                        string query = "DELETE FROM Purchase_Orders WHERE PurchaseOrderID = @PurchaseOrderID";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@PurchaseOrderID", purchaseOrderID);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                // Refresh DataGridView after deletion
                LoadPurchaseOrders();
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
