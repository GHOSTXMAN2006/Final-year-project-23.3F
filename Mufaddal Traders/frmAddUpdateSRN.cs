using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics; // <-- For Debug.WriteLine
using System.Linq;


namespace Mufaddal_Traders
{
    public partial class frmAddUpdateSRN : Form
    {
        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private string connectionString = DatabaseConfig.ConnectionString;

        public frmAddUpdateSRN()
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

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Console.WriteLine("DEBUG: Enter key pressed. Searching for: " + txtSearch.Text);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        Console.WriteLine("DEBUG: Database connection opened successfully.");

                        string searchQuery = @"
                SELECT SRN_ID, SupplierID, DiscardedGoodID, ItemID, ItemQty, Status
                FROM tblSRN
                WHERE CAST(SRN_ID AS NVARCHAR) = @SearchText";

                        SqlCommand cmd = new SqlCommand(searchQuery, conn);
                        cmd.Parameters.AddWithValue("@SearchText", txtSearch.Text.Trim());
                        Console.WriteLine($"DEBUG: Search parameter value: {txtSearch.Text.Trim()}");

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            txtSRN_ID.Text = reader["SRN_ID"].ToString();
                            int supplierID = Convert.ToInt32(reader["SupplierID"]);
                            int discardedGoodID = reader["DiscardedGoodID"] != DBNull.Value ? Convert.ToInt32(reader["DiscardedGoodID"]) : -1;

                            Console.WriteLine($"DEBUG: Setting SupplierID: {supplierID}");
                            Console.WriteLine($"DEBUG: Setting DiscardedGoodID: {discardedGoodID}");

                            // Set SupplierID
                            cmbSupplierID.SelectedValue = supplierID;
                            cmbSupplierID.BindingContext = new BindingContext(); // Refresh ComboBox to avoid stale data
                            txtItemID.Text = reader["ItemID"].ToString();
                            txtQty.Text = reader["ItemQty"].ToString();

