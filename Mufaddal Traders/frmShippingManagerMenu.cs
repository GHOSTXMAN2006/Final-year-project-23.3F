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
    public partial class frmShippingManagerMenu : Form
    {

        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);


        public frmShippingManagerMenu()
        {
            InitializeComponent();
        }

        //ForMainForm
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

        private void tileGIN_Click(object sender, EventArgs e)
        {
            frmGIN ginForm = new frmGIN();

            ginForm.Show();

            this.Hide();
        }

        private void tileSRN_Click(object sender, EventArgs e)
        {
            frmSRN srnForm = new frmSRN();

            srnForm.Show();

            this.Hide();
        }

        private void tileCusOrder_Click(object sender, EventArgs e)
        {
            frmCustomerOrders customerOrdersForm = new frmCustomerOrders();

            customerOrdersForm.Show();

            this.Hide();
        }

        private void tileDelOrder_Click(object sender, EventArgs e)
        {
            frmDeliveryOrder deliveryOrdersForm = new frmDeliveryOrder();

            deliveryOrdersForm.Show();

            this.Hide();
        }

        private void tileShipment_Click(object sender, EventArgs e)
        {
            frmShipments shipmentsForm = new frmShipments();

            shipmentsForm.Show();

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

            // Optionally, you can also clear other session variables if needed

            // Close the current form (Dashboard)
            this.Close();

            // Show the login form again
            frmLogin loginForm = new frmLogin();
            loginForm.Show();
        }

        private void tileItems_Click(object sender, EventArgs e)
        {
            frmItems2 items2 = new frmItems2();

            items2.Show();

            this.Hide();
        }

        private void tileStock_Click(object sender, EventArgs e)
        {
            frmStockBalance stockBalance = new frmStockBalance();

            stockBalance.Show();

            this.Hide();
        }
    }
}
