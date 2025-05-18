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
using Excel = Microsoft.Office.Interop.Excel;

namespace restaur.forms
{
    public partial class Storage : Form
    {
        
        DB_connect dB_Connect =new DB_connect();
        public Storage()
        {
            InitializeComponent();
            
            fill_order();
            draw_table();
        }

        public void draw_table()
        {
            //Заполнение таблицы
            dg_storage.Rows.Clear();
            dB_Connect.openConnect();
            var cmd = new NpgsqlCommand("select * from storage where name like '%" + search.Text + "%'", dB_Connect.conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            //Получение ответа от бд
            while (reader.Read())
            {
                dg_storage.Rows.Add(reader[0], reader[1], reader[2], reader[3], reader[4], reader[5], reader[6]);
            }
            alert();
            cmd.Dispose();
            dB_Connect.closeConnect();
        }
        private void alert()
        {
            for(int i=0;i<dg_storage.RowCount;i++)
            {
                DateTime dt = Convert.ToDateTime(dg_storage.Rows[i].Cells["date"].Value.ToString());
                dt=dt.AddDays(Convert.ToInt16(dg_storage.Rows[i].Cells["exp_date"].Value.ToString()));
                if (dt < DateTime.UtcNow)
                    dg_storage.Rows[i].Cells["date"].Style= new DataGridViewCellStyle { ForeColor=Color.Red };
                else
                    dg_storage.Rows[i].Cells["date"].Style= dg_storage.DefaultCellStyle;
                if(Convert.ToDouble(dg_storage.Rows[i].Cells["rest"].Value) < Convert.ToDouble(dg_storage.Rows[i].Cells["limit"].Value))
                    dg_storage.Rows[i].Cells["rest"].Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                else
                    dg_storage.Rows[i].Cells["rest"].Style = dg_storage.DefaultCellStyle;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Add_storage add_Storage = new Add_storage();
            add_Storage.btn_update.Enabled = false;
            add_Storage.btn_update.Visible = false;
            add_Storage.btn_save.Enabled = true;
            add_Storage.btn_save.Visible = true;
            add_Storage.Show();
        }

        private void search_TextChanged(object sender, EventArgs e)
        {
            draw_table();
        }

        private void dg_storage_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var dg = dg_storage;
            string colname = dg.Columns[e.ColumnIndex].Name;
            if (colname == "edit")
            {

                Add_storage add_Storage = new Add_storage();
                add_Storage.id= Convert.ToInt16(dg.Rows[e.RowIndex].Cells["id"].Value.ToString());
                
                add_Storage.name.Text = dg.Rows[e.RowIndex].Cells["name"].Value.ToString();
                add_Storage.price.Text = dg.Rows[e.RowIndex].Cells["price"].Value.ToString();
                add_Storage.count.Text = dg.Rows[e.RowIndex].Cells["rest"].Value.ToString();
                add_Storage.min_count.Text = dg.Rows[e.RowIndex].Cells["limit"].Value.ToString();
                add_Storage.exp_date.Text= dg.Rows[e.RowIndex].Cells["exp_date"].Value.ToString();
                add_Storage.btn_save.Enabled = false;
                add_Storage.btn_save.Visible = false;

                add_Storage.btn_update.Enabled = true;
                add_Storage.btn_update.Visible = true;
                add_Storage.ShowDialog();
            }
            if (colname == "delete")
            {
                if (MessageBox.Show("Уверены, что хотите удалить запись?", "Удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        var cmd = new NpgsqlCommand("DELETE FROM storage where id = @id", dB_Connect.conn);
                        cmd.Parameters.AddWithValue("@id", Convert.ToInt16(dg.Rows[e.RowIndex].Cells["id"].Value));
                        dB_Connect.openConnect();
                        NpgsqlDataReader reader = cmd.ExecuteReader();
                        dB_Connect.closeConnect();
                        cmd.Dispose();
                        MessageBox.Show("Запись была удалена");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Export_excel export_Excel = new Export_excel();
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if(folderBrowserDialog.ShowDialog() == DialogResult.OK)
                export_Excel.ExportDatagrid(order_prod,folderBrowserDialog.SelectedPath);
        }

        
        
        private void order_prod_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var dg = order_prod;
            string colname = dg.Columns[e.ColumnIndex].Name;
            if (colname == "del_or" && dg.RowCount!=1 && dg.Rows[e.RowIndex].Index!=dg.RowCount-1)
            { 
                dg.Rows.RemoveAt(dg.Rows[e.RowIndex].Index);
            }
        }
        private void fill_order()
        {
            dg_storage.Rows.Clear();
            dB_Connect.openConnect();
            var cmd = new NpgsqlCommand("select name, CASE WHEN (min_count-count)>=0 then (min_count-count) ELSE min_count END from storage where count<min_count or " +
                "CAST(date as DATE)+exp_date < current_date;", dB_Connect.conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            //Получение ответа от бд
            while (reader.Read())
            {
                order_prod.Rows.Add(reader[0], reader[1]);
            }
            cmd.Dispose();
            dB_Connect.closeConnect();

        }
    }
}
