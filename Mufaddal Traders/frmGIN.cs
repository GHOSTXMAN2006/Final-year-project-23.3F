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
    public partial class frmGIN : Form
    {
        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private readonly string connectionString = DatabaseConfig.ConnectionString;


        public frmGIN()
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
            frmStorekeeperMenu menuForm = new frmStorekeeperMenu();

            menuForm.Show();

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
            frmStorekeeperMenu menuForm = new frmStorekeeperMenu();

            menuForm.Show();

            this.Hide();

        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            frmHome homeForm = new frmHome();

            homeForm.Show();

            this.Hide();
        }

        private void frmGIN_Load(object sender, EventArgs e)
        {
            LoadGINData();
        }

        private void LoadGINData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
            SELECT 
                GIN_ID,
                WarehouseID,
                Warehouse_Name,
                SupplierID,
                Supplier_Name,
                ItemID,
                Item_Name,
                GIN_Quantity,
                Status
            FROM tblGIN";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable ginTable = new DataTable();
                    adapter.Fill(ginTable);

                    // Bind data to DataGridView
                    dgvDisplay.DataSource = ginTable;

                    // Set DataGridView row height
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

                    // Adjust column widths proportionally (example setup)
                    dgvDisplay.Columns["GIN_ID"].Width = (int)(dgvDisplay.Width * 0.08);
                    dgvDisplay.Columns["WarehouseID"].Width = (int)(dgvDisplay.Width * 0.13);
                    dgvDisplay.Columns["Warehouse_Name"].Width = (int)(dgvDisplay.Width * 0.16);
                    dgvDisplay.Columns["SupplierID"].Width = (int)(dgvDisplay.Width * 0.10);
                    dgvDisplay.Columns["Supplier_Name"].Width = (int)(dgvDisplay.Width * 0.15);
                    dgvDisplay.Columns["ItemID"].Width = (int)(dgvDisplay.Width * 0.08);
                    dgvDisplay.Columns["Item_Name"].Width = (int)(dgvDisplay.Width * 0.13);
                    dgvDisplay.Columns["GIN_Quantity"].Width = (int)(dgvDisplay.Width * 0.12);
                    dgvDisplay.Columns["Status"].Width = (int)(dgvDisplay.Width * 0.10);

                    // Loop through rows to apply custom styles, if needed
                    foreach (DataGridViewRow row in dgvDisplay.Rows)
                    {
                        if (row.Cells["Status"].Value != null)
                        {
                            string status = row.Cells["Status"].Value.ToString();

                            // Optionally color-code based on status
                            if (status == "N")
                            {
                                row.Cells["Status"].Style.ForeColor = Color.Red;
                            }
                            else if (status == "Y")
                            {
                                row.Cells["Status"].Style.ForeColor = Color.Green;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred while loading GIN data: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
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

        private void btnManage_Click(object sender, EventArgs e)
        {
            frmAddUpdateGIN addUpdateGIN = new frmAddUpdateGIN();

            addUpdateGIN.Show();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string query = @"
                SELECT 
                    GIN_ID,
                    WarehouseID,
                    Warehouse_Name,
                    SupplierID,
                    Supplier_Name,
                    ItemID,
                    Item_Name,
                    GIN_Quantity,
                    Status
                FROM tblGIN
                WHERE CAST(GIN_ID AS NVARCHAR) LIKE '%' + @Search + '%' 
                    OR Warehouse_Name LIKE '%' + @Search + '%'
                    OR Supplier_Name LIKE '%' + @Search + '%'
                    OR Item_Name LIKE '%' + @Search + '%'";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Search", txtSearch.Text.Trim());

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable searchResult = new DataTable();
                        adapter.Fill(searchResult);

                        // Bind search results to DataGridView
                        dgvDisplay.DataSource = searchResult;

                        // Apply formatting and styling
                        dgvDisplay.RowTemplate.Height = 40;
                        dgvDisplay.DefaultCellStyle.Font = new Font("Arial", 12);

                        // Header styling
                        dgvDisplay.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
                        dgvDisplay.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSeaGreen;
                        dgvDisplay.EnableHeadersVisualStyles = false;

                        // Adjust column widths proportionally
                        dgvDisplay.Columns["GIN_ID"].Width = (int)(dgvDisplay.Width * 0.08);
                        dgvDisplay.Columns["WarehouseID"].Width = (int)(dgvDisplay.Width * 0.13);
                        dgvDisplay.Columns["Warehouse_Name"].Width = (int)(dgvDisplay.Width * 0.16);
                        dgvDisplay.Columns["SupplierID"].Width = (int)(dgvDisplay.Width * 0.10);
                        dgvDisplay.Columns["Supplier_Name"].Width = (int)(dgvDisplay.Width * 0.15);
                        dgvDisplay.Columns["ItemID"].Width = (int)(dgvDisplay.Width * 0.08);
                        dgvDisplay.Columns["Item_Name"].Width = (int)(dgvDisplay.Width * 0.13);
                        dgvDisplay.Columns["GIN_Quantity"].Width = (int)(dgvDisplay.Width * 0.12);
                        dgvDisplay.Columns["Status"].Width = (int)(dgvDisplay.Width * 0.10);

                        // Loop through rows to apply custom styles
                        foreach (DataGridViewRow row in dgvDisplay.Rows)
                        {
                            if (row.Cells["Status"].Value != null)
                            {
                                string status = row.Cells["Status"].Value.ToString();

                                // Optionally color-code based on status
                                if (status == "N")
                                {
                                    row.Cells["Status"].Style.ForeColor = Color.Red;
                                }
                                else if (status == "Y")
                                {
                                    row.Cells["Status"].Style.ForeColor = Color.Green;
                                }
                            }
                        }

                        // Display message if no results found
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
            LoadGINData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDisplay.SelectedRows.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show(
                    "Are you sure you want to delete the selected GIN record?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (dialogResult == DialogResult.Yes)
                {
                    int selectedGIN_ID = Convert.ToInt32(dgvDisplay.SelectedRows[0].Cells["GIN_ID"].Value);

                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            SqlTransaction transaction = conn.BeginTransaction();

                            try
                            {
                                // Fetch the GIN record to revert stock balance changes
                                string fetchQuery = @"
                        SELECT WarehouseID, ItemID, GIN_Quantity
                        FROM tblGIN
                        WHERE GIN_ID = @GIN_ID";

                                SqlCommand fetchCmd = new SqlCommand(fetchQuery, conn, transaction);
                                fetchCmd.Parameters.AddWithValue("@GIN_ID", selectedGIN_ID);
                                SqlDataReader reader = fetchCmd.ExecuteReader();

                                if (!reader.Read())
                                {
                                    MessageBox.Show("The selected GIN record does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                int warehouseID = (int)reader["WarehouseID"];
                                int itemID = (int)reader["ItemID"];
                                int ginQuantity = (int)reader["GIN_Quantity"];
                                reader.Close();

                                // Revert stock balance
                                string updateStockQuery = @"
                        UPDATE tblStockBalance
                        SET ItemQty = ItemQty + @GIN_Quantity
                        WHERE ItemID = @ItemID AND WarehouseID = @WarehouseID";

                                SqlCommand updateStockCmd = new SqlCommand(updateStockQuery, conn, transaction);
                                updateStockCmd.Parameters.AddWithValue("@GIN_Quantity", ginQuantity);
                                updateStockCmd.Parameters.AddWithValue("@ItemID", itemID);
                                updateStockCmd.Parameters.AddWithValue("@WarehouseID", warehouseID);
                                updateStockCmd.ExecuteNonQuery();

                                // Delete the GIN record
                                string deleteQuery = @"
                        DELETE FROM tblGIN
                        WHERE GIN_ID = @GIN_ID";

                                SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn, transaction);
                                deleteCmd.Parameters.AddWithValue("@GIN_ID", selectedGIN_ID);
                                deleteCmd.ExecuteNonQuery();

                                transaction.Commit();

                                MessageBox.Show("GIN record deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Reload the data grid view
                                LoadGINData();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                MessageBox.Show($"Error deleting GIN record: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Please select a record to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnViewReport_Click(object sender, EventArgs e)
        {
            try
            {
                // Open the GIN report form
                rptGIN reportForm = new rptGIN();  // No need to pass GIN_ID
                reportForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while opening the report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
