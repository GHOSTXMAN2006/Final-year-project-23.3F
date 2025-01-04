using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmDeliveryOrder : Form
    {
        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private readonly string connectionString = DatabaseConfig.ConnectionString;

        public frmDeliveryOrder()
        {
            InitializeComponent();
        }

        // Form Load
        private void frmDeliveryOrder_Load(object sender, EventArgs e)
        {
            LoadDeliveryOrders(); // Load data on form load
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
            frmShippingManagerMenu shippingManagerMenu = new frmShippingManagerMenu();

            shippingManagerMenu.Show();

            this.Hide();
        }

        private void btnAccount_Click(object sender, EventArgs e)
        {
            frmAccount accountForm = new frmAccount();

            accountForm.Show();

            this.Hide();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            frmDashboard dashboardForm = new frmDashboard();

            dashboardForm.Show();

            this.Hide();
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {

        }

        private void btnMenu_Click(object sender, EventArgs e)
        {

        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            frmHome homeForm = new frmHome();

            homeForm.Show();

            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear the session or global variables
            frmLogin.userType = string.Empty;
            frmLogin.userName = string.Empty;
            frmLogin.userPassword = string.Empty;
            frmLogin.userEmail = string.Empty;
            frmLogin.userTelephone = string.Empty;
            frmLogin.userAddress = string.Empty;
            frmLogin.userDescription = string.Empty;
            frmLogin.profilePicture = null; // Clear profile picture if any

            // Optionally, you can also clear other session variables if needed

            // Close the current form (Dashboard)
            this.Close();

            // Show the login form again
            frmLogin loginForm = new frmLogin();
            loginForm.Show();
        }

        private void LoadDeliveryOrders()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                    SELECT 
                        DeliveryOrderID,
                        CustomerOrderID,
                        CustomerName,
                        Details,
                        DeliveryAmount,
                        TotalAmount,
                        DeliveryDate,
                        WarehouseID
                    FROM tblDeliveryOrder";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable deliveryOrdersTable = new DataTable();
                    adapter.Fill(deliveryOrdersTable);

                    // Bind data to DataGridView
                    dgvDisplay.DataSource = deliveryOrdersTable;

                    FormatDataGridView(); // Apply formatting
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading delivery orders: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridView()
        {
            dgvDisplay.RowTemplate.Height = 40;
            foreach (DataGridViewRow row in dgvDisplay.Rows)
            {
                row.Height = 40;
            }

            dgvDisplay.DefaultCellStyle.Font = new Font("Arial", 12);
            dgvDisplay.DefaultCellStyle.BackColor = Color.White;

            dgvDisplay.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
            dgvDisplay.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSeaGreen;
            dgvDisplay.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvDisplay.EnableHeadersVisualStyles = false;

            dgvDisplay.Columns["DeliveryOrderID"].Width = (int)(dgvDisplay.Width * 0.14);
            dgvDisplay.Columns["CustomerOrderID"].Width = (int)(dgvDisplay.Width * 0.14);
            dgvDisplay.Columns["CustomerName"].Width = (int)(dgvDisplay.Width * 0.18);
            dgvDisplay.Columns["Details"].Width = (int)(dgvDisplay.Width * 0.20);
            dgvDisplay.Columns["DeliveryAmount"].Width = (int)(dgvDisplay.Width * 0.14);
            dgvDisplay.Columns["TotalAmount"].Width = (int)(dgvDisplay.Width * 0.10);
            dgvDisplay.Columns["DeliveryDate"].Width = (int)(dgvDisplay.Width * 0.12);
            dgvDisplay.Columns["WarehouseID"].Width = (int)(dgvDisplay.Width * 0.12);
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchValue = txtSearch.Text.Trim();
                if (string.IsNullOrEmpty(searchValue))
                {
                    LoadDeliveryOrders(); // Load all if search is empty
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = @"
                        SELECT 
                            DeliveryOrderID,
                            CustomerOrderID,
                            CustomerName,
                            Details,
                            DeliveryAmount,
                            TotalAmount,
                            DeliveryDate,
                            WarehouseID
                        FROM tblDeliveryOrder
                        WHERE CAST(DeliveryOrderID AS NVARCHAR) LIKE @SearchValue";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@SearchValue", $"%{searchValue}%");
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable searchResult = new DataTable();
                        adapter.Fill(searchResult);

                        dgvDisplay.DataSource = searchResult;

                        FormatDataGridView(); // Apply formatting after search

                        if (searchResult.Rows.Count == 0)
                        {
                            MessageBox.Show("No records found for the given search criteria.", "No Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during search: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadDeliveryOrders(); // Reload data
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDisplay.SelectedRows.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show(
                    "Are you sure you want to delete the selected delivery order?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (dialogResult == DialogResult.Yes)
                {
                    int deliveryOrderID = Convert.ToInt32(dgvDisplay.SelectedRows[0].Cells["DeliveryOrderID"].Value);
                    int customerOrderID = Convert.ToInt32(dgvDisplay.SelectedRows[0].Cells["CustomerOrderID"].Value);
                    int warehouseID = Convert.ToInt32(dgvDisplay.SelectedRows[0].Cells["WarehouseID"].Value);
                    int itemID = GetDeliveryOrderItemID(deliveryOrderID);  // New method to get ItemID
                    int deliveryQty = GetDeliveryOrderQuantity(deliveryOrderID);  // Retrieve the delivery quantity

                    Console.WriteLine($"DEBUG: Deleting DeliveryOrderID: {deliveryOrderID}");
                    Console.WriteLine($"DEBUG: Associated CustomerOrderID: {customerOrderID}");
                    Console.WriteLine($"DEBUG: Associated WarehouseID: {warehouseID}");
                    Console.WriteLine($"DEBUG: Associated ItemID: {itemID}");
                    Console.WriteLine($"DEBUG: Quantity to add back: {deliveryQty}");

                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            SqlTransaction transaction = conn.BeginTransaction();

                            try
                            {
                                // Delete the delivery order
                                string deleteQuery = @"
                        DELETE FROM tblDeliveryOrder WHERE DeliveryOrderID = @DeliveryOrderID";

                                SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn, transaction);
                                deleteCmd.Parameters.AddWithValue("@DeliveryOrderID", deliveryOrderID);
                                int rowsDeleted = deleteCmd.ExecuteNonQuery();
                                Console.WriteLine($"DEBUG: Rows affected by delivery order delete: {rowsDeleted}");

                                // Update stock balance (add back the quantity)
                                string updateStockQuery = @"
                        UPDATE tblStockBalance 
                        SET ItemQty = ItemQty + @Quantity 
                        WHERE WarehouseID = @WarehouseID AND ItemID = @ItemID";

                                SqlCommand updateStockCmd = new SqlCommand(updateStockQuery, conn, transaction);
                                updateStockCmd.Parameters.AddWithValue("@Quantity", deliveryQty);
                                updateStockCmd.Parameters.AddWithValue("@WarehouseID", warehouseID);
                                updateStockCmd.Parameters.AddWithValue("@ItemID", itemID);  // Added ItemID filter
                                int rowsStockUpdated = updateStockCmd.ExecuteNonQuery();
                                Console.WriteLine($"DEBUG: Rows affected by stock update: {rowsStockUpdated}");

                                if (rowsStockUpdated == 0)
                                {
                                    Console.WriteLine("DEBUG WARNING: Stock balance update failed. No rows were affected.");
                                }

                                // Update customer order status
                                string updateStatusQuery = @"
                        UPDATE tblCustomerOrders 
                        SET Status = 'N' 
                        WHERE CustomerOrderID = @CustomerOrderID";

                                SqlCommand updateStatusCmd = new SqlCommand(updateStatusQuery, conn, transaction);
                                updateStatusCmd.Parameters.AddWithValue("@CustomerOrderID", customerOrderID);
                                int rowsStatusUpdated = updateStatusCmd.ExecuteNonQuery();
                                Console.WriteLine($"DEBUG: Rows affected by customer order status update: {rowsStatusUpdated}");

                                transaction.Commit();
                                Console.WriteLine("DEBUG: Transaction committed successfully.");

                                MessageBox.Show("Delivery order deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadDeliveryOrders(); // Reload data grid view
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                Console.WriteLine($"DEBUG ERROR: Transaction rolled back due to: {ex.Message}");
                                MessageBox.Show($"Error deleting delivery order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"DEBUG ERROR: Database connection error: {ex.Message}");
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a delivery order to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private int GetDeliveryOrderQuantity(int deliveryOrderID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                SELECT CO.ItemQty 
                FROM tblDeliveryOrder DO
                INNER JOIN tblCustomerOrders CO ON DO.CustomerOrderID = CO.CustomerOrderID
                WHERE DO.DeliveryOrderID = @DeliveryOrderID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@DeliveryOrderID", deliveryOrderID);
                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        int qty = Convert.ToInt32(result);
                        Console.WriteLine($"DEBUG: Retrieved ItemQty for DeliveryOrderID {deliveryOrderID}: {qty}");
                        return qty;
                    }
                    else
                    {
                        Console.WriteLine($"DEBUG WARNING: No quantity found for DeliveryOrderID {deliveryOrderID}");
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG ERROR: Failed to retrieve ItemQty for DeliveryOrderID {deliveryOrderID}: {ex.Message}");
                return 0;
            }
        }

        private int GetDeliveryOrderItemID(int deliveryOrderID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
            SELECT CO.ItemID 
            FROM tblDeliveryOrder DO
            INNER JOIN tblCustomerOrders CO ON DO.CustomerOrderID = CO.CustomerOrderID
            WHERE DO.DeliveryOrderID = @DeliveryOrderID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@DeliveryOrderID", deliveryOrderID);
                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        int itemID = Convert.ToInt32(result);
                        Console.WriteLine($"DEBUG: Retrieved ItemID for DeliveryOrderID {deliveryOrderID}: {itemID}");
                        return itemID;
                    }
                    else
                    {
                        Console.WriteLine($"DEBUG WARNING: No ItemID found for DeliveryOrderID {deliveryOrderID}");
                        return 0;  // Default to 0 if not found
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG ERROR: Failed to retrieve ItemID for DeliveryOrderID {deliveryOrderID}: {ex.Message}");
                return 0;
            }
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            frmAddDeliveryOrder addDeliveryOrder = new frmAddDeliveryOrder();
            addDeliveryOrder.Show();
        }
    }
}
