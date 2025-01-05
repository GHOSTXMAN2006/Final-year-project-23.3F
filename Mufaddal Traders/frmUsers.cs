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
            this.Hide();
        }

        private void frmUsers_Load(object sender, EventArgs e)
        {

        }
    }
}
