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
    public partial class frmLoginInterface : Form
    {
        public frmLoginInterface()
        {
            InitializeComponent();
        }

        private void frmLoginInterface_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            pnlLoginInterface1.BackColor = Color.FromArgb(100, 0, 0, 0);
            this.pnlCreateAcc.Visible = false;
            pnlNewPassword.Visible = false;
            pnlForgotPassword.Visible = false;
        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel1_Click_1(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            pnlCreateAcc.Visible = true;
            pnlLoginInterface1.Visible = false;
        }

        private void label8_Click(object sender, EventArgs e)
        {
            pnlLoginInterface1.Visible = true;
            pnlCreateAcc.Visible = false;
        }

        private void lblForgotpassword_Click(object sender, EventArgs e)
        {
            pnlForgotPassword.Visible = true;
            pnlLoginInterface1.Visible = false;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            pnlLoginInterface1.Visible = true;
            pnlForgotPassword.Visible=false;
        }
    }
}
