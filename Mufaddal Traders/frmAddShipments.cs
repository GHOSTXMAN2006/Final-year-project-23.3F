using CrystalDecisions.Windows.Forms;
using Microsoft.VisualBasic;
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

namespace Mufaddal_Traders
{
    public partial class frmAddShipments : Form
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

        public frmAddShipments()
        {
            InitializeComponent();
        }

        //For addUpdateForms
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

        private void cmbCustomerOrderID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCustomerOrderID.SelectedValue != null)
            {
                int customerOrderID = Convert.ToInt32(cmbCustomerOrderID.SelectedValue);
                LoadCustomerName(customerOrderID);
                LoadOrderDetails(customerOrderID);

                // Clear warehouse-related fields
                txtWarehouseName.Clear();
                QtyDisplay.Text = string.Empty;
                cmbWarehouseID.DataSource = null;

                // Load warehouses for the selected item
                LoadWarehouses(customerOrderID);
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
            int orderQty = selectedItemQuantity;  // This is the order quantity to be deducted from stock
            int selectedItemID = GetSelectedItemID();  // Assume a method to get the `ItemID`

            // Debug: Show selected values
            Console.WriteLine($"DEBUG: Selected WarehouseID: {warehouseID}");
            Console.WriteLine($"DEBUG: Available Quantity in Warehouse: {availableQty}");
            Console.WriteLine($"DEBUG: Quantity required for shipment: {orderQty}");
            Console.WriteLine($"DEBUG: Selected ItemID: {selectedItemID}");

            if (orderQty <= 0)
            {
                MessageBox.Show("Invalid shipment quantity. Please select a valid order.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        // Insert shipment
                        string query = @"
                SET IDENTITY_INSERT tblShipments ON;
                INSERT INTO tblShipments
                (ShipmentID, CustomerOrderID, CustomerName, Details, ShipmentAmount, TotalAmount, DeliveryDate, WarehouseID)
                VALUES 
                (@ShipmentID, @CustomerOrderID, @CustomerName, @Details, @ShipmentAmount, @TotalAmount, @DeliveryDate, @WarehouseID);
                SET IDENTITY_INSERT tblShipments OFF;";

                        SqlCommand cmd = new SqlCommand(query, conn, transaction);
                        cmd.Parameters.AddWithValue("@ShipmentID", Convert.ToInt32(txtShipmentID.Text));
                        cmd.Parameters.AddWithValue("@CustomerOrderID", Convert.ToInt32(cmbCustomerOrderID.SelectedValue));
                        cmd.Parameters.AddWithValue("@CustomerName", txtCustomerName.Text);
                        cmd.Parameters.AddWithValue("@Details", txtDetails.Text);
                        cmd.Parameters.AddWithValue("@ShipmentAmount", decimal.Parse(txtShipmentAmount.Text));
                        cmd.Parameters.AddWithValue("@TotalAmount", decimal.Parse(txtTotalAmount.Text.Replace("Rs.", "").Trim()));
                        cmd.Parameters.AddWithValue("@DeliveryDate", dtpDate.Value.Date);
                        cmd.Parameters.AddWithValue("@WarehouseID", warehouseID);

                        Console.WriteLine($"DEBUG: Inserting shipment with ShipmentID: {txtShipmentID.Text}");
                        int rowsAffected = cmd.ExecuteNonQuery();
                        Console.WriteLine($"DEBUG: Rows affected by shipment insert: {rowsAffected}");

                        // Update stock balance (only for the specific ItemID)
                        string updateStockQuery = @"
                UPDATE tblStockBalance 
                SET ItemQty = ItemQty - @Quantity 
                WHERE WarehouseID = @WarehouseID AND ItemID = @ItemID";

                        SqlCommand updateStockCmd = new SqlCommand(updateStockQuery, conn, transaction);
                        updateStockCmd.Parameters.AddWithValue("@Quantity", orderQty);
                        updateStockCmd.Parameters.AddWithValue("@WarehouseID", warehouseID);
                        updateStockCmd.Parameters.AddWithValue("@ItemID", selectedItemID);  // Update only for the selected item

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

                        MessageBox.Show("Shipment saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Clear fields and reload customer orders
                        ClearFields();
                        LoadCustomerOrders();  // Reload CustomerOrderID to hide completed orders
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"DEBUG ERROR: Transaction rolled back due to: {ex.Message}");
                        MessageBox.Show($"Error saving shipment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            // Fetch the ItemID for the selected CustomerOrderID
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT ItemID FROM tblCustomerOrders WHERE CustomerOrderID = @CustomerOrderID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CustomerOrderID", Convert.ToInt32(cmbCustomerOrderID.SelectedValue));
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG ERROR: Failed to retrieve ItemID: {ex.Message}");
                return 0;
            }
        }

        private void ClearFields()
        {
            // Clear all form fields
            txtShipmentID.Clear();
            cmbCustomerOrderID.SelectedIndex = -1;  // Reset customer order selection
            txtCustomerName.Clear();                // Clear customer name
            txtDetails.Clear();                      // Clear order details
            txtShipmentAmount.Clear();                // Clear shipment amount
            txtTotalAmount.Clear();                   // Clear total amount
            dtpDate.Value = DateTime.Now;              // Reset the date picker to today's date
            cmbWarehouseID.SelectedIndex = -1;         // Reset warehouse selection
            txtWarehouseName.Clear();                  // Clear warehouse name
            QtyDisplay.Text = string.Empty;            // Clear available quantity display

            // Reload the next available ShipmentID
            LoadShipmentID();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Clear all form fields
            txtShipmentID.Clear();
            cmbCustomerOrderID.SelectedIndex = -1;
            txtCustomerName.Clear();
            txtDetails.Clear();
            txtShipmentAmount.Clear();
            txtTotalAmount.Clear();
            dtpDate.Value = DateTime.Now;
            cmbWarehouseID.SelectedIndex = -1;  // Clear the selected warehouse
            txtWarehouseName.Clear();           // Clear warehouse name
            QtyDisplay.Text = string.Empty;     // Clear available quantity display

            // Reload Shipment ID
            LoadShipmentID();
        }

        private void frmAddShipments_Load(object sender, EventArgs e)
        {
            // Load the ShipmentID when the form loads
            LoadShipmentID();

            // Load available customer orders for shipment (with "Export" status 'E')
            LoadCustomerOrders();

            // Set default date to current date
            dtpDate.Value = DateTime.Now;

            // Clear the form fields on load
            btnClear_Click(sender, e);
        }

        private void LoadShipmentID()
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
            WHERE Number NOT IN (SELECT ShipmentID FROM tblShipments)
            ORDER BY Number";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    txtShipmentID.Text = result != null ? result.ToString() : "1"; // Default to 1 if no records
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG ERROR: Failed to load ShipmentID: {ex.Message}");
                MessageBox.Show($"Error loading ShipmentID: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCustomerOrders()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
            SELECT CustomerOrderID 
            FROM tblCustomerOrders 
            WHERE Status = 'N' AND LocalOrExport = 'E'"; // Export orders only

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
                Console.WriteLine($"DEBUG ERROR: Failed to load customer orders: {ex.Message}");
                MessageBox.Show($"Error loading Customer Orders: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCustomerName(int customerOrderID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
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

        private decimal baseItemTotal = 0; // Store the base total amount

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
                        baseItemTotal = Convert.ToDecimal(reader["TotalPrice"]);  // Save base total price

                        selectedItemQuantity = quantity;  // Set selectedItemQuantity for later use

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

        private void txtShipmentAmount_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtShipmentAmount.Text, out decimal shipmentAmount))
            {
                // Add shipment amount to the base item total
                decimal finalAmount = baseItemTotal + shipmentAmount;
                txtTotalAmount.Text = $"Rs. {finalAmount:F2}";
            }
            else
            {
                // Reset to base total if the shipment amount is cleared
                txtTotalAmount.Text = $"Rs. {baseItemTotal:F2}";
            }
        }

    }
}
