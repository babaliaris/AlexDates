using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexDates
{
    class Stuff : Person
    {

        public Stuff(int id, string name, string lastname, string email, string phone)
            : base(id, name, lastname, email, phone)
        {
        }
    }
}
