using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace Accounting
{
    public partial class PrintReport : Form
    {
        string name;
        DateTime date1,date2;
        public PrintReport(string name,DateTime date1,DateTime date2)
        {
            InitializeComponent();
            this.name = name;
            this.date1 = date1;
            this.date2 = date2;
        }

        private void PrintReport_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'DataSet1.DataTable1' table. You can move, or remove it, as needed.
            this.DataTable1TableAdapter.Fill(this.DataSet1.DataTable1, name, date1, date2);
            ReportParameterCollection p = new ReportParameterCollection();
            p.Add(new ReportParameter("ReportParameter1",name));
            this.reportViewer1.LocalReport.SetParameters(p);
            this.reportViewer1.RefreshReport();
        }
    }
}
