using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmAddDeliveryOrder : Form
    {
        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private readonly string connectionString = DatabaseConfig.ConnectionString;
        private int selectedItemQuantity = 0;  // Store selected item quantity

        public frmAddDeliveryOrder()
        {
            InitializeComponent();
        }

        // Close button
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Minimize window
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // Allow window dragging
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate required fields
            if (cmbWarehouseID.SelectedValue == null || string.IsNullOrWhiteSpace(QtyDisplay.Text))
            {
                MessageBox.Show("Please select a valid warehouse with sufficient stock.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int warehouseID = Convert.ToInt32(cmbWarehouseID.SelectedValue);
            int availableQty = Convert.ToInt32(QtyDisplay.Text);
            int orderQty = selectedItemQuantity;
            int selectedItemID = GetSelectedItemID();  // Retrieve the ItemID for this order

            // Debug information
            Console.WriteLine($"DEBUG: Selected WarehouseID: {warehouseID}");
            Console.WriteLine($"DEBUG: Available Quantity in Warehouse: {availableQty}");
            Console.WriteLine($"DEBUG: Quantity required for order: {orderQty}");
            Console.WriteLine($"DEBUG: Selected ItemID: {selectedItemID}");

            if (selectedItemID == 0)
            {
                MessageBox.Show("Invalid ItemID. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (orderQty <= 0)
            {
                MessageBox.Show("Invalid order quantity. Please select a valid order.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (availableQty < orderQty)
            {
                MessageBox.Show($"Insufficient stock! Available: {availableQty}, Required: {orderQty}.", "Stock Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();
                    Console.WriteLine("DEBUG: SQL Connection opened and transaction started.");

                    try
                    {
                        // Insert delivery order
                        string query = @"
                SET IDENTITY_INSERT tblDeliveryOrder ON;
                INSERT INTO tblDeliveryOrder 
                (DeliveryOrderID, CustomerOrderID, CustomerName, Details, DeliveryAmount, TotalAmount, DeliveryDate, WarehouseID)
                VALUES 
                (@DeliveryOrderID, @CustomerOrderID, @CustomerName, @Details, @DeliveryAmount, @TotalAmount, @DeliveryDate, @WarehouseID);
                SET IDENTITY_INSERT tblDeliveryOrder OFF;";

                        SqlCommand cmd = new SqlCommand(query, conn, transaction);
                        cmd.Parameters.AddWithValue("@DeliveryOrderID", Convert.ToInt32(txtDeliveryOrderID.Text));
                        cmd.Parameters.AddWithValue("@CustomerOrderID", Convert.ToInt32(cmbCustomerOrderID.SelectedValue));
                        cmd.Parameters.AddWithValue("@CustomerName", txtCustomerName.Text);
                        cmd.Parameters.AddWithValue("@Details", txtDetails.Text);
                        cmd.Parameters.AddWithValue("@DeliveryAmount", decimal.Parse(txtDeliveryAmount.Text));
                        cmd.Parameters.AddWithValue("@TotalAmount", decimal.Parse(txtTotalAmount.Text.Replace("Rs.", "").Trim()));
                        cmd.Parameters.AddWithValue("@DeliveryDate", dtpDate.Value.Date);
                        cmd.Parameters.AddWithValue("@WarehouseID", warehouseID);

                        Console.WriteLine($"DEBUG: Inserting delivery order with DeliveryOrderID: {txtDeliveryOrderID.Text}");
                        int rowsAffected = cmd.ExecuteNonQuery();
                        Console.WriteLine($"DEBUG: Rows affected by delivery order insert: {rowsAffected}");

                        // Update stock balance
                        string updateStockQuery = "UPDATE tblStockBalance SET ItemQty = ItemQty - @Quantity WHERE WarehouseID = @WarehouseID AND ItemID = @ItemID";
                        SqlCommand updateStockCmd = new SqlCommand(updateStockQuery, conn, transaction);
                        updateStockCmd.Parameters.AddWithValue("@Quantity", orderQty);
                        updateStockCmd.Parameters.AddWithValue("@WarehouseID", warehouseID);
                        updateStockCmd.Parameters.AddWithValue("@ItemID", selectedItemID);

                        Console.WriteLine($"DEBUG: Updating stock balance: Reducing quantity by {orderQty} for ItemID: {selectedItemID} in WarehouseID: {warehouseID}");
                        rowsAffected = updateStockCmd.ExecuteNonQuery();
                        Console.WriteLine($"DEBUG: Rows affected by stock update: {rowsAffected}");

                        if (rowsAffected == 0)
                        {
                            throw new Exception("DEBUG ERROR: Stock balance update failed. No rows were affected.");
                        }

                        // Update customer order status
                        string updateOrderQuery = "UPDATE tblCustomerOrders SET Status = 'Y' WHERE CustomerOrderID = @CustomerOrderID";
                        SqlCommand updateOrderCmd = new SqlCommand(updateOrderQuery, conn, transaction);
                        updateOrderCmd.Parameters.AddWithValue("@CustomerOrderID", Convert.ToInt32(cmbCustomerOrderID.SelectedValue));

                        Console.WriteLine($"DEBUG: Updating customer order status to 'Y' for CustomerOrderID: {cmbCustomerOrderID.SelectedValue}");
                        rowsAffected = updateOrderCmd.ExecuteNonQuery();
                        Console.WriteLine($"DEBUG: Rows affected by customer order update: {rowsAffected}");

                        transaction.Commit();
                        Console.WriteLine("DEBUG: Transaction committed successfully.");

                        MessageBox.Show("Delivery order saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Clear fields and reload customer orders
                        ClearFields();
                        LoadCustomerOrders();  // Reload CustomerOrderID to hide completed orders
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"DEBUG ERROR: Transaction rolled back due to: {ex.Message}");
                        MessageBox.Show($"Error saving delivery order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG ERROR: Database connection error: {ex.Message}");
                MessageBox.Show($"Database connection error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetSelectedItemID()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT ItemID FROM tblCustomerOrders WHERE CustomerOrderID = @CustomerOrderID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CustomerOrderID", Convert.ToInt32(cmbCustomerOrderID.SelectedValue));
                    conn.Open();
                    object result = cmd.ExecuteScalar(); // Get the first column of the first row
                    return result != null ? Convert.ToInt32(result) : 0;  // Return ItemID if found, otherwise 0
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG ERROR: Failed to retrieve ItemID: {ex.Message}");
                MessageBox.Show($"Error retrieving ItemID: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;  // Return 0 if an error occurs
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields(); // Clear all form fields
        }

        private ToolTip quantityToolTip = new ToolTip();

        private void frmAddDeliveryOrder_Load(object sender, EventArgs e)
        {
            LoadDeliveryOrderID(); // Load auto-generated DeliveryOrderID
            LoadCustomerOrders();  // Load CustomerOrderID where status = 'N'

            // Assign tooltip
            quantityToolTip.SetToolTip(QtyDisplay, "Available Quantity in Warehouse");
        }

        private void LoadDeliveryOrderID()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                    SELECT TOP 1 Number
                    FROM (
                        SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Number
                        FROM master.dbo.spt_values
                    ) AS Numbers
                    WHERE Number NOT IN (SELECT DeliveryOrderID FROM tblDeliveryOrder)
                    ORDER BY Number";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    txtDeliveryOrderID.Text = result != null ? result.ToString() : "1";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading DeliveryOrderID: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCustomerOrders()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT CustomerOrderID FROM tblCustomerOrders WHERE Status = 'N' AND LocalOrExport = 'L'";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cmbCustomerOrderID.DisplayMember = "CustomerOrderID";
                    cmbCustomerOrderID.ValueMember = "CustomerOrderID";
                    cmbCustomerOrderID.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Customer Orders: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbCustomerOrderID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCustomerOrderID.SelectedValue != null)
            {
                int customerOrderID = Convert.ToInt32(cmbCustomerOrderID.SelectedValue);
                LoadCustomerName(customerOrderID);
                LoadOrderDetails(customerOrderID);

                // Reset warehouse-related fields
                txtWarehouseName.Clear();
                QtyDisplay.Text = string.Empty;
                cmbWarehouseID.DataSource = null;

                LoadWarehouses(customerOrderID);  // Load warehouses for the selected item.
            }
        }

        private void LoadCustomerName(int customerOrderID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                    SELECT C.Name
                    FROM tblCustomers C
                    INNER JOIN tblCustomerOrders O ON C.CustomerID = O.CustomerID
                    WHERE O.CustomerOrderID = @CustomerOrderID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CustomerOrderID", customerOrderID);
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    txtCustomerName.Text = result != null ? result.ToString() : string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Customer Name: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private decimal baseItemTotal = 0; // New field to store the base item total

        private void LoadOrderDetails(int customerOrderID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
            SELECT I.Item_Name, O.ItemQty, I.Item_Price, (O.ItemQty * I.Item_Price) AS TotalPrice
            FROM tblCustomerOrders O
            INNER JOIN Items I ON O.ItemID = I.ItemID
            WHERE O.CustomerOrderID = @CustomerOrderID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CustomerOrderID", customerOrderID);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string itemName = reader["Item_Name"].ToString();
                        int quantity = Convert.ToInt32(reader["ItemQty"]);
                        decimal price = Convert.ToDecimal(reader["Item_Price"]);
                        baseItemTotal = Convert.ToDecimal(reader["TotalPrice"]);

                        selectedItemQuantity = quantity;  // Store the order quantity
                        txtDetails.Text = $"Item: {itemName}\r\nQuantity: {quantity}\r\nPrice: Rs. {price:F2}";
                        txtTotalAmount.Text = $"Rs. {baseItemTotal:F2}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Order Details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtDeliveryAmount_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtDeliveryAmount.Text, out decimal deliveryAmount))
            {
                decimal finalAmount = baseItemTotal + deliveryAmount; // Recalculate from base total
                txtTotalAmount.Text = $"Rs. {finalAmount:F2}";
            }
            else
            {
                // Reset to base total if the delivery amount is cleared
                txtTotalAmount.Text = $"Rs. {baseItemTotal:F2}";
            }
        }

        private void LoadWarehouses(int customerOrderID)
        {
            // Clear previous values
            cmbWarehouseID.DataSource = null;
            txtWarehouseName.Clear();
            QtyDisplay.Text = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
            SELECT S.WarehouseID, W.Store_Name AS WarehouseName, S.ItemQty
            FROM tblStockBalance S
            INNER JOIN Warehouse W ON S.WarehouseID = W.StoreID
            INNER JOIN tblCustomerOrders O ON O.ItemID = S.ItemID
            WHERE O.CustomerOrderID = @CustomerOrderID AND S.ItemQty > 0";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CustomerOrderID", customerOrderID);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        cmbWarehouseID.DisplayMember = "WarehouseID";
                        cmbWarehouseID.ValueMember = "WarehouseID";
                        cmbWarehouseID.DataSource = dt;
                    }
                    else
                    {
                        MessageBox.Show("No warehouses found with stock for this item.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cmbWarehouseID.DataSource = null;
                        txtWarehouseName.Clear();
                        QtyDisplay.Text = "0";  // Set default to 0 if no warehouses available
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Warehouses: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbWarehouseID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbWarehouseID.SelectedValue != null)
            {
                int warehouseID = Convert.ToInt32(cmbWarehouseID.SelectedValue);

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = @"
                    SELECT W.Store_Name AS WarehouseName, S.ItemQty 
                    FROM tblStockBalance S
                    INNER JOIN Warehouse W ON S.WarehouseID = W.StoreID
                    WHERE S.WarehouseID = @WarehouseID";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@WarehouseID", warehouseID);
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            txtWarehouseName.Text = reader["WarehouseName"].ToString();
                            int availableQty = Convert.ToInt32(reader["ItemQty"]);
                            QtyDisplay.Text = availableQty.ToString();
                            new ToolTip().SetToolTip(QtyDisplay, "Available Quantity in Warehouse");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading Warehouse details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearFields()
        {
            txtDeliveryOrderID.Clear();
            cmbCustomerOrderID.SelectedIndex = -1;
            txtCustomerName.Clear();
            txtDetails.Clear();
            txtDeliveryAmount.Clear();
            txtTotalAmount.Clear();
            dtpDate.Value = DateTime.Now;
            cmbWarehouseID.SelectedIndex = -1;  // Clear the selected warehouse
            txtWarehouseName.Clear();           // Clear warehouse name
            QtyDisplay.Text = string.Empty;     // Clear available quantity display
            LoadDeliveryOrderID();              // Reload DeliveryOrderID after clearing
        }

    }
}
