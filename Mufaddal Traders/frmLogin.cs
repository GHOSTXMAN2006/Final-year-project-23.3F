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
    public partial class frmLogin : Form
    {

        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);


        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            //pnlLoginInterface1.BackColor = Color.FromArgb(100, 0, 0, 0);
            this.pnlCreateAcc.Visible = false;
            pnlNewPassword.Visible = false;
            pnlForgotPassword.Visible = false;
            pnlForgotPassword.BackColor = Color.FromArgb(150, 192, 192, 192);
            pnlCreateAcc.BackColor = Color.FromArgb(150, 192, 192, 192);
            pnlLoginInterface1.BackColor = Color.FromArgb(150, 192, 192, 192);
            pnlNewPassword.BackColor = Color.FromArgb(150, 192, 192, 192);
            





            /*// Set the label's background to transparent
            label4.BackColor = Color.Transparent;

            // Assign the parent (the form itself or a panel with the background)
            label4.Parent = this; // 'this' refers to the current form

            // Force a redraw to ensure transparency is applied
            label4.Invalidate();*/
        }

        private void lblCreateAccount_Click(object sender, EventArgs e)
        {
            pnlCreateAcc.Visible = true;
            pnlLoginInterface1.Visible = false;
        }

        private void lblLogin_Click(object sender, EventArgs e)
        {
            pnlLoginInterface1.Visible = true;
            pnlCreateAcc.Visible = false;
        }

        private void lblForgotpassword_Click(object sender, EventArgs e)
        {
            pnlForgotPassword.Visible = true;
            pnlLoginInterface1.Visible = false;
        }

        private void picBackToLogin_Click(object sender, EventArgs e)
        {
            pnlLoginInterface1.Visible = true;
            pnlForgotPassword.Visible = false;
        }

        private void picHeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        // Custom close button logic
        private void picClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Custom minimize button logic
        private void picMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pnlNewPassword_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

    }
}
