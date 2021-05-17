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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.CenterToScreen();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*DialogResult result = MessageBox.Show("Do you really want to exit the application?", "Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }*/

            Application.Exit();
        }

        public void applyMouseHoverEffects(Button button)
        {
            button.BackColor = Color.FromArgb(42, 84, 127);
        }

        public void clearMouseHoverEffects(Button button)
        {
            button.BackColor = Color.Transparent;
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            applyMouseHoverEffects(button2);
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            clearMouseHoverEffects(button2);
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            applyMouseHoverEffects(button1);
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            clearMouseHoverEffects(button1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UpdateData updateData = new UpdateData();
            updateData.Owner = this;
            updateData.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddData addDataForm = new AddData();
            addDataForm.Owner = this;
            addDataForm.Show();
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            applyMouseHoverEffects(button3);
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            clearMouseHoverEffects(button3);
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            applyMouseHoverEffects(button4);
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            clearMouseHoverEffects(button4);
        }

        private void button6_MouseEnter(object sender, EventArgs e)
        {
            applyMouseHoverEffects(button6);
        }

        private void button6_MouseLeave(object sender, EventArgs e)
        {
            clearMouseHoverEffects(button6);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Queries queries = new Queries();
            queries.Owner = this;
            queries.Show();
        }
    }
}
