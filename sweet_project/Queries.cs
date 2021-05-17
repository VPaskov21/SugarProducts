using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sweet_project
{
    public partial class Queries : Form
    {
        public Queries()
        {
            InitializeComponent();
        }

        MainForm mainForm = new MainForm();

        private void button2_Click(object sender, EventArgs e)
        {
            SalesBeforeDate salesBeforeDate = new SalesBeforeDate();
            salesBeforeDate.Owner = this;
            salesBeforeDate.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Owner.Show();
            this.Close();
        }

        private void Queries_Load(object sender, EventArgs e)
        {
            this.Owner.Hide();
            this.CenterToScreen();
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button2);
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button2);
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button1);
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClientWithMostSales clientsales = new ClientWithMostSales();
            clientsales.Owner = this;
            clientsales.Show();
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button3);
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SalesByCategory sales = new SalesByCategory();
            sales.Owner = this;
            sales.Show();
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button4);
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button4);
        }

        private void button5_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button5);
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button5);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ProductsByCategory products = new ProductsByCategory();
            products.Owner = this;
            products.Show();
        }

        private void button7_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button7);
        }

        private void button7_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button7);
        }

        private void button8_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button8);
        }

        private void button8_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button8);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ProducedSoldAvailableProducts products = new ProducedSoldAvailableProducts();
            products.Owner = this;
            products.Show();
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

        private void button5_Click(object sender, EventArgs e)
        {
            MostSoldProducts products = new MostSoldProducts();
            products.Owner = this;
            products.Show();
        }
    }
}
