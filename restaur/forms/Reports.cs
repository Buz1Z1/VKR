using Guna.Charts.WinForms;
using Microsoft.Office.Interop.Word;
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
    public partial class Reports : Form
    {
        DB_connect dB_Connect = new DB_connect();
        public Reports()
        {
            InitializeComponent();
            count_orders();
        }
        private void count_orders()
        {
            try
            {
                gunaChart1.Datasets.Clear();
                var dataset = new GunaLineDataset();
                //Заполнение таблицы
                dB_Connect.openConnect();
                var cmd = new NpgsqlCommand("select CAST(date as DATE), count(*) from orders group by CAST(date as DATE) order by CAST(date as DATE) ASC;", dB_Connect.conn);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                
                //Получение ответа от бд
                while (reader.Read())
                {
                    dataset.DataPoints.Add(reader[0].ToString(), double.Parse(reader[1].ToString()));
                }
                dB_Connect.closeConnect();
                cmd.Dispose();
                //gunaChart1.Title.Text = "Количество заказов";
                gunaChart1.Legend.Display = false;
                gunaChart1.YAxes.Display = true;
                gunaChart1.XAxes.Display = true;
                gunaChart1.Datasets.Add(dataset);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                gunaChart2.Datasets.Clear();
                var dataset = new GunaBarDataset();
                string date1 = dateTimePicker1.Value.ToShortDateString();
                string date2 = dateTimePicker4.Value.ToShortDateString();
                var cmd = new NpgsqlCommand("select dd.name, sum(od.count) from order_detail od " +
                    "join dishes dd on dd.id=od.id_dish join orders oo on oo.id=od.id_order " +
                    "where CAST(oo.date as DATE) between '" + date1 + "' and '" + date2 + "' group by dd.name", dB_Connect.conn);
                dB_Connect.openConnect();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    dataset.DataPoints.Add(reader[0].ToString(), double.Parse(reader[1].ToString()));
                }
                dB_Connect.closeConnect();
                cmd.Dispose();
                gunaChart2.YAxes.Display = true;
                gunaChart2.XAxes.Display = true;
                gunaChart2.Legend.Display = false;
                gunaChart2.Datasets.Add(dataset);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                gunaChart3.Datasets.Clear();
                double total = 0;
                var dataset = new GunaBarDataset();
                string date1 = dateTimePicker2.Value.ToShortDateString();
                string date2 = dateTimePicker3.Value.ToShortDateString();
                var cmd = new NpgsqlCommand("select o.date, sum(o.sum) from orders o where CAST(o.date as DATE) between '" + date1 + "' and '" + date2 + "' group by o.date order by CAST(o.date as DATE) ASC", dB_Connect.conn);
                dB_Connect.openConnect();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    dataset.DataPoints.Add(reader[0].ToString(), double.Parse(reader[1].ToString()));
                    total += double.Parse(reader[1].ToString());
                }
                dB_Connect.closeConnect();
                cmd.Dispose();
                gunaChart3.YAxes.Display = true;
                gunaChart3.XAxes.Display = true;
                gunaChart3.Legend.Display = false;
                gunaChart3.Datasets.Add(dataset);
                label5.Text = total.ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void dateTimePicker5_ValueChanged(object sender, EventArgs e)
        {
            gunaChart4.Datasets.Clear();
            var dataset = new GunaBarDataset();
            string date1 = dateTimePicker5.Value.ToShortDateString();
            string date2 = dateTimePicker6.Value.ToShortDateString();
            dB_Connect.openConnect();
            var cmd = new NpgsqlCommand("select e.fio, count(o.id_emp) from orders o join employee e on o.id_emp=e.id" +
                " where CAST(date as DATE) between '" + date1 + "' and '" + date2 + "' group by e.fio", dB_Connect.conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                dataset.DataPoints.Add(reader[0].ToString(), double.Parse(reader[1].ToString()));
            }
            dB_Connect.closeConnect();
            cmd.Dispose();
            gunaChart4.YAxes.Display = true;
            gunaChart4.XAxes.Display = true;
            gunaChart4.Legend.Display = false;
            gunaChart4.Datasets.Add(dataset);
        }
    }

    
}
