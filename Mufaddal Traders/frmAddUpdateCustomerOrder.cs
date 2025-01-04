using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmAddUpdateCustomerOrder : Form
    {
        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private string connectionString = DatabaseConfig.ConnectionString;

        public frmAddUpdateCustomerOrder()
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

        private void frmAddUpdateCustomerOrder_Load(object sender, EventArgs e)
        {
            LoadOrderID(); // Load the next available Order ID
            LoadItemIDs(); // Load all Item IDs into combo box
            LoadCustomerIDs(); // Load all Customer IDs into combo box
        }

        private void LoadOrderID()
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
                    WHERE Number NOT IN (SELECT CustomerOrderID FROM tblCustomerOrders)
                    ORDER BY Number";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    txtOrderID.Text = result != null ? result.ToString() : "1";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Order ID: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadItemIDs()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT ItemID FROM Items";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cmbItemID.DisplayMember = "ItemID";
                    cmbItemID.ValueMember = "ItemID";
                    cmbItemID.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Item IDs: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCustomerIDs()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT CustomerID FROM tblCustomers";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cmbCustomerID.DisplayMember = "CustomerID";
                    cmbCustomerID.ValueMember = "CustomerID";
                    cmbCustomerID.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Customer IDs: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbItemID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbItemID.SelectedValue != null)
            {
                int selectedItemID = Convert.ToInt32(cmbItemID.SelectedValue);
                LoadItemName(selectedItemID);
            }
        }

        private void LoadItemName(int itemID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Item_Name FROM Items WHERE ItemID = @ItemID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ItemID", itemID);
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    txtItemName.Text = result != null ? result.ToString() : string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Item Name: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbCustomerID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCustomerID.SelectedValue != null)
            {
                int selectedCustomerID = Convert.ToInt32(cmbCustomerID.SelectedValue);
                LoadCustomerName(selectedCustomerID);
            }
        }

        private void LoadCustomerName(int customerID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Name FROM tblCustomers WHERE CustomerID = @CustomerID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CustomerID", customerID);
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

        private void ClearFields()
        {
            txtOrderID.Clear();
            cmbItemID.SelectedIndex = -1;
            txtItemName.Clear();
            cmbCustomerID.SelectedIndex = -1;
            txtCustomerName.Clear();
            txtItemQty.Clear();
            rbLocal.Checked = false;
            rbExport.Checked = false;
            LoadOrderID();
        }

        private string GetLocalOrExportValue()
        {
            if (rbLocal.Checked) return "L";
            if (rbExport.Checked) return "E";
            return null; // Return null if neither is selected
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    MessageBox.Show("Please enter a Customer Order ID to search.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int searchOrderID;
                if (!int.TryParse(txtSearch.Text, out searchOrderID))
                {
                    MessageBox.Show("Please enter a valid numeric Customer Order ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = @"
                    SELECT CustomerOrderID, ItemID, ItemName, ItemQty, CustomerID, OrderDate, Status, LocalOrExport
                    FROM tblCustomerOrders
                    WHERE CustomerOrderID = @CustomerOrderID";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@CustomerOrderID", searchOrderID);
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            txtOrderID.Text = reader["CustomerOrderID"].ToString();
                            cmbItemID.SelectedValue = Convert.ToInt32(reader["ItemID"]);
                            txtItemName.Text = reader["ItemName"].ToString();
                            txtItemQty.Text = reader["ItemQty"].ToString();
                            cmbCustomerID.SelectedValue = Convert.ToInt32(reader["CustomerID"]);
                            txtCustomerName.Text = GetCustomerNameByID(Convert.ToInt32(reader["CustomerID"]));
                            DateTime orderDate = Convert.ToDateTime(reader["OrderDate"]);
                            rbLocal.Checked = reader["LocalOrExport"].ToString() == "L";
                            rbExport.Checked = reader["LocalOrExport"].ToString() == "E";
                        }
                        else
                        {
                            MessageBox.Show($"No customer order found for ID: {searchOrderID}", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            ClearFields();
                        }

                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error searching for customer order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string GetCustomerNameByID(int customerID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Name FROM tblCustomers WHERE CustomerID = @CustomerID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CustomerID", customerID);
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    return result?.ToString() ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Customer Name: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOrderID.Text) || cmbItemID.SelectedValue == null || cmbCustomerID.SelectedValue == null)
            {
                MessageBox.Show("Please load an order before updating.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string localOrExport = GetLocalOrExportValue();
            if (localOrExport == null)
            {
                MessageBox.Show("Please select Local or Export option.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = conn,
                        Transaction = transaction,
                        CommandText = @"
                    UPDATE tblCustomerOrders
                    SET 
                        ItemID = @ItemID,
                        ItemName = @ItemName,
                        ItemQty = @ItemQty,
                        CustomerID = @CustomerID,
                        OrderDate = @OrderDate,
                        Status = @Status,
                        LocalOrExport = @LocalOrExport
                    WHERE CustomerOrderID = @CustomerOrderID;"
                    };

                    // Add parameters
                    cmd.Parameters.AddWithValue("@CustomerOrderID", Convert.ToInt32(txtOrderID.Text));
                    cmd.Parameters.AddWithValue("@ItemID", Convert.ToInt32(cmbItemID.SelectedValue));
                    cmd.Parameters.AddWithValue("@ItemName", txtItemName.Text);
                    cmd.Parameters.AddWithValue("@ItemQty", string.IsNullOrEmpty(txtItemQty.Text) ? 1 : Convert.ToInt32(txtItemQty.Text));  // Default to 1 if empty
                    cmd.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(cmbCustomerID.SelectedValue));
                    cmd.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Status", "N");  // Default status for pending orders
                    cmd.Parameters.AddWithValue("@LocalOrExport", localOrExport);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    transaction.Commit();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Customer order updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFields();
                    }
                    else
                    {
                        MessageBox.Show("No record was updated. Please check the Order ID.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string localOrExport = GetLocalOrExportValue();
            if (localOrExport == null)
            {
                MessageBox.Show("Please select Local or Export option.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtOrderID.Text) || cmbItemID.SelectedValue == null || cmbCustomerID.SelectedValue == null || string.IsNullOrEmpty(txtItemQty.Text))
            {
                MessageBox.Show("Please fill all the required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int itemQty;
            if (!int.TryParse(txtItemQty.Text, out itemQty) || itemQty <= 0)
            {
                MessageBox.Show("Please enter a valid quantity greater than 0.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = conn,
                        Transaction = transaction,
                        CommandText = @"
                SET IDENTITY_INSERT tblCustomerOrders ON;

                INSERT INTO tblCustomerOrders (CustomerOrderID, ItemID, ItemName, ItemQty, CustomerID, OrderDate, Status, LocalOrExport)
                VALUES (@CustomerOrderID, @ItemID, @ItemName, @ItemQty, @CustomerID, @OrderDate, @Status, @LocalOrExport);

                SET IDENTITY_INSERT tblCustomerOrders OFF;"
                    };

                    // Add parameters
                    cmd.Parameters.AddWithValue("@CustomerOrderID", Convert.ToInt32(txtOrderID.Text));
                    cmd.Parameters.AddWithValue("@ItemID", Convert.ToInt32(cmbItemID.SelectedValue));
                    cmd.Parameters.AddWithValue("@ItemName", txtItemName.Text);
                    cmd.Parameters.AddWithValue("@ItemQty", itemQty);  // Corrected to use the entered quantity
                    cmd.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(cmbCustomerID.SelectedValue));
                    cmd.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Status", "N"); // Defaulting to 'N' for pending status
                    cmd.Parameters.AddWithValue("@LocalOrExport", localOrExport);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    transaction.Commit();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Customer order saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFields();
                    }
                    else
                    {
                        MessageBox.Show("No record was saved. Please check the data.", "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
