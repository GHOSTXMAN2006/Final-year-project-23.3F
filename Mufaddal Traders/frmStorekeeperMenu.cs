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
    public partial class frmStorekeeperMenu : Form
    {

        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);


        public frmStorekeeperMenu()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void tileItems_Click(object sender, EventArgs e)
        {
            frmStorekeeperItems storekeeperItemForm = new frmStorekeeperItems();

            storekeeperItemForm.Show();

            this.Hide();
        }

        private void picHeader_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSettings_Click(object sender, EventArgs e)
        {

        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            // Create an instance of frmHome
            frmHome homeForm = new frmHome();

            // Show the frmHome
            homeForm.Show();

            // Close the current form (frmStorekeeperMenu)
            this.Hide();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {

        }

        private void btnHistory_Click(object sender, EventArgs e)
        {

        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            // Create an instance of frmDashboard
            frmDashboard dashboardForm = new frmDashboard();

            // Show the frmDashboard
            dashboardForm.Show();

            // Close the current form (frmStorekeeperMenu)
            this.Hide();
        }

        private void btnAccount_Click(object sender, EventArgs e)
        {
            // Create an instance of frmAccount
            frmAccount accountForm = new frmAccount();

            // Show the frmAccount
            accountForm.Show();

            // Close the current form (frmStorekeeperMenu)
            this.Hide();
        }

        private void pnlChat_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tilePC_Click(object sender, EventArgs e)
        {
            frmStorekeeperPurchaseContract storekeeperPurchaseContract = new frmStorekeeperPurchaseContract();

            storekeeperPurchaseContract.Show();

            this.Hide();
        }

        private void picHeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
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

        private void frmStorekeeperMenu_Load(object sender, EventArgs e)
        {

        }

        private void tileStock_Click(object sender, EventArgs e)
        {
            frmStorekeeperStock storekeeperStock = new frmStorekeeperStock();

            storekeeperStock.Show();

            this.Hide();
        }

        private void tileWarehouse_Click(object sender, EventArgs e)
        {
            frmStorekeeperWarehouse storekeeperWarehouse = new frmStorekeeperWarehouse();

            storekeeperWarehouse.Show();

            this.Hide();
        }

        private void tileStockTransfer_Click(object sender, EventArgs e)
        {
            frmStorekeeperStockTransfer storekeeperStockTransfer = new frmStorekeeperStockTransfer();

            storekeeperStockTransfer.Show();

            this.Hide();
        }

        private void tileSuppliers_Click(object sender, EventArgs e)
        {
            frmStorekeeperSuppliers storekeeperSuppliers = new frmStorekeeperSuppliers();

            storekeeperSuppliers.Show();

            this.Hide();
        }

        private void tilePO_Click(object sender, EventArgs e)
        {
            frmStorekeeperPurchaseOrder storekeeperPurchaseOrder = new frmStorekeeperPurchaseOrder();

            storekeeperPurchaseOrder.Show();

            this.Hide();
        }

        private void tileGIN_Click(object sender, EventArgs e)
        {
            frmStorekeeperGIN storekeeperGIN = new frmStorekeeperGIN();

            storekeeperGIN.Show();

            this.Hide();
        }
    }
}
