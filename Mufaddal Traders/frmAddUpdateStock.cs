using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Mufaddal_Traders
{
    public partial class frmAddUpdateStock : Form
    {

        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private string connectionString = @"Data source=MSI ; Initial Catalog=Mufaddal_Traders_db ; Integrated Security=True";


        public void LoadComboBoxes()
        {
           
            string sql = "SELECT WarehouseID FROM Warehouse";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        cmbWarehouseID.Items.Add((dr["WarehouseID"].ToString()));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error !!" + ex.Message);
                }
            }
        }

        private void PopulateItemIDs()
        {
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT ItemID FROM Items", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cmbItemID.Items.Add(reader["ItemID"].ToString());
                }

                reader.Close();
            }
        }

       
        public frmAddUpdateStock()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtItemName.Text) || string.IsNullOrWhiteSpace(txtItemPrice.Text) || string.IsNullOrEmpty(txtItemQuantity.Text))
            {
                MessageBox.Show("Please fill in all fields before saving.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbItemID == null || cmbWarehouseID == null)
            {
                MessageBox.Show("Please select a Item and warehouse.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO Stock (StockID, ItemID, Item_Name, Item_Qty, Item_Price, WarehouseID, Warehouse_Name)" +
                         "VALUES (@stockID, @itemID, @itemName, @itemQty, @itemPrice, warehouseID, warehousename)";
                try
                {


                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@stockID", StockID.Text);
                    cmd.Parameters.AddWithValue("@itemID", cmbItemID.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@itemName", txtItemName.Text);
                    cmd.Parameters.AddWithValue("@itemPrice", txtItemPrice.Text);
                    cmd.Parameters.AddWithValue("@itemQty", txtItemQuantity.Text);
                    cmd.Parameters.AddWithValue("@warehouseID", cmbWarehouseID.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@warehousename", txtItemQuantity.Text);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Product added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                    }
                    else
                    {
                        MessageBox.Show("Failed to add product.", "Failes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }catch(Exception ex)
                {
                    MessageBox.Show("ERROR !!" +ex.Message);
                }

            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void frmAddUpdateStock_Load(object sender, EventArgs e)
        {
            LoadComboBoxes();
            PopulateItemIDs();
            AutoGenerateID();       // Call the method to auto-generate StockID
        }


        private void AutoGenerateID()
        {
            string connectionString = "Server=MSI;Database=Mufaddal_Traders_db;Trusted_Connection=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Query to fetch the maximum StockID
                    string query = "SELECT ISNULL(MAX(CAST(StockID AS INT)), 0) + 1 AS NextStockID FROM Stock";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Execute the query and get the result
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        StockID.Text = result.ToString(); // Set the next StockID in the TextBox
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }



        private void cmbWarehouseID_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string selectedID = cmbWarehouseID.SelectedItem.ToString();

            string sql = "SELECT Warehouse_Name FROM Warehouse WHERE WarehouseID = @id";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", selectedID);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtWarehouseName.Text = reader["WarehouseName"].ToString();
                }

                reader.Close();
            }
        }

        private void cmbItemID_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedID = cmbItemID.SelectedItem.ToString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Item_Name FROM Items WHERE ItemID = @id", conn);
                cmd.Parameters.AddWithValue("@id", selectedID);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // Correct the column name to match the database schema
                    txtItemName.Text = reader["Item_Name"].ToString();
                }

                reader.Close();
            }
        }


        private void btnClear_Click(object sender, EventArgs e)
        {

        }
    }
}
