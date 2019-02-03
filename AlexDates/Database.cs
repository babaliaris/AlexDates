using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Windows.Forms;

namespace AlexDates
{
    class Database
    {
        //__________Class Variables__________//
        private MySqlConnection connection;
        //__________Class Variables__________//



        //Constructor.
        public Database(string ip, string username, string pass, string database)
        {
            string connString = "SERVER=" + ip + ";" + "DATABASE=" + database + ";UID=" + username + ";PASSWORD=" + pass + ";";
            connection = new MySqlConnection(connString);
        }





        //Establish Connection.
        private bool Connect()
        {

            //Try creating the connection.
            try
            {
                connection.Open();
                return true;
            }


            //Connection Failed.
            catch (MySqlException)
            {
                MessageBox.Show("Το πρόγραμμα δεν μπόρεσε να συνδεθεί στην Βάση Δεδομένων. Παρακαλώ ελέγξτε αν η Βάση Δεδομένων" +
                    " είναι ανοιχτεί.", "Πρόβλημα Σύνδεσης Στην Βάση Δεδομένων", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }




        //Close the connection.
        private void Close()
        {
            connection.Close();
        }



        //Show Uknown Error.
        private void UknownError(string func, int number, int error_code, uint code, string message)
        {
            MessageBox.Show("Ένα απροσδιόριστο πρόβλημα συνέβει στην βάση δεδομένων. Δοκίμασε ξανά αργότερα." +
                        "\n\nΣυνάρτηση: " + func + "\nΑριθμός Σφάλματος: " + number + "\nΚώδικας Σφάλματος: "
                        + error_code + "\nΓενικός Κώδικας: " + code + "" + "\nΜήνυμα: " + message,
                        "Απροσδιόριστο Πρόβλημα", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        
        //Insert Stuff.
        public void InsertStuff(Stuff stuff)
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection   = connection;

                //Create the procedure.
                cmd.CommandText = "add_stuff";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add the parameters.
                cmd.Parameters.AddWithValue("stuff_name", stuff.name);
                cmd.Parameters.AddWithValue("stuff_lname", stuff.lastname);
                cmd.Parameters.AddWithValue("stuff_email", stuff.email);
                cmd.Parameters.AddWithValue("stuff_phone", stuff.phone);


                //Try executing the command.
                try
                {

                    //Execute the commant.
                    cmd.ExecuteNonQuery();

                    //Show success message.
                    MessageBox.Show("Ο/Η "+stuff.name+" "+stuff.lastname+", μπήκε στο σύστημα!",
                        "Επιτυχής Εισαγωγή", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {

                    //Duplicate entry of product name.
                    if (e.Number == 1062)
                        MessageBox.Show("Ο/Η " + stuff.name + " " + stuff.lastname + ", υπάρχει ήδη!", "Υπάρχει",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //Uknown Error.
                    else
                        this.UknownError("InsertStuff", e.Number, e.ErrorCode, e.Code, e.Message);

                }

                //Close the connection.
                this.Close();
            }
        }






        //Get Stuff keys.
        public List<string> GetStuffKeys()
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;

                //Create the procedure.
                cmd.CommandText = "get_stuff_keys";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;


                //Try executing the command.
                try
                {
                    List<string> keys = new List<string>();

                    MySqlDataReader data = cmd.ExecuteReader();

                    while (data.Read())
                        keys.Add((string)data["firstname"] + " " + (string)data["lastname"]);

                    data.Close();
                    this.Close();

                    return keys;
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                   this.UknownError("GetStuffKeys", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }


            //Retrun empty list.
            return new List<string>();
        }





        //Edit Stuff.
        public void EditStuff(Stuff stuff, string name, string lastname)
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection  = connection;

                //Create the procedure.
                cmd.CommandText = "edit_stuff";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add the parameters.
                cmd.Parameters.AddWithValue("key_name", name);
                cmd.Parameters.AddWithValue("key_lname", lastname);
                cmd.Parameters.AddWithValue("stuff_name",  stuff.name);
                cmd.Parameters.AddWithValue("stuff_lname", stuff.lastname);
                cmd.Parameters.AddWithValue("stuff_email", stuff.email);
                cmd.Parameters.AddWithValue("stuff_phone", stuff.phone);


                //Try executing the command.
                try
                {

                    //Execute the commant.
                    cmd.ExecuteNonQuery();

                    //Show success message.
                    MessageBox.Show("Ο/Η " + name + " " + lastname + ", ενημερώθηκε με επιτυχία!",
                        "Επιτυχής Επεξεργασία", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {

                    //Duplicate entry of product name.
                    if (e.Number == 1062)
                        MessageBox.Show("Ο/Η " + stuff.name + " " + stuff.lastname + ", υπάρχει ήδη!", "Υπάρχει",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //Uknown Error.
                    else
                        this.UknownError("EditStuff", e.Number, e.ErrorCode, e.Code, e.Message);

                }

                //Close the connection.
                this.Close();
            }
        }






        //Get Stuff.
        public Stuff GetStuff(string name, string lastname)
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;

                //Create the procedure.
                cmd.CommandText = "get_stuff";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add Parameters.
                cmd.Parameters.AddWithValue("key_name", name);
                cmd.Parameters.AddWithValue("key_lname", lastname);

                //Try executing the command.
                try
                {

                    Stuff new_stuff = null;
                    MySqlDataReader data = cmd.ExecuteReader();

                    while (data.Read())
                        new_stuff = new Stuff((int)data["id"], (string)data["firstname"],
                            (string)data["lastname"], (string)data["email"], (string)data["phone"]);

                    data.Close();
                    this.Close();

                    return new_stuff;
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    this.UknownError("GetStuff", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }


            return null;
        }





        //Delete Stuff.
        public void DeleteStuff(string name, string lastname)
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;

                //Create the procedure.
                cmd.CommandText = "delete_stuff";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add Parameters.
                cmd.Parameters.AddWithValue("key_name", name);
                cmd.Parameters.AddWithValue("key_lname", lastname);

                //Try executing the command.
                try
                {

                    DialogResult result = MessageBox.Show("Θέλεις σίγουρα να διαγράψεις τον " + name + " " + lastname + " ;",
                        "Σίγουρα?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Ο/Η " + name + " " + lastname + ", διαγράφηκε από το σύστημα.",
                        "Επιτυχής Διαγραφή.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    this.UknownError("DeleteStuff", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }
        }










        //Insert Client.
        public void InsertClient(Client client)
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;

                //Create the procedure.
                cmd.CommandText = "add_client";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add the parameters.
                cmd.Parameters.AddWithValue("client_name", client.name);
                cmd.Parameters.AddWithValue("client_lname", client.lastname);
                cmd.Parameters.AddWithValue("client_email", client.email);
                cmd.Parameters.AddWithValue("client_phone", client.phone);
                cmd.Parameters.AddWithValue("client_disc", client.description);
                cmd.Parameters.AddWithValue("client_bonus", client.bonus);


                //Try executing the command.
                try
                {

                    //Execute the commant.
                    cmd.ExecuteNonQuery();

                    //Show success message.
                    MessageBox.Show("Ο/Η " + client.name + " " + client.lastname + ", μπήκε στο σύστημα!",
                        "Επιτυχής Εισαγωγή", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    this.UknownError("InsertClient", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }
        }





        //Edit Client.
        public void EditClient(Client client, int id)
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection   = connection;

                //Create the procedure.
                cmd.CommandText = "edit_client";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add the parameters.
                cmd.Parameters.AddWithValue("key_int", id);
                cmd.Parameters.AddWithValue("client_name", client.name);
                cmd.Parameters.AddWithValue("client_lname", client.lastname);
                cmd.Parameters.AddWithValue("client_email", client.email);
                cmd.Parameters.AddWithValue("client_phone", client.phone);
                cmd.Parameters.AddWithValue("client_disc", client.description);
                cmd.Parameters.AddWithValue("client_bonus", client.bonus);


                //Try executing the command.
                try
                {

                    //Execute the commant.
                    cmd.ExecuteNonQuery();

                    //Show success message.
                    MessageBox.Show("Ο/Η " + client.name + " " + client.lastname + ", ενημερώθηκε με επιτυχία!",
                        "Επιτυχής Επεξεργασία", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    this.UknownError("EditClient", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }
        }






        //Get Client.
        public Client GetClient(int id)
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection   = connection;


                //Create the procedure.
                cmd.CommandText = "get_client";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add Parameters.
                cmd.Parameters.AddWithValue("key_int", id);

                //Try executing the command.
                try
                {

                    Client new_client    = new Client(-1, "", "", "", "", "", 0);
                    MySqlDataReader data = cmd.ExecuteReader();

                    while (data.Read())
                        new_client = new Client((int)data["id"], (string)data["firstname"],
                            (string)data["lastname"], (string)data["email"], (string)data["phone"], 
                            (string)data["descrip"], (float)data["bonus"]);

                    data.Close();
                    this.Close();

                    return new_client;
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    this.UknownError("GetClient", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }


            return null;
        }






        //Get Client By phone or Lastname.
        public List<Client> GetClient(string phone, string lastname)
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection   = connection;

                //Search clients by phone.
                if (lastname == null)
                {
                    //Create the procedure.
                    cmd.CommandText = "get_client_by_phone";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    //Add Parameters.
                    cmd.Parameters.AddWithValue("key_phone", phone);
                }


                //Search clients by last name.
                else
                {
                    //Create the procedure.
                    cmd.CommandText = "get_client_by_lastname";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    //Add Parameters.
                    cmd.Parameters.AddWithValue("key_lname", lastname);
                }

                //Try executing the command.
                try
                {

                    List<Client> clients = new List<Client>();

                    MySqlDataReader data = cmd.ExecuteReader();

                    while (data.Read())
                    {
                        clients.Add( new Client((int)data["id"], (string)data["firstname"],
                            (string)data["lastname"], (string)data["email"], (string)data["phone"],
                            (string)data["descrip"], (float)data["bonus"]));
                    }

                    data.Close();
                    this.Close();

                    return clients;
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    this.UknownError("GetClientByPhoneOrLastname", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }


            return null;
        }





        //Get Last Client ID.
        public int GetLastClientID()
        {

            //Connect to execute command.
            if (this.Connect())
            {

                string q = "SELECT id FROM clients ORDER BY id DESC LIMIT 1;";

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand(q, connection);

                //Try executing the command.
                try
                {

                    int id = -1;

                    MySqlDataReader data = cmd.ExecuteReader();

                    while (data.Read())
                    {
                        id = (int)data["id"];
                    }

                    data.Close();
                    this.Close();

                    return id;
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    this.UknownError("GetLastClientID", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }


            return -1;
        }





        //Delete Client.
        public void DeleteClient(int id, string name, string lastname)
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection   = connection;

                //Create the procedure.
                cmd.CommandText = "delete_client";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add Parameters.
                cmd.Parameters.AddWithValue("key_int", id);

                //Try executing the command.
                try
                {

                    DialogResult result = MessageBox.Show("Θέλεις σίγουρα να διαγράψεις τον " + name + " " + lastname + " ;",
                        "Σίγουρα?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Ο/Η " + name + " " + lastname + ", διαγράφηκε από το σύστημα.",
                        "Επιτυχής Διαγραφή.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    this.UknownError("DeleteClient", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }
        }





        //Insert Service.
        public void InsertService(Item service)
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection   = connection;

                //Create the procedure.
                cmd.CommandText = "add_service";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add the parameters.
                cmd.Parameters.AddWithValue("service_name", service.name);
                cmd.Parameters.AddWithValue("service_price", service.price);


                //Try executing the command.
                try
                {

                    //Execute the commant.
                    cmd.ExecuteNonQuery();

                    //Show success message.
                    MessageBox.Show("Η υπηρεσία " +service.name+ ", μπήκε στο σύστημα!",
                        "Επιτυχής Εισαγωγή", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    //Duplicate entry of service name.
                    if (e.Number == 1062)
                        MessageBox.Show("Η υπηρεσία " + service.name + ", υπάρχει ήδη!", "Υπάρχει",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                    else
                        this.UknownError("InsertService", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }
        }





        //Get Service Geys.
        public List<string> GetServiceKeys()
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;

                //Create the procedure.
                cmd.CommandText = "get_service_keys";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;


                //Try executing the command.
                try
                {
                    List<string> keys = new List<string>();

                    MySqlDataReader data = cmd.ExecuteReader();

                    while (data.Read())
                    {
                        keys.Add( (string)data["servicename"] );
                    }

                    data.Close();
                    this.Close();

                    return keys;
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    this.UknownError("InsertService", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }

            return new List<string>();
        }





        //Edit Service.
        public void EditService(Item service, string name)
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection   = connection;

                //Create the procedure.
                cmd.CommandText = "edit_service";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add the parameters.
                cmd.Parameters.AddWithValue("service_key", name);
                cmd.Parameters.AddWithValue("service_name", service.name);
                cmd.Parameters.AddWithValue("service_price", service.price);


                //Try executing the command.
                try
                {

                    //Execute the commant.
                    cmd.ExecuteNonQuery();

                    //Show success message.
                    MessageBox.Show("Η υπηρεσία " + service.name + ", ενημερώθηκε με επιτυχία!",
                        "Επιτυχής Ενημέρωση", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    //Duplicate entry of service name.
                    if (e.Number == 1062)
                        MessageBox.Show("Η υπηρεσία " + service.name + ", υπάρχει ήδη!", "Υπάρχει",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                    else
                        this.UknownError("EditService", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }
        }





        //Get Service.
        public Item GetService(string name)
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection   = connection;


                //Create the procedure.
                cmd.CommandText = "get_service";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add Parameters.
                cmd.Parameters.AddWithValue("service_key", name);

                //Try executing the command.
                try
                {

                    Item item = null;
                    MySqlDataReader data = cmd.ExecuteReader();

                    while (data.Read())
                        item = new Item((string)data["servicename"], (float)data["price"]);

                    data.Close();
                    this.Close();

                    return item;
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    this.UknownError("GetService", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }


            return null;
        }






        //Delete Service.
        public void DeleteService(string name)
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;

                //Create the procedure.
                cmd.CommandText = "delete_service";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add Parameters.
                cmd.Parameters.AddWithValue("service_key", name);

                //Try executing the command.
                try
                {

                    DialogResult result = MessageBox.Show("Θέλεις σίγουρα να διαγράψεις την υπηρεσία " + name +" ;",
                        "Σίγουρα?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Η υπηρεσία" + name +", διαγράφηκε από το σύστημα.",
                        "Επιτυχής Διαγραφή.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    this.UknownError("DeleteService", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }
        }





        //Insert Gift.
        public void InsertGift(Item gift)
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection   = connection;

                //Create the procedure.
                cmd.CommandText = "add_gift";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add the parameters.
                cmd.Parameters.AddWithValue("gift_name", gift.name);
                cmd.Parameters.AddWithValue("gift_price", gift.price);


                //Try executing the command.
                try
                {

                    //Execute the commant.
                    cmd.ExecuteNonQuery();

                    //Show success message.
                    MessageBox.Show("Το δώρο " + gift.name + ", μπήκε στο σύστημα!",
                        "Επιτυχής Εισαγωγή", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    //Duplicate entry of service name.
                    if (e.Number == 1062)
                        MessageBox.Show("Το δώρο " + gift.name + ", υπάρχει ήδη!", "Υπάρχει",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                    else
                        this.UknownError("InsertGift", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }
        }





        //Get Gift Geys.
        public List<string> GetGiftKeys()
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection   = connection;

                //Create the procedure.
                cmd.CommandText = "get_gift_keys";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;


                //Try executing the command.
                try
                {
                    List<string> keys = new List<string>();

                    MySqlDataReader data = cmd.ExecuteReader();

                    while (data.Read())
                    {
                        keys.Add((string)data["giftname"]);
                    }

                    data.Close();
                    this.Close();

                    return keys;
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    this.UknownError("GetGiftKeys", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }

            return new List<string>();
        }





        //Edit Gift.
        public void EditGift(Item gift, string name)
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;

                //Create the procedure.
                cmd.CommandText = "edit_gift";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add the parameters.
                cmd.Parameters.AddWithValue("gift_key", name);
                cmd.Parameters.AddWithValue("gift_name", gift.name);
                cmd.Parameters.AddWithValue("gift_price", gift.price);


                //Try executing the command.
                try
                {

                    //Execute the commant.
                    cmd.ExecuteNonQuery();

                    //Show success message.
                    MessageBox.Show("Το δώρο " + gift.name + ", ενημερώθηκε με επιτυχία!",
                        "Επιτυχής Ενημέρωση", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    //Duplicate entry of service name.
                    if (e.Number == 1062)
                        MessageBox.Show("Το δώρο " + gift.name + ", υπάρχει ήδη!", "Υπάρχει",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                    else
                        this.UknownError("EditGift", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }
        }





        //Get Gift.
        public Item GetGift(string name)
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;


                //Create the procedure.
                cmd.CommandText = "get_gift";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add Parameters.
                cmd.Parameters.AddWithValue("gift_key", name);

                //Try executing the command.
                try
                {

                    Item item = null;
                    MySqlDataReader data = cmd.ExecuteReader();

                    while (data.Read())
                        item = new Item((string)data["giftname"], (float)data["price"]);

                    data.Close();
                    this.Close();

                    return item;
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    this.UknownError("GetGift", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }


            return null;
        }






        //Delete Gift.
        public void DeleteGift(string name)
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;

                //Create the procedure.
                cmd.CommandText = "delete_gift";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add Parameters.
                cmd.Parameters.AddWithValue("gift_key", name);

                //Try executing the command.
                try
                {

                    DialogResult result = MessageBox.Show("Θέλεις σίγουρα να διαγράψεις το δώρο " + name + " ;",
                        "Σίγουρα?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Το δώρο" + name + ", διαγράφηκε από το σύστημα.",
                        "Επιτυχής Διαγραφή.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    this.UknownError("DeleteGift", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }
        }






        //Update Client Bonus.
        public bool UpdateClientBonus(Client client)
        {
            bool ok = true;

            //Connect to execute command.
            if (this.Connect())
            {

                string q = "UPDATE clients SET bonus = " + client.bonus.ToString() + " WHERE id = " + client.id.ToString() + ";";

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();


                //Try executing the command.
                try
                {
                    cmd.ExecuteNonQuery();
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    ok = false;
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    ok = false;
                    this.UknownError("UpdateClientBonus", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }

            return ok;
        }






        //Edit Gift.
        public void AddHasGift(Client client, Item gift)
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection   = connection;

                //Create the procedure.
                cmd.CommandText = "add_hasgift";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add the parameters.
                cmd.Parameters.AddWithValue("in_client_id", client.id);
                cmd.Parameters.AddWithValue("in_gift_id", gift.name);


                //Try executing the command.
                try
                {

                    //Show a confirmation message.
                    DialogResult result = MessageBox.Show("Πρόκειται να δώσεις στον/στην "+client.name+" "+client.lastname+" με κωδικό" +
                        " πελάτη "+client.id+" , το δώρο "+gift.name+". Σίγουρα θέλεις να συνεχίσεις;",
                        "Σίγουρα?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


                    //If yes.
                    if (result == DialogResult.Yes)
                    {

                        //If clients bonus is enough.
                        if (client.bonus >= gift.price)
                        {
                            cmd.ExecuteNonQuery();

                            //Show success.
                            MessageBox.Show("Το δώρο " + gift.name + ", προστέθηκε με επυτιχία!", "Επιτυχία!",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }


                        //Not enough bonus.
                        else
                            //Show success.
                            MessageBox.Show("To ποσό του πελάτη δεν επαρκή για την εξαργύρωση του δώρου " + gift.name, "Δεν Επαρκή",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                   this.UknownError("AddHasGift", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }
        }





        //Set payment.
        public void SetPayment(Stuff stuff, float amount, DateTime date_in, DateTime date_to)
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection   = connection;

                //Create the procedure.
                cmd.CommandText = "set_payment";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add the parameters.
                cmd.Parameters.AddWithValue("in_stuff_id", stuff.id);
                cmd.Parameters.AddWithValue("in_amount", amount);
                cmd.Parameters.AddWithValue("in_date_from", date_in);
                cmd.Parameters.AddWithValue("in_date_to", date_to);


                //Try executing the command.
                try
                {
                    //Execute the commant.
                    cmd.ExecuteNonQuery();
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                   this.UknownError("SetPayment", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }
        }





        //Get payment.
        public float GetPayment(Stuff stuff, DateTime date_in, DateTime date_to)
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Query.
                string q = "SELECT amount FROM payments WHERE stuff_id = " + stuff.id + " AND date_from = '" + date_in.ToString("yyyy/MM/dd")
                    + "' AND date_to = '" + date_to.ToString("yyyy/MM/dd") + "';";

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand(q, connection);


                //Try executing the command.
                try
                {
                    float amount = -1;

                    MySqlDataReader data = cmd.ExecuteReader();

                    while (data.Read())
                        amount = (float)data["amount"];


                    data.Close();
                    this.Close();

                    return amount;
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    this.UknownError("GetPayment", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }

            return -1;
        }






        //Set payment.
        public bool AddAppointment(Appointment appointment)
        {
            bool ok = true;

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection   = connection;

                //Create the procedure.
                cmd.CommandText = "add_appointment";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add the parameters.
                cmd.Parameters.AddWithValue("in_stuff_id", appointment.stuff_id);
                cmd.Parameters.AddWithValue("in_client_id", appointment.client_id);
                cmd.Parameters.AddWithValue("in_date", appointment.date);
                cmd.Parameters.AddWithValue("in_service", appointment.service);
                cmd.Parameters.AddWithValue("in_payment", appointment.payment);
                cmd.Parameters.AddWithValue("in_row", appointment.row);
                cmd.Parameters.AddWithValue("in_name", appointment.name);
                cmd.Parameters.AddWithValue("in_lname", appointment.lastname);
                cmd.Parameters.AddWithValue("in_phone", appointment.phone);


                //Try executing the command.
                try
                {
                    //Execute the commant.
                    cmd.ExecuteNonQuery();
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    ok = false;
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    ok = false;
                    this.UknownError("AddAppointment", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }

            return ok;
        }





        //Get payment.
        public List<Appointment> GetAppointments(Stuff stuff, DateTime date)
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Query.
                string q = "SELECT * FROM appointments WHERE stuff_id = " + stuff.id + " AND date_in = '" + date.ToString("yyyy/MM/dd") + "';";

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand(q, connection);


                //Try executing the command.
                try
                {

                    //Create an empty list.
                    List<Appointment> appointmets = new List<Appointment>();

                    //Execute the query.
                    MySqlDataReader data = cmd.ExecuteReader();

                    //Get all the appointments into the list.
                    while (data.Read())
                        appointmets.Add(new Appointment
                                (
                                    (int)data["stuff_id"],
                                    (int)data["client_id"],
                                    (DateTime)data["date_in"],
                                    (string)data["service_name"],
                                    (bool)data["payment_done"],
                                    (int)data["row_index"],
                                    (string)data["client_name"],
                                    (string)data["client_lastname"],
                                    (string)data["client_phone"]

                                )
                            );


                    data.Close();
                    this.Close();

                    return appointmets;
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    this.UknownError("GetAppointments", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }


            //Return an empty list.
            return new List<Appointment>();
        }
    }
}
