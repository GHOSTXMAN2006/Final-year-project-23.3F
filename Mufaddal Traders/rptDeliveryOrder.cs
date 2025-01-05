using System;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class rptDeliveryOrder : Form
    {
        // Property to hold the selected DeliveryOrderID passed from the calling form
        public int SelectedDeliveryOrderID { get; set; }

        public rptDeliveryOrder(int deliveryOrderID)
        {
            InitializeComponent();
            SelectedDeliveryOrderID = deliveryOrderID; // Store the passed DeliveryOrderID
        }

        private void rptDeliveryOrder_Load(object sender, EventArgs e)
        {
            try
            {
                // Create the Crystal Report object (change the report name accordingly)
                CrystalReport3 report = new CrystalReport3();

                // Set the parameter value for DeliveryOrderID in the report
                report.SetParameterValue("@DeliveryOrderID", SelectedDeliveryOrderID);

                // Set the report source for the viewer
                crystalReportViewer1.ReportSource = report;

                // Refresh the viewer to display the report
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading the report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
