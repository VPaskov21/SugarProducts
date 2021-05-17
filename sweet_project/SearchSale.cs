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
    public partial class SearchSale : Form
    {
        public SearchSale()
        {
            InitializeComponent();
        }

        List<SaleOrder> saleOrders = new List<SaleOrder>();

        class SaleOrder
        {
            int n_id;
            List<Product> soldProducts = new List<Product>();
            string str_date;
            double d_price;
            string str_client;

            public SaleOrder()
            {

            }

            public List<Product> getSoldProducts()
            {
                return soldProducts;
            }

            public void setSoldProduct(List<Product> products)
            {
                this.soldProducts = products;
            }

            public SaleOrder(int id, string client, string saleDate, double price)
            {
                n_id = id;
                str_date = saleDate;
                str_client = client;
                d_price = price;
            }

            public int getSaleOrderID()
            {
                return this.n_id;
            }

            public string getSaleDate()
            {
                return str_date;
            }

            public double getTotalPrice()
            {
                return d_price;
            }
        }

        class Product
        {
            int n_id;
            string str_name;
            double d_price;
            int n_quantity;

            public Product(string name, double price, int quantity)
            {
                str_name = name;
                d_price = price;
                n_quantity = quantity;
            }

            public Product(Product product)
            {
                this.str_name = product.str_name;
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

        private void SearchSale_Load(object sender, EventArgs e)
        {
            this.Owner.Hide();
            this.CenterToScreen();

            label2.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            dataGridView1.Visible = false;

            string dbDir = AppDomain.CurrentDomain.BaseDirectory + "sweet_project.accdb";
            string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbDir.Replace("\\", "\\\\");
            OleDbConnection cnn = new OleDbConnection(connString);

            try
            {
                cnn.Open();
                OleDbDataReader reader = null;
                OleDbCommand command = new OleDbCommand("SELECT SaleID, c.ClientName, SaleDate, TotalSalePrice from (Sales s " + 
                                                        "INNER JOIN Clients c on s.ClientID = c.ClientID)", cnn);
                reader = command.ExecuteReader();

                string id = "", clientname = "", saledate = "", totalsaleprice = "";
                while (reader.Read())
                {
                    id = reader["SaleID"].ToString();
                    clientname = reader["ClientName"].ToString();
                    saledate = Convert.ToDateTime(reader["SaleDate"]).ToShortDateString();
                    totalsaleprice = reader["TotalSalePrice"].ToString();
                    SaleOrder saleOrder = new SaleOrder(Convert.ToInt32(id), clientname, saledate, Convert.ToDouble(totalsaleprice));
                    saleOrders.Add(saleOrder);
                    comboBox1.Items.Add(id);
                }

                foreach (SaleOrder saleOrder in saleOrders)
                {
                    List<Product> productsInOrder = new List<Product>();
                    reader = null;
                    command = new OleDbCommand("SELECT p.ProductName, p.Price, QuantitySold from (SalesDetails sd " + 
                                                "INNER JOIN Products p on sd.ProductID = p.ProductID) WHERE sd.SaleID = @id", cnn);
                    command.Parameters.Add("@id", OleDbType.Integer).Value = saleOrder.getSaleOrderID();
                    reader = command.ExecuteReader();

                    string productName = "", productPrice = "", quantitySold = "";
                    while (reader.Read())
                    {
                        productName = reader["ProductName"].ToString();
                        productPrice = reader["Price"].ToString();
                        quantitySold = reader["QuantitySold"].ToString();

                        Product product = new Product(productName, Convert.ToDouble(productPrice), Convert.ToInt32(quantitySold));
                        productsInOrder.Add(product);
                    }

                    saleOrder.setSoldProduct(productsInOrder);
                }

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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Height = 600;
            this.CenterToScreen();
            label2.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            foreach (SaleOrder saleOrder in saleOrders)
            {
                if (saleOrder.getSaleOrderID().Equals(Convert.ToInt32(comboBox1.SelectedItem)))
                {
                    label4.Text = saleOrder.getSaleDate();
                    label6.Text = saleOrder.getTotalPrice().ToString() + " лв.";
                    foreach (Product product in saleOrder.getSoldProducts())
                    {
                        double price = product.getQuantity() * product.getPrice();
                        string[] row = new string[] {
                            "",
                            product.getName(),
                            product.getQuantity().ToString() + " броя",
                            product.getPrice() + " лв.",
                            price.ToString() + " лв."
                        };

                        dataGridView1.Rows.Add(row);
                    }
                }
            }
        }
    }
}
