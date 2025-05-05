using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace restaur
{
    internal class input_check
    {
        public bool isdigit(object sender, KeyPressEventArgs e) //проверка на число
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',' && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
                return true;
            else
                return false;
        }
    }
}
