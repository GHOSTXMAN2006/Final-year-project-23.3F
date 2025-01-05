using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmUserProfile : Form
    {
        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private string connectionString = DatabaseConfig.ConnectionString;

        public frmUserProfile()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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

        private void btnMenu_Click(object sender, EventArgs e)
        {
            frmITadminMenu menu = new frmITadminMenu();
            menu.Show();
            this.Close();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            frmHome frmHome = new frmHome();
            frmHome.Show();
            this.Close();
        }

        private void btnAccount_Click(object sender, EventArgs e)
        {

        }

        private void LoadUserIDs()
        {
            string query = "SELECT UserID FROM Users";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        cmbUserID.Items.Add(reader["UserID"].ToString()); // Add UserID to the ComboBox
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        }

        private void frmUserProfile_Load(object sender, EventArgs e)
        {
            LoadUserIDs();
        }

        private void cmbUserID_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedUserID = cmbUserID.SelectedItem.ToString();

            string query = "SELECT UserName, UserEmail, UserAddress, UserTelephone, Password, Description, ProfilePicture FROM Users WHERE UserID = @UserID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserID", selectedUserID);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Populate the text fields with user data
                        txtUsername.Text = reader["UserName"].ToString();
                        txtEmail.Text = reader["UserEmail"].ToString();
                        txtAddress.Text = reader["UserAddress"].ToString();
                        txtTelephone.Text = reader["UserTelephone"].ToString();
                        txtPassword.Text = reader["Password"].ToString();
                        txtBio.Text = reader["Description"].ToString();

                        // Handle profile picture
                        if (reader["ProfilePicture"] != DBNull.Value)
                        {
                            // Assuming ProfilePicture is stored as VARBINARY
                            byte[] imageBytes = (byte[])reader["ProfilePicture"];
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                btnProfilePic.Image = Image.FromStream(ms);
                            }
                        }


                    }
                    else
                    {
                        MessageBox.Show("User data not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
        }

    }
}
