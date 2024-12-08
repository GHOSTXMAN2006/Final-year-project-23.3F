using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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

            // Configure label4 (Heading: Welcome to MTSMS)
            label4.Parent = pictureBox2; // Align with the background PictureBox
            label4.Text = "Welcome to MTSMS,";
            label4.Font = new Font("Microsoft Sans Serif", 40, FontStyle.Bold); // Adjust the font size
            label4.ForeColor = Color.Black; // Set the text color
            label4.BackColor = Color.Transparent; // Make the background transparent
            label4.AutoSize = true; // Adjust the size to fit the text
            label4.Location = new Point(30, 100); // Set the position

            // Configure label8 (Subtext: Description of MTSMS)
            label8.Parent = pictureBox2; // Align with the background PictureBox
            label8.Text = "Your ultimate solution for seamless stock management. \r\nOptimize your inventory with precision and efficiency, empowering \r\nyour business to thrive. \r\n\r\nExperience the simplicity of innovation in managing your stock \r\nlike never before.";
            label8.Font = new Font("Microsoft Sans Serif", 16, FontStyle.Regular); // Adjust the font size
            label8.ForeColor = Color.Black; // Set the text color
            label8.BackColor = Color.Transparent; // Make the background transparent
            label8.AutoSize = true; // Adjust the size to fit the text
            label8.Location = new Point(35, 185); // Set the position below label4

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

        private void picHeader2_MouseDown(object sender, MouseEventArgs e)
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

        private void pnlNewPassword_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void transparentLabel2_Click(object sender, EventArgs e)
        {

        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            // Create an instance of frmHome
            frmHome homeForm = new frmHome();

            // Show the frmHome
            homeForm.Show();

            // Close the current form (frmLogin)
            this.Hide();
        }

        private void pnlForgotPassword_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnLoginInterfaceLoginButton_Click(object sender, EventArgs e)
        {
            string username = txtLoginUsername.Text;
            string password = txtLoginPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please Enter Username or Password", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string cs = @"Data source=MSI ;   Initial Catalog =Mufaddal_Traders_db ;     Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(cs))
            {
                try
                {
                    conn.Open();

                    string sql = "SELECT COUNT(1) FROM Users WHERE UserName = @username AND Password = @password";
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    int usercount = Convert.ToInt32(cmd.ExecuteScalar());
                    if (usercount > 0)
                    {
                        MessageBox.Show("Successfully Loged In.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Invalid Username or Password", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR !!" + ex.Message);
                }
            }
        }

        private void btnCreateAccountinterfaceLoginButton_Click(object sender, EventArgs e)
        {
            string username = txtCAUsername.Text;
            string password = txtCRPassword.Text;
            string confpassword = txtCRConfirmPassword.Text;
            string email = txtCREmail.Text;
            string tele = txtCRTel.Text;
            string usertype = cbCRUserType.SelectedItem?.ToString();

            //basic validation
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confpassword)
                || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(tele) || string.IsNullOrEmpty(usertype))
            {
                MessageBox.Show("Prodct Enter data", "Failes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confpassword)
            {
                MessageBox.Show("Please enter the same password in both fields.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string cs = @"Data source=MSI ;   Initial Catalog =Mufaddal_Traders_db ;     Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(cs))
            {
                //Exception Handeling
                try
                {
                    conn.Open();

                    //sql statement
                    string sql = "INSERT INTO Users (UserName, Password, UserEmail, UserTelephone, UserType) VALUES (@username, @password, @email," +
                                 " @tele, @usertype)";
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue(" @password", password);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@tele", tele);
                    cmd.Parameters.AddWithValue("@usertype", usertype);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Successfully Created an Account.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        pnlLoginInterface1.Visible = true;
                        pnlCreateAcc.Visible = false;

                    }
                    else
                    {
                        MessageBox.Show("Error in Creating an Account", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR !!" + ex.Message);
                }
            }





        }
    }
}
