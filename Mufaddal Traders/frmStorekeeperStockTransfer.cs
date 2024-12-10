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
    public partial class frmStorekeeperStockTransfer : Form
    {

        private string connectionString = @"Data source=DESKTOP-O0Q3714\SQLEXPRESS ; Initial Catalog=Mufaddal_Traders_db ; Integrated Security=True";

        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);


        public frmStorekeeperStockTransfer()
        {
            InitializeComponent();
        }

        private void frmStorekeeperStockTransfer_Load(object sender, EventArgs e)
        {

            LoadDataIntoGridView(); // Call the method to load data into the grid


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
            frmStorekeeperMenu storekeeperMenu = new frmStorekeeperMenu();

            storekeeperMenu.Show();

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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddStockTransfer addStockTransfer = new frmAddStockTransfer();

            addStockTransfer.Show();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {

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

            // Optionally, you can also clear other session variables if needed

            // Close the current form (Dashboard)
            this.Close();

            // Show the login form again
            frmLogin loginForm = new frmLogin();
            loginForm.Show();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadDataIntoGridView(); // Call the method to load data into the grid
        }


        private void LoadDataIntoGridView()
        {
            // Create a connection to the database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    // SQL query to get stock transfer data
                    string query = @"
                SELECT 
                    ST.ST_ID, 
                    ST.ST_Date, 
                    ST.ST_Qty, 
                    I.Item_Name, 
                    W1.Store_Name AS StartingLocation, 
                    W2.Store_Name AS EndingLocation
                FROM 
                    Stock_Transfer ST
                JOIN 
                    Items I ON ST.ItemID = I.ItemID
                JOIN 
                    Warehouse W1 ON ST.StoreID = W1.StoreID
                JOIN 
                    Warehouse W2 ON ST.StoreID = W2.StoreID";  // Adjust if needed for ending location

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Bind the DataTable to the DataGridView
                    dgvST.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading stock transfer data: " + ex.Message);
                }
            }
        }


    }
}
