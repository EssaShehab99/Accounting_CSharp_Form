using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accounting
{
    public partial class Main_Form : Form
    {
        int user_id;
        string user_type;
        string user_name;
        public Main_Form(int user_id, string user_type, string user_name)
        {
            InitializeComponent();
            this.user_id = user_id;
            this.user_type = user_type;
            this.user_name = user_name;
        }

        private void Main_Form_Load(object sender, EventArgs e)
        {
            if (user_type == "user")
            {
                المنجاتToolStripMenuItem.Enabled = false;
                العملاءToolStripMenuItem.Enabled = false;
                سندقيدبسيطToolStripMenuItem.Enabled = false;
            }
        }

        private void تسجيلطلبToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Request reqst = new Request(user_id,user_type,user_name);
            reqst.MdiParent = this;
            reqst.Show();
        }

        private void Main_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                if (Application.OpenForms[i].Name != "Menu")
                    Application.OpenForms[i].Close();
            }
        }

        private void إدارةالمستخدمينToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Users user = new Users(user_id, user_type);
            user.MdiParent = this;
            user.Show();
        }

        private void العملاءToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void إدارةالعملاءToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Customers cstmr = new Customers(user_id, user_type);
            cstmr.MdiParent = this;
            cstmr.Show();
        }

        private void إدارةالمنتجاتToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Products prduct = new Products(user_id, user_type);
            prduct.MdiParent = this;
            prduct.Show();
        }

        private void كشفحسابToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report reprt = new Report();
            reprt.MdiParent = this;
            reprt.Show();
        }

        private void سندقيدبسيطToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exchange_Money change = new Exchange_Money(user_id, user_type);
            change.MdiParent = this;
            change.Show();
        }
    }
}
