﻿using System;
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
                GeneratePurchaseOrderID();
            }

            LoadSuppliers();
            // We will not call LoadItems here because we want to wait until a supplier is selected.
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
                    cmbSupplierID.DisplayMember = "Name"; // Display the supplier name
                    cmbSupplierID.ValueMember = "SupplierID";  // Use SupplierID as value
                    cmbSupplierID.SelectedIndex = -1;          // No default selection
                    cmbSupplierID.SelectedIndexChanged += cmbSupplierID_SelectedIndexChanged; // Attach the event
                }
            }
        }

        private void LoadItems()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT ItemID, Item_Name FROM Items";  // Adjust table and column names accordingly
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("No items found in the database.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;  // Exit if no items found
                        }

                        // Assign the data to each combo box for ItemID1 to ItemID5
                        for (int i = 1; i <= 5; i++)
                        {
                            ComboBox cmbItemID = (ComboBox)this.Controls.Find($"cmbItemID{i}", true).FirstOrDefault();
                            if (cmbItemID != null)
                            {
                                cmbItemID.DataSource = dt.Copy(); // Use a copy to prevent sharing data between combo boxes
                                cmbItemID.DisplayMember = "ItemID"; // Display the ItemID
                                cmbItemID.ValueMember = "ItemID";  // Use ItemID as value
                                cmbItemID.SelectedIndex = -1;      // No default selection
                                cmbItemID.SelectedIndexChanged += cmbItemID_SelectedIndexChanged; // Attach the event
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"SQL error: {sqlEx.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbSupplierID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSupplierID.SelectedItem != null)
            {
                // Cast the selected item to DataRowView
                DataRowView rowView = cmbSupplierID.SelectedItem as DataRowView;
                if (rowView != null)
                {
                    int supplierID = Convert.ToInt32(rowView["SupplierID"]);

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "SELECT Name FROM tblManageSuppliers WHERE SupplierID = @SupplierID";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@SupplierID", supplierID);
                            txtSupplierName.Text = cmd.ExecuteScalar()?.ToString();
                        }
                    }

                    // After selecting a supplier, now load the items
                    LoadItems();
                }
            }
        }

        private void cmbItemID_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            if (cmb != null && cmb.SelectedItem != null)
            {
                // Cast the selected item as DataRowView to access the DataRow's fields
                DataRowView rowView = cmb.SelectedItem as DataRowView;
                if (rowView != null)
                {
                    // Get the ItemID from the DataRowView
                    int selectedID = Convert.ToInt32(rowView["ItemID"]);

                    // Find the corresponding TextBox dynamically
                    TextBox txtItemName = (TextBox)this.Controls.Find($"txtItemName{cmb.Name.Last()}", true).FirstOrDefault();

                    if (txtItemName != null)
                    {
                        try
                        {
                            using (SqlConnection conn = new SqlConnection(connectionString))
                            {
                                conn.Open();
                                string query = "SELECT Item_Name FROM Items WHERE ItemID = @ItemID";
                                using (SqlCommand cmd = new SqlCommand(query, conn))
                                {
                                    cmd.Parameters.AddWithValue("@ItemID", selectedID);
                                    txtItemName.Text = cmd.ExecuteScalar()?.ToString() ?? "Name not found"; // Display the item name
                                }
                            }
                        }
                        catch (SqlException sqlEx)
                        {
                            MessageBox.Show($"SQL error: {sqlEx.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void GeneratePurchaseOrderID()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ISNULL(MAX(PurchaseOrderID), 0) + 1 AS NextID FROM Purchase_Orders";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    txtPO_ID.Text = cmd.ExecuteScalar().ToString();
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
                    foreach (var item in GetItems())
                    {
                        // Updated query excluding PurchaseOrderID since it is an identity column
                        string query = "INSERT INTO Purchase_Orders (ItemID, ItemName, ItemQty, SupplierID) " +
                                       "VALUES (@ItemID, @ItemName, @ItemQty, @SupplierID)";

                        using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ItemID", item.ItemID);
                            cmd.Parameters.AddWithValue("@ItemName", item.ItemName);
                            cmd.Parameters.AddWithValue("@ItemQty", item.ItemQty);
                            cmd.Parameters.AddWithValue("@SupplierID", supplierID);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                throw new Exception("No rows were inserted. The purchase order might already exist or there is an issue with the data.");
                            }
                        }
                    }

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
