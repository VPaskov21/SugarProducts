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
    public partial class SearchProduction : Form
    {
        public SearchProduction()
        {
            InitializeComponent();
        }

        private void SearchProduction_Load(object sender, EventArgs e)
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
                OleDbCommand command = new OleDbCommand("SELECT p.ProductName, Quantity, ProductionDate, ExpirationDate from (ProducedProducts pp " + 
                                                        "INNER JOIN Products p on pp.ProductID = p.ProductID)", cnn);

                reader = command.ExecuteReader();
                string name = "", quantity = "", proddate = "", expdate = "";
                while (reader.Read())
                {
                    name = reader["ProductName"].ToString();
                    quantity = reader["Quantity"].ToString();
                    proddate = Convert.ToDateTime(reader["ProductionDate"]).ToShortDateString();
                    expdate = Convert.ToDateTime(reader["ExpirationDate"]).ToShortDateString();

                    string[] row = new string[]{
                            "",
                            name,
                            quantity,
                            proddate,
                            expdate
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Owner.Show();
            this.Close();
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

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            for (int u = 0; u < dataGridView1.RowCount; u++)
            {
                string value = dataGridView1.Rows[u].Cells[4].Value.ToString();
                if (value.ToLower().Contains(textBox4.Text.ToLower()))
                {
                    dataGridView1.Rows[u].Visible = true;
                }
                else
                {
                    dataGridView1.Rows[u].Visible = false;
                }
            }
        }
    }
}
