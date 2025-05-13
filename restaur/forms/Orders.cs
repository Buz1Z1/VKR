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
            dg_orders.Rows.Clear();
            dB_Connect.openConnect();
            var cmd = new NpgsqlCommand("select o.date,o.time, c.fio, t.name,o.sum,e.fio from orders o " +
                "left join client c on c.id=o.id_client " +
                "left join tables t on t.id=o.id_table " +
                "left join employee e on e.id=o.id_emp order by CAST(o.date as DATE) DESC", dB_Connect.conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {

                dg_orders.Rows.Add(reader[0].ToString(), reader[1].ToString(),
                    reader[2].ToString(), reader[3].ToString(),
                    reader[4].ToString(), reader[5].ToString());
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
