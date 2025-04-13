using Guna.Charts.WinForms;
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
    public partial class Reports : Form
    {
        DB_connect dB_Connect = new DB_connect();
        public Reports()
        {
            InitializeComponent();
        }
        private void cnt_orders()
        {
            var dataset = new GunaLineDataset();
            for (int i = 0; i < 24; i++)
            {
                dataset.DataPoints.Add()
            }
            dB_Connect.openConnect();
        }
    }

    
}
