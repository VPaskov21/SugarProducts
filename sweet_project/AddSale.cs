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
using System.Windows.Forms.VisualStyles;

namespace sweet_project
{
    public partial class AddSale : Form
    {
        public AddSale()
        {
            InitializeComponent();
        }

        MainForm mainForm = new MainForm();
        AddProduction addProduction = new AddProduction();
        SaleOrder saleOrder;

        List<Client> clients = new List<Client>();
        List<Product> products = new List<Product>();
        //List<Product> saleProducts = new List<Product>();
        double overall_price = 0;
        string targetPath = AppDomain.CurrentDomain.BaseDirectory + @"Images\Products\";
        bool isQuantityCorrect = true;

        class SaleOrder
        {
            int n_id;
            List<Product> soldProducts = new List<Product>();
            double d_price;
            Client client;

            public SaleOrder()
            {

            }

            public List<Product> getSoldProducts()
            {
                return soldProducts;
            }

            public void setSaleOrderID(int id)
            {
                n_id = id;
            }

            public int getSaleOrderID()
            {
                return this.n_id;
            }
        }

        class Client
        {
            int n_id;
            string str_name;

            public Client(int id, string name)
            {
                n_id = id;
                str_name = name;
            }

            public int getClientId()
            {
                return n_id;
            }
        }
    
        class Product
        {
            int n_id;
            string str_name;
            string str_image;
            double d_price;
            int n_quantity;

            public Product(int id, string name, string image, double price, int quantity)
            {
                n_id = id;
                str_name = name;
                str_image = image;
                d_price = price;
                n_quantity = quantity;
            }

            public Product(Product product)
            {
                this.n_id = product.n_id;
                this.str_name = product.str_name;
                this.str_image = product.str_image;
                this.d_price = product.d_price;
                this.n_quantity = product.n_quantity;
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

            public void setQuantity(int quantity)
            {
                this.n_quantity = quantity;
            }

            public int getQuantity()
            {
                return n_quantity;
            }
        }

        private void AddSale_Load(object sender, EventArgs e)
        {
            this.Owner.Hide();
            this.CenterToScreen();

            saleOrder = new SaleOrder();

            dateTimePicker1.MinDate = DateTime.Now;
            dateTimePicker1.Value = DateTime.Now;

            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(32, 65, 97);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Lucida Bright", 13F);
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.Rows.Clear();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(32, 65, 97);
                row.DefaultCellStyle.ForeColor = Color.White;
            }

            /*DataGridViewButtonColumn button = new DataGridViewButtonColumn();
            {
                button.Name = "button";
                button.HeaderText = "";
                button.Text = "Добави";
                button.UseColumnTextForButtonValue = true; //dont forget this line
                this.dataGridView1.Columns.Add(button);
            }*/

            if (addProduction.checkDateTime(dateTimePicker1))
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
                OleDbCommand command = new OleDbCommand("SELECT ClientID, ClientName from Clients", cnn);
                reader = command.ExecuteReader();
                string id = "", name = "";
                while (reader.Read())
                {
                    id = reader["ClientID"].ToString();
                    name = reader["ClientName"].ToString();
                    Client client = new Client(Convert.ToInt32(id), name);
                    clients.Add(client);
                    comboBox1.Items.Add(name);
                }

                reader = null;
                command = new OleDbCommand("SELECT MAX(SaleID) as Sale_ID from Sales", cnn);
                reader = command.ExecuteReader();
                int sale_id = 0;
                while (reader.Read())
                {
                    sale_id = Convert.ToInt32(reader["Sale_ID"].ToString()) + 1;
                }
                saleOrder.setSaleOrderID(sale_id);

                label4.Text = sale_id.ToString();

