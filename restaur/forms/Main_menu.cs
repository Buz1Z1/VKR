using restaur.forms;
using restaur.forms.POS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace restaur
{
    public partial class Main_menu : Form
    {
        public Main_menu()
        {
            InitializeComponent();
            date_time.Text = DateTime.Now.ToLongDateString();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Меню";
            
            AddControls(new Menu());
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            label1.Text = "Отчеты";
            AddControls(new Reports());
        }

        private void AddControls(Form form)
        {
            Center.Controls.Clear();
            form.Dock = DockStyle.Fill;
            form.TopLevel = false;
            Center.Controls.Add(form);
            form.Show();
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            //Application.Exit();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            label1.Text = "Сотрудники";
            AddControls(new Employee());
        }

        private void guna2Button5_Click_1(object sender, EventArgs e)
        {
            label1.Text = "Заказы";
            AddControls(new Orders());
        }

        private void guna2ControlBox2_Click(object sender, EventArgs e)
        {

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2ControlBox3_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            label1.Text = "Столы";
            AddControls(new Tables());
        }

        private void btn_clients_Click(object sender, EventArgs e)
        {
            label1.Text = "Клиенты";
            AddControls(new Client());
        }

        private void btn_POS_Click(object sender, EventArgs e)
        {
            this.Close();
            POS pos = new POS();
            pos.Show();
        }

        private void btn_wareh_Click(object sender, EventArgs e)
        {
            label1.Text = "Склад";
            AddControls(new Storage());
        }
    }
}
