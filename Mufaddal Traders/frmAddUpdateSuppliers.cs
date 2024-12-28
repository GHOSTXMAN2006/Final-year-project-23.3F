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
        private string connectionString = DatabaseConfig.ConnectionString;

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
        // Save button functionality
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                MessageBox.Show("Please fill in all fields before saving.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the next available SupplierID
            int supplierID = GetNextSupplierID();

            // Add SupplierID to the parameters
            SqlParameter[] parameters = GetSqlParameters();

            // Resize the array and add SupplierID
            SqlParameter[] updatedParameters = new SqlParameter[parameters.Length + 1];
            for (int i = 0; i < parameters.Length; i++)
            {
                updatedParameters[i] = parameters[i];
            }
            updatedParameters[parameters.Length] = new SqlParameter("@SupplierID", supplierID);

            // Save to the database with IDENTITY_INSERT enabled
            ExecuteQuery(
                "INSERT INTO tblManageSuppliers (SupplierID, Name, Telephone, Email, Address, Description) " +
                "VALUES (@SupplierID, @Name, @Telephone, @Email, @Address, @Description)",
                updatedParameters,
                true // Enable IDENTITY_INSERT
            );

            // Update the SupplierID textbox
            txtID.Text = GetNextSupplierID().ToString();
        }



        // Update button functionality
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs() || !int.TryParse(txtID.Text, out int supplierId))
            {
                MessageBox.Show("Please fill in all fields and ensure SupplierID is valid.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get SQL parameters
            SqlParameter[] parameters = GetSqlParameters();

            // Resize the array and add SupplierID
            SqlParameter[] updatedParameters = new SqlParameter[parameters.Length + 1];
            for (int i = 0; i < parameters.Length; i++)
            {
                updatedParameters[i] = parameters[i];
            }
            updatedParameters[parameters.Length] = new SqlParameter("@SupplierID", supplierId);

            // Update the database
            ExecuteQuery("UPDATE tblManageSuppliers SET Name = @Name, Telephone = @Telephone, Email = @Email, Address = @Address, Description = @Description " +
                         "WHERE SupplierID = @SupplierID",
                         updatedParameters);
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
                        using (SqlCommand cmd = new SqlCommand("SET IDENTITY_INSERT tblManageSuppliers ON;", conn))
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
                            txtID.Text = GetNextSupplierID().ToString(); // Auto-generate SupplierID again
                        }
                        else
                        {
                            MessageBox.Show("Operation failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    if (enableIdentityInsert)
                    {
                        // Disable IDENTITY_INSERT
                        using (SqlCommand cmd = new SqlCommand("SET IDENTITY_INSERT tblManageSuppliers OFF;", conn))
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
                string query = @"
            SELECT TOP 1 Number
            FROM (SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Number
                  FROM master.dbo.spt_values) AS Numbers
            WHERE Number NOT IN (SELECT SupplierID FROM tblManageSuppliers)
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
                    MessageBox.Show("Error fetching next SupplierID: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return nextID;
        }
    }
}