                reader = null;
                command = new OleDbCommand("SELECT ProductID, ProductName, ProductImage, Price, AvailableQuantity from Products", cnn);
                reader = command.ExecuteReader();
                string product = "", price = "", image = "", availablequantity = "";
                id = "";
                int rowNumber = 0;
                dataGridView1.RowTemplate.Height = 90;
                while (reader.Read())
                {
                    id = reader["ProductID"].ToString();
                    product = reader["ProductName"].ToString();
                    image = reader["ProductImage"].ToString();
                    price = reader["Price"].ToString();
                    availablequantity = reader["AvailableQuantity"].ToString();
                    Product productObj = new Product(Convert.ToInt32(id), product, image, Convert.ToDouble(price), Convert.ToInt32(availablequantity));
                    products.Add(productObj);

                    string[] row = new string[] { "",
                        product,
                        availablequantity,
                        price + " лв.",
                        ""
                    };

                    dataGridView1.Rows.Add(row);
                    dataGridView1.Rows[rowNumber].DefaultCellStyle.BackColor = Color.FromArgb(32, 65, 97);
                    dataGridView1.Rows[rowNumber].DefaultCellStyle.ForeColor = Color.White;
                    dataGridView1.Rows[rowNumber].Cells[0].Value = Image.FromFile(targetPath + image);
                    dataGridView1.Rows[rowNumber].Cells[5].Value = "Добави";
                    dataGridView1.Rows[rowNumber].Cells[6].Value = id;
                    rowNumber++;
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Hide the ones that you want with the filter you want.
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

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                pictureBox1.Image = Image.FromFile(@"Images\check.png");
            }
            else
            {
                pictureBox1.Image = Image.FromFile(@"Images\error.png");
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<Product> soldProducts = saleOrder.getSoldProducts();
            double totalSalePrice = 0;
            foreach (Product product in soldProducts)
            {
                //MessageBox.Show(product.getName()+"\n"+product.getPrice()+"\n"+product.getQuantity());
                double productPrice = product.getPrice() * product.getQuantity();
                totalSalePrice += productPrice;
            }
            string dbDir = AppDomain.CurrentDomain.BaseDirectory + "sweet_project.accdb";
            string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbDir.Replace("\\", "\\\\");

            OleDbConnection cnn = new OleDbConnection(connString);
            
            if(comboBox1.SelectedIndex != -1 &&
                addProduction.checkDateTime(dateTimePicker1) &&
                isQuantityCorrect)
            {
                try
                {
                    cnn.Open();

                    int saleID = saleOrder.getSaleOrderID();
                    int clientID = clients[comboBox1.SelectedIndex].getClientId();
                    string saleDate = dateTimePicker1.Value.ToShortDateString();
                    int newAvailableQuantity = 0;

                    OleDbCommand command = new OleDbCommand("INSERT INTO Sales (SaleID, ClientID, SaleDate, TotalSalePrice)" +
                       " VALUES (@saleid, @clientid, @date, @price)", cnn);
                    command.Parameters.Add("@saleid", OleDbType.Integer).Value = saleID;
                    command.Parameters.Add("@clientid", OleDbType.Integer).Value = clientID;
                    command.Parameters.Add("@date", OleDbType.Date).Value = saleDate;
                    command.Parameters.Add("@date", OleDbType.Double).Value = totalSalePrice;
                    command.ExecuteNonQuery();

                    foreach (Product product in soldProducts)
                    {
                        command = new OleDbCommand("INSERT INTO SalesDetails (SaleID, ProductID, QuantitySold)" +
                            " VALUES (@saleid, @productid, @quantity)", cnn);
                        command.Parameters.Add("@saleid", OleDbType.Integer).Value = saleID;
                        command.Parameters.Add("@productid", OleDbType.Integer).Value = product.getProductId();
                        command.Parameters.Add("@quantity", OleDbType.Integer).Value = product.getQuantity();
                        command.ExecuteNonQuery();

                        
                        foreach (Product theProduct in products)
                        {
                            if (theProduct.getProductId() == product.getProductId())
                            {
                                newAvailableQuantity = theProduct.getQuantity() - product.getQuantity();
                            }
                        }
                        command = new OleDbCommand("UPDATE Products SET AvailableQuantity = @quantity WHERE [ProductID] = @productid", cnn);
                        command.Parameters.Add("@quantity", OleDbType.Integer).Value = newAvailableQuantity;
                        command.Parameters.Add("@productid", OleDbType.Integer).Value = product.getProductId();
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Операцията завърши успешно.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult res = MessageBox.Show("Желаете ли да продължите въвеждането?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        comboBox1.SelectedIndex = -1;
                        label6.Text = "";
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

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                string quantity = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                string availableQuantity = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();

                if (quantity == "")
                {
                    MessageBox.Show("Трябва да въведете количество.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (Convert.ToInt32(quantity) > Convert.ToInt32(availableQuantity))
                    {
                        MessageBox.Show("Количеството не може да надвишава наличността.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        isQuantityCorrect = false;
                    }
                    else
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[5] = new DataGridViewTextBoxCell { Value = "Добавено" };
                        dataGridView1.Rows[e.RowIndex].Cells[5].ReadOnly = true;
                        dataGridView1.Rows[e.RowIndex].Cells[5].Selected = false;

                        isQuantityCorrect = true;

                        List<Product> productsInOrder = saleOrder.getSoldProducts();

                        foreach (Product product in products)
                        {
                            if(product.getName().Equals(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()))
                            {
                                Product productToBeAdded = new Product(product);
                                productToBeAdded.setQuantity(Convert.ToInt32(quantity));
                                double productPrice = productToBeAdded.getPrice() * productToBeAdded.getQuantity();
                                overall_price = productPrice;
                                label6.Text = overall_price.ToString() + " лв.";
                                productsInOrder.Add(productToBeAdded);
                            }
                        }
                    }
                }
            }
        }
    }
}
