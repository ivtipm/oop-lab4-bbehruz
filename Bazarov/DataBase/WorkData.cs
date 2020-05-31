using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataBase
{
    public class WorkData
    {
        ArrayList phoneAbonent = new ArrayList();

        // вернуть список
        public ArrayList PhoneAbonent
        {
            get { return phoneAbonent; }
        }

        // добавление абонента в список
        public void AddAbonent(ushort id, string fam, string name, 
            string otch, ulong number, int account)
        {
            PhoneAbonent abonent = new PhoneAbonent(id, fam, name, otch, number, account);
            phoneAbonent.Add(abonent);
        }

        // удаление всего списка
        public void DeletePhoneAbonent() => phoneAbonent.Clear();

        // удаление элемента списка по индексу
        public void DeleteAbonent(int index) => phoneAbonent.RemoveAt(index);

        // изменить фамилию абонента по индексу
        public void ChangeSecondName(string fam, int index)
        {
            PhoneAbonent abonent = (PhoneAbonent)phoneAbonent[index];
            abonent.SecondName = fam;
        }

        // изменить имя абонента по индексу
        public void ChangeFirstName(string name, int index)
        {
            PhoneAbonent abonent = (PhoneAbonent)phoneAbonent[index];
            abonent.FirstName = name;
        }

        // изменить отчество абонента по индексу
        public void ChangeMiddleName(string otch, int index)
        {
            PhoneAbonent abonent = (PhoneAbonent)phoneAbonent[index];
            abonent.MiddleName = otch;
        }

        // изменить номер телефона по индексу
        public void ChangeNumber(ulong num, int index)
        {
            PhoneAbonent abonent = (PhoneAbonent)phoneAbonent[index];
            abonent.Number = num;
        }

        // изменить лицевой баланс по индексу
        public void ChangeAccount(int account, int index)
        {
            PhoneAbonent abonent = (PhoneAbonent)phoneAbonent[index];
            abonent.Account = account;
        }

        // сохранение списка в файл
        public void SaveToFile(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename, false, System.Text.Encoding.Unicode))
            {
                foreach (PhoneAbonent s in phoneAbonent)
                {
                    sw.WriteLine(s.ToString());
                }
            }
        }

        // восстанавливает список, записанную в файл
        public void OpenFile(string filename)
        {
            if (!System.IO.File.Exists(filename))
                throw new Exception("Файл не существует");
            if (phoneAbonent.Count != 0)
                DeletePhoneAbonent();
            using (StreamReader sw = new StreamReader(filename))
            {
                while (!sw.EndOfStream)
                {
                    string str = sw.ReadLine();
                    String[] dataFromFile = str.Split(new String[] { "|" },
                        StringSplitOptions.RemoveEmptyEntries);
                    ushort id = (ushort)Convert.ToInt32(dataFromFile[0]);
                    string secondName = dataFromFile[1];
                    string firstName = dataFromFile[2];
                    string middleName = dataFromFile[3];
                    ulong number = (ulong)Convert.ToDouble(dataFromFile[4]);
                    int account = (int)Convert.ToInt64(dataFromFile[5]);
                    AddAbonent(id, secondName, firstName, middleName, number, account);
                }
            }
        }

        // Поиск по заданному параметру и возвращение индексов найденных элементов
        // Вернет -1, если элементы не найдены
        public List<int> SearchFile(string query)
        {
            List<int> count = new List<int>();

            //Проверяются ID 
            if (ushort.TryParse(query, out ushort id_query))
            {
                for (int i = 0; i < phoneAbonent.Count; i++)
                {
                    PhoneAbonent abonent = (PhoneAbonent)phoneAbonent[i];

                    if (abonent.ID == id_query)
                    {
                        count.Add(i);
                        break; // Если нашли запись по уникальному ID, то закончить поиск
                    }
                }

                if (count.Count == 0)
                {
                    count.Add(-1);
                }
                return count;
            }

            // Поиск по текстовым полям записи
            query = query.ToLower(); // перевод в нижний регистр
            query = query.Replace(" ", "");
            for (int i = 0; i < phoneAbonent.Count; i++)
            {
                PhoneAbonent abonent = (PhoneAbonent)phoneAbonent[i];
                if (abonent.SecondName.ToLower().Replace(" ", "").Contains(query))
                    count.Add(i);
            }
            if (count.Count == 0)
                count.Add(-1);
            return count;
        }

        // сортировка элементов во всём списке
        public void Sort()
        {
            phoneAbonent.Sort(new NameComparer());
        }
    }


    public class NameComparer : IComparer
    {

        public NameComparer() : base() { }

        int IComparer.Compare(object x, object y)
        {
            PhoneAbonent abonent1 = (PhoneAbonent)x;
            PhoneAbonent abonent2 = (PhoneAbonent)y;

            return abonent1.SecondName.CompareTo(abonent2.SecondName);
        }
    }
}


