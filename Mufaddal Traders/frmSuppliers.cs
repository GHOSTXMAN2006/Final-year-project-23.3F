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
    public partial class frmSuppliers : Form
    {

        private string connectionString = DatabaseConfig.ConnectionString;

        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);


        public frmSuppliers()
        {
            InitializeComponent();
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
            // Check the userType to open the corresponding menu form
            switch (frmLogin.userType)  // Accessing userType from frmLogin
            {
                case "Storekeeper":
                    new frmStorekeeperMenu().Show();
                    break;
                case "Shipping Manager":
                    new frmShippingManagerMenu().Show();
                    break;
                case "Accountant":
                    new frmAccountantsMenu().Show();
                    break;
                case "Marketing and Sales Department":
                    new frmMSD_Menu().Show();
                    break;
                default:
                    MessageBox.Show("Invalid User Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

            // Hide the current dashboard form (optional, to switch to the menu form)
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
            // Check the userType to open the corresponding menu form
            switch (frmLogin.userType)  // Accessing userType from frmLogin
            {
                case "Storekeeper":
                    new frmStorekeeperMenu().Show();
                    break;
                case "Shipping Manager":
                    new frmShippingManagerMenu().Show();
                    break;
                case "Accountant":
                    new frmAccountantsMenu().Show();
                    break;
                case "Marketing and Sales Department":
                    new frmMSD_Menu().Show();
                    break;
                default:
                    MessageBox.Show("Invalid User Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

            // Hide the current dashboard form (optional, to switch to the menu form)
            this.Hide();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            frmHome homeForm = new frmHome();

            homeForm.Show();

            this.Hide();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Ensure at least one row is selected
            if (dgvDisplay.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select one or more rows to delete.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    bool recordDeleted = false; // Flag to track if any record is deleted

                    foreach (DataGridViewRow row in dgvDisplay.SelectedRows)
                    {
                        int supplierID = Convert.ToInt32(row.Cells["SupplierID"].Value);

                        // Check for associations with Purchase Orders
                        string checkOrdersQuery = @"
                SELECT DISTINCT PurchaseOrderID 
                FROM Purchase_Orders
                WHERE SupplierID = @SupplierID";

                        SqlCommand checkOrdersCmd = new SqlCommand(checkOrdersQuery, conn);
                        checkOrdersCmd.Parameters.AddWithValue("@SupplierID", supplierID);
                        SqlDataReader ordersReader = checkOrdersCmd.ExecuteReader();

                        List<int> orderIDs = new List<int>();
                        while (ordersReader.Read())
                        {
                            orderIDs.Add(ordersReader.GetInt32(0));
                        }
                        ordersReader.Close();

                        // Check for associations with Purchase Contracts
                        string checkContractsQuery = @"
                SELECT DISTINCT PurchaseContractID 
                FROM Purchase_Contract
                WHERE SupplierID = @SupplierID";

                        SqlCommand checkContractsCmd = new SqlCommand(checkContractsQuery, conn);
                        checkContractsCmd.Parameters.AddWithValue("@SupplierID", supplierID);
                        SqlDataReader contractsReader = checkContractsCmd.ExecuteReader();

                        List<int> contractIDs = new List<int>();
                        while (contractsReader.Read())
                        {
                            contractIDs.Add(contractsReader.GetInt32(0));
                        }
                        contractsReader.Close();

                        // If there are related Purchase Orders or Contracts, show the message and stop deletion
                        if (orderIDs.Count > 0 || contractIDs.Count > 0)
                        {
                            string message = "This supplier is associated with the following records:\n\n";

                            if (orderIDs.Count > 0)
                            {
                                message += $"Purchase Orders: {string.Join(", ", orderIDs)}\n";
                            }

                            if (contractIDs.Count > 0)
                            {
                                message += $"Purchase Contracts: {string.Join(", ", contractIDs)}\n";
                            }

                            message += "\nPlease remove these associations before deleting the supplier.";

                            MessageBox.Show(message, "Deletion Restricted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // If no associations, confirm deletion
                        DialogResult result = MessageBox.Show(
                            $"Are you sure you want to delete Supplier ID = {supplierID}?",
                            "Delete Confirmation",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (result == DialogResult.Yes)
                        {
                            // Proceed with deletion
                            string deleteQuery = "DELETE FROM tblManageSuppliers WHERE SupplierID = @SupplierID";
                            using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                            {
                                deleteCmd.Parameters.AddWithValue("@SupplierID", supplierID);
                                deleteCmd.ExecuteNonQuery();
                                recordDeleted = true; // Set the flag if deletion is successful
                            }
                        }
                    }

                    if (recordDeleted)
                    {
                        MessageBox.Show("Record(s) deleted successfully!",
                                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Reload the grid only if at least one record is deleted
                        LoadSupplierData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void frmStorekeeperSuppliers_Load(object sender, EventArgs e)
        {

            LoadSupplierData();

            // Check the userType and show/hide buttons accordingly
            if (frmLogin.userType != "Storekeeper")
            {
                btnManage.Visible = false;
                btnDelete.Visible = false;
            }
            else
            {

                btnManage.Visible = true;
                btnDelete.Visible = true;
            }
        }

        private void LoadSupplierData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Pull all fields from tblManageSuppliers
                    string query = @"
                SELECT 
                    SupplierID,
                    Name,
                    Telephone,
                    Email,
                    Address,
                    [Description]
                FROM tblManageSuppliers
            ";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Bind to DataGridView
                    dgvDisplay.DataSource = dt;

                    // -- Apply DataGridView formatting --

                    // Row heights
                    dgvDisplay.RowTemplate.Height = 40;
                    foreach (DataGridViewRow row in dgvDisplay.Rows)
                    {
                        row.Height = 40;
                    }

                    // Basic font and styling
                    dgvDisplay.DefaultCellStyle.Font = new Font("Arial", 12);

                    // Header styling
                    dgvDisplay.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
                    dgvDisplay.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSeaGreen;
                    dgvDisplay.EnableHeadersVisualStyles = false;

                    // Example column widths (adjust as you like)
                    dgvDisplay.Columns["SupplierID"].Width = 115;
                    dgvDisplay.Columns["Name"].Width = 160;
                    dgvDisplay.Columns["Telephone"].Width = 125;
                    dgvDisplay.Columns["Email"].Width = 200;
                    dgvDisplay.Columns["Address"].Width = 300;
                    dgvDisplay.Columns["Description"].Width = 336;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading suppliers: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadSupplierData();
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            frmAddUpdateSuppliers addUpdateSuppliers = new frmAddUpdateSuppliers();
            addUpdateSuppliers.Show();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchText = txtSearch.Text.Trim();

                // If search text is empty, reload all data
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    LoadSupplierData();
                    return;
                }

                string query = @"
            SELECT 
                SupplierID,
                Name,
                Telephone,
                Email,
                Address,
                [Description]
            FROM tblManageSuppliers
            WHERE 
                SupplierID = @SupplierID OR 
                Name LIKE '%' + @Name + '%'
        ";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(query, conn);

                        // Determine whether to search by SupplierID or Name
                        if (int.TryParse(searchText, out int supplierID))
                        {
                            // If numeric, search by SupplierID
                            cmd.Parameters.AddWithValue("@SupplierID", supplierID);
                            cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                        }
                        else
                        {
                            // Otherwise, search by Name
                            cmd.Parameters.AddWithValue("@SupplierID", DBNull.Value);
                            cmd.Parameters.AddWithValue("@Name", searchText);
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable searchResult = new DataTable();
                        adapter.Fill(searchResult);

                        // Bind results to DataGridView
                        dgvDisplay.DataSource = searchResult;

                        // Apply formatting to DataGridView
                        dgvDisplay.RowTemplate.Height = 40;
                        foreach (DataGridViewRow row in dgvDisplay.Rows)
                        {
                            row.Height = 40;
                        }

                        dgvDisplay.DefaultCellStyle.Font = new Font("Arial", 12);
                        dgvDisplay.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
                        dgvDisplay.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSeaGreen;
                        dgvDisplay.EnableHeadersVisualStyles = false;

                        dgvDisplay.Columns["SupplierID"].Width = 115;
                        dgvDisplay.Columns["Name"].Width = 160;
                        dgvDisplay.Columns["Telephone"].Width = 125;
                        dgvDisplay.Columns["Email"].Width = 200;
                        dgvDisplay.Columns["Address"].Width = 300;
                        dgvDisplay.Columns["Description"].Width = 336;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during search: {ex.Message}",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
