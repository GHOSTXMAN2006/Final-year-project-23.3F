﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmAddUpdatePurchaseContract : Form
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

        public frmAddUpdatePurchaseContract()
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

        // Load the combo boxes with supplier and item data when the form loads
        private void frmAddUpdatePurchaseContract_Load(object sender, EventArgs e)
        {
            LoadSupplierData();
            LoadItemData();
            LoadAutoGeneratedContractID();  // Ensure contract ID is auto-generated
        }

        // Function to load Supplier data into the combo box
        // Load Supplier Data
        // Load Supplier Data into ComboBox (Display only SupplierID)
        private void LoadSupplierData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT SupplierID, Name FROM tblManageSuppliers";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                // Clear ComboBox before adding new items
                txtSupplierID.Items.Clear();

                // Load SupplierIDs into ComboBox
                while (reader.Read())
                {
                    // Add only SupplierID to ComboBox (as plain numbers)
                    txtSupplierID.Items.Add(reader["SupplierID"].ToString());
                }
                conn.Close();
            }
        }

        // Load Item Data into ComboBox (Display only ItemID)
        private void LoadItemData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ItemID, Item_Name FROM Items";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                // Clear ComboBox before adding new items
                txtItemID.Items.Clear();

                // Load ItemIDs into ComboBox (as plain numbers)
                while (reader.Read())
                {
                    // Add only ItemID to ComboBox (as plain numbers)
                    txtItemID.Items.Add(reader["ItemID"].ToString());
                }
                conn.Close();
            }
        }

        // When selecting SupplierID, load the corresponding SupplierName
        private void txtSupplierID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtSupplierID.SelectedItem != null)
            {
                string selectedSupplierID = txtSupplierID.SelectedItem.ToString();

                // Look up corresponding Supplier Name based on SupplierID
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Name FROM tblManageSuppliers WHERE SupplierID = @SupplierID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SupplierID", selectedSupplierID);

                    conn.Open();
                    object result = cmd.ExecuteScalar();  // Get the corresponding name

                    // Display the Supplier Name in the txtSupplierName TextBox
                    if (result != null)
                    {
                        txtSupplierName.Text = result.ToString();
                    }
                    conn.Close();
                }
            }
        }

        // When selecting ItemID, load the corresponding ItemName
        private void txtItemID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtItemID.SelectedItem != null)
            {
                string selectedItemID = txtItemID.SelectedItem.ToString();

                // Look up corresponding Item Name based on ItemID
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Item_Name FROM Items WHERE ItemID = @ItemID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ItemID", selectedItemID);

                    conn.Open();
                    object result = cmd.ExecuteScalar();  // Get the corresponding name

                    // Display the Item Name in the txtItemName TextBox
                    if (result != null)
                    {
                        txtItemName.Text = result.ToString();
                    }
                    conn.Close();
                }
            }
        }



        // Function to load the next available Purchase Contract ID
        private void LoadAutoGeneratedContractID()
        {
            Debug.WriteLine("LoadNextPurchaseContractID method called 🐞");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Query to find the smallest missing PurchaseContractID starting from 1
                    string query = @"
                WITH Numbers AS (
                    SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Number
                    FROM master.dbo.spt_values -- Generate a sequence of numbers
                )
                SELECT MIN(Number)
                FROM Numbers
                WHERE Number NOT IN (SELECT PurchaseContractID FROM Purchase_Contract)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    object result = cmd.ExecuteScalar();

                    // Assign the smallest available ID to the text box
                    txtContractID.Text = result != null && result != DBNull.Value
                        ? result.ToString()
                        : "1"; // Default to 1 if no records exist
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Purchase Contract ID: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }




        // Search functionality - search by PurchaseContractID or Supplier Name
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchText = txtSearch.Text.Trim();
                if (!string.IsNullOrEmpty(searchText))
                {
                    SearchAndLoadContract(searchText);
                }
            }
        }


        // Function to search for a contract by ContractID and load it into the form
        private void SearchAndLoadContract(string searchText)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT PurchaseContractID, SupplierID, SupplierName, StartDate, EndDate, ItemID, Item_Name, Description 
            FROM Purchase_Contract
            WHERE PurchaseContractID = @SearchText";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SearchText", searchText);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        // Load data into form fields
                        txtContractID.Text = reader["PurchaseContractID"].ToString();
                        txtSupplierID.SelectedItem = reader["SupplierID"].ToString();
                        txtSupplierName.Text = reader["SupplierName"].ToString();
                        txtStartDate.Text = reader["StartDate"].ToString();
                        txtEndDate.Text = reader["EndDate"].ToString();
                        txtItemID.SelectedItem = reader["ItemID"].ToString();
                        txtItemName.Text = reader["Item_Name"].ToString();
                        txtDescription.Text = reader["Description"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Contract not found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ClearFields();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Update functionality to update contract data
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ValidateInputs() && int.TryParse(txtContractID.Text, out int contractID))
            {
                SqlParameter[] parameters = GetSqlParameters();
                // Replace the last parameter in the array with the contract ID for the update
                parameters[parameters.Length - 1] = new SqlParameter("@PurchaseContractID", contractID); // Use PurchaseContractID for the update query

                ExecuteQuery("UPDATE Purchase_Contract SET SupplierID = @SupplierID, SupplierName = @SupplierName, StartDate = @StartDate, " +
                             "EndDate = @EndDate, ItemID = @ItemID, Item_Name = @Item_Name, Description = @Description WHERE PurchaseContractID = @PurchaseContractID", parameters);
            }
            else
            {
                MessageBox.Show("Please ensure all fields are filled and a valid PurchaseContractID is entered.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        // Clear the form
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        // Function to clear all input fields
        private void ClearFields()
        {
            txtContractID.Clear();
            txtSupplierID.SelectedIndex = -1;
            txtSupplierName.Clear();
            txtStartDate.Value = DateTime.Now;
            txtEndDate.Value = DateTime.Now;
            txtItemID.SelectedIndex = -1;
            txtItemName.Clear();
            txtDescription.Clear();
            LoadAutoGeneratedContractID(); // Reload the new PurchaseContractID
        }

        // Function to validate inputs before saving or updating
        private bool ValidateInputs()
        {
            if (txtSupplierID.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Supplier.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return !(string.IsNullOrWhiteSpace(txtStartDate.Text) ||
                     string.IsNullOrWhiteSpace(txtEndDate.Text) ||
                     txtItemID.SelectedIndex == -1 ||
                     string.IsNullOrWhiteSpace(txtDescription.Text));
        }


        // Function to get SQL parameters from text fields
        private SqlParameter[] GetSqlParameters()
        {
            return new SqlParameter[] {
        new SqlParameter("@SupplierID", txtSupplierID.SelectedItem), // Ensure correct parameter name
        new SqlParameter("@SupplierName", txtSupplierName.Text.Trim()),
        new SqlParameter("@StartDate", txtStartDate.Value),
        new SqlParameter("@EndDate", txtEndDate.Value),
        new SqlParameter("@ItemID", txtItemID.SelectedItem), // Ensure correct parameter name
        new SqlParameter("@Item_Name", txtItemName.Text.Trim()),
        new SqlParameter("@Description", txtDescription.Text.Trim()),
        // Placeholder for PurchaseContractID to be set in btnUpdate_Click
        new SqlParameter("@PurchaseContractID", DBNull.Value)
    };
        }




        // do not remove ti=his event.. keep this as it is
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // do not remove ti=his event.. keep this as it is
        }

        // Execute the query (Insert/Update)
        private void ExecuteQuery(string query, SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parameters);
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Operation successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("Operation failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Enable IDENTITY_INSERT for the table
                string enableIdentityInsert = "SET IDENTITY_INSERT Purchase_Contract ON";
                string disableIdentityInsert = "SET IDENTITY_INSERT Purchase_Contract OFF";

                // Check if the record already exists
                string checkQuery = "SELECT COUNT(*) FROM Purchase_Contract WHERE PurchaseContractID = @PurchaseContractID";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@PurchaseContractID", txtContractID.Text);

                try
                {
                    // Check if the record exists
                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("This record already exists. Please use the Update button to modify it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Enable IDENTITY_INSERT
                    SqlCommand enableCmd = new SqlCommand(enableIdentityInsert, conn);
                    enableCmd.ExecuteNonQuery();

                    // Insert the new record
                    string insertQuery = @"
                INSERT INTO Purchase_Contract (PurchaseContractID, SupplierID, SupplierName, StartDate, EndDate, ItemID, Item_Name, Description) 
                VALUES (@PurchaseContractID, @SupplierID, @SupplierName, @StartDate, @EndDate, @ItemID, @Item_Name, @Description)";

                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@PurchaseContractID", txtContractID.Text);
                    insertCmd.Parameters.AddWithValue("@SupplierID", txtSupplierID.SelectedItem);
                    insertCmd.Parameters.AddWithValue("@SupplierName", txtSupplierName.Text.Trim());
                    insertCmd.Parameters.AddWithValue("@StartDate", txtStartDate.Value);
                    insertCmd.Parameters.AddWithValue("@EndDate", txtEndDate.Value);
                    insertCmd.Parameters.AddWithValue("@ItemID", txtItemID.SelectedItem);
                    insertCmd.Parameters.AddWithValue("@Item_Name", txtItemName.Text.Trim());
                    insertCmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());

                    int result = insertCmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Record saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Reload the form with the next available ID
                        ClearFields();
                    }
                    else
                    {
                        MessageBox.Show("Failed to save the record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    // Disable IDENTITY_INSERT
                    SqlCommand disableCmd = new SqlCommand(disableIdentityInsert, conn);
                    disableCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

    }
}