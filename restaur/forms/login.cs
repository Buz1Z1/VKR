using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
namespace restaur
{
    public partial class login : Form
    {
        DB_connect db = new DB_connect();
        public login()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            //Авторизация

            //Подключение к бд
            db.openConnect();

            //Получение ввода пользователя
            string login =guna2TextBox2.Text;
            string password=guna2TextBox1.Text;

            //Формирование строки sql команды
            var cmd = new NpgsqlCommand("SELECT * FROM users WHERE login = @login and pass = @pass", db.conn);
            cmd.Parameters.AddWithValue("login", login);
            cmd.Parameters.AddWithValue("pass", password);
            //Исполнение команды
            NpgsqlDataReader reader = cmd.ExecuteReader();

            //Получение ответа от бд
            DataTable dt = new DataTable();
            dt.Load(reader);
            
            //Проверка
            if (dt.Rows.Count > 0)
            {
                this.Hide();
                Main_menu main = new Main_menu();
                main.Show();
            }
            else
                MessageBox.Show("Неверный логин или пароль");
            cmd.Dispose();
            db.closeConnect();
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
            //this.Close();
        }
    }
}
