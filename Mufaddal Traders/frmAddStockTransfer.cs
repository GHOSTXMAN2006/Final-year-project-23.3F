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

        private string connectionString = DatabaseConfig.ConnectionString;

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

        // Add ToolTip to QtyDisplay
        private void frmAddStockTransfer_Load(object sender, EventArgs e)
        {
            LoadItems();
            LoadTransferID();

            // Create ToolTip instances for QtyDisplay and QtyDisplay2
            ToolTip toolTip = new ToolTip();

            // Set ToolTip properties
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 1000;
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = true;

            // Associate ToolTip with QtyDisplay and QtyDisplay2
            toolTip.SetToolTip(QtyDisplay, "Quantity of the selected item in the starting warehouse.");
            toolTip.SetToolTip(QtyDisplay2, "Quantity of the selected item in the ending warehouse.");
        }


        // Method to load the next available Stock Transfer ID
        private void LoadTransferID()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    // Query to find the smallest available ID starting from 1
                    string query = @"
                    SELECT TOP 1 Number
                    FROM (
                        SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Number
                        FROM master.dbo.spt_values
                    ) AS Numbers
                    WHERE Number NOT IN (SELECT ST_ID FROM Stock_Transfer)
                    ORDER BY Number";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    // If result is null, start with 1
                    txtTransferID.Text = result != null ? result.ToString() : "1";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading transfer ID: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void LoadWarehousesBasedOnItemID(int itemId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = @"
                SELECT DISTINCT sb.WarehouseID, w.Store_Name
                FROM tblStockBalance sb
                INNER JOIN Warehouse w ON sb.WarehouseID = w.StoreID
                WHERE sb.ItemID = @ItemID AND sb.ItemQty > 0";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ItemID", itemId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbStartingLocation.DisplayMember = "Store_Name";
                    cmbStartingLocation.ValueMember = "WarehouseID";
                    cmbStartingLocation.DataSource = dt;

                    cmbEndingLocation.DataSource = null; // Clear cmbEndingLocation initially
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading warehouses: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void cmbStartingLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStartingLocation.SelectedValue != null && cmbItemID.SelectedValue != null)
            {
                int selectedStartingWarehouse = (int)cmbStartingLocation.SelectedValue;
                int selectedItemID = (int)cmbItemID.SelectedValue;

                // Display the quantity for the selected item and starting warehouse
                DisplayQuantityForSelectedWarehouse(selectedItemID, selectedStartingWarehouse);

                // Load ending locations, excluding the selected starting warehouse
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        string query = @"
                    SELECT StoreID, Store_Name
                    FROM Warehouse
                    WHERE StoreID != @StartingWarehouse";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@StartingWarehouse", selectedStartingWarehouse);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        cmbEndingLocation.DisplayMember = "Store_Name";
                        cmbEndingLocation.ValueMember = "StoreID";
                        cmbEndingLocation.DataSource = dt;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading ending locations: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                // Reset QtyDisplay and cmbEndingLocation if no valid selection
                QtyDisplay.Text = "000000";
                cmbEndingLocation.DataSource = null;
            }
        }


        private void DisplayQuantityForSelectedWarehouse(int itemId, int warehouseId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = @"
                SELECT ItemQty 
                FROM tblStockBalance 
                WHERE ItemID = @ItemID AND WarehouseID = @WarehouseID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ItemID", itemId);
                    cmd.Parameters.AddWithValue("@WarehouseID", warehouseId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int quantity))
                    {
                        QtyDisplay.Text = quantity.ToString("D6"); // Format as 6 digits
                    }
                    else
                    {
                        QtyDisplay.Text = "000000"; // Default if no quantity is found
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching quantity: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    QtyDisplay.Text = "000000"; // Reset in case of an error
                }
            }
        }

        private void cmbEndingLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEndingLocation.SelectedValue != null && cmbItemID.SelectedValue != null)
            {
                int selectedEndingWarehouse = (int)cmbEndingLocation.SelectedValue;
                int selectedItemID = (int)cmbItemID.SelectedValue;

                // Display the quantity for the selected item and ending warehouse
                DisplayQuantityForEndingWarehouse(selectedItemID, selectedEndingWarehouse);
            }
            else
            {
                // Reset QtyDisplay2 if no valid selection
                QtyDisplay2.Text = "000000";
                QtyDisplay2.ForeColor = QtyDisplay2.ForeColor; // Keep default property color
            }
        }


        private void DisplayQuantityForEndingWarehouse(int itemId, int warehouseId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = @"
            SELECT ISNULL(ItemQty, 0) AS ItemQty
            FROM tblStockBalance 
            WHERE ItemID = @ItemID AND WarehouseID = @WarehouseID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ItemID", itemId);
                    cmd.Parameters.AddWithValue("@WarehouseID", warehouseId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    int quantity = 0;

                    // Ensure result is parsed correctly
                    if (result != null && int.TryParse(result.ToString(), out quantity))
                    {
                        QtyDisplay2.Text = quantity.ToString("D6"); // Always display 6-digit quantity

                        // Update the text color
                        if (quantity > 0)
                        {
                            QtyDisplay2.ForeColor = System.Drawing.Color.DarkOliveGreen;
                        }
                        else
                        {
                            QtyDisplay2.ForeColor = System.Drawing.Color.Crimson;
                        }
                    }
                    else
                    {
                        // Handle case where no result is found
                        QtyDisplay2.Text = "000000"; // Default 6-digit display
                        QtyDisplay2.ForeColor = System.Drawing.Color.Crimson; // Crimson for zero or invalid result
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching quantity for ending warehouse: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    QtyDisplay2.Text = "000000"; // Reset in case of error
                    QtyDisplay2.ForeColor = System.Drawing.Color.Crimson; // Default to crimson
                }
            }
        }



        private void cmbItemID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbItemID.SelectedValue != null)
            {
                int selectedItemID = (int)cmbItemID.SelectedValue;

                // Load warehouses with quantities > 0 for the selected ItemID
                LoadWarehousesBasedOnItemID(selectedItemID);

                // Reset ending location and QtyDisplay
                cmbEndingLocation.DataSource = null;
                QtyDisplay.Text = "000000";

                // Load the item name into txtName
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Item_Name FROM Items WHERE ItemID = @ItemID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ItemID", selectedItemID);

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            txtName.Text = reader["Item_Name"].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading item name: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (cmbItemID.SelectedValue == null || cmbStartingLocation.SelectedValue == null || cmbEndingLocation.SelectedValue == null)
                {
                    MessageBox.Show("Please select valid Item ID, Starting Location, and Ending Location.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtQty.Text, out int transferQty) || transferQty <= 0)
                {
                    MessageBox.Show("Please enter a valid quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int itemId = (int)cmbItemID.SelectedValue;
                int startingWarehouseId = (int)cmbStartingLocation.SelectedValue;
                int endingWarehouseId = (int)cmbEndingLocation.SelectedValue;

                // Fetch the available quantity and warehouse names
                int availableQty = 0;
                string itemName = string.Empty;
                string startingWarehouseName = string.Empty;
                string endingWarehouseName = string.Empty;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string fetchDataQuery = @"
                SELECT sb.ItemQty, i.Item_Name AS ItemName, 
                       (SELECT Store_Name FROM Warehouse WHERE StoreID = @StartingWarehouse) AS StartingWarehouseName,
                       (SELECT Store_Name FROM Warehouse WHERE StoreID = @EndingWarehouse) AS EndingWarehouseName
                FROM tblStockBalance sb
                INNER JOIN Items i ON sb.ItemID = i.ItemID
                WHERE sb.ItemID = @ItemID AND sb.WarehouseID = @StartingWarehouse";

                    SqlCommand cmd = new SqlCommand(fetchDataQuery, conn);
                    cmd.Parameters.AddWithValue("@ItemID", itemId);
                    cmd.Parameters.AddWithValue("@StartingWarehouse", startingWarehouseId);
                    cmd.Parameters.AddWithValue("@EndingWarehouse", endingWarehouseId);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        availableQty = reader.IsDBNull(reader.GetOrdinal("ItemQty")) ? 0 : reader.GetInt32(reader.GetOrdinal("ItemQty"));
                        itemName = reader["ItemName"].ToString();
                        startingWarehouseName = reader["StartingWarehouseName"].ToString();
                        endingWarehouseName = reader["EndingWarehouseName"].ToString();
                    }
                    conn.Close();
                }

                // Validate if the quantity to transfer is more than available
                if (transferQty > availableQty)
                {
                    MessageBox.Show($"Transfer quantity ({transferQty}) exceeds available quantity ({availableQty}) in the selected warehouse.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Save the Stock Transfer and update quantities in warehouses
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        // Temporarily enable IDENTITY_INSERT for Stock_Transfer
                        SqlCommand identityOnCmd = new SqlCommand("SET IDENTITY_INSERT Stock_Transfer ON", conn, transaction);
                        identityOnCmd.ExecuteNonQuery();

                        // Insert the Stock Transfer record
                        string insertTransferQuery = @"
                    INSERT INTO Stock_Transfer (ST_ID, ST_Date, ST_Qty, ItemID, Starting_Warehouse, Ending_Warehouse) 
                    VALUES (@ST_ID, @ST_Date, @ST_Qty, @ItemID, @StartingWarehouse, @EndingWarehouse)";

                        SqlCommand insertCmd = new SqlCommand(insertTransferQuery, conn, transaction);
                        insertCmd.Parameters.AddWithValue("@ST_ID", Convert.ToInt32(txtTransferID.Text)); // Use the auto-generated ID
                        insertCmd.Parameters.AddWithValue("@ST_Date", DateTime.Today);
                        insertCmd.Parameters.AddWithValue("@ST_Qty", transferQty);
                        insertCmd.Parameters.AddWithValue("@ItemID", itemId);
                        insertCmd.Parameters.AddWithValue("@StartingWarehouse", startingWarehouseId);
                        insertCmd.Parameters.AddWithValue("@EndingWarehouse", endingWarehouseId);
                        insertCmd.ExecuteNonQuery();

                        // Disable IDENTITY_INSERT after insertion
                        SqlCommand identityOffCmd = new SqlCommand("SET IDENTITY_INSERT Stock_Transfer OFF", conn, transaction);
                        identityOffCmd.ExecuteNonQuery();

                        // Update the quantity in the starting warehouse (subtract)
                        string updateStartingWarehouseQuery = @"
                    UPDATE tblStockBalance 
                    SET ItemQty = ItemQty - @Qty 
                    WHERE ItemID = @ItemID AND WarehouseID = @WarehouseID";

                        SqlCommand updateStartingCmd = new SqlCommand(updateStartingWarehouseQuery, conn, transaction);
                        updateStartingCmd.Parameters.AddWithValue("@Qty", transferQty);
                        updateStartingCmd.Parameters.AddWithValue("@ItemID", itemId);
                        updateStartingCmd.Parameters.AddWithValue("@WarehouseID", startingWarehouseId);
                        updateStartingCmd.ExecuteNonQuery();

                        // Update the quantity in the ending warehouse (add or insert)
                        string updateEndingWarehouseQuery = @"
                    IF EXISTS (SELECT 1 FROM tblStockBalance WHERE ItemID = @ItemID AND WarehouseID = @WarehouseID)
                    BEGIN
                        UPDATE tblStockBalance 
                        SET ItemQty = ItemQty + @Qty 
                        WHERE ItemID = @ItemID AND WarehouseID = @WarehouseID
                    END
                    ELSE
                    BEGIN
                        INSERT INTO tblStockBalance (ItemID, WarehouseID, ItemQty, ItemName, WarehouseName) 
                        VALUES (@ItemID, @WarehouseID, @Qty, @ItemName, @WarehouseName)
                    END";

                        SqlCommand updateEndingCmd = new SqlCommand(updateEndingWarehouseQuery, conn, transaction);
                        updateEndingCmd.Parameters.AddWithValue("@Qty", transferQty);
                        updateEndingCmd.Parameters.AddWithValue("@ItemID", itemId);
                        updateEndingCmd.Parameters.AddWithValue("@WarehouseID", endingWarehouseId);
                        updateEndingCmd.Parameters.AddWithValue("@ItemName", itemName);
                        updateEndingCmd.Parameters.AddWithValue("@WarehouseName", endingWarehouseName);
                        updateEndingCmd.ExecuteNonQuery();

                        // Commit transaction
                        transaction.Commit();

                        MessageBox.Show("Stock Transfer Saved Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();

                        // Trigger the event to notify other form
                        StockTransferred?.Invoke(this, EventArgs.Empty);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




    }
}
