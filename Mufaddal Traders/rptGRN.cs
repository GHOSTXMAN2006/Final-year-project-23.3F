using System;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class rptGRN : Form
    {
        // Property to hold the selected GRN_ID passed from the calling form
        public int SelectedGRN_ID { get; set; }

        public rptGRN(int grnID)
        {
            InitializeComponent();
            SelectedGRN_ID = grnID; // Store the passed GRN_ID
        }

        private void rptGRN_Load(object sender, EventArgs e)
        {
            try
            {
                // Create the Crystal Report object
                CrystalReport1 report = new CrystalReport1(); // Ensure the report is named correctly

                // Set the parameter value for GRN_ID in the report
                report.SetParameterValue("@GRN_ID", SelectedGRN_ID);

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

        private void rptGRN_Load_1(object sender, EventArgs e)
        {

        }
    }
}
