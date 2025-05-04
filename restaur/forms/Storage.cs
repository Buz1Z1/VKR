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
            draw_table();
            fill_order();
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
                dg_storage.Rows.Add(reader[0], reader[1], reader[2], reader[3], reader[4], reader[5]);
            }
            cmd.Dispose();
            dB_Connect.closeConnect();
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
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if(folderBrowserDialog.ShowDialog() == DialogResult.OK)
                ExportDatagrid(order_prod,folderBrowserDialog.SelectedPath);
        }

        public void ExportDatagrid(DataGridView dg, string filePath)
        {
            try
            {
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook wb = excelApp.Workbooks.Add();
                Excel.Worksheet ws = wb.Sheets[1];
                excelApp.Visible = false;
                excelApp.DisplayAlerts = false;
                ws.Cells[1, 2] = "Заявка на заказ товара от " + DateTime.Now.ToShortDateString();
                ws.Cells[2, 2] = "Наименование";
                ws.Cells[2, 3] = "Количество для заказа";
                for (int i = 0; i <=dg.RowCount-2; i++)
                {
                    ws.Cells[i + 3, 1] = i + 1;
                    for (int j = 0; j <=dg.ColumnCount - 2; j++)
                    {
                        ws.Cells[i + 3, j+2] = dg.Rows[i].Cells[j].Value;
                        
                    }
                }
                ws.Columns.AutoFit();
                string fileName = DateTime.Now.ToString("dd_MM HH_mm") + ".xlsx";
                string fullPath=System.IO.Path.Combine(filePath,fileName);
                wb.SaveAs(fullPath);
                wb.Close();
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(ws);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                dg.Rows.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте в Excel: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void order_prod_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var dg = order_prod;
            string colname = dg.Columns[e.ColumnIndex].Name;
            if (colname == "del_or" && dg.RowCount!=1 && dg.Rows[e.RowIndex].Index!=dg.RowCount-1)
            { 
                dg.Rows.RemoveAt(dg.Rows[e.RowIndex].Index); //обработка нулевой строки
            }
        }
        private void fill_order()
        {
            dg_storage.Rows.Clear();
            dB_Connect.openConnect();
            var cmd = new NpgsqlCommand("select name, (min_count-count) as need from storage where count<min_count;", dB_Connect.conn);
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
