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
            gunaChart1.Datasets.Clear();
            var dataset = new GunaLineDataset();
            //Заполнение таблицы
            dB_Connect.openConnect();
            var cmd = new NpgsqlCommand("select date, count(*) from orders group by date", dB_Connect.conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            //Получение ответа от бд
            while (reader.Read())
            {
                dataset.DataPoints.Add(reader[0].ToString(), double.Parse(reader[1].ToString()));
            }
            cmd.Dispose();
            dB_Connect.closeConnect();
            gunaChart1.Title.Text = "Количество заказов";
            gunaChart1.YAxes.Display = true;
            gunaChart1.XAxes.Display = true;
            gunaChart1.Datasets.Add(dataset);
            //dataset.Container.Dispose();
        }

        private void count_dishes()
        { 
            gunaChart2.Datasets.Clear();
            
        }
    }

    
}
