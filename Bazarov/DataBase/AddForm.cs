using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataBase
{
    public partial class AddForm : Form
    {
         
        public AddForm()
        {
            InitializeComponent();
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            Form1 f = this.Owner as Form1;
            try
            {
                ushort id = f.generateID();
                string fam = textBoxFam.Text;
                string name = textBoxName.Text;
                string otch = textBoxOtch.Text;
                ulong num = (ulong)Convert.ToDouble(textBoxNum.Text);
                int account = (int)Convert.ToInt64(textBoxAccount.Text);
                textBoxFam.Text = "";
                textBoxName.Text = "";
                textBoxOtch.Text = "";
                textBoxNum.Text = "";
                textBoxAccount.Text = "";
                f.data.AddAbonent(id, fam, name, otch, num, account);
                int n = f.data.PhoneAbonent.Count;
                f.dataGridView1.Rows.Add(id, fam, name, otch, num, account);
                f.BanChangeColumn(n - 1);
            }
            catch
            {
                MessageBox.Show("Некорректные данные!");
            }
        }
    }
}
