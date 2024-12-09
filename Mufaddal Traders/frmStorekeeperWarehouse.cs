using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmStorekeeperWarehouse : Form
    {
        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        // Connection string for SQL Server
        private string connectionString = @"Data Source=LAPTOP-S6UOBFRN\SQLEXPRESS;Initial Catalog=backup2;Integrated Security=True";

        public frmStorekeeperWarehouse()
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
            // keep this event as it is..
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

        private void btnSettings_Click(object sender, EventArgs e)
        {
            // keep this event as it is..
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            frmStorekeeperMenu menuForm = new frmStorekeeperMenu();
            menuForm.Show();
            this.Hide();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // keep this event as it is..
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            frmAddUpdateWarehouse addUpdateWarehouse = new frmAddUpdateWarehouse();
            addUpdateWarehouse.Show();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddUpdateWarehouse addUpdateWarehouse = new frmAddUpdateWarehouse();
            addUpdateWarehouse.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // keep this event as it is..
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // keep this event as it is..
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // keep this event as it is..
        }

        private void picHeader_Paint(object sender, PaintEventArgs e)
        {
            // keep this event as it is..
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // keep this event as it is..
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            // keep this event as it is..
        }

        private void frmStorekeeperWarehouse_Load(object sender, EventArgs e)
        {
            LoadDataIntoGridView(); // Call the method to load data into the grid
        }


        /// <summary>
        /// Loads data from the Warehouse table into the gridView and sets proper column headers.
        /// </summary>
        private void LoadDataIntoGridView()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT StoreID, Store_Name, Store_Location FROM Warehouse";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Set data source of the gridView to display the warehouse data
                    gridView.DataSource = dataTable;

                    // Rename the columns to be more readable
                    gridView.Columns[0].HeaderText = "Store ID";
                    gridView.Columns[1].HeaderText = "Store Name";
                    gridView.Columns[2].HeaderText = "Store Location";

                    // Auto-size the columns to fit the content
                    gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    // Optional: Format headers and appearance
                    gridView.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Bold); // Header font size
                    gridView.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
                    gridView.DefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Regular); // Cell font size
                    gridView.DefaultCellStyle.ForeColor = Color.Black; // Cell text color
                    gridView.DefaultCellStyle.SelectionBackColor = Color.LightBlue; // Selected row background
                    gridView.DefaultCellStyle.SelectionForeColor = Color.Black; // Selected row text color
                    gridView.EnableHeadersVisualStyles = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading warehouse details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
