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
using System.Text.RegularExpressions;

namespace sweet_project
{
    public partial class AddProduction : Form
    {
        public AddProduction()
        {
            InitializeComponent();
        }

        MainForm mainForm = new MainForm();
        List<Product> products = new List<Product>();

        class Product
        {
            int n_id;
            string str_name;
            double d_price;

            public Product(int id, string name, double price)
            {
                n_id = id;
                str_name = name;
                d_price = price;
            }

            public int getProductId()
            {
                return n_id;
            }

            public string getName()
            {
                return str_name;
            }

            public double getPrice()
            {
                return d_price;
            }
        }

        private void AddProduction_Load(object sender, EventArgs e)
        {
            this.Owner.Hide();
            this.CenterToScreen();

            if (checkDateTime(dateTimePicker1))
            {
                pictureBox4.Image = Image.FromFile(@"Images\check.png");
            }
            else
            {
                pictureBox4.Image = Image.FromFile(@"Images\error.png");
            }

            string dbDir = AppDomain.CurrentDomain.BaseDirectory + "sweet_project.accdb";
            string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbDir.Replace("\\", "\\\\");
            OleDbConnection cnn = new OleDbConnection(connString);
            try
            {
                cnn.Open();
                OleDbDataReader reader = null;
                OleDbCommand command = new OleDbCommand("SELECT ProductID, ProductName, Price from Products", cnn);
                reader = command.ExecuteReader();
                string id = "", product = "", price = "";
                while (reader.Read())
                {
                    id = reader["ProductID"].ToString();
                    product = reader["ProductName"].ToString();
                    price = reader["Price"].ToString();
                    Product productObj = new Product(Convert.ToInt32(id), product.ToString(), Convert.ToDouble(price));
                    products.Add(productObj);
                    comboBox1.Items.Add(product);
                }

                cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.ToString());
            }
        }

        public bool checkDateTime(DateTimePicker dateTimePicker)
        {
            DateTime dtValue = DateTime.Now;
            bool bres = DateTime.TryParse(dateTimePicker.Value.ToString(), out dtValue);

            if (bres == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Owner.Show();
            this.Close();
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

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button4);
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button4);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                string product = comboBox1.SelectedItem.ToString();
                foreach (Product productObj in products)
                {
                    if (productObj.getName().Equals(product))
                    {
                        label5.Text = productObj.getPrice().ToString() + " лв.";
                    }
                }
                pictureBox1.Image = Image.FromFile(@"Images\check.png");
            }
            else
            {
                pictureBox1.Image = Image.FromFile(@"Images\error.png");
            }
            
        }

        public bool checkProductionQuantity(string quantity)
        {
            Regex regex = new Regex(@"^[0-9]+$");
            if (regex.IsMatch(quantity))
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
            if (checkProductionQuantity(textBox1.Text))
            {
                pictureBox2.Image = Image.FromFile(@"Images\check.png");
            }
            else
            {
                pictureBox2.Image = Image.FromFile(@"Images\error.png");
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private bool checkExpiration(string time)
        {
            Regex regex = new Regex(@"^[0-9]+$");
            if (regex.IsMatch(time))
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
            if (checkExpiration(textBox2.Text))
            {
                pictureBox5.Image = Image.FromFile(@"Images\check.png");
            }
            else
            {
                pictureBox5.Image = Image.FromFile(@"Images\error.png");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string dbDir = AppDomain.CurrentDomain.BaseDirectory + "sweet_project.accdb";
            string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbDir.Replace("\\", "\\\\");

            OleDbConnection cnn = new OleDbConnection(connString);

            if(comboBox1.SelectedIndex != -1 &&
                checkProductionQuantity(textBox1.Text) &&
                checkDateTime(dateTimePicker1) &&
                checkExpiration(textBox2.Text))
            {
                try
                {
                    cnn.Open();

                    int id = products[comboBox1.SelectedIndex].getProductId();
                    string quantity = textBox1.Text;
                    string productionDate = dateTimePicker1.Value.ToShortDateString();
                    DateTime dateSelected = dateTimePicker1.Value.Date;
                    string expirationDate = dateSelected.AddMonths(Convert.ToInt32(textBox2.Text)).ToShortDateString();

                    OleDbCommand command = new OleDbCommand("INSERT INTO ProducedProducts (ProductID, Quantity, ProductionDate, ExpirationDate)" +
                        " VALUES (@productid, @quantity, @productiondate, @expirationdate)", cnn);

                    command.Parameters.Add("@productid", OleDbType.Integer).Value = id;
                    command.Parameters.Add("@quantity", OleDbType.Integer).Value = Convert.ToInt32(quantity);
                    command.Parameters.Add("@productiondate", OleDbType.Date).Value = productionDate;
                    command.Parameters.Add("@expirationdate", OleDbType.Date).Value = expirationDate;
                    command.ExecuteNonQuery();

                    OleDbDataReader reader = null;
                    command = new OleDbCommand("SELECT AvailableQuantity FROM Products WHERE [ProductID] = @productid", cnn);
                    command.Parameters.Add("@productid", OleDbType.Integer).Value = id;
                    reader = command.ExecuteReader();
                    int availableQuantity = 0;
                    while (reader.Read())
                    {
                        availableQuantity = Convert.ToInt32(reader["AvailableQuantity"].ToString());
                    }

                    availableQuantity += Convert.ToInt32(quantity);
                    OleDbCommand cmd = new OleDbCommand("UPDATE Products SET AvailableQuantity = @quantity WHERE [ProductID] = @productid", cnn);
                    cmd.Parameters.Add("@quantity", OleDbType.Integer).Value = availableQuantity;
                    cmd.Parameters.Add("@productid", OleDbType.Integer).Value = id;
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Операцията завърши успешно.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult res = MessageBox.Show("Желаете ли да продължите въвеждането?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        comboBox1.SelectedIndex = -1;
                        textBox1.Clear();
                        textBox2.Clear();
                        label5.Text = "";                       
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
