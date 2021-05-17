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
using System.IO;

namespace sweet_project
{
    public partial class UpdateProduct : Form
    {
        public UpdateProduct()
        {
            InitializeComponent();
        }

        MainForm mainForm = new MainForm();
        AddProduct addProduct = new AddProduct();
        List<Product> products = new List<Product>();
        string sourceFile;
        string targetPath = AppDomain.CurrentDomain.BaseDirectory + @"Images\Products";
        Product product;

        static int position = 0;

        class Product
        {
            int n_id;
            string str_name;
            string str_description;
            string str_image;
            double d_price;
            string str_type;

            public Product(int id, string name, string description, string image, double price, string type)
            {
                n_id = id;
                str_name = name;
                str_description = description;
                str_image = image;
                d_price = price;
                str_type = type;
            }

            public int getId()
            {
                return n_id;
            }

            public string getName()
            {
                return str_name;
            }

            public string getDescription()
            {
                return str_description;
            }

            public string getImage()
            {
                return str_image;
            }

            public double getPrice()
            {
                return d_price;
            }

            public string getType()
            {
                return str_type;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Owner.Show();
            this.Close();
        }

        public void setProduct(int id, string name, string description, string image, double price, string type)
        {

            product = new Product(id, name, description, image, price, type);
        }

        private void UpdateProduct_Load(object sender, EventArgs e)
        {
            this.Owner.Hide();
            this.CenterToScreen();

            textBox1.Text = product.getName();
            textBox2.Text = product.getDescription();
            textBox3.Text = product.getImage();
            textBox4.Text = product.getPrice().ToString();
            comboBox1.SelectedItem = product.getType();
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

        private void button7_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button7);
        }

        private void button7_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button7);
        }

        private void button5_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button5);
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button5);
        }

        private void loadDataIntoFields()
        {
            
        }
        private void button7_Click(object sender, EventArgs e)
        {
            string dbDir = AppDomain.CurrentDomain.BaseDirectory + "sweet_project.accdb";
            string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbDir.Replace("\\", "\\\\");
            OleDbConnection cnn = new OleDbConnection(connString);

            if(addProduct.checkProductName(textBox1.Text) &&
               addProduct.checkProductDescription(textBox2.Text) &&
                textBox3.Text != "" &&
                addProduct.checkProductPrice(textBox4.Text))
            {
                try
                {
                    string productName = textBox1.Text;
                    string productDescription = textBox2.Text;
                    string productImage = textBox3.Text;
                    double price;

                    if (Double.TryParse(textBox4.Text, out price))
                    {
                        string destFile = Path.Combine(targetPath, textBox3.Text);
                        File.Copy(sourceFile, destFile, true);

                        cnn.Open();

                        OleDbCommand command = new OleDbCommand("UPDATE Products SET ProductName = @name, ProductDescription = @description, ProductImage = @image, Price = @price, TypeID = @type WHERE [ProductID] = @id", cnn);

                        command.Parameters.Add("@name", OleDbType.Char).Value = productName;
                        command.Parameters.Add("@description", OleDbType.Char).Value = productDescription;
                        command.Parameters.Add("@image", OleDbType.Char).Value = productImage;
                        command.Parameters.Add("@id", OleDbType.Integer).Value = comboBox1.SelectedIndex + 1;
                        command.Parameters.Add("@price", OleDbType.Double).Value = price;

                        command.ExecuteNonQuery();

                        MessageBox.Show("Операцията завърши успешно.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        cnn.Close();
                    }
                    else
                    {
                        MessageBox.Show("Възникна грешка при изпълнението на заявката.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            addProduct.setFileDialogProperties(openFileDialog1);
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = openFileDialog1.SafeFileName;
                sourceFile = openFileDialog1.FileName;
                pictureBox3.Image = Image.FromFile(@"Images\check.png");
            }
            else
            {
                pictureBox3.Image = Image.FromFile(@"Images\error.png");
            }
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button3);
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button3);
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
