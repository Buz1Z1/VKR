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
    public partial class teh_editcs : Form
    {
        //public class Product
        //{
        //    public int id { get; set; }
        //    public string name { get; set; }
        //    public double price { get; set; }
        //}
        public int id_dish;
        DataTable dt_prod = new DataTable();
        
        DB_connect dB_Connect = new DB_connect();
        public teh_editcs(int id_dish)
        {
            InitializeComponent();
            fill_table_prod();
            fill_teh(id_dish);
        }
        private void fill_table_prod()
        {
            dB_Connect.openConnect();
            var cmd = new NpgsqlCommand("select id, name, price from storage", dB_Connect.conn);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            da.Fill(dt_prod);
            cmd.Dispose();
        }

        public void fill_teh(int id_dish)
        {
            //dB_Connect.openConnect();
            //var cmd = new NpgsqlCommand("select name, price from storage where id_dish=@id_dish", dB_Connect.conn);
            //cmd.Parameters.AddWithValue("@id_dish",id_dish);

            //NpgsqlDataReader reader = cmd.ExecuteReader();
            var combobox = (DataGridViewComboBoxColumn)dg_teh.Columns[1];
            combobox.DisplayMember = "name";
            combobox.ValueMember = "id";
            combobox.DataSource = dt_prod;
            
            //cmd.Dispose();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dg_teh_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(dg_teh.Columns[e.ColumnIndex].Name == "name")
            {
                //обработка стоимости
            }
        }
    }
}
