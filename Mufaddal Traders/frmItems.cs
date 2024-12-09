using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Mufaddal_Traders
{
    public partial class frmItems : Form
    {
        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private bool isEditMode = false;  // Flag to track if we are in Edit Mode
        private int selectedItemId = -1;  // Store selected ItemID
        private string connectionString = @"Data source=DESKTOP-O0Q3714\SQLEXPRESS ; Initial Catalog=Mufaddal_Traders_db ; Integrated Security=True";

        public frmItems()
        {
            InitializeComponent();
        }

        private void frmItems_Load(object sender, EventArgs e)
        {
            // Check the userType and show/hide buttons accordingly
            if (frmLogin.userType != "Storekeeper")
            {
                btnAdd.Visible = false;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
            }
            else
            {
                btnAdd.Visible = true;
                btnUpdate.Visible = true;
                btnDelete.Visible = true;
            }

            LoadItems(); // Load the items when the form is loaded
        }

        public void LoadItems()
        {
            flowItems.Controls.Clear(); // Clear previous items

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ItemID, Item_Name, Item_Image FROM Items"; // Adjust query if needed
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int itemId = reader.GetInt32(0); // ItemID
                        string itemName = reader.GetString(1); // Item_Name
                        byte[] itemImageBytes = reader["Item_Image"] as byte[]; // Item_Image (binary data)

                        // Create the item panel
                        Panel itemPanel = new Panel
                        {
                            Width = 180,
                            Height = 250,
                            Margin = new Padding(10),
                            BackColor = Color.White,
                            BorderStyle = BorderStyle.FixedSingle,
                            Tag = itemId // Use ItemID as tag to identify the item
                        };

                        // Create the PictureBox for item image
                        PictureBox itemPictureBox = new PictureBox
                        {
                            Width = 160,
                            Height = 180,
                            Top = 10,
                            Left = 10,
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Tag = itemId
                        };

                        // If image exists, set it, otherwise use default
                        if (itemImageBytes != null && itemImageBytes.Length > 0)
                        {
                            itemPictureBox.Image = ByteArrayToImage(itemImageBytes);
                        }
                        else
                        {
                            // Fallback default image (make sure 3486568.png is in your Resources)
                            itemPictureBox.Image = Properties.Resources._3486568;  // Use the correct image name here
                        }

                        // Create the label for item name
                        Label itemLabel = new Label
                        {
                            AutoSize = false,
                            Width = 160,
                            Height = 40,
                            Top = 200,
                            Left = 10,
                            TextAlign = ContentAlignment.MiddleCenter,
                            Text = itemName,
                            Font = new Font("Segoe UI", 10, FontStyle.Bold),
                            Tag = itemId
                        };

                        // Event handlers for click and double-click
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading items: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }

        private Timer clickTimer = new Timer { Interval = 300 }; // Adjust interval as needed
        private bool isDoubleClick = false;

        private void Item_Click(object sender, EventArgs e)
        {
            if (!isDoubleClick)
            {
                clickTimer.Tick += (s, args) =>
                {
                    clickTimer.Stop();
                    Panel clickedPanel = sender as Panel ?? ((Control)sender).Parent as Panel;
                    if (clickedPanel != null)
                    {
                        // Single-click logic
                        foreach (Control control in flowItems.Controls)
                        {
                            if (control is Panel panel)
                                panel.BackColor = Color.White; // Reset colors
                        }
                        clickedPanel.BackColor = Color.LightGray; // Highlight selected
                        selectedItemId = (int)clickedPanel.Tag;
                    }
                };
                clickTimer.Start();
            }
        }

        private void Item_DoubleClick(object sender, EventArgs e)
        {
            isDoubleClick = true;
            clickTimer.Stop();
            Panel clickedPanel = sender as Panel ?? ((Control)sender).Parent as Panel;
            if (clickedPanel != null)
            {
                // Double-click logic
                int selectedItemId = (int)clickedPanel.Tag;
                frmItemDetails itemDetailsForm = new frmItemDetails(selectedItemId);
                itemDetailsForm.Show();
            }
            isDoubleClick = false;
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Check if an item is selected
            if (selectedItemId == -1)
            {
                MessageBox.Show("Please select an item to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirm before deleting
            var confirmResult = MessageBox.Show("Are you sure you want to delete this item?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmResult == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string deleteQuery = "DELETE FROM Items WHERE ItemID = @itemId";
                        SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn);
                        deleteCmd.Parameters.AddWithValue("@itemId", selectedItemId);

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

        private void btnSettings_Click(object sender, EventArgs e)
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

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT ItemID, Item_Name, Item_Image 
            FROM Items 
            WHERE ItemID LIKE @SearchText OR Item_Name LIKE @SearchText";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int itemId = reader.GetInt32(0); // ItemID
                        string itemName = reader.GetString(1); // Item_Name
                        byte[] itemImageBytes = reader["Item_Image"] as byte[]; // Item_Image

                        // Create the item panel (reuse your existing logic for LoadItems)
                        Panel itemPanel = new Panel
                        {
                            Width = 180,
                            Height = 250,
                            Margin = new Padding(10),
                            BackColor = Color.White,
                            BorderStyle = BorderStyle.FixedSingle,
                            Tag = itemId
                        };

                        PictureBox itemPictureBox = new PictureBox
                        {
                            Width = 160,
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
                            itemPictureBox.Image = Properties.Resources._3486568; // Default image
                        }

                        Label itemLabel = new Label
                        {
                            AutoSize = false,
                            Width = 160,
                            Height = 40,
                            Top = 200,
                            Left = 10,
                            TextAlign = ContentAlignment.MiddleCenter,
                            Text = itemName,
                            Font = new Font("Segoe UI", 10, FontStyle.Bold),
                            Tag = itemId
                        };

                        // Add event handlers
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

    }
}
