using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmManagePurchaseOrder : Form
    {
        public frmManagePurchaseOrder()
        {
            InitializeComponent();
        }

        private void frmManagePurchaseOrder_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            pnlpurchaseOrder.BackColor = Color.FromArgb(100, 255, 255, 255);
        }
    }
}
