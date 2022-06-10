using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Accounting
{
    public partial class Report : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=F:\Accounting\Accounting\db_accounting.mdf;Integrated Security=True");
        public Report()
        {
            InitializeComponent();
        }        
        private void Report_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select customer_name from customers", con);
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    comboBox1.Items.Add(dr["customer_name"].ToString());
                }
                dr.Close();
            }
            catch
            {
            }
            finally
            {
                con.Close();
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter("select i.price_item'مدين',null'دائن',p.product_name'التفاصيل',v.date'التاريخ' from Invoices v  join items i on(v.id=i.Invoice_id) join products p on(i.product_id=p.id) join customers c on(v.customer_id=c.id) where c.customer_name=N'" + comboBox1.Text + "' AND (v.date BETWEEN '" + dateTimePicker1.Value + "' AND '" + dateTimePicker2.Value + "')  union all select e1.amount'مدين',null'دائن',e1.details'التفاصيل',e1.date'التاريخ' from exchange e1 join customers c on(e1.account_from=c.id) where c.customer_name=N'" + comboBox1.Text + "' AND (e1.date BETWEEN '" + dateTimePicker1.Value + "' AND '" + dateTimePicker2.Value + "')  union all select null'مدين',e2.amount'دائن',e2.details'التفاصيل',e2.date'التاريخ' from exchange e2 join customers c on(e2.account_to=c.id) where c.customer_name=N'" + comboBox1.Text + "' AND (e2.date BETWEEN '" + dateTimePicker1.Value + "' AND '" + dateTimePicker2.Value + "')", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrintReport print = new PrintReport(comboBox1.Text, dateTimePicker1.Value, dateTimePicker2.Value);
            print.Show();

        }
    }
}
