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
    public partial class AddData : Form
    {
        public AddData()
        {
            InitializeComponent();
        }

        MainForm mainForm = new MainForm();

        private void AddData_Load(object sender, EventArgs e)
        {

            this.Owner.Hide();
            this.CenterToScreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Owner.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddSale addSale = new AddSale();
            addSale.Owner = this;
            addSale.Show();
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {            
            mainForm.applyMouseHoverEffects(button2);
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button2);
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

        private void button6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddProduct addProduct = new AddProduct();
            addProduct.Owner = this;
            addProduct.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddClient addClient = new AddClient();
            addClient.Owner = this;
            addClient.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AddProduction addProduction = new AddProduction();
            addProduction.Owner = this;
            addProduction.Show();
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            mainForm.applyMouseHoverEffects(button1);
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            mainForm.clearMouseHoverEffects(button1);
        }
    }
}
