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
    public partial class SearchForm : Form
    {
        public SearchForm()
        {
            InitializeComponent();
        }

        private void Close_Btn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Search_Btn_Click(object sender, EventArgs e)
        {
            Form1 f = this.Owner as Form1;
            if ((f.data.PhoneAbonent.Count == 0) || (textBoxSearch.Text == ""))
                return;
            f.dataGridView1.ClearSelection();
            List<int> foundElements = f.data.SearchFile(textBoxSearch.Text);
            if (foundElements[0] == -1)
            {
                MessageBox.Show("Ничего не удалось найти!");
                return;
            }
            f.dataGridView1.CurrentCell = f.dataGridView1[0, foundElements[0]];
            for (int i = 0; i < foundElements.Count; i++)
            {
                f.dataGridView1.Rows[foundElements[i]].Selected = true;
            }
        }
    }
}
