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

namespace restaur.forms
{
    public partial class Tables : Form
    {
        DB_connect dB_Connect=new DB_connect();
        public Tables()
        {
            InitializeComponent();
            draw_table();
            draw_reserv();
        }

        public void draw_table()
        {
            //Заполнение таблицы

            guna2DataGridView1.Rows.Clear();
            dB_Connect.openConnect();
            var cmd = new NpgsqlCommand("SELECT * FROM tables where name like '%" + search.Text + "%'", dB_Connect.conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            //Получение ответа от бд
            while (reader.Read())
            {
                guna2DataGridView1.Rows.Add(reader["id"].ToString(), reader["name"].ToString());
            }
            cmd.Dispose();
            dB_Connect.closeConnect();
            
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = guna2DataGridView1.Columns[e.ColumnIndex].Name;
            if (colname == "edit")
            {

                Add_table edit = new Add_table();
                edit.label1.Text = "Изменить";
                edit.name.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["name"].Value.ToString();
                edit.id = Convert.ToInt16(guna2DataGridView1.Rows[e.RowIndex].Cells["id"].Value);

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

                    var cmd = new NpgsqlCommand("DELETE FROM tables where id = @id", dB_Connect.conn);
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt16(guna2DataGridView1.Rows[e.RowIndex].Cells["id"].Value));
                    dB_Connect.openConnect();
                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    dB_Connect.closeConnect();
                    cmd.Dispose();
                    if (MessageBox.Show("Успех", "Успех", MessageBoxButtons.OK) == DialogResult.OK)
                        draw_table();
                }
            }
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Add_table add_Table = new Add_table();
            add_Table.btn_save.Enabled = true;
            add_Table.btn_save.Visible = true;

            add_Table.btn_update.Enabled = false;
            add_Table.btn_update.Visible = false;
            add_Table.label1.Text = "Добавить стол";
            add_Table.Show();
        }

        private void search_TextChanged(object sender, EventArgs e)
        {
            draw_table();
        }
        public void draw_reserv()
        {
            //Заполнение таблицы

            guna2DataGridView2.Rows.Clear();
            dB_Connect.openConnect();
            var cmd = new NpgsqlCommand("select r.id, t.name, r.descr, r.time, r.date from reserv r " +
                "left join tables t on r.id_table=t.id", dB_Connect.conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            //Получение ответа от бд
            while (reader.Read())
            {
                guna2DataGridView2.Rows.Add(reader["id"].ToString(), reader["name"].ToString(), 
                    reader["descr"].ToString(), reader["time"].ToString(), reader["date"].ToString());
            }
            cmd.Dispose();
            dB_Connect.closeConnect();

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            dB_Connect.openConnect();
            Add_reserv edit = new Add_reserv();
            var cmd = new NpgsqlCommand("Select * from tables", dB_Connect.conn);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmd.Dispose();
            dB_Connect.closeConnect();
            edit.btn_save.Enabled = true;
            edit.btn_save.Visible = true;
            edit.label1.Text = "Добавить бронь";
            edit.btn_update.Enabled = false;
            edit.btn_update.Visible = false;
            edit.tables.DisplayMember = "name";
            edit.tables.ValueMember = "id";
            edit.tables.SelectedIndex = -1;
            edit.tables.DataSource = dt;
            edit.Show();
        }

        private void guna2DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = guna2DataGridView2.Columns[e.ColumnIndex].Name;
            if (colname == "edit_res")
            {
                Add_reserv edit = new Add_reserv();
                edit.label1.Text = "Изменить";
                edit.descr.Text = guna2DataGridView2.Rows[e.RowIndex].Cells["descr"].Value.ToString();
                edit.id = Convert.ToInt16(guna2DataGridView2.Rows[e.RowIndex].Cells["id_r"].Value);
                edit.time.Text=guna2DataGridView2.Rows[e.RowIndex].Cells["time"].Value.ToString();
                edit.date.Value=Convert.ToDateTime(guna2DataGridView2.Rows[e.RowIndex].Cells["date"].Value);
                dB_Connect.openConnect();
                var cmd = new NpgsqlCommand("Select * from tables", dB_Connect.conn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmd.Dispose();
                dB_Connect.closeConnect();
                edit.label1.Text = "Редактирвоание брони";
                edit.tables.DisplayMember = "name";
                edit.tables.ValueMember = "id";
                edit.tables.SelectedIndex = -1;
                edit.tables.DataSource = dt;
                edit.btn_save.Enabled = false;
                edit.btn_save.Visible = false;

                edit.btn_update.Enabled = true;
                edit.btn_update.Visible = true;
                edit.ShowDialog();
            }
            if (colname == "del_res")
            {
                if (MessageBox.Show("Уверены, что хотите удалить запись?", "Удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    var cmd = new NpgsqlCommand("DELETE FROM reserv where id = @id", dB_Connect.conn);
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt16(guna2DataGridView2.Rows[e.RowIndex].Cells["id_r"].Value));
                    dB_Connect.openConnect();
                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    dB_Connect.closeConnect();
                    cmd.Dispose();
                    if (MessageBox.Show("Успех", "Успех", MessageBoxButtons.OK) == DialogResult.OK)
                        draw_table();
                }
            }
        }
    }
}
