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
    public partial class Products : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=F:\Accounting\Accounting\db_accounting.mdf;Integrated Security=True");
        int user_id, test_click = 0;
        string user_type;
        public Products(int user_id, string user_type)
        {
            InitializeComponent();
            this.user_id = user_id;
            this.user_type = user_type;
        }
        private void inserttodataGridView()
        {
            try
            {
                    SqlDataAdapter da = new SqlDataAdapter("select id as'رقم المنتج',product_name as'اسم المنتج',price as'سعر المنتج',quantity'الكمية',Expiry_date as'تاريخ الإنتهاء',date'تاريخ الإدخال' from products", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
            }
            catch
            {
                MessageBox.Show("مشكلة في عرض المنتجات ", "128", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        
        private void Products_Load(object sender, EventArgs e)
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
                SqlCommand cmd = new SqlCommand("select user_name from users", con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (textBox1.Text == (dr[0]).ToString())
                    {
                        MessageBox.Show("اسم المنتج موجود مسبقاً", "130", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        test = 1;
                    }
                }
            }
            catch
            {
                MessageBox.Show("مشكلة في جلب بيانات المنتجات", "131", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
                    SqlCommand cmd = new SqlCommand("insert into products(product_name,price,quantity,Expiry_date,user_id,date) values(N'" + textBox1.Text + "'," + textBox3.Text + "," + textBox2.Text + ",'"+dateTimePicker1.Value+"',"+user_id+",'"+DateTime.Now+"')", con);
                    try
                    {
                        con.Open();

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("تم إضافة المنتج بنجاح", "133", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        SqlCommand cmd = new SqlCommand("Update products set product_name=N'" + textBox1.Text + "',price=" + textBox3.Text + ",quantity=" + textBox2.Text + ",Expiry_date='" + dateTimePicker1.Value + "' where id=" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "", con);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("تم تعديل بيانات المنتج بنجاح", "136", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        SqlCommand cmd = new SqlCommand("delete products where id=" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "", con);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("تم حذف المنتج بنجاح", "141", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            test_click = 1;
            try
            {
                textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                textBox3.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                textBox2.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells[4].Value.ToString());
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

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
