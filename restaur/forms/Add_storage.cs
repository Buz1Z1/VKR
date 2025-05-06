using Guna.UI2.WinForms.Suite;
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
    public partial class Add_storage : Form
    {
        input_check ic;
        public int id;
        DB_connect dB_Connect = new DB_connect();
        public Add_storage()
        {
            InitializeComponent();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                var cmd = new NpgsqlCommand("update storage set name=@name,min_count=@min_count,count=@count,price=@price where id=@id", dB_Connect.conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", name.Text);
                cmd.Parameters.AddWithValue("@min_count", double.Parse(min_count.Text));
                cmd.Parameters.AddWithValue("@count", double.Parse(count.Text));
                cmd.Parameters.AddWithValue("@price", double.Parse(price.Text));
                cmd.Parameters.AddWithValue("@date", DateTime.UtcNow.ToShortDateString());
                dB_Connect.openConnect();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                dB_Connect.closeConnect();
                Storage storage = new Storage();
                if (MessageBox.Show("Успех", "Успех", MessageBoxButtons.OK) == DialogResult.OK)
                    storage.draw_table();
                this.Close();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                var cmd = new NpgsqlCommand("insert into storage (name,min_count,count,price,date) values(@name,@min_count,@count,@price,@date) ", dB_Connect.conn);
                
                cmd.Parameters.AddWithValue("@name", name.Text);
                cmd.Parameters.AddWithValue("@min_count", double.Parse(min_count.Text));
                cmd.Parameters.AddWithValue("@count", double.Parse(count.Text));
                cmd.Parameters.AddWithValue("@price", double.Parse(price.Text));
                cmd.Parameters.AddWithValue("@date", DateTime.UtcNow.ToShortDateString());
                dB_Connect.openConnect();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                dB_Connect.closeConnect();

                Storage storage = new Storage();
                if (MessageBox.Show("Успех", "Успех", MessageBoxButtons.OK) == DialogResult.OK)
                    storage.draw_table();
                this.Close();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }


        private void count_KeyPress(object sender, KeyPressEventArgs e) //проверка на число
        {
            if (ic.isdigit(sender, e))
                e.Handled=true;
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
