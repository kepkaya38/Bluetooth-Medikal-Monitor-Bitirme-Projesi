using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Diagnostics;
using ZedGraph;


namespace Bluetooth_Medikal_Monitör
{
    
    public partial class Form1 : Form
    {
        string Gelen = string.Empty;
        string[] ports = SerialPort.GetPortNames();
        // Grafik tanımlanıyor
        GraphPane myPane = new GraphPane();
        RollingPointPairList listPointsOne = new RollingPointPairList(1000);
        LineItem myCurveOne;
        


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        { 
            // Bağlantı Portları Eklendi 
            foreach (string port in ports)
            {
                cbComPort.Items.Add(port);
            }
            //Band Değerleri Eklendi
            cbBaud.Items.Add("2400");
            cbBaud.Items.Add("4800");
            cbBaud.Items.Add("9600");
            cbBaud.Items.Add("19200");
            cbBaud.Items.Add("38400");
            cbBaud.Items.Add("57600");
            cbBaud.Items.Add("115200");
            cbBaud.SelectedIndex = 2;

        }


        //Seri port Bağlama
        private void btnBaglan_Click(object sender, EventArgs e)
        {
            if (SeriPort.IsOpen == false)
            {
                if (cbComPort.Text == "")
                    return;
                SeriPort.PortName = cbComPort.Text;
                SeriPort.BaudRate = Convert.ToInt32(cbBaud.Text);
                try
                {
                    SeriPort.Open();
                }
                catch (Exception er)
                {
                    MessageBox.Show("Error:" + er.Message);
                }
            }
            else
            {
                rtbEkran.Text = "seriport zaten açık";
            }
        }
        // Formu kapatır
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (SeriPort.IsOpen == true)
            {
                SeriPort.Close();
                
            }
            string message = "Sağlıcakla Kalmanız Dileğiyle";
            string title = "Bluetooth Medikal Monitör";
            MessageBox.Show(message, title);
            Application.Exit();
        }
           
        //Port bağlantısını keser
        private void btnKes_Click(object sender, EventArgs e)
        {
            if (SeriPort.IsOpen == true)
            {
                SeriPort.Close();
            }
        }
        //Data alma
        private void SeriPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                Gelen = SeriPort.ReadLine();
                this.Invoke(new EventHandler(DisplayText));
            }
            catch (Exception er)
            {
                MessageBox.Show("Error:" + er.Message);
            }
        }

        private void DisplayText(object s, EventArgs e)
        {
            //Gelen veriyi bölme ve yazdırma
            label31.Text = Gelen;
            if (timer1.Enabled == true)
            {
                string ctrl = Gelen.Substring(0, 1);
                if (ctrl=="$")
                {
                string[] parsed = Gelen.Split('#');
                string ArananC = "#";
                int bulunanC = (Gelen.Length - Gelen.Replace(ArananC, "").Length) / ArananC.Length;
                if (bulunanC == 3)
                {
                    label17.Text = parsed[1];
                    label25.Text = parsed[2];
                    
                }
            }
        }
            rtbEkran.Text += Gelen;
            rtbEkran.SelectionStart = rtbEkran.Text.Length;
            rtbEkran.ScrollToCaret();
            
            Gelen = String.Empty;
        }

        private void btnEkraniTemizle_Click(object sender, EventArgs e)
        {
            rtbEkran.Clear();
        }

     

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            //Gelen Veriyi Kaydetme
            saveFileDialog1.Title = "Lütfen Dosya Kaydetme Yerini Seçin";
            saveFileDialog1.Filter = "(*.doc)|*.doc|(*.txt)|*.txt|Tüm dosyalar(*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.InitialDirectory = "C:\\";
            saveFileDialog1.ShowDialog();

            StreamWriter Kaydet = new StreamWriter(saveFileDialog1.FileName);
            Kaydet.Write(rtbEkran.Text);
            Kaydet.Close();
        }

        private void btnAc_Click(object sender, EventArgs e)
        {
            //Kayıtlı Veri Açma
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            try //
            {
            }
            catch 
            {
                MessageBox.Show(openFileDialog1.FileName + " Can not open this file");
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        public void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int sayi = Convert.ToInt32(label10.Text);
            sayi++;
            label10.Text = sayi.ToString();
            
        }

    }
}
