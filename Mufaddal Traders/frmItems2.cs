using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmItems2 : Form
    {

        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private int selectedItemId = -1;  // Store selected ItemID
        private string connectionString = DatabaseConfig.ConnectionString;


        public frmItems2()
        {
            InitializeComponent();
        }


        private void frmItems2_Load(object sender, EventArgs e)
        {
            try
            {
                if (flowItems == null)
                {
                    throw new NullReferenceException("flowItems control is not initialized.");
                }

                // Ensure proper padding for flowItems
                flowItems.Padding = new Padding(20, 20, 20, 40);

                // Toggle button visibility based on userType
                btnAdd.Visible = frmLogin.userType == "Storekeeper";
                btnUpdate.Visible = frmLogin.userType == "Storekeeper";
                btnDelete.Visible = frmLogin.userType == "Storekeeper";

                LoadItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in frmItems2_Load: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        public void LoadItems()
        {
            try
            {
                flowItems.Controls.Clear(); // Clear previous items

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT ItemID, Item_Name, Item_Image FROM Items";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int itemId = reader.GetInt32(0);
                        string itemName = reader.GetString(1);
                        byte[] itemImageBytes = reader["Item_Image"] as byte[];

                        // Create item panel
                        ItemPanel itemPanel = new ItemPanel
                        {
                            Width = 260,
                            Height = 260,
                            Margin = new Padding(16),
                            BackColor = Color.White,
                            BorderRadius = 20, // Rounded corners
                            Tag = itemId // Store ItemID for later use
                        };

                        // PictureBox for item image
                        PictureBox itemPictureBox = new PictureBox
                        {
                            Width = 240,
                            Height = 180,
                            Top = 10,
                            Left = 10,
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Tag = itemId // Ensure the Tag is set for all controls
                        };

                        if (itemImageBytes != null && itemImageBytes.Length > 0)
                        {
                            itemPictureBox.Image = ByteArrayToImage(itemImageBytes);
                        }
                        else
                        {
                            itemPictureBox.Image = Properties.Resources.noImageDefault; // Fallback image
                        }

                        // Label for item name
                        Label itemLabel = new Label
                        {
                            AutoSize = false,
                            Width = itemPanel.Width,
                            Height = 40,
                            Top = 200,
                            Left = 0,
                            TextAlign = ContentAlignment.MiddleCenter,
                            Text = itemName,
                            Font = new Font("Segoe UI", 10, FontStyle.Bold),
                            Tag = itemId // Ensure the Tag is set
                        };

                        // Event handlers for Click and DoubleClick
                        itemPanel.Click += Item_Click;
                        itemPanel.DoubleClick += Item_DoubleClick;
                        itemPictureBox.Click += Item_Click;
                        itemPictureBox.DoubleClick += Item_DoubleClick;
                        itemLabel.Click += Item_Click;
                        itemLabel.DoubleClick += Item_DoubleClick;

                        itemPanel.Controls.Add(itemPictureBox);
                        itemPanel.Controls.Add(itemLabel);

                        flowItems.Controls.Add(itemPanel); // Add to flow layout
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading items: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private Image ByteArrayToImage(byte[] byteArray)
        {
            try
            {
                if (byteArray == null || byteArray.Length == 0)
                {
                    return Properties.Resources.noImageDefault; // Default image
                }

                using (MemoryStream ms = new MemoryStream(byteArray))
                {
                    return Image.FromStream(ms);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error converting byte array to image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return Properties.Resources.noImageDefault; // Fallback to default image
            }
        }




        private Timer clickTimer = new Timer { Interval = 300 }; // Adjust interval as needed
        private bool isDoubleClick = false;

        private void Item_Click(object sender, EventArgs e)
        {
            try
            {
                Control clickedControl = sender as Control;
                ItemPanel clickedPanel = clickedControl?.Parent as ItemPanel ?? clickedControl as ItemPanel;

                if (clickedPanel == null) return;

                // Reset colors for all panels
                foreach (Control control in flowItems.Controls)
                {
                    if (control is ItemPanel panel)
                    {
                        SetPanelAndChildrenBackColor(panel, Color.White); // Reset to default color
                    }
                }

                // Highlight the selected panel
                SetPanelAndChildrenBackColor(clickedPanel, Color.LightSlateGray); // Highlight color

                selectedItemId = (int)clickedPanel.Tag; // Store selected item ID
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting item: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void SetPanelAndChildrenBackColor(ItemPanel panel, Color backColor)
        {
            if (panel == null) return;

            panel.BackColor = backColor;

            foreach (Control child in panel.Controls)
            {
                if (child != null)
                {
                    child.BackColor = backColor;
                }
            }
        }


        private void Item_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                // Identify the control that triggered the double-click
                Control clickedControl = sender as Control;
                ItemPanel clickedPanel = clickedControl?.Parent as ItemPanel ?? clickedControl as ItemPanel;

                if (clickedPanel == null)
                {
                    MessageBox.Show("Unable to identify the clicked item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Retrieve the ItemID from the clicked panel's Tag property
                if (clickedPanel.Tag is int selectedItemId)
                {
                    // Open the details form for the selected item
                    frmItemDetails itemDetailsForm = new frmItemDetails(selectedItemId);
                    itemDetailsForm.ShowDialog(); // Use ShowDialog to wait for the form to close
                }
                else
                {
                    MessageBox.Show("The selected item's details could not be loaded. Invalid ItemID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing double-click: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Check if an item is selected
            if (selectedItemId == -1)
            {
                MessageBox.Show("Please select an item to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    Dictionary<string, List<int>> referencingRecords = new Dictionary<string, List<int>>();

                    // Function to execute queries and add referenced records
                    void CheckForReferences(string tableName, string query, string parameterName)
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue(parameterName, selectedItemId);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                List<int> recordIDs = new List<int>();
                                while (reader.Read())
                                {
                                    recordIDs.Add(reader.GetInt32(0)); // Assuming first column is the primary key
                                }
                                if (recordIDs.Count > 0)
                                {
                                    referencingRecords.Add(tableName, recordIDs);
                                }
                            }
                        }
                    }

                    // Check each table for references
                    CheckForReferences("Purchase_Orders", "SELECT PurchaseOrderID FROM Purchase_Orders WHERE ItemID = @ItemID", "@ItemID");
                    CheckForReferences("Sales_Invoice", "SELECT Sales_ID FROM Sales_Invoice WHERE ItemID = @ItemID", "@ItemID");
                    CheckForReferences("Stock_Transfer", "SELECT ST_ID FROM Stock_Transfer WHERE ItemID = @ItemID", "@ItemID");
                    CheckForReferences("tblGIN", "SELECT GIN_ID FROM tblGIN WHERE ItemID = @ItemID", "@ItemID");
                    CheckForReferences("tblGRN", "SELECT GRN_ID FROM tblGRN WHERE ItemID LIKE '%' + CAST(@ItemID AS NVARCHAR) + '%'", "@ItemID");
                    CheckForReferences("tblStockBalance", "SELECT WarehouseID FROM tblStockBalance WHERE ItemID = @ItemID", "@ItemID");

                    if (referencingRecords.Count > 0)
                    {
                        // Construct detailed restriction message
                        StringBuilder messageBuilder = new StringBuilder();
                        messageBuilder.AppendLine("Cannot delete item. It is referenced in the following records:");

                        foreach (var tableEntry in referencingRecords)
                        {
                            string tableName = tableEntry.Key;
                            List<int> recordIDs = tableEntry.Value;
                            messageBuilder.AppendLine($"- {tableName}: Record IDs [{string.Join(", ", recordIDs)}]");
                        }

                        MessageBox.Show(messageBuilder.ToString(), "Deletion Restricted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Exit without showing the confirmation dialog
                    }

                    // If there are no references, ask for confirmation before deleting
                    var confirmResult = MessageBox.Show("Are you sure you want to delete this item?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirmResult != DialogResult.Yes)
                    {
                        return;
                    }

                    // Proceed with deletion
                    string deleteQuery = "DELETE FROM Items WHERE ItemID = @ItemID";
                    SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn);
                    deleteCmd.Parameters.AddWithValue("@ItemID", selectedItemId);
                    int rowsAffected = deleteCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Item deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadItems(); // Reload items to reflect the deletion
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting item: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedItemId == -1)
            {
                MessageBox.Show("Please select an item to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            frmAddUpdateItems updateForm = new frmAddUpdateItems(this, selectedItemId);
            updateForm.ShowDialog(); // Use ShowDialog to wait for the form to close
        }



        private void picHeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            frmHome homeForm = new frmHome();
            homeForm.Show();
            this.Hide();
        }

        private void btnAccount_Click(object sender, EventArgs e)
        {
            frmAccount accountFrm = new frmAccount();
            accountFrm.Show();
            this.Hide();
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

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            frmDashboard dashboardForm = new frmDashboard();
            dashboardForm.Show();
            this.Hide();
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            // Placeholder for future functionality
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddUpdateItems addUpdateForm = new frmAddUpdateItems(this);
            addUpdateForm.ShowDialog(); // Use ShowDialog to wait for the form to close
        }


        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadItems(); // Reload the items in the flow layout
        }


        private void SearchItems(string searchText)
        {
            flowItems.Controls.Clear(); // Clear existing items

            // Validate the input
            if (string.IsNullOrWhiteSpace(searchText))
            {
                MessageBox.Show("Please enter a valid search term.", "Invalid Search", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Check if the search text is numeric for `ItemID`, otherwise search by `Item_Name`
                string query;
                if (int.TryParse(searchText, out int parsedId))
                {
                    // Exact match for ItemID
                    query = @"
                SELECT ItemID, Item_Name, Item_Image 
                FROM Items 
                WHERE ItemID = @SearchText";
                }
                else
                {
                    // Partial match for Item_Name
                    query = @"
                SELECT ItemID, Item_Name, Item_Image 
                FROM Items 
                WHERE Item_Name LIKE @SearchText";
                    searchText = "%" + searchText + "%"; // Add wildcards for partial matching
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SearchText", searchText);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int itemId = reader.GetInt32(0); // ItemID
                        string itemName = reader.GetString(1); // Item_Name
                        byte[] itemImageBytes = reader["Item_Image"] as byte[]; // Item_Image

                        // Create the item panel
                        ItemPanel itemPanel = new ItemPanel
                        {
                            Width = 260,
                            Height = 260,
                            Margin = new Padding(16),
                            BackColor = Color.White,
                            BorderRadius = 20, // Corner radius for the panel
                            Tag = itemId // Use ItemID as tag to identify the item
                        };

                        // PictureBox for item image
                        PictureBox itemPictureBox = new PictureBox
                        {
                            Width = 240,
                            Height = 180,
                            Top = 10,
                            Left = 10,
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Tag = itemId
                        };

                        if (itemImageBytes != null && itemImageBytes.Length > 0)
                        {
                            itemPictureBox.Image = ByteArrayToImage(itemImageBytes);
                        }
                        else
                        {
                            itemPictureBox.Image = Properties.Resources.noImageDefault; // Default image
                        }

                        // Label for item name
                        Label itemLabel = new Label
                        {
                            AutoSize = false,
                            Width = itemPanel.Width,
                            Height = 40,
                            Top = 200,
                            Left = 0,
                            TextAlign = ContentAlignment.MiddleCenter,
                            Text = itemName,
                            Font = new Font("Segoe UI", 10, FontStyle.Bold),
                            Tag = itemId
                        };

                        // Event handlers
                        itemPictureBox.Click += Item_Click;
                        itemLabel.Click += Item_Click;
                        itemPanel.Click += Item_Click;

                        itemPictureBox.DoubleClick += Item_DoubleClick;
                        itemLabel.DoubleClick += Item_DoubleClick;
                        itemPanel.DoubleClick += Item_DoubleClick;

                        itemPanel.Controls.Add(itemPictureBox);
                        itemPanel.Controls.Add(itemLabel);

                        flowItems.Controls.Add(itemPanel);
                    }

                    if (!reader.HasRows)
                    {
                        MessageBox.Show("No items found matching your search.", "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error searching items: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // Check if Enter key is pressed
            {
                string searchText = txtSearch.Text.Trim();
                if (!string.IsNullOrEmpty(searchText))
                {
                    SearchItems(searchText); // Call the search method
                }
                else
                {
                    LoadItems(); // Reload all items if search text is empty
                }
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
    }
}
