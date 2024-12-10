using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmAddUpdateWarehouse : Form
    {
        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        // Connection string for SQL Server
        private string connectionString = @"Data source=DESKTOP-O0Q3714\SQLEXPRESS ; Initial Catalog=Mufaddal_Traders_db ; Integrated Security=True";

        public frmAddUpdateWarehouse()
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

        private void picHeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Keep this event as it is..
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // Check if Enter key is pressed
            {
                string searchText = txtSearch.Text.Trim();
                if (!string.IsNullOrEmpty(searchText))
                {
                    SearchAndLoadWarehouse(searchText); // Call the search and load method
                    txtSearch.Clear(); // Clear the search textbox after search
                }
            }
        }

        private void SearchAndLoadWarehouse(string searchText)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Prepare query for StoreID (int) or Store_Name (string)
                string query = @"
            SELECT TOP 1 StoreID, Store_Name, Store_Location 
            FROM Warehouse 
            WHERE StoreID = @StoreID OR Store_Name LIKE @StoreName";

                SqlCommand cmd = new SqlCommand(query, conn);

                // Check if the search text is numeric (StoreID)
                if (int.TryParse(searchText, out int storeId))
                {
                    // If the search text is numeric, search for StoreID
                    cmd.Parameters.AddWithValue("@StoreID", storeId);  // Pass as integer for StoreID
                    cmd.Parameters.AddWithValue("@StoreName", DBNull.Value);  // Pass DBNull for Store_Name parameter
                }
                else
                {
                    // If it's a name search, use the LIKE pattern
                    cmd.Parameters.AddWithValue("@StoreID", DBNull.Value);  // Pass DBNull for StoreID parameter
                    cmd.Parameters.AddWithValue("@StoreName", "%" + searchText + "%");  // Use LIKE for Store_Name with wildcards
                }

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Populate fields with the warehouse details
                        txtID.Text = reader["StoreID"].ToString();
                        txtName.Text = reader["Store_Name"].ToString();
                        txtLocation.Text = reader["Store_Location"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("No matching warehouse found for the given ID or Name.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ClearFields(); // Clear fields if no records found
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading warehouse details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Get next StoreID if needed for manual management
        private int GetNextStoreID()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ISNULL(MAX(StoreID), 0) + 1 FROM Warehouse"; // Get next StoreID
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    return (int)cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching next StoreID: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1; // Return an invalid ID if there's an error
                }
            }
        }

        // Save Button Click Event
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Keep this event as it is..
        }

        // Update Button Click Event
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text) || string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                MessageBox.Show("Please fill in all fields before updating.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtID.Text, out int storeId))
            {
                MessageBox.Show("StoreID must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = "UPDATE Warehouse SET Store_Name = @Store_Name, Store_Location = @Store_Location WHERE StoreID = @StoreID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@StoreID", storeId);
                        cmd.Parameters.AddWithValue("@Store_Name", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Store_Location", txtLocation.Text.Trim());
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Record updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                            int nextStoreID = GetNextStoreID();
                            txtID.Text = nextStoreID.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Update failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Clear Fields Method
        private void ClearFields()
        {
            txtName.Clear();
            txtLocation.Clear();
        }

        private void frmAddUpdateWarehouse_Load(object sender, EventArgs e)
        {
            // Auto-generate the next StoreID and load it into the text box
            int nextStoreID = GetNextStoreID();
            if (nextStoreID != -1)
            {
                txtID.Text = nextStoreID.ToString();
            }
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                MessageBox.Show("Please fill in all fields before saving.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = "INSERT INTO Warehouse (Store_Name, Store_Location) VALUES (@Store_Name, @Store_Location)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Store_Name", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Store_Location", txtLocation.Text.Trim());
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Record saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                            int nextStoreID = GetNextStoreID();
                            txtID.Text = nextStoreID.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Save failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // Clear all fields except for the ID when clearing
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtLocation.Clear();
            int nextStoreID = GetNextStoreID();
            txtID.Text = nextStoreID.ToString(); // Reset ID to the next value
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
