using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmAddUpdateCustomers : Form
    {
        // DLL imports to allow dragging the form
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        // Connection string for SQL Server
        private string connectionString = DatabaseConfig.ConnectionString;

        public frmAddUpdateCustomers()
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
                    SearchAndLoadCustomer(searchText); // Call the search and load method
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

            // Get the next available CustomerID
            int customerID = GetNextCustomerID();

            // Add CustomerID to the parameters
            SqlParameter[] parameters = GetSqlParameters();

            // Resize the array and add CustomerID
            SqlParameter[] updatedParameters = new SqlParameter[parameters.Length + 1];
            for (int i = 0; i < parameters.Length; i++)
            {
                updatedParameters[i] = parameters[i];
            }
            updatedParameters[parameters.Length] = new SqlParameter("@CustomerID", customerID);

            // Save to the database with IDENTITY_INSERT enabled
            ExecuteQuery(
                "INSERT INTO tblCustomers (CustomerID, Name, Telephone, Email, Address, Description) " +
                "VALUES (@CustomerID, @Name, @Telephone, @Email, @Address, @Description)",
                updatedParameters,
                true // Enable IDENTITY_INSERT
            );

            // Update the CustomerID textbox
            txtID.Text = GetNextCustomerID().ToString();
        }

        // Update button functionality
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs() || !int.TryParse(txtID.Text, out int customerId))
            {
                MessageBox.Show("Please fill in all fields and ensure CustomerID is valid.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get SQL parameters
            SqlParameter[] parameters = GetSqlParameters();

            // Resize the array and add CustomerID
            SqlParameter[] updatedParameters = new SqlParameter[parameters.Length + 1];
            for (int i = 0; i < parameters.Length; i++)
            {
                updatedParameters[i] = parameters[i];
            }
            updatedParameters[parameters.Length] = new SqlParameter("@CustomerID", customerId);

            // Update the database
            ExecuteQuery("UPDATE tblCustomers SET Name = @Name, Telephone = @Telephone, Email = @Email, Address = @Address, Description = @Description " +
                         "WHERE CustomerID = @CustomerID",
                         updatedParameters);
        }

        // Clear fields button functionality
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
            txtID.Text = GetNextCustomerID().ToString(); // Auto-generate CustomerID again
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
        private void ExecuteQuery(string query, SqlParameter[] parameters, bool enableIdentityInsert = false)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    if (enableIdentityInsert)
                    {
                        // Enable IDENTITY_INSERT
                        using (SqlCommand cmd = new SqlCommand("SET IDENTITY_INSERT tblCustomers ON;", conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parameters);
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Operation successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                            txtID.Text = GetNextCustomerID().ToString(); // Auto-generate CustomerID again
                        }
                        else
                        {
                            MessageBox.Show("Operation failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    if (enableIdentityInsert)
                    {
                        // Disable IDENTITY_INSERT
                        using (SqlCommand cmd = new SqlCommand("SET IDENTITY_INSERT tblCustomers OFF;", conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Operation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frmAddUpdateCustomers_Load(object sender, EventArgs e)
        {
            // Auto-generate CustomerID when form loads
            txtID.Text = GetNextCustomerID().ToString();
        }

        // Search and load customer data based on CustomerID or Name
        private void SearchAndLoadCustomer(string searchText)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT TOP 1 CustomerID, Name, Telephone, Email, Address, Description 
                    FROM tblCustomers 
                    WHERE CustomerID = @CustomerID OR Name LIKE @Name";

                SqlCommand cmd = new SqlCommand(query, conn);

                if (int.TryParse(searchText, out int customerId))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);
                    cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Name", "%" + searchText + "%");
                }

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        txtID.Text = reader["CustomerID"].ToString();
                        txtName.Text = reader["Name"].ToString();
                        txtTel.Text = reader["Telephone"].ToString();
                        txtEmail.Text = reader["Email"].ToString();
                        txtAddress.Text = reader["Address"].ToString();
                        txtDescription.Text = reader["Description"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("No matching customer found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ClearFields();
                        txtID.Text = GetNextCustomerID().ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading customer details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Get the next available CustomerID
        private int GetNextCustomerID()
        {
            int nextID = 1;  // Default starting point if there are no customers yet
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT TOP 1 Number
            FROM (SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Number
                  FROM master.dbo.spt_values) AS Numbers
            WHERE Number NOT IN (SELECT CustomerID FROM tblCustomers)
            ORDER BY Number";

                SqlCommand cmd = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    nextID = result != DBNull.Value ? Convert.ToInt32(result) : 1; // If no gaps, start at 1
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching next CustomerID: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return nextID;
        }
    }
}
