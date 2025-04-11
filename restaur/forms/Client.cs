using Npgsql;
using restaur.forms;
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
    public partial class Client : Form
    {
        DB_connect dB_Connect = new DB_connect();
        public Client()
        {
            InitializeComponent();
            draw_table();
        }
        public void draw_table()
        {
            //Заполнение таблицы
            dg_client.Rows.Clear();
            dB_Connect.openConnect();
            var cmd = new NpgsqlCommand("select * from client where fio like '%" + search.Text + "%'", dB_Connect.conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            //Получение ответа от бд
            while (reader.Read())
            {

                dg_client.Rows.Add(reader[0].ToString(), reader[1].ToString(),
                    reader[2].ToString(), reader[3].ToString(), reader[4].ToString(),
                    reader[5].ToString(), reader[6].ToString(), reader[7].ToString());
            }
            cmd.Dispose();
            dB_Connect.closeConnect();
        }

        private void search_TextChanged(object sender, EventArgs e)
        {
            draw_table();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Add_client add_Client = new Add_client();
            add_Client.Show();
            add_Client.label1.Text = "Создать клиента";
            add_Client.btn_save.Enabled = true;
            add_Client.btn_save.Visible = true;

            add_Client.btn_update.Enabled = false;
            add_Client.btn_update.Visible = false;
        }

        private void dg_client_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var dg = dg_client;
            string colname = dg.Columns[e.ColumnIndex].Name;
            if (colname == "edit")
            {

                Add_client edit = new Add_client();
                edit.fio.Text = dg.Rows[e.RowIndex].Cells["fio"].Value.ToString();
                edit.id.Text = dg.Rows[e.RowIndex].Cells["id"].Value.ToString();
                edit.email.Text = dg.Rows[e.RowIndex].Cells["email"].ToString();
                edit.phone.Text = dg.Rows[e.RowIndex].Cells["phone"].ToString();
                edit.card_num.Text = dg.Rows[e.RowIndex].Cells["card_num"].ToString();
                edit.date_birth.Text= dg.Rows[e.RowIndex].Cells["birth"].ToString();
                edit.bonus.Text = dg.Rows[e.RowIndex].Cells["bonus"].ToString();
                edit.d_create.Text = dg.Rows[e.RowIndex].Cells["date_create"].ToString();

                edit.label1.Text = "Редактировать клиента";
                
                edit.btn_save.Enabled = false;
                edit.btn_save.Visible = false;

                edit.btn_update.Enabled = true;
                edit.btn_update.Visible = true;
                edit.ShowDialog();
            }
            if (colname == "delete")
            {
                if (MessageBox.Show("Уверены, что хотите удалить запись?", "Удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    var cmd = new NpgsqlCommand("DELETE FROM client where id = @id", dB_Connect.conn);
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt16(dg.Rows[e.RowIndex].Cells["id"].Value));
                    dB_Connect.openConnect();
                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    dB_Connect.closeConnect();
                    cmd.Dispose();
                    MessageBox.Show("Запись была удалена");
                }
            }
        }

        private void Client_Load(object sender, EventArgs e)
        {

        }
    }
}
