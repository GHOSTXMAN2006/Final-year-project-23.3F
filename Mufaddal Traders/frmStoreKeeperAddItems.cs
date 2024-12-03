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
    public partial class frmStoreKeeperAddItems : Form
    {
        public frmStoreKeeperAddItems()
        {
            InitializeComponent();
        }

        private void frmStoreKeeperAddItems_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            pnlAddItems.BackColor = Color.FromArgb(100, 255, 255, 255);
        }

        private void pnlChannelling_Paint(object sender, PaintEventArgs e)
        {

        }

        private void picBoxSupplierDetails_Click(object sender, EventArgs e)
        {

        }
    }
}
