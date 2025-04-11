using Guna.UI2.WinForms;
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
    public partial class Employee : Form
    {
        DB_connect dB_Connect = new DB_connect();
        public Employee()
        {
            InitializeComponent();
            draw_table();
            
        }
        public void draw_table()
        {
            //Заполнение таблицы

            guna2DataGridView1.Rows.Clear();
            dB_Connect.openConnect();
            var cmd = new NpgsqlCommand("SELECT * FROM employee where fio like '%" + search.Text + "%'", dB_Connect.conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            //Получение ответа от бд
            while (reader.Read())
            {
                guna2DataGridView1.Rows.Add(reader["id"].ToString(), reader["fio"].ToString(), reader["birth"].ToString(),
                    reader["addres"].ToString(), reader["passport"].ToString(), reader["phone"].ToString(),
                    reader["job"].ToString(), reader["salary"].ToString());
            }
            cmd.Dispose();
            dB_Connect.closeConnect();
            //string query = "SELECT * FROM employee where fio like '%" + search.Text + "%'";
            //ListBox listBox = new ListBox();
            //listBox.Items.Add(id);
            //listBox.Items.Add(fio);
            //listBox.Items.Add(birth);
            //listBox.Items.Add(addres);
            //listBox.Items.Add(passport);
            //listBox.Items.Add(phone);
            //listBox.Items.Add(salary);
            //listBox.Items.Add(job);
            //dB_Connect.LoadDG(guna2DataGridView1,listBox,query);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //редактирование и удаление строк с сотрудниками
            string colname = guna2DataGridView1.Columns[e.ColumnIndex].Name;
            if (colname =="edit")
            {
                
                Add_emp edit = new Add_emp();
                edit.label1.Text = "Редактирование сотрудника";
                edit.fio.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["fio"].Value.ToString();
                edit.date_birth.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["birth"].Value.ToString();
                edit.addres.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["addres"].Value.ToString();
                edit.passport.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["passport"].Value.ToString();
                edit.phone.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["phone"].Value.ToString();
                edit.job.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["job"].Value.ToString();
                edit.salary.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["salary"].Value.ToString();
                edit.id.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString();

                edit.btn_save.Enabled = false;
                edit.btn_save.Visible = false;

                edit.btn_update.Enabled = true;
                edit.btn_update.Visible = true;
                edit.ShowDialog();
            }
            if (colname=="delete")
            {
                if (MessageBox.Show("Уверены, что хотите удалить запись?","Удаление",MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    
                    var cmd= new NpgsqlCommand("DELETE FROM employee where id = @id", dB_Connect.conn);
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt16(guna2DataGridView1.Rows[e.RowIndex].Cells["id"].Value));
                    dB_Connect.openConnect();
                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    dB_Connect.closeConnect();
                    cmd.Dispose();
                    MessageBox.Show("Запись была удалена");
                    
                }
            }
            draw_table();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
            Add_emp add_Emp = new Add_emp();
            add_Emp.btn_save.Enabled = true;
            add_Emp.btn_save.Visible = true;

            add_Emp.label1.Text = "Добавление сотрудника";
            add_Emp.btn_update.Enabled = false;
            add_Emp.btn_update.Visible = false;
            add_Emp.ShowDialog();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Main_menu mm = new Main_menu();
            mm.Show();
        }

        private void search_TextChanged(object sender, EventArgs e)
        {
            draw_table();
        }
    }
}
