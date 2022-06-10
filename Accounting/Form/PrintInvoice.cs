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
using System.Data.SqlClient;

namespace Accounting
{
    public partial class PrintInvoice : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=F:\Accounting\Accounting\db_accounting.mdf;Integrated Security=True");
        int id;
        string username;
        public PrintInvoice(int id, string username)
        {
            InitializeComponent();
            this.id = id;
            this.username = username;
        }

        private void PrintInvoice_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();            
            // TODO: This line of code loads data into the 'DataSet2.DataTable1' table. You can move, or remove it, as needed.
            this.DataTable1TableAdapter.Fill(this.DataSet2.DataTable1, id);
            ReportParameterCollection p = new ReportParameterCollection();
            p.Add(new ReportParameter("idcustomer", id.ToString()));
            p.Add(new ReportParameter("userid", username));
            this.reportViewer1.LocalReport.SetParameters(p);
            this.reportViewer1.RefreshReport();
        }
    }
}