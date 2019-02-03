using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexDates
{
    public class Client : Person
    {
        public string description;
        public float bonus;

        public Client()
        {
        }

        public Client(int id, string name, string lastname, string email, string phone, string desc, float bonus)
            : base(id, name, lastname, email, phone)
        {
            this.description = desc;
            this.bonus       = bonus;
        }
    }
}
