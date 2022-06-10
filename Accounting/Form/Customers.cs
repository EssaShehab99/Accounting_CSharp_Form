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
    public partial class Customers : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=F:\Accounting\Accounting\db_accounting.mdf;Integrated Security=True");
        int user_id, test_click = 0;
        string user_type;
        public Customers(int user_id, string user_type)
        {
            InitializeComponent();
            this.user_id = user_id;
            this.user_type = user_type;
        }
        private void inserttodataGridView()
        {
            try
            {

                SqlDataAdapter da = new SqlDataAdapter("select c.id as'رقم العميل',c.customer_name as'اسم العميل',isnull(c.amount_creditor,0)-isnull(c.amount_debit,0) as'رصيد الحساب',c.phone_number as'رقم الهاتف',c.date'التاريخ' from customers c join users u on(c.user_id=u.Id)", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;

                
            }
            catch
            {
                MessageBox.Show("مشكلة في عرض الحسابات ", "128", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        private void Customers_Load(object sender, EventArgs e)
        {
            inserttodataGridView();
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            test_click = 0;
            int test = 0;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select customer_name from customers", con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (textBox1.Text == (dr[0]).ToString())
                    {
                        MessageBox.Show("اسم العميل موجود مسبقاً", "130", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        test = 1;
                    }
                }
            }
            catch
            {
                MessageBox.Show("مشكلة في جلب بيانات الحسابات", "131", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally
            {
                con.Close();
            }
            if (test == 0)
            {
                DialogResult result = MessageBox.Show("هل تريد الاستمرار في عملية الإضافة", "132", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand("insert into customers(customer_name,amount_creditor,phone_number,date,user_id) values(N'" + textBox1.Text + "',N'" + textBox3.Text + "',N'" + textBox2.Text + "','"+DateTime.Now+"',"+user_id+")", con);
                    try
                    {
                        con.Open();

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("تم إضافة الحساب بنجاح", "133", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        inserttodataGridView();
                    }
                    catch
                    {
                        MessageBox.Show("لم تتم العملية بنجاح", "134", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        con.Close();
                        inserttodataGridView();
                    }
                }
            }
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            test_click = 1;
            try
            {
                textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                textBox3.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                textBox2.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                con.Close();
                button2.Enabled = true;
                button3.Enabled = true;
            }
            catch
            {
                MessageBox.Show("مشكلة في عرض بيانات العملية", "139", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally
            {
                con.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (test_click == 1 || user_type != "admin")
            {
                DialogResult result = MessageBox.Show("هل تريد الاستمرار في عملية التعديل", "135", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("Update customers set customer_name=N'" + textBox1.Text + "',amount_creditor=" + textBox3.Text + ",phone_number=N'" + textBox2.Text + "' where id=" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "", con);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("تم تعديل بيانات العميل بنجاح", "136", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        MessageBox.Show("لم تتم العملية بنجاح", "137", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        con.Close();
                        inserttodataGridView();
                    }

                }
                test_click = 0;
            }
            else
            {
                MessageBox.Show("الرجاء تحديد العملية المراد تعديلها", "138", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (test_click == 1 || user_type != "admin")
            {
                DialogResult result = MessageBox.Show("هل تريد الاستمرار في عملية الحذف", "140", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("delete customers where id=" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "", con);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("تم حذف العميل بنجاح", "141", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        MessageBox.Show("لم تتم العملية بنجاح", "142", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        con.Close();
                        inserttodataGridView();
                    }

                }
                test_click = 0;
            }
            else
            {
                MessageBox.Show("الرجاء تحديد العملية المراد حذفها", "143", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
