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
    public partial class frmAddUpdatePaymentVoucher : Form
    {

        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private string connectionString = DatabaseConfig.ConnectionString;

        public frmAddUpdatePaymentVoucher()
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

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Debug.WriteLine("==== txtSearch_KeyDown triggered ====");
                Debug.WriteLine($"Search Key Entered: {txtSearch.Text}");

                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    MessageBox.Show("Please enter a Payment ID to search.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int paymentID;
                if (!int.TryParse(txtSearch.Text.Trim(), out paymentID))
                {
                    MessageBox.Show("Invalid Payment ID format. Please enter a valid numeric Payment ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Debug.WriteLine($"Searching for PaymentID: {paymentID}");

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        Debug.WriteLine("Database connection opened successfully.");

                        string query = @"
                SELECT PaymentID, PurchaseType, PurchaseID, PurchaseDetails, PaymentAmount, Status
                FROM tblPaymentVoucher
                WHERE PaymentID = @PaymentID";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@PaymentID", paymentID);

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            // Populate fields with search results
                            txtPaymentID.Text = reader["PaymentID"].ToString();
                            string purchaseType = reader["PurchaseType"].ToString();
                            rbPurchaseOrder.Checked = purchaseType == "O";
                            rbPurchaseContract.Checked = purchaseType == "C";
                            cmbPurchaseID.SelectedValue = Convert.ToInt32(reader["PurchaseID"]);
                            txtDetails.Text = reader["PurchaseDetails"].ToString();
                            txtPrice.Text = $"Rs. {Convert.ToDecimal(reader["PaymentAmount"]):N2}";
                            cbStatus.Checked = reader["Status"].ToString() == "Y";

                            Debug.WriteLine("Search successful. Fields populated.");
                        }
                        else
                        {
                            MessageBox.Show("No payment voucher found with the entered Payment ID.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Debug.WriteLine("Search result: No record found.");
                            ClearFields();  // Clear the fields if no record is found
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error occurred during search: {ex.Message}");
                        MessageBox.Show($"An error occurred while searching. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                            Debug.WriteLine("Database connection closed.");
                        }
                    }
                }

                Debug.WriteLine("==== txtSearch_KeyDown completed ====");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("==== btnSave_Click triggered ====");
            Debug.WriteLine($"Form Timestamp: {DateTime.Now}");

            // Check for missing fields
            if (string.IsNullOrWhiteSpace(txtPaymentID.Text) ||
                cmbPurchaseID.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Please fill in all required fields before saving.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Debug.WriteLine("Save operation aborted: missing required fields.");
                return;
            }

            try
            {
                int paymentID = int.Parse(txtPaymentID.Text);
                string purchaseType = rbPurchaseOrder.Checked ? "O" : "C";
                int purchaseID = int.Parse(cmbPurchaseID.SelectedValue.ToString());
                string purchaseDetails = txtDetails.Text.Trim();
                decimal paymentAmount = decimal.Parse(txtPrice.Text.Replace("Rs.", "").Trim());  // Remove currency symbol and trim
                string status = cbStatus.Checked ? "Y" : "N";  // 'Y' if checked, otherwise 'N'

                Debug.WriteLine($"PaymentID: {paymentID}, PurchaseType: {purchaseType}, PurchaseID: {purchaseID}, PaymentAmount: {paymentAmount}, Status: {status}");

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    Debug.WriteLine("Database connection opened successfully.");

                    // Turn on IDENTITY_INSERT
                    string identityInsertOn = "SET IDENTITY_INSERT tblPaymentVoucher ON;";
                    using (SqlCommand cmdOn = new SqlCommand(identityInsertOn, conn))
                    {
                        cmdOn.ExecuteNonQuery();
                        Debug.WriteLine("IDENTITY_INSERT enabled.");
                    }

                    // Insert the record
                    string query = @"
            INSERT INTO tblPaymentVoucher (PaymentID, PurchaseType, PurchaseID, PurchaseDetails, PaymentAmount, Status)
            VALUES (@PaymentID, @PurchaseType, @PurchaseID, @PurchaseDetails, @PaymentAmount, @Status)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PaymentID", paymentID);
                        cmd.Parameters.AddWithValue("@PurchaseType", purchaseType);
                        cmd.Parameters.AddWithValue("@PurchaseID", purchaseID);
                        cmd.Parameters.AddWithValue("@PurchaseDetails", purchaseDetails);
                        cmd.Parameters.AddWithValue("@PaymentAmount", paymentAmount);
                        cmd.Parameters.AddWithValue("@Status", status);

                        Debug.WriteLine("Executing SQL INSERT command...");
                        int rowsAffected = cmd.ExecuteNonQuery();
                        Debug.WriteLine($"Rows affected: {rowsAffected}");

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Payment voucher saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Debug.WriteLine("Save successful. Clearing fields...");
                            ClearFields();  // Clear fields after successful save
                        }
                        else
                        {
                            MessageBox.Show("Failed to save payment voucher.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    // Turn off IDENTITY_INSERT
                    string identityInsertOff = "SET IDENTITY_INSERT tblPaymentVoucher OFF;";
                    using (SqlCommand cmdOff = new SqlCommand(identityInsertOff, conn))
                    {
                        cmdOff.ExecuteNonQuery();
                        Debug.WriteLine("IDENTITY_INSERT disabled.");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occurred during save: {ex.Message}");
                MessageBox.Show($"An error occurred while saving the payment voucher. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Debug.WriteLine("==== btnSave_Click completed ====");
        }

        private void ClearFields()
        {
            Debug.WriteLine("==== ClearFields triggered ====");

            // Clear all textboxes
            txtPaymentID.Clear();
            Debug.WriteLine("txtPaymentID cleared.");
            txtDetails.Clear();
            Debug.WriteLine("txtDetails cleared.");
            txtPrice.Clear();
            Debug.WriteLine("txtPrice cleared.");

            // Reset combobox
            cmbPurchaseID.SelectedIndex = -1;
            Debug.WriteLine("cmbPurchaseID selection cleared. SelectedIndex set to -1.");

            // Reset radio buttons
            rbPurchaseOrder.Checked = false;
            rbPurchaseContract.Checked = false;
            Debug.WriteLine("Radio buttons cleared (no selection).");

            // Reset status checkbox
            cbStatus.Checked = false;  // Equivalent to "N"
            Debug.WriteLine($"cbStatus checked set to: {cbStatus.Checked}");

            // Load the next available Payment ID
            LoadNextPaymentID();
            Debug.WriteLine($"New Payment ID after clearing: {txtPaymentID.Text}");

            Debug.WriteLine("==== ClearFields completed ====");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("==== btnUpdate_Click triggered ====");
            Debug.WriteLine($"Form Timestamp: {DateTime.Now}");

            if (string.IsNullOrWhiteSpace(txtPaymentID.Text) ||
                cmbPurchaseID.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Please fill in all required fields before updating.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Debug.WriteLine("Update operation aborted: missing required fields.");
                return;
            }

            try
            {
                int paymentID = int.Parse(txtPaymentID.Text);
                string purchaseType = rbPurchaseOrder.Checked ? "O" : "C";
                int purchaseID = int.Parse(cmbPurchaseID.SelectedValue.ToString());
                string purchaseDetails = txtDetails.Text.Trim();
                decimal paymentAmount = decimal.Parse(txtPrice.Text.Replace("Rs.", "").Trim());
                string status = cbStatus.Checked ? "Y" : "N";

                Debug.WriteLine($"Updating PaymentID: {paymentID}, PurchaseType: {purchaseType}, PurchaseID: {purchaseID}, PaymentAmount: {paymentAmount}, Status: {status}");

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    Debug.WriteLine("Database connection opened successfully.");

                    // Check if the payment voucher exists
                    string checkQuery = "SELECT COUNT(*) FROM tblPaymentVoucher WHERE PaymentID = @PaymentID";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@PaymentID", paymentID);
                        int exists = (int)checkCmd.ExecuteScalar();

                        if (exists == 0)
                        {
                            MessageBox.Show("No payment voucher found with the entered Payment ID. Update aborted.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Debug.WriteLine("Update aborted: Payment ID does not exist.");
                            return;
                        }
                    }

                    // Get the current values from the database for comparison
                    string getQuery = "SELECT PurchaseType, PurchaseID, PurchaseDetails, PaymentAmount, Status FROM tblPaymentVoucher WHERE PaymentID = @PaymentID";
                    using (SqlCommand getCmd = new SqlCommand(getQuery, conn))
                    {
                        getCmd.Parameters.AddWithValue("@PaymentID", paymentID);
                        SqlDataReader reader = getCmd.ExecuteReader();
                        if (reader.Read())
                        {
                            string currentPurchaseType = reader["PurchaseType"].ToString();
                            int currentPurchaseID = Convert.ToInt32(reader["PurchaseID"]);
                            string currentDetails = reader["PurchaseDetails"].ToString();
                            decimal currentAmount = Convert.ToDecimal(reader["PaymentAmount"]);
                            string currentStatus = reader["Status"].ToString();
                            reader.Close();

                            // Check if values have changed
                            if (currentPurchaseType == purchaseType &&
                                currentPurchaseID == purchaseID &&
                                currentDetails == purchaseDetails &&
                                currentAmount == paymentAmount &&
                                currentStatus == status)
                            {
                                MessageBox.Show("No changes detected. Data is already up to date.", "No Changes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Debug.WriteLine("Update aborted: No changes detected.");
                                return;
                            }
                        }
                    }

                    // SQL Update query
                    string query = @"
            UPDATE tblPaymentVoucher
            SET PurchaseType = @PurchaseType,
                PurchaseID = @PurchaseID,
                PurchaseDetails = @PurchaseDetails,
                PaymentAmount = @PaymentAmount,
                Status = @Status
            WHERE PaymentID = @PaymentID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PurchaseType", purchaseType);
                        cmd.Parameters.AddWithValue("@PurchaseID", purchaseID);
                        cmd.Parameters.AddWithValue("@PurchaseDetails", purchaseDetails);
                        cmd.Parameters.AddWithValue("@PaymentAmount", paymentAmount);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@PaymentID", paymentID);

                        Debug.WriteLine("Executing SQL UPDATE command...");
                        int rowsAffected = cmd.ExecuteNonQuery();
                        Debug.WriteLine($"Rows affected: {rowsAffected}");

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Payment voucher updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Debug.WriteLine("Update successful. Clearing fields...");
                            ClearFields();  // Clear fields after successful update
                        }
                        else
                        {
                            MessageBox.Show("Update failed. No rows were affected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Debug.WriteLine("Update failed: No rows were affected.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occurred during update: {ex.Message}");
                MessageBox.Show($"An error occurred while updating the payment voucher. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Debug.WriteLine("==== btnUpdate_Click completed ====");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("btnClear_Click triggered at: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            // Clearing Payment ID TextBox
            txtPaymentID.Clear();
            Debug.WriteLine("txtPaymentID cleared.");

            // Clearing Details TextBox
            txtDetails.Clear();
            Debug.WriteLine("txtDetails cleared.");

            // Clearing Price TextBox
            txtPrice.Clear();
            Debug.WriteLine("txtPrice cleared.");

            // Resetting Status CheckBox
            cbStatus.Text = "N"; // Reset status to "N"
            Debug.WriteLine($"cbStatus set to: {cbStatus.Text}");

            // Clearing Purchase ID ComboBox selection
            cmbPurchaseID.SelectedIndex = -1;
            Debug.WriteLine("cmbPurchaseID selection cleared. SelectedIndex set to -1.");

            // Calling LoadNextPaymentID to get the next available Payment ID
            Debug.WriteLine("Calling LoadNextPaymentID to generate new Payment ID...");
            LoadNextPaymentID();
            Debug.WriteLine($"New Payment ID after LoadNextPaymentID: {txtPaymentID.Text}");

            Debug.WriteLine("btnClear_Click completed.");
        }

        private void frmAddUpdatePaymentVoucher_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("==== frmAddUpdatePaymentVoucher_Load triggered ====");
            Debug.WriteLine("Form Load Timestamp: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            try
            {
                Debug.WriteLine("Step 1: Loading next Payment ID...");
                LoadNextPaymentID(); // Auto-generates the next Payment ID
                Debug.WriteLine($"Current Payment ID after LoadNextPaymentID(): {txtPaymentID.Text}");

                Debug.WriteLine("Step 2: Setting default status...");
                cbStatus.Checked = false; // Default status unchecked (equivalent to "N")
                Debug.WriteLine($"cbStatus default Checked state: {cbStatus.Checked}");

                Debug.WriteLine("Step 3: Clearing radio button selections...");
                rbPurchaseOrder.Checked = false; // No radio button selected by default
                rbPurchaseContract.Checked = false;

                Debug.WriteLine("Step 4: Resetting ComboBox data source...");
                cmbPurchaseID.DataSource = null; // Reset combo box data
                Debug.WriteLine("cmbPurchaseID.DataSource set to null (reset).");

                Debug.WriteLine("Step 5: Clearing text fields...");
                txtDetails.Clear();
                Debug.WriteLine("txtDetails cleared.");
                txtPrice.Clear();
                Debug.WriteLine("txtPrice cleared.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occurred during form load: {ex.Message}");
                MessageBox.Show($"An error occurred while loading the form. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Debug.WriteLine("==== frmAddUpdatePaymentVoucher_Load completed ====");
        }

        private void LoadNextPaymentID()
        {
            Debug.WriteLine("==== LoadNextPaymentID started ====");
            Debug.WriteLine("Connecting to the database...");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    Debug.WriteLine("Database connection opened successfully.");

                    string query = @"
                SELECT TOP 1 Number
                FROM (SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Number
                      FROM master.dbo.spt_values) AS Numbers
                WHERE Number NOT IN (SELECT PaymentID FROM tblPaymentVoucher)
                ORDER BY Number";
                    Debug.WriteLine($"SQL Query prepared: {query}");

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        Debug.WriteLine("Executing SQL query to get the next available Payment ID...");
                        object result = cmd.ExecuteScalar();
                        Debug.WriteLine($"Query executed. Result: {(result != null ? result.ToString() : "null")}");

                        // Set the next Payment ID or default to "1"
                        txtPaymentID.Text = result != null ? result.ToString() : "1";
                        Debug.WriteLine($"txtPaymentID.Text set to: {txtPaymentID.Text}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error occurred while loading the next Payment ID: {ex.Message}");
                    MessageBox.Show($"Failed to load the next Payment ID. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        Debug.WriteLine("Database connection closed.");
                    }
                }
            }

            Debug.WriteLine("==== LoadNextPaymentID completed ====");
        }

        private void rbPurchaseOrder_CheckedChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("==== rbPurchaseOrder_CheckedChanged triggered ====");
            Debug.WriteLine($"rbPurchaseOrder.Checked: {rbPurchaseOrder.Checked}");

            if (rbPurchaseOrder.Checked)
            {
                Debug.WriteLine("Loading Purchase Order IDs...");
                LoadPurchaseIDs("O"); // 'O' for Purchase Order
            }
            else
            {
                Debug.WriteLine("rbPurchaseOrder unchecked.");
            }

            Debug.WriteLine("==== rbPurchaseOrder_CheckedChanged completed ====");
        }

        private void rbPurchaseContract_CheckedChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("==== rbPurchaseContract_CheckedChanged triggered ====");
            Debug.WriteLine($"rbPurchaseContract.Checked: {rbPurchaseContract.Checked}");

            if (rbPurchaseContract.Checked)
            {
                Debug.WriteLine("Loading Purchase Contract IDs...");
                LoadPurchaseIDs("C"); // 'C' for Purchase Contract
            }
            else
            {
                Debug.WriteLine("rbPurchaseContract unchecked.");
            }

            Debug.WriteLine("==== rbPurchaseContract_CheckedChanged completed ====");
        }

        private void LoadPurchaseIDs(string purchaseType)
        {
            Debug.WriteLine("==== LoadPurchaseIDs started ====");
            Debug.WriteLine($"Purchase Type received: {purchaseType}");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    Debug.WriteLine("Connecting to the database...");
                    conn.Open();
                    Debug.WriteLine("Database connection opened successfully.");

                    string query = purchaseType == "O"
                        ? "SELECT DISTINCT PurchaseID FROM tblGRN WHERE PurchaseType = 'O'"
                        : "SELECT DISTINCT PurchaseID FROM tblGRN WHERE PurchaseType = 'C'";

                    Debug.WriteLine($"SQL Query: {query}");

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable data = new DataTable();
                    adapter.Fill(data);
                    Debug.WriteLine($"Number of records fetched: {data.Rows.Count}");

                    cmbPurchaseID.DataSource = data;
                    cmbPurchaseID.DisplayMember = "PurchaseID";
                    cmbPurchaseID.ValueMember = "PurchaseID";

                    if (data.Rows.Count > 0)
                    {
                        cmbPurchaseID.SelectedIndex = 0; // Automatically select the first item
                        Debug.WriteLine($"First PurchaseID auto-selected: {cmbPurchaseID.SelectedValue}");
                    }
                    else
                    {
                        cmbPurchaseID.SelectedIndex = -1; // No selection if no records found
                        Debug.WriteLine("No Purchase IDs available for the selected type.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error: {ex.Message}");
                    MessageBox.Show($"Failed to load Purchase IDs. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        Debug.WriteLine("Database connection closed.");
                    }
                }
            }

            Debug.WriteLine("==== LoadPurchaseIDs completed ====");
        }

        private void cmbPurchaseID_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("==== cmbPurchaseID_SelectedIndexChanged triggered ====");
            Debug.WriteLine($"Current SelectedIndex: {cmbPurchaseID.SelectedIndex}");
            Debug.WriteLine($"Current SelectedValue: {cmbPurchaseID.SelectedValue}");

            if (cmbPurchaseID.SelectedIndex == -1 || cmbPurchaseID.SelectedValue == null || cmbPurchaseID.SelectedValue is DataRowView)
            {
                Debug.WriteLine("Invalid selection detected. Exiting method.");
                return; // Ignore invalid selections or null values
            }

            string purchaseID = cmbPurchaseID.SelectedValue.ToString();
            Debug.WriteLine($"Selected PurchaseID: {purchaseID}");

            string purchaseType = rbPurchaseOrder.Checked ? "O" : "C"; // Determine purchase type
            Debug.WriteLine($"Determined PurchaseType: {purchaseType}");

            Debug.WriteLine("Calling LoadDetailsForGRN...");
            LoadDetailsForGRN(purchaseID, purchaseType);
            Debug.WriteLine("==== cmbPurchaseID_SelectedIndexChanged completed ====");
        }

        private void LoadDetailsForGRN(string purchaseID, string purchaseType)
        {
            Debug.WriteLine("==== LoadDetailsForGRN started ====");
            Debug.WriteLine($"PurchaseID received: {purchaseID}");
            Debug.WriteLine($"PurchaseType received: {purchaseType}");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    Debug.WriteLine("Connecting to the database...");
                    conn.Open();
                    Debug.WriteLine("Database connection opened successfully.");

                    string query = @"
                SELECT g.ItemID, g.ItemQuantity 
                FROM tblGRN g
                WHERE g.PurchaseID = @PurchaseID AND g.PurchaseType = @PurchaseType";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@PurchaseID", purchaseID);
                    cmd.Parameters.AddWithValue("@PurchaseType", purchaseType);
                    Debug.WriteLine("SQL parameters added to the query.");

                    SqlDataReader reader = cmd.ExecuteReader();
                    Debug.WriteLine("SQL query executed. Reading results...");

                    string itemIDList = "";
                    string itemQuantityList = "";
                    if (reader.Read())
                    {
                        itemIDList = reader["ItemID"].ToString();      // e.g., "2,9,13,14,15"
                        itemQuantityList = reader["ItemQuantity"].ToString();  // e.g., "12,1,2,3,4"
                    }
                    reader.Close();

                    if (string.IsNullOrEmpty(itemIDList) || string.IsNullOrEmpty(itemQuantityList))
                    {
                        Debug.WriteLine("No items found for the selected PurchaseID.");
                        MessageBox.Show("No items found for the selected Purchase ID. Please check the GRN records.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    string[] itemIDs = itemIDList.Split(',');
                    string[] itemQuantities = itemQuantityList.Split(',');

                    if (itemIDs.Length != itemQuantities.Length)
                    {
                        Debug.WriteLine("Mismatch between ItemID and ItemQuantity counts.");
                        MessageBox.Show("Data mismatch: Number of Item IDs and Quantities do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    StringBuilder detailsBuilder = new StringBuilder();
                    decimal totalPrice = 0;

                    for (int i = 0; i < itemIDs.Length; i++)
                    {
                        int itemID = Convert.ToInt32(itemIDs[i]);
                        int itemQuantity = Convert.ToInt32(itemQuantities[i]);

                        // Get item details from Items table
                        string itemQuery = "SELECT Item_Name, Item_Price FROM Items WHERE ItemID = @ItemID";
                        SqlCommand itemCmd = new SqlCommand(itemQuery, conn);
                        itemCmd.Parameters.AddWithValue("@ItemID", itemID);
                        SqlDataReader itemReader = itemCmd.ExecuteReader();

                        if (itemReader.Read())
                        {
                            string itemName = itemReader["Item_Name"].ToString();
                            decimal itemPrice = Convert.ToDecimal(itemReader["Item_Price"]);
                            decimal itemTotal = itemPrice * itemQuantity;

                            Debug.WriteLine($"Item {i + 1}: {itemName}, Quantity: {itemQuantity}, Price: Rs. {itemPrice}, Total: Rs. {itemTotal}");

                            detailsBuilder.AppendLine($"{itemName}: {itemQuantity} x Rs. {itemPrice:N2} = Rs. {itemTotal:N2}");
                            totalPrice += itemTotal;
                        }
                        itemReader.Close();
                    }

                    txtDetails.Text = detailsBuilder.ToString();
                    txtPrice.Text = $"Rs. {totalPrice:N2}";
                    Debug.WriteLine($"Details TextBox set to: \n{txtDetails.Text}");
                    Debug.WriteLine($"Total Price TextBox set to: {txtPrice.Text}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error occurred while loading details: {ex.Message}");
                    MessageBox.Show($"Failed to load details. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        Debug.WriteLine("Database connection closed.");
                    }
                }
            }

            Debug.WriteLine("==== LoadDetailsForGRN completed ====");
        }

        private void cbStatus_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("==== cbStatus_Click triggered ====");
            Debug.WriteLine($"Current cbStatus text before click: {cbStatus.Text}");

            // Prevent the text change when the CheckBox is clicked
            cbStatus.Checked = !cbStatus.Checked; // Toggles check without affecting the text
            Debug.WriteLine($"cbStatus checked state toggled: {cbStatus.Checked}");
            Debug.WriteLine("cbStatus text remains unchanged.");
        }

    }
}
