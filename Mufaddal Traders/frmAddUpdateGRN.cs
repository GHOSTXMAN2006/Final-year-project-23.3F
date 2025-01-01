using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics; // <-- For Debug.WriteLine
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmAddUpdateGRN : Form
    {

        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        // Connection string for SQL Server
        private string connectionString = DatabaseConfig.ConnectionString;
        // somewhere at the class level:
        private bool isProgrammaticRadioChange = false;

        public frmAddUpdateGRN()
        {
            Debug.WriteLine("frmAddUpdateGRN constructor called 🐞");
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("btnClose_Click method called 🐞");
            this.Close();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("btnMinimize_Click method called 🐞");
            this.WindowState = FormWindowState.Minimized;
        }

        private void picHeader_MouseDown(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("picHeader_MouseDown method called 🐞");
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("btnBack_Click method called 🐞");
            this.Close();
        }

        private void frmAddUpdateGRN_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("frmAddUpdateGRN_Load method started 🐞");

            try
            {
                Debug.WriteLine("Attempting to load the next GRN ID...");
                LoadNextGRNID();
                Debug.WriteLine("LoadNextGRNID executed successfully ✅");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in LoadNextGRNID: {ex.Message} ❌");
                MessageBox.Show($"Failed to load the next GRN ID. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                Debug.WriteLine("Attempting to load Warehouse IDs...");
                LoadWarehouseIDs();
                Debug.WriteLine("LoadWarehouseIDs executed successfully ✅");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in LoadWarehouseIDs: {ex.Message} ❌");
                MessageBox.Show($"Failed to load warehouse information. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Debug.WriteLine("frmAddUpdateGRN_Load method completed 🐞");
        }

        private void LoadNextGRNID()
        {
            Debug.WriteLine("LoadNextGRNID method called 🐞");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    Debug.WriteLine("Attempting to open SQL connection...");
                    conn.Open();
                    Debug.WriteLine("SQL connection opened successfully ✅");

                    // Query to find the smallest available GRN_ID starting from 1
                    string query = @"
            SELECT TOP 1 Number
            FROM (SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Number
                  FROM master.dbo.spt_values) AS Numbers
            WHERE Number NOT IN (SELECT GRN_ID FROM tblGRN)
            ORDER BY Number";

                    Debug.WriteLine("Query to find the next available GRN_ID prepared:");
                    Debug.WriteLine(query);

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        Debug.WriteLine("Executing SQL query to fetch the next GRN_ID...");
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            Debug.WriteLine($"Query result received: {result}");
                            txtGRN_ID.Text = result.ToString();
                            Debug.WriteLine($"txtGRN_ID updated with value: {txtGRN_ID.Text}");
                        }
                        else
                        {
                            Debug.WriteLine("Query result is null. Setting txtGRN_ID to default value '1'.");
                            txtGRN_ID.Text = "1";
                        }
                    }
                }
                catch (SqlException sqlEx)
                {
                    Debug.WriteLine($"SQL error occurred while loading GRN ID: {sqlEx.Message} ❌");
                    MessageBox.Show($"SQL error occurred while loading GRN ID: {sqlEx.Message}", "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Unexpected error occurred while loading GRN ID: {ex.Message} ❌");
                    MessageBox.Show($"Unexpected error occurred while loading GRN ID: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        Debug.WriteLine("Closing SQL connection...");
                        conn.Close();
                        Debug.WriteLine("SQL connection closed ✅");
                    }
                }
            }

            Debug.WriteLine("LoadNextGRNID method completed 🐞");
        }

        private void rbPurchaseOrder_CheckedChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("rbPurchaseOrder_CheckedChanged method called 🐞");

            // Check if the change is programmatic to avoid unintended behavior
            Debug.WriteLine($"isProgrammaticRadioChange = {isProgrammaticRadioChange}");
            if (isProgrammaticRadioChange)
            {
                Debug.WriteLine("Change is programmatic. Exiting method without further processing 🚫");
                return;
            }

            if (rbPurchaseOrder.Checked)
            {
                Debug.WriteLine("rbPurchaseOrder is checked ✅");

                Debug.WriteLine("Calling ClearSupplierAndItemInformation to reset item and supplier details...");
                ClearSupplierAndItemInformation();
                Debug.WriteLine("Supplier and item information cleared successfully ✅");

                Debug.WriteLine("Setting txtItemQtys to read-only and changing background color...");
                txtItemQtys.ReadOnly = true;
                txtItemQtys.BackColor = Color.BurlyWood;
                Debug.WriteLine("txtItemQtys settings updated: ReadOnly = true, BackColor = BurlyWood ✅");

                Debug.WriteLine("Calling LoadPurchaseGIN_ID with parameter 'O' to load Purchase Orders...");
                LoadPurchaseGIN_ID("O"); // 'O' for Purchase Order
                Debug.WriteLine("LoadPurchaseGIN_ID method completed ✅");

                Debug.WriteLine("Resetting cmbPurchaseOrGIN_ID selection...");
                cmbPurchaseOrGIN_ID.SelectedIndex = -1;
                Debug.WriteLine("cmbPurchaseOrGIN_ID reset successfully ✅");
            }
            else
            {
                Debug.WriteLine("rbPurchaseOrder is not checked. Exiting method without further processing 🚫");
            }

            Debug.WriteLine("rbPurchaseOrder_CheckedChanged method completed 🐞");
        }

        private void rbPurchaseContract_CheckedChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("rbPurchaseContract_CheckedChanged method called 🐞");

            // Check if the change is programmatic to avoid unintended behavior
            Debug.WriteLine($"isProgrammaticRadioChange = {isProgrammaticRadioChange}");
            if (isProgrammaticRadioChange)
            {
                Debug.WriteLine("Change is programmatic. Exiting method without further processing 🚫");
                return;
            }

            if (rbPurchaseContract.Checked)
            {
                Debug.WriteLine("rbPurchaseContract is checked ✅");

                Debug.WriteLine("Calling ClearSupplierAndItemInformation to reset item and supplier details...");
                ClearSupplierAndItemInformation();
                Debug.WriteLine("Supplier and item information cleared successfully ✅");

                Debug.WriteLine("Setting txtItemQtys to editable and changing background color to default...");
                txtItemQtys.ReadOnly = false;
                txtItemQtys.BackColor = SystemColors.Control;
                Debug.WriteLine("txtItemQtys settings updated: ReadOnly = false, BackColor = Default ✅");

                Debug.WriteLine("Calling LoadPurchaseGIN_ID with parameter 'C' to load Purchase Contracts...");
                LoadPurchaseGIN_ID("C"); // 'C' for Purchase Contract
                Debug.WriteLine("LoadPurchaseGIN_ID method completed ✅");

                Debug.WriteLine("Resetting cmbPurchaseOrGIN_ID selection...");
                cmbPurchaseOrGIN_ID.SelectedIndex = -1;
                Debug.WriteLine("cmbPurchaseOrGIN_ID reset successfully ✅");
            }
            else
            {
                Debug.WriteLine("rbPurchaseContract is not checked. Exiting method without further processing 🚫");
            }

            Debug.WriteLine("rbPurchaseContract_CheckedChanged method completed 🐞");
        }

        private void rbGIN_CheckedChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("rbGIN_CheckedChanged method called 🐞");

            if (rbGIN.Checked)
            {
                Debug.WriteLine("rbGIN is checked ✅");

                Debug.WriteLine("Calling ClearSupplierAndItemInformation to reset item and supplier details...");
                ClearSupplierAndItemInformation();
                Debug.WriteLine("Supplier and item information cleared successfully ✅");

                Debug.WriteLine("Calling LoadPurchaseGIN_ID with parameter 'G' to load GINs...");
                LoadPurchaseGIN_ID("G"); // Load GINs with Status = 'N'
                Debug.WriteLine("LoadPurchaseGIN_ID method completed ✅");
            }
            else
            {
                Debug.WriteLine("rbGIN is not checked. Exiting method without further processing 🚫");
            }

            Debug.WriteLine("rbGIN_CheckedChanged method completed 🐞");
        }

        private void ClearSupplierAndItemInformation()
        {
            Debug.WriteLine("ClearSupplierAndItemInformation method called 🐞");

            // Clearing txtSupplierID
            Debug.WriteLine($"Clearing txtSupplierID. Current Value: '{txtSupplierID.Text}'");
            txtSupplierID.Clear();
            Debug.WriteLine("txtSupplierID cleared successfully ✅");

            // Clearing txtSupplierName
            Debug.WriteLine($"Clearing txtSupplierName. Current Value: '{txtSupplierName.Text}'");
            txtSupplierName.Clear();
            Debug.WriteLine("txtSupplierName cleared successfully ✅");

            // Clearing txtItemIDs
            Debug.WriteLine($"Clearing txtItemIDs. Current Value: '{txtItemIDs.Text}'");
            txtItemIDs.Clear();
            Debug.WriteLine("txtItemIDs cleared successfully ✅");

            // Clearing txtItemNames
            Debug.WriteLine($"Clearing txtItemNames. Current Value: '{txtItemNames.Text}'");
            txtItemNames.Clear();
            Debug.WriteLine("txtItemNames cleared successfully ✅");

            // Clearing txtItemQtys
            Debug.WriteLine($"Clearing txtItemQtys. Current Value: '{txtItemQtys.Text}'");
            txtItemQtys.Clear();
            Debug.WriteLine("txtItemQtys cleared successfully ✅");

            // Resetting cmbPurchaseOrGIN_ID selected index
            Debug.WriteLine($"Resetting cmbPurchaseOrGIN_ID selected index. Current Index: {cmbPurchaseOrGIN_ID.SelectedIndex}");
            cmbPurchaseOrGIN_ID.SelectedIndex = -1;
            Debug.WriteLine("cmbPurchaseOrGIN_ID selected index reset successfully ✅");

            // Clearing cmbPurchaseOrGIN_ID text
            Debug.WriteLine($"Clearing cmbPurchaseOrGIN_ID text. Current Value: '{cmbPurchaseOrGIN_ID.Text}'");
            cmbPurchaseOrGIN_ID.Text = string.Empty;
            Debug.WriteLine("cmbPurchaseOrGIN_ID text cleared successfully ✅");

            Debug.WriteLine("ClearSupplierAndItemInformation method completed 🐞");
        }

        private void LoadPurchaseGIN_ID(string type)
        {
            Debug.WriteLine($"LoadPurchaseGIN_ID method called 🐞 for type: '{type}'");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    Debug.WriteLine("Opening database connection...");
                    conn.Open();
                    Debug.WriteLine("Database connection opened successfully ✅");

                    string query = string.Empty;

                    // Determine the query based on the type
                    if (type == "O") // Purchase Orders
                    {
                        Debug.WriteLine("Preparing query for Purchase Orders...");
                        query = @"
                SELECT DISTINCT PurchaseOrderID AS ID
                FROM Purchase_Orders
                WHERE Status = 'N'";
                    }
                    else if (type == "C") // Purchase Contracts
                    {
                        Debug.WriteLine("Preparing query for Purchase Contracts...");
                        query = @"
                SELECT DISTINCT PurchaseContractID AS ID
                FROM Purchase_Contract";
                    }
                    else if (type == "G") // GINs
                    {
                        Debug.WriteLine("Preparing query for GINs...");
                        query = @"
                SELECT DISTINCT GIN_ID AS ID
                FROM tblGIN
                WHERE Status = 'N'";
                    }
                    else
                    {
                        Debug.WriteLine($"Invalid type provided: {type}");
                        throw new ArgumentException("Invalid type provided to LoadPurchaseGIN_ID.");
                    }

                    Debug.WriteLine($"Query prepared: {query}");

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable data = new DataTable();

                    Debug.WriteLine("Executing query to fetch data...");
                    adapter.Fill(data);
                    Debug.WriteLine($"Query executed successfully ✅ Rows fetched: {data.Rows.Count}");

                    if (data.Rows.Count > 0)
                    {
                        Debug.WriteLine("Fetched IDs:");
                        foreach (DataRow row in data.Rows)
                        {
                            Debug.WriteLine($"- {row["ID"]}");
                        }
                    }
                    else
                    {
                        Debug.WriteLine("No data found for the specified type.");
                    }

                    // Binding data to the ComboBox
                    Debug.WriteLine("Clearing previous data source for cmbPurchaseOrGIN_ID...");
                    cmbPurchaseOrGIN_ID.DataSource = null;

                    Debug.WriteLine("Setting DisplayMember and ValueMember for cmbPurchaseOrGIN_ID...");
                    cmbPurchaseOrGIN_ID.DisplayMember = "ID";
                    cmbPurchaseOrGIN_ID.ValueMember = "ID";

                    Debug.WriteLine("Binding new data source to cmbPurchaseOrGIN_ID...");
                    cmbPurchaseOrGIN_ID.DataSource = data;

                    Debug.WriteLine("Resetting selected index for cmbPurchaseOrGIN_ID...");
                    cmbPurchaseOrGIN_ID.SelectedIndex = -1;

                    Debug.WriteLine("Data bound to cmbPurchaseOrGIN_ID successfully ✅");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in LoadPurchaseGIN_ID: {ex.Message}");
                    MessageBox.Show($"Error loading data for type '{type}': {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Debug.WriteLine("Exiting LoadPurchaseGIN_ID method 🐞");
                }
            }
        }

        private void cmbPurchaseOrGIN_ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("cmbPurchaseOrGIN_ID_SelectedIndexChanged triggered 🛠️");
            try
            {
                if (cmbPurchaseOrGIN_ID.SelectedItem == null)
                {
                    Debug.WriteLine("No item selected in cmbPurchaseOrGIN_ID. Exiting method.");
                    return; // Prevent null reference exception
                }

                string selectedID = cmbPurchaseOrGIN_ID.SelectedValue?.ToString() ?? string.Empty;
                Debug.WriteLine($"Selected ID: {selectedID}");

                // Validate the selected ID
                if (string.IsNullOrEmpty(selectedID))
                {
                    Debug.WriteLine("Selected ID is empty. Exiting method.");
                    MessageBox.Show("Please select a valid ID from the dropdown.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (rbPurchaseOrder.Checked)
                {
                    Debug.WriteLine("rbPurchaseOrder is checked. Preparing to load Purchase Order details...");
                    Debug.WriteLine($"Calling LoadPurchaseOrderDetails with ID: {selectedID}");
                    LoadPurchaseOrderDetails(selectedID);
                }
                else if (rbPurchaseContract.Checked)
                {
                    Debug.WriteLine("rbPurchaseContract is checked. Preparing to load Purchase Contract details...");
                    Debug.WriteLine($"Calling LoadPurchaseContractDetails with ID: {selectedID}");
                    LoadPurchaseContractDetails(selectedID);
                }
                else if (rbGIN.Checked)
                {
                    Debug.WriteLine("rbGIN is checked. Preparing to load GIN details...");
                    if (int.TryParse(selectedID, out int ginID))
                    {
                        Debug.WriteLine($"Parsed GIN ID as integer: {ginID}");
                        Debug.WriteLine($"Calling LoadSelectedGINDetails with GIN ID: {ginID}");
                        LoadSelectedGINDetails(ginID);
                    }
                    else
                    {
                        Debug.WriteLine("Selected value for GIN ID is not a valid integer. Displaying error message.");
                        MessageBox.Show("Invalid GIN ID selected. Please select a valid GIN ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    Debug.WriteLine("No radio button is checked. Unable to determine action.");
                    MessageBox.Show("Please select a valid option (Purchase Order, Purchase Contract, or GIN).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in cmbPurchaseOrGIN_ID_SelectedIndexChanged: {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                MessageBox.Show($"An unexpected error occurred while processing your selection: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Debug.WriteLine("Exiting cmbPurchaseOrGIN_ID_SelectedIndexChanged method 🛠️");
            }
        }

        private void LoadSelectedGINDetails(int ginID)
        {
            Debug.WriteLine($"LoadSelectedGINDetails method called for GIN_ID: {ginID}");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    Debug.WriteLine("Attempting to open database connection...");
                    conn.Open();
                    Debug.WriteLine("Database connection opened successfully ✅");

                    string query = @"
        SELECT SupplierID, Supplier_Name, ItemID, Item_Name, GIN_Quantity
        FROM tblGIN
        WHERE GIN_ID = @GIN_ID";

                    Debug.WriteLine("Preparing SQL command...");
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@GIN_ID", ginID);
                    Debug.WriteLine("SQL command prepared successfully.");

                    Debug.WriteLine($"Executing query for GIN_ID: {ginID}");
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Debug.WriteLine("Query executed successfully. Processing results...");

                        txtSupplierID.Text = reader["SupplierID"].ToString();
                        Debug.WriteLine($"SupplierID loaded: {txtSupplierID.Text}");

                        txtSupplierName.Text = reader["Supplier_Name"].ToString();
                        Debug.WriteLine($"Supplier_Name loaded: {txtSupplierName.Text}");

                        txtItemIDs.Text = reader["ItemID"].ToString();
                        Debug.WriteLine($"ItemID loaded: {txtItemIDs.Text}");

                        txtItemNames.Text = reader["Item_Name"].ToString();
                        Debug.WriteLine($"Item_Name loaded: {txtItemNames.Text}");

                        txtItemQtys.Text = reader["GIN_Quantity"].ToString();
                        Debug.WriteLine($"GIN_Quantity loaded: {txtItemQtys.Text}");

                        Debug.WriteLine("GIN details loaded successfully ✅");
                    }
                    else
                    {
                        Debug.WriteLine("No details found for the specified GIN_ID.");
                        MessageBox.Show("No details found for the selected GIN.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearSupplierAndItemInformation();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error loading GIN details: {ex.Message}");
                    Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                    MessageBox.Show($"Error loading GIN details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Debug.WriteLine("Exiting LoadSelectedGINDetails method.");
                }
            }
        }

        private void LoadPurchaseOrderDetails(string purchaseID)
        {
            Debug.WriteLine("LoadPurchaseOrderDetails method called 🐞");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    Debug.WriteLine("Attempting to open database connection...");
                    conn.Open();
                    Debug.WriteLine("Database connection opened successfully ✅");

                    string query = @"
        SELECT po.SupplierID, s.Name AS SupplierName, i.ItemID, i.Item_Name, po.ItemQty
        FROM Purchase_Orders po
        JOIN Items i ON po.ItemID = i.ItemID
        JOIN tblManageSuppliers s ON po.SupplierID = s.SupplierID
        WHERE po.PurchaseOrderID = @PurchaseID";

                    Debug.WriteLine("Preparing SQL command...");
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@PurchaseID", purchaseID);
                    Debug.WriteLine($"SQL command prepared with PurchaseID: {purchaseID}");

                    Debug.WriteLine("Executing query...");
                    SqlDataReader reader = cmd.ExecuteReader();
                    Debug.WriteLine("Query executed successfully. Processing results...");

                    // Clear text fields
                    Debug.WriteLine("Clearing previous supplier and item information...");
                    txtSupplierID.Clear();
                    txtSupplierName.Clear();
                    txtItemIDs.Clear();
                    txtItemNames.Clear();
                    txtItemQtys.Clear();

                    while (reader.Read())
                    {
                        txtSupplierID.Text = reader["SupplierID"].ToString();
                        Debug.WriteLine($"SupplierID loaded: {txtSupplierID.Text}");

                        txtSupplierName.Text = reader["SupplierName"].ToString();
                        Debug.WriteLine($"SupplierName loaded: {txtSupplierName.Text}");

                        txtItemIDs.AppendText(reader["ItemID"].ToString() + Environment.NewLine);
                        Debug.WriteLine($"ItemID loaded: {reader["ItemID"]}");

                        txtItemNames.AppendText(reader["Item_Name"].ToString() + Environment.NewLine);
                        Debug.WriteLine($"ItemName loaded: {reader["Item_Name"]}");

                        txtItemQtys.AppendText(reader["ItemQty"].ToString() + Environment.NewLine);
                        Debug.WriteLine($"ItemQty loaded: {reader["ItemQty"]}");
                    }

                    Debug.WriteLine("Purchase Order details loaded successfully ✅");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error loading Purchase Order details: {ex.Message}");
                    MessageBox.Show("Error loading Purchase Order details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Debug.WriteLine("Exiting LoadPurchaseOrderDetails method.");
                }
            }
        }

        private void LoadPurchaseContractDetails(string purchaseID)
        {
            Debug.WriteLine("LoadPurchaseContractDetails method called 🐞");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    Debug.WriteLine("Attempting to open database connection...");
                    conn.Open();
                    Debug.WriteLine("Database connection opened successfully ✅");

                    string query = @"
        SELECT pc.SupplierID,
               s.Name AS SupplierName,
               i.ItemID,
               i.Item_Name
        FROM Purchase_Contract pc
        JOIN Items i ON pc.ItemID = i.ItemID
        JOIN tblManageSuppliers s ON pc.SupplierID = s.SupplierID
        WHERE pc.PurchaseContractID = @PurchaseID";

                    Debug.WriteLine("Preparing SQL command...");
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@PurchaseID", purchaseID);
                    Debug.WriteLine($"SQL command prepared with PurchaseID: {purchaseID}");

                    Debug.WriteLine("Executing query...");
                    SqlDataReader reader = cmd.ExecuteReader();
                    Debug.WriteLine("Query executed successfully. Processing results...");

                    // Temporarily store existing text in txtItemQtys
                    string existingQtyText = txtItemQtys.Text;
                    Debug.WriteLine($"Preserving existing ItemQtys text: {existingQtyText}");

                    // Clear text fields for IDs, Names, etc.
                    Debug.WriteLine("Clearing previous supplier and item information...");
                    txtSupplierID.Clear();
                    txtSupplierName.Clear();
                    txtItemIDs.Clear();
                    txtItemNames.Clear();

                    while (reader.Read())
                    {
                        txtSupplierID.Text = reader["SupplierID"].ToString();
                        Debug.WriteLine($"SupplierID loaded: {txtSupplierID.Text}");

                        txtSupplierName.Text = reader["SupplierName"].ToString();
                        Debug.WriteLine($"SupplierName loaded: {txtSupplierName.Text}");

                        txtItemIDs.AppendText(reader["ItemID"].ToString() + Environment.NewLine);
                        Debug.WriteLine($"ItemID loaded: {reader["ItemID"]}");

                        txtItemNames.AppendText(reader["Item_Name"].ToString() + Environment.NewLine);
                        Debug.WriteLine($"ItemName loaded: {reader["Item_Name"]}");
                    }

                    Debug.WriteLine("Purchase Contract details loaded successfully ✅");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error loading Purchase Contract details: {ex.Message}");
                    MessageBox.Show("Error loading Purchase Contract details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Debug.WriteLine("Exiting LoadPurchaseContractDetails method.");
                }
            }
        }

        private void LoadWarehouseIDs()
        {
            Debug.WriteLine("LoadWarehouseIDs method called 🐞");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    Debug.WriteLine("Database connection opened for loading Warehouse IDs ✅");

                    string query = "SELECT StoreID FROM Warehouse";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    cmbWarehouseID.Items.Clear();
                    Debug.WriteLine("Cleared previous items in cmbWarehouseID");

                    while (reader.Read())
                    {
                        string storeID = reader["StoreID"].ToString();
                        cmbWarehouseID.Items.Add(storeID);
                        Debug.WriteLine($"Loaded Warehouse ID: {storeID}");
                    }

                    Debug.WriteLine("Finished loading Warehouse IDs into cmbWarehouseID.");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error loading Warehouse IDs: {ex.Message}");
                    MessageBox.Show($"Error loading Warehouse IDs: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cmbWarehouseID_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("cmbWarehouseID_SelectedIndexChanged method called 🐞");

            if (cmbWarehouseID.SelectedItem == null)
            {
                Debug.WriteLine("No Warehouse selected in cmbWarehouseID. Exiting method 🚫.");
                txtWarehouseName.Text = string.Empty;
                return;
            }

            string selectedWarehouseID = cmbWarehouseID.SelectedItem.ToString();
            Debug.WriteLine($"Selected WarehouseID: {selectedWarehouseID}");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    Debug.WriteLine("Database connection opened for fetching Warehouse Name ✅");

                    string query = "SELECT Store_Name FROM Warehouse WHERE StoreID = @StoreID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StoreID", selectedWarehouseID);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        txtWarehouseName.Text = result.ToString();
                        Debug.WriteLine($"Fetched Warehouse Name: {txtWarehouseName.Text}");
                    }
                    else
                    {
                        txtWarehouseName.Text = string.Empty;
                        Debug.WriteLine($"No Warehouse Name found for StoreID: {selectedWarehouseID}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in cmbWarehouseID_SelectedIndexChanged: {ex.Message}");
                    MessageBox.Show($"Error loading Warehouse Name: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("btnSave_Click method triggered 🐞");

            try
            {
                // Step 1: Validate Inputs
                Debug.WriteLine("Validating inputs...");
                if (!ValidateInputs())
                {
                    MessageBox.Show("Please ensure all fields are correctly filled.",
                                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Debug.WriteLine("Input validation failed. Exiting btnSave_Click.");
                    return;
                }

                // Step 2: Validate Monthly Purchase Contract (if applicable)
                Debug.WriteLine("Validating monthly purchase contract...");
                if (!ValidatePurchaseContractMonthly())
                {
                    Debug.WriteLine("Monthly validation for Purchase Contract failed. Exiting btnSave_Click.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    Debug.WriteLine("Database connection opened successfully ✅");

                    // Step 3: Collect Item Information
                    Debug.WriteLine("Collecting item information...");
                    string[] itemIDs = txtItemIDs.Lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
                    string[] itemQuantities = txtItemQtys.Lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

                    if (itemIDs.Length != itemQuantities.Length)
                    {
                        MessageBox.Show("Mismatch between items and quantities.",
                                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Debug.WriteLine("Mismatch between items and quantities 🚫. Exiting btnSave_Click.");
                        return;
                    }

                    Debug.WriteLine($"Collected ItemIDs: {string.Join(", ", itemIDs)}");
                    Debug.WriteLine($"Collected ItemQuantities: {string.Join(", ", itemQuantities)}");

                    // Step 4: Validate Selected Warehouse
                    string warehouseID = cmbWarehouseID.SelectedItem?.ToString();
                    if (string.IsNullOrWhiteSpace(warehouseID))
                    {
                        MessageBox.Show("Please select a valid Warehouse ID.",
                                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Debug.WriteLine("Warehouse ID is invalid or not selected 🚫. Exiting btnSave_Click.");
                        return;
                    }
                    Debug.WriteLine($"Selected WarehouseID: {warehouseID}");

                    // Step 5: Validate Selected Purchase/GIN ID
                    string purchaseOrGIN_ID = cmbPurchaseOrGIN_ID.SelectedValue?.ToString();
                    if (string.IsNullOrWhiteSpace(purchaseOrGIN_ID))
                    {
                        MessageBox.Show("Please select a valid Purchase/GIN ID.",
                                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Debug.WriteLine("Purchase/GIN ID is invalid or not selected 🚫. Exiting btnSave_Click.");
                        return;
                    }
                    Debug.WriteLine($"Selected Purchase/GIN ID: {purchaseOrGIN_ID}");

                    // Step 6: Begin Transaction
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            Debug.WriteLine("Starting database transaction...");

                            // Step 7: Insert GRN Record
                            Debug.WriteLine("Inserting GRN record...");
                            string insertGRNQuery = @"
                    INSERT INTO tblGRN (GRN_ID, PurchaseID, PurchaseType, SupplierID, ItemID, ItemQuantity, WarehouseID, GRN_Date, GRN_Type)
                    VALUES (@GRN_ID, @PurchaseID, @PurchaseType, @SupplierID, @ItemIDs, @ItemQuantities, @WarehouseID, GETDATE(), @GRN_Type)";

                            using (SqlCommand cmd = new SqlCommand(insertGRNQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@GRN_ID", txtGRN_ID.Text);
                                cmd.Parameters.AddWithValue("@PurchaseID", purchaseOrGIN_ID);
                                cmd.Parameters.AddWithValue("@PurchaseType", rbPurchaseOrder.Checked ? "O" : rbPurchaseContract.Checked ? "C" : "G");
                                cmd.Parameters.AddWithValue("@SupplierID", txtSupplierID.Text);
                                cmd.Parameters.AddWithValue("@ItemIDs", string.Join(",", itemIDs));
                                cmd.Parameters.AddWithValue("@ItemQuantities", string.Join(",", itemQuantities));
                                cmd.Parameters.AddWithValue("@WarehouseID", warehouseID);
                                cmd.Parameters.AddWithValue("@GRN_Type", cmbGRN_Type.SelectedItem?.ToString() ?? string.Empty);

                                Debug.WriteLine("Executing insert GRN query...");
                                cmd.ExecuteNonQuery();
                                Debug.WriteLine("GRN record inserted successfully ✅");
                            }

                            // Step 8: Update Stock Balance
                            Debug.WriteLine("Updating stock balance...");
                            for (int i = 0; i < itemIDs.Length; i++)
                            {
                                string updateStockBalanceQuery = @"
                        MERGE INTO tblStockBalance AS Target
                        USING (SELECT @ItemID AS ItemID, @WarehouseID AS WarehouseID, @ItemQty AS ItemQty, @ItemName AS ItemName) AS Source
                        ON Target.ItemID = Source.ItemID AND Target.WarehouseID = Source.WarehouseID
                        WHEN MATCHED THEN
                            UPDATE SET Target.ItemQty = Target.ItemQty + Source.ItemQty
                        WHEN NOT MATCHED THEN
                            INSERT (ItemID, ItemName, WarehouseID, WarehouseName, ItemQty)
                            VALUES (Source.ItemID, Source.ItemName, Source.WarehouseID, 
                                    (SELECT Store_Name FROM Warehouse WHERE StoreID = Source.WarehouseID), Source.ItemQty);";

                                using (SqlCommand cmd = new SqlCommand(updateStockBalanceQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@ItemID", itemIDs[i]);
                                    cmd.Parameters.AddWithValue("@ItemName", txtItemNames.Lines[i].Trim());
                                    cmd.Parameters.AddWithValue("@WarehouseID", warehouseID);
                                    cmd.Parameters.AddWithValue("@ItemQty", int.Parse(itemQuantities[i]));

                                    Debug.WriteLine($"Executing stock balance update for ItemID: {itemIDs[i]}...");
                                    cmd.ExecuteNonQuery();
                                    Debug.WriteLine($"Stock balance updated for ItemID: {itemIDs[i]} ✅");
                                }
                            }

                            // Step 9: Update GIN Status if GIN Type
                            if (rbGIN.Checked)
                            {
                                Debug.WriteLine("Updating GIN status for GIN Type...");
                                string updateGINStatusQuery = "UPDATE tblGIN SET Status = 'Y' WHERE GIN_ID = @GIN_ID";

                                using (SqlCommand cmd = new SqlCommand(updateGINStatusQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@GIN_ID", purchaseOrGIN_ID);
                                    Debug.WriteLine("Executing GIN status update query...");
                                    cmd.ExecuteNonQuery();
                                    Debug.WriteLine("GIN status updated to 'Y' ✅");
                                }
                            }

                            // Step 10: Commit Transaction
                            Debug.WriteLine("Committing transaction...");
                            transaction.Commit();
                            Debug.WriteLine("Transaction committed successfully ✅");

                            MessageBox.Show("GRN record saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Clear Fields
                            ClearFields();
                            LoadNextGRNID();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error during transaction. Rolling back. Error: {ex.Message}");
                            transaction.Rollback();
                            MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected error in btnSave_Click: {ex.Message}");
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidatePurchaseContractMonthly()
        {
            Debug.WriteLine("ValidatePurchaseContractMonthly method called 🐞");

            if (rbPurchaseContract.Checked)
            {
                Debug.WriteLine("Purchase Contract radio button is checked. Proceeding with validation...");

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        Debug.WriteLine("Opening database connection...");
                        conn.Open();
                        Debug.WriteLine("Database connection opened successfully ✅");

                        string query = @"
            SELECT COUNT(*) 
            FROM tblGRN 
            WHERE PurchaseType = 'C' AND PurchaseID = @PurchaseID 
            AND MONTH(GRN_Date) = MONTH(GETDATE()) AND YEAR(GRN_Date) = YEAR(GETDATE())";

                        Debug.WriteLine("Preparing SQL command...");
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            string purchaseID = cmbPurchaseOrGIN_ID.SelectedItem != null
                                                ? cmbPurchaseOrGIN_ID.SelectedValue.ToString()
                                                : cmbPurchaseOrGIN_ID.Text;

                            Debug.WriteLine($"Selected PurchaseID: {purchaseID}");

                            if (string.IsNullOrWhiteSpace(purchaseID))
                            {
                                Debug.WriteLine("Purchase ID is empty or null. Validation failed.");
                                MessageBox.Show("Purchase ID is required for Purchase Contract validation.",
                                                "Validation Error",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Warning);
                                return false;
                            }

                            cmd.Parameters.AddWithValue("@PurchaseID", purchaseID);
                            Debug.WriteLine("Executing SQL query...");
                            int count = (int)cmd.ExecuteScalar();
                            Debug.WriteLine($"Query executed. Records found: {count}");

                            if (count > 0)
                            {
                                Debug.WriteLine("Validation failed: A GRN already exists for this Purchase Contract this month.");
                                MessageBox.Show("A GRN for this Purchase Contract has already been created this month.",
                                                "Validation Error",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Warning);
                                return false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error in ValidatePurchaseContractMonthly: {ex.Message}");
                        MessageBox.Show($"An error occurred during validation: {ex.Message}",
                                        "Validation Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return false;
                    }
                    finally
                    {
                        Debug.WriteLine("Exiting ValidatePurchaseContractMonthly method.");
                    }
                }
            }
            else
            {
                Debug.WriteLine("Purchase Contract radio button is not checked. Skipping validation.");
            }

            Debug.WriteLine("Validation successful. Returning true ✅");
            return true;
        }

        private bool ValidateInputs()
        {
            Debug.WriteLine("ValidateInputs method called 🐞");

            // Validate GRN ID
            if (string.IsNullOrWhiteSpace(txtGRN_ID.Text))
            {
                Debug.WriteLine("Validation failed: GRN ID is empty.");
                MessageBox.Show("GRN ID cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            Debug.WriteLine($"GRN ID validated: {txtGRN_ID.Text}");

            // Validate Purchase ID
            if (cmbPurchaseOrGIN_ID.SelectedIndex == -1 && string.IsNullOrWhiteSpace(cmbPurchaseOrGIN_ID.Text))
            {
                Debug.WriteLine("Validation failed: Purchase ID is not selected or empty.");
                MessageBox.Show("Please select or enter a valid Purchase ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (cmbPurchaseOrGIN_ID.SelectedIndex == -1 && !IsPurchaseIDValid(cmbPurchaseOrGIN_ID.Text))
            {
                Debug.WriteLine($"Validation failed: Purchase ID '{cmbPurchaseOrGIN_ID.Text}' is invalid.");
                MessageBox.Show("Please select or enter a valid Purchase ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            Debug.WriteLine($"Purchase ID validated: {cmbPurchaseOrGIN_ID.Text}");

            // Validate Supplier Information
            if (string.IsNullOrWhiteSpace(txtSupplierID.Text) || string.IsNullOrWhiteSpace(txtSupplierName.Text))
            {
                Debug.WriteLine("Validation failed: Supplier information is incomplete.");
                MessageBox.Show("Supplier information is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            Debug.WriteLine($"Supplier validated: ID = {txtSupplierID.Text}, Name = {txtSupplierName.Text}");

            // Validate Item Information
            if (string.IsNullOrWhiteSpace(txtItemIDs.Text) || string.IsNullOrWhiteSpace(txtItemNames.Text))
            {
                Debug.WriteLine("Validation failed: Item information is incomplete.");
                MessageBox.Show("Item information is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            Debug.WriteLine($"Item information validated: IDs = {txtItemIDs.Text}, Names = {txtItemNames.Text}");

            // Validate Warehouse ID
            if (cmbWarehouseID.SelectedIndex == -1 || string.IsNullOrWhiteSpace(cmbWarehouseID.Text))
            {
                Debug.WriteLine("Validation failed: Warehouse ID is not selected or empty.");
                MessageBox.Show("Please select a valid Warehouse ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            Debug.WriteLine($"Warehouse ID validated: {cmbWarehouseID.Text}");

            Debug.WriteLine("All validations passed ✅");
            return true;
        }

        private bool IsPurchaseIDValid(string purchaseID)
        {
            Debug.WriteLine($"Validating PurchaseID: {purchaseID}");
            if (string.IsNullOrWhiteSpace(purchaseID))
            {
                Debug.WriteLine("Validation failed: PurchaseID is empty or null.");
                return false;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    Debug.WriteLine("Database connection opened successfully for PurchaseID validation.");

                    string query = @"
        SELECT COUNT(1) 
        FROM (
            SELECT PurchaseOrderID AS PurchaseID FROM Purchase_Orders
            UNION
            SELECT PurchaseContractID AS PurchaseID FROM Purchase_Contract
        ) AS AllPurchases
        WHERE PurchaseID = @PurchaseID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@PurchaseID", purchaseID);
                    Debug.WriteLine("Executing PurchaseID validation query...");
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    Debug.WriteLine($"Query executed. Validation result: {count > 0}");

                    return count > 0;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in IsPurchaseIDValid: {ex.Message}");
                    MessageBox.Show("Error validating Purchase ID: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        private void ClearFields()
        {
            Debug.WriteLine("ClearFields method called 🐞");

            // Clear text fields
            Debug.WriteLine("Clearing GRN ID...");
            txtGRN_ID.Clear();

            Debug.WriteLine("Resetting Purchase/GIN ID combo box...");
            cmbPurchaseOrGIN_ID.SelectedIndex = -1;

            Debug.WriteLine("Clearing supplier information...");
            txtSupplierID.Clear();
            txtSupplierName.Clear();

            Debug.WriteLine("Clearing item information...");
            txtItemIDs.Clear();
            txtItemNames.Clear();
            txtItemQtys.Clear();

            Debug.WriteLine("Resetting Warehouse combo box...");
            cmbWarehouseID.SelectedIndex = -1;
            txtWarehouseName.Clear();

            Debug.WriteLine("Resetting GRN Type combo box...");
            cmbGRN_Type.SelectedIndex = -1;

            Debug.WriteLine("Resetting radio buttons...");
            rbPurchaseOrder.Checked = false;
            rbPurchaseContract.Checked = false;

            Debug.WriteLine("All fields cleared successfully.");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("btnUpdate_Click method invoked 🛠️");

            try
            {
                // Step 1: Validate GRN ID existence
                Debug.WriteLine("Validating GRN ID...");
                if (string.IsNullOrWhiteSpace(txtGRN_ID.Text))
                {
                    MessageBox.Show("GRN ID cannot be empty. Please search for a GRN to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if GRN exists
                bool grnExists;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string checkGRNQuery = "SELECT COUNT(1) FROM tblGRN WHERE GRN_ID = @GRN_ID";
                    using (SqlCommand cmd = new SqlCommand(checkGRNQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@GRN_ID", txtGRN_ID.Text);
                        grnExists = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                        Debug.WriteLine($"GRN existence check: GRN_ID={txtGRN_ID.Text}, Exists={grnExists}");
                    }
                }

                if (!grnExists)
                {
                    MessageBox.Show("GRN ID does not exist. Please create a new GRN instead.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Step 2: Fetch Form Values
                Debug.WriteLine("Fetching form values...");
                string newPurchaseType = rbPurchaseOrder.Checked ? "O" : rbGIN.Checked ? "G" : "C";
                string newPurchaseID = cmbPurchaseOrGIN_ID.SelectedValue?.ToString() ?? cmbPurchaseOrGIN_ID.Text;
                string newWarehouseID = cmbWarehouseID.SelectedValue?.ToString() ?? cmbWarehouseID.Text;
                string newGRNType = cmbGRN_Type.SelectedItem?.ToString();

                if (string.IsNullOrWhiteSpace(newPurchaseID) || newPurchaseID == "System.Data.DataRowView")
                {
                    MessageBox.Show("Invalid or empty Purchase ID. Please select a valid option.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Debug.WriteLine("Invalid Purchase ID detected.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(newWarehouseID))
                {
                    MessageBox.Show("Warehouse ID is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Step 3: Validate and Parse Item IDs and Quantities
                Debug.WriteLine("Validating Item IDs and Quantities...");
                string[] itemIDs = txtItemIDs.Lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
                string[] itemQuantities = txtItemQtys.Lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

                if (itemIDs.Length != itemQuantities.Length)
                {
                    MessageBox.Show("Mismatch between Item IDs and Quantities. Please check your inputs.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                List<int> parsedQuantities = new List<int>();
                foreach (string qty in itemQuantities)
                {
                    if (int.TryParse(qty.Trim(), out int quantity))
                    {
                        parsedQuantities.Add(quantity);
                    }
                    else
                    {
                        MessageBox.Show($"Invalid quantity value: '{qty}'. Please enter valid numeric quantities.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Debug.WriteLine($"Invalid quantity detected: {qty}");
                        return;
                    }
                }

                Debug.WriteLine($"Parsed Item IDs: {string.Join(",", itemIDs)}");
                Debug.WriteLine($"Parsed Quantities: {string.Join(",", parsedQuantities)}");

                // Step 4: Update Database
                Debug.WriteLine("Connecting to database for update...");
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Update GRN record
                            Debug.WriteLine("Updating GRN record...");
                            string updateGRNQuery = @"
                    UPDATE tblGRN
                    SET PurchaseID = @PurchaseID,
                        PurchaseType = @PurchaseType,
                        SupplierID = @SupplierID,
                        ItemID = @ItemIDs,
                        ItemQuantity = @ItemQuantities,
                        WarehouseID = @WarehouseID,
                        GRN_Date = GETDATE(),
                        GRN_Type = @GRN_Type
                    WHERE GRN_ID = @GRN_ID";

                            using (SqlCommand updateCmd = new SqlCommand(updateGRNQuery, conn, transaction))
                            {
                                updateCmd.Parameters.AddWithValue("@GRN_ID", txtGRN_ID.Text);
                                updateCmd.Parameters.AddWithValue("@PurchaseID", newPurchaseID);
                                updateCmd.Parameters.AddWithValue("@PurchaseType", newPurchaseType);
                                updateCmd.Parameters.AddWithValue("@SupplierID", txtSupplierID.Text);
                                updateCmd.Parameters.AddWithValue("@ItemIDs", string.Join(",", itemIDs));
                                updateCmd.Parameters.AddWithValue("@ItemQuantities", string.Join(",", parsedQuantities));
                                updateCmd.Parameters.AddWithValue("@WarehouseID", newWarehouseID);
                                updateCmd.Parameters.AddWithValue("@GRN_Type", newGRNType ?? string.Empty);

                                updateCmd.ExecuteNonQuery();
                                Debug.WriteLine("GRN record updated successfully.");
                            }

                            // Update GIN Status if necessary
                            Debug.WriteLine("Updating GIN status if applicable...");
                            if (newPurchaseType == "G")
                            {
                                string updateGINStatusQuery = "UPDATE tblGIN SET Status = 'Y' WHERE GIN_ID = @GIN_ID";
                                using (SqlCommand updateGINCmd = new SqlCommand(updateGINStatusQuery, conn, transaction))
                                {
                                    updateGINCmd.Parameters.AddWithValue("@GIN_ID", newPurchaseID);
                                    updateGINCmd.ExecuteNonQuery();
                                    Debug.WriteLine("GIN status updated to 'Y'.");
                                }
                            }
                            else
                            {
                                string resetGINStatusQuery = "UPDATE tblGIN SET Status = 'N' WHERE GIN_ID = @GIN_ID";
                                using (SqlCommand resetGINCmd = new SqlCommand(resetGINStatusQuery, conn, transaction))
                                {
                                    resetGINCmd.Parameters.AddWithValue("@GIN_ID", newPurchaseID);
                                    resetGINCmd.ExecuteNonQuery();
                                    Debug.WriteLine("GIN status reset to 'N'.");
                                }
                            }

                            // Commit transaction
                            transaction.Commit();
                            MessageBox.Show("GRN updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Clear fields and reload
                            ClearFields();
                            LoadNextGRNID();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Debug.WriteLine($"Transaction rolled back due to error: {ex.Message}");
                            MessageBox.Show($"An error occurred during the update: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected error: {ex.Message}");
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("btnClear_Click method called 🐞");

            // Step 1: Clear all fields and reset radio buttons
            Debug.WriteLine("Calling ClearFields to reset form fields...");
            ClearFields();
            Debug.WriteLine("ClearFields method executed successfully ✅");

            // Step 2: Reload the next GRN ID for new entries
            Debug.WriteLine("Calling LoadNextGRNID to generate the next available GRN ID...");
            LoadNextGRNID();
            Debug.WriteLine("LoadNextGRNID executed successfully. GRN ID ready for new entry ✅");
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine("txtSearch_KeyDown method called 🐞");

            if (e.KeyCode == Keys.Enter)
            {
                Debug.WriteLine("Enter key pressed. Starting search operation...");

                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    MessageBox.Show("Please enter a GRN ID to search.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Debug.WriteLine("Search aborted: GRN ID is empty.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        Debug.WriteLine("Opening database connection...");
                        conn.Open();
                        Debug.WriteLine("Database connection opened successfully ✅");

                        string query = "SELECT * FROM tblGRN WHERE GRN_ID = @GRN_ID";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@GRN_ID", txtSearch.Text.Trim());

                        Debug.WriteLine($"Executing query: {query} with GRN_ID = {txtSearch.Text.Trim()}");
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            Debug.WriteLine("Record found. Populating fields...");

                            // Populate fields from tblGRN
                            txtGRN_ID.Text = reader["GRN_ID"].ToString();
                            Debug.WriteLine($"GRN_ID loaded: {txtGRN_ID.Text}");

                            string purchaseId = reader["PurchaseID"].ToString();
                            txtSupplierID.Text = reader["SupplierID"].ToString();
                            Debug.WriteLine($"PurchaseID loaded: {purchaseId}, SupplierID loaded: {txtSupplierID.Text}");

                            // Convert comma-separated IDs/Qtys to newline-separated
                            txtItemIDs.Text = reader["ItemID"].ToString().Replace(",", Environment.NewLine);
                            txtItemQtys.Text = reader["ItemQuantity"].ToString().Replace(",", Environment.NewLine);
                            Debug.WriteLine($"ItemIDs loaded: {txtItemIDs.Text.Replace(Environment.NewLine, ", ")}, Quantities loaded: {txtItemQtys.Text.Replace(Environment.NewLine, ", ")}");

                            cmbWarehouseID.Text = reader["WarehouseID"].ToString();
                            cmbGRN_Type.Text = reader["GRN_Type"].ToString();
                            Debug.WriteLine($"WarehouseID loaded: {cmbWarehouseID.Text}, GRN_Type loaded: {cmbGRN_Type.Text}");

                            // Temporarily mark that we're changing the radio in code
                            isProgrammaticRadioChange = true;
                            try
                            {
                                string purchaseType = reader["PurchaseType"].ToString();
                                Debug.WriteLine($"PurchaseType loaded: {purchaseType}");

                                if (purchaseType == "O")
                                {
                                    rbPurchaseOrder.Checked = true;
                                    txtItemQtys.ReadOnly = true;
                                    txtItemQtys.BackColor = Color.BurlyWood;
                                    Debug.WriteLine("Purchase Type set to 'O' (Purchase Order). ItemQtys set to read-only with BurlyWood background.");
                                }
                                else if (purchaseType == "C")
                                {
                                    rbPurchaseContract.Checked = true;
                                    txtItemQtys.ReadOnly = false;
                                    txtItemQtys.BackColor = SystemColors.Control;
                                    Debug.WriteLine("Purchase Type set to 'C' (Purchase Contract). ItemQtys set to editable with default background.");
                                }
                                else if (purchaseType == "G")
                                {
                                    rbGIN.Checked = true;
                                    txtItemQtys.ReadOnly = true;
                                    txtItemQtys.BackColor = Color.BurlyWood;
                                    Debug.WriteLine("Purchase Type set to 'G' (GIN). ItemQtys set to read-only with BurlyWood background.");
                                }
                            }
                            finally
                            {
                                // Revert this after setting the radio button
                                isProgrammaticRadioChange = false;
                                Debug.WriteLine("isProgrammaticRadioChange reverted to false.");
                            }

                            // Set the PurchaseID combo text
                            cmbPurchaseOrGIN_ID.Text = purchaseId;
                            Debug.WriteLine($"Purchase/GIN ID combo box updated with value: {cmbPurchaseOrGIN_ID.Text}");

                            // Call the relevant loading method to get SupplierName & ItemNames
                            if (rbPurchaseOrder.Checked)
                            {
                                Debug.WriteLine("Calling LoadPurchaseOrderDetails...");
                                LoadPurchaseOrderDetails(purchaseId);
                            }
                            else if (rbPurchaseContract.Checked)
                            {
                                Debug.WriteLine("Calling LoadPurchaseContractDetails...");
                                LoadPurchaseContractDetails(purchaseId);
                            }
                            else if (rbGIN.Checked)
                            {
                                Debug.WriteLine("Calling LoadSelectedGINDetails...");
                                if (int.TryParse(purchaseId, out int ginID))
                                {
                                    LoadSelectedGINDetails(ginID);
                                }
                                else
                                {
                                    Debug.WriteLine($"GIN ID '{purchaseId}' is not a valid integer. Skipping GIN details load.");
                                }
                            }
                        }
                        else
                        {
                            Debug.WriteLine("No record found for the entered GRN ID.");
                            MessageBox.Show("No record found for the entered GRN ID.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error during search operation: {ex.Message}");
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        Debug.WriteLine("Closing database connection...");
                        conn.Close();
                        Debug.WriteLine("Database connection closed ✅");
                    }
                }
            }
        }
    }
}
