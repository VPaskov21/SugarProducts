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
    public partial class SearchProduct : Form
    {
        public SearchProduct()
        {
            InitializeComponent();
        }

        MainForm mainForm = new MainForm();
        string targetPath = AppDomain.CurrentDomain.BaseDirectory + @"Images\Products\";

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                string price = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString().Replace(" лв.", "");                

                UpdateProduct updateProduct = new UpdateProduct();
                updateProduct.setProduct(Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value),
                                            dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString(),
                                            dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString(),
                                            dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString(),
                                            Convert.ToDouble(price.Trim()),
                                            dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString());
                updateProduct.Owner = this;
                updateProduct.Show();
            } 
        }

        private void SearchProduct_Load(object sender, EventArgs e)
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
                OleDbCommand command = new OleDbCommand("SELECT p.ProductID, p.ProductName, p.ProductDescription, p.ProductImage, p.Price, pt.Type FROM " +
                                                        "(Products p INNER JOIN ProductType pt on p.TypeID = pt.ID)", cnn);
                reader = command.ExecuteReader();
                string id = "", name = "", description = "", image = "", price = "", type = "";
                int rowNumber = 0;
                while (reader.Read())
                {
                    id = reader["ProductID"].ToString();
                    name = reader["ProductName"].ToString();
                    description = reader["ProductDescription"].ToString();
                    image = reader["ProductImage"].ToString();
                    price = reader["Price"].ToString();
                    type = reader["Type"].ToString();

                    string[] row = new string[]{
                        id,
                        "",
                        name,
                        description,
                        price + " лв.",
                        type,
                        "Промени",
                        image
                    };

                    dataGridView1.Rows.Add(row);
                    dataGridView1.Rows[rowNumber].Cells[1].Value = System.Drawing.Image.FromFile(targetPath + image);
                    dataGridView1.Rows[rowNumber].DefaultCellStyle.BackColor = Color.FromArgb(32, 65, 97);
                    dataGridView1.Rows[rowNumber].DefaultCellStyle.ForeColor = Color.White;
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            for (int u = 0; u < dataGridView1.RowCount; u++)
            {
                string value = dataGridView1.Rows[u].Cells[2].Value.ToString();
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
                string value = dataGridView1.Rows[u].Cells[3].Value.ToString();
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
                string value = dataGridView1.Rows[u].Cells[4].Value.ToString();
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
                string value = dataGridView1.Rows[u].Cells[5].Value.ToString();
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
