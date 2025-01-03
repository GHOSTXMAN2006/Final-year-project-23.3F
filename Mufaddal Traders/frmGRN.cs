using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmGRN : Form
    {

        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private readonly string connectionString = DatabaseConfig.ConnectionString;


        public frmGRN()
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Make sure a row is selected in dgvDisplay
            if (dgvDisplay.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to delete.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Retrieve the GRN_ID, PurchaseType, and PurchaseID from the selected row
            int selectedGRNID = Convert.ToInt32(dgvDisplay.SelectedRows[0].Cells["GRN_ID"].Value);
            string selectedPurchaseType = dgvDisplay.SelectedRows[0].Cells["PurchaseType"].Value?.ToString();
            string selectedPurchaseID = dgvDisplay.SelectedRows[0].Cells["PurchaseID"].Value?.ToString();
            string itemIDsCsv = dgvDisplay.SelectedRows[0].Cells["ItemID"].Value?.ToString() ?? "";
            string itemQtysCsv = dgvDisplay.SelectedRows[0].Cells["ItemQuantity"].Value?.ToString() ?? "";
            string warehouseID = dgvDisplay.SelectedRows[0].Cells["WarehouseID"].Value?.ToString() ?? "";

            // Declare arrays outside the block to ensure their scope
            string[] itemIDArray = new string[0];
            string[] itemQtyArray = new string[0];

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Parse item IDs and quantities
                    itemIDArray = itemIDsCsv.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    itemQtyArray = itemQtysCsv.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    // Check for negative stock
                    if (itemIDArray.Length == itemQtyArray.Length && !string.IsNullOrEmpty(warehouseID))
                    {
                        for (int i = 0; i < itemIDArray.Length; i++)
                        {
                            int qtyToSubtract = int.Parse(itemQtyArray[i]);
                            string currentItemID = itemIDArray[i].Trim();

                            string checkStockQuery = @"
                    SELECT ItemQty
                    FROM tblStockBalance
                    WHERE ItemID = @ItemID AND WarehouseID = @WarehouseID";

                            using (SqlCommand checkCmd = new SqlCommand(checkStockQuery, conn))
                            {
                                checkCmd.Parameters.AddWithValue("@ItemID", currentItemID);
                                checkCmd.Parameters.AddWithValue("@WarehouseID", warehouseID);

                                object stockObj = checkCmd.ExecuteScalar();
                                int currentStock = stockObj != null ? Convert.ToInt32(stockObj) : 0;

                                if (currentStock - qtyToSubtract < 0)
                                {
                                    MessageBox.Show($"Deleting this GRN would result in negative stock for ItemID = {currentItemID} in WarehouseID = {warehouseID}. Operation aborted.",
                                                    "Deletion Restricted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                        }
                    }
                }

                // Ask for confirmation
                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to delete GRN ID = {selectedGRNID}?",
                    "Delete Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        using (SqlTransaction transaction = conn.BeginTransaction())
                        {
                            try
                            {
                                // 1) Delete the GRN record
                                string deleteQuery = "DELETE FROM tblGRN WHERE GRN_ID = @GRN_ID";
                                using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn, transaction))
                                {
                                    deleteCmd.Parameters.AddWithValue("@GRN_ID", selectedGRNID);
                                    deleteCmd.ExecuteNonQuery();
                                }

                                // 2) Revert stock quantities
                                for (int i = 0; i < itemIDArray.Length; i++)
                                {
                                    int qtyToSubtract = int.Parse(itemQtyArray[i]);
                                    string currentItemID = itemIDArray[i].Trim();

                                    string revertStockQuery = @"
                            UPDATE tblStockBalance
                            SET ItemQty = ItemQty - @Qty
                            WHERE ItemID = @ItemID AND WarehouseID = @WarehouseID";

                                    using (SqlCommand revertCmd = new SqlCommand(revertStockQuery, conn, transaction))
                                    {
                                        revertCmd.Parameters.AddWithValue("@Qty", qtyToSubtract);
                                        revertCmd.Parameters.AddWithValue("@ItemID", currentItemID);
                                        revertCmd.Parameters.AddWithValue("@WarehouseID", warehouseID);
                                        revertCmd.ExecuteNonQuery();
                                    }
                                }

                                // 3) Revert purchase order status if applicable
                                if (selectedPurchaseType == "O" && !string.IsNullOrEmpty(selectedPurchaseID))
                                {
                                    string revertStatusQuery = @"
                            UPDATE Purchase_Orders
                            SET Status = 'N'
                            WHERE PurchaseOrderID = @PurchaseID";

                                    using (SqlCommand revertCmd = new SqlCommand(revertStatusQuery, conn, transaction))
                                    {
                                        revertCmd.Parameters.AddWithValue("@PurchaseID", selectedPurchaseID);
                                        revertCmd.ExecuteNonQuery();
                                    }
                                }

                                // Commit transaction
                                transaction.Commit();

                                MessageBox.Show("Record deleted and stock reverted successfully!",
                                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Refresh the grid
                                LoadGRNData();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                MessageBox.Show($"An error occurred: {ex.Message}",
                                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}",
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

        private void frmGRN_Load(object sender, EventArgs e)
        {
            // Load the GRN table into dgvDisplay
            LoadGRNData();
        }

        private void LoadGRNData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
            SELECT 
                GRN_ID,
                PurchaseID,
                PurchaseType,
                SupplierID,
                ItemID,
                ItemQuantity,
                WarehouseID,
                GRN_Date,
                GRN_Type
            FROM tblGRN";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable grnTable = new DataTable();
                    adapter.Fill(grnTable);

                    // Bind data to DataGridView
                    dgvDisplay.DataSource = grnTable;

                    // Set DataGridView row height
                    dgvDisplay.RowTemplate.Height = 40;
                    foreach (DataGridViewRow row in dgvDisplay.Rows)
                    {
                        row.Height = 40;
                    }

                    // Basic font and styling
                    dgvDisplay.DefaultCellStyle.Font = new Font("Arial", 12);

                    // Header styling (no center alignment)
                    dgvDisplay.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
                    dgvDisplay.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSeaGreen;
                    dgvDisplay.EnableHeadersVisualStyles = false;

                    // Adjust column widths proportionally (example setup)
                    dgvDisplay.Columns["GRN_ID"].Width = (int)(dgvDisplay.Width * 0.10);
                    dgvDisplay.Columns["PurchaseID"].Width = (int)(dgvDisplay.Width * 0.10);
                    dgvDisplay.Columns["PurchaseType"].Width = (int)(dgvDisplay.Width * 0.12);
                    dgvDisplay.Columns["SupplierID"].Width = (int)(dgvDisplay.Width * 0.10);
                    dgvDisplay.Columns["ItemID"].Width = (int)(dgvDisplay.Width * 0.10);
                    dgvDisplay.Columns["ItemQuantity"].Width = (int)(dgvDisplay.Width * 0.15);
                    dgvDisplay.Columns["WarehouseID"].Width = (int)(dgvDisplay.Width * 0.17);
                    dgvDisplay.Columns["GRN_Date"].Width = (int)(dgvDisplay.Width * 0.15);
                    dgvDisplay.Columns["GRN_Type"].Width = (int)(dgvDisplay.Width * 0.10);

                    // Loop through rows to apply style for PurchaseType column
                    foreach (DataGridViewRow row in dgvDisplay.Rows)
                    {
                        if (row.Cells["PurchaseType"].Value != null)
                        {
                            string pType = row.Cells["PurchaseType"].Value.ToString();

                            // Make the cell text bold
                            row.Cells["PurchaseType"].Style.Font = new Font(
                                dgvDisplay.DefaultCellStyle.Font,
                                FontStyle.Bold
                            );

                            // Apply color-code based on type
                            if (pType == "C") // Contract
                            {
                                row.Cells["PurchaseType"].Style.ForeColor = Color.CadetBlue;
                            }
                            else if (pType == "O") // Purchase Order
                            {
                                row.Cells["PurchaseType"].Style.ForeColor = Color.YellowGreen;
                            }
                            else if (pType == "G") // GIN
                            {
                                row.Cells["PurchaseType"].Style.ForeColor = Color.DarkOrange;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred while loading GRN data: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error
                );
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            // Simply reload the data
            LoadGRNData();
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            frmAddUpdateGRN addUpdateGRN = new frmAddUpdateGRN();

            addUpdateGRN.Show();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchText = txtSearch.Text.Trim();

                // If the search text is empty, load all GRN data
                if (string.IsNullOrEmpty(searchText))
                {
                    Debug.WriteLine("Search text is empty. Calling LoadGRNData() to load all records.");
                    LoadGRNData();
                    return;
                }

                string query = @"
        SELECT 
            GRN_ID,
            PurchaseID,
            PurchaseType,
            SupplierID,
            ItemID,
            ItemQuantity,
            WarehouseID,
            GRN_Date,
            GRN_Type
        FROM tblGRN
        WHERE CAST(GRN_ID AS NVARCHAR) = @Search";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Search", searchText);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable searchResult = new DataTable();
                        adapter.Fill(searchResult);

                        // Bind search results to dgvDisplay
                        dgvDisplay.DataSource = searchResult;

                        // --- Reapply the same formatting as in LoadGRNData() ---

                        // 1. Row Height
                        dgvDisplay.RowTemplate.Height = 40;
                        foreach (DataGridViewRow row in dgvDisplay.Rows)
                        {
                            row.Height = 40;
                        }

                        // 2. Basic Font
                        dgvDisplay.DefaultCellStyle.Font = new Font("Arial", 12);

                        // 3. Header Styling
                        dgvDisplay.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
                        dgvDisplay.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSeaGreen;
                        dgvDisplay.EnableHeadersVisualStyles = false;

                        // 4. Adjust column widths proportionally
                        dgvDisplay.Columns["GRN_ID"].Width = (int)(dgvDisplay.Width * 0.10);
                        dgvDisplay.Columns["PurchaseID"].Width = (int)(dgvDisplay.Width * 0.10);
                        dgvDisplay.Columns["PurchaseType"].Width = (int)(dgvDisplay.Width * 0.12);
                        dgvDisplay.Columns["SupplierID"].Width = (int)(dgvDisplay.Width * 0.10);
                        dgvDisplay.Columns["ItemID"].Width = (int)(dgvDisplay.Width * 0.10);
                        dgvDisplay.Columns["ItemQuantity"].Width = (int)(dgvDisplay.Width * 0.15);
                        dgvDisplay.Columns["WarehouseID"].Width = (int)(dgvDisplay.Width * 0.17);
                        dgvDisplay.Columns["GRN_Date"].Width = (int)(dgvDisplay.Width * 0.15);
                        dgvDisplay.Columns["GRN_Type"].Width = (int)(dgvDisplay.Width * 0.10);

                        // 5. Apply style for "PurchaseType" column
                        foreach (DataGridViewRow row in dgvDisplay.Rows)
                        {
                            if (row.Cells["PurchaseType"].Value != null)
                            {
                                string pType = row.Cells["PurchaseType"].Value.ToString();

                                // Make the cell text bold
                                row.Cells["PurchaseType"].Style.Font = new Font(
                                    dgvDisplay.DefaultCellStyle.Font,
                                    FontStyle.Bold
                                );

                                // Apply color-code based on type
                                if (pType == "C") // Contract
                                {
                                    row.Cells["PurchaseType"].Style.ForeColor = Color.CadetBlue;
                                }
                                else if (pType == "O") // Purchase Order
                                {
                                    row.Cells["PurchaseType"].Style.ForeColor = Color.YellowGreen;
                                }
                                else if (pType == "G") // GIN
                                {
                                    row.Cells["PurchaseType"].Style.ForeColor = Color.DarkOrange;
                                }
                            }
                        }
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
