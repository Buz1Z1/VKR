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
    public partial class Add_table : Form
    {
        DB_connect dB_Connect =new DB_connect();
        public Add_table()
        {
            InitializeComponent();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                var cmd = new NpgsqlCommand("UPDATE tables SET name=@name where id=@id", dB_Connect.conn);
                cmd.Parameters.AddWithValue("@name", name.Text);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt16(id.Text));
                dB_Connect.openConnect();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                dB_Connect.closeConnect();
                MessageBox.Show("Успех");
                name.Text = "";
                Tables tab = new Tables();
                if (MessageBox.Show("Успех", "Успех", MessageBoxButtons.OK) == DialogResult.OK)
                    tab.draw_table();
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
                var cmd = new NpgsqlCommand("INSERT into tables (name) values(@name)", dB_Connect.conn);
                cmd.Parameters.AddWithValue("@name", name.Text);
                dB_Connect.openConnect();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                dB_Connect.closeConnect();
                name.Text = "";
                Tables tab= new Tables();
                if (MessageBox.Show("Успех", "Успех", MessageBoxButtons.OK) == DialogResult.OK)
                    tab.draw_table();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
