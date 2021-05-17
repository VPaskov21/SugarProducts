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
    public partial class UpdateData : Form
    {
        public UpdateData()
        {
            InitializeComponent();
        }

        MainForm mainForm = new MainForm();

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button2);
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button2);
        }

        private void UpdateData_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            this.Owner.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Owner.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SearchClient searchClient = new SearchClient();
            searchClient.Owner = this;
            searchClient.Show();
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button1);
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button1);
        }

        private void button5_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button5);
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button5);
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button3);
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button3);
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button4);
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button4);
        }

        private void button6_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button6);
        }

        private void button6_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button6);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SearchProduct searchProduct = new SearchProduct();
            searchProduct.Owner = this;
            searchProduct.Show();


            /*UpdateProduct updateProduct = new UpdateProduct();
            updateProduct.Owner = this;
            updateProduct.Show();*/
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SearchProduction searchProduction = new SearchProduction();
            searchProduction.Owner = this;
            searchProduction.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SearchSale searchSale = new SearchSale();
            searchSale.Owner = this;
            searchSale.Show();
        }
    }
}
