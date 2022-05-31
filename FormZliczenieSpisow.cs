using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Inwentaryzacja_Funkcje_Dodatkowe
{
    public partial class FormZliczenieSpisow : Form
    {
        private Boolean ZmianaDanych;

        public FormZliczenieSpisow()
        {
            InitializeComponent();
            ZmianaDanych = false;
        }
        
        public static void DodajPozDoSpisu(string Symbol, String Nazwa, String Cena, String Ilosc, System.Windows.Forms.DataGridView dataGridView)
        {
            int newRowIndex = dataGridView.Rows.Add();
            dataGridView[0, newRowIndex].Value = Symbol;
            dataGridView[1, newRowIndex].Value = Nazwa;
            dataGridView[2, newRowIndex].Value = Ilosc;
            float cena_f = float.Parse(Cena);
            dataGridView[3, newRowIndex].Value = cena_f.ToString("F");
            float wartosc = (float.Parse(Ilosc)) * cena_f;
            dataGridView[4, newRowIndex].Value = wartosc.ToString("F");
            foreach (DataGridViewRow wiersz in dataGridView.SelectedRows)
                wiersz.Selected = false;
            dataGridView.Rows[newRowIndex].Selected = true;
            dataGridView.FirstDisplayedScrollingRowIndex = newRowIndex;
        }

        public static void DodajPozDoSpisu(string Symbol, String Nazwa, String Cena, String Ilosc, String Wartosc, System.Windows.Forms.DataGridView dataGridView)
        {
            int newRowIndex = dataGridView.Rows.Add();
            dataGridView[0, newRowIndex].Value = Symbol;
            dataGridView[1, newRowIndex].Value = Nazwa;
            dataGridView[2, newRowIndex].Value = Ilosc;
            float cena_f = float.Parse(Cena);
            dataGridView[3, newRowIndex].Value = cena_f.ToString("F");
            dataGridView[4, newRowIndex].Value = Wartosc;
            foreach (DataGridViewRow wiersz in dataGridView.SelectedRows)
                wiersz.Selected = false;
            dataGridView.Rows[newRowIndex].Selected = true;
            dataGridView.FirstDisplayedScrollingRowIndex = newRowIndex;
        }

        //--------------- Dodanie plików inwentaryzacji do spisu -------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr != DialogResult.OK)
                return;
            Cursor = Cursors.WaitCursor;
            foreach(string nazwaPliku in openFileDialog1.FileNames)
            {
                StreamReader reader = new StreamReader(nazwaPliku,Encoding.UTF8);
                if (!reader.EndOfStream)
                {
                    string[] wiersz;
                    do
                    {
                        wiersz = reader.ReadLine().Split('\t');
                        DodajPozDoSpisu(wiersz[0].Trim(), wiersz[1].Trim(), wiersz[2].Trim(), wiersz[3].Trim(), dataGridView1);
                    }
                    while (!reader.EndOfStream);
                    ZmianaDanych = true;
                }
                reader.Close();
            }
            label2.Text = dataGridView1.RowCount.ToString();
            float wartRazem = 0;
            foreach (DataGridViewRow wiersz in dataGridView1.Rows)
                wartRazem +=float.Parse(wiersz.Cells["Wartosc"].Value.ToString());
            label3.Text = wartRazem.ToString("F");
            Cursor = Cursors.Default;
        }
  //------------- Zliczenie Spisów Inwentaryzacyjnych -----------------------------------
        private void button2_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            Boolean czyZliczono = false;
            for (int x=0;x<dataGridView1.RowCount-1;x++)
            {
                for(int i=x+1;i<dataGridView1.RowCount;i++)
                {
                    if(dataGridView1["Symbol",x].Value.Equals(dataGridView1["Symbol", i].Value))
                    {
                        int ilx = int.Parse(dataGridView1["Ilosc", x].Value.ToString());
                        int ili = int.Parse(dataGridView1["Ilosc", i].Value.ToString());
                        ilx += ili;
                        float wartoscx = ilx * float.Parse(dataGridView1["Cena", x].Value.ToString());
                        dataGridView1["Ilosc", x].Value = ilx.ToString();
                        dataGridView1["Wartosc", x].Value = wartoscx.ToString("F");
                        dataGridView1.Rows.RemoveAt(i);
                        i--;
                        czyZliczono = true;
                    }
                }
            }
            if (czyZliczono)
            {
                label2.Text = dataGridView1.RowCount.ToString();
                float wartRazem = 0;
                foreach (DataGridViewRow wiersz in dataGridView1.Rows)
                    wartRazem += float.Parse(wiersz.Cells["Wartosc"].Value.ToString());
                label3.Text = wartRazem.ToString("F");
                foreach (DataGridViewRow wiersz in dataGridView1.SelectedRows)
                    wiersz.Selected = false;
                dataGridView1.Rows[0].Selected = true;
                ZmianaDanych = true;
                Cursor = Cursors.Default;
                MessageBox.Show("Spis Inwentaryzacyjny został zliczony", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Brak pozycji do zliczenia", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

//--------- Zapisz Spis Inwentaryzacyjny do pliku ----------------------------------
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult saveFileDialog1_dr = saveFileDialog1.ShowDialog();
            if (saveFileDialog1_dr == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                StreamWriter writer = new StreamWriter(saveFileDialog1.OpenFile(),Encoding.UTF8);
                foreach (DataGridViewRow wiersz in dataGridView1.Rows)
                {
                    DataGridViewCellCollection rowCells = wiersz.Cells;
                    writer.WriteLine(rowCells["Symbol"].Value.ToString() + '\t'
                                    + rowCells["Nazwa"].Value.ToString() + '\t'
                                    + rowCells["Ilosc"].Value.ToString() + '\t'
                                    + rowCells["Cena"].Value.ToString() + '\t'
                                    + rowCells["Wartosc"].Value.ToString());
                }
                writer.Close();
                ZmianaDanych = false;
                Cursor = Cursors.Default;
            }
        }

//------------ Otwarcie zapisanego Spisu Inwentaryzacyjnego --------------------------------
        private void button4_Click(object sender, EventArgs e)
        {
            if (ZmianaDanych)
            {
                DialogResult x = MessageBox.Show("Spis Inwentaryzacyjny zawiera niezapisane dane. Otwarcie pliku z Innym Spisem Inwentaryzacyjnym spowoduje usunięcie istniejącego Spisu. " +
                                  "Czy nadal chcesz otworzyć plik ze Spisem Inwentaryzacyjnym ?",
                                  "Spis Inwentaryzacyjny zawiera dane", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (x != DialogResult.Yes)
                    return;
            }
            DialogResult dr = openFileDialog2.ShowDialog();
            if (dr != DialogResult.OK)
                return;
            StreamReader reader = new StreamReader(openFileDialog2.OpenFile(), Encoding.UTF8);
            if(!reader.EndOfStream)
            {
                Cursor = Cursors.WaitCursor;
                dataGridView1.Rows.Clear();
                String[] wierszP;
                do
                {
                    wierszP = reader.ReadLine().Split('\t');
                    DodajPozDoSpisu(wierszP[0].Trim(), wierszP[1].Trim(), wierszP[3].Trim(), wierszP[2].Trim(), wierszP[4].Trim(), dataGridView1);
                }
                while (!reader.EndOfStream);
                label2.Text = dataGridView1.RowCount.ToString();
                float wartRazem = 0;
                foreach (DataGridViewRow wiersz in dataGridView1.Rows)
                    wartRazem += float.Parse(wiersz.Cells["Wartosc"].Value.ToString());
                label3.Text = wartRazem.ToString("F");
                ZmianaDanych = false;
            }
            reader.Close();
            Cursor = Cursors.Default;
        }

 //-------------- Czyszczenie Spisu ------------------------------------------
        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                if (ZmianaDanych)
                {
                    DialogResult dr = MessageBox.Show("Spis Inwentaryzacyjny zawiera niezapisane dane.\r\nCzy napewno chcesz wyczyścić całą zawartość Spisu Inwentaryzacyjnego ?", "Ostrzeżenie !"
                    , MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (dr != DialogResult.Yes)
                        return;
                }
                dataGridView1.Rows.Clear();
                label2.Text = "0";
                label3.Text = "0,00";
                ZmianaDanych = false;
            }
        }

    //----------- Edycja pozycji -----------------------------------------------
        private void button6_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count>0)
            {
                if(dataGridView1.SelectedRows.Count!=1)
                {
                    MessageBox.Show("Wybierz tylko jedną pozycję do edycji", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                DataGridViewCellCollection cells = dataGridView1.SelectedRows[0].Cells;
                Form_Edycja_Pozycji formEdycjaPozycji = new Form_Edycja_Pozycji(cells["Symbol"].Value.ToString(), cells["Nazwa"].Value.ToString(), cells["Ilosc"].Value.ToString(), cells["Cena"].Value.ToString());
                formEdycjaPozycji.ShowDialog();
                if(formEdycjaPozycji.ExitCode==1)
                {
                    int ilosc = formEdycjaPozycji.Ilosc;
                    float cena = formEdycjaPozycji.Cena;
                    float wartosc = ilosc * cena;
                    cells["Nazwa"].Value = formEdycjaPozycji.Nazwa;
                    cells["Ilosc"].Value = ilosc.ToString();
                    cells["Cena"].Value = cena.ToString("F");
                    cells["Wartosc"].Value = wartosc.ToString("F");
                    float wartRazem = 0;
                    foreach (DataGridViewRow wiersz in dataGridView1.Rows)
                        wartRazem += float.Parse(wiersz.Cells["Wartosc"].Value.ToString());
                    label3.Text = wartRazem.ToString("F");
                    ZmianaDanych = true;
                }
            }
        }

  //----------- Dodaj pozycję -------------------------------------------
        private void button7_Click(object sender, EventArgs e)
        {
            FormNowaPozycja formNowaPozycja = new FormNowaPozycja();
            Boolean repeat;
            String symbol;
            int ilosc = 0;
            float cena = 0;
            float wartosc = 0;
            do
            {
                formNowaPozycja.ShowDialog();
                if (formNowaPozycja.ExitCode != 1)
                    return;
                Cursor = Cursors.WaitCursor;
                symbol = formNowaPozycja.Symbol;
                ilosc = formNowaPozycja.Ilosc;
                cena = formNowaPozycja.Cena;
                wartosc = ilosc * cena;
                repeat = false;
                foreach (DataGridViewRow wiersz in dataGridView1.Rows)
                {
                    if (wiersz.Cells["Symbol"].Value.ToString() == symbol)
                    {
                        foreach (DataGridViewRow _wiersz in dataGridView1.SelectedRows)
                            _wiersz.Selected = false;
                        wiersz.Selected = true;
                        dataGridView1.FirstDisplayedScrollingRowIndex = wiersz.Index;
                        Cursor = Cursors.Default;
                        MessageBox.Show("Pozycja o Symbolu: " + symbol + " już istnieje!\r\nZmień Symbol na inny.", "Symbol już istnieje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        repeat = true;
                        break;
                    }
                }
            }
            while (repeat);
            DodajPozDoSpisu(symbol, formNowaPozycja.Nazwa, cena.ToString("F"), ilosc.ToString(), wartosc.ToString("F"), dataGridView1);
            label2.Text = dataGridView1.RowCount.ToString();
            float wartRazem = 0;
            foreach (DataGridViewRow wiersz in dataGridView1.Rows)
                wartRazem += float.Parse(wiersz.Cells["Wartosc"].Value.ToString());
            label3.Text = wartRazem.ToString("F");
            ZmianaDanych = true;
            Cursor = Cursors.Default;
        }

  //--------- Usuwanie pozycji spisu ---------------------------------
        private void button8_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count<1)
            {
                MessageBox.Show("Brak pozycji do usunięcia!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if(dataGridView1.SelectedRows.Count==1)
            {
                DialogResult dr = MessageBox.Show("Czy napewno chcesz usunąć pozycję: " + dataGridView1.SelectedRows[0].Cells["Symbol"].Value.ToString()
                                 + " " + dataGridView1.SelectedRows[0].Cells["Nazwa"].Value.ToString() + " ?", "Pytanie ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dr != DialogResult.Yes)
                    return;
            }
            else
            {
                DialogResult dr = MessageBox.Show("Czy napewno chcesz usunąć "+dataGridView1.SelectedRows.Count.ToString()+" pozycji ?", "Pytanie ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dr != DialogResult.Yes)
                    return;
            }
            foreach (DataGridViewRow wiersz in dataGridView1.SelectedRows)
                dataGridView1.Rows.Remove(wiersz);
            label2.Text = dataGridView1.RowCount.ToString();
            float wartRazem = 0;
            foreach (DataGridViewRow wiersz in dataGridView1.Rows)
                wartRazem += float.Parse(wiersz.Cells["Wartosc"].Value.ToString());
            label3.Text = wartRazem.ToString("F");
            ZmianaDanych = true;
        }

        private void FormZliczenieSpisow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ZmianaDanych)
            {
                DialogResult dr = MessageBox.Show("Istnieją niezapisane dane.\r\nCzy zapisać Spis Inwentaryzacyjny ?", "Niezapisane dane", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (dr == DialogResult.Yes)
                {
                    button3_Click(this, EventArgs.Empty);
                    if (ZmianaDanych)
                        e.Cancel = true;
                }
                if (dr == DialogResult.Cancel)
                    e.Cancel = true;
            }
        }

    //------- Export do pliku CSV ------------------------------------------
        private void button9_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount<1)
            {
                MessageBox.Show("Brak danych do exportu!", "Brak danych", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            DialogResult dr = saveFileDialog2.ShowDialog();
            if (dr != DialogResult.OK)
                return;
            Cursor = Cursors.WaitCursor;
            StreamWriter writer = new StreamWriter(saveFileDialog2.OpenFile());
            foreach (DataGridViewRow wiersz in dataGridView1.Rows)
            {
                string wierszP="";
                foreach (DataGridViewCell cell in wiersz.Cells)
                {
                    string wartosc = cell.Value.ToString();
                    if (wartosc.Contains(','))
                        wartosc='"'+wartosc+'"';
                    wierszP += wartosc+',';
                }
                wierszP = wierszP.TrimEnd(',');
                writer.WriteLine(wierszP);
            }
            writer.Close();
            Cursor = Cursors.Default;
        }
    }
}
