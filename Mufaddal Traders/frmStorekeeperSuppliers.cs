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
    public partial class frmStorekeeperSuppliers : Form
    {

        private string connectionString = @"Data source=DESKTOP-O0Q3714\SQLEXPRESS ; Initial Catalog=Mufaddal_Traders_db ; Integrated Security=True";

        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);


        public frmStorekeeperSuppliers()
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
            frmAddUpdateSuppliers addUpdateSuppliers = new frmAddUpdateSuppliers();

            addUpdateSuppliers.Show();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddUpdateSuppliers addUpdateSuppliers = new frmAddUpdateSuppliers();

            addUpdateSuppliers.Show();
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

        private void frmStorekeeperSuppliers_Load(object sender, EventArgs e)
        {

            LoadSupplierData();


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
        }

        private void LoadSupplierData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    // Define your query here to get the data (for example from the Suppliers table)
                    string query = "SELECT SupplierID, SupplierName, SupplierAddress, SupplierContact FROM Suppliers";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Set the DataSource of the DataGridView to the DataTable
                    dgvDisplay.DataSource = dt;

                    // Optionally, you can adjust the column headers if needed
                    dgvDisplay.Columns["SupplierID"].HeaderText = "Supplier ID";
                    dgvDisplay.Columns["SupplierName"].HeaderText = "Supplier Name";
                    dgvDisplay.Columns["SupplierAddress"].HeaderText = "Supplier Address";
                    dgvDisplay.Columns["SupplierContact"].HeaderText = "Supplier Contact";

                    // You can also modify the column widths and other styles
                    dgvDisplay.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading suppliers: " + ex.Message);
                }
            }
        }
    }
}
