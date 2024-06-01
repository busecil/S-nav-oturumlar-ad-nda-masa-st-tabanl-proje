using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Oturumlar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Oturumlar009Varliklar veriTabani = new Oturumlar009Varliklar();
        List<Oturum> Oturumlar;

        private void Form1_Load(object sender, EventArgs e)
        {
            Oturumlar = veriTabani.Oturumlar.ToList();
            dataGridView1.DataSource = Oturumlar;
            dataGridView1.Columns[0].Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Oturum eklenecekOturum = new Oturum();
            DateTime secilenSaat = dateTimePicker1.Value;
            DateTime saat = new DateTime (1,1,1,secilenSaat.Hour,secilenSaat.Minute,0);
            eklenecekOturum.Saat = saat.TimeOfDay;
            eklenecekOturum.Aktif = checkBox1.Checked ? true : false;
            Oturumlar.Add(eklenecekOturum);
            veriTabani.Oturumlar.Add (eklenecekOturum); 
            veriTabani.SaveChangesAsync();
            veriYukle();
            temizle();
                
        }

        private void temizle()
        {
            label2.Text = "-1";
            textBox1.Clear();
            checkBox1.Checked = true;
        }

        private void veriYukle()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Oturumlar;
            dataGridView1.Columns[0].Visible = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                dataGridView1.DataSource = Oturumlar.Where(n => n.Saat.ToString().Contains(textBox1.Text.ToLower())).ToList();
            }
            else
            {
                veriYukle();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (label2.Text == "-1")
            {
                MessageBox.Show("Önce silinecek Kaydı seçiniz");
                return;
            }
            int ID;
            bool sonuc = int.TryParse(label2.Text, out ID);
            if (sonuc)
            {
                Oturum o = veriTabani.Oturumlar.Find(ID);
                veriTabani.Oturumlar.Remove(o);
                Oturumlar.Remove(o);
                veriTabani.SaveChangesAsync();
                veriYukle();
                temizle();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (label2.Text == "-1")
            {
                MessageBox.Show("Önce güncellenecek Kaydı seçiniz");
                return;
            }
            int ID;
            bool sonuc = int.TryParse(label2.Text, out ID);
            if (sonuc)
            {
                Oturum O = veriTabani.Oturumlar.Find(ID);
                DateTime secilenSaat = dateTimePicker1.Value;
                DateTime saat = new DateTime(1, 1, 1, secilenSaat.Hour, secilenSaat.Minute, 0);
                O.Saat = saat.TimeOfDay;
                O.Aktif = checkBox1.Checked ? true : false;
                veriTabani.Oturumlar.AddOrUpdate(O);
                veriTabani.SaveChangesAsync();
                veriYukle();
                temizle();
            }
            else
            {
                MessageBox.Show("güncellenemedi");
            }
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dataGridView1.CurrentRow.Index > -1)
            {
                parcala();
            }
        }

        private void parcala()
        {
            object secilenDeger = dataGridView1.CurrentRow.Cells[1].Value;
            DateTime secilenSaat;
            DateTime.TryParse(secilenDeger.ToString(), out secilenSaat);
            dateTimePicker1.Value = secilenSaat;
            label2.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            checkBox1.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells[2].Value);
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                temizle();
            }
        }
    }
    
}
