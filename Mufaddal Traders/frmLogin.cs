using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmLogin : Form
    {
        // Declare userType as a global variable
        public static string userType;

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
            pnlCreateAcc.Visible = false;
            pnlNewPassword.Visible = false;
            pnlForgotPassword.Visible = false;

            // Configure label4 (Heading: Welcome to MTSMS)
            label4.Parent = pictureBox2;
            label4.Text = "Welcome to MTSMS,";
            label4.Font = new Font("Microsoft Sans Serif", 40, FontStyle.Bold);
            label4.ForeColor = Color.Black;
            label4.BackColor = Color.Transparent;
            label4.AutoSize = true;
            label4.Location = new Point(30, 100);

            // Configure label8 (Subtext: Description of MTSMS)
            label8.Parent = pictureBox2;
            label8.Text = "Your ultimate solution for seamless stock management. \r\nOptimize your inventory with precision and efficiency, empowering \r\nyour business to thrive. \r\n\r\nExperience the simplicity of innovation in managing your stock \r\nlike never before.";
            label8.Font = new Font("Microsoft Sans Serif", 16, FontStyle.Regular);
            label8.ForeColor = Color.Black;
            label8.BackColor = Color.Transparent;
            label8.AutoSize = true;
            label8.Location = new Point(35, 185);
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

        private void btnHome_Click(object sender, EventArgs e)
        {
            frmHome homeForm = new frmHome();
            homeForm.Show();
            this.Hide();
        }

        private void btnCreateAccountinterfaceLoginButton_Click(object sender, EventArgs e)
        {
            string username = txtCAUsername.Text;
            string password = txtCRPassword.Text;
            string confpassword = txtCRConfirmPassword.Text;
            string email = txtCREmail.Text;
            string tele = txtCRTel.Text;
            string usertype = cbCRUserType.SelectedItem?.ToString();

            // Basic validation for empty fields
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confpassword) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(tele) || string.IsNullOrEmpty(usertype))
            {
                MessageBox.Show("Please fill all fields.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if passwords match
            if (password != confpassword)
            {
                MessageBox.Show("Passwords do not match.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate email format
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Invalid email format.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate telephone number (must be 10 digits)
            if (!IsValidTelephone(tele))
            {
                MessageBox.Show("Invalid telephone number. It must be 10 digits.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string hashedPassword = HashPassword(password);
            string cs = @"Data source=DESKTOP-O0Q3714\SQLEXPRESS ; Initial Catalog=Mufaddal_Traders_db ; Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(cs))
            {
                try
                {
                    conn.Open();
                    string sql = "INSERT INTO Users (UserName, Password, UserEmail, UserTelephone, UserType) VALUES (@username, @password, @email, @tele, @usertype)";
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@tele", tele);
                    cmd.Parameters.AddWithValue("@usertype", usertype);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Account created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Clear the fields after successful creation
                        txtCAUsername.Clear();
                        txtCRPassword.Clear();
                        txtCRConfirmPassword.Clear();
                        txtCREmail.Clear();
                        txtCRTel.Clear();
                        cbCRUserType.SelectedIndex = -1; // Deselect any selected user type

                        pnlLoginInterface1.Visible = true;
                        pnlCreateAcc.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Failed to create account.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnLoginInterfaceLoginButton_Click(object sender, EventArgs e)
        {
            string username = txtLoginUsername.Text;
            string password = txtLoginPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please Enter Username and Password", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string hashedPassword = HashPassword(password);
            string cs = @"Data source=DESKTOP-O0Q3714\SQLEXPRESS ; Initial Catalog=Mufaddal_Traders_db ; Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(cs))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT UserType FROM Users WHERE UserName = @username AND Password = @password";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        userType = result.ToString(); // Set the global variable for userType
                        MessageBox.Show("Successfully Logged In.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Start the "session" (simulated here by setting the userType)
                        switch (userType)
                        {
                            case "Storekeeper":
                                new frmStorekeeperMenu().Show();
                                break;
                            case "ShippingManager":
                                new frmShippingManagerMenu().Show();
                                break;
                            case "Accountant":
                                new frmAccountantsDashboard().Show();
                                break;
                            case "Marketing and Sales Department":
                                new frmMSD_Dashboard().Show();
                                break;
                            default:
                                MessageBox.Show("Invalid User Type", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;
                        }

                        this.Hide(); // Hide the login form
                    }
                    else
                    {
                        MessageBox.Show("Invalid Username or Password", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Validate email format using a regular expression
        private bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        // Validate telephone number (must be 10 digits)
        private bool IsValidTelephone(string tele)
        {
            var teleRegex = new Regex(@"^\d{10}$");
            return teleRegex.IsMatch(tele);
        }
    }
}
