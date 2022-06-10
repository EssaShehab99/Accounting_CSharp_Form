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
    public partial class Request : Form
    {
        DataTable dt = new DataTable();

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=F:\Accounting\Accounting\db_accounting.mdf;Integrated Security=True");
        int user_id, testclick = 0;
        string user_type;
        string user_name;
        public Request(int userid, string usertype, string user_name)
        {
            this.user_id = userid;
            this.user_type = usertype;
            this.user_name = user_name;
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void Request_Load(object sender, EventArgs e)
        {
            if (user_type == "admin")
            {
                textBox4.ReadOnly = false;
                comboBox2.Text = user_name;
            }
            dt.Columns.Add("1");
            dt.Columns.Add("2");
            dt.Columns.Add("3");
            dt.Columns.Add("4");
            inserttodataGridView();
            if (user_type == "user")
            {
                comboBox2.Text = user_name;
                comboBox2.Enabled = false;
            }
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button9.Enabled = false;
            SqlCommand cmd = new SqlCommand("select product_name from products", con);
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    comboBox1.Items.Add(dr["product_name"].ToString());
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
            SqlCommand cmd2 = new SqlCommand("select customer_name from customers", con);
            try
            {
                con.Open();
                SqlDataReader dr2 = cmd2.ExecuteReader();
                while (dr2.Read())
                {
                    comboBox2.Items.Add(dr2["customer_name"].ToString());
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void button5_Click(object sender, EventArgs e)
        {
            search();
            testclick = 0;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button9.Enabled = false;

        }
        void search()
        {
            SqlCommand cmd = new SqlCommand("select product_name,quantity,price from products where id=" + textBox1.Text + "", con);
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    textBox3.Text = dr["quantity"].ToString();
                    textBox2.Text = dr["price"].ToString();
                    comboBox1.Text = dr["product_name"].ToString();

                }
                dr.Close();
                textBox4.Text = Convert.ToString(Convert.ToDecimal(textBox2.Text) + (Convert.ToDecimal(textBox2.Text) * 0.1M));
            }
            catch
            {
                MessageBox.Show("الرجاء ادخال رقم المنتج الصحيح");
            }
            finally
            {
                con.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (int.Parse(dataGridView2.Rows[0].Cells[0].Value.ToString()) > 0)
            {
                try
                {
                    con.Open();
                    SqlCommand cmd1 = new SqlCommand("select amount_debit from customers where customer_name=N'" + comboBox2.Text + "'", con);
                    SqlCommand cmd2 = new SqlCommand("select id from customers where customer_name=N'" + comboBox2.Text + "'", con);
                    SqlCommand cmd5 = new SqlCommand("select max(id) from Invoices", con);
                    SqlCommand cmd8 = new SqlCommand("SELECT  isnull(amount_debit,0) FROM customers where id=" + cmd2.ExecuteScalar() + "", con);

                    decimal sum1 = 0M;
                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        try
                        {
                            sum1 += Convert.ToDecimal(dataGridView2.Rows[i].Cells[3].Value);
                        }
                        catch
                        {
                        }
                    }
                    decimal amountdebit = decimal.Parse(cmd8.ExecuteScalar().ToString()) + sum1;
                    SqlCommand cmd3 = new SqlCommand("insert into Invoices(customer_id,total_price,date,user_id)values(" + cmd2.ExecuteScalar().ToString() + "," + sum1 + ",'" + DateTime.Now + "'," + user_id + ")", con);
                    SqlCommand cmd9 = new SqlCommand("update customers set amount_debit=" + amountdebit + " where id=" + cmd2.ExecuteScalar().ToString() + "", con);

                    cmd3.ExecuteNonQuery();
                    cmd9.ExecuteNonQuery();
                    MessageBox.Show("تم اضافة الفاتورة");
                    testclick = 0;
                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        try
                        {
                            SqlCommand cmd4 = new SqlCommand("insert into items(product_id,Invoice_id,number_items,price_item)values(" + dataGridView2.Rows[i].Cells[0].Value + "," + cmd5.ExecuteScalar().ToString() + "," + dataGridView2.Rows[i].Cells[2].Value + "," + dataGridView2.Rows[i].Cells[3].Value + ")", con);
                            SqlCommand cmd7 = new SqlCommand("SELECT quantity FROM products where id=" + dataGridView2.Rows[i].Cells[0].Value + "", con);
                            int quantity = int.Parse(cmd7.ExecuteScalar().ToString()) - int.Parse(dataGridView2.Rows[i].Cells[2].Value.ToString());
                            SqlCommand cmd6 = new SqlCommand("update products set quantity=" + quantity + " where id=" + dataGridView2.Rows[i].Cells[0].Value + "", con);

                            cmd4.ExecuteNonQuery();
                            cmd6.ExecuteNonQuery();

                        }
                        catch
                        {
                        }
                    }

                }
                catch
                {
                }
                finally
                {
                    con.Close();
                }
                inserttodataGridView();
            }
            else
            {
                MessageBox.Show("الرجاء اضافة الطلب إلى القائمة");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                decimal total_price = Convert.ToDecimal(decimal.Parse(textBox4.Text) * decimal.Parse(textBox5.Text));
                string[] row = { textBox1.Text, comboBox1.Text, textBox5.Text, total_price.ToString() };
                dataGridView2.Rows.Add(row);
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
            }
            catch
            {
                MessageBox.Show("الرجاء اكمال البيانات والضغط على زر البحث");
            }

        }
        private void inserttodataGridView()
        {
            try
            {
                if (user_type == "user")
                {
                    SqlDataAdapter da = new SqlDataAdapter("select v.id'رقم الفاتورة',v.total_price'مبلغ الفاتورة',c.customer_name'اسم الحساب',u.user_name'اسم المستخدم',v.date'التاريخ' from Invoices v join customers c on(v.customer_id=c.id) join users u on(v.user_id=u.Id) where v.user_id=" + user_id + "", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                else
                {
                    SqlDataAdapter da = new SqlDataAdapter("select v.id'رقم الفاتورة',v.total_price'مبلغ الفاتورة',c.customer_name'اسم الحساب',u.user_name'اسم المستخدم',v.date'التاريخ' from Invoices v join customers c on(v.customer_id=c.id) join users u on(v.user_id=u.Id)", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }

            }
            catch
            {
            }
        }
        private void backtodataGridView()
        {
            int quantity = 0;
            try
            {
                con.Open();
                SqlCommand cmd1 = new SqlCommand("select amount_debit from customers where customer_name=N'" + dataGridView1.SelectedRows[0].Cells[2].Value.ToString() + "'", con);
                decimal amountdebit = decimal.Parse(cmd1.ExecuteScalar().ToString()) - decimal.Parse(dataGridView1.SelectedRows[0].Cells[1].Value.ToString());
                SqlCommand cmd2 = new SqlCommand("update customers set amount_debit=" + amountdebit + " where customer_name=N'" + dataGridView1.SelectedRows[0].Cells[2].Value.ToString() + "'", con);
                cmd2.ExecuteNonQuery();

                for (int i = 0; i <= dt.Rows.Count; i++)
                {
                    try
                    {
                        SqlCommand cmd3 = new SqlCommand("SELECT quantity FROM products where id=" + dt.Rows[i][0].ToString() + "", con);
                        quantity = int.Parse(cmd3.ExecuteScalar().ToString()) + int.Parse(dt.Rows[i][2].ToString());
                        SqlCommand cmd4 = new SqlCommand("update products set quantity=" + quantity + " where id=" + dt.Rows[i][0].ToString() + "", con);
                        cmd4.ExecuteNonQuery();
                    }
                    catch
                    {
                    }
                }
                SqlCommand cmd5 = new SqlCommand("delete Invoices where id=" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "", con);
                SqlCommand cmd6 = new SqlCommand("delete items where Invoice_id=" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "", con);
                cmd6.ExecuteNonQuery();
                cmd5.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("مشكلة في ارجاع البيانات", "34", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally
            {
                con.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (testclick == 1)
            {
                backtodataGridView();
                MessageBox.Show("تم حذف الفاتورة");
                testclick = 0;
            }
            else
            {
                MessageBox.Show("الرجاء تحديد العملية المراد تعديلها", "138", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button9.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (testclick == 1)
            {
                backtodataGridView();

                if (int.Parse(dataGridView2.Rows[0].Cells[0].Value.ToString()) > 0)
                {
                    try
                    {
                        con.Open();
                        SqlCommand cmd1 = new SqlCommand("select amount_debit from customers where customer_name=N'" + comboBox2.Text + "'", con);
                        SqlCommand cmd2 = new SqlCommand("select id from customers where customer_name=N'" + comboBox2.Text + "'", con);
                        SqlCommand cmd5 = new SqlCommand("select max(id) from Invoices", con);
                        SqlCommand cmd8 = new SqlCommand("SELECT  isnull(amount_debit,0) FROM customers where id=" + cmd2.ExecuteScalar() + "", con);

                        decimal sum1 = 0M;
                        for (int i = 0; i < dataGridView2.Rows.Count; i++)
                        {
                            try
                            {
                                sum1 += Convert.ToDecimal(dataGridView2.Rows[i].Cells[3].Value);
                            }
                            catch
                            {
                            }
                        }
                        decimal amountdebit = decimal.Parse(cmd8.ExecuteScalar().ToString()) + sum1;
                        SqlCommand cmd3 = new SqlCommand("insert into Invoices(customer_id,total_price,date,user_id)values(" + cmd2.ExecuteScalar().ToString() + "," + sum1 + ",'" + DateTime.Now + "'," + user_id + ")", con);
                        SqlCommand cmd9 = new SqlCommand("update customers set amount_debit=" + amountdebit + " where id=" + cmd2.ExecuteScalar().ToString() + "", con);

                        cmd3.ExecuteNonQuery();
                        cmd9.ExecuteNonQuery();
                        MessageBox.Show("تم تعديل الفاتورة");
                        testclick = 0;
                        for (int i = 0; i < dataGridView2.Rows.Count; i++)
                        {
                            try
                            {
                                SqlCommand cmd4 = new SqlCommand("insert into items(product_id,Invoice_id,number_items,price_item)values(" + dataGridView2.Rows[i].Cells[0].Value + "," + cmd5.ExecuteScalar().ToString() + "," + dataGridView2.Rows[i].Cells[2].Value + "," + dataGridView2.Rows[i].Cells[3].Value + ")", con);
                                SqlCommand cmd7 = new SqlCommand("SELECT quantity FROM products where id=" + dataGridView2.Rows[i].Cells[0].Value + "", con);
                                int quantity = int.Parse(cmd7.ExecuteScalar().ToString()) - int.Parse(dataGridView2.Rows[i].Cells[2].Value.ToString());
                                SqlCommand cmd6 = new SqlCommand("update products set quantity=" + quantity + " where id=" + dataGridView2.Rows[i].Cells[0].Value + "", con);

                                cmd4.ExecuteNonQuery();
                                cmd6.ExecuteNonQuery();

                            }
                            catch
                            {
                            }
                        }

                    }
                    catch
                    {
                    }
                    finally
                    {
                        con.Close();
                    }
                    inserttodataGridView();
                }
                else
                {
                    MessageBox.Show("الرجاء اضافة الطلب إلى القائمة");
                }
                inserttodataGridView();
            }
            else
            {
                MessageBox.Show("الرجاء تحديد العملية المراد تعديلها", "138", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button9.Enabled = false;
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button9.Enabled = true;
            testclick = 1;

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select i.product_id,p.product_name,i.number_items,i.price_item*i.number_items'price' from items i join products p on (i.product_id=p.id) where i.Invoice_id=" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "", con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string[] row = { dr["product_id"].ToString(), dr["product_name"].ToString(), dr["number_items"].ToString(), dr["price"].ToString() };
                    dataGridView2.Rows.Add(row);
                    dt.Rows.Add(row);
                }
            }
            catch
            {
                MessageBox.Show("مشكلة في عرض العمليات السابقة", "34", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally
            {
                con.Close();
            }
            try
            {
                con.Open();
                SqlCommand cmd2 = new SqlCommand("select  c.customer_name from Invoices v join customers c on(v.customer_id=c.id) where v.id=" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "", con);
                comboBox2.Text = cmd2.ExecuteScalar().ToString();
            }
            catch
            {
            }
            finally
            {
                con.Close();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dt.Clear();
            dataGridView2.Rows.Clear();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            comboBox1.Text = "";
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button9.Enabled = false;

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            inserttodataGridView();
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button9.Enabled = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (testclick == 1)
            {
                PrintInvoice prtinic = new PrintInvoice(int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString()),user_name);
                prtinic.Show();
            }
            else
            {
                MessageBox.Show("الرجاء تحديد العملية المراد طباعتها", "138", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
