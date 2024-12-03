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
    public partial class frmHomeInterface : Form
    {

        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public frmHomeInterface()
        {
            InitializeComponent();
        }


        private void frmHomeInterface_Load(object sender, EventArgs e)
        {
            // Use working area to respect taskbar space
            this.FormBorderStyle = FormBorderStyle.None;  // Removes borders
            this.StartPosition = FormStartPosition.Manual; // Allows custom placement
            this.Bounds = Screen.PrimaryScreen.WorkingArea; // Excludes taskbar area
            this.TopMost = false;  // Ensures form does not cover the taskbar
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {

        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
