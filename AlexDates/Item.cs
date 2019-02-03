using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexDates
{
    class Item
    {
        public string name;
        public float price;

        public Item()
        {
        }

        public Item(string name, float price)
        {
            this.name  = name;
            this.price = price;
        }
    }
}
