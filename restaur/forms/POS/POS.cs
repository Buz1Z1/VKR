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

namespace restaur.forms.POS
{
    public partial class POS : Form
        
    {
        DB_connect dB_Connect = new DB_connect();

        int id_client=0;
        
        public POS()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void loadCategory()
        {
            dB_Connect.openConnect();
            var cmd = new NpgsqlCommand("Select * from dish_category", dB_Connect.conn);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            da.Fill(dt);
            category_panel.Controls.Clear();
            
            
            if (dt.Rows.Count > 0)
            {
                Guna.UI2.WinForms.Guna2Button all = new Guna.UI2.WinForms.Guna2Button();
                all.FillColor = Color.FromArgb(55, 99, 101);
                all.Size = new Size(160, 45);
                all.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                all.Text = "Все";
                all.Click += new EventHandler(all_cat_Click);
                all.Margin = new Padding(2, 5, 2, 5);
                category_panel.Controls.Add(all);
                foreach (DataRow dr in dt.Rows)
                {
                    Guna.UI2.WinForms.Guna2Button button = new Guna.UI2.WinForms.Guna2Button();
                    button.FillColor = Color.FromArgb(55, 99, 101);
                    button.Size = new Size(160, 45);
                    button.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                    button.Text = dr["name"].ToString();
                    button.Click += new EventHandler(choose);
                    button.Margin=new Padding(2,5,2,5);
                    category_panel.Controls.Add(button);

                }
            }
            dB_Connect.closeConnect();
            cmd.Dispose();
        }

        private void choose(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button button = (Guna.UI2.WinForms.Guna2Button)sender;
            foreach (var item in products.Controls)
            {
                var prod = (product)item;
                prod.Visible = prod.Pcategory.ToLower().Contains(button.Text.Trim().ToLower());
            }
        }

