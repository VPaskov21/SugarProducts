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
    public partial class ProducedSoldAvailableProducts : Form
    {
        public ProducedSoldAvailableProducts()
        {
            InitializeComponent();
        }

        string targetPath = AppDomain.CurrentDomain.BaseDirectory + @"Images\Products\";
        List<Product> products = new List<Product>();
        MainForm mainForm = new MainForm();

        class Product
        {
            int n_id;
            string str_productName;
            string str_image;
            int n_quantitySold;
            int n_quantityProduced;
            int n_availableQuantity;

            public Product(int id, string name, string image)
            {
                n_id = id;
                str_productName = name;
                str_image = image;
            }

            public int getId()
            {
                return n_id;
            }

            public void setAvailableQuantity(int quantity)
            {
                n_availableQuantity = quantity;
            }

            public void setProducedQuantity(int quantity)
            {
                n_quantityProduced = quantity;
            }

            public void setSoldQuantity(int quantity)
            {
                n_quantitySold = quantity;
            }

            public int getAvailableQuantity()
            {
                return n_availableQuantity;
            }

            public int getProducedQuantity()
            {
                return n_quantityProduced;
            }

            public int getSoldQuantity()
            {
                return n_quantitySold;
            }

            public string getProductName()
            {
                return str_productName;
            }

            public string getProductImage()
            {
                return str_image;
            }
        }

        private void ProducedSoldAvailableProducts_Load(object sender, EventArgs e)
        {
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

            this.Owner.Hide();
            this.CenterToScreen();

            string dbDir = AppDomain.CurrentDomain.BaseDirectory + "sweet_project.accdb";
            string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbDir.Replace("\\", "\\\\");

            OleDbConnection cnn = new OleDbConnection(connString);

            try
            {
                cnn.Open();
                OleDbDataReader reader = null;
                OleDbCommand command = new OleDbCommand("SELECT ProductID, ProductName, ProductImage, AvailableQuantity from Products", cnn);
                reader = command.ExecuteReader();
                string id = "", name= "", image = "", availablequantity = "";
                int n_availablequantity = 0;
                while (reader.Read())
                {
                    id = reader["ProductID"].ToString();
                    name = reader["ProductName"].ToString();
                    image = reader["ProductImage"].ToString();
                    availablequantity = reader["AvailableQuantity"].ToString();

                    if (!availablequantity.Equals(""))
                    {
                        n_availablequantity = Convert.ToInt32(availablequantity);
                    }

                    Product product = new Product(Convert.ToInt32(id), name, image);
                    product.setAvailableQuantity(n_availablequantity);
                    products.Add(product);
                }
                int rowNumber = 0;
                foreach (Product product in products)
                {
                    reader = null;
                    command = new OleDbCommand("SELECT SUM(sd.QuantitySold) as SoldQuantity From SalesDetails sd WHERE sd.ProductID = @id", cnn);
                    command.Parameters.Add("@id", OleDbType.Integer).Value = product.getId();
                    reader = command.ExecuteReader();
                    string producedquantity = "", soldquantity = "";
                    int n_producedquantity = 0, n_soldquantity = 0;

                    while (reader.Read())
                    {
                        soldquantity = "";
                        soldquantity = reader["SoldQuantity"].ToString();

                        if (!soldquantity.Equals(""))
                        {
                            n_soldquantity = Convert.ToInt32(soldquantity);
                        }

                        product.setSoldQuantity(n_soldquantity);
                    }


                    reader = null;
                    command = new OleDbCommand("SELECT SUM(pp.Quantity) as ProducedQuantity From ProducedProducts pp WHERE pp.ProductID = @id", cnn);
                    command.Parameters.Add("@id", OleDbType.Integer).Value = product.getId();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        producedquantity = "";
                        producedquantity = reader["ProducedQuantity"].ToString();

                        if (!producedquantity.Equals(""))
                        {
                            n_producedquantity = Convert.ToInt32(producedquantity);
                        }

                        product.setProducedQuantity(n_producedquantity);
                    }


                    string[] row = new string[] {
                        "",
                        product.getProductName(),
                        product.getProducedQuantity().ToString() + " броя",
                        product.getSoldQuantity().ToString() + " броя",
                        product.getAvailableQuantity().ToString() + " броя"
                        };


                    dataGridView1.Rows.Add(row);
                    dataGridView1.Rows[rowNumber].Cells[0].Value = System.Drawing.Image.FromFile(targetPath + product.getProductImage());
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Owner.Show();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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

                string producedQuantity = row.Cells[2].Value.ToString();
                pdfTable.AddCell(new Phrase(producedQuantity, times));

                string soldQuantity = row.Cells[3].Value.ToString();
                pdfTable.AddCell(new Phrase(soldQuantity, times));

                string availableQuantity = row.Cells[4].Value.ToString();
                pdfTable.AddCell(new Phrase(availableQuantity, times));
            }

            //Exporting to PDF.
            string folderPath = AppDomain.CurrentDomain.BaseDirectory + @"Reports\";
            using (FileStream stream = new FileStream(folderPath + "ProducedSoldAvailableProductsReport.pdf", FileMode.Create))
            {
                Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();

                iTextSharp.text.Font header = new iTextSharp.text.Font(bfTimes, 25);
                Paragraph headerText = new Paragraph("Произведени, продадени и налични продукти", header);
                headerText.Alignment = Element.ALIGN_CENTER;
                headerText.SpacingAfter = 25;

                pdfDoc.Add(headerText);

                Paragraph paragraph = new Paragraph(" ");
                pdfDoc.Add(paragraph);
                pdfTable.HorizontalAlignment = 1;
                pdfDoc.Add(pdfTable);

                iTextSharp.text.Font footer = new iTextSharp.text.Font(bfTimes, 15);
                Paragraph footerText = new Paragraph("Информационна система за захарни изделия" + "\nТози отчет е генериран на " + DateTime.Now, footer);
                footerText.Alignment = Element.ALIGN_CENTER;
                pdfDoc.Add(footerText);
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
