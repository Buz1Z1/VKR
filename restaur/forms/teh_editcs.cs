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
        public int id_dish;
        DataTable dt_prod = new DataTable();
        List<Product> list = new List<Product>();
        class Product
        {
            public int id { get; set; }
            public string name { get; set; }
            public double price { get; set; }
            
        }

        DB_connect dB_Connect = new DB_connect();
        public teh_editcs()
        {
            InitializeComponent();
            fill_table_prod();
            fill_teh();
        }
        private void fill_table_prod()
        {
            dB_Connect.openConnect();
            var cmd = new NpgsqlCommand("select id, name, price from storage", dB_Connect.conn);
            //NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            //Получение ответа от бд
            while (reader.Read())
            {
                Product product = new Product();
                product.id = Convert.ToInt16(reader["id"].ToString());
                product.name = reader["name"].ToString();
                product.price = Convert.ToDouble(reader["price"].ToString());
                list.Add(product);
            }
            cmd.Dispose();
            dB_Connect.closeConnect();
            //da.Fill(dt_prod);
            //cmd.Dispose();
        }

        public void fill_teh()
        {
            //dB_Connect.openConnect();
            //var cmd = new NpgsqlCommand("select name, price from storage where id_dish=@id_dish", dB_Connect.conn);
            //cmd.Parameters.AddWithValue("@id_dish",id_dish);

            //NpgsqlDataReader reader = cmd.ExecuteReader();
            var combobox = (DataGridViewComboBoxColumn)dg_teh.Columns[1];
            combobox.DataSource = list;
            combobox.DisplayMember = "name";
            combobox.ValueMember = "id";
            //combobox.DataSource = list;
            
            //cmd.Dispose();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dg_teh_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

            double total = 0;
            for(int i=0;i<dg_teh.RowCount-1;i++)
            {
                int id = Convert.ToInt16(dg_teh.Rows[i].Cells[0].Value.ToString());
                Product pp = list.First(p => p.id == id);
                total += pp.price * double.Parse(dg_teh.Rows[i].Cells[2].Value.ToString());
            }
            total_price.Text = total.ToString();
        }

        private void dg_teh_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if(e.Control is ComboBox)
            {
                (e.Control as ComboBox).SelectedValueChanged -= cb_SelectedValueChanged;                             
                (e.Control as ComboBox).SelectedValueChanged += cb_SelectedValueChanged;
            }
        }
        private void cb_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                var ind = dg_teh.CurrentCellAddress;
                if (!(sender is ComboBox comboBox) || comboBox.SelectedValue == null)
                    return;
                var aa = (sender as ComboBox).SelectedValue.ToString();
                DataGridViewTextBoxCell cel = (DataGridViewTextBoxCell)dg_teh.Rows[ind.Y].Cells[0];
                cel.Value = aa.ToString();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            string querry = "INSERT into teh_card (id_dish,id_prod,weight) values ";
            for (int i = 0; i < dg_teh.RowCount - 1; i++)
            {
                querry += "(" +id_dish.ToString()+ ", " + dg_teh.Rows[i].Cells["id"].Value.ToString() + ", " + double.Parse(dg_teh.Rows[i].Cells["weight"].Value.ToString()) + "),";
            }//ошибка в формировании строки
            querry = querry.Remove(querry.Length - 1, 1);
            querry += ";";
            dB_Connect.openConnect();
            var cmd = new NpgsqlCommand(querry, dB_Connect.conn);
            NpgsqlDataReader ww = cmd.ExecuteReader();
            dB_Connect.closeConnect();
        }
    }
}
