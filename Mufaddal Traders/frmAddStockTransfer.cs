using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmAddStockTransfer : Form
    {
        // Event to notify when stock is transferred
        public event EventHandler StockTransferred;

        private string connectionString = @"Data source=DESKTOP-O0Q3714\SQLEXPRESS ; Initial Catalog=Mufaddal_Traders_db ; Integrated Security=True";

        public frmAddStockTransfer()
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

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddStockTransfer_Load(object sender, EventArgs e)
        {
            LoadItems();
            LoadWarehouses();
            LoadTransferID();  // Call the method to load the next Transfer ID
        }

        // Method to load the next available Stock Transfer ID
        private void LoadTransferID()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    // SQL query to get the next available ID (assuming ID starts from 1)
                    string query = "SELECT ISNULL(MAX(ST_ID), 0) + 1 FROM Stock_Transfer";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    conn.Close();

                    // Display the next available ST_ID in txtTransferID
                    txtTransferID.Text = result.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading transfer ID: " + ex.Message);
                }
            }
        }
        // Load Items into cmbItemID
        private void LoadItems()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT ItemID, Item_Name FROM Items", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cmbItemID.DisplayMember = "ItemID";  // Display the Item ID
                    cmbItemID.ValueMember = "ItemID";  // Store the ItemID
                    cmbItemID.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading items: " + ex.Message);
                }
            }
        }

        // Load Warehouses into cmbStartingLocation and cmbEndingLocation
        private void LoadWarehouses()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT StoreID, Store_Name FROM Warehouse", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbStartingLocation.DisplayMember = "Store_Name";
                    cmbStartingLocation.ValueMember = "StoreID";
                    cmbStartingLocation.DataSource = dt;

                    cmbEndingLocation.DisplayMember = "Store_Name";
                    cmbEndingLocation.ValueMember = "StoreID";
                    cmbEndingLocation.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading warehouses: " + ex.Message);
                }
            }
        }

        // When an ItemID is selected, load the item name into txtName
        private void cmbItemID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbItemID.SelectedValue != null)
            {
                int itemId = (int)cmbItemID.SelectedValue;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Item_Name FROM Items WHERE ItemID = @ItemID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ItemID", itemId);

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            // Set the Item_Name from the database into the txtName textbox
                            txtName.Text = reader["Item_Name"].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading item name: " + ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }


        // Save the Stock Transfer and close the form
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                int itemId = (int)cmbItemID.SelectedValue;
                int startingLocation = (int)cmbStartingLocation.SelectedValue;
                int endingLocation = (int)cmbEndingLocation.SelectedValue;

                // Validate quantity input
                if (!int.TryParse(txtQty.Text, out int transferQty) || transferQty <= 0)
                {
                    MessageBox.Show("Please enter a valid quantity.");
                    return;
                }

                // Use today's date for the stock transfer
                DateTime transferDate = DateTime.Today;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Stock_Transfer (ST_Date, ST_Qty, StoreID, ItemID) " +
                                   "VALUES (@ST_Date, @ST_Qty, @StoreID, @ItemID)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ST_Date", transferDate);
                    cmd.Parameters.AddWithValue("@ST_Qty", transferQty);
                    cmd.Parameters.AddWithValue("@StoreID", startingLocation);  // Assuming starting location is used here
                    cmd.Parameters.AddWithValue("@ItemID", itemId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                MessageBox.Show("Stock Transfer Saved Successfully!");
                this.Close();  // Close the form after saving

                // Trigger the event to notify other form
                StockTransferred?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving stock transfer: " + ex.Message);
            }
        }
    }
}
