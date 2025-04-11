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

namespace restaur
{
    public partial class Orders : Form
    {
        DB_connect dB_Connect = new DB_connect();
        public Orders()
        {
            InitializeComponent();
            fill_data();
        }
        public void fill_data()
        {
            guna2DataGridView1.Rows.Clear();
            dB_Connect.openConnect();
            var cmd = new NpgsqlCommand("SELECT * FROM orders", dB_Connect.conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            //DataTable dt = new DataTable();
            //dt.Load(reader);
            //guna2DataGridView1.DataSource = dt;
            //Получение ответа от бд
            while (reader.Read())
            {

                guna2DataGridView1.Rows.Add(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString());
            }
            cmd.Dispose();
            dB_Connect.closeConnect();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
