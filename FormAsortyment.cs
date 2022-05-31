using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inwentaryzacja_Funkcje_Dodatkowe
{
    public partial class FormAsortyment : Form
    {
        public FormAsortyment()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormAsortymentPobierz formAsPob = new FormAsortymentPobierz();
            formAsPob.ShowDialog();

        }
    }
}