        private void all_cat_Click(object sender, EventArgs e)
        {
            foreach (var item in products.Controls)
            {
                var prod = (product)item;
                prod.Visible = true;
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void loadItems(string id,string name, string price, string category, Image image)
        {
            var item = new product()
            {
                Pid = Convert.ToInt32(id),
                Name = name,
                Price = price,
                Pcategory = category,
                Pimage = image
            };
            products.Controls.Add(item);
            item.selected += (ss, ee) =>
            {
                var wdg = (product)ss;
                foreach(DataGridViewRow o in dg_order.Rows)
                {
                    if(Convert.ToInt16(o.Cells[0].Value) == wdg.Pid)
                    {
                        o.Cells["quantity"].Value = int.Parse(o.Cells["quantity"].Value.ToString())+1;
                        o.Cells["amount"].Value = int.Parse(o.Cells["quantity"].Value.ToString()) * double.Parse(o.Cells["price"].Value.ToString());
                        Total();
                        return;
                    }
                    
                }
                dg_order.Rows.Add(new object[] { wdg.Pid, wdg.Name, 1, wdg.Price, wdg.Price });
                Total();
            };
        }


        private void loadProducts()
        {
            dB_Connect.openConnect();
            var cmd = new NpgsqlCommand("select * from dishes as d left join dish_category as c on d.category_id=c.id where d.name like '%" + search.Text + "%'", dB_Connect.conn);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach( DataRow dr in dt.Rows)
            {
                loadItems(dr[0].ToString(), dr[1].ToString(), dr[3].ToString(),dr[7].ToString(),
                Bitmap.FromFile(dr[4].ToString()));
            }
            cmd.Dispose();
            dB_Connect.closeConnect();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
            Main_menu mm= new Main_menu();
            mm.Show();
        }

        private void POS_load(object sender, EventArgs e)
        {
            products.Controls.Clear();
            loadProducts();
            loadCategory();
            loadTabWaiter();
        }

        private void loadTabWaiter()
        {
            var cmd = new NpgsqlCommand("Select * from tables", dB_Connect.conn);
            dB_Connect.openConnect();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmd.Dispose();
            
            choose_table.DisplayMember = "name";
            choose_table.ValueMember = "id";
            choose_table.SelectedIndex = -1;
            choose_table.DataSource = dt;

            cmd = new NpgsqlCommand("select id, fio from employee where job='Официант'", dB_Connect.conn);
            da = new NpgsqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            cmd.Dispose();
            dB_Connect.closeConnect();
            choose_waiter.DisplayMember = "fio";
            choose_waiter.ValueMember = "id";
            choose_waiter.SelectedIndex = -1;
            choose_waiter.DataSource = dt;
        }

        private void search_TextChanged(object sender, EventArgs e)
        {
            products.Controls.Clear();
            loadProducts();
        }

        private void Total()
        {
            double total = 0;
            foreach(DataGridViewRow dg in dg_order.Rows)
            {
                total += double.Parse(dg.Cells["amount"].Value.ToString());
            }
            Total_sum.Text = total.ToString("N2");
        }

        private void dg_order_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = dg_order.Columns[e.ColumnIndex].Name;
            if (colname == "delete")
            {
                dg_order.Rows.Remove(dg_order.Rows[e.RowIndex]);
                Total();
            }
            
        }
        private void clear()
        {
            t_ofic.Text = "";
            t_table.Text = "";
            dg_order.Rows.Clear();
            choose_table.SelectedIndex = -1;
            choose_waiter.SelectedIndex = -1;
            Total_sum.Text = "0.0";
            id_client = 0;
            label8.Text = "";
            label9.Text = "";
            guna2TextBox1.Clear();
        }
        private void new_order_Click(object sender, EventArgs e)
        {
            clear();            
        }

        private void Create_order_Click(object sender, EventArgs e)
        {
            try
            {
                //заполнение заказа
                var cmd = new NpgsqlCommand("INSERT into orders (date,id_client,id_table,sum,id_emp,time) values (@date,@id_client,@id_table,@sum,@id_emp,@time) RETURNING id;", dB_Connect.conn);
                cmd.Parameters.AddWithValue("@date", DateTime.Now.ToShortDateString());
                cmd.Parameters.AddWithValue("@time", DateTime.Now.ToShortTimeString());
                cmd.Parameters.AddWithValue("@id_client", id_client);
                cmd.Parameters.AddWithValue("@sum", Convert.ToDouble(Total_sum.Text));
                if(Convert.ToInt16(choose_table.SelectedValue)==-1)
                    cmd.Parameters.AddWithValue("@id_table", 0);
                else
                    cmd.Parameters.AddWithValue("@id_table", Convert.ToInt16(choose_table.SelectedValue));

                if (Convert.ToInt16(choose_waiter.SelectedValue) == -1)
                    cmd.Parameters.AddWithValue("@id_emp", 0);
                else
                    cmd.Parameters.AddWithValue("@id_emp", Convert.ToInt16(choose_waiter.SelectedValue));
                dB_Connect.openConnect();
                int reader = (int)cmd.ExecuteScalar();

                //заполнение деталей заказа
                cmd.Parameters.Clear();
                string querry = "INSERT into order_detail (id_order,id_dish,count) values ";
                foreach (DataGridViewRow i in dg_order.Rows)
                {
                    querry += "(" + reader.ToString() + ", " + i.Cells["id"].Value.ToString() + ", " + i.Cells["quantity"].Value.ToString() + "),";
                }
                querry = querry.Remove(querry.Length - 1, 1);
                querry += ";";
                cmd = new NpgsqlCommand(querry, dB_Connect.conn);
                NpgsqlDataReader ww = cmd.ExecuteReader();
                dB_Connect.closeConnect();
                cmd.Dispose();
                if (id_client != 0)
                {
                    dB_Connect.openConnect();
                    double plusbonus = Convert.ToDouble(Total_sum.Text) * 0.05;
                    cmd = new NpgsqlCommand("update client set bonus=bonus+" + plusbonus.ToString(), dB_Connect.conn);
                    NpgsqlDataReader ss = cmd.ExecuteReader();
                    cmd.Dispose();
                    dB_Connect.closeConnect();
                }
                clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void create_client_Click(object sender, EventArgs e)
        {
            Add_client add_Client = new Add_client();
            add_Client.Show();
            add_Client.label1.Text = "Создать клиента";
            add_Client.btn_save.Enabled = true;
            add_Client.btn_save.Visible = true;
            add_Client.d_create.Text = DateTime.Now.ToShortDateString();
            add_Client.btn_update.Enabled = false;
            add_Client.btn_update.Visible = false;
        }

        private void choose_clint_Click(object sender, EventArgs e)
        {
            dB_Connect.openConnect();
            var cmd = new NpgsqlCommand("select id,fio,bonus from client where card_num='"+guna2TextBox1.Text+"';", dB_Connect.conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                label8.Text = reader["fio"].ToString();
                label9.Text = reader["bonus"].ToString();
                id_client = Convert.ToInt16(reader["id"]);
            }
            dB_Connect.closeConnect();
            reader.DisposeAsync();
            cmd.Dispose();
            
        }
    }
}
