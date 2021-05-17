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
using System.IO;
using System.Data.OleDb;

namespace sweet_project
{
    public partial class AddProduct : Form
    {
        public AddProduct()
        {
            InitializeComponent();
        }

        string sourceFile;
        string targetPath = AppDomain.CurrentDomain.BaseDirectory + @"Images\Products";

        MainForm mainForm = new MainForm();

        private void button2_Click(object sender, EventArgs e)
        {
            string dbDir = AppDomain.CurrentDomain.BaseDirectory + "sweet_project.accdb";
            string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbDir.Replace("\\", "\\\\");
            OleDbConnection cnn = new OleDbConnection(connString);

            if(checkProductName(textBox1.Text) &&
                checkProductDescription(textBox2.Text) &&
                textBox3.Text != "" &&
                comboBox1.SelectedIndex != -1 &&
                checkProductPrice(textBox4.Text))
            {
                try
                {
                    string productName = textBox1.Text;
                    string productDescription = textBox2.Text;
                    string productImage = textBox3.Text;
                    int productType = comboBox1.SelectedIndex + 1;
                    

                    cnn.Open();

                    double price;
                    if (Double.TryParse(textBox4.Text, out price))
                    {
                        string destFile = Path.Combine(targetPath, textBox3.Text);
                        File.Copy(sourceFile, destFile, true);
                        OleDbCommand command = new OleDbCommand("INSERT INTO Products (ProductName, ProductDescription, ProductImage, Price, TypeID)" +
                            " VALUES (@name, @description, @image, @price, @type)", cnn);

                        command.Parameters.Add("@name", OleDbType.Char).Value = productName;
                        command.Parameters.Add("@description", OleDbType.Char).Value = productDescription;
                        command.Parameters.Add("@image", OleDbType.Char).Value = productImage;
                        command.Parameters.Add("@price", OleDbType.Double).Value = price;
                        command.Parameters.Add("@type", OleDbType.Integer).Value = productType;
                        command.ExecuteNonQuery();

                        MessageBox.Show("Операцията завърши успешно.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DialogResult res = MessageBox.Show("Желаете ли да продължите въвеждането?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (res == DialogResult.Yes)
                        {
                            textBox1.Clear();
                            textBox2.Clear();
                            textBox3.Clear();
                            textBox4.Clear();
                        }
                        else
                        {
                            cnn.Close();
                            this.Owner.Show();
                            this.Close();
                        }
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

        private void AddProduct_Load(object sender, EventArgs e)
        {
            this.Owner.Hide();
            this.CenterToScreen();
            //MessageBox.Show(targetPath);
        }

        public void setFileDialogProperties(OpenFileDialog openFileDialog)
        {
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Title = "Browse files";
            openFileDialog1.Filter = "Image Files(.jpg,.jpeg,.png)|*.jpg;*.jpeg;*.png";
            openFileDialog1.FileName = "";
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Owner.Show();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label5_Click(object sender, EventArgs e)
        {

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

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button3);
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button3);
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

        public bool checkProductName(string name)
        {
            Regex regex = new Regex(@"^[a-zA-Zа-яА-Я0-9 ]+$");
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
            if (checkProductName(textBox1.Text))
            {
                pictureBox1.Image = Image.FromFile(@"Images\check.png");
            }
            else
            {
                pictureBox1.Image = Image.FromFile(@"Images\error.png");
            }
        }

        public bool checkProductDescription(string description)
        {
            Regex regex = new Regex(@"^[a-zA-Zа-яА-Я0-9 ,?.!]+$");
            if (regex.IsMatch(description))
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
            if (checkProductDescription(textBox2.Text))
            {
                pictureBox2.Image = Image.FromFile(@"Images\check.png");
            }
            else
            {
                pictureBox2.Image = Image.FromFile(@"Images\error.png");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            setFileDialogProperties(openFileDialog1);
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

        public bool checkProductPrice(string price)
        {
            Regex regex = new Regex(@"^[0-9,]+$");
            if (regex.IsMatch(price))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (checkProductPrice(textBox4.Text))
            {
                pictureBox4.Image = Image.FromFile(@"Images\check.png");
            }
            else
            {
                pictureBox4.Image = Image.FromFile(@"Images\error.png");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Наистина ли желаете да напуснете въвеждането?\nПромените няма да бъдат запазени!", "Изход", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                this.Owner.Show();
                this.Close();
            }
        }
    }
}
