using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace restaur.forms.POS
{
    public partial class choose_table : Form
    {
        DB_connect dB_Connect=new DB_connect();
        public string Table_name;
        public choose_table()
        {
            InitializeComponent();
            fill();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Table_name = "";
            this.Close();
        }
        
        private void fill()
        {
            var cmd = new NpgsqlCommand("select * from tables", dB_Connect.conn);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                Guna.UI2.WinForms.Guna2Button button = new Guna.UI2.WinForms.Guna2Button();
                button.FillColor = Color.FromArgb(55, 99, 101);
                button.Size = new Size(160, 45);
                //button.Location=
                button.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                button.Text = dr["name"].ToString();
                //button.Dock = DockStyle.Top;
                button.Click += new EventHandler(choose);
                button.Margin = new Padding(2, 5, 2, 5);
                panel1.Controls.Add(button);
            }
        }

        private void choose(object sender, EventArgs e)
        {
            Table_name=(sender as Guna.UI2.WinForms.Guna2Button).Text.ToString();
            this.Close();
        }
    }
}