                            // Load discarded goods list and include the current DiscardedGoodID (if exists)
                            if (discardedGoodID != -1)
                            {
                                LoadDiscardedGoods(discardedGoodID);  // Populate warehouse and discarded goods fields
                            }
                            else
                            {
                                LoadDiscardedGoods();  // Load only available discarded goods if no specific ID is linked
                                cmbDiscardedGoodID.SelectedIndex = -1;
                                txtWarehouseID.Text = string.Empty;
                            }
                        }
                        else
                        {
                            ClearFields();
                            MessageBox.Show("No matching records found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error searching for SRN record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                            conn.Close();
                    }
                }
            }
        }

        private void LoadDiscardedGoods(int? selectedDiscardedGoodID = null)
        {
            Console.WriteLine("DEBUG: Entering LoadDiscardedGoods method...");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    Console.WriteLine("DEBUG: Attempting to open database connection...");
                    conn.Open();
                    Console.WriteLine("DEBUG: Database connection opened successfully.");

                    // SQL query to load available discarded goods and the current linked one
                    string query = @"
            SELECT DiscardedGoodsID, 
                   CASE WHEN Status = '-' THEN 'Available' ELSE 'Assigned to SRN' END AS StatusText
            FROM tblDiscardedGoods
            WHERE Status = '-' OR DiscardedGoodsID = @SelectedDiscardedGoodID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SelectedDiscardedGoodID", selectedDiscardedGoodID ?? (object)DBNull.Value);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    Console.WriteLine($"DEBUG: DataTable filled with {dt.Rows.Count} row(s).");

                    if (dt.Rows.Count > 0)
                    {
                        cmbDiscardedGoodID.DataSource = dt;
                        cmbDiscardedGoodID.DisplayMember = "DiscardedGoodsID";
                        cmbDiscardedGoodID.ValueMember = "DiscardedGoodsID";
                        cmbDiscardedGoodID.BindingContext = new BindingContext();  // Force refresh

                        if (selectedDiscardedGoodID.HasValue)
                        {
                            cmbDiscardedGoodID.SelectedValue = selectedDiscardedGoodID;
                            Console.WriteLine($"DEBUG: Selected DiscardedGoodID set to: {selectedDiscardedGoodID}");
                        }
                        else
                        {
                            cmbDiscardedGoodID.SelectedIndex = -1;
                        }
                    }
                    else
                    {
                        Console.WriteLine("WARNING: No discarded goods found.");
                        MessageBox.Show("No discarded goods available.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmbDiscardedGoodID.DataSource = null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: {ex.Message}");
                    MessageBox.Show("Error loading discarded goods: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        Console.WriteLine("DEBUG: Database connection closed.");
                    }
                }
            }

            Console.WriteLine("DEBUG: Exiting LoadDiscardedGoods method.");
        }

        private void frmAddUpdateSRN_Load(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("DEBUG: Entering frmAddUpdateSRN_Load");

                Console.WriteLine("DEBUG: Attempting to load the next available SRN ID...");
                LoadNextSRNID();
                Console.WriteLine("DEBUG: Successfully loaded the next available SRN ID.");

                Console.WriteLine("DEBUG: Attempting to load supplier IDs...");
                LoadSupplierIDs();
                Console.WriteLine("DEBUG: Successfully loaded supplier IDs.");

                Console.WriteLine("DEBUG: Attempting to load discarded goods IDs...");
                LoadDiscardedGoods();
                Console.WriteLine("DEBUG: Successfully loaded discarded goods IDs.");

                Console.WriteLine("DEBUG: Exiting frmAddUpdateSRN_Load with success.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: Exception occurred in frmAddUpdateSRN_Load.");
                Console.WriteLine($"ERROR: Message: {ex.Message}");
                Console.WriteLine($"ERROR: StackTrace: {ex.StackTrace}");

                MessageBox.Show("An error occurred while loading the form: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Console.WriteLine("DEBUG: Entering btnSave_Click method...");

            if (string.IsNullOrWhiteSpace(txtSRN_ID.Text) || string.IsNullOrWhiteSpace(cmbSupplierID.Text) ||
                string.IsNullOrWhiteSpace(cmbDiscardedGoodID.Text) || string.IsNullOrWhiteSpace(txtItemID.Text) ||
                string.IsNullOrWhiteSpace(txtQty.Text))
            {
                MessageBox.Show("Please fill all the required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Console.WriteLine("WARNING: Validation failed - some fields are empty.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    Console.WriteLine("DEBUG: Attempting to open database connection...");
                    conn.Open();
                    Console.WriteLine("DEBUG: Database connection opened successfully.");

                    transaction = conn.BeginTransaction();
                    Console.WriteLine("DEBUG: SQL Transaction started.");

                    string enableIdentityInsertQuery = "SET IDENTITY_INSERT tblSRN ON";
                    SqlCommand enableCmd = new SqlCommand(enableIdentityInsertQuery, conn, transaction);
                    enableCmd.ExecuteNonQuery();
                    Console.WriteLine("DEBUG: IDENTITY_INSERT enabled for tblSRN.");

                    // SQL Query to insert SRN record
                    string insertQuery = @"
                INSERT INTO tblSRN (SRN_ID, SupplierID, DiscardedGoodID, ItemID, ItemQty, Status)
                VALUES (@SRN_ID, @SupplierID, @DiscardedGoodID, @ItemID, @ItemQty, 'N')";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn, transaction);
                    insertCmd.Parameters.AddWithValue("@SRN_ID", Convert.ToInt32(txtSRN_ID.Text));
                    insertCmd.Parameters.AddWithValue("@SupplierID", Convert.ToInt32(cmbSupplierID.SelectedValue));
                    insertCmd.Parameters.AddWithValue("@DiscardedGoodID", Convert.ToInt32(cmbDiscardedGoodID.SelectedValue));
                    insertCmd.Parameters.AddWithValue("@ItemID", Convert.ToInt32(txtItemID.Text));
                    insertCmd.Parameters.AddWithValue("@ItemQty", Convert.ToInt32(txtQty.Text));
                    Console.WriteLine($"DEBUG: SQL Insert Query prepared: {insertQuery}");

                    int rowsAffected = insertCmd.ExecuteNonQuery();
                    Console.WriteLine($"DEBUG: Insert query executed successfully. Rows affected: {rowsAffected}");

                    // SQL Query to update the status of the discarded good
                    string updateStatusQuery = @"
                UPDATE tblDiscardedGoods
                SET Status = 'R'
                WHERE DiscardedGoodsID = @DiscardedGoodID";
                    SqlCommand updateCmd = new SqlCommand(updateStatusQuery, conn, transaction);
                    updateCmd.Parameters.AddWithValue("@DiscardedGoodID", Convert.ToInt32(cmbDiscardedGoodID.SelectedValue));
                    Console.WriteLine($"DEBUG: SQL Update Query prepared: {updateStatusQuery}");

                    rowsAffected = updateCmd.ExecuteNonQuery();
                    Console.WriteLine($"DEBUG: Update query executed successfully. Rows affected: {rowsAffected}");

                    string disableIdentityInsertQuery = "SET IDENTITY_INSERT tblSRN OFF";
                    SqlCommand disableCmd = new SqlCommand(disableIdentityInsertQuery, conn, transaction);
                    disableCmd.ExecuteNonQuery();
                    Console.WriteLine("DEBUG: IDENTITY_INSERT disabled for tblSRN.");

                    transaction.Commit();
                    Console.WriteLine("DEBUG: SQL Transaction committed successfully.");

                    MessageBox.Show("SRN record saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear fields after successful save
                    ClearFields();
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("ERROR: SQL Exception occurred in btnSave_Click.");
                    Console.WriteLine($"ERROR: Message: {sqlEx.Message}");
                    Console.WriteLine($"ERROR: StackTrace: {sqlEx.StackTrace}");
                    Console.WriteLine($"ERROR: Error Number: {sqlEx.Number}");

                    transaction?.Rollback();
                    Console.WriteLine("DEBUG: Transaction rolled back due to an error.");
                    MessageBox.Show("Error saving SRN record (Database issue): " + sqlEx.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR: General Exception occurred in btnSave_Click.");
                    Console.WriteLine($"ERROR: Message: {ex.Message}");
                    Console.WriteLine($"ERROR: StackTrace: {ex.StackTrace}");

                    transaction?.Rollback();
                    Console.WriteLine("DEBUG: Transaction rolled back due to an error.");
                    MessageBox.Show("Error saving SRN record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        Console.WriteLine("DEBUG: Database connection closed.");
                    }
                }

            }

            Console.WriteLine("DEBUG: Exiting btnSave_Click method.");
        }

        private void ClearFields()
        {
            Console.WriteLine("DEBUG: Entering ClearFields method...");

            txtSRN_ID.Text = string.Empty;
            cmbSupplierID.SelectedIndex = -1;
            txtSupplierName.Text = string.Empty;
            cmbDiscardedGoodID.SelectedIndex = -1;
            txtWarehouseID.Text = string.Empty;
            txtItemID.Text = string.Empty;
            txtQty.Text = string.Empty;

            Console.WriteLine("DEBUG: All fields cleared.");
            Console.WriteLine("DEBUG: Exiting ClearFields method.");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Console.WriteLine("DEBUG: Entering btnUpdate_Click method...");

            if (string.IsNullOrWhiteSpace(txtSRN_ID.Text) || string.IsNullOrWhiteSpace(cmbSupplierID.Text) ||
                string.IsNullOrWhiteSpace(cmbDiscardedGoodID.Text) || string.IsNullOrWhiteSpace(txtItemID.Text) ||
                string.IsNullOrWhiteSpace(txtQty.Text))
            {
                MessageBox.Show("Please fill all the required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Console.WriteLine("WARNING: Validation failed - some fields are empty.");
                return;
            }

            int currentDiscardedGoodID = Convert.ToInt32(cmbDiscardedGoodID.SelectedValue);
            int oldDiscardedGoodID = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    Console.WriteLine("DEBUG: Attempting to open database connection...");
                    conn.Open();
                    Console.WriteLine("DEBUG: Database connection opened successfully.");

                    transaction = conn.BeginTransaction();
                    Console.WriteLine("DEBUG: SQL Transaction started.");

                    // Get the old DiscardedGoodID for comparison
                    string getOldDiscardedGoodIDQuery = @"
                SELECT DiscardedGoodID
                FROM tblSRN
                WHERE SRN_ID = @SRN_ID";
                    SqlCommand getOldCmd = new SqlCommand(getOldDiscardedGoodIDQuery, conn, transaction);
                    getOldCmd.Parameters.AddWithValue("@SRN_ID", Convert.ToInt32(txtSRN_ID.Text));
                    object oldDiscardedGoodIDObj = getOldCmd.ExecuteScalar();
                    oldDiscardedGoodID = oldDiscardedGoodIDObj != null ? Convert.ToInt32(oldDiscardedGoodIDObj) : -1;

                    Console.WriteLine($"DEBUG: Old DiscardedGoodID: {oldDiscardedGoodID}, New DiscardedGoodID: {currentDiscardedGoodID}");

                    // Update SRN record
                    string updateQuery = @"
                UPDATE tblSRN
                SET SupplierID = @SupplierID, DiscardedGoodID = @DiscardedGoodID, ItemID = @ItemID, ItemQty = @ItemQty
                WHERE SRN_ID = @SRN_ID";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, conn, transaction);
                    updateCmd.Parameters.AddWithValue("@SRN_ID", Convert.ToInt32(txtSRN_ID.Text));
                    updateCmd.Parameters.AddWithValue("@SupplierID", Convert.ToInt32(cmbSupplierID.SelectedValue));
                    updateCmd.Parameters.AddWithValue("@DiscardedGoodID", currentDiscardedGoodID);
                    updateCmd.Parameters.AddWithValue("@ItemID", Convert.ToInt32(txtItemID.Text));
                    updateCmd.Parameters.AddWithValue("@ItemQty", Convert.ToInt32(txtQty.Text));
                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    Console.WriteLine($"DEBUG: Update query executed successfully. Rows affected: {rowsAffected}");

                    // If the discarded good has changed, update their statuses
                    if (oldDiscardedGoodID != currentDiscardedGoodID)
                    {
                        Console.WriteLine("DEBUG: DiscardedGoodID changed. Updating statuses...");

                        // Set old discarded good status to available
                        if (oldDiscardedGoodID > 0)
                        {
                            string resetStatusQuery = @"
                        UPDATE tblDiscardedGoods
                        SET Status = '-'
                        WHERE DiscardedGoodsID = @OldDiscardedGoodID";
                            SqlCommand resetCmd = new SqlCommand(resetStatusQuery, conn, transaction);
                            resetCmd.Parameters.AddWithValue("@OldDiscardedGoodID", oldDiscardedGoodID);
                            resetCmd.ExecuteNonQuery();
                            Console.WriteLine($"DEBUG: Old DiscardedGoodID {oldDiscardedGoodID} status set to '-' (available).");
                        }

                        // Set new discarded good status to returned
                        string setStatusQuery = @"
                    UPDATE tblDiscardedGoods
                    SET Status = 'R'
                    WHERE DiscardedGoodsID = @NewDiscardedGoodID";
                        SqlCommand setStatusCmd = new SqlCommand(setStatusQuery, conn, transaction);
                        setStatusCmd.Parameters.AddWithValue("@NewDiscardedGoodID", currentDiscardedGoodID);
                        setStatusCmd.ExecuteNonQuery();
                        Console.WriteLine($"DEBUG: New DiscardedGoodID {currentDiscardedGoodID} status set to 'R' (returned).");
                    }

                    transaction.Commit();
                    Console.WriteLine("DEBUG: SQL Transaction committed successfully.");

                    MessageBox.Show("SRN record updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear fields after successful update
                    ClearFields();
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("ERROR: SQL Exception occurred in btnUpdate_Click.");
                    Console.WriteLine($"ERROR: Message: {sqlEx.Message}");
                    Console.WriteLine($"ERROR: StackTrace: {sqlEx.StackTrace}");
                    Console.WriteLine($"ERROR: Error Number: {sqlEx.Number}");

                    transaction?.Rollback();
                    Console.WriteLine("DEBUG: Transaction rolled back due to an error.");
                    MessageBox.Show("Error updating SRN record (Database issue): " + sqlEx.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR: General Exception occurred in btnUpdate_Click.");
                    Console.WriteLine($"ERROR: Message: {ex.Message}");
                    Console.WriteLine($"ERROR: StackTrace: {ex.StackTrace}");

                    transaction?.Rollback();
                    Console.WriteLine("DEBUG: Transaction rolled back due to an error.");
                    MessageBox.Show("Error updating SRN record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        Console.WriteLine("DEBUG: Database connection closed.");
                    }
                }
            }

            Console.WriteLine("DEBUG: Exiting btnUpdate_Click method.");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Console.WriteLine("DEBUG: Entering btnClear_Click method...");

            try
            {
                // Load only the next SRN ID, no other lists
                LoadNextSRNID();

                // Clear text fields
                txtSupplierName.Clear();
                txtItemID.Clear();
                txtQty.Clear();
                txtSearch.Clear();
                txtWarehouseID.Clear();

                // Reset ComboBox selections without reloading data
                cmbSupplierID.SelectedIndex = -1;
                cmbDiscardedGoodID.SelectedIndex = -1;

                // Set focus to SRN ID
                txtSRN_ID.Focus();
                Console.WriteLine("DEBUG: Form cleared successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Exception occurred in btnClear_Click. Message: {ex.Message}");
                MessageBox.Show("Error while clearing the form: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Console.WriteLine("DEBUG: Exiting btnClear_Click method.");
        }

        private void LoadNextSRNID()
        {
            Console.WriteLine("DEBUG: Entering LoadNextSRNID method...");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    Console.WriteLine("DEBUG: Attempting to open database connection...");
                    conn.Open();
                    Console.WriteLine("DEBUG: Database connection opened successfully.");

                    string query = @"
            SELECT TOP 1 Number
            FROM (
                SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Number
                FROM master.dbo.spt_values
            ) AS Numbers
            WHERE Number NOT IN (SELECT SRN_ID FROM tblSRN)
            ORDER BY Number";

                    Console.WriteLine("DEBUG: SQL Query prepared.");
                    Console.WriteLine($"DEBUG: SQL Query: {query}");

                    SqlCommand cmd = new SqlCommand(query, conn);
                    Console.WriteLine("DEBUG: SqlCommand object created successfully.");

                    Console.WriteLine("DEBUG: Executing SQL query to fetch the next SRN ID...");
                    object result = cmd.ExecuteScalar();
                    Console.WriteLine($"DEBUG: Query execution complete. Result: {(result != null ? result.ToString() : "NULL")}");

                    if (result != null)
                    {
                        txtSRN_ID.Text = result.ToString();
                        Console.WriteLine($"DEBUG: txtSRN_ID set to: {txtSRN_ID.Text}");
                    }
                    else
                    {
                        txtSRN_ID.Text = "1";
                        Console.WriteLine("DEBUG: No available SRN_ID found. Defaulting txtSRN_ID to 1.");
                    }
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("ERROR: SQL Exception occurred in LoadNextSRNID.");
                    Console.WriteLine($"ERROR: Message: {sqlEx.Message}");
                    Console.WriteLine($"ERROR: StackTrace: {sqlEx.StackTrace}");
                    Console.WriteLine($"ERROR: Error Number: {sqlEx.Number}");
                    MessageBox.Show("Error loading SRN ID (Database issue): " + sqlEx.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR: General Exception occurred in LoadNextSRNID.");
                    Console.WriteLine($"ERROR: Message: {ex.Message}");
                    Console.WriteLine($"ERROR: StackTrace: {ex.StackTrace}");
                    MessageBox.Show("Error loading SRN ID: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        Console.WriteLine("DEBUG: Database connection closed.");
                    }
                    else
                    {
                        Console.WriteLine("DEBUG: Connection was not open, no need to close.");
                    }
                }
            }

            Console.WriteLine("DEBUG: Exiting LoadNextSRNID method.");
        }

        private void LoadSupplierIDs()
        {
            Console.WriteLine("DEBUG: Entering LoadSupplierIDs method...");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    Console.WriteLine("DEBUG: Attempting to open database connection...");
                    conn.Open();
                    Console.WriteLine("DEBUG: Database connection opened successfully.");

                    string query = "SELECT SupplierID, Name FROM tblManageSuppliers";
                    Console.WriteLine($"DEBUG: SQL Query prepared: {query}");

                    SqlCommand cmd = new SqlCommand(query, conn);
                    Console.WriteLine("DEBUG: SqlCommand object created.");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    Console.WriteLine("DEBUG: SqlDataAdapter object created.");

                    DataTable dt = new DataTable();
                    Console.WriteLine("DEBUG: DataTable object created.");

                    Console.WriteLine("DEBUG: Filling DataTable with data...");
                    da.Fill(dt);
                    Console.WriteLine($"DEBUG: DataTable filled with {dt.Rows.Count} row(s).");

                    if (dt.Rows.Count > 0)
                    {
                        Console.WriteLine("DEBUG: Setting cmbSupplierID DataSource.");
                        cmbSupplierID.DataSource = dt;
                        cmbSupplierID.DisplayMember = "SupplierID";
                        cmbSupplierID.ValueMember = "SupplierID";
                        Console.WriteLine("DEBUG: ComboBox cmbSupplierID populated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("WARNING: No SupplierIDs found in the database.");
                        MessageBox.Show("No Supplier IDs found. Please add suppliers to the database.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmbSupplierID.DataSource = null;
                    }
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("ERROR: SQL Exception occurred in LoadSupplierIDs.");
                    Console.WriteLine($"ERROR: Message: {sqlEx.Message}");
                    Console.WriteLine($"ERROR: StackTrace: {sqlEx.StackTrace}");
                    Console.WriteLine($"ERROR: Error Number: {sqlEx.Number}");
                    MessageBox.Show("Error loading Supplier IDs (Database issue): " + sqlEx.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR: General Exception occurred in LoadSupplierIDs.");
                    Console.WriteLine($"ERROR: Message: {ex.Message}");
                    Console.WriteLine($"ERROR: StackTrace: {ex.StackTrace}");
                    MessageBox.Show("Error loading Supplier IDs: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        Console.WriteLine("DEBUG: Database connection closed.");
                    }
                    else
                    {
                        Console.WriteLine("DEBUG: Connection was not open, no need to close.");
                    }
                }
            }

            Console.WriteLine("DEBUG: Exiting LoadSupplierIDs method.");
        }

        private void LoadDiscardedGoods()
        {
            Console.WriteLine("DEBUG: Entering LoadDiscardedGoods method...");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    Console.WriteLine("DEBUG: Attempting to open database connection...");
                    conn.Open();
                    Console.WriteLine("DEBUG: Database connection opened successfully.");

                    string query = "SELECT DiscardedGoodsID FROM tblDiscardedGoods WHERE Status = '-'";
                    Console.WriteLine($"DEBUG: SQL Query prepared: {query}");

                    SqlCommand cmd = new SqlCommand(query, conn);
                    Console.WriteLine("DEBUG: SqlCommand object created.");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    Console.WriteLine("DEBUG: SqlDataAdapter object created.");

                    DataTable dt = new DataTable();
                    Console.WriteLine("DEBUG: DataTable object created.");

                    Console.WriteLine("DEBUG: Filling DataTable with data...");
                    da.Fill(dt);
                    Console.WriteLine($"DEBUG: DataTable filled with {dt.Rows.Count} row(s).");

                    if (dt.Rows.Count > 0)
                    {
                        Console.WriteLine("DEBUG: Setting cmbDiscardedGoodID DataSource.");
                        cmbDiscardedGoodID.DataSource = dt;
                        cmbDiscardedGoodID.DisplayMember = "DiscardedGoodsID";
                        cmbDiscardedGoodID.ValueMember = "DiscardedGoodsID";
                        Console.WriteLine("DEBUG: ComboBox cmbDiscardedGoodID populated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("WARNING: No Discarded Goods found with Status = '-'.");
                        MessageBox.Show("No Discarded Goods found with status '-'. Please ensure data exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmbDiscardedGoodID.DataSource = null;
                    }
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("ERROR: SQL Exception occurred in LoadDiscardedGoods.");
                    Console.WriteLine($"ERROR: Message: {sqlEx.Message}");
                    Console.WriteLine($"ERROR: StackTrace: {sqlEx.StackTrace}");
                    Console.WriteLine($"ERROR: Error Number: {sqlEx.Number}");
                    MessageBox.Show("Error loading Discarded Goods (Database issue): " + sqlEx.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR: General Exception occurred in LoadDiscardedGoods.");
                    Console.WriteLine($"ERROR: Message: {ex.Message}");
                    Console.WriteLine($"ERROR: StackTrace: {ex.StackTrace}");
                    MessageBox.Show("Error loading Discarded Goods: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        Console.WriteLine("DEBUG: Database connection closed.");
                    }
                    else
                    {
                        Console.WriteLine("DEBUG: Connection was not open, no need to close.");
                    }
                }
            }

            Console.WriteLine("DEBUG: Exiting LoadDiscardedGoods method.");
        }

        private void cmbSupplierID_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("DEBUG: Entering cmbSupplierID_SelectedIndexChanged method...");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    Console.WriteLine("DEBUG: Attempting to open database connection...");
                    conn.Open();
                    Console.WriteLine("DEBUG: Database connection opened successfully.");

                    if (cmbSupplierID.SelectedValue == null || cmbSupplierID.SelectedValue is DataRowView)
                    {
                        Console.WriteLine("WARNING: cmbSupplierID.SelectedValue is null or DataRowView. Exiting method early.");
                        txtSupplierName.Text = string.Empty;
                        return;
                    }

                    int supplierID = Convert.ToInt32(cmbSupplierID.SelectedValue);
                    Console.WriteLine($"DEBUG: Selected SupplierID: {supplierID}");

                    string query = "SELECT Name FROM tblManageSuppliers WHERE SupplierID = @SupplierID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SupplierID", supplierID);

                    Console.WriteLine("DEBUG: Executing SQL query...");
                    object result = cmd.ExecuteScalar();
                    Console.WriteLine($"DEBUG: Query execution result: {(result != null ? result.ToString() : "NULL")}");

                    txtSupplierName.Text = result?.ToString() ?? string.Empty;
                    Console.WriteLine($"DEBUG: txtSupplierName set to: {txtSupplierName.Text}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR: Exception occurred in cmbSupplierID_SelectedIndexChanged.");
                    Console.WriteLine($"ERROR: Message: {ex.Message}");
                    MessageBox.Show("Error loading Supplier Name: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        Console.WriteLine("DEBUG: Database connection closed.");
                    }
                }
            }

            Console.WriteLine("DEBUG: Exiting cmbSupplierID_SelectedIndexChanged method.");
        }

        private void cmbDiscardedGoodID_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("DEBUG: Entering cmbDiscardedGoodID_SelectedIndexChanged method...");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    Console.WriteLine("DEBUG: Attempting to open database connection...");
                    conn.Open();
                    Console.WriteLine("DEBUG: Database connection opened successfully.");

                    if (cmbDiscardedGoodID.SelectedValue == null || cmbDiscardedGoodID.SelectedValue is DataRowView)
                    {
                        Console.WriteLine("WARNING: cmbDiscardedGoodID.SelectedValue is null or DataRowView. Exiting method early.");
                        txtWarehouseID.Text = string.Empty;
                        txtItemID.Text = string.Empty;
                        txtQty.Text = string.Empty;
                        return;
                    }

                    int discardedGoodsID = Convert.ToInt32(cmbDiscardedGoodID.SelectedValue);
                    Console.WriteLine($"DEBUG: Selected DiscardedGoodsID: {discardedGoodsID}");

                    string query = @"
                SELECT WarehouseID, ItemID, ItemQty
                FROM tblDiscardedGoods
                WHERE DiscardedGoodsID = @DiscardedGoodsID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@DiscardedGoodsID", discardedGoodsID);

                    Console.WriteLine("DEBUG: Executing SQL query...");
                    SqlDataReader reader = cmd.ExecuteReader();
                    Console.WriteLine("DEBUG: Query executed, reader object created.");

                    if (reader.Read())
                    {
                        txtWarehouseID.Text = reader["WarehouseID"].ToString();
                        txtItemID.Text = reader["ItemID"].ToString();
                        txtQty.Text = reader["ItemQty"].ToString();
                        Console.WriteLine("DEBUG: Fields set with discarded goods data.");
                    }
                    else
                    {
                        Console.WriteLine("WARNING: No data found for the selected DiscardedGoodsID.");
                        txtWarehouseID.Text = string.Empty;
                        txtItemID.Text = string.Empty;
                        txtQty.Text = string.Empty;
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR: Exception occurred in cmbDiscardedGoodID_SelectedIndexChanged.");
                    Console.WriteLine($"ERROR: Message: {ex.Message}");
                    MessageBox.Show("Error loading Discarded Good details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        Console.WriteLine("DEBUG: Database connection closed.");
                    }
                }
            }

            Console.WriteLine("DEBUG: Exiting cmbDiscardedGoodID_SelectedIndexChanged method.");
        }

    }
}
