using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics; // <-- For Debug.WriteLine
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmAddUpdateUsers : Form
    {
        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private string connectionString = DatabaseConfig.ConnectionString;

        public frmAddUpdateUsers()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // Method to hash the password
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Debug: Begin method execution
            Console.WriteLine("DEBUG: btnSave_Click triggered at " + DateTime.Now);

            string username = txtUserName.Text;
            string password = txtPassword.Text;
            string confirmPassword = txtConfPass.Text;
            string userType = cmbUserType.SelectedItem?.ToString() ?? "User"; // Default to "User" if not selected
            string email = "-"; // Default value for email

            Console.WriteLine($"DEBUG: Input values - Username: '{username}', Password Length: {password.Length}, Confirm Password Length: {confirmPassword.Length}, UserType: '{userType}'");

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                Console.WriteLine("DEBUG: One or more fields are empty. Exiting method.");
                MessageBox.Show("Please fill in all fields.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                Console.WriteLine("DEBUG: Passwords do not match. Exiting method.");
                MessageBox.Show("Passwords do not match.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Console.WriteLine("DEBUG: Passwords match. Proceeding with hashing.");
            string hashedPassword = HashPassword(password);
            Console.WriteLine($"DEBUG: Hashed Password: {hashedPassword}");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    Console.WriteLine("DEBUG: Opening SQL connection...");
                    conn.Open();
                    Console.WriteLine("DEBUG: Connection opened successfully.");

                    transaction = conn.BeginTransaction();
                    Console.WriteLine("DEBUG: Transaction started.");

                    string insertQuery = "INSERT INTO Users (UserName, Password, UserEmail, UserType) VALUES (@username, @hashedPassword, @userEmail, @userType)";
                    SqlCommand cmd = new SqlCommand(insertQuery, conn, transaction);

                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@hashedPassword", hashedPassword);
                    cmd.Parameters.AddWithValue("@userEmail", email);
                    cmd.Parameters.AddWithValue("@userType", userType);

                    Console.WriteLine("DEBUG: SQL Command created and parameters added.");

                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine($"DEBUG: ExecuteNonQuery completed. Rows affected: {rowsAffected}");

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("DEBUG: Account created successfully. Committing transaction...");
                        transaction.Commit();
                        MessageBox.Show("Account created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Clearing form fields
                        Console.WriteLine("DEBUG: Clearing input fields.");
                        txtUserName.Clear();
                        txtPassword.Clear();
                        txtConfPass.Clear();
                        cmbUserType.SelectedIndex = -1;
                    }
                    else
                    {
                        Console.WriteLine("DEBUG: No rows affected. Rolling back transaction.");
                        transaction.Rollback();
                        MessageBox.Show("Failed to create account.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"DEBUG: Exception occurred: {ex.Message}. Rolling back transaction.");
                    if (transaction != null)
                    {
                        transaction.Rollback();
                        Console.WriteLine("DEBUG: Transaction rolled back.");
                    }
                    MessageBox.Show("ERROR: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                        Console.WriteLine("DEBUG: SQL connection closed.");
                    }
                }
            }

            Console.WriteLine("DEBUG: btnSave_Click execution finished.");
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text;
            string password = txtPassword.Text;
            string confirmPassword = txtConfPass.Text;
            string userType = cmbUserType.SelectedItem?.ToString() ?? "User"; // Default to "User" if nothing is selected

            // Validation checks
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill in all fields.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Console.WriteLine("DEBUG: Passwords match. Proceeding with update.");

            // Hash the new password
            string newHashedPassword = HashPassword(password);
            Console.WriteLine($"DEBUG: New Hashed Password: {newHashedPassword}");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    conn.Open();
                    Console.WriteLine("DEBUG: Connection opened.");
                    transaction = conn.BeginTransaction();

                    // Fetch current data for comparison
                    string getUserDataQuery = "SELECT Password, UserType FROM Users WHERE UserName = @Username";
                    SqlCommand getUserDataCmd = new SqlCommand(getUserDataQuery, conn, transaction);
                    getUserDataCmd.Parameters.AddWithValue("@Username", username);

                    SqlDataReader reader = getUserDataCmd.ExecuteReader();

                    if (!reader.Read())
                    {
                        MessageBox.Show("User does not exist. Cannot update details.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        reader.Close();
                        transaction.Rollback();
                        Console.WriteLine("DEBUG: User not found. Transaction rolled back.");
                        return;
                    }

                    // Get existing data
                    string oldHashedPassword = reader["Password"].ToString();
                    string oldUserType = reader["UserType"].ToString();
                    reader.Close();

                    // Check if new data is the same as old data
                    if (newHashedPassword == oldHashedPassword && userType == oldUserType)
                    {
                        MessageBox.Show("No changes detected. Please modify the fields before updating.", "No Changes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        transaction.Rollback();
                        Console.WriteLine("DEBUG: No changes detected. Transaction rolled back.");
                        return;
                    }

                    // Update query
                    string updateQuery = @"
                UPDATE Users 
                SET Password = @Password, UserType = @UserType
                WHERE UserName = @Username";

                    SqlCommand cmd = new SqlCommand(updateQuery, conn, transaction);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", newHashedPassword);
                    cmd.Parameters.AddWithValue("@UserType", userType);

                    Console.WriteLine("DEBUG: SQL Command created and parameters added.");

                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine($"DEBUG: ExecuteNonQuery completed. Rows affected: {rowsAffected}");

                    if (rowsAffected > 0)
                    {
                        transaction.Commit();
                        Console.WriteLine("DEBUG: User details updated successfully. Transaction committed.");
                        MessageBox.Show("User details updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Clear the form fields
                        txtUserName.Clear();
                        txtPassword.Clear();
                        txtConfPass.Clear();
                        cmbUserType.SelectedIndex = -1;
                    }
                    else
                    {
                        transaction.Rollback();
                        Console.WriteLine("DEBUG: No rows affected. Transaction rolled back.");
                        MessageBox.Show("Failed to update user details.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"DEBUG: Exception occurred: {ex.Message}");
                    if (transaction != null)
                    {
                        transaction.Rollback();
                        Console.WriteLine("DEBUG: Transaction rolled back due to exception.");
                    }
                    MessageBox.Show("ERROR: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                        Console.WriteLine("DEBUG: SQL connection closed.");
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Clear text fields
            txtUserName.Clear();
            txtPassword.Clear();
            txtConfPass.Clear();

            // Reset the ComboBox
            cmbUserType.SelectedIndex = -1; // Deselect any selected item in the UserType ComboBox

            // Log for debugging
            Console.WriteLine("DEBUG: Form fields cleared.");
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void frmAddUpdateUsers_Load(object sender, EventArgs e)
        {

        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // Trigger search on Enter key press
            {
                int userId;
                if (!int.TryParse(txtSearch.Text, out userId))
                {
                    MessageBox.Show("Please enter a valid numeric User ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        Console.WriteLine($"DEBUG: Searching for User ID {userId} (excluding admin)...");

                        conn.Open();
                        string searchQuery = "SELECT * FROM Users WHERE UserID = @UserID AND UserName != 'admin'";
                        SqlCommand cmd = new SqlCommand(searchQuery, conn);
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            // Load user details into form fields
                            txtUserName.Text = reader["UserName"].ToString();
                            txtPassword.Text = reader["Password"].ToString(); // Display the password directly
                            txtConfPass.Text = reader["Password"].ToString(); // Also set confirm password for convenience
                            cmbUserType.SelectedItem = reader["UserType"].ToString();

                            Console.WriteLine("DEBUG: User details loaded successfully.");
                        }
                        else
                        {
                            MessageBox.Show("User not found or user is admin.", "No Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"DEBUG: Exception occurred: {ex.Message}");
                        MessageBox.Show("ERROR: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                            Console.WriteLine("DEBUG: SQL connection closed.");
                        }
                    }
                }
            }
        }

    }
}
