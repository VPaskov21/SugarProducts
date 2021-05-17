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
    public partial class SalesBeforeDate : Form
    {
        public SalesBeforeDate()
        {
            InitializeComponent();
        }

        MainForm mainForm = new MainForm();
        AddProduction addProduction = new AddProduction();
        List<Product> products = new List<Product>();
        string targetPath = AppDomain.CurrentDomain.BaseDirectory + @"Images\Products\";
        List<Sale> sales = new List<Sale>();

        class Sale
        {
            int n_id;
            
            public Sale(int id)
            {
                n_id = id;
            }

            public int getSaleId()
            {
                return this.n_id;
            }
        }

        class Product
        {
            int n_id;
            string str_name;

            public Product(int id, string name)
            {
                n_id = id;
                str_name = name;
            }

            public int getId()
            {
                return n_id;
            }
        }

        private void SalesBeforeDate_Load(object sender, EventArgs e)
        {
            this.Height = 267;
            dataGridView1.Visible = false;
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

            this.Owner.Hide();
            this.CenterToScreen();

            string dbDir = AppDomain.CurrentDomain.BaseDirectory + "sweet_project.accdb";
            string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbDir.Replace("\\", "\\\\");

            OleDbConnection cnn = new OleDbConnection(connString);

            try
            {
                cnn.Open();

                OleDbDataReader reader = null;
                OleDbCommand command = new OleDbCommand("SELECT ProductID, ProductName from Products", cnn);

                reader = command.ExecuteReader();
                string id = "", name = "";
                while (reader.Read())
                {
                    id = reader["ProductID"].ToString();
                    name = reader["ProductName"].ToString();
                    Product product = new Product(Convert.ToInt32(id), name);
                    products.Add(product);
                    comboBox1.Items.Add(name);
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Height = 593;
            this.CenterToScreen();
            dataGridView1.Visible = true;
            pictureBox1.Visible = true;
            dataGridView1.Rows.Clear();
            string dbDir = AppDomain.CurrentDomain.BaseDirectory + "sweet_project.accdb";
            string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbDir.Replace("\\", "\\\\");

            OleDbConnection cnn = new OleDbConnection(connString);

            if(comboBox1.SelectedIndex != -1 &&
                addProduction.checkDateTime(dateTimePicker1))
            {
                try
                {
                    cnn.Open();

                    string saleDate = dateTimePicker1.Value.ToShortDateString();
                    string product = comboBox1.SelectedText;

                    OleDbDataReader reader = null;
                    /*command = new OleDbCommand("SELECT c.ClientName, p.ProductName, p.ProductImage, s.SaleDate, s.SalePrice " + 
                                                    " FROM ((Sales s INNER JOIN Clients c on s.ClientID = c.ClientID) LEFT JOIN Products p on s.ProductID = p.ProductID)" +
                                                    " WHERE s.ProductID = @id AND s.SaleDate < @date", cnn);*/

                    OleDbCommand command = new OleDbCommand("SELECT s.SaleID, c.ClientName, p.ProductName, p.ProductImage, sd.QuantitySold, s.SaleDate, (p.Price * sd.QuantitySold) as SalePrice" +
                                                    " FROM (((SalesDetails sd INNER JOIN Sales s on sd.SaleID = s.SaleID) LEFT JOIN Products p on sd.ProductID = p.ProductID)" +
                                                    " LEFT JOIN Clients c on s.ClientID = c.ClientID)" +
                                                    " WHERE sd.ProductID = @id AND s.SaleDate < @date", cnn);
                    command.Parameters.Add("@id", OleDbType.Integer).Value = products[comboBox1.SelectedIndex].getId();
                    command.Parameters.Add("@date", OleDbType.Date).Value = saleDate;
                    reader = command.ExecuteReader();

                    string saleid = "", clientName = "", productName = "", productimage = "", quantity = "", saledate = "", saleprice = "";
                    int rowNumber = 0;
                    dataGridView1.RowTemplate.Height = 90;
                    while (reader.Read())
                    {
                        saleid = reader["SaleID"].ToString();
                        clientName = reader["ClientName"].ToString();
                        productName = reader["ProductName"].ToString();
                        productimage = reader["ProductImage"].ToString();
                        quantity = reader["QuantitySold"].ToString();
                        saledate = reader["SaleDate"].ToString();
                        DateTime dateTime = DateTime.Parse(saledate);
                        saleprice = reader["SalePrice"].ToString();

                        string[] row = new string[] { "",
                            productName,
                            clientName,
                            saleid,
                            quantity,
                            dateTime.ToShortDateString(),
                            saleprice + " лв."
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
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            const string FONT = "c:/windows/fonts/arial.ttf";
            BaseFont bfTimes = BaseFont.CreateFont(FONT, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 12);           

            string salesDate = "";
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
                System.Drawing.Image imageByte = (System.Drawing.Image) row.Cells[0].Value;
                iTextSharp.text.Image myImage = iTextSharp.text.Image.GetInstance(imageByte, BaseColor.WHITE);
                pdfTable.AddCell(myImage);

                string productName = row.Cells[1].Value.ToString();
                pdfTable.AddCell(new Phrase(productName,times));

                string clientName = row.Cells[2].Value.ToString();
                pdfTable.AddCell(new Phrase(clientName, times));

                string saleId = row.Cells[3].Value.ToString();
                pdfTable.AddCell(new Phrase(saleId, times));

                string quantity = row.Cells[4].Value.ToString();
                pdfTable.AddCell(new Phrase(quantity + " броя", times));

                salesDate = row.Cells[5].Value.ToString();
                pdfTable.AddCell(new Phrase(salesDate, times));

                string price = row.Cells[6].Value.ToString();
                pdfTable.AddCell(new Phrase(price, times));
            }

            //Exporting to PDF.
            string folderPath = AppDomain.CurrentDomain.BaseDirectory + @"Reports\";
            using (FileStream stream = new FileStream(folderPath + "SalesReport.pdf", FileMode.Create))
            {
                Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();

                iTextSharp.text.Font header = new iTextSharp.text.Font(bfTimes, 25);
                Paragraph headerText = new Paragraph("Продажби преди дата " + salesDate, header);
                headerText.Alignment = Element.ALIGN_CENTER;
                headerText.SpacingAfter = 25;

                pdfDoc.Add(headerText);
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

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button2);
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button2);
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
