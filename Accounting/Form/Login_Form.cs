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
    public partial class Login_Form : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=F:\Accounting\Accounting\db_accounting.mdf;Integrated Security=True");
        public Login_Form()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select user_name from users", con);
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.Read())
                {
                    button1.Text = "إبداء";
                    comboBox1.Enabled = false;
                    textBox1.Enabled = false;
                    
                }
                else
                {
                    con.Close();
                    con.Open();
                    SqlCommand cmd2 = new SqlCommand("select user_name from users", con);
                    SqlDataReader dr2 = cmd2.ExecuteReader();
                    while (dr2.Read())
                    {
                        comboBox1.Items.Add(dr2[0]).ToString();
                    }
                }
            }
            catch
            {
                MessageBox.Show("مشكلة في قواعد البيانات");
            }
            finally
            {
                con.Close();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select user_name from users", con);
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.Read())
                {
                    DialogResult result = MessageBox.Show("شكراً لك لاستخدامك نظامنا إضغط موافق لبدأ استخدام النظام", "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        Users user = new Users(0, "admin");
                        user.Show();
                        this.Hide();
                    }
                    else
                        this.Close();
                }
                else
            enter();
            }
            catch
            {
                MessageBox.Show("مشكلة في قواعد البيانات");
            }
            finally
            {
                con.Close();
            }
        }
        private void enter()
        {
            SqlCommand cmd = new SqlCommand("select * from users where user_name=N'" + comboBox1.Text + "'and user_password=N'" + textBox1.Text + "'", con);
            try
            {
                con.Close();
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int user_id = Convert.ToInt16(dr["id"].ToString());
                    string user_type = dr["user_type"].ToString();
                    string username = dr["user_name"].ToString();
                    Main_Form manform = new Main_Form(user_id, user_type,username);
                    manform.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("اسم المستخدم أو كلمة المرور غير صحيحية");
                }
            }
            catch
            {
                MessageBox.Show("مشكلة في قواعد البيانات");
            }
            finally
            {
                con.Close();
            }
        }

      

      
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                enter();
            }
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                enter();
            }
        }

      
    }
}
