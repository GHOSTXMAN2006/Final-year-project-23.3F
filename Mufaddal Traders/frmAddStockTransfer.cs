using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmAddStockTransfer : Form
    {
        // DLL imports to allow dragging the form
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        // Connection string to connect to the database
        private string connectionString = @"Data Source=DESKTOP-O0Q3714\SQLEXPRESS;Initial Catalog=Mufaddal_Traders_db;Integrated Security=True";

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

        private void frmAddStockTransfer_Load(object sender, EventArgs e)
        {
            LoadItems();
            LoadWarehouses();
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
                    cmbItemID.DisplayMember = "Item_Name";  // Display the Item Name
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
                    SqlDataAdapter da = new SqlDataAdapter("SELECT StoreID, Warehouse_Name FROM Warehouse", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbStartingLocation.DisplayMember = "Warehouse_Name";
                    cmbStartingLocation.ValueMember = "StoreID";
                    cmbStartingLocation.DataSource = dt;

                    cmbEndingLocation.DisplayMember = "Warehouse_Name";
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
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtName.Text = reader["Item_Name"].ToString();
                    }
                    conn.Close();
                }
            }
        }

        // Save the Stock Transfer and close the form
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int itemId = (int)cmbItemID.SelectedValue;
                int startingLocation = (int)cmbStartingLocation.SelectedValue;
                int endingLocation = (int)cmbEndingLocation.SelectedValue;
                int transferQty = int.Parse(txtQty.Text);  // Assuming txtQty is where user enters quantity

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
                ReloadDataGrid();  // Refresh DataGrid in frmStorekeeperStockTransfer
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving stock transfer: " + ex.Message);
            }
        }

        // Reload the DataGrid in frmStorekeeperStockTransfer
        private void ReloadDataGrid()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Stock_Transfer", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                frmStorekeeperStockTransfer.dgvST.DataSource = dt;  // Refreshing the dgvST in frmStorekeeperStockTransfer
            }
        }
    }
}
