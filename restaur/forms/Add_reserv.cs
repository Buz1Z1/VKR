using Guna.UI2.WinForms.Suite;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace restaur.forms
{
    public partial class Add_reserv : Form
    {
        public int id;
        DB_connect dB_Connect =new DB_connect();
        public Add_reserv()
        {
            InitializeComponent();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                var cmd = new NpgsqlCommand("UPDATE reserv SET id_table=@id_table, descr=@descr, time=@time, date=@date where id=@id", dB_Connect.conn);
                cmd.Parameters.AddWithValue("@id_table", Convert.ToInt16(tables.SelectedValue));
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@time", time.Text);
                cmd.Parameters.AddWithValue("@date", date.Value.ToShortDateString());
                cmd.Parameters.AddWithValue("@descr", descr.Text);
                dB_Connect.openConnect();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                dB_Connect.closeConnect();
                Tables tab = new Tables();
                if (MessageBox.Show("Успех", "Успех", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    tab.draw_reserv();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                var cmd = new NpgsqlCommand("INSERT into reserv (id_table,descr,time,date) values(@id_table,@descr,@time,@date)", dB_Connect.conn);
                cmd.Parameters.AddWithValue("@id_table", Convert.ToInt16(tables.SelectedValue));
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@time", time.Text);
                cmd.Parameters.AddWithValue("@date", date.Value.ToShortDateString());
                cmd.Parameters.AddWithValue("@descr", descr.Text);
                dB_Connect.openConnect();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                dB_Connect.closeConnect();
                Tables tab = new Tables();
                if (MessageBox.Show("Успех", "Успех", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    tab.draw_reserv();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
