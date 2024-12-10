using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmAddUpdateSuppliers : Form
    {
        // DLL imports to allow dragging the form
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        // Connection string for SQL Server
        private string connectionString = @"Data Source=LAPTOP-S6UOBFRN\SQLEXPRESS;Initial Catalog=backup2;Integrated Security=True";

        public frmAddUpdateSuppliers()
        {
            InitializeComponent();
        }

        // Close button functionality
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Minimize button functionality
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // Dragging functionality for the header panel
        private void picHeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        // Back button functionality to close the form
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Trigger search on Enter key press in txtSearch field
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // Detect Enter key press
            {
                string searchText = txtSearch.Text.Trim();
                if (!string.IsNullOrEmpty(searchText))
                {
                    SearchAndLoadSupplier(searchText); // Call the search and load method
                    txtSearch.Clear(); // Clear the search textbox after search
                }
            }
        }

        // Save button functionality
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                MessageBox.Show("Please fill in all fields before saving.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SqlParameter[] parameters = GetSqlParameters();
            ExecuteQuery("INSERT INTO tblManageSuppliers (Name, Telephone, Email, Address, Description) " +
                         "VALUES (@Name, @Telephone, @Email, @Address, @Description)",
                         parameters);
        }

        // Update button functionality
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs() || !int.TryParse(txtID.Text, out int supplierId))
            {
                MessageBox.Show("Please fill in all fields and ensure SupplierID is valid.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the parameters for the SQL query
            SqlParameter[] parameters = GetSqlParameters();

            // Add the SupplierID parameter for the update query
            // The SupplierID must be the last parameter for the WHERE clause
            Array.Resize(ref parameters, parameters.Length + 1);
            parameters[parameters.Length - 1] = new SqlParameter("@SupplierID", supplierId);

            ExecuteQuery("UPDATE tblManageSuppliers SET Name = @Name, Telephone = @Telephone, Email = @Email, Address = @Address, Description = @Description WHERE SupplierID = @SupplierID", parameters);
        }


        // Clear fields button functionality
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
            txtID.Text = GetNextSupplierID().ToString(); // Auto-generate SupplierID again
        }

        // Clear all text fields
        private void ClearFields()
        {
            txtName.Clear();
            txtTel.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            txtDescription.Clear();
        }

        // Validate input fields
        private bool ValidateInputs()
        {
            return !(string.IsNullOrWhiteSpace(txtName.Text) ||
                     string.IsNullOrWhiteSpace(txtTel.Text) ||
                     string.IsNullOrWhiteSpace(txtEmail.Text) ||
                     string.IsNullOrWhiteSpace(txtAddress.Text) ||
                     string.IsNullOrWhiteSpace(txtDescription.Text));
        }

        // Get SQL parameters from text fields
        private SqlParameter[] GetSqlParameters()
        {
            return new SqlParameter[] {
                new SqlParameter("@Name", txtName.Text.Trim()),
                new SqlParameter("@Telephone", txtTel.Text.Trim()),
                new SqlParameter("@Email", txtEmail.Text.Trim()),
                new SqlParameter("@Address", txtAddress.Text.Trim()),
                new SqlParameter("@Description", txtDescription.Text.Trim())
            };
        }


    // Execute SQL query with provided parameters
    private void ExecuteQuery(string query, SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parameters);
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Operation successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                            txtID.Text = GetNextSupplierID().ToString(); // Auto-generate SupplierID again

                        }
                        else
                        {
                            MessageBox.Show("Operation failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Operation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frmAddUpdateSuppliers_Load(object sender, EventArgs e)
        {
            // Auto-generate SupplierID when form loads
            txtID.Text = GetNextSupplierID().ToString();
        }

        // Search and load supplier data based on SupplierID or Name
        private void SearchAndLoadSupplier(string searchText)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT TOP 1 SupplierID, Name, Telephone, Email, Address, Description 
                    FROM tblManageSuppliers 
                    WHERE SupplierID = @SupplierID OR Name LIKE @Name";

                SqlCommand cmd = new SqlCommand(query, conn);

                if (int.TryParse(searchText, out int supplierId))
                {
                    cmd.Parameters.AddWithValue("@SupplierID", supplierId);
                    cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SupplierID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Name", "%" + searchText + "%");
                }

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        txtID.Text = reader["SupplierID"].ToString();
                        txtName.Text = reader["Name"].ToString();
                        txtTel.Text = reader["Telephone"].ToString();
                        txtEmail.Text = reader["Email"].ToString();
                        txtAddress.Text = reader["Address"].ToString();
                        txtDescription.Text = reader["Description"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("No matching supplier found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ClearFields();
                        txtID.Text = GetNextSupplierID().ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading supplier details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Not used but kept as required
        }

        // Get the next available SupplierID
        private int GetNextSupplierID()
        {
            int nextID = 1;  // Default starting point if there are no suppliers yet
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT MAX(SupplierID) FROM tblManageSuppliers";
                SqlCommand cmd = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    nextID = result != DBNull.Value ? Convert.ToInt32(result) + 1 : 1; // Increment the max ID by 1
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching next SupplierID: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return nextID;
        }
    }
}
