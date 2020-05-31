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
    public partial class Form1 : Form
    {
        public WorkData data = new WorkData();
        public string oldValue = "";
        public string filename = "";

        public Form1()
        {
            InitializeComponent();
            dataGridView1.Rows[0].ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            labelSave.Visible = false;
            InitializeTimers();
        }

        private void InitializeTimers()
        {
            // Таймер для отображения надписи, свидетельствующей о сохранении файла
            // длительностью 3 сек
            timer1.Interval = 3000;
            timer1.Tick += new EventHandler(timer1_Tick);
        }

        private void btnAddRecord_Click(object sender, EventArgs e)
        {
            AddForm af = new AddForm();
            af.Owner = this;
            af.Show();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchForm sf = new SearchForm();
            sf.Owner = this;
            sf.Show();
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            if (data.PhoneAbonent.Count != 0)
            {
                dataGridView1.Rows.Clear();
                data.Sort();
                WriteToDataGrid();
            }
        }

        private void WriteToDataGrid()
        {
            for (int i = 0; i < data.PhoneAbonent.Count; i++)
            {
                PhoneAbonent abonent = (PhoneAbonent)data.PhoneAbonent[i];
                dataGridView1.Rows.Add(abonent.ID, abonent.SecondName,
                    abonent.FirstName, abonent.MiddleName, abonent.Number, abonent.Account);
                BanChangeColumn(i);
            }
            // последнюю строку запрещаем редактировать
            dataGridView1.Rows[data.PhoneAbonent.Count].ReadOnly = true;
        }

        // Запретить редактировать столбец по указанному индексу
        public void BanChangeColumn(int index) =>
            dataGridView1.Rows[index].Cells[0].ReadOnly = true;

        // Генерация не повторяющихся id
        public ushort generateID()
        {
            Random r = new Random();
            int id = r.Next(0, 1000);
            for (int i = 0; i < data.PhoneAbonent.Count; i++)
            {
                PhoneAbonent abonent = (PhoneAbonent)data.PhoneAbonent[i];
                if (abonent.ID == id)
                {
                    i = 0;
                    id = r.Next(0, 1000);
                }
            }
            return (ushort)id;
        }

        private void btnClearRow_Click(object sender, EventArgs e)
        {
            int count = dataGridView1.Rows.Count;
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                int index = row.Index; // индекс выбранной строки
                if (index == count - 1) return;
                data.DeleteAbonent(index);
                dataGridView1.Rows.RemoveAt(index);
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            if (data.PhoneAbonent.Count != 0)
            {
                DialogResult dialogResult = MessageBox.Show("Уверены," +
                    "что хотите удалить все элементы?", "Подтверждение", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    data.DeletePhoneAbonent();
                    dataGridView1.Rows.Clear();
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int indRow = dataGridView1.Rows[e.RowIndex].Index;
            int indColumn = dataGridView1.Columns[e.ColumnIndex].Index;
            object value = dataGridView1.Rows[indRow].Cells[indColumn].Value;
            // Если значение не было введено, то оставляем старое
            if (value is null)
            {
                MessageBox.Show("Вы не ввели значение.");
                dataGridView1.Rows[indRow].Cells[indColumn].Value = oldValue;
                return;
            }
            if (indColumn == 1)
            {
                data.ChangeSecondName((string)value, indRow);
            }
            else
            if (indColumn == 2)
            {
                data.ChangeFirstName((string)value, indRow);
            }
            else
            if (indColumn == 3)
            {
                data.ChangeMiddleName((string)value, indRow);
            }
            else
            if (indColumn == 4)
            {
                data.ChangeNumber((ulong)Convert.ToDouble(value), indRow);
            }
            else
            if (indColumn == 5)
            {
                data.ChangeAccount((int)Convert.ToInt64(value), indRow);
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                oldValue = ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        }

        // Восстановление данных из файла в таблицу и в список
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            filename = openFileDialog1.FileName;
            this.Text = filename + " - База данных телефонных абонентов";
            dataGridView1.Rows.Clear();
            data.OpenFile(filename);
            WriteToDataGrid();
        }

        // Сохранение данных в файл
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filename == "")
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                filename = saveFileDialog1.FileName;
                this.Text = filename + " - База данных телефонных абонентов";
            }
            // При каждом сохранении будет появляться надпись "Сохранение..."
            labelSave.Visible = true;
            timer1.Enabled = true;
            timer1.Start();

            data.SaveToFile(filename);
        }

        //"Справка"
        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Info = "База данных телефонных абонентов, версия 1.4 " + "\n\n" +
                "Powered by Visual Studio 2019, C#" + "\n\n" +
                "2020 г.";
            MessageBox.Show(Info, "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            labelSave.Visible = false;
            timer1.Enabled = false;
            timer1.Stop();
        }
    }
}
