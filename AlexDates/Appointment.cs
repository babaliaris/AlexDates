using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexDates
{
    class Appointment
    {
        public int stuff_id;
        public int client_id;
        public DateTime date;
        public string service;
        public bool payment;
        public int row;
        public string name;
        public string lastname;
        public string phone;

        public Appointment()
        {
        }

        public Appointment(int stuff_id, int client_id, DateTime date, string service, bool payment,
            int row, string name, string lastname, string phone)
        {
            this.stuff_id  = stuff_id;
            this.client_id = client_id;
            this.date      = date;
            this.service   = service;
            this.payment   = payment;
            this.row       = row;
            this.name      = name;
            this.lastname  = lastname;
            this.phone     = phone;
        }

    }
}
