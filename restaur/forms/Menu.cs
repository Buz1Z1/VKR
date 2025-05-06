using Npgsql;
using restaur.forms;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace restaur
{
    public partial class Menu : Form
    {
        DB_connect dB_Connect = new DB_connect();
        public Menu()
        {
            InitializeComponent();
            draw_table();
            draw_rec();
            fill_teh();

        }
        public void draw_table()
        {
            //Заполнение таблицы
            dg_menu.Rows.Clear();
            dB_Connect.openConnect();
            var cmd = new NpgsqlCommand("select d.id,d.image,d.name,c.name,d.descryption,price from dishes as d " +
                "left join dish_category as c on d.category_id=c.id where d.name like '%" + search.Text + "%'", dB_Connect.conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            //Получение ответа от бд
            try
            {
                while (reader.Read())
                {
                    dg_menu.Rows.Add(reader[0].ToString(), Image.FromFile(reader[1].ToString()),
                        reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString());
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            cmd.Dispose();
            reader.DisposeAsync();
            dB_Connect.closeConnect();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Dish_edit dish_Edit = new Dish_edit();
            dish_Edit.Show();
            dish_Edit.btn_save.Enabled = true;
            dish_Edit.btn_save.Visible = true;

            var cmd = new NpgsqlCommand("Select * from dish_category", dB_Connect.conn);
            dB_Connect.openConnect();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmd.Dispose();
            dB_Connect.closeConnect();

            dish_Edit.category.DisplayMember = "name";
            dish_Edit.category.ValueMember = "id";
            dish_Edit.category.SelectedIndex = -1;
            dish_Edit.category.DataSource = dt;

            dish_Edit.btn_update.Enabled = false;
            dish_Edit.btn_update.Visible = false;

        }
        

        private void search_TextChanged(object sender, EventArgs e)
        {
            draw_table();
        }

        //тех карты
        private void fill_teh()
        {
            teh_card.Rows.Clear();
            dB_Connect.openConnect();
            var cmd = new NpgsqlCommand("SELECT id, name FROM dishes where name like '%" + search_teh.Text + "%'", dB_Connect.conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            //Получение ответа от бд
            while (reader.Read())
            {
                teh_card.Rows.Add(reader["id"].ToString(), reader["name"].ToString());
            }
            cmd.Dispose();
            dB_Connect.closeConnect();
        }


        private void dish_SelectedIndexChanged(object sender, EventArgs e)
        {
            //draw_rec();
        }


        //категория
        public void draw_rec()
        {
            dg_category.Rows.Clear();
            dB_Connect.openConnect();
            var cmd = new NpgsqlCommand("SELECT * FROM dish_category where name like '%" + search_category.Text + "%'", dB_Connect.conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            //Получение ответа от бд
            while (reader.Read())
            {
                dg_category.Rows.Add(reader["id"], reader["name"].ToString());
            }
            cmd.Dispose();
            dB_Connect.closeConnect();
        }



        private void pictureBox2_Click(object sender, EventArgs e) //добавить категорию
        {
            Add_category add_Category = new Add_category();
            add_Category.Show();
            add_Category.label1.Text = "Добавить категорию";

            add_Category.btn_save.Enabled = true;
            add_Category.btn_save.Visible = true;

            add_Category.btn_update.Enabled = false;
            add_Category.btn_update.Visible = false;
        }

        private void dg_category_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var dg = dg_category;
            string colname = dg.Columns[e.ColumnIndex].Name;
            if (colname == "edit1")
            {

                Add_category edit = new Add_category();
                edit.name.Text = dg.Rows[e.RowIndex].Cells["name_c"].Value.ToString();
                edit.id.Text = dg.Rows[e.RowIndex].Cells["id_c"].Value.ToString();
                edit.label1.Text = "Редактировать категорию";
                edit.btn_save.Enabled = false;
                edit.btn_save.Visible = false;

                edit.btn_update.Enabled = true;
                edit.btn_update.Visible = true;
                edit.ShowDialog();
            }
            if (colname == "delete1")
            {
                if (MessageBox.Show("Уверены, что хотите удалить запись?", "Удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        var cmd = new NpgsqlCommand("DELETE FROM dish_category where id = @id", dB_Connect.conn);
                        cmd.Parameters.AddWithValue("@id", Convert.ToInt16(dg.Rows[e.RowIndex].Cells["id_c"].Value));
                        dB_Connect.openConnect();
                        NpgsqlDataReader reader = cmd.ExecuteReader();
                        dB_Connect.closeConnect();
                        cmd.Dispose();
                        MessageBox.Show("Запись была удалена");
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    
                }
            }
        }

        private void dg_menu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var dg = dg_menu;
            string colname = dg.Columns[e.ColumnIndex].Name;
            if (colname == "edit")
            {
                var cmd = new NpgsqlCommand("Select image from dishes where id="+ Convert.ToInt16(dg.Rows[e.RowIndex].Cells["id"].Value), dB_Connect.conn);
                dB_Connect.openConnect();
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmd.Dispose();
                Dish_edit edit = new Dish_edit();
                edit.image_path = dt.Rows[0]["image"].ToString();
                edit.name.Text = dg.Rows[e.RowIndex].Cells["name_d"].Value.ToString();
                edit.descryption.Text = dg.Rows[e.RowIndex].Cells["desc"].Value.ToString();
                edit.image.ImageLocation = edit.image_path;
                edit.price.Text = dg.Rows[e.RowIndex].Cells["price"].Value.ToString();
                edit.id = Convert.ToInt16(dg.Rows[e.RowIndex].Cells["id"].Value);
                
                cmd = new NpgsqlCommand("Select * from dish_category",dB_Connect.conn);
                da = new NpgsqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                cmd.Dispose();
                dB_Connect.closeConnect();
                edit.category.DisplayMember = "name";
                edit.category.ValueMember = "id";
                edit.category.SelectedIndex = -1;
                edit.category.DataSource = dt;
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
                    var cmd = new NpgsqlCommand("DELETE FROM dishes where id = @id", dB_Connect.conn);
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt16(dg.Rows[e.RowIndex].Cells["id"].Value));
                    dB_Connect.openConnect();
                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    dB_Connect.closeConnect();
                    cmd.Dispose();
                    MessageBox.Show("Запись была удалена");
                    if (MessageBox.Show("Успех", "Успех", MessageBoxButtons.OK) == DialogResult.OK)
                        draw_rec();
                }
            }
        }

        private void teh_card_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var dg = teh_card;
            string colname = dg.Columns[e.ColumnIndex].Name;
            if (colname == "teh_edit")
            {
                //var cmd = new NpgsqlCommand("select id, name, price from storage" + Convert.ToInt16(dg.Rows[e.RowIndex].Cells["id"].Value), dB_Connect.conn);
                //dB_Connect.openConnect();
                //NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //da.Fill(dt);
                //cmd.Dispose();
                //dB_Connect.closeConnect();
                teh_editcs t_e = new teh_editcs();
                t_e.Show();
                
            }
            if (colname == "delete")// поменять
            {
                if (MessageBox.Show("Уверены, что хотите удалить запись?", "Удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    var cmd = new NpgsqlCommand("", dB_Connect.conn);
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt16(dg.Rows[e.RowIndex].Cells["id"].Value));
                    dB_Connect.openConnect();
                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    dB_Connect.closeConnect();
                    cmd.Dispose();
                    MessageBox.Show("Запись была удалена");
                    if (MessageBox.Show("Успех", "Успех", MessageBoxButtons.OK) == DialogResult.OK)
                        draw_rec();
                }
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            teh_editcs teh_Editcs = new teh_editcs();
            teh_Editcs.Show();
        }
    }
}
