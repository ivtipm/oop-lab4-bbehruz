using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class PhoneAbonent
    {
        string secondName; // Фамилия 
        string firstName; // Имя
        string middleName; // Отчество
        ulong number; // Номер телефона
        int account; // Баланс в руб.
        ushort id; // id абонента

        public PhoneAbonent(ushort id, string fam, string name, string otch, ulong number, int account)
        {
            this.secondName = fam;
            this.firstName = name;
            this.middleName = otch;
            this.number = number;
            this.account = account;
            this.id = id;
        }

        public string SecondName
        {
            get { return secondName; }
            set { secondName = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string MiddleName
        {
            get { return middleName; }
            set { middleName = value; }
        }

        public ulong Number
        {
            get { return number; }
            set { number = value; }
        }

        public int Account
        {
            get { return account; }
            set { account = value; }
        }

        public ushort ID
        {
            get { return id; }
            set { id = value; }
        }

        public override string ToString()
        {
            return id + "|" + secondName + "|" + firstName + "|" +
                middleName + "|" + number + "|" + account;
        }
    }
}
