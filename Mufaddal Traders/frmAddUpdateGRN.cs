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

        private void LoadPurchaseGIN_ID(string type, string selectedID = null)
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
WHERE Status = 'N'
OR GIN_ID IN (
    SELECT PurchaseID
    FROM tblGRN
    WHERE PurchaseType = 'G' AND GRN_ID = @CurrentGRN_ID
)";

                        if (!string.IsNullOrEmpty(selectedID))
                        {
                            query += " OR GIN_ID = @SelectedID";
                        }
                    }
                    else
                    {
                        Debug.WriteLine($"Invalid type provided: {type}");
                        throw new ArgumentException("Invalid type provided to LoadPurchaseGIN_ID.");
                    }

                    Debug.WriteLine($"Query prepared: {query}");

                    SqlCommand cmd = new SqlCommand(query, conn);

                    if (type == "G")
                    {
                        // Add Current GRN_ID for reference
                        int currentGRN_ID = string.IsNullOrWhiteSpace(txtGRN_ID.Text) ? 0 : Convert.ToInt32(txtGRN_ID.Text);
                        cmd.Parameters.AddWithValue("@CurrentGRN_ID", currentGRN_ID);
                        Debug.WriteLine($"Parameter added: @CurrentGRN_ID = {currentGRN_ID}");

                        if (!string.IsNullOrEmpty(selectedID))
                        {
                            cmd.Parameters.AddWithValue("@SelectedID", selectedID);
                            Debug.WriteLine($"Parameter added: @SelectedID = {selectedID}");
                        }
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable data = new DataTable();

                    Debug.WriteLine("Executing query to fetch data...");
                    adapter.Fill(data);
                    Debug.WriteLine($"Query executed successfully ✅ Rows fetched: {data.Rows.Count}");

                    // Update ComboBox with fetched data
                    cmbPurchaseOrGIN_ID.DataSource = null;
                    cmbPurchaseOrGIN_ID.DisplayMember = "ID";
                    cmbPurchaseOrGIN_ID.ValueMember = "ID";
                    cmbPurchaseOrGIN_ID.DataSource = data;

                    cmbPurchaseOrGIN_ID.SelectedIndex = -1;
                    if (!string.IsNullOrEmpty(selectedID))
                    {
                        cmbPurchaseOrGIN_ID.SelectedValue = selectedID;
                        Debug.WriteLine($"cmbPurchaseOrGIN_ID selected value set to: {selectedID}");
                    }

                    Debug.WriteLine("Data bound to cmbPurchaseOrGIN_ID successfully ✅");
                }
                catch (SqlException sqlEx)
                {
                    Debug.WriteLine($"SQL EXCEPTION: {sqlEx.Message}");
                    MessageBox.Show($"Database error while loading GIN IDs: {sqlEx.Message}", "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"EXCEPTION: {ex.Message}");
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

                    string query = "SELECT StoreID, Store_Name FROM Warehouse";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    cmbWarehouseID.DisplayMember = "StoreID"; // Show Warehouse IDs in the dropdown
                    cmbWarehouseID.ValueMember = "StoreID";
                    cmbWarehouseID.DataSource = dt;

                    Debug.WriteLine("Warehouse IDs loaded into cmbWarehouseID successfully ✅");
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

            string selectedWarehouseID = cmbWarehouseID.SelectedValue?.ToString();
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
                    Debug.WriteLine($"Error in cmbWarehouseID_SelectedIndexChanged: {ex.Message} ❌");
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
                    if (cmbWarehouseID.SelectedValue == null || !(cmbWarehouseID.SelectedValue is int))
                    {
                        MessageBox.Show("Please select a valid Warehouse ID.",
                                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Debug.WriteLine("Warehouse ID is invalid or not selected 🚫. Exiting btnSave_Click.");
                        return;
                    }
                    int warehouseID = (int)cmbWarehouseID.SelectedValue;
                    Debug.WriteLine($"Selected WarehouseID: {warehouseID}");

                    // Step 5: Validate Selected Purchase/GIN ID
                    if (cmbPurchaseOrGIN_ID.SelectedValue == null)
                    {
                        MessageBox.Show("Please select a valid Purchase/GIN ID.",
                                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Debug.WriteLine("Purchase/GIN ID is invalid or not selected 🚫. Exiting btnSave_Click.");
                        return;
                    }
                    string purchaseOrGIN_ID = cmbPurchaseOrGIN_ID.SelectedValue.ToString();
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
                                // Log the old stock value before updating
                                Debug.WriteLine($"Fetching old stock for ItemID: {itemIDs[i]} in Warehouse: {warehouseID}...");
                                string fetchOldStockQuery = @"
        SELECT ItemQty 
        FROM tblStockBalance 
        WHERE ItemID = @ItemID AND WarehouseID = @WarehouseID";
                                int oldStock = 0;
                                using (SqlCommand fetchCmd = new SqlCommand(fetchOldStockQuery, conn, transaction))
                                {
                                    fetchCmd.Parameters.AddWithValue("@ItemID", itemIDs[i]);
                                    fetchCmd.Parameters.AddWithValue("@WarehouseID", warehouseID);
                                    object result = fetchCmd.ExecuteScalar();
                                    if (result != null)
                                    {
                                        oldStock = Convert.ToInt32(result);
                                        Debug.WriteLine($"Old stock for ItemID {itemIDs[i]}: {oldStock}");
                                    }
                                    else
                                    {
                                        Debug.WriteLine($"No existing stock record found for ItemID {itemIDs[i]} in Warehouse {warehouseID}. Assuming 0.");
                                    }
                                }

                                // Update stock balance
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

                                // Log the new stock value after updating
                                Debug.WriteLine($"Fetching new stock for ItemID: {itemIDs[i]} in Warehouse: {warehouseID}...");
                                using (SqlCommand verifyCmd = new SqlCommand(fetchOldStockQuery, conn, transaction))
                                {
                                    verifyCmd.Parameters.AddWithValue("@ItemID", itemIDs[i]);
                                    verifyCmd.Parameters.AddWithValue("@WarehouseID", warehouseID);
                                    object updatedResult = verifyCmd.ExecuteScalar();
                                    if (updatedResult != null)
                                    {
                                        int newStock = Convert.ToInt32(updatedResult);
                                        Debug.WriteLine($"New stock for ItemID {itemIDs[i]}: {newStock} (Old: {oldStock}, Added: {itemQuantities[i]})");
                                    }
                                    else
                                    {
                                        Debug.WriteLine($"Failed to fetch new stock for ItemID {itemIDs[i]} in Warehouse {warehouseID}. Possible issue.");
                                    }
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
            if (cmbWarehouseID.SelectedValue == null || !(cmbWarehouseID.SelectedValue is int))
            {
                Debug.WriteLine("Validation failed 🚫: Warehouse ID is missing or invalid.");
                MessageBox.Show("Please select a valid Warehouse ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            Debug.WriteLine($"Validation passed ✅: Warehouse ID = {cmbWarehouseID.SelectedValue}");

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














        private bool ValidateUpdateInputs()
        {
            Debug.WriteLine("DEBUG: ValidateUpdateInputs method invoked 🔍");

            // Validate GRN ID
            if (string.IsNullOrWhiteSpace(txtGRN_ID.Text))
            {
                MessageBox.Show("Error: GRN ID cannot be empty. Please search for a valid GRN.");
                Debug.WriteLine("ERROR: GRN ID is empty.");
                return false;
            }

            // Validate Purchase Type (Radio Buttons)
            if (!rbPurchaseOrder.Checked && !rbPurchaseContract.Checked && !rbGIN.Checked)
            {
                MessageBox.Show("Error: Please select a Purchase Type (Purchase Order, Contract, or GIN).");
                Debug.WriteLine("ERROR: No Purchase Type selected.");
                return false;
            }

            // Validate Purchase/GIN ID
            if (string.IsNullOrWhiteSpace(cmbPurchaseOrGIN_ID.Text))
            {
                MessageBox.Show("Error: Please select a valid Purchase Order/Contract/GIN ID.");
                Debug.WriteLine("ERROR: Purchase Order/Contract/GIN ID is missing.");
                return false;
            }

            // Validate Supplier ID and Supplier Name
            if (string.IsNullOrWhiteSpace(txtSupplierID.Text) || string.IsNullOrWhiteSpace(txtSupplierName.Text))
            {
                MessageBox.Show("Error: Supplier information is incomplete.");
                Debug.WriteLine("ERROR: Supplier ID or Name is empty.");
                return false;
            }

            // Validate Item IDs and Quantities (multiline textboxes)
            if (string.IsNullOrWhiteSpace(txtItemIDs.Text) || string.IsNullOrWhiteSpace(txtItemQtys.Text))
            {
                MessageBox.Show("Error: Item IDs and Quantities cannot be empty.");
                Debug.WriteLine("ERROR: Item IDs or Quantities are empty.");
                return false;
            }

            // Split Item IDs and Quantities by newline
            string[] itemIDs = txtItemIDs.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            string[] itemQuantities = txtItemQtys.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            if (itemIDs.Length != itemQuantities.Length)
            {
                MessageBox.Show("Error: The number of Item IDs and Quantities do not match.");
                Debug.WriteLine("ERROR: Mismatch in Item IDs and Quantities count.");
                return false;
            }

            // Validate individual quantities
            for (int i = 0; i < itemQuantities.Length; i++)
            {
                string itemQty = itemQuantities[i].Trim();
                if (!int.TryParse(itemQty, out int parsedQty) || parsedQty < 0)
                {
                    MessageBox.Show($"Error: Invalid quantity '{itemQty}' for Item ID '{itemIDs[i]}'. Please enter valid numeric values.");
                    Debug.WriteLine($"ERROR: Invalid quantity value detected - '{itemQty}' for Item ID '{itemIDs[i]}'.");
                    return false;
                }
            }

            // Validate Warehouse ID
            if (string.IsNullOrWhiteSpace(cmbWarehouseID.Text))
            {
                MessageBox.Show("Error: Please select a Warehouse.");
                Debug.WriteLine("ERROR: Warehouse not selected.");
                return false;
            }

            // Validate GRN Type
            if (cmbGRN_Type.SelectedIndex == -1)
            {
                MessageBox.Show("Error: Please select a GRN Type.");
                Debug.WriteLine("ERROR: GRN Type not selected.");
                return false;
            }

            Debug.WriteLine("DEBUG: All validations passed ✅.");
            return true;
        }

        private DataRow FetchExistingGRNData(int grnID)
        {
            Debug.WriteLine($"DEBUG: FetchExistingGRNData method invoked for GRN ID: {grnID} 🔍");

            DataRow existingData = null;
            string query = "SELECT * FROM tblGRN WHERE GRN_ID = @GRN_ID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    Debug.WriteLine("DEBUG: Database connection opened successfully.");

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@GRN_ID", grnID);
                        Debug.WriteLine($"DEBUG: SQL query prepared with GRN ID: {grnID}");

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            Debug.WriteLine($"DEBUG: SQL query executed. Rows returned: {dataTable.Rows.Count}");

                            if (dataTable.Rows.Count > 0)
                            {
                                existingData = dataTable.Rows[0];
                                Debug.WriteLine("DEBUG: Existing GRN data found.");
                            }
                            else
                            {
                                Debug.WriteLine("WARNING: No GRN data found for the provided GRN ID.");
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Database error while fetching GRN data: " + sqlEx.Message);
                Debug.WriteLine("SQL EXCEPTION: " + sqlEx.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred while fetching GRN data: " + ex.Message);
                Debug.WriteLine("EXCEPTION: " + ex.ToString());
            }

            return existingData;
        }

        private bool RollbackStockBalances(DataRow existingGRNData)
        {
            Debug.WriteLine("DEBUG: RollbackStockBalances method invoked 🔄");

            string[] itemIDs = existingGRNData["ItemID"].ToString().Split(',');
            string[] itemQuantities = existingGRNData["ItemQuantity"].ToString().Split(',');
            int warehouseID = Convert.ToInt32(existingGRNData["WarehouseID"]);

            if (itemIDs.Length != itemQuantities.Length)
            {
                Debug.WriteLine("ERROR: Mismatch in the number of Item IDs and Quantities in the existing GRN data.");
                MessageBox.Show("Error: Mismatch in Item IDs and Quantities in the existing GRN data. Rollback failed.");
                return false;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    Debug.WriteLine("DEBUG: Database connection opened successfully for rollback.");

                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        for (int i = 0; i < itemIDs.Length; i++)
                        {
                            int itemID = Convert.ToInt32(itemIDs[i].Trim());
                            int quantityToRevert = Convert.ToInt32(itemQuantities[i].Trim());

                            int currentStockQty = GetCurrentStockQty(itemID, warehouseID, conn, transaction);
                            int resultingStockQty = currentStockQty - quantityToRevert;

                            // 🚨 Check for negative stock values BEFORE rollback
                            if (resultingStockQty < 0)
                            {
                                string errorMessage = $"Error: Rollback would cause negative stock for Item ID '{itemID}' in Warehouse ID '{warehouseID}'.\n\n" +
                                                      $"Current Stock: {currentStockQty}, Quantity to Revert: {quantityToRevert}, Resulting Stock: {resultingStockQty}.";
                                MessageBox.Show(errorMessage);
                                Debug.WriteLine($"ERROR: {errorMessage}");
                                transaction.Rollback();  // Roll back the transaction immediately
                                return false;
                            }

                            string rollbackQuery = @"
                    UPDATE tblStockBalance
                    SET ItemQty = ItemQty - @Quantity
                    WHERE ItemID = @ItemID AND WarehouseID = @WarehouseID";

                            using (SqlCommand cmd = new SqlCommand(rollbackQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@Quantity", quantityToRevert);
                                cmd.Parameters.AddWithValue("@ItemID", itemID);
                                cmd.Parameters.AddWithValue("@WarehouseID", warehouseID);

                                int rowsAffected = cmd.ExecuteNonQuery();
                                Debug.WriteLine($"DEBUG: Rollback for ItemID: {itemID}, Quantity to revert: {quantityToRevert}, Rows Affected: {rowsAffected}");

                                if (rowsAffected == 0)
                                {
                                    Debug.WriteLine($"ERROR: Rollback failed for ItemID: {itemID}. No matching stock balance found.");
                                    MessageBox.Show($"Error: Rollback failed for ItemID: {itemID}. No matching stock balance found.");
                                    transaction.Rollback();
                                    return false;
                                }
                            }
                        }

                        transaction.Commit();
                        Debug.WriteLine("DEBUG: Stock balances rollback committed successfully.");
                    }
                }

                return true;
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Database error during stock balance rollback: " + sqlEx.Message);
                Debug.WriteLine("SQL EXCEPTION: " + sqlEx.ToString());
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred during stock balance rollback: " + ex.Message);
                Debug.WriteLine("EXCEPTION: " + ex.ToString());
                return false;
            }
        }

        private bool UpdateGRNData()
        {
            Debug.WriteLine("DEBUG: UpdateGRNData method invoked ✏️");

            int grnID = Convert.ToInt32(txtGRN_ID.Text.Trim());
            string newPurchaseOrGINID = cmbPurchaseOrGIN_ID.Text.Trim();
            string newPurchaseType = rbPurchaseOrder.Checked ? "O" : rbPurchaseContract.Checked ? "C" : "G";

            DataRow existingGRNData = FetchExistingGRNData(grnID);
            if (existingGRNData == null)
            {
                MessageBox.Show("Error: GRN not found in the database.");
                return false;
            }

            string existingPurchaseType = existingGRNData["PurchaseType"].ToString().Trim();
            string existingPurchaseOrGINID = existingGRNData["PurchaseID"].ToString().Trim();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    Debug.WriteLine("DEBUG: Database connection opened for updating GRN.");

                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        // Update the GRN record
                        string updateGRNQuery = @"
                UPDATE tblGRN
                SET PurchaseID = @PurchaseOrGINID,
                    PurchaseType = @PurchaseType,
                    SupplierID = @SupplierID,
                    ItemID = @ItemIDs,
                    ItemQuantity = @ItemQuantities,
                    WarehouseID = @WarehouseID,
                    GRN_Date = @GRNDate,
                    GRN_Type = @GRNType
                WHERE GRN_ID = @GRN_ID";

                        using (SqlCommand cmd = new SqlCommand(updateGRNQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@PurchaseOrGINID", newPurchaseOrGINID);
                            cmd.Parameters.AddWithValue("@PurchaseType", newPurchaseType);
                            cmd.Parameters.AddWithValue("@SupplierID", Convert.ToInt32(txtSupplierID.Text.Trim()));
                            cmd.Parameters.AddWithValue("@ItemIDs", string.Join(",", txtItemIDs.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)));
                            cmd.Parameters.AddWithValue("@ItemQuantities", string.Join(",", txtItemQtys.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)));
                            cmd.Parameters.AddWithValue("@WarehouseID", Convert.ToInt32(cmbWarehouseID.Text.Trim()));
                            cmd.Parameters.AddWithValue("@GRNDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@GRNType", cmbGRN_Type.Text.Trim());
                            cmd.Parameters.AddWithValue("@GRN_ID", grnID);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                Debug.WriteLine("ERROR: No rows updated.");
                                transaction.Rollback();
                                return false;
                            }
                        }

                        // If the Purchase Type has changed, update the status in respective tables
                        if (existingPurchaseType != newPurchaseType || existingPurchaseOrGINID != newPurchaseOrGINID)
                        {
                            // Handle change to/from Purchase Order
                            if (existingPurchaseType == "O")
                            {
                                UpdatePurchaseOrderStatus(existingPurchaseOrGINID, "N", conn, transaction); // Reset old PO
                            }
                            if (newPurchaseType == "O")
                            {
                                UpdatePurchaseOrderStatus(newPurchaseOrGINID, "Y", conn, transaction); // Activate new PO
                            }

                            // Handle change to/from GIN
                            if (existingPurchaseType == "G")
                            {
                                UpdateGINStatus(existingPurchaseOrGINID, "N", conn, transaction); // Reset old GIN
                            }
                            if (newPurchaseType == "G")
                            {
                                UpdateGINStatus(newPurchaseOrGINID, "Y", conn, transaction); // Activate new GIN
                            }
                        }

                        transaction.Commit();
                        Debug.WriteLine("DEBUG: GRN record and related statuses updated successfully ✅.");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred during GRN update: " + ex.Message);
                Debug.WriteLine("EXCEPTION: " + ex.ToString());
                return false;
            }
        }

        private void UpdatePurchaseOrderStatus(string purchaseOrderID, string status, SqlConnection conn, SqlTransaction transaction)
        {
            if (!string.IsNullOrEmpty(purchaseOrderID))
            {
                string query = "UPDATE Purchase_Orders SET Status = @Status WHERE PurchaseOrderID = @PurchaseOrderID";
                using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@PurchaseOrderID", Convert.ToInt32(purchaseOrderID));
                    cmd.ExecuteNonQuery();
                    Debug.WriteLine($"DEBUG: Purchase Order status updated. ID: {purchaseOrderID}, Status: {status}");
                }
            }
        }

        private void UpdateGINStatus(string ginID, string status, SqlConnection conn, SqlTransaction transaction)
        {
            if (!string.IsNullOrEmpty(ginID))
            {
                string query = "UPDATE tblGIN SET Status = @Status WHERE GIN_ID = @GIN_ID";
                using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@GIN_ID", Convert.ToInt32(ginID));
                    cmd.ExecuteNonQuery();
                    Debug.WriteLine($"DEBUG: GIN status updated. ID: {ginID}, Status: {status}");
                }
            }
        }

        private bool UpdateStockBalances()
        {
            Debug.WriteLine("DEBUG: UpdateStockBalances method invoked 🔄");

            string[] itemIDs = txtItemIDs.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            string[] itemQuantities = txtItemQtys.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            int warehouseID = Convert.ToInt32(cmbWarehouseID.Text.Trim());
            string warehouseName = txtWarehouseName.Text.Trim();

            if (itemIDs.Length != itemQuantities.Length)
            {
                Debug.WriteLine("ERROR: Mismatch in the number of Item IDs and Quantities entered.");
                MessageBox.Show("Error: The number of Item IDs and Quantities do not match. Stock balance update failed.");
                return false;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    Debug.WriteLine("DEBUG: Database connection opened successfully for stock balance update.");

                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        for (int i = 0; i < itemIDs.Length; i++)
                        {
                            int itemID = Convert.ToInt32(itemIDs[i].Trim());
                            int quantityToAdd = Convert.ToInt32(itemQuantities[i].Trim());

                            // Check stock levels before inserting/updating
                            int currentStockQty = GetCurrentStockQty(itemID, warehouseID, conn, transaction);
                            int updatedStockQty = currentStockQty + quantityToAdd;

                            if (updatedStockQty < 0)
                            {
                                string errorMessage = $"Error: Stock update would cause negative stock for Item ID '{itemID}' in Warehouse '{warehouseName}'.\n\n" +
                                                      $"Current Stock: {currentStockQty}, Quantity to Add: {quantityToAdd}, Resulting Stock: {updatedStockQty}.";
                                MessageBox.Show(errorMessage);
                                Debug.WriteLine($"ERROR: {errorMessage}");
                                transaction.Rollback();  // Prevent committing changes
                                return false;
                            }

                            string updateQuery = @"
                    MERGE INTO tblStockBalance AS target
                    USING (SELECT @ItemID AS ItemID, @WarehouseID AS WarehouseID) AS source
                    ON target.ItemID = source.ItemID AND target.WarehouseID = source.WarehouseID
                    WHEN MATCHED THEN
                        UPDATE SET ItemQty = ItemQty + @Quantity
                    WHEN NOT MATCHED THEN
                        INSERT (ItemID, ItemName, ItemDescription, WarehouseID, WarehouseName, ItemQty)
                        VALUES (@ItemID, @ItemName, '', @WarehouseID, @WarehouseName, @Quantity);";

                            using (SqlCommand cmd = new SqlCommand(updateQuery, conn, transaction))
                            {
                                string itemName = GetItemName(itemID, conn, transaction);
                                cmd.Parameters.AddWithValue("@ItemID", itemID);
                                cmd.Parameters.AddWithValue("@Quantity", quantityToAdd);
                                cmd.Parameters.AddWithValue("@WarehouseID", warehouseID);
                                cmd.Parameters.AddWithValue("@WarehouseName", warehouseName);
                                cmd.Parameters.AddWithValue("@ItemName", itemName);

                                int rowsAffected = cmd.ExecuteNonQuery();
                                Debug.WriteLine($"DEBUG: Stock balance update for ItemID: {itemID}, Quantity: {quantityToAdd}. Rows Affected: {rowsAffected}");
                            }
                        }

                        transaction.Commit();
                        Debug.WriteLine("DEBUG: Stock balances updated and committed successfully ✅.");
                    }
                }

                return true;
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Database error during stock balance update: " + sqlEx.Message);
                Debug.WriteLine("SQL EXCEPTION: " + sqlEx.ToString());
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred during stock balance update: " + ex.Message);
                Debug.WriteLine("EXCEPTION: " + ex.ToString());
                return false;
            }
        }

        private int GetCurrentStockQty(int itemID, int warehouseID, SqlConnection conn, SqlTransaction transaction)
        {
            try
            {
                string query = "SELECT ItemQty FROM tblStockBalance WHERE ItemID = @ItemID AND WarehouseID = @WarehouseID";
                using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@ItemID", itemID);
                    cmd.Parameters.AddWithValue("@WarehouseID", warehouseID);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0; // Return 0 if no stock found
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR: Failed to fetch current stock for ItemID {itemID}: {ex.Message}");
                return 0; // Assume 0 if there's an issue
            }
        }
        private string GetItemName(int itemID, SqlConnection conn, SqlTransaction transaction)
        {
            try
            {
                string query = "SELECT Item_Name FROM Items WHERE ItemID = @ItemID";
                using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@ItemID", itemID);
                    object result = cmd.ExecuteScalar();
                    return result != null ? result.ToString() : "Unknown Item";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR: Failed to fetch item name for ItemID {itemID}: {ex.Message}");
                return "Unknown Item";
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                Debug.WriteLine("DEBUG: btnUpdate_Click method invoked 🕾");

                // Step 1: Validate input fields
                Debug.WriteLine("DEBUG: Starting validation...");
                if (!ValidateUpdateInputs())
                {
                    Debug.WriteLine("DEBUG: Input validation failed. Returning from btnUpdate_Click.");
                    return;
                }
                Debug.WriteLine("DEBUG: Input validation passed ✅.");

                // Step 2: Fetch existing GRN data for comparison
                Debug.WriteLine($"DEBUG: Fetching existing GRN data for GRN ID: {txtGRN_ID.Text}...");
                DataRow existingGRNData = FetchExistingGRNData(Convert.ToInt32(txtGRN_ID.Text));
                if (existingGRNData == null)
                {
                    MessageBox.Show("Error: GRN not found in the database. Please try again.");
                    Debug.WriteLine($"ERROR: No GRN data found for GRN ID: {txtGRN_ID.Text}.");
                    return;
                }
                Debug.WriteLine("DEBUG: Existing GRN data fetched successfully.");

                // Step 3: Compare existing and new data (detect changes)
                Debug.WriteLine("DEBUG: Comparing GRN data...");
                if (!FetchGRNDetailsForComparison(existingGRNData))
                {
                    MessageBox.Show("No changes detected. No update required.");
                    Debug.WriteLine("DEBUG: No changes detected in GRN data.");
                    return;
                }
                Debug.WriteLine("DEBUG: Changes detected, proceeding with update...");

                // Step 4: Rollback stock balances based on existing data
                Debug.WriteLine("DEBUG: Rolling back stock balances for existing data...");
                if (!RollbackStockBalances(existingGRNData))
                {
                    MessageBox.Show("Error: Failed to rollback stock balances. Please try again.");
                    Debug.WriteLine("ERROR: Rollback of stock balances failed.");
                    return;
                }
                Debug.WriteLine("DEBUG: Stock balances rolled back successfully.");

                // Step 5: Update GRN record
                Debug.WriteLine("DEBUG: Updating GRN record...");
                if (!UpdateGRNData())
                {
                    MessageBox.Show("Error: Failed to update GRN record.");
                    Debug.WriteLine("ERROR: GRN update failed.");
                    return;
                }
                Debug.WriteLine("DEBUG: GRN record updated successfully.");

                // Step 6: Update stock balances with new data
                Debug.WriteLine("DEBUG: Updating stock balances with new data...");
                if (!UpdateStockBalances())
                {
                    MessageBox.Show("Error: Failed to update stock balances.");
                    Debug.WriteLine("ERROR: Update of stock balances failed.");
                    return;
                }
                Debug.WriteLine("DEBUG: Stock balances updated successfully.");

                // Final Message
                MessageBox.Show("GRN updated successfully!");
                Debug.WriteLine("INFO: GRN update process completed successfully ✅.");

                // Clear the form after update
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message);
                Debug.WriteLine("EXCEPTION: " + ex.ToString());
            }
        }

        private bool FetchGRNDetailsForComparison(DataRow existingGRNData)
        {
            Debug.WriteLine("DEBUG: FetchGRNDetailsForComparison method invoked 🔍");

            // Extract existing GRN data
            string existingPurchaseOrGINID = existingGRNData["PurchaseID"].ToString().Trim();
            string existingPurchaseType = existingGRNData["PurchaseType"].ToString().Trim();
            string existingSupplierID = existingGRNData["SupplierID"].ToString().Trim();
            string existingItemIDs = existingGRNData["ItemID"].ToString().Trim();
            string existingItemQuantities = existingGRNData["ItemQuantity"].ToString().Trim();
            string existingWarehouseID = existingGRNData["WarehouseID"].ToString().Trim();
            string existingGRNType = existingGRNData["GRN_Type"].ToString().Trim();

            // Extract current data from the form
            string currentPurchaseOrGINID = cmbPurchaseOrGIN_ID.Text.Trim();
            string currentPurchaseType = rbPurchaseOrder.Checked ? "O" : rbPurchaseContract.Checked ? "C" : "G";
            string currentSupplierID = txtSupplierID.Text.Trim();
            string currentItemIDs = string.Join(",", txtItemIDs.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries));
            string currentItemQuantities = string.Join(",", txtItemQtys.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries));
            string currentWarehouseID = cmbWarehouseID.Text.Trim();
            string currentGRNType = cmbGRN_Type.Text.Trim();

            // Compare values
            if (existingPurchaseOrGINID != currentPurchaseOrGINID ||
                existingPurchaseType != currentPurchaseType ||
                existingSupplierID != currentSupplierID ||
                existingItemIDs != currentItemIDs ||
                existingItemQuantities != currentItemQuantities ||
                existingWarehouseID != currentWarehouseID ||
                existingGRNType != currentGRNType)
            {
                Debug.WriteLine("DEBUG: Changes detected in the GRN data. Update is required.");
                return true;
            }

            Debug.WriteLine("DEBUG: No changes detected in the GRN data. No update needed.");
            return false;
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

                if (!ValidateSearchInput())
                {
                    Debug.WriteLine("Search input validation failed 🚫. Exiting.");
                    return;
                }

                try
                {
                    var grnData = FetchGRNData(txtSearch.Text.Trim());

                    if (grnData != null)
                    {
                        PopulateGRNFields(grnData);
                        Debug.WriteLine("GRN data populated successfully ✅");
                    }
                    else
                    {
                        MessageBox.Show("No record found for the entered GRN ID.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Debug.WriteLine("No record found for the entered GRN ID.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error during search operation: {ex.Message}");
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateSearchInput()
        {
            Debug.WriteLine("ValidateSearchInput method called 🐞");
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                Debug.WriteLine("Validation failed: Search text is empty 🚫");
                MessageBox.Show("Please enter a GRN ID to search.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            Debug.WriteLine($"Validation passed ✅: Search text = {txtSearch.Text.Trim()}");
            return true;
        }

        private DataRow FetchGRNData(string grnId)
        {
            Debug.WriteLine($"FetchGRNData method called 🐞 with GRN_ID: {grnId}");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    Debug.WriteLine("Attempting to open SQL connection...");
                    conn.Open();
                    Debug.WriteLine("SQL connection opened successfully ✅");

                    string query = "SELECT * FROM tblGRN WHERE GRN_ID = @GRN_ID";
                    Debug.WriteLine($"SQL Query: {query}");

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@GRN_ID", grnId);
                        Debug.WriteLine($"Parameter added: @GRN_ID = {grnId}");

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            Debug.WriteLine("Initializing data adapter and data table...");
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            Debug.WriteLine($"Query executed. Rows fetched: {dt.Rows.Count}");

                            if (dt.Rows.Count > 0)
                            {
                                Debug.WriteLine("Data found ✅. Returning the first row.");
                                return dt.Rows[0];
                            }
                            else
                            {
                                Debug.WriteLine("No data found for the given GRN_ID 🚫.");
                                return null;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error during database operation: {ex.Message}");
                    throw; // Re-throwing exception for handling at a higher level
                }
                finally
                {
                    Debug.WriteLine("Closing SQL connection...");
                    conn.Close();
                    Debug.WriteLine("SQL connection closed ✅");
                }
            }
        }

        private void PopulateGRNFields(DataRow grnData)
        {
            Debug.WriteLine("PopulateGRNFields method called 🐞");

            try
            {
                // Populate GRN fields
                txtGRN_ID.Text = grnData["GRN_ID"].ToString();
                Debug.WriteLine($"txtGRN_ID populated: {txtGRN_ID.Text}");

                txtSupplierID.Text = grnData["SupplierID"].ToString();
                Debug.WriteLine($"txtSupplierID populated: {txtSupplierID.Text}");

                txtItemIDs.Text = grnData["ItemID"].ToString().Replace(",", Environment.NewLine);
                Debug.WriteLine($"txtItemIDs populated: {txtItemIDs.Text.Replace(Environment.NewLine, ", ")}");

                txtItemQtys.Text = grnData["ItemQuantity"].ToString().Replace(",", Environment.NewLine);
                Debug.WriteLine($"txtItemQtys populated: {txtItemQtys.Text.Replace(Environment.NewLine, ", ")}");

                cmbWarehouseID.Text = grnData["WarehouseID"].ToString();
                Debug.WriteLine($"cmbWarehouseID populated: {cmbWarehouseID.Text}");

                cmbGRN_Type.Text = grnData["GRN_Type"].ToString();
                Debug.WriteLine($"cmbGRN_Type populated: {cmbGRN_Type.Text}");

                // Handle radio button selection
                Debug.WriteLine("Setting purchase type based on PurchaseType value...");
                SetPurchaseType(grnData["PurchaseType"].ToString());
                Debug.WriteLine($"PurchaseType set successfully: {grnData["PurchaseType"]}");

                // Dynamically load cmbPurchaseOrGIN_ID based on the related type
                Debug.WriteLine("Loading cmbPurchaseOrGIN_ID...");
                string purchaseType = grnData["PurchaseType"].ToString();
                string relatedID = grnData["PurchaseID"].ToString();

                if (!string.IsNullOrWhiteSpace(relatedID))
                {
                    Debug.WriteLine($"Related ID detected: {relatedID}. Loading IDs into cmbPurchaseOrGIN_ID.");
                    LoadPurchaseGIN_ID(purchaseType);

                    // After loading all IDs, set the selected value to the current GRN's ID
                    cmbPurchaseOrGIN_ID.SelectedValue = relatedID;
                    Debug.WriteLine($"cmbPurchaseOrGIN_ID populated and set to: {relatedID}");
                }
                else
                {
                    Debug.WriteLine("No related ID found. cmbPurchaseOrGIN_ID will remain empty.");
                }

                // Populate ComboBox and Text fields for SupplierName and ItemNames
                Debug.WriteLine("Fetching and populating additional data...");
                var supplierName = FetchSupplierName(txtSupplierID.Text);
                txtSupplierName.Text = supplierName;
                Debug.WriteLine($"txtSupplierName populated: {txtSupplierName.Text}");

                var itemNames = FetchItemNames(txtItemIDs.Lines);
                txtItemNames.Text = string.Join(Environment.NewLine, itemNames);
                Debug.WriteLine($"txtItemNames populated: {string.Join(", ", itemNames)}");

                Debug.WriteLine("Fields populated successfully ✅");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while populating GRN fields: {ex.Message}");
                throw; // Re-throwing exception for higher-level handling
            }
            finally
            {
                Debug.WriteLine("Exiting PopulateGRNFields method ✅");
            }
        }

        private string FetchSupplierName(string supplierID)
        {
            Debug.WriteLine($"FetchSupplierName method called 🐞 with SupplierID: {supplierID}");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Name FROM tblManageSuppliers WHERE SupplierID = @SupplierID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SupplierID", supplierID);
                    var result = cmd.ExecuteScalar();
                    Debug.WriteLine(result != null
                        ? $"SupplierName fetched: {result.ToString()}"
                        : "No SupplierName found for the given SupplierID 🚫");
                    return result?.ToString() ?? string.Empty;
                }
            }
        }

        private IEnumerable<string> FetchItemNames(string[] itemIDs)
        {
            Debug.WriteLine($"FetchItemNames method called 🐞 with ItemIDs: {string.Join(", ", itemIDs)}");

            // Return empty if no item IDs provided
            if (itemIDs == null || itemIDs.Length == 0)
            {
                Debug.WriteLine("No ItemIDs provided. Returning an empty list.");
                return Enumerable.Empty<string>();
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    Debug.WriteLine("Attempting to open SQL connection...");
                    conn.Open();
                    Debug.WriteLine("SQL connection opened successfully ✅");

                    // Create parameterized query for safety (avoiding string concatenation for SQL injection)
                    string query = $"SELECT Item_Name FROM Items WHERE ItemID IN ({string.Join(", ", itemIDs.Select((id, index) => $"@ItemID{index}"))})";
                    Debug.WriteLine($"SQL Query: {query}");

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Add parameters for each ItemID
                        for (int i = 0; i < itemIDs.Length; i++)
                        {
                            cmd.Parameters.AddWithValue($"@ItemID{i}", itemIDs[i].Trim());
                        }

                        List<string> itemNames = new List<string>();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string itemName = reader["Item_Name"].ToString();
                                Debug.WriteLine($"Item_Name fetched: {itemName}");
                                itemNames.Add(itemName);
                            }
                        }
                        Debug.WriteLine($"Total Item_Names fetched: {itemNames.Count}");
                        return itemNames;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error during FetchItemNames: {ex.Message}");
                    MessageBox.Show($"Error fetching item names: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return Enumerable.Empty<string>(); // Return an empty list to avoid breaking the application
                }
                finally
                {
                    Debug.WriteLine("Closing SQL connection...");
                    conn.Close();
                    Debug.WriteLine("SQL connection closed ✅");
                }
            }
        }

        private void SetPurchaseType(string purchaseType)
        {
            Debug.WriteLine("SetPurchaseType method called 🐞");
            Debug.WriteLine($"Received PurchaseType: {purchaseType}");

            isProgrammaticRadioChange = true;
            try
            {
                switch (purchaseType)
                {
                    case "O":
                        Debug.WriteLine("Setting PurchaseType to 'O' (Purchase Order)");
                        rbPurchaseOrder.Checked = true;
                        txtItemQtys.ReadOnly = true;
                        txtItemQtys.BackColor = Color.BurlyWood;
                        Debug.WriteLine("PurchaseOrder radio button checked. ItemQtys set to read-only with BurlyWood background.");
                        break;

                    case "C":
                        Debug.WriteLine("Setting PurchaseType to 'C' (Purchase Contract)");
                        rbPurchaseContract.Checked = true;
                        txtItemQtys.ReadOnly = false;
                        txtItemQtys.BackColor = SystemColors.Control;
                        Debug.WriteLine("PurchaseContract radio button checked. ItemQtys set to editable with default background.");
                        break;

                    case "G":
                        Debug.WriteLine("Setting PurchaseType to 'G' (GIN)");
                        rbGIN.Checked = true;
                        txtItemQtys.ReadOnly = true;
                        txtItemQtys.BackColor = Color.BurlyWood;
                        Debug.WriteLine("GIN radio button checked. ItemQtys set to read-only with BurlyWood background.");
                        break;

                    default:
                        Debug.WriteLine($"Unknown PurchaseType: {purchaseType}. No action taken.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in SetPurchaseType: {ex.Message}");
                throw; // Re-throw exception for higher-level handling
            }
            finally
            {
                isProgrammaticRadioChange = false;
                Debug.WriteLine("isProgrammaticRadioChange reverted to false.");
                Debug.WriteLine("Exiting SetPurchaseType method ✅");
            }
        }

    }
}
