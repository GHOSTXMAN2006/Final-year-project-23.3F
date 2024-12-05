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
    public partial class frmDashboard : Form
    {

        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);


        public frmDashboard()
        {
            InitializeComponent();
        }

        private void picHeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btnAccount_Click(object sender, EventArgs e)
        {
            // Create an instance of frmAccount
            frmAccount accountForm = new frmAccount();

            // Show the frmAccount
            accountForm.Show();

            // Close the current form (frmDashboard)
            this.Hide();
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            // Create an instance of frmStorekeeperMenu
            frmStorekeeperMenu menuForm = new frmStorekeeperMenu();

            // Show the frmStorekeeperMenu
            menuForm.Show();

            // Close the current form (frmDashboard)
            this.Hide();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            // Create an instance of frmHome
            frmHome homeForm = new frmHome();

            // Show the frmHome
            homeForm.Show();

            // Close the current form (frmDashboard)
            this.Hide();
        }
    }
}
