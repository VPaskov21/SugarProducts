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
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

namespace sweet_project
{
    public partial class MostSoldProducts : Form
    {
        public MostSoldProducts()
        {
            InitializeComponent();
        }

        string targetPath = AppDomain.CurrentDomain.BaseDirectory + @"Images\Products\";
        MainForm mainForm = new MainForm();

        class Product
        {
            string str_name;
            string str_image;
            int n_soldproducts;
            double d_saleprice;

            public Product(string name, string image, int sold, double saleprice)
            {
                str_name = name;
                str_image = image;
                n_soldproducts = sold;
                d_saleprice = saleprice;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Owner.Show();
            this.Close();
        }



        private void MostSoldProducts_Load(object sender, EventArgs e)
        {
            this.Owner.Hide();
            this.CenterToScreen();

            pictureBox1.Image = System.Drawing.Image.FromFile(@"Images\pdf.png");

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
                OleDbCommand command = new OleDbCommand("SELECT p.ProductImage, p.ProductName, SUM(QuantitySold) as SoldQuantity, SUM(QuantitySold*p.Price) as SalePrice from " + 
                                                        "(Products p INNER JOIN SalesDetails sd on sd.ProductID = p.ProductID) GROUP BY ProductImage, ProductName ORDER BY SUM(QuantitySold) DESC", cnn);

                reader = command.ExecuteReader();
                string productimage = "", productname = "", str_sold = "", str_price = "";
                int soldquantity = 0;
                double saleprice = 0;
                int rowNumber = 0;
                dataGridView1.RowTemplate.Height = 90;
                while (reader.Read())
                {
                    productimage = reader["ProductImage"].ToString();
                    productname = reader["ProductName"].ToString();
                    str_sold = reader["SoldQuantity"].ToString();
                    str_price = reader["SalePrice"].ToString();

                    if (!str_sold.Equals(""))
                    {
                        soldquantity = Convert.ToInt32(str_sold);
                    }

                    if (!str_price.Equals(""))
                    {
                        saleprice = Convert.ToDouble(str_price);
                    }

                    Product product = new Product(productname, productimage, soldquantity, saleprice);

                    string[] row = new string[] {
                        "",
                        productname,
                        soldquantity.ToString() + " броя",
                        saleprice.ToString() + " лв."
                    };

                    dataGridView1.Rows.Add(row);
                    dataGridView1.Rows[rowNumber].Cells[0].Value = System.Drawing.Image.FromFile(targetPath + productimage);
                    dataGridView1.Rows[rowNumber].DefaultCellStyle.BackColor = Color.FromArgb(32, 65, 97);
                    dataGridView1.Rows[rowNumber].DefaultCellStyle.ForeColor = Color.White;
                    rowNumber++;

                }

                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.ToString());
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            const string FONT = "c:/windows/fonts/arial.ttf";
            BaseFont bfTimes = BaseFont.CreateFont(FONT, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 12);

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
                System.Drawing.Image imageByte = (System.Drawing.Image)row.Cells[0].Value;
                iTextSharp.text.Image myImage = iTextSharp.text.Image.GetInstance(imageByte, BaseColor.WHITE);
                pdfTable.AddCell(myImage);

                string productName = row.Cells[1].Value.ToString();
                pdfTable.AddCell(new Phrase(productName, times));

                string soldProducts = row.Cells[2].Value.ToString();
                pdfTable.AddCell(new Phrase(soldProducts, times));

                string salePrice = row.Cells[3].Value.ToString();
                pdfTable.AddCell(new Phrase(salePrice, times));
            }

            //Exporting to PDF.
            string folderPath = AppDomain.CurrentDomain.BaseDirectory + @"Reports\";
            using (FileStream stream = new FileStream(folderPath + "MostSoldProductsReport.pdf", FileMode.Create))
            {
                Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();

                iTextSharp.text.Font header = new iTextSharp.text.Font(bfTimes, 25);
                Paragraph headerText = new Paragraph("Най-продавани продукти", header);
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

        private void button6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
