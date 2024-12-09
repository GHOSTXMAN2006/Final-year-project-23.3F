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

        public frmAddUpdateItems(int id = -1)
        {
            InitializeComponent();
            itemId = id; // Store the item ID passed to the form
        }

        private void frmAddUpdateItems_Load(object sender, EventArgs e)
        {
            if (itemId != -1)
            {
                LoadItemDetails(itemId);
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
                cmd.Parameters.AddWithValue("@ID", txtID.Text);
                cmd.Parameters.AddWithValue("@Name", txtName.Text);
                cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
                cmd.Parameters.AddWithValue("@Price", txtPrice.Text);
                cmd.Parameters.AddWithValue("@MFDate", dtpMFDate.Value);
                cmd.Parameters.AddWithValue("@EXPDate", dtpEXPDate.Value);

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
                    cmd.Parameters.AddWithValue("@Image", DBNull.Value);
                }

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Item saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
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


    }
}
