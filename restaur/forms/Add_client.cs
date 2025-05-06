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
    public partial class Add_client : Form
    {
        input_check i_c = new input_check();
        public int id;
        DB_connect dB_Connect = new DB_connect();
        Client client= new Client();
        public Add_client()
        {
            InitializeComponent();
        }
        private void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                var cmd = new NpgsqlCommand("update client set fio=@fio,d_birth=@d_birth,email=@email,phone=@phone," +
                    "card_num=@card_num,bonus=@bonus,d_create=@d_create where id=@id", dB_Connect.conn);
                cmd.Parameters.AddWithValue("@fio", fio.Text);
                cmd.Parameters.AddWithValue("@email", email.Text);
                cmd.Parameters.AddWithValue("@phone", phone.Text);
                cmd.Parameters.AddWithValue("@bonus", Convert.ToInt32(bonus.Text));
                cmd.Parameters.AddWithValue("@card_num", card_num.Text);
                cmd.Parameters.AddWithValue("@d_birth", DateTime.Parse(date_birth.Text));
                cmd.Parameters.AddWithValue("@d_create", DateTime.Parse(d_create.Text));
                cmd.Parameters.AddWithValue("@id", id);
                dB_Connect.openConnect();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                dB_Connect.closeConnect();
                if (MessageBox.Show("Успех", "Успех", MessageBoxButtons.OK) == DialogResult.OK)
                    client.draw_table();
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
                var cmd = new NpgsqlCommand("INSERT into client (fio,d_birth,email,phone,card_num,bonus,d_create) values(@fio,@d_birth,@email,@phone,@card_num,@bonus,@d_create)", dB_Connect.conn);
                cmd.Parameters.AddWithValue("@fio", fio.Text);
                cmd.Parameters.AddWithValue("@email", email.Text);
                cmd.Parameters.AddWithValue("@phone",phone.Text);
                cmd.Parameters.AddWithValue("@bonus", Convert.ToInt32(bonus.Text));
                cmd.Parameters.AddWithValue("@card_num", card_num.Text);
                cmd.Parameters.AddWithValue("@d_birth", DateTime.Parse(date_birth.Text));
                cmd.Parameters.AddWithValue("@d_create", DateTime.Today);

                dB_Connect.openConnect();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                dB_Connect.closeConnect();
                if (MessageBox.Show("Успех", "Успех", MessageBoxButtons.OK) == DialogResult.OK)
                    client.draw_table();
                this.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void date_birth_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (i_c.isdigit(sender, e))
                e.Handled = true;
        }
    }
}
