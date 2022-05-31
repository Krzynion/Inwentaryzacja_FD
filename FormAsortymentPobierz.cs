using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Inwentaryzacja_Funkcje_Dodatkowe
{
    public partial class FormAsortymentPobierz : Form
    {
        private SqlConnection connection;
        public FormAsortymentPobierz()
        {
            InitializeComponent();
           
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Text = "(Wybierz Bazę Danych)";
            comboBox1.Enabled = false;
            String nazwaSerwera = comboBox2.Text.Trim();
            if (nazwaSerwera.Length == 0)
            {
                MessageBox.Show("Nazwa Serwera nie może być pusta!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                comboBox2.Focus();
                return;
            }
            String connectionString = "";
            if (radioButton1.Checked)
                connectionString = "Data Source=" + nazwaSerwera + ";Initial Catalog=master;Integrated Security=True";
            else
            {
                String userID = textBox2.Text.Trim();
                if (userID.Length == 0)
                {
                    MessageBox.Show("Pole Nazwa Użytkownika nie może być puste!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    textBox2.Focus();
                    return;
                }
                String haslo = textBox3.Text.Trim();
                if (haslo.Length == 0)
                {
                    MessageBox.Show("Pole Hasło nie może być puste!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    textBox3.Focus();
                    return;
                }
                connectionString = "Data Source=" + nazwaSerwera + ";Initial Catalog=master;User ID=" + userID + ";Password=" + haslo;
                //Data Source=ASUSPC\SQLEXPRESS;Initial Catalog=master;User ID=user1;Password=***********
            }
            Cursor = Cursors.WaitCursor;
            Enabled = false;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand polecenieSql = new SqlCommand("SELECT name FROM master.sys.databases WHERE owner_sid<>0x01", connection);
                SqlDataReader reader = polecenieSql.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                        comboBox1.Items.Add(reader.GetString(0));
                    reader.Close();
                }
                else
                {
                    reader.Close();
                    throw new Exception("Brak informacji o dostępnych Bazach Danych.");
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Błąd połączenia z serwerem: \"" + connection.DataSource + "\" !\r\n" + ex.Message, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                connection.Close();
                Enabled = true;
                return;
            }
            Cursor = Cursors.Default;
            MessageBox.Show("Połączono z serwerem: \"" + connection.DataSource + "\".","Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
            comboBox1.Enabled = true;
            Enabled = true;
            comboBox1.Focus();
        }

       
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                textBox2.Enabled = false;
                textBox3.Enabled = false;
            }
            else
            {
                textBox2.Enabled = true;
                textBox3.Enabled = true;
            }
        }

        private void FormAsortymentPobierz_Shown(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            Enabled = false;
            DataTable dt = Microsoft.SqlServer.Management.Smo.SmoApplication.EnumAvailableSqlServers();
            foreach(DataRow wiersz in dt.Rows)
                comboBox2.Items.Add(wiersz["Name"]);
            if (comboBox2.Items.Count > 0)
                comboBox2.Text = comboBox2.Items[0].ToString();
            Enabled = true;        
            Cursor = Cursors.Default;
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            comboBox3.Text = "(Wybierz Magazyn)";
            comboBox3.Items.Clear();
            comboBox3.Enabled = false;
            foreach (Object elListy in comboBox1.Items)
                if (comboBox1.Text == elListy.ToString())
                {
                    comboBox3.Enabled = true;
                    break;
                }
        }

        private void comboBox3_Enter(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if(comboBox3.Items.Count==0)
            {
                try
                {
                    SqlCommand polecenieSql = new SqlCommand("SELECT Nazwa FROM "+comboBox1.Text+".ModelDanychContainer.Magazyny", connection);
                    SqlDataReader reader = polecenieSql.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                            comboBox3.Items.Add(reader.GetString(0));
                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                        throw new Exception("Brak informacji o dostępnych Magazynach.");
                    }
                }
                catch (Exception ex)
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show("Baza Danych \""+comboBox1.Text+"\" nie zawiera informacji o dostępnych magazynach!\r\n" + ex.Message, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    comboBox3.Enabled = false;
                    comboBox1.Focus();
                    return;
                }
            }
            Cursor = Cursors.Default;
        }

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Enabled = false;
            button2.Enabled = false;
            foreach (Object elListy in comboBox3.Items)
                if (comboBox3.Text == elListy.ToString())
                {
                    dateTimePicker1.Enabled = true;
                    button2.Enabled = true;
                    break;
                }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string baza = comboBox1.Text;
            string magazyn = comboBox3.Text;
            string data = dateTimePicker1.Text;
        }
    }
}
