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
    public partial class FormNowaPozycja : Form
    {
        private String symbol;
        private String nazwa;
        private int ilosc = 0;
        private float cena = 0;
        private int exitCode = 0;

        public FormNowaPozycja()
        {
            InitializeComponent();
        }

        public string Symbol
        {
            get
            {
                return symbol;
            }
        }
        public string Nazwa
        {
            get
            {
                return nazwa;
            }
        }
        public int Ilosc
        {
            get
            {
                return ilosc;
            }
        }
        public float Cena
        {
            get
            {
                return cena;
            }
        }
        public int ExitCode
        {
            get
            {
                return exitCode;
            }
        }

   //---------- Anuluj ---------------------------------
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormNowaPozycja_FormClosed(object sender, FormClosedEventArgs e)
        {
            exitCode = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            exitCode = 1;
            symbol = textBox1.Text.Trim();
            if (symbol.Length < 3)
                exitCode = -6;
            nazwa = textBox2.Text.Trim();
            if (nazwa.Length < 3)
                exitCode = -1;
            try
            {
                ilosc = int.Parse(textBox3.Text);
                if (ilosc < 1)
                    exitCode = -3;
            }
            catch
            {
                exitCode = -2;
            }
            try
            {
                cena = float.Parse(textBox4.Text);
                if (cena < 0)
                    exitCode = -5;
            }
            catch
            {
                exitCode = -4;
            }
            switch (exitCode)
            {
                case -1:
                    MessageBox.Show("Pole Nazwa musi zawierać conajmniej 3 znaki.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox2.Focus();
                    return;
                case -2:
                    MessageBox.Show("Pole Ilość musi zawierć liczbę całkowitą.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox3.Focus();
                    return;
                case -3:
                    MessageBox.Show("Liczba w polu Ilość musi być wieksza od 0.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox3.Focus();
                    return;
                case -4:
                    MessageBox.Show("Pole Cena musi zawierać liczbę.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox4.Focus();
                    return;
                case -5:
                    MessageBox.Show("Liczba w polu Cena nie może być ujemna.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox4.Focus();
                    return;
                case -6:
                    MessageBox.Show("Pole Symbol musi zawierać conajmniej 3 znaki.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Focus();
                    return;
            }
            Hide();
        }
    }
}
