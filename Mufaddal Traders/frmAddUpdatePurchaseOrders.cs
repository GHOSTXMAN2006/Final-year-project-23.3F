using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmAddUpdatePurchaseOrders : Form
    {
        private readonly string connectionString = @"Data source=DESKTOP-O0Q3714\SQLEXPRESS ; Initial Catalog=Mufaddal_Traders_db ; Integrated Security=True";

        public string OperationMode { get; set; }
        public int PurchaseOrderID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int ItemQty { get; set; }
        public int SupplierID { get; set; }

        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public frmAddUpdatePurchaseOrders()
        {
            InitializeComponent();
        }

        private void frmAddUpdatePurchaseOrders_Load(object sender, EventArgs e)
        {
            if (OperationMode == "Add")
            {
                PurchaseOrderID = GeneratePurchaseOrderID();
                txtPO_ID.Text = PurchaseOrderID.ToString(); // Display the generated ID in the form
            }

            // Load suppliers and items on form load
            LoadSuppliers();
            LoadItems();
        }



        private void LoadSuppliers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT SupplierID, Name FROM tblManageSuppliers";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cmbSupplierID.DataSource = dt;
                    cmbSupplierID.DisplayMember = "SupplierID"; // Display the Supplier ID
                    cmbSupplierID.ValueMember = "SupplierID";  // Use SupplierID as value
                    cmbSupplierID.SelectedIndex = -1;          // Ensure no default selection
                    txtSupplierName.Text = string.Empty;       // Clear supplier name initially
                }
            }
        }




        private void LoadItems()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ItemID, Item_Name FROM Items";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    for (int i = 1; i <= 5; i++)
                    {
                        ComboBox cmbItemID = (ComboBox)this.Controls.Find($"cmbItemID{i}", true).FirstOrDefault();
                        if (cmbItemID != null)
                        {
                            cmbItemID.DataSource = dt.Copy(); // Use a copy to avoid sharing between comboboxes
                            cmbItemID.DisplayMember = "ItemID"; // Display ItemID
                            cmbItemID.ValueMember = "ItemID";  // Use ItemID as value
                            cmbItemID.SelectedIndex = -1;      // Ensure no default selection
                        }

                        // Clear txtItemName values initially
                        TextBox txtItemName = (TextBox)this.Controls.Find($"txtItemName{i}", true).FirstOrDefault();
                        if (txtItemName != null)
                        {
                            txtItemName.Text = string.Empty;
                        }
                    }
                }
            }
        }




        private void cmbSupplierID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSupplierID.SelectedItem is DataRowView selectedRow)
            {
                // Extract SupplierID safely from the DataRowView
                int supplierID = Convert.ToInt32(selectedRow["SupplierID"]);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Name FROM tblManageSuppliers WHERE SupplierID = @SupplierID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SupplierID", supplierID);
                        txtSupplierName.Text = cmd.ExecuteScalar()?.ToString() ?? string.Empty; // Set supplier name
                    }
                }
            }
        }






        private void cmbItemID_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            if (cmb != null && cmb.SelectedItem is DataRowView selectedRow)
            {
                // Extract ItemID safely from the DataRowView
                int selectedID = Convert.ToInt32(selectedRow["ItemID"]);

                // Find the corresponding txtItemName based on the ComboBox name
                TextBox txtItemName = (TextBox)this.Controls.Find($"txtItemName{cmb.Name.Last()}", true).FirstOrDefault();

                if (txtItemName != null)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "SELECT Item_Name FROM Items WHERE ItemID = @ItemID";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@ItemID", selectedID);
                            txtItemName.Text = cmd.ExecuteScalar()?.ToString() ?? string.Empty; // Populate item name
                        }
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            int supplierID = Convert.ToInt32(cmbSupplierID.SelectedValue);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Generate a unique PurchaseOrderID
                    int purchaseOrderID = GeneratePurchaseOrderID(conn, transaction);

                    // Get all the items from the form
                    var items = GetItems();
                    if (items.Count == 0)
                    {
                        MessageBox.Show("No items to save. Please add at least one item.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Insert each item under the same PurchaseOrderID
                    foreach (var item in items)
                    {
                        string queryInsertItem = "INSERT INTO Purchase_Orders (PurchaseOrderID, ItemID, ItemName, ItemQty, SupplierID, Status) " +
                                                 "VALUES (@PurchaseOrderID, @ItemID, @ItemName, @ItemQty, @SupplierID, @Status);";

                        using (SqlCommand cmd = new SqlCommand(queryInsertItem, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@PurchaseOrderID", purchaseOrderID);
                            cmd.Parameters.AddWithValue("@ItemID", item.ItemID);
                            cmd.Parameters.AddWithValue("@ItemName", item.ItemName);
                            cmd.Parameters.AddWithValue("@ItemQty", item.ItemQty);
                            cmd.Parameters.AddWithValue("@SupplierID", supplierID);
                            cmd.Parameters.AddWithValue("@Status", 'N');

                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Commit the transaction
                    transaction.Commit();
                    MessageBox.Show("Purchase order saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (SqlException sqlEx)
                {
                    transaction.Rollback();
                    MessageBox.Show($"SQL error: {sqlEx.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Generates a new unique PurchaseOrderID without a transaction (for Load event).
        /// </summary>
        private int GeneratePurchaseOrderID()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ISNULL(MAX(PurchaseOrderID), 0) + 1 FROM Purchase_Orders";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        private int GeneratePurchaseOrderID(SqlConnection conn, SqlTransaction transaction)
        {
            string query = "SELECT ISNULL(MAX(PurchaseOrderID), 0) + 1 FROM Purchase_Orders";
            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                return (int)cmd.ExecuteScalar();
            }
        }



        /// <summary>
        /// Validates the form to ensure all necessary fields are filled.
        /// </summary>
        private bool ValidateForm()
        {
            if (cmbSupplierID.SelectedValue == null)
            {
                MessageBox.Show("Please select a supplier.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (GetItems().Count == 0)
            {
                MessageBox.Show("Please add at least one item.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Retrieves all items entered in the form.
        /// </summary>
        private List<(int ItemID, string ItemName, int ItemQty)> GetItems()
        {
            var items = new List<(int ItemID, string ItemName, int ItemQty)>();

            for (int i = 1; i <= 5; i++)
            {
                ComboBox cmbItemID = (ComboBox)this.Controls.Find($"cmbItemID{i}", true).FirstOrDefault();
                TextBox txtItemName = (TextBox)this.Controls.Find($"txtItemName{i}", true).FirstOrDefault();
                TextBox txtQty = (TextBox)this.Controls.Find($"txtQty{i}", true).FirstOrDefault();

                if (cmbItemID != null && txtItemName != null && txtQty != null &&
                    cmbItemID.SelectedValue != null && !string.IsNullOrWhiteSpace(txtItemName.Text) &&
                    int.TryParse(txtQty.Text, out int qty) && qty > 0)
                {
                    items.Add((Convert.ToInt32(cmbItemID.SelectedValue), txtItemName.Text, qty));
                }
            }

            return items;
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
    }
}
