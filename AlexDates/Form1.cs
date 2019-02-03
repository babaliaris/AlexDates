using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlexDates
{
    public partial class Form1 : Form
    {
        private Database db;
        private MyTextBox[,] m_textBoxes;
        private Label[] m_Labels;
        private MyCheckBox[] m_Checkboxes;
        private MyComboBox[] m_ComboBoxes;
        private MyTextBox m_prevTxtBox = null;

        public Form1()
        {
            InitializeComponent();

            //Create the database connection.
            db = new Database("localhost", "babaliaris", "123456", "alex_dates");

            //Create the date graphics.
            CreateDateGraphics();

            //Update stuff choosers.
            UpdateStuffChoosers();

            //Update Service Choosers.
            UpdateServiceChoosers();

            //Update gift choosers.
            UpdateGiftChoosers();

            //Update the Dates Information.
            dates_date_info.Text = dates_date_picker.Value.ToString("dd/MM/yyyy");
            dates_day_info.Text  = DateTimeFormatInfo.CurrentInfo.GetDayName(dates_date_picker.Value.DayOfWeek);
        }





        //Create Dates Graphics.
        private void CreateDateGraphics()
        {

            //Clear Layout.
            dates_table_layout.Controls.Clear();
            dates_table_layout.ColumnStyles.Clear();
            dates_table_layout.RowStyles.Clear();

            //-----------------Add the styles-----------------//
            dates_table_layout.RowCount = 30;
            for (int x = 0; x < dates_table_layout.RowCount; x++)
                dates_table_layout.RowStyles.Add(new RowStyle() { Height = 100, SizeType = SizeType.Percent });

            dates_table_layout.ColumnCount = 6;
            for (int x = 0; x < dates_table_layout.ColumnCount; x++)
                dates_table_layout.ColumnStyles.Add(new ColumnStyle() { Width = 100, SizeType = SizeType.Percent });
            //-----------------Add the styles-----------------//

            //Add the layout.
            dates_group_box.Controls.Add(dates_table_layout);


            //------------Add the Column Names------------//
            Label label = new Label();
            label.Text = "Ώρα";
            dates_table_layout.Controls.Add(label, 0, 0);

            label = new Label();
            label.Text  = "Όνομα";
            dates_table_layout.Controls.Add(label, 1, 0);

            label = new Label();
            label.Text = "Επώνυμο";
            dates_table_layout.Controls.Add(label, 2, 0);

            label = new Label();
            label.Text = "Τηλέφωνο";
            dates_table_layout.Controls.Add(label, 3, 0);

            label = new Label();
            label.Text = "Υπηρεσία";
            dates_table_layout.Controls.Add(label, 4, 0);
            //------------Add the Column Names------------//


            string[] times = {"", "08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30",
                              "13:00", "13:30", "14:00", "14:30", "15:00", "15:30", "16:00", "16:30", "17:00", "17:30",
                              "18:00", "18:30", "19:00", "19:30", "20:00", "20:30", "21:30", "21:30", "22:00"};

            m_textBoxes  = new MyTextBox[29,3];
            m_Labels     = new Label[29];
            m_Checkboxes = new MyCheckBox[29];
            m_ComboBoxes = new MyComboBox[29];


            //Add Rows.
            for (int i = 1; i < 30; i++)
            {

                //Create the row time.
                label      = new Label();
                label.Text = times[i];
                label.Font = new Font("Arial", 12, FontStyle.Bold);
                dates_table_layout.Controls.Add(label, 0, i);

                //Store all the Label objects in an array.
                m_Labels[i - 1] = label;

                //Create a CheckBox and add it to the layout.
                MyCheckBox box = new MyCheckBox();
                box.Text       = "Πληρωμή";
                dates_table_layout.Controls.Add(box, 5, i);

                //Store it in the array.
                m_Checkboxes[i - 1] = box;
                box.row = i - 1;

                //Add CheckBox Event.
                box.CheckedChanged += new EventHandler(this.CheckBoxCheckedChagned);


                //Create a ComboBox and add it to the layout.
                MyComboBox comb    = new MyComboBox();
                comb.DropDownStyle = ComboBoxStyle.DropDownList;
                dates_table_layout.Controls.Add(comb, 4, i);

                //Store it into the array.
                m_ComboBoxes[i - 1] = comb;
                comb.row = i - 1;

                //Add ComboBox Handler.
                comb.SelectedIndexChanged += new EventHandler(this.ComboBoxIndexChanged);



                //Go through each column.
                for (int j = 1; j < 4; j++)
                {

                    //Create the TextBox and add it to the layout.
                    MyTextBox textBox = new MyTextBox();
                    textBox.Size      = new Size(150, textBox.Size.Height);
                    textBox.Font      = new Font("Arial", 8, FontStyle.Bold);
                    textBox.CharacterCasing = CharacterCasing.Upper;
                    dates_table_layout.Controls.Add(textBox, j, i);

                    //Add Event Handlers.
                    textBox.Enter       += new EventHandler(this.TextBoxFocuseGained);
                    textBox.Leave       += new EventHandler(this.TextBoxFocuseLost);
                    textBox.KeyPress    += new KeyPressEventHandler(this.TextBoxKeyEnterEvent);
                    textBox.TextChanged += new EventHandler(this.TextBoxTextChangedEvent);

                    //Store all the TextBox object in a 2D array.
                    m_textBoxes[i - 1, j - 1] = textBox;

                    //Set The Textbox Properties.
                    textBox.row    = i - 1;
                    textBox.column = j - 1;
                }
            }
        }




        private void ComboBoxIndexChanged(object sender, EventArgs e)
        {
            MyComboBox combo = (MyComboBox)sender;

            //If the event is Enabled.
            if (!combo.event_disabled)
            {
                bool success = AddDate(combo.row);

                if (success)
                    SubmitedTextChanged(m_textBoxes[combo.row, 0]);
            }
        }


        //CheckBox Event.
        private void CheckBoxCheckedChagned(object sender, EventArgs e)
        {
            MyCheckBox txt = (MyCheckBox)sender;

            //If the event is not disabled.
            if (!txt.disable_event)
            {
                bool success = AddDate(txt.row);

                if (success)
                    SubmitedTextChanged(m_textBoxes[txt.row, 0]);

                m_textBoxes[txt.row, 0].Enabled = !txt.Checked;
                m_textBoxes[txt.row, 1].Enabled = !txt.Checked;
                m_textBoxes[txt.row, 2].Enabled = !txt.Checked;
                m_ComboBoxes[txt.row].Enabled   = !txt.Checked;
            }

            else
            {
                m_textBoxes[txt.row, 0].Enabled = !txt.Checked;
                m_textBoxes[txt.row, 1].Enabled = !txt.Checked;
                m_textBoxes[txt.row, 2].Enabled = !txt.Checked;
                m_ComboBoxes[txt.row].Enabled   = !txt.Checked;
            }
        }





        //TextBox Focus Gain.
        private void TextBoxFocuseGained(object sender, EventArgs e)
        {
            MyTextBox txt = (MyTextBox)sender;

            txt.focuse_gained = true;

            m_Labels[txt.row].ForeColor = Color.Green;

            //Restore Previous Text.
            if (m_prevTxtBox != null && m_prevTxtBox.row != txt.row)
                SubmitedTextRestored(m_prevTxtBox);
        }





        //TextBox Focus Lost.
        private void TextBoxFocuseLost(object sender, EventArgs e)
        {
            MyTextBox txt = (MyTextBox)sender;

            m_Labels[txt.row].ForeColor = Color.Black;

            txt.focuse_gained = false;

            m_prevTxtBox = txt;

        }





        //TextBox Key Pressed.
        private void TextBoxKeyEnterEvent(object sender, KeyPressEventArgs e)
        {

            //Return (ENTER) key Pressed.
            if (e.KeyChar == (char)Keys.Return)
            {
                MyTextBox text_box = (MyTextBox)sender;
                bool success       = AddDate(text_box.row);

                if (success)
                    SubmitedTextChanged((MyTextBox)sender);
            }
        }





        //TextBox Text Changed.
        private void TextBoxTextChangedEvent(object sender, EventArgs e)
        {
            MyTextBox txt = (MyTextBox)sender;

            if (!txt.disableTextChanged)
            {
                m_textBoxes[txt.row, 0].ForeColor = Color.Black;
                m_textBoxes[txt.row, 1].ForeColor = Color.Black;
                m_textBoxes[txt.row, 2].ForeColor = Color.Black;
            }
        }




        //TextBox Submited Text Chagned.
        private void SubmitedTextChanged(MyTextBox txt)
        {
            m_textBoxes[txt.row, 0].ForeColor    = Color.Green;
            m_textBoxes[txt.row, 0].submited     = true;
            m_textBoxes[txt.row, 0].submitedText = m_textBoxes[txt.row, 0].Text;

            m_textBoxes[txt.row, 1].ForeColor    = Color.Green;
            m_textBoxes[txt.row, 1].submited     = true;
            m_textBoxes[txt.row, 1].submitedText = m_textBoxes[txt.row, 1].Text;

            m_textBoxes[txt.row, 2].ForeColor    = Color.Green;
            m_textBoxes[txt.row, 2].submited     = true;
            m_textBoxes[txt.row, 2].submitedText = m_textBoxes[txt.row, 2].Text;
        }





        //TextBox Restore Text.
        private void SubmitedTextRestored(MyTextBox txt)
        {

            //Disable Text Changed Event.
            m_textBoxes[txt.row, 0].disableTextChanged = true;
            m_textBoxes[txt.row, 1].disableTextChanged = true;
            m_textBoxes[txt.row, 2].disableTextChanged = true;

            m_textBoxes[txt.row, 0].ForeColor = Color.Green;
            m_textBoxes[txt.row, 0].Text      = m_textBoxes[txt.row, 0].submitedText;

            m_textBoxes[txt.row, 1].ForeColor = Color.Green;
            m_textBoxes[txt.row, 1].Text      = m_textBoxes[txt.row, 1].submitedText;

            m_textBoxes[txt.row, 2].ForeColor = Color.Green;
            m_textBoxes[txt.row, 2].Text      = m_textBoxes[txt.row, 2].submitedText;

            //Enable Text Changed Event.
            m_textBoxes[txt.row, 0].disableTextChanged = false;
            m_textBoxes[txt.row, 1].disableTextChanged = false;
            m_textBoxes[txt.row, 2].disableTextChanged = false;
        }






        //-----------------------------------------------Add Date-----------------------------------------------//
        private bool AddDate(int row)
        {

            //Stuff member was not selected.
            if (dates_stuff_chooser.SelectedIndex < 0)
            {
                MessageBox.Show("Δεν έχεις επιλέξη προσωπικό.", "Λάθος Δεδομένα", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }


            //Service was not selected.
            else if (m_ComboBoxes[row].SelectedIndex < 0)
            {
                MessageBox.Show("Δεν έχεις επιλέξη υπηρεσία.", "Λάθος Δεδομένα", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }


            //Get the user input.
            string[] stuff_keys = ((string)dates_stuff_chooser.SelectedItem).Split(' ');
            DateTime date_in    = dates_date_picker.Value;
            string name         = m_textBoxes[row, 0].Text;
            string lastname     = m_textBoxes[row, 1].Text;
            string phone        = m_textBoxes[row, 2].Text;
            string service      = m_ComboBoxes[row].Text;
            bool payment        = m_Checkboxes[row].Checked;


            //Client Variable.
            Client client_found = null;



            //If the phone entry is not empty
            //try to find a client account.
            if (m_textBoxes[row, 2].TextLength > 0)
            {

                //Search for a client.
                Client client = ChooseClient(m_textBoxes[row, 2].Text, null, false);

                //Client found.
                if (client != null)
                {
                    client_found = client;

                    name     = client.name;
                    lastname = client.lastname;

                    m_textBoxes[row, 0].Text = name;
                    m_textBoxes[row, 1].Text = lastname;
                }
            }

            //If the lastname entry is not empty
            //try to find a client account.
            else if (m_textBoxes[row, 1].TextLength > 0)
            {

                //Search for a client.
                Client client = ChooseClient(null, m_textBoxes[row, 1].Text, false);

                //Client found.
                if (client != null)
                {
                    client_found = client;

                    name     = client.name;
                    phone    = client.phone;

                    m_textBoxes[row, 0].Text = name;
                    m_textBoxes[row, 2].Text = phone;
                }
            }


            //Get the stuff member.
            Stuff stuff = db.GetStuff(stuff_keys[0], stuff_keys[1]);


            //If stuff in not null.
            if (stuff != null)
            {

                //If a client account was found.
                if (client_found != null)
                {
                    //Create the appointment.
                    Appointment appointment = new Appointment
                        (
                            stuff.id,
                            client_found.id,
                            date_in,
                            service,
                            payment,
                            row,
                            name,
                            lastname,
                            phone
                        );

                    db.AddAppointment(appointment);
                }


                //Client account not found.
                else
                {
                    //Create the appointment.
                    Appointment appointment = new Appointment
                        (
                            stuff.id,
                            -1,
                            date_in,
                            service,
                            payment,
                            row,
                            name,
                            lastname,
                            phone
                        );

                    db.AddAppointment(appointment);
                }
            }


            //Return false if the stuff member
            //could not be retrieved.
            else
                return false;


            return true;
        }
        //-----------------------------------------------Add Date-----------------------------------------------//







        //===================================Insert Stuff===================================//
        private void insert_stuff_button_Click(object sender, EventArgs e)
        {

            //Get user input.
            string choosed = (string)edit_stuff_chooser.SelectedItem;
            string name    = insert_stuff_name.Text;
            string lname   = insert_stuff_lname.Text;
            string email   = insert_stuff_email.Text;
            string phone   = insert_stuff_phone.Text;

            //Check user's input.
            if ((name.Length == 0 || lname.Length == 0) && !delete_stuff_radio.Checked)
            {
                MessageBox.Show("Το Όνομα και το Επώνυμο είναι υποχρεωτικά.", "Λάθος Δεδομένα",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //On edit, check if the chooser has selected something.
            else if ((delete_stuff_radio.Checked || edit_stuff_radio.Checked) && edit_stuff_chooser.SelectedIndex < 0)
            {
                MessageBox.Show("Πρέπει πρώτα να επιλέξης καποιο πρόσωπο για αυτήν την ενέργεια.", "Δεν επέλεξε κάποιο άτομο",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Insert Stuff.
            if (insert_stuff_radio.Checked)
            {

                //Create the new stuff.
                Stuff new_stuff = new Stuff(-1, name, lname, email, phone);

                //Insert him into the database.
                db.InsertStuff(new_stuff);

                //Update the stuff choosers.
                UpdateStuffChoosers();
            }


            //Edit Stuff.
            else if (edit_stuff_radio.Checked)
            {
                //Create the new stuff.
                Stuff new_stuff = new Stuff(-1, name, lname, email, phone);

                //Get the keys.
                string[] keys = choosed.Split(' ');

                //Edit stuff.
                db.EditStuff(new_stuff, keys[0], keys[1]);

                //Update the stuff choosers.
                UpdateStuffChoosers();
            }


            //Delete Stuff Member.
            else
            {
                string[] keys = choosed.Split(' ');

                db.DeleteStuff(keys[0], keys[1]);

                //Update the stuff choosers.
                UpdateStuffChoosers();
            }
        }
        //===================================Insert Stuff===================================//





        //Update Stuff Choosers.
        private void UpdateStuffChoosers()
        {

            //Clear the choosers.
            edit_stuff_chooser.Items.Clear();
            payments_stuff_chooser.Items.Clear();
            dates_stuff_chooser.Items.Clear();

            //Get teh keys from the database.
            List<string> keys = db.GetStuffKeys();

            //Fill the choosers.
            foreach (string k in keys)
            {
                edit_stuff_chooser.Items.Add(k);
                payments_stuff_chooser.Items.Add(k);
                dates_stuff_chooser.Items.Add(k);
            }
        }




        //Edit-Delete Stuff Chooser Index Changed.
        private void edit_stuff_chooser_SelectedIndexChanged(object sender, EventArgs e)
        {
            string choosed = (string)edit_stuff_chooser.SelectedItem;

            if (edit_stuff_chooser.SelectedIndex >= 0)
            {
                string[] keys = choosed.Split(' ');

                Stuff stuff = db.GetStuff(keys[0], keys[1]);

                if (stuff != null)
                {
                    insert_stuff_name.Text = stuff.name;
                    insert_stuff_lname.Text = stuff.lastname;
                    insert_stuff_email.Text = stuff.email;
                    insert_stuff_phone.Text = stuff.phone;
                }
            }
        }



        //Insert Stuff Radio Changed.
        private void insert_stuff_radio_CheckedChanged(object sender, EventArgs e)
        {
            if (insert_stuff_radio.Checked)
            {
                insert_stuff_button.Text = "Εισαγωγή Εργαζομένου";
            }
        }


        //Edit Stuff Radio Changed.
        private void edit_stuff_radio_CheckedChanged(object sender, EventArgs e)
        {
            if (edit_stuff_radio.Checked)
            {
                insert_stuff_button.Text = "Επεξεργασία Εργαζομένου";
            }
        }


        //Delete Stuff Radio Changed.
        private void delete_stuff_radio_CheckedChanged(object sender, EventArgs e)
        {
            if (delete_stuff_radio.Checked)
            {
                insert_stuff_button.Text = "Διαγραφή Εργαζομένου";
            }
        }






        //===================================Insert Client===================================//
        private void insert_client_button_Click(object sender, EventArgs e)
        {

            //Get User Input.
            string  name     = insert_clientname_entry.Text;
            string  lastname = insert_client_lname_entry.Text;
            string  email    = insert_client_email_entry.Text;
            string  phone    = insert_client_phone_entry.Text;
            string  desc     = insert_client_disc_entry.Text;


            //If everything is empty, return.
            if (name.Length == 0 && lastname.Length == 0 && email.Length == 0 && desc.Length == 0 && !delete_client_radio.Checked)
            {
                MessageBox.Show("Δεν γίνεται να έχεις όλα τα πεδία κενά!", "Λάθος Δεδομένα", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }


            //Insert a client into the system.
            if (insert_client_radio.Checked)
            {

                //Create the client.
                Client client = new Client(-1, name, lastname, email, phone, desc, 0);

                //Add him on the database.
                db.InsertClient(client);


                int id = db.GetLastClientID();
                //Update the id info.
                if (id != -1)
                    insert_client_code_info.Text = id.ToString();
            }


            //Edit a client.
            else if (edit_client_radio.Checked)
            {

                //The code info is empty.
                if (insert_client_code_info.TextLength <= 0)
                {
                    MessageBox.Show("Δεν έχεις επιλέξει κάποιον πελάτη για επεξεργασία." +
                        "Χρησιμοποίησε την αναζήτηση για να επιλέξεις κάποιον.", "Λάθος Επιλογή", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                //Get the id.
                int id = int.Parse(insert_client_code_info.Text);

                //Get the client.
                Client old_client = db.GetClient(id);

                //Create the client.
                Client client = new Client(-1, name, lastname, email, phone, desc, old_client.bonus);

                //Update the database.
                db.EditClient(client, id);
            }


            //Delete a Client.
            else
            {
                //The code info is empty.
                if (insert_client_code_info.TextLength <= 0)
                {
                    MessageBox.Show("Δεν έχεις επιλέξει κάποιον πελάτη για διαγραφή." +
                        "Χρησιμοποίησε την αναζήτηση για να επιλέξεις κάποιον.", "Λάθος Επιλογή", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                //Get the id.
                int id = int.Parse(insert_client_code_info.Text);

                //Get the client.
                Client client = db.GetClient(id);

                //Delete the client.
                db.DeleteClient(id, client.name, client.lastname);

                //Clear the info.
                ClearClientInfo();
            }
        }


        //Client Search Button Pressed.
        private void insert_client_search_button_Click(object sender, EventArgs e)
        {

            //Search By Phone.
            if (insert_client_phone_search_entry.TextLength > 0)
            {

                //Get the client.
                Client client = ChooseClient(insert_client_phone_search_entry.Text, null, true);

                //Update the info.
                if (client != null)
                    UpdateClientInfo(client);
            }


            //Search by last name.
            else if (insert_client_lname_search_entry.TextLength > 0)
            {
                //Get the client.
                Client client = ChooseClient(null, insert_client_lname_search_entry.Text, true);

                //Update the info.
                if (client != null)
                    UpdateClientInfo(client);
            }


            //Search by id.
            else
            {

                int id = -1;

                //See if the user gave a wrong number format.
                try
                {
                    id = int.Parse(insert_client_code_search_entry.Text);
                }

                //Indeed he gave a wrong format.
                catch (FormatException)
                {
                    MessageBox.Show("Ο κωδικός πρέπει να είναι ακέραιος αριθμός!", "Πρόβλημα Κωδικού",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                //Get the client from the database.
                Client client = db.GetClient(id);

                //If everything run smoothly.
                if (client != null)
                {

                    //Client found.
                    if (client.id != -1)
                        UpdateClientInfo(client);

                    //Client not found.
                    else
                        MessageBox.Show("Δεν βρέθηκε πελάτης με κωδικό: " + id,
                        "Δεν Βρέθηκε Πελάτης", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        //Update Client Info Entries.
        private void UpdateClientInfo(Client client)
        {
            insert_clientname_entry.Text   = client.name;
            insert_client_lname_entry.Text = client.lastname;
            insert_client_email_entry.Text = client.email;
            insert_client_phone_entry.Text = client.phone;
            insert_client_disc_entry.Text  = client.description;
            insert_client_code_info.Text   = client.id.ToString();
        }


        //Clear all the client's info.
        private void ClearClientInfo()
        {
            insert_clientname_entry.Clear();
            insert_client_lname_entry.Clear();
            insert_client_email_entry.Clear();
            insert_client_phone_entry.Clear();
            insert_client_disc_entry.Clear();
            insert_client_code_info.Clear();
        }
        //===================================Insert Client===================================//





        
        //==============================Enable Disable Client Search Entries==============================//
        private void insert_client_phone_search_entry_TextChanged(object sender, EventArgs e)
        {
            if (insert_client_phone_search_entry.TextLength > 0)
            {
                insert_client_lname_search_entry.Enabled = false;
                insert_client_code_search_entry.Enabled  = false;
            }

            else
            {
                insert_client_lname_search_entry.Enabled = true;
                insert_client_code_search_entry.Enabled  = true;
            }
        }

        private void insert_client_lname_search_entry_TextChanged(object sender, EventArgs e)
        {
            if (insert_client_lname_search_entry.TextLength > 0)
            {
                insert_client_phone_search_entry.Enabled = false;
                insert_client_code_search_entry.Enabled  = false;
            }

            else
            {
                insert_client_phone_search_entry.Enabled = true;
                insert_client_code_search_entry.Enabled  = true;
            }
        }

        private void insert_client_code_search_entry_TextChanged(object sender, EventArgs e)
        {
            if (insert_client_code_search_entry.TextLength > 0)
            {
                insert_client_phone_search_entry.Enabled = false;
                insert_client_lname_search_entry.Enabled = false;
            }

            else
            {
                insert_client_phone_search_entry.Enabled = true;
                insert_client_lname_search_entry.Enabled = true;
            }
        }
        //==============================Enable Disable Client Search Entries==============================//






        //Choose Client.
        public Client ChooseClient(string phone, string lastname, bool show_error)
        {
            //Get the clients from the database.
            List<Client> clients = db.GetClient(phone, lastname);

            //If there is utlist one cient.
            if (clients != null && clients.Count > 0)
            {

                //Only one client found.
                if (clients.Count == 1)
                    return clients[0];

                //More than one found.
                else
                {

                    //Open the client chooser.
                    Form chooser = new ChooseClientForm(clients);
                    chooser.ShowDialog();

                    //Get the combo box control.
                    Control[] constrols = chooser.Controls.Find("comboBox1", false);

                    //Cast it.
                    ComboBox combo = (ComboBox)constrols[0];

                    //Update the Client info.
                    if (combo.SelectedIndex >= 0)
                        return clients[combo.SelectedIndex];
                }
            }

            //Clients not found.
            else if (clients != null && clients.Count == 0 && show_error)
            {

                if (lastname == null)
                    MessageBox.Show("Δεν βρέθηκαν πελάτες με τηλέφωνο: " + phone,
                        "Δεν Βρέθηκαν Πελάτες", MessageBoxButtons.OK, MessageBoxIcon.Error);

                else
                    MessageBox.Show("Δεν βρέθηκαν πελάτες με επώνυμο: " + lastname,
                        "Δεν Βρέθηκαν Πελάτες", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


            return null;
        }



        //========================Client Radios Checked EVENTS========================//
        private void insert_client_radio_CheckedChanged(object sender, EventArgs e)
        {
            if (insert_client_radio.Checked)
                insert_client_button.Text = "Εισαγωγή Πελάτη";
        }

        private void edit_client_radio_CheckedChanged(object sender, EventArgs e)
        {
            if (edit_client_radio.Checked)
                insert_client_button.Text = "Επεξεργασία Πελάτη";
        }

        private void delete_client_radio_CheckedChanged(object sender, EventArgs e)
        {
            if (delete_client_radio.Checked)
                insert_client_button.Text = "Διαγραφή Πελάτη";
        }
        //========================Client Radios Checked EVENTS========================//





        //========================Services Radios Checked EVENTS========================//
        private void insert_service_radio_CheckedChanged(object sender, EventArgs e)
        {
            if (insert_service_radio.Checked)
                insert_service_button.Text = "Εισαγωγή Υπηρεσίας";
        }

        private void edit_service_radio_CheckedChanged(object sender, EventArgs e)
        {
            if (edit_service_radio.Checked)
                insert_service_button.Text = "Επεξεργασία Υπηρεσίας";
        }

        private void delete_service_radio_CheckedChanged(object sender, EventArgs e)
        {
            if (delete_service_radio.Checked)
                insert_service_button.Text = "Διαγραφή Υπηρεσίας";
        }
        //========================Services Radios Checked EVENTS========================//




        //===========================Insert-Edit-Delete Services===========================//
        private void insert_service_button_Click(object sender, EventArgs e)
        {
            string name = insert_service_name_entry.Text;
            float price = (float)insert_service_price_entry.Value;

            //Wrong fields.
            if (name.Length == 0 && !delete_service_radio.Checked)
            {
                MessageBox.Show("Το πεδίο Όνομα είναι υποχρεωτικό!", "Λάθος Δεδομένα", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }


            //Insert Service.
            if (insert_service_radio.Checked)
            {
                Item item = new Item(name, price);

                db.InsertService(item);

                UpdateServiceChoosers();
            }


            //Edit Service.
            else if (edit_service_radio.Checked)
            {
                if (insert_service_chooser.SelectedIndex >= 0)
                {
                    Item item = new Item(name, price);

                    db.EditService(item, (string)insert_service_chooser.SelectedItem);

                    UpdateServiceChoosers();
                }

                else
                    MessageBox.Show("Δεν έχεις επιλέξη κάποια υπηρεσία για επεξεργασία.",
                        "Λάθος Επιλογή", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            //Delete Service.
            else
            {
                if (insert_service_chooser.SelectedIndex >= 0)
                {
                    db.DeleteService((string)insert_service_chooser.SelectedItem);

                    UpdateServiceChoosers();
                }

                else
                    MessageBox.Show("Δεν έχεις επιλέξη κάποια υπηρεσία για διαγραφή.",
                        "Λάθος Επιλογή", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //Update Services Choosers.
        private void UpdateServiceChoosers()
        {
            List<string> keys = db.GetServiceKeys();

            insert_service_chooser.Items.Clear();

            foreach (MyComboBox combo in m_ComboBoxes)
                combo.Items.Clear();

            foreach (string key in keys)
            {
                insert_service_chooser.Items.Add(key);

                foreach (MyComboBox combo in m_ComboBoxes)
                    combo.Items.Add(key);
            }
        }


        //Insert Service Chooser Changed.
        private void insert_service_chooser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (insert_service_chooser.SelectedIndex >= 0)
            {
                Item service = db.GetService((string)insert_service_chooser.SelectedItem);

                insert_service_name_entry.Text   = service.name;
                insert_service_price_entry.Value = (decimal)service.price;
            }
        }
        //===========================Insert-Edit-Delete Services===========================//





        //========================Gifts Radios Checked EVENTS========================//
        private void gift_insert_radio_CheckedChanged(object sender, EventArgs e)
        {
            if (gift_insert_radio.Checked)
                bonus_gift_insert_btn.Text = "Εισαγωγή Δώρου";
        }

        private void gift_edit_radio_CheckedChanged(object sender, EventArgs e)
        {
            if (gift_edit_radio.Checked)
                bonus_gift_insert_btn.Text = "Επεξεργασία Δώρου";
        }

        private void gift_delete_radio_CheckedChanged(object sender, EventArgs e)
        {
            if (gift_delete_radio.Checked)
                bonus_gift_insert_btn.Text = "Διαγραφή Δώρου";
        }
        //========================Gifts Radios Checked EVENTS========================//





        //===========================Insert-Edit-Delete Gifts===========================//
        private void bonus_gift_insert_btn_Click(object sender, EventArgs e)
        {
            string name = bonus_gift_name_entry.Text;
            float price = (float)bonus_gift_price_entry.Value;

            //Wrong fields.
            if (name.Length == 0 && !gift_delete_radio.Checked)
            {
                MessageBox.Show("Το πεδίο Όνομα είναι υποχρεωτικό!", "Λάθος Δεδομένα", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }


            //Insert Service.
            if (gift_insert_radio.Checked)
            {
                Item item = new Item(name, price);

                db.InsertGift(item);

                UpdateGiftChoosers();
            }


            //Edit Service.
            else if (gift_edit_radio.Checked)
            {
                if (bonus_gift_chooser.SelectedIndex >= 0)
                {
                    Item item = new Item(name, price);

                    db.EditGift(item, (string)bonus_gift_chooser.SelectedItem);

                    UpdateGiftChoosers();
                }

                else
                    MessageBox.Show("Δεν έχεις επιλέξη κάποιο δώρο για επεξεργασία.",
                        "Λάθος Επιλογή", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            //Delete Service.
            else
            {
                if (bonus_gift_chooser.SelectedIndex >= 0)
                {
                    db.DeleteGift((string)bonus_gift_chooser.SelectedItem);

                    UpdateGiftChoosers();
                }

                else
                    MessageBox.Show("Δεν έχεις επιλέξη κάποιο δώρο για διαγραφή.",
                        "Λάθος Επιλογή", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //Update Services Choosers.
        private void UpdateGiftChoosers()
        {
            bonus_gift_chooser.Items.Clear();
            bonus_give_gift_chooser.Items.Clear();

            List<string> keys = db.GetGiftKeys();

            foreach (string key in keys)
            {
                bonus_gift_chooser.Items.Add(key);
                bonus_give_gift_chooser.Items.Add(key);
            }
        }


        //Insert Gift Chooser Changed.
        private void bonus_gift_chooser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bonus_gift_chooser.SelectedIndex >= 0)
            {
                Item gift = db.GetGift((string)bonus_gift_chooser.SelectedItem);

                bonus_gift_name_entry.Text   = gift.name;
                bonus_gift_price_entry.Value = (decimal)gift.price;
            }
        }
        //===========================Insert-Edit-Delete Gifts===========================//




        //================================Gifts Manager================================//

        //Gift Manual Changer.
        private void bonus_barcode_radio_CheckedChanged(object sender, EventArgs e)
        {

            //Barcode Gifter.
            if (bonus_barcode_radio.Checked)
            {
                bonus_phone_search_entry.Enabled = false;
                bonus_lnam_search_entry.Enabled  = false;
                bonus_search_client_btn.Enabled  = false;
                bonus_manual_give_btn.Enabled    = false;
                bonus_auto_code_search.Enabled   = true;
            }


            //Manual Gifter.
            else
            {
                bonus_phone_search_entry.Enabled = true;
                bonus_lnam_search_entry.Enabled  = true;
                bonus_search_client_btn.Enabled  = true;
                bonus_manual_give_btn.Enabled    = true;
                bonus_auto_code_search.Enabled   = false;
            }
        }

        private void bonus_phone_search_entry_TextChanged(object sender, EventArgs e)
        {
            if (bonus_phone_search_entry.TextLength > 0)
            {
                bonus_lnam_search_entry.Enabled = false;
            }

            else
            {
                bonus_lnam_search_entry.Enabled = true;
            }
        }

        private void bonus_lnam_search_entry_TextChanged(object sender, EventArgs e)
        {
            if (bonus_lnam_search_entry.TextLength > 0)
            {
                bonus_phone_search_entry.Enabled = false;
            }

            else
            {
                bonus_phone_search_entry.Enabled = true;
            }
        }



        //Give Gift Manually Button Pressed!!!!!!!!!!!!
        private void bonus_manual_give_btn_Click(object sender, EventArgs e)
        {

            //A client has been selected.
            if (bonus_gift_code_info.TextLength > 0)
            {

                //Get the client from the database.
                Client client = db.GetClient(int.Parse(bonus_gift_code_info.Text));


                //Give him the gift.
                if (client != null)
                    GiveGift(client);
            }


            //No client selected.
            else
                MessageBox.Show("Δεν έχεις επιλέξει κάποιον πελάτη." +
                        "Χρησιμοποίησε την αναζήτηση για να επιλέξεις κάποιον.", "Λάθος Επιλογή", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
        }



        //Give Gift.
        private void GiveGift(Client client)
        {

            //A gift has been selected.
            if (bonus_give_gift_chooser.SelectedIndex >= 0)
            {
                //Get the gift from the database.
                Item gift = db.GetGift((string)bonus_give_gift_chooser.SelectedItem);

                //Give the gift.
                db.AddHasGift(client, gift);

                //Update the client info.
                Client updated_client = db.GetClient(client.id);
                UpdateBonusClientInfo(updated_client);
            }

            //No gift selected.
            else
                MessageBox.Show("Δεν έχεις επιλέξει κάποιον δώρο.", "Λάθος Επιλογή", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
        }




        //Search Client To Give Gift Button.
        private void bonus_search_client_btn_Click(object sender, EventArgs e)
        {

            //Search by phone.
            if (bonus_phone_search_entry.TextLength > 0)
            {
                Client client = ChooseClient(bonus_phone_search_entry.Text, null, true);

                if (client != null)
                    UpdateBonusClientInfo(client);
            }


            //Search by name.
            else if (bonus_lnam_search_entry.TextLength > 0)
            {
                Client client = ChooseClient(null, bonus_lnam_search_entry.Text, true);

                if (client != null)
                    UpdateBonusClientInfo(client);
            }
        }


        //Update Bonus Client Info.
        private void UpdateBonusClientInfo(Client client)
        {
            bonus_gift_code_info.Text = client.id.ToString();
            bonus_name_info.Text      = client.name;
            bonus_lname_info.Text     = client.lastname;
            bonus_email_info.Text     = client.email;
            bonus_phone_info.Text     = client.phone;
            bonus_bonus_info.Text     = client.bonus.ToString();
        }



        //Auto Barcode.
        private void bonus_auto_code_search_KeyPress(object sender, KeyPressEventArgs e)
        {

            //If Enter is pressed.
            if (e.KeyChar == (char)Keys.Return)
            {
                //Some variables.
                int id = -1;
                string code = bonus_auto_code_search.Text.Replace("\r", "").Replace("\n", "");

                //Try to convert to integer.
                try
                {
                    id = int.Parse(code);
                }


                //Format was not good.
                catch (FormatException)
                {
                    MessageBox.Show("Ο κωδικός πρέπει να είναι ακέραιος αριθμός!", "Πρόβλημα Κωδικού",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                //Get the client from the db.
                Client client = db.GetClient(id);

                //do stuff if everything went smoothly.
                if (client != null)
                {

                    //Client found.
                    if (client.id != -1)
                        GiveGift(client);


                    //Client did not found.
                    else
                    {
                        MessageBox.Show("Δεν βρέθηκε πελάτης με κωδικό " + id, "Δεν βρέθηκε",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        //================================Gifts Manager================================//





        //================================Payments Manager================================//

        //Payments Stuff Chooser Changed.
        private void payments_stuff_chooser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (payments_stuff_chooser.SelectedIndex >= 0)
            {
                string[] keys = payments_stuff_chooser.Text.Split(' ');
                Stuff stuff   = db.GetStuff(keys[0], keys[1]);

                if (stuff != null)
                {
                    UpdatePaymentInfo(stuff, payments_from_entry.Value, payments_to_entry.Value);
                }
            }
        }


        //Apply Payment.
        private void button7_Click(object sender, EventArgs e)
        {

            //Get the user info.
            string name      = payments_name_info.Text;
            string lastname  = payments_lastname_info.Text;
            DateTime date_in = payments_from_entry.Value;
            DateTime date_to = payments_to_entry.Value;
            float amount     = (float)payments_amount_entry.Value;

            //Check.
            if (name.Length <= 0 || lastname.Length <= 0)
            {
                MessageBox.Show("Δεν έχεις επιλέξει κάποιο προσωπικό.", "Λάθος Επιλογή", MessageBoxButtons.OK
                    , MessageBoxIcon.Error);
                return;
            }

            //Get the stuff from the database.
            Stuff stuff = db.GetStuff(name, lastname);

            //Set the payment.
            if (stuff != null)
            {

                //Set the payment.
                db.SetPayment(stuff, amount, date_in, date_to);

                //Update the info.
                UpdatePaymentInfo(stuff, date_in, date_to);
            }
        }


        //Update payments amount.
        private void UpdatePaymentInfo(Stuff stuff, DateTime date_in, DateTime date_to)
        {
            payments_name_info.Text     = stuff.name;
            payments_lastname_info.Text = stuff.lastname;
            payments_email_info.Text    = stuff.email;
            payments_phone_info.Text    = stuff.phone;

            float amount = db.GetPayment(stuff, date_in, date_to);

            if (amount == -1)
                payments_amount_info.Text = "0";

            else
                payments_amount_info.Text = amount.ToString();
        }



        //Date From Updated.
        private void payments_from_entry_ValueChanged(object sender, EventArgs e)
        {
            if (payments_stuff_chooser.SelectedIndex >= 0)
            {
                string[] keys = payments_stuff_chooser.Text.Split(' ');
                Stuff stuff = db.GetStuff(keys[0], keys[1]);

                if (stuff != null)
                {
                    UpdatePaymentInfo(stuff, payments_from_entry.Value, payments_to_entry.Value);
                }
            }
        }

        //Date To Updated.
        private void payments_to_entry_ValueChanged(object sender, EventArgs e)
        {
            if (payments_stuff_chooser.SelectedIndex >= 0)
            {
                string[] keys = payments_stuff_chooser.Text.Split(' ');
                Stuff stuff = db.GetStuff(keys[0], keys[1]);

                if (stuff != null)
                {
                    UpdatePaymentInfo(stuff, payments_from_entry.Value, payments_to_entry.Value);
                }
            }
        }
        //================================Payments Manager================================//





        //Dates Date Picker Changed.
        private void dates_date_picker_ValueChanged(object sender, EventArgs e)
        {
            dates_date_info.Text = dates_date_picker.Value.ToString("dd/MM/yyyy");
            dates_day_info.Text  = DateTimeFormatInfo.CurrentInfo.GetDayName(dates_date_picker.Value.DayOfWeek);

            UpdateDatesInfo();
        }

        //Dates Next Button Clicked.
        private void dates_next_btn_Click(object sender, EventArgs e)
        {
            dates_date_picker.Value = dates_date_picker.Value.AddDays(1);

            UpdateDatesInfo();
        }

        //Dates Previous Button Clicked.
        private void dates_prev_btn_Click(object sender, EventArgs e)
        {
            dates_date_picker.Value = dates_date_picker.Value.AddDays(-1);

            UpdateDatesInfo();
        }


        //Dates Stuff Chooser Changed.
        private void dates_stuff_chooser_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDatesInfo();
        }


        //Update Dates Info.
        private void UpdateDatesInfo()
        {

            //Clear everything.
            for (int row = 0; row < 29; row++)
            {
                m_textBoxes[row, 0].Clear();
                m_textBoxes[row, 1].Clear();
                m_textBoxes[row, 2].Clear();

                m_ComboBoxes[row].event_disabled = true;
                m_ComboBoxes[row].SelectedIndex  = -1;
                m_ComboBoxes[row].event_disabled = false;

                m_Checkboxes[row].disable_event = true;
                m_Checkboxes[row].Checked       = false;
                m_Checkboxes[row].disable_event = false;
            }



            //If a stuff mebmer has been selected.
            if (dates_stuff_chooser.SelectedIndex >= 0)
            {
                string[] keys = ((string)dates_stuff_chooser.SelectedItem).Split(' ');

                Stuff stuff = db.GetStuff(keys[0], keys[1]);

                //Stuff retrieved successfully.
                if (stuff != null)
                {
                    //Get the date.
                    DateTime date = dates_date_picker.Value;

                    List<Appointment> appointments = db.GetAppointments(stuff, date);

                    foreach (Appointment appoint in appointments)
                    {

                        //Load the text inputs.
                        m_textBoxes[appoint.row, 0].Text = appoint.name;
                        m_textBoxes[appoint.row, 1].Text = appoint.lastname;
                        m_textBoxes[appoint.row, 2].Text = appoint.phone;

                        //Make the text color to green.
                        m_textBoxes[appoint.row, 0].ForeColor = Color.Green;
                        m_textBoxes[appoint.row, 1].ForeColor = Color.Green;
                        m_textBoxes[appoint.row, 2].ForeColor = Color.Green;

                        //Load the service chooser.
                        m_ComboBoxes[appoint.row].event_disabled = true;
                        m_ComboBoxes[appoint.row].SelectedItem   = appoint.service;
                        m_ComboBoxes[appoint.row].event_disabled = false;

                        //Load the payment checkbox.
                        m_Checkboxes[appoint.row].disable_event = true;
                        m_Checkboxes[appoint.row].Checked       = appoint.payment;
                        m_Checkboxes[appoint.row].disable_event = false;
                    }
                }
            }
        }
    }
}
