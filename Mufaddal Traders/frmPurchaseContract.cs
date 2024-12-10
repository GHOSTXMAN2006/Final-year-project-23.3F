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

        private string connectionString = @"Data source=DESKTOP-O0Q3714\SQLEXPRESS ; Initial Catalog=Mufaddal_Traders_db ; Integrated Security=True";


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

        private void btnSettings_Click(object sender, EventArgs e)
        {
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            frmAddUpdatePurchaseContract addUpdatePurchaseContract = new frmAddUpdatePurchaseContract();

            addUpdatePurchaseContract.Show();
        }

        private void btnAdd_Click(object sender, EventArgs e)
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

                    // Format the DataGridView columns
                    dgvDisplay.Columns["PurchaseContractID"].HeaderText = "Contract ID";
                    dgvDisplay.Columns["SupplierID"].HeaderText = "Supplier ID";
                    dgvDisplay.Columns["SupplierName"].HeaderText = "Supplier Name";
                    dgvDisplay.Columns["StartDate"].HeaderText = "Start Date";
                    dgvDisplay.Columns["EndDate"].HeaderText = "End Date";
                    dgvDisplay.Columns["ItemID"].HeaderText = "Item ID";
                    dgvDisplay.Columns["Item_Name"].HeaderText = "Item Name";
                    dgvDisplay.Columns["Description"].HeaderText = "Description";

                    dgvDisplay.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading purchase contracts: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frmPurchaseContract_Load(object sender, EventArgs e)
        {
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


            LoadPurchaseContracts();
        }
    }
}
