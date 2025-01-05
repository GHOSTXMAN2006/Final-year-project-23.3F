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
    public partial class frmITadminMenu : Form
    {
        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private string connectionString = DatabaseConfig.ConnectionString;

        public frmITadminMenu()
        {
            InitializeComponent();
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            frmAccount accountForm = new frmAccount();

            accountForm.Show();

            this.Hide();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            frmDashboard dashboardForm = new frmDashboard();

            dashboardForm.Show();

            this.Hide();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            frmHome homeForm = new frmHome();

            homeForm.Show();

            this.Hide();
        }

        private void frmITadminMenu_Load(object sender, EventArgs e)
        {

        }
    }
}
