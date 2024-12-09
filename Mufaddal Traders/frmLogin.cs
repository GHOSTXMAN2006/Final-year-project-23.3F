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
        // Declare global variable of the Logging users
        public static string userType;
        public static string userName;
        public static string userPassword;
        public static string userEmail;
        public static string userTelephone;
        public static string userAddress;
        public static string userDescription;
        public static byte[] profilePicture;


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

            // Validate that both fields are not empty
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please Enter Username and Password", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string hashedPassword = HashPassword(password); // Hash the entered password
            string cs = @"Data source=DESKTOP-O0Q3714\SQLEXPRESS ; Initial Catalog=Mufaddal_Traders_db ; Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(cs))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT UserType, UserName, UserEmail, UserTelephone, UserAddress, Description, ProfilePicture, Password FROM Users WHERE UserName = @username AND Password = @password";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", hashedPassword); // Hash the entered password before comparing

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // Store user information in global variables
                            userType = reader["UserType"].ToString();
                            userName = reader["UserName"].ToString();
                            userEmail = reader["UserEmail"].ToString();
                            userTelephone = reader["UserTelephone"].ToString();
                            userAddress = reader["UserAddress"].ToString();
                            userDescription = reader["Description"].ToString();
                            userPassword = reader["Password"].ToString(); // This will store the hashed password from the DB

                            // Store profile picture as byte array
                            if (reader["ProfilePicture"] != DBNull.Value)
                            {
                                profilePicture = (byte[])reader["ProfilePicture"]; // Store the binary image data
                            }
                            else
                            {
                                profilePicture = null;  // In case the user has no profile picture
                            }
                        }

                        MessageBox.Show("Successfully Logged In.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Open the common dashboard for all user types
                        frmDashboard dashboardForm = new frmDashboard();  // Assuming frmDashboard is your common dashboard
                        dashboardForm.Show();

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

        private void txtFPSave_Click(object sender, EventArgs e)
        {
            string username = txtFPUsername.Text;
            string newPassword = txtFPPassword.Text;
            string confirmPassword = txtFPConfirmPassword.Text;

            // Validate password fields
            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill in both password fields.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate password strength (optional)
            if (!IsValidPassword(newPassword))
            {
                MessageBox.Show("Password must be at least 6 characters long.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if the new password is the same as the old password
            string cs = @"Data source=DESKTOP-O0Q3714\SQLEXPRESS ; Initial Catalog=Mufaddal_Traders_db ; Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(cs))
            {
                try
                {
                    conn.Open();

                    // Fetch the current password from the database
                    string sql = "SELECT Password FROM Users WHERE UserName = @username";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    string currentPassword = cmd.ExecuteScalar()?.ToString();

                    if (currentPassword == null)
                    {
                        MessageBox.Show("Username not found.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Compare the current password with the new password
                    if (currentPassword == HashPassword(newPassword))
                    {
                        MessageBox.Show("The new password cannot be the same as the old password.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Hash the new password
                    string hashedPassword = HashPassword(newPassword);

                    // Update the password in the database
                    string updateSql = "UPDATE Users SET Password = @password WHERE UserName = @username";
                    SqlCommand updateCmd = new SqlCommand(updateSql, conn);
                    updateCmd.Parameters.AddWithValue("@password", hashedPassword);
                    updateCmd.Parameters.AddWithValue("@username", username);

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Password successfully changed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Clear the fields after successful password change
                        txtFPUsername.Clear();
                        txtFPEmail.Clear();
                        txtFPPassword.Clear();
                        txtFPConfirmPassword.Clear();

                        // Optionally, hide the reset panel and show login panel again
                        pnlNewPassword.Visible = false;
                        pnlForgotPassword.Visible = false;
                        pnlLoginInterface1.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("Failed to change the password.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void picCompare_Click(object sender, EventArgs e)
        {
            string username = txtFPUsername.Text;
            string email = txtFPEmail.Text;

            // Validate that both fields are not empty
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please fill both Username and Email fields.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if the username and email match in the database
            string cs = @"Data source=DESKTOP-O0Q3714\SQLEXPRESS ; Initial Catalog=Mufaddal_Traders_db ; Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(cs))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT COUNT(1) FROM Users WHERE UserName = @username AND UserEmail = @email";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@email", email);

                    int userCount = Convert.ToInt32(cmd.ExecuteScalar());
                    if (userCount > 0)
                    {
                        // If user exists, hide picCompare and show pnlNewPassword
                        picCompare.Visible = false;
                        pnlNewPassword.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("Username and Email do not match.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool IsValidPassword(string password)
        {
            // Password should be at least 6 characters long
            return password.Length >= 6;
        }


    }
}
