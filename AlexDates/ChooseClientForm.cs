using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlexDates
{
    public partial class ChooseClientForm : Form
    {

        public List<Client> clients;
        public Client selected;

        
        //Constructor.
        public ChooseClientForm(List<Client> clients)
        {
            InitializeComponent();

            this.clients = clients;

            foreach (Client c in clients)
                comboBox1.Items.Add(c.name + " " + c.lastname + " " + c.id);
        }


        //Combo box changed.
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                textBox1.Text = clients[comboBox1.SelectedIndex].name;
                textBox2.Text = clients[comboBox1.SelectedIndex].lastname;
                textBox3.Text = clients[comboBox1.SelectedIndex].email;
                textBox4.Text = clients[comboBox1.SelectedIndex].phone;
                textBox5.Text = clients[comboBox1.SelectedIndex].id.ToString();
                textBox6.Text = clients[comboBox1.SelectedIndex].description;
            }
        }


        //Choose button clicked.
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
                this.Close();

            else
                MessageBox.Show("Δεν επέλεξες κάποιον πελάτη.", "Πρέπει να επιλέξεις", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
