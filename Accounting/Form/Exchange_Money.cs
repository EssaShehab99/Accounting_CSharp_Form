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
    public partial class Exchange_Money : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=F:\Accounting\Accounting\db_accounting.mdf;Integrated Security=True");
        int user_id, test_click = 0;
        string user_type;
        public Exchange_Money(int user_id, string user_type)
        {
            InitializeComponent();
            this.user_id = user_id;
            this.user_type = user_type;
        }
        private void backdata()
        {
            try
            {
                con.Open();
                SqlCommand cmd1 = new SqlCommand("select isnull(amount_debit,0) from customers where customer_name=N'" + dataGridView1.SelectedRows[0].Cells[1].Value.ToString() + "'", con);
                SqlCommand cmd2 = new SqlCommand("select isnull(amount_creditor,0) from customers where customer_name=N'" + dataGridView1.SelectedRows[0].Cells[2].Value.ToString() + "'", con);
                SqlCommand cmd6 = new SqlCommand("select id from customers where customer_name=N'" + dataGridView1.SelectedRows[0].Cells[1].Value.ToString() + "'", con);
                SqlCommand cmd7 = new SqlCommand("select id from customers where customer_name=N'" + dataGridView1.SelectedRows[0].Cells[2].Value.ToString() + "'", con);
                decimal amountdebit = decimal.Parse(cmd1.ExecuteScalar().ToString()) - Convert.ToDecimal(dataGridView1.SelectedRows[0].Cells[3].Value.ToString());
                decimal amountcreditor = decimal.Parse(cmd2.ExecuteScalar().ToString()) - Convert.ToDecimal(dataGridView1.SelectedRows[0].Cells[3].Value.ToString());
                SqlCommand cmd3 = new SqlCommand("update customers set amount_debit=" + amountdebit + " where customer_name=N'" + dataGridView1.SelectedRows[0].Cells[1].Value.ToString() + "'", con);
                SqlCommand cmd4 = new SqlCommand("update customers set amount_creditor=" + amountcreditor + " where customer_name=N'" + dataGridView1.SelectedRows[0].Cells[2].Value.ToString() + "'", con);
                cmd3.ExecuteNonQuery();
                cmd4.ExecuteNonQuery();

            }
            catch
            {
                MessageBox.Show("مشكلة في إرجاع المبالغ", "Back", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }
        private void inserttodataGridView()
        {
            try
            {

                SqlDataAdapter da = new SqlDataAdapter("select e.id'رقم العملية',c1.customer_name'الحساب المحول',c2.customer_name'الحساب المستلم',e.amount'المبلغ',e.details'التفاصيل',u.user_name'المستخدم',e.date'التاريخ' from exchange e join customers c1 on(e.account_from=c1.id) join customers c2 on(e.account_to=c2.id) join users u on(e.user_id=u.Id)", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;


            }
            catch
            {
                MessageBox.Show("مشكلة في عرض الحسابات ");
            }
        }
        private void Exchang_Money_Load(object sender, EventArgs e)
        {
            inserttodataGridView();
            SqlCommand cmd2 = new SqlCommand("select customer_name from customers", con);
            try
            {
                con.Open();
                SqlDataReader dr2 = cmd2.ExecuteReader();
                while (dr2.Read())
                {
                    comboBox2.Items.Add(dr2["customer_name"].ToString());
                    comboBox1.Items.Add(dr2["customer_name"].ToString());
                }
                dr2.Close();
            }
            catch
            {
            }
            finally
            {
                con.Close();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            test_click = 0;
            if (comboBox1.Text != comboBox2.Text)
            {
                try
                {
                    con.Open();
                    SqlCommand cmd1 = new SqlCommand("select isnull(amount_debit,0) from customers where customer_name=N'" + comboBox1.Text + "'", con);
                    SqlCommand cmd2 = new SqlCommand("select isnull(amount_creditor,0) from customers where customer_name=N'" + comboBox2.Text + "'", con);
                    SqlCommand cmd6 = new SqlCommand("select id from customers where customer_name=N'" + comboBox1.Text + "'", con);
                    SqlCommand cmd7 = new SqlCommand("select id from customers where customer_name=N'" + comboBox2.Text + "'", con);
                    decimal amountdebit = decimal.Parse(cmd1.ExecuteScalar().ToString()) + Convert.ToDecimal(textBox1.Text);
                    decimal amountcreditor = decimal.Parse(cmd2.ExecuteScalar().ToString()) + Convert.ToDecimal(textBox1.Text);
                    SqlCommand cmd3 = new SqlCommand("update customers set amount_debit=" + amountdebit + " where customer_name=N'" + comboBox1.Text + "'", con);
                    SqlCommand cmd4 = new SqlCommand("update customers set amount_creditor=" + amountcreditor + " where customer_name=N'" + comboBox2.Text + "'", con);
                    SqlCommand cmd5 = new SqlCommand("insert into exchange(account_from,account_to,amount,details,user_id,date) values(" + cmd6.ExecuteScalar() + "," + cmd7.ExecuteScalar() + "," + textBox1.Text + ",N'" + textBox2.Text + "'," + user_id + ",'" + DateTime.Now + "')", con);
                    cmd5.ExecuteNonQuery();
                    cmd3.ExecuteNonQuery();
                    cmd4.ExecuteNonQuery();
                    MessageBox.Show("تمت عملية التحويل بنجاح");
                }
                catch
                {
                    MessageBox.Show("لم تتم عملية التحويل", "Insert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                    inserttodataGridView();
                }
            }
            else
            {
                MessageBox.Show("لا يمكن التحويل إلى نفس الحساب");
            }
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            test_click = 1;
            try
            {
                comboBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                comboBox2.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                textBox1.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                textBox2.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                con.Close();
            }
            catch
            {
                MessageBox.Show("مشكلة في عرض بيانات العملية", "Display", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally
            {
                con.Close();
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (test_click == 1)
            {
                if (comboBox1.Text != comboBox2.Text)
                {
                    backdata();
                    try
                    {
                        con.Open();
                        SqlCommand cmd1 = new SqlCommand("select isnull(amount_debit,0) from customers where customer_name=N'" + comboBox1.Text + "'", con);
                        SqlCommand cmd2 = new SqlCommand("select isnull(amount_creditor,0) from customers where customer_name=N'" + comboBox2.Text + "'", con);
                        SqlCommand cmd6 = new SqlCommand("select id from customers where customer_name=N'" + comboBox1.Text + "'", con);
                        SqlCommand cmd7 = new SqlCommand("select id from customers where customer_name=N'" + comboBox2.Text + "'", con);
                        decimal amountdebit = decimal.Parse(cmd1.ExecuteScalar().ToString()) + Convert.ToDecimal(textBox1.Text);
                        decimal amountcreditor = decimal.Parse(cmd2.ExecuteScalar().ToString()) + Convert.ToDecimal(textBox1.Text);
                        SqlCommand cmd3 = new SqlCommand("update customers set amount_debit=" + amountdebit + " where customer_name=N'" + comboBox1.Text + "'", con);
                        SqlCommand cmd4 = new SqlCommand("update customers set amount_creditor=" + amountcreditor + " where customer_name=N'" + comboBox2.Text + "'", con);
                        SqlCommand cmd5 = new SqlCommand("update exchange set account_from=" + cmd6.ExecuteScalar() + ",account_to=" + cmd7.ExecuteScalar() + ",amount=" + textBox1.Text + ",details=N'" + textBox2.Text + "'", con);
                        cmd3.ExecuteNonQuery();
                        cmd4.ExecuteNonQuery();
                        cmd5.ExecuteNonQuery();
                        MessageBox.Show("تمت عملية التعديل بنجاح");
                    }
                    catch
                    {
                        MessageBox.Show("لم تتم عملية التحويل", "Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        con.Close();
                        test_click = 0;
                        inserttodataGridView();
                    }
                }
                else
                {
                    MessageBox.Show("لا يمكن التحويل إلى نفس الحساب");
                }
            }
            else
            {
                MessageBox.Show("الرجاء تحديد العملية");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (test_click == 1)
            {
                if (comboBox1.Text != comboBox2.Text)
                {
                    backdata();
                    try
                    {
                        con.Open();
                        SqlCommand cmd1 = new SqlCommand("delete exchange where id =" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "", con);
                        cmd1.ExecuteNonQuery();
                        MessageBox.Show("تمت حذف العملية بنجاح");
                    }
                    catch
                    {
                        MessageBox.Show("لم تتم عملية الحذف","Delete",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                    finally
                    {
                        con.Close();
                        test_click = 0;
                        inserttodataGridView();
                    }
                }
                else
                {
                    MessageBox.Show("لا يمكن التحويل إلى نفس الحساب");
                }
            }
            else
            {
                MessageBox.Show("الرجاء تحديد العملية");
            }
        }
    }
}