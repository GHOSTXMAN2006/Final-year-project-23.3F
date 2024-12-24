using System;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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

        private string connectionString = @"Data source=DESKTOP-O0Q3714\SQLEXPRESS ; Initial Catalog=Mufaddal_Traders_db ; Integrated Security=True";

        public frmAddUpdateStock()
        {
            InitializeComponent();
        }


        private void frmAddUpdateStock_Load(object sender, EventArgs e)
        {
            LoadComboBoxes();
            PopulateItemIDs();
            AutoGenerateID();

            // Bind events
            txtSearch.KeyDown += txtSearch_KeyDown;
            cmbWarehouseID.SelectedIndexChanged += cmbWarehouseID_SelectedIndexChanged;
            cmbItemID.SelectedIndexChanged += cmbItemID_SelectedIndexChanged; // Bind item selection event
        }


        private void LoadComboBoxes()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT StoreID FROM Warehouse", conn);
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        cmbWarehouseID.Items.Add(dr["StoreID"].ToString());
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading warehouses: " + ex.Message);
                }
            }
        }

        private void PopulateItemIDs()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
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
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading items: " + ex.Message);
                }
            }
        }

        private void AutoGenerateID()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ISNULL(MAX(StockID), 0) + 1 AS NextStockID FROM Stock";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    StockID.Text = cmd.ExecuteScalar()?.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error generating StockID: " + ex.Message);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtItemName.Text) ||
                string.IsNullOrWhiteSpace(txtItemPrice.Text) ||
                string.IsNullOrWhiteSpace(txtItemQuantity.Text) ||
                cmbItemID.SelectedItem == null ||
                cmbWarehouseID.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string sql = @"INSERT INTO Stock (ItemID, Stock_Date, Item_Qty, Item_Price)
                                   VALUES (@itemID, @stockDate, @itemQty, @itemPrice)";
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@itemID", cmbItemID.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@stockDate", DateTime.Now.Date);
                    cmd.Parameters.AddWithValue("@itemQty", int.Parse(txtItemQuantity.Text));
                    cmd.Parameters.AddWithValue("@itemPrice", decimal.Parse(txtItemPrice.Text));

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Stock added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to add stock.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void cmbWarehouseID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbWarehouseID.SelectedItem == null) return;

            string selectedID = cmbWarehouseID.SelectedItem.ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT Store_Name FROM Warehouse WHERE StoreID = @id", conn);
                    cmd.Parameters.AddWithValue("@id", selectedID);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtWarehouseName.Text = reader["Store_Name"].ToString();
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading warehouse details: " + ex.Message);
                }
            }
        }


        private void cmbItemID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbItemID.SelectedItem == null) return;

            string selectedID = cmbItemID.SelectedItem.ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT Item_Name FROM Items WHERE ItemID = @id", conn);
                    cmd.Parameters.AddWithValue("@id", selectedID);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtItemName.Text = reader["Item_Name"].ToString();
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading item details: " + ex.Message);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.cmbItemID.SelectedIndex = -1;
            this.txtItemName.Clear();
            this.txtItemPrice.Clear();
            this.txtItemQuantity.Clear();
            this.cmbWarehouseID.SelectedIndex = -1;
            this.txtWarehouseName.Clear();
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtItemName.Text) ||
                string.IsNullOrWhiteSpace(txtItemPrice.Text) ||
                string.IsNullOrWhiteSpace(txtItemQuantity.Text) ||
                cmbItemID.SelectedItem == null ||
                cmbWarehouseID.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all fields before updating.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Corrected the SQL query to match the database schema
                    string sql = @"UPDATE Stock
                           SET ItemID = @ItemID, Item_Qty = @Item_Qty, Item_Price = @Item_Price
                           WHERE StockID = @StockID";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    // Correctly map the parameters
                    cmd.Parameters.AddWithValue("@StockID", StockID.Text);
                    cmd.Parameters.AddWithValue("@ItemID", cmbItemID.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Item_Qty", int.Parse(txtItemQuantity.Text));
                    cmd.Parameters.AddWithValue("@Item_Price", decimal.Parse(txtItemPrice.Text));

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Stock successfully updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to update stock. Please check the data and try again.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while updating stock: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("SELECT * FROM Stock WHERE StockID = @id", conn);
                        cmd.Parameters.AddWithValue("@id", txtSearch.Text);

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            cmbItemID.SelectedItem = reader["ItemID"].ToString();
                            txtItemQuantity.Text = reader["Item_Qty"].ToString();
                            txtItemPrice.Text = reader["Item_Price"].ToString();
                            cmbWarehouseID.SelectedItem = reader["WarehouseID"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("No record found with the given StockID.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error retrieving stock details: " + ex.Message);
                    }
                }
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
