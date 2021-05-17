using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Data.OleDb;

namespace sweet_project
{
    public partial class SalesByCategory : Form
    {
        public SalesByCategory()
        {
            InitializeComponent();
        }

        List<Category> categories = new List<Category>();
        MainForm mainForm = new MainForm();

        class Category
        {
            int n_categoryId;
            string str_categoryName;
            List<Product> soldProducts = new List<Product>();

            public Category(int id, string name)
            {
                this.n_categoryId = id;
                this.str_categoryName = name;
            }

            public void setSoldProducts(List<Product> products)
            {
                this.soldProducts = products;
            }

            public string getCategoryName()
            {
                return this.str_categoryName;
            }

            public void addToSoldProducts(Product product)
            {
                soldProducts.Add(product);
            }

            public List<Product> getSoldProducts()
            {
                return this.soldProducts;
            }
        }

        class Product
        {
            string str_productName;
            int n_soldProducts;
            double d_salePrice;

            public Product(string name, int soldproducts, double saleprice)
            {
                str_productName = name;
                n_soldProducts = soldproducts;
                d_salePrice = saleprice;
            }

            public string getProductName()
            {
                return str_productName;
            }

            public int getSoldProducts()
            {
                return n_soldProducts;
            }

            public double getSalePrice()
            {
                return d_salePrice;
            }
        }

        private void SalesByCategory_Load(object sender, EventArgs e)
        {
            this.Owner.Hide();
            this.CenterToScreen();
            this.dataGridView1.Visible = false;

            pictureBox1.Image = System.Drawing.Image.FromFile(@"Images\pdf.png");
            pictureBox1.Visible = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(32, 65, 97);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Lucida Bright", 13F);
            dataGridView1.EnableHeadersVisualStyles = false;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(32, 65, 97);
                row.DefaultCellStyle.ForeColor = Color.White;
            }

            string dbDir = AppDomain.CurrentDomain.BaseDirectory + "sweet_project.accdb";
            string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbDir.Replace("\\", "\\\\");

            OleDbConnection cnn = new OleDbConnection(connString);

            try
            {
                cnn.Open();

                OleDbDataReader reader = null;
                OleDbCommand command = new OleDbCommand("SELECT ID, Type from ProductType", cnn);

                reader = command.ExecuteReader();
                string id = "", name = "";
                while (reader.Read())
                {
                    id = reader["ID"].ToString();
                    name = reader["Type"].ToString();
                    Category category = new Category(Convert.ToInt32(id), name);
                    categories.Add(category);
                    comboBox1.Items.Add(name);

                }

                command = new OleDbCommand("SELECT p.ProductName, pt.Type, SUM(QuantitySold) as Quantity, SUM(QuantitySold*p.Price) as SalePrice from ((SalesDetails sd INNER JOIN Products p on sd.ProductID = p.ProductID)" +
                                            "LEFT JOIN ProductType pt on p.TypeID = pt.ID) GROUP BY p.ProductName, pt.Type", cnn);
                reader = command.ExecuteReader();
                string productName = "", quantity = "", price = "", type = "";
                while (reader.Read())
                {
                    productName = reader["ProductName"].ToString();
                    type = reader["Type"].ToString();
                    quantity = reader["Quantity"].ToString();
                    price = reader["SalePrice"].ToString();
                    //MessageBox.Show(productName + " " + type + " " + quantity + " " + price);
                    Product product = new Product(productName, Convert.ToInt32(quantity), Convert.ToDouble(price));
                    foreach (Category category in categories)
                    {
                        if (category.getCategoryName().Equals(type))
                        {
                            category.addToSoldProducts(product);
                        }
                    }
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

        private void button6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void loadDataIntoDataGridView(string categoryName)
        {
            foreach (Category category in categories)
            {
                if (category.getCategoryName().Equals(categoryName))
                {
                    foreach (Product product in category.getSoldProducts())
                    {
                        string[] row = new string[]{
                                    product.getProductName(),
                                    product.getSoldProducts().ToString() + " броя",
                                    product.getSalePrice().ToString() + " лв."
                                };
                        dataGridView1.Rows.Add(row);
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Height = 560;
            this.CenterToScreen();
            pictureBox1.Visible = true;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            switch (comboBox1.SelectedItem.ToString())
            {
                case "вафли":
                    loadDataIntoDataGridView("вафли");
                    break;
                case "бисквити":
                    loadDataIntoDataGridView("бисквити");
                    break;
                case "шоколад":
                    loadDataIntoDataGridView("шоколад");
                    break;
                case "бонбони":
                    loadDataIntoDataGridView("бонбони");
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            const string FONT = "c:/windows/fonts/arial.ttf";
            BaseFont bfTimes = BaseFont.CreateFont(FONT, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 12);

            string category = comboBox1.SelectedItem.ToString();
            //Creating iTextSharp Table from the DataTable data
            PdfPTable pdfTable = new PdfPTable(dataGridView1.ColumnCount);
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.WidthPercentage = 75;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfTable.DefaultCell.BorderWidth = 1;
            pdfTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

            //Adding Header row
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, times));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.FixedHeight = 25;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                pdfTable.AddCell(cell);
            }

            //Adding DataRow
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string productName = row.Cells[0].Value.ToString();
                pdfTable.AddCell(new Phrase(productName, times));

                string soldQuantity = row.Cells[1].Value.ToString();
                pdfTable.AddCell(new Phrase(soldQuantity, times));

                string salePrice = row.Cells[2].Value.ToString();
                pdfTable.AddCell(new Phrase(salePrice, times));
            }

            //Exporting to PDF.
            string folderPath = AppDomain.CurrentDomain.BaseDirectory + @"Reports\";
            using (FileStream stream = new FileStream(folderPath + "SalesByCategoryReport.pdf", FileMode.Create))
            {
                Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();

                iTextSharp.text.Font header = new iTextSharp.text.Font(bfTimes, 25);
                Paragraph headerText = new Paragraph("Продажби за категория " + category, header);
                headerText.Alignment = Element.ALIGN_CENTER;
                headerText.SpacingAfter = 25;

                pdfDoc.Add(headerText);

                Paragraph paragraph = new Paragraph(" ");
                pdfDoc.Add(paragraph);
                pdfTable.HorizontalAlignment = 1;
                pdfDoc.Add(pdfTable);
                pdfDoc.Close();
                stream.Close();
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
