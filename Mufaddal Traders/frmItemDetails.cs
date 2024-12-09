using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmItemDetails : Form
    {
        private int itemId;

        // Connection string to the database
        private string connectionString = @"Data Source=DESKTOP-O0Q3714\SQLEXPRESS;Initial Catalog=Mufaddal_Traders_db;Integrated Security=True";

        public frmItemDetails(int itemId)
        {
            InitializeComponent();
            this.itemId = itemId;
        }

        // Load item details when the form is loaded
        private void frmItemDetails_Load(object sender, EventArgs e)
        {
            LoadItemDetails(itemId);
        }

        // Method to load item details from the database
        private void LoadItemDetails(int itemId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Item_Name, Item_Description, Item_Price, Manufacture_Date, Expiry_Date, Item_Image FROM Items WHERE ItemID = @itemId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@itemId", itemId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // Set the values of the controls
                            txtID.Text = itemId.ToString();
                            txtName.Text = reader["Item_Name"].ToString();
                            txtDescription.Text = reader["Item_Description"].ToString();
                            txtPrice.Text = reader["Item_Price"].ToString();
                            txtMFDate.Text = reader["Manufacture_Date"].ToString();
                            txtEXPDate.Text = reader["Expiry_Date"].ToString();

                            byte[] itemImageBytes = reader["Item_Image"] as byte[];
                            if (itemImageBytes != null && itemImageBytes.Length > 0)
                            {
                                picItem.Image = ByteArrayToImage(itemImageBytes);
                            }
                            else
                            {
                                picItem.Image = Properties.Resources._3486568; // Default placeholder image
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Item not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading item details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Method to convert byte array to image
        private Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }

        // Handle cancel button click to close the form
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
