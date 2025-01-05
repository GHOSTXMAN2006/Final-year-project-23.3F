using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmShipments : Form
    {
        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private readonly string connectionString = DatabaseConfig.ConnectionString;

        public frmShipments()
        {
            InitializeComponent();
        }

        // Form Load
        private void frmShipments_Load(object sender, EventArgs e)
        {
            LoadShipments(); // Load data on form load
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
            // Placeholder for future history-related functionality
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            // Placeholder for future menu-related functionality
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            frmHome homeForm = new frmHome();
            homeForm.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            frmLogin.userType = string.Empty;
            frmLogin.userName = string.Empty;
            frmLogin.userPassword = string.Empty;
            frmLogin.userEmail = string.Empty;
            frmLogin.userTelephone = string.Empty;
            frmLogin.userAddress = string.Empty;
            frmLogin.userDescription = string.Empty;
            frmLogin.profilePicture = null;
            this.Close();
            frmLogin loginForm = new frmLogin();
            loginForm.Show();
        }

        private void LoadShipments()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                    SELECT 
                        ShipmentID,
                        CustomerOrderID,
                        CustomerName,
                        Details,
                        ShipmentAmount,
                        TotalAmount,
                        DeliveryDate,
                        WarehouseID
                    FROM tblShipments";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable shipmentsTable = new DataTable();
                    adapter.Fill(shipmentsTable);
                    dgvDisplay.DataSource = shipmentsTable;

                    FormatDataGridView(); // Apply formatting
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading shipments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            dgvDisplay.Columns["ShipmentID"].Width = (int)(dgvDisplay.Width * 0.14);
            dgvDisplay.Columns["CustomerOrderID"].Width = (int)(dgvDisplay.Width * 0.14);
            dgvDisplay.Columns["CustomerName"].Width = (int)(dgvDisplay.Width * 0.18);
            dgvDisplay.Columns["Details"].Width = (int)(dgvDisplay.Width * 0.20);
            dgvDisplay.Columns["ShipmentAmount"].Width = (int)(dgvDisplay.Width * 0.14);
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
                    LoadShipments(); // Load all if search is empty
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = @"
                        SELECT 
                            ShipmentID,
                            CustomerOrderID,
                            CustomerName,
                            Details,
                            ShipmentAmount,
                            TotalAmount,
                            DeliveryDate,
                            WarehouseID
                        FROM tblShipments
                        WHERE CAST(ShipmentID AS NVARCHAR) LIKE @SearchValue";

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
            LoadShipments(); // Reload data
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDisplay.SelectedRows.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show(
                    "Are you sure you want to delete the selected shipment?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (dialogResult == DialogResult.Yes)
                {
                    int shipmentID = Convert.ToInt32(dgvDisplay.SelectedRows[0].Cells["ShipmentID"].Value);
                    int customerOrderID = Convert.ToInt32(dgvDisplay.SelectedRows[0].Cells["CustomerOrderID"].Value);
                    int warehouseID = Convert.ToInt32(dgvDisplay.SelectedRows[0].Cells["WarehouseID"].Value);
                    int itemID = GetShipmentItemID(shipmentID);
                    int shipmentQty = GetShipmentQuantity(shipmentID);

                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            SqlTransaction transaction = conn.BeginTransaction();

                            try
                            {
                                string deleteQuery = "DELETE FROM tblShipments WHERE ShipmentID = @ShipmentID";
                                SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn, transaction);
                                deleteCmd.Parameters.AddWithValue("@ShipmentID", shipmentID);
                                int rowsDeleted = deleteCmd.ExecuteNonQuery();

                                string updateStockQuery = "UPDATE tblStockBalance SET ItemQty = ItemQty + @Quantity WHERE WarehouseID = @WarehouseID AND ItemID = @ItemID";
                                SqlCommand updateStockCmd = new SqlCommand(updateStockQuery, conn, transaction);
                                updateStockCmd.Parameters.AddWithValue("@Quantity", shipmentQty);
                                updateStockCmd.Parameters.AddWithValue("@WarehouseID", warehouseID);
                                updateStockCmd.Parameters.AddWithValue("@ItemID", itemID);
                                int rowsStockUpdated = updateStockCmd.ExecuteNonQuery();

                                string updateStatusQuery = "UPDATE tblCustomerOrders SET Status = 'N' WHERE CustomerOrderID = @CustomerOrderID";
                                SqlCommand updateStatusCmd = new SqlCommand(updateStatusQuery, conn, transaction);
                                updateStatusCmd.Parameters.AddWithValue("@CustomerOrderID", customerOrderID);
                                int rowsStatusUpdated = updateStatusCmd.ExecuteNonQuery();

                                transaction.Commit();
                                MessageBox.Show("Shipment deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadShipments(); // Reload data
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                MessageBox.Show($"Error deleting shipment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a shipment to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private int GetShipmentQuantity(int shipmentID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT CO.ItemQty FROM tblShipments S INNER JOIN tblCustomerOrders CO ON S.CustomerOrderID = CO.CustomerOrderID WHERE S.ShipmentID = @ShipmentID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ShipmentID", shipmentID);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        private int GetShipmentItemID(int shipmentID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT CO.ItemID FROM tblShipments S INNER JOIN tblCustomerOrders CO ON S.CustomerOrderID = CO.CustomerOrderID WHERE S.ShipmentID = @ShipmentID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ShipmentID", shipmentID);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddShipments addShipment = new frmAddShipments();
            addShipment.Show();
        }
    }
}
