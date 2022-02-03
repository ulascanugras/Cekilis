using Cekilis.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cekilis
{
    public partial class Form1 : Form
    {
        List<string> adlar;
        Random rnd = new Random();
        public Form1()
        {
            InitializeComponent();
            Icon = Resources.ball;
            VerileriOku();
            Listele();
            
        }

        private  void VerileriOku()
        {
            try
            {
                adlar= File.ReadAllLines("adlar.txt").ToList();
            }
            catch (Exception)
            {

                adlar = new List<string>();
            }
          
        }

        void Karistir(List<string> liste)
        {
            int tahlihliIndeks;
            string gecici;
            for (int i = 0; i < liste.Count; i++)
            {
                tahlihliIndeks = rnd.Next(i, liste.Count);
                gecici = liste[i];
                liste[i] = liste[tahlihliIndeks];
                liste[tahlihliIndeks] = gecici;
            }

        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            // hata kontrolu 
            string ad = txtAd.Text.Trim();
            if (ad == "")
            {
                MessageBox.Show("Eklemek için bir ad girlmelisiniz.");
                return;
            }
            // mevcut secili indeksi not al 
            int sid = lstAdlar.SelectedIndex;

            //verinin eklenmesi
            adlar.Add(ad);
            //formun temizlenmesi
            txtAd.Clear();

            // listbox güncelleme
            Listele();
            //aynı sıradaki elemanı seç
            lstAdlar.SelectedIndex = sid; // birisini ekledikten sonra secili olan secili kalıyor.
        }
        private void Listele()
        {
            lstAdlar.Items.Clear();
            foreach (string ad in adlar)
                lstAdlar.Items.Add(ad);
        }

        private void txtAd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // ekle butonuna basmadan entere basarak ısım eklemek ıcın bu konu yaptık.
            {
                e.SuppressKeyPress = true; // bu tusa basılammıs gıbı yoksa dınk sesı gelıyo

                btnEkle.PerformClick(); // enterde butonun ekle kısmına atadık.
            }
        }

        private void btnCekilis_Click(object sender, EventArgs e) // burada cekilis yap kigini aktif etmek ıcın yaptık.
        {
            if (adlar.Count == 0) return;
            Karistir(adlar);
            lblSonuc.Text = adlar[0];
            if (chkKaldır.Checked)
                adlar.RemoveAt(0);
            Listele();
            
        }

        private void lstAdlar_KeyDown(object sender, KeyEventArgs e) // listele kısmına sağ tıklayıp evetnlerden key downa cift tıkladım
        {
                int sid = lstAdlar.SelectedIndex;
            if (e.KeyCode == Keys.Delete && sid>-1) // burada amac secili olan ındexi deleteye bastıgımız an siler ve tekrar listeledik.
            {
                adlar.RemoveAt(sid);
                Listele();
                lstAdlar.SelectedIndex = sid < lstAdlar.Items.Count ? sid : lstAdlar.Items.Count - 1; // sildigimiz bir seyin indexini degistirmeden silme.
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) // bir dosaya kaydettik.
        {
            DialogResult dr = MessageBox.Show("program kapatılıcaktır. Değişiklikleri kaydetmek istiyormsunuz ?", "Çekiliş", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
            switch (dr)
            {
                
                case DialogResult.Cancel:
                    e.Cancel = true;

                    break;
                case DialogResult.Yes:
            File.WriteAllLines("adlar.txt",adlar);
                    break;
                case DialogResult.No:
                    break;
            }
        }


        private void btnUp_Click(object sender, EventArgs e) // yukarı butonunu yyukarı yonde hareket ettırdık
        {
            int sid = lstAdlar.SelectedIndex;
            if (sid < 1) return;
            string gecici = adlar[sid - 1];
            adlar[sid - 1] = adlar[sid];
            adlar[sid] = gecici;
            Listele();
            lstAdlar.SelectedIndex = sid - 1;
            
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            int sid = lstAdlar.SelectedIndex;
            if (sid <0 || sid>=adlar.Count-1 ) return;
            string gecici = adlar[sid + 1];
            adlar[sid + 1] = adlar[sid];
            adlar[sid] = gecici;
            Listele();
            lstAdlar.SelectedIndex = sid +1;

        } // asagı butonu asagı yonde hareket ettırdık.

        private void lstAdlar_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnUp.Enabled = lstAdlar.SelectedIndex > 0;// sectigimiz isim en yukarda ise yukarı butonu in aktif oluyor
            btnDown.Enabled = lstAdlar.SelectedIndex != lstAdlar.Items.Count-1; //sectigimiz isim en asagıda ise asagı butonu in aktif oluyor
        }
    }
}
