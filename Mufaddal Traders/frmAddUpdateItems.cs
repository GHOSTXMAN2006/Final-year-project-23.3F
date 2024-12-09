using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmAddUpdateItems : Form
    {

        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);


        private string connectionString = @"Data source=DESKTOP-O0Q3714\SQLEXPRESS ; Initial Catalog=Mufaddal_Traders_db ; Integrated Security=True";
        private int itemId;

        private frmItems parentForm;

        public frmAddUpdateItems(frmItems parent, int id = -1)
        {
            InitializeComponent();
            itemId = id; // Store the item ID passed to the form
            parentForm = parent; // Store reference to the parent form
        }


        private void frmAddUpdateItems_Load(object sender, EventArgs e)
        {
            if (itemId != -1)
            {
                LoadItemDetails(itemId);
            }
            else
            {
                int nextId = GetNextItemId();
                if (nextId != -1)
                {
                    txtID.Text = nextId.ToString(); // Set the next available ID
                    txtID.ReadOnly = true;          // Make ID field read-only for new items
                }
            }
        }



        private int GetNextItemId()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ISNULL(MAX(ItemID), 0) + 1 FROM Items"; // Get next ID
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    return (int)cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching next ItemID: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1; // Return an invalid ID if there's an error
                }
            }
        }

        private void LoadItemDetails(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Items WHERE ItemID = @ItemID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ItemID", id);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        txtID.Text = reader["ItemID"].ToString();
                        txtName.Text = reader["Item_Name"].ToString();
                        txtPrice.Text = reader["Item_Price"].ToString();
                        txtDescription.Text = reader["Item_Description"].ToString();
                        dtpMFDate.Value = Convert.ToDateTime(reader["Manufacture_Date"]);
                        dtpEXPDate.Value = Convert.ToDateTime(reader["Expiry_Date"]);

                        if (reader["Item_Image"] != DBNull.Value)
                        {
                            byte[] imageBytes = (byte[])reader["Item_Image"];
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                btnUpload.Image = Image.FromStream(ms);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Item details could not be loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading item details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query;

                // Check if itemId is -1 for a new item or a valid ID for an existing item
                if (itemId == -1) // Insert new item
                {
                    query = @"INSERT INTO Items (Item_Name, Item_Description, Item_Price, Manufacture_Date, Expiry_Date, Item_Image) 
                      VALUES (@Name, @Description, @Price, @MFDate, @EXPDate, @Image)";
                }
                else // Update existing item
                {
                    query = @"UPDATE Items 
                      SET Item_Name = @Name, Item_Description = @Description, Item_Price = @Price, 
                          Manufacture_Date = @MFDate, Expiry_Date = @EXPDate, Item_Image = @Image 
                      WHERE ItemID = @ID";
                }

                SqlCommand cmd = new SqlCommand(query, conn);

                // Add parameters for both Insert and Update
                cmd.Parameters.AddWithValue("@Name", txtName.Text);
                cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
                cmd.Parameters.AddWithValue("@Price", txtPrice.Text);
                cmd.Parameters.AddWithValue("@MFDate", dtpMFDate.Value);
                cmd.Parameters.AddWithValue("@EXPDate", dtpEXPDate.Value);

                // If there is an uploaded image, convert it to a byte array
                if (btnUpload.Image != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        btnUpload.Image.Save(ms, btnUpload.Image.RawFormat);
                        cmd.Parameters.AddWithValue("@Image", ms.ToArray());
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Image", DBNull.Value); // Handle null image
                }

                // Add the ItemID parameter if updating
                if (itemId != -1)
                {
                    cmd.Parameters.AddWithValue("@ID", itemId); // Use the itemId to identify the item for updating
                }

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery(); // Execute the query

                    MessageBox.Show("Item saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reload items in the parent form
                    parentForm?.LoadItems(); // Assuming parent form has a LoadItems method to refresh the list

                    this.Close(); // Close the current form after saving
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving item: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select an Item Image",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                btnUpload.Image = Image.FromFile(openFileDialog.FileName);
            }
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

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // Check if Enter key is pressed
            {
                string searchText = txtSearch.Text.Trim();
                if (!string.IsNullOrEmpty(searchText))
                {
                    SearchAndLoadItem(searchText); // Call the search and load method
                }
            }
        }



        private void SearchAndLoadItem(string searchText)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Search by ItemID or Item_Name
                string query = @"
            SELECT TOP 1 * 
            FROM Items 
            WHERE ItemID = @SearchText OR Item_Name LIKE @SearchText";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SearchText", searchText);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Populate fields with the item's details
                        itemId = Convert.ToInt32(reader["ItemID"]); // Set itemId to the found ItemID
                        txtID.Text = reader["ItemID"].ToString();
                        txtName.Text = reader["Item_Name"].ToString();
                        txtPrice.Text = reader["Item_Price"].ToString();
                        txtDescription.Text = reader["Item_Description"].ToString();
                        dtpMFDate.Value = Convert.ToDateTime(reader["Manufacture_Date"]);
                        dtpEXPDate.Value = Convert.ToDateTime(reader["Expiry_Date"]);

                        if (reader["Item_Image"] != DBNull.Value)
                        {
                            byte[] imageBytes = (byte[])reader["Item_Image"];
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                btnUpload.Image = Image.FromStream(ms);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No matching item found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading item details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


    }
}
