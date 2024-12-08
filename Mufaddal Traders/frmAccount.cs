using System;
using System.Drawing;
using System.IO;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Runtime.InteropServices;

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

            // Save changes to the database
            SaveUserDetails();
        }

        private void btnProfilePic_Click(object sender, EventArgs e)
        {
            // Only allow profile picture update when in edit mode
            if (txtUsername.Enabled)
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
        }

        // Method to set the fields as read-only or editable
        private void SetFieldsReadOnly(bool readOnly)
        {
            txtUsername.ReadOnly = readOnly;
            txtEmail.ReadOnly = readOnly;
            txtTelephone.ReadOnly = readOnly;
            txtAddress.ReadOnly = readOnly;
            txtBio.ReadOnly = readOnly;
            txtPassword.ReadOnly = readOnly;
        }

        // Method to save user details to the database
        private void SaveUserDetails()
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
                    string sql = "UPDATE Users SET UserEmail = @userEmail, UserTelephone = @userTelephone, UserAddress = @userAddress, Description = @description, ProfilePicture = @profilePicture WHERE UserName = @username";
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@userEmail", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@userTelephone", txtTelephone.Text);
                    cmd.Parameters.AddWithValue("@userAddress", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@description", txtBio.Text);
                    cmd.Parameters.AddWithValue("@profilePicture", (object)profilePictureBytes ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text);

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

        // Reset the fields to their initial state
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

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtBio_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
