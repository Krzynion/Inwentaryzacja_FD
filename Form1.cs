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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void zamknij_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormZliczenieSpisow formZlSpisow = new FormZliczenieSpisow();
            Hide();
            formZlSpisow.ShowDialog();
            Show();
        }

    //----------- Asortyment --------------------------------------
        private void button2_Click(object sender, EventArgs e)
        {
            FormAsortyment formAsortyment = new FormAsortyment();
            Hide();
            formAsortyment.ShowDialog();
            Show();
        }
    }
}
