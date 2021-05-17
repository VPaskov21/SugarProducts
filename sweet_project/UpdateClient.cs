using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace sweet_project
{
    public partial class UpdateClient : Form
    {
        public UpdateClient()
        {
            InitializeComponent();
        }

        MainForm mainForm = new MainForm();
        AddClient addClient = new AddClient();
        List<Client> clients = new List<Client>();

        static int position = 0;

        Client client;

        class Client
        {
            int n_id;
            string str_name;
            string str_address;
            string str_phone;

            public Client(int id, string name, string address, string phone)
            {
                n_id = id;
                str_name = name;
                str_address = address;
                str_phone = phone;
            }

            public int getClientId()
            {
                return n_id;
            }

            public string getClientName()
            {
                return this.str_name;
            }

            public string getClientAddress()
            {
                return this.str_address;
            }

            public string getClientPhone()
            {
                return this.str_phone;
            }
        }

        public void setClient(int id, string name, string address, string phone)
        {
            client = new Client(id, name, address, phone);
        }

        private void UpdateClient_Load(object sender, EventArgs e)
        {
            this.Owner.Hide();
            this.CenterToScreen();

            /*string dbDir = AppDomain.CurrentDomain.BaseDirectory + "sweet_project.accdb";
            string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbDir.Replace("\\", "\\\\");
            OleDbConnection cnn = new OleDbConnection(connString);

            try
            {
                cnn.Open();
                OleDbDataReader reader = null;
                OleDbCommand command = new OleDbCommand("SELECT ClientID, ClientName, Address, Phone from Clients", cnn);
                reader = command.ExecuteReader();
                string id = "", name = "", address = "", phone = "";
                while (reader.Read())
                {
                    id = reader["ClientID"].ToString();
                    name = reader["ClientName"].ToString();
                    address = reader["Address"].ToString();
                    phone = reader["Phone"].ToString();
                    Client client = new Client(Convert.ToInt32(id), name, address, phone);
                    clients.Add(client);
                }

                textBox1.Text = clients[position].getClientName();
                textBox2.Text = clients[position].getClientAddress();
                textBox3.Text = clients[position].getClientPhone();

                if (clients.Count > 1)
                {
                    button4.Enabled = true;
                }

                cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.ToString());
            }*/

            textBox1.Text = client.getClientName();
            textBox2.Text = client.getClientAddress();
            textBox3.Text = client.getClientPhone();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Owner.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string dbDir = AppDomain.CurrentDomain.BaseDirectory + "sweet_project.accdb";
            string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbDir.Replace("\\", "\\\\");
            OleDbConnection cnn = new OleDbConnection(connString);

            if (addClient.checkClientName(textBox1.Text) &&
                addClient.checkClientAddress(textBox2.Text) &&
                addClient.checkClientPhoneNumber(textBox3.Text))
            {
                try
                {
                    string clientName = textBox1.Text;
                    string clientAddress = textBox2.Text;
                    string clientPhoneNumber = textBox3.Text;

                    cnn.Open();

                    OleDbCommand command = new OleDbCommand("UPDATE Clients SET ClientName = @name, Address = @address, Phone = @phone WHERE [ClientID] = @id", cnn);
                    
                    command.Parameters.Add("@name", OleDbType.Char).Value = clientName;
                    command.Parameters.Add("@address", OleDbType.Char).Value = clientAddress;
                    command.Parameters.Add("@phone", OleDbType.Char).Value = clientPhoneNumber;
                    command.Parameters.Add("@id", OleDbType.Integer).Value = clients[position].getClientId();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Операцията завърши успешно.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    cnn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception: " + ex.ToString());
                }
                
            }
        }

        private void loadDataIntoFields()
        {
            textBox1.Text = clients[position].getClientName();
            textBox2.Text = clients[position].getClientAddress();
            textBox3.Text = clients[position].getClientPhone();
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button1);
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button1);
        }

        private void button6_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button6);
        }

        private void button6_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button6);
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button2);
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button2);
        }

        private void button5_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button5);
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button5);
        }
    }
}
