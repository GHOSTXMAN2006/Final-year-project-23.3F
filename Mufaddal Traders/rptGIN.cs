using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class rptGIN : Form
    {
        public rptGIN()
        {
            InitializeComponent();
        }

        private void rptGIN_Load(object sender, EventArgs e)
        {
            try
            {
                // Create and load the Crystal Report
                CrystalReport2 report = new CrystalReport2();  // Use correct report file
                crystalReportViewer1.ReportSource = report;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading the report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
