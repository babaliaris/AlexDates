using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexDates
{

    public class Person
    {
        public int id;
        public string name;
        public string lastname;
        public string email;
        public string phone;

        public Person()
        {
        }

        public Person(int id, string name, string lastname, string email, string phone)
        {
            this.id       = id;
            this.name     = name;
            this.lastname = lastname;
            this.email    = email;
            this.phone    = phone;
        }
    }
}
