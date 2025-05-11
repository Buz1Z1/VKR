using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
namespace restaur
{
    //Класс для подключения к базе данных
    internal class DB_connect
    {
        internal NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;Database=test;User Id=postgres;Password=step;");
        internal NpgsqlConnection getConnect
        {
            get { return conn; }
        }
        internal void openConnect()
        {
            if (conn.State==System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
        }
        internal void closeConnect()
        {
            if(conn.State==System.Data.ConnectionState.Open)
                conn.Close();
        }

        internal void LoadDG(DataGridView dg, ListBox lb,string query)
        {
            try
            {
                openConnect();
                var cmd= new NpgsqlCommand(query,conn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                for(int i=0;i<lb.Items.Count;i++)
                {
                    string column = ((DataGridViewColumn)lb.Items[i]).Name;
                    dg.Columns[column].DataPropertyName = dt.Columns[i].ToString();
                }
                dg.DataSource = dt;
                cmd.Dispose();
                closeConnect();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                
            }
        }
    }
}
