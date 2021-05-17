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
    public partial class SearchClient : Form
    {
        public SearchClient()
        {
            InitializeComponent();
        }

        MainForm mainForm = new MainForm();

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

        private void SearchClient_Load(object sender, EventArgs e)
        {
            this.Owner.Hide();
            this.CenterToScreen();

            string dbDir = AppDomain.CurrentDomain.BaseDirectory + "sweet_project.accdb";
            string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbDir.Replace("\\", "\\\\");
            OleDbConnection cnn = new OleDbConnection(connString);
            dataGridView1.Visible = true;


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

                    string[] row = new string[]{
                            id,
                            name,
                            address,
                            phone,
                            "Промени"
                        };

                    dataGridView1.Rows.Add(row);
                }

                cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.ToString());
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Owner.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
             
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                UpdateClient updateClient = new UpdateClient();
                updateClient.setClient(Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value),
                                            dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(),
                                            dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString(),
                                            dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
                updateClient.Owner = this;
                updateClient.Show();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            for (int u = 0; u < dataGridView1.RowCount; u++)
            {
                string value = dataGridView1.Rows[u].Cells[1].Value.ToString();
                if (value.ToLower().Contains(textBox1.Text.ToLower()))
                {
                    dataGridView1.Rows[u].Visible = true;
                }
                else
                {
                    dataGridView1.Rows[u].Visible = false;
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            for (int u = 0; u < dataGridView1.RowCount; u++)
            {
                string value = dataGridView1.Rows[u].Cells[2].Value.ToString();
                if (value.ToLower().Contains(textBox2.Text.ToLower()))
                {
                    dataGridView1.Rows[u].Visible = true;
                }
                else
                {
                    dataGridView1.Rows[u].Visible = false;
                }
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            for (int u = 0; u < dataGridView1.RowCount; u++)
            {
                string value = dataGridView1.Rows[u].Cells[3].Value.ToString();
                if (value.ToLower().Contains(textBox3.Text.ToLower()))
                {
                    dataGridView1.Rows[u].Visible = true;
                }
                else
                {
                    dataGridView1.Rows[u].Visible = false;
                }
            }
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
    }
}
