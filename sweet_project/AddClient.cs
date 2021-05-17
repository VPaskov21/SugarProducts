using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.OleDb;

namespace sweet_project
{
    public partial class AddClient : Form
    {
        public AddClient()
        {
            InitializeComponent();
        }

        MainForm mainForm = new MainForm();

        
        private void AddClient_Load(object sender, EventArgs e)
        {
            this.Owner.Hide();
            this.CenterToScreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Owner.Show();
            this.Close();
        }

        public bool checkClientName(string name)
        {
            Regex regex = new Regex(@"^[a-zA-Zа-яА-Я ]+$");
            if (regex.IsMatch(name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (checkClientName(textBox1.Text))
            {
                pictureBox1.Image = Image.FromFile(@"Images\check.png");
            }
            else
            {
                pictureBox1.Image = Image.FromFile(@"Images\error.png");
            }
        }

        public bool checkClientAddress(string address)
        {
            Regex regex = new Regex(@"^[a-zA-Zа-яА-Я0-9 ,?.!""№-]+$");
            if (regex.IsMatch(address))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (checkClientAddress(textBox2.Text))
            {
                pictureBox2.Image = Image.FromFile(@"Images\check.png");
            }
            else
            {
                pictureBox2.Image = Image.FromFile(@"Images\error.png");
            }
        }

        public bool checkClientPhoneNumber(string phoneNumber)
        {
            //Regex regex = new Regex(@"^[+]{1}(359)[ ]?(87|88|89|98)[0-9]{7}$");
            Regex regex = new Regex(@"^(087|088|089|098)[0-9]{7}$");
            if (regex.IsMatch(phoneNumber))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (checkClientPhoneNumber(textBox3.Text))
            {
                pictureBox3.Image = Image.FromFile(@"Images\check.png");
            }
            else
            {
                pictureBox3.Image = Image.FromFile(@"Images\error.png");
            }
        }

        private void button6_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button6);
        }

        private void button6_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button6);
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button1);
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button1);
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button2);
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button2);
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button4);
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button4);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string dbDir = AppDomain.CurrentDomain.BaseDirectory + "sweet_project.accdb";
            string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbDir.Replace("\\", "\\\\");
            OleDbConnection cnn = new OleDbConnection(connString);
            if(checkClientName(textBox1.Text) &&
                checkClientAddress(textBox2.Text) &&
                checkClientPhoneNumber(textBox3.Text))
            {
                string clientName = textBox1.Text;
                string clientAddress = textBox2.Text;
                string clientPhoneNumber = textBox3.Text;

                try
                {
                    cnn.Open();

                    OleDbCommand command = new OleDbCommand("INSERT INTO Clients (ClientName, Address, Phone)" +
                        " VALUES (@name, @address, @phone)", cnn);

                    command.Parameters.Add("@name", OleDbType.Char).Value = clientName;
                    command.Parameters.Add("@address", OleDbType.Char).Value = clientAddress;
                    command.Parameters.Add("@phone", OleDbType.Char).Value = clientPhoneNumber;

                    command.ExecuteNonQuery();
                    MessageBox.Show("Операцията завърши успешно.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult res = MessageBox.Show("Желаете ли да продължите въвеждането?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                    }
                    else
                    {
                        cnn.Close();
                        this.Owner.Show();
                        this.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception: " + ex.ToString());
                }
            }
        }
    }
}
