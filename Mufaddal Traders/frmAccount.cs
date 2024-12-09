using System;
using System.Drawing;
using System.IO;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Mufaddal_Traders
{
    public partial class frmAccount : Form
    {
        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private bool isEditMode = false;  // Flag to track if we are in Edit Mode

        public frmAccount()
        {
            InitializeComponent();
        }

        private void frmAccount_Load(object sender, EventArgs e)
        {
            // Initialize fields with user information from the global variables
            txtUsername.Text = frmLogin.userName;
            txtEmail.Text = frmLogin.userEmail;
            txtTelephone.Text = frmLogin.userTelephone;
            txtAddress.Text = frmLogin.userAddress;
            txtBio.Text = frmLogin.userDescription;
            txtPassword.Text = frmLogin.userPassword;

            // Set labels to reflect current username and email
            lblUsername.Text = frmLogin.userName;
            lblEmail.Text = frmLogin.userEmail;
            lblWelcomeName.Text = frmLogin.userName;

            // Load profile picture if available
            if (frmLogin.profilePicture != null)
            {
                using (MemoryStream ms = new MemoryStream(frmLogin.profilePicture))
                {
                    btnProfilePic.Image = Image.FromStream(ms);
                }
            }
            else
            {
                // Default profile picture if no image is set
                btnProfilePic.Image = global::Mufaddal_Traders.Properties.Resources._69159871;
            }

            // Set fields to read-only initially
            SetFieldsReadOnly(true);
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

        // When Edit button is clicked, make fields editable
        private void btnEdit_Click(object sender, EventArgs e)
        {
            SetFieldsReadOnly(false);
            isEditMode = true; // Set to edit mode

            // Allow profile picture change when in edit mode
            btnProfilePic.Enabled = true; // Now profile picture can be changed
            btnEdit.FillColor = Color.FromArgb(200, 100, 30);  // Darker color when in edit mode
        }

        // When Save button is clicked, save the changes
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtTelephone.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if the password is changed
            if (!string.IsNullOrEmpty(txtPassword.Text))
            {
                // Prompt user for the old password
                string oldPassword = Microsoft.VisualBasic.Interaction.InputBox("Please enter your old password", "Old Password", "");

                if (ValidateOldPassword(oldPassword))
                {
                    // If the old password is correct, hash the new password
                    string newHashedPassword = HashPassword(txtPassword.Text);

                    // Proceed with saving the user details
                    SaveUserDetails(newHashedPassword);
                }
                else
                {
                    MessageBox.Show("Old password is incorrect. Please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                // No password change, save other details
                SaveUserDetails(null);
            }
        }

        private void btnProfilePic_Click(object sender, EventArgs e)
        {
            // Only allow profile picture update when in edit mode
            if (isEditMode)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Title = "Select a Profile Picture",
                    Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Load the selected image into the PictureBox
                    btnProfilePic.Image = Image.FromFile(openFileDialog.FileName);
                }
            }
            else
            {
                MessageBox.Show("You must click Edit to update your profile picture.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Method to set the fields as read-only or editable
        private void SetFieldsReadOnly(bool readOnly)
        {
            txtUsername.ReadOnly = readOnly;
            txtEmail.ReadOnly = readOnly;
            txtTelephone.ReadOnly = readOnly;
            txtAddress.ReadOnly = readOnly;
            txtBio.ReadOnly = readOnly;
            txtPassword.ReadOnly = readOnly;  // This is where the password field is also controlled

            /*// Disable profile picture upload if not in edit mode
            btnProfilePic.Enabled = !readOnly;  // Keep button enabled visually, but block the action*/
        }

        // Method to save user details to the database
        private void SaveUserDetails(string newHashedPassword)
        {
            byte[] profilePictureBytes = null;
            if (btnProfilePic.Image != global::Mufaddal_Traders.Properties.Resources._69159871)
            {
                // Convert the profile picture to byte array
                using (MemoryStream ms = new MemoryStream())
                {
                    btnProfilePic.Image.Save(ms, btnProfilePic.Image.RawFormat);
                    profilePictureBytes = ms.ToArray();
                }
            }

            string cs = @"Data source=DESKTOP-O0Q3714\SQLEXPRESS ; Initial Catalog=Mufaddal_Traders_db ; Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(cs))
            {
                try
                {
                    conn.Open();
                    string sql = "UPDATE Users SET UserEmail = @userEmail, UserTelephone = @userTelephone, UserAddress = @userAddress, Description = @description, ProfilePicture = @profilePicture";

                    // Only update password if it's changed
                    if (!string.IsNullOrEmpty(newHashedPassword))
                    {
                        sql += ", Password = @password";
                    }

                    sql += " WHERE UserName = @username";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@userEmail", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@userTelephone", txtTelephone.Text);
                    cmd.Parameters.AddWithValue("@userAddress", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@description", txtBio.Text);
                    cmd.Parameters.AddWithValue("@profilePicture", (object)profilePictureBytes ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text);

                    // If password is updated, include it in the parameters
                    if (!string.IsNullOrEmpty(newHashedPassword))
                    {
                        cmd.Parameters.AddWithValue("@password", newHashedPassword);
                    }

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Profile updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // After save, reload the data and make fields read-only again
                        frmLogin.userEmail = txtEmail.Text;
                        frmLogin.userTelephone = txtTelephone.Text;
                        frmLogin.userAddress = txtAddress.Text;
                        frmLogin.userDescription = txtBio.Text;

                        // Load profile picture again
                        frmLogin.profilePicture = profilePictureBytes;

                        // Update labels with new information
                        lblUsername.Text = txtUsername.Text;
                        lblEmail.Text = txtEmail.Text;
                        lblWelcomeName.Text = txtUsername.Text;

                        SetFieldsReadOnly(true);

                        // Reset the Edit button color back to default
                        btnEdit.FillColor = Color.FromArgb(244, 124, 44);  // Original color when not editable
                        isEditMode = false; // Exit edit mode
                    }
                    else
                    {
                        MessageBox.Show("Failed to update profile.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Method to validate the old password
        private bool ValidateOldPassword(string oldPassword)
        {
            string hashedOldPassword = HashPassword(oldPassword);

            string cs = @"Data source=DESKTOP-O0Q3714\SQLEXPRESS ; Initial Catalog=Mufaddal_Traders_db ; Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(cs))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT Password FROM Users WHERE UserName = @username";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", frmLogin.userName);

                    string currentPassword = cmd.ExecuteScalar()?.ToString();

                    if (currentPassword == hashedOldPassword)
                    {
                        return true;  // Old password is correct
                    }
                    else
                    {
                        return false;  // Incorrect old password
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        // This method hashes a password using SHA-256
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
                return builder.ToString(); // Returns the hashed password
            }
        }


        private void btnReset_Click(object sender, EventArgs e)
        {
            // Reset the fields to the original values
            txtUsername.Text = frmLogin.userName;
            txtEmail.Text = frmLogin.userEmail;
            txtTelephone.Text = frmLogin.userTelephone;
            txtAddress.Text = frmLogin.userAddress;
            txtBio.Text = frmLogin.userDescription;

            // Reset the profile picture
            if (frmLogin.profilePicture != null)
            {
                using (MemoryStream ms = new MemoryStream(frmLogin.profilePicture))
                {
                    btnProfilePic.Image = Image.FromStream(ms);
                }
            }
            else
            {
                // Default profile picture if no image is set
                btnProfilePic.Image = global::Mufaddal_Traders.Properties.Resources._69159871;
            }

            // Set fields to read-only
            SetFieldsReadOnly(true);

            // Reset the Edit button color back to default
            btnEdit.FillColor = Color.FromArgb(244, 124, 44);  // Original color when not editable
            isEditMode = false; // Exit edit mode
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            // Create an instance of frmHome
            frmHome homeForm = new frmHome();

            // Show the frmHome
            homeForm.Show();

            // Close the current form (frmAccount)
            this.Hide();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            // Check the userType to open the corresponding menu form
            switch (frmLogin.userType)  // Accessing userType from frmLogin
            {
                case "Storekeeper":
                    new frmStorekeeperMenu().Show();
                    break;
                case "ShippingManager":
                    new frmShippingManagerMenu().Show();
                    break;
                case "Accountant":
                    new frmAccountantsMenu().Show();
                    break;
                case "Marketing and Sales Department":
                    new frmMSD_Menu().Show();
                    break;
                default:
                    MessageBox.Show("Invalid User Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

            // Hide the current dashboard form (optional, to switch to the menu form)
            this.Hide();
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
    }
}
