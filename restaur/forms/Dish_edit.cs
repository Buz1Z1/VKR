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
    public partial class Dish_edit : Form
    {
        DB_connect dB_Connect = new DB_connect();
        public string image_path;
        public int id;
        public Dish_edit()
        {
            InitializeComponent();
        }

        private void clear_form()
        {
            name.Clear();
            
            descryption.Clear();
            price.Clear();
            image.ImageLocation="";
            
        }
     
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Image Files (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    image.ImageLocation = dialog.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void image_Click(object sender, EventArgs e)
        {

        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                var cmd = new NpgsqlCommand("UPDATE dishes SET name=@name,category_id=@category_id,descryption=@descryption,price=@price,image=@image where id=@id", dB_Connect.conn);
                cmd.Parameters.AddWithValue("@name", name.Text);
                cmd.Parameters.AddWithValue("@descryption", descryption.Text);
                cmd.Parameters.AddWithValue("@category_id", Convert.ToInt16(category.SelectedValue));
                cmd.Parameters.AddWithValue("@image", image_path);
                cmd.Parameters.AddWithValue("@price", Convert.ToDouble(price.Text));
                cmd.Parameters.AddWithValue("@id", id);
                dB_Connect.openConnect();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                dB_Connect.closeConnect();
                clear_form();
                Menu menu = new Menu();
                if (MessageBox.Show("Успех", "Успех", MessageBoxButtons.OK) == DialogResult.OK)
                    menu.draw_table();
                this.Close();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
            
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                var cmd = new NpgsqlCommand("INSERT into dishes (name,descryption,category_id,price,image) values(@name,@descryption,@category_id,@price,@image)", dB_Connect.conn);
                cmd.Parameters.AddWithValue("@name", name.Text);
                cmd.Parameters.AddWithValue("@descryption", descryption.Text);
                cmd.Parameters.AddWithValue("@category_id",Convert.ToInt16(category.SelectedValue));
                cmd.Parameters.AddWithValue("@image", image.ImageLocation.ToString());
                cmd.Parameters.AddWithValue("@price", Convert.ToDouble(price.Text));
                dB_Connect.openConnect();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                dB_Connect.closeConnect();
                clear_form();
                Menu menu = new Menu();
                if (MessageBox.Show("Успех", "Успех", MessageBoxButtons.OK) == DialogResult.OK)
                    menu.draw_table();
                this.Close();
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
