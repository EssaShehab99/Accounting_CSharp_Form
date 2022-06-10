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
    public partial class Users : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=F:\Accounting\Accounting\db_accounting.mdf;Integrated Security=True");
        int user_id, test_click = 0;
        string user_type;
        public Users(int user_id, string user_type)
        {
            InitializeComponent();
            this.user_id = user_id;
            this.user_type = user_type;
        }
        private void inserttodataGridView()
        {
            try
            {

                if (user_type == "admin")
                {
                    SqlDataAdapter da = new SqlDataAdapter("select id as'رقم المستخدم',user_name as'اسم المستخدم',user_password as'كلمة المرور',user_type as'نوع المستخدم' from users", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;

                }
                else if (user_type == "user")
                {
                    SqlDataAdapter da = new SqlDataAdapter("select id as'رقم المستخدم',user_name as'اسم المستخدم',user_password as'كلمة المرور',user_type as'نوع المستخدم' from users where id=" + user_id + "", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;

                }
            }
            catch
            {   
                MessageBox.Show("مشكلة في عرض المستخدمين ", "128", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        private void Users_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("admin");
            comboBox1.Items.Add("user");
            inserttodataGridView();
            if (user_id == 0)
            {
                comboBox1.Items.Add("admin");
                comboBox1.Enabled = false;
                textBox1.Text = "مدير النظام";
                textBox1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
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
                        SqlCommand cmd = new SqlCommand("Update users set user_name=N'" + textBox1.Text + "',user_type=N'" + comboBox1.Text + "',user_password=N'" + textBox2.Text + "' where id=" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "", con);
                        SqlCommand cmd2 = new SqlCommand("Update customers set customer_name=N'" + textBox1.Text + "'where customer_name=N'" + dataGridView1.SelectedRows[0].Cells[1].Value.ToString() + "'", con);
                        cmd.ExecuteNonQuery();
                        cmd2.ExecuteNonQuery();
                        MessageBox.Show("تم تعديل بيانات المستخدم بنجاح", "136", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        SqlCommand cmd = new SqlCommand("delete users where id=" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "", con);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("تم حذف المستخدم بنجاح", "141", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void button1_Click_1(object sender, EventArgs e)
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
                        MessageBox.Show("اسم المستخدم موجود مسبقاً", "130", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    if (user_id == 0)
                    {
                        SqlCommand cmd = new SqlCommand("insert into users(user_name,user_password,user_type) values(N'" + textBox1.Text + "',N'" + textBox2.Text + "','admin')", con);
                        SqlCommand cmd2 = new SqlCommand("insert into customers(customer_name,date,user_id) values(N'" + textBox1.Text + "','" + DateTime.Now + "',null)", con);
                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            cmd2.ExecuteNonQuery();
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
                            this.Close();
                        }
                    }
                    else
                    {
                        SqlCommand cmd = new SqlCommand("insert into users(user_name,user_password,user_type) values(N'" + textBox1.Text + "',N'" + textBox2.Text + "',N'" + comboBox1.Text + "')", con);
                        SqlCommand cmd2 = new SqlCommand("insert into customers(customer_name,date,user_id) values(N'" + textBox1.Text + "','" + DateTime.Now + "'," + user_id + ")", con);
                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            cmd2.ExecuteNonQuery();
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

        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            test_click = 1;
            try
            {
                textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                textBox2.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                comboBox1.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                con.Close();
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

        private void Users_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (user_id == 0)
            {
                for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
                {
                    if (Application.OpenForms[i].Name != "Menu")
                        Application.OpenForms[i].Close();
                }
            }
        }
    }
}
