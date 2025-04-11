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
    public partial class product : UserControl
    {
        public product()
        {
            InitializeComponent();
        }
        public event EventHandler selected = null;
        public int Pid { get; set; }
        public string Pcategory { get; set; }
        public string Price 
        {   
            get { return Pprice.Text; }
            set { Pprice.Text = value;} 
        }
        public string Name
        {
            get { return Pname.Text; }
            set { Pname.Text = value; }
        }
        public Image Pimage
        {
            get { return image.Image; }
            set { image.Image = value; }
        }

        private void image_Click(object sender, EventArgs e)
        {
            selected?.Invoke(this, e);
        }
    }
}
