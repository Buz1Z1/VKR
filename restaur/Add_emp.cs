using Npgsql;
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
    public partial class Add_emp : Form
    {
        input_check i_c = new input_check();
        DB_connect dB_Connect = new DB_connect();
        Employee emp = new Employee();
        public Add_emp()
        {
            InitializeComponent();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void clear_text()
        {
            fio.Clear();
            date_birth.Clear();
            addres.Clear();
            email.Clear();
            phone.Clear();
            job.SelectedIndex=-1;
            salary.Clear();
            id.Clear();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            //Добавление сотрудника
            string birth = date_birth.Text;
            DateTime b = DateTime.Parse(birth);
            try
            {
                var cmd = new NpgsqlCommand("INSERT into employee (fio,birth,addres,email,phone,job,salary) values(@fio,@birth,@addres,@email,@phone,@job,@salary)", dB_Connect.conn);
                cmd.Parameters.AddWithValue("@fio", fio.Text);
                cmd.Parameters.AddWithValue("@birth", b.ToShortDateString());
                cmd.Parameters.AddWithValue("@addres", addres.Text);
                cmd.Parameters.AddWithValue("@email", email.Text);
                cmd.Parameters.AddWithValue("@phone", phone.Text);
                cmd.Parameters.AddWithValue("@job", job.Text);
                cmd.Parameters.AddWithValue("@salary", Convert.ToDouble(salary.Text));
                dB_Connect.openConnect();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                dB_Connect.closeConnect();
                MessageBox.Show("Успех");
                clear_text();
                if (MessageBox.Show("Успех", "Успех", MessageBoxButtons.OK) == DialogResult.OK)
                    emp.draw_table();
                this.Close();
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            
            string birth = date_birth.Text;
            DateTime b = DateTime.Parse(birth);
            var cmd = new NpgsqlCommand("update employee SET fio=@fio,birth=@birth,addres=@addres,email=@email,phone=@phone,job=@job,salary=@salary WHERE id =@id", dB_Connect.conn);
            cmd.Parameters.AddWithValue("@id", Convert.ToInt16(id.Text));
            cmd.Parameters.AddWithValue("@fio", fio.Text);
            cmd.Parameters.AddWithValue("@birth", b);
            cmd.Parameters.AddWithValue("@addres", addres.Text);
            cmd.Parameters.AddWithValue("@email", email.Text);
            cmd.Parameters.AddWithValue("@phone", phone.Text);
            cmd.Parameters.AddWithValue("@job", job.Text);
            cmd.Parameters.AddWithValue("@salary", Convert.ToDouble(salary.Text));
            dB_Connect.openConnect();
            NpgsqlDataReader reader = cmd.ExecuteReader();
            dB_Connect.closeConnect();
            MessageBox.Show("Успех");
            clear_text();
            if (MessageBox.Show("Успех", "Успех", MessageBoxButtons.OK) == DialogResult.OK)
                emp.draw_table();
            this.Close();
        }

        private void salary_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (i_c.isdigit(sender, e))
                e.Handled = true;
        }
    }
}
