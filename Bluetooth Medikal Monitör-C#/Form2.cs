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
    
        public partial class Form2 : Form
        {
            string Gelen = string.Empty;
            string[] ports = SerialPort.GetPortNames();
            // 1
            GraphPane myPane = new GraphPane();
            RollingPointPairList listPointsOne = new RollingPointPairList(1000);
            LineItem myCurveOne;
            


            public Form2()
            {
                InitializeComponent();
            }

            private void Form2_Load(object sender, EventArgs e)
            {
                //1
                myPane = zedGraphControl1.GraphPane;
                myPane.XAxis.Title.Text = "Time";
                myPane.YAxis.Title.Text = "Data";
                zedGraphControl1.Invalidate();
                zedGraphControl1.AxisChange();
                zedGraphControl1.Refresh();

                /* Adding connected ports */
                foreach (string port in ports)
                {
                    cbComPort.Items.Add(port);
                }
                /* Baudrates are added */
                cbBaud.Items.Add("2400");
                cbBaud.Items.Add("4800");
                cbBaud.Items.Add("9600");
                cbBaud.Items.Add("19200");
                cbBaud.Items.Add("38400");
                cbBaud.Items.Add("57600");
                cbBaud.Items.Add("115200");
                cbBaud.SelectedIndex = 2;

            }



            private void btnBaglan_Click(object sender, EventArgs e)
            {
                if (SeriPort.IsOpen == false)
                {

                    SeriPort.PortName = "COM9";
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
                    rtbEkran.Text = "seriport already open";
                }
            }

            private void Form2_FormClosed(object sender, FormClosedEventArgs e)
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

            private void btnKes_Click(object sender, EventArgs e)
            {
                if (SeriPort.IsOpen == true)
                {
                    SeriPort.Close();
                }
            }

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
                label31.Text = Gelen;
                if (timer1.Enabled == true)
                {
                    string ctrl = Gelen.Substring(0, 1);
                    if (ctrl == "$")
                    {
                        string[] parsed = Gelen.Split('#');
                        string ArananC = "#";
                        int bulunanC = (Gelen.Length - Gelen.Replace(ArananC, "").Length) / ArananC.Length;
                        if (bulunanC == 2)
                        {
                            label17.Text = parsed[1];
                            
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
                saveFileDialog1.Title = "Please select place to save file";
                saveFileDialog1.Filter = "(*.doc)|*.doc|(*.txt)|*.txt|Tüm dosyalar(*.*)|*.*";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.InitialDirectory = "C:\\";
                saveFileDialog1.ShowDialog();

                StreamWriter Kaydet = new StreamWriter(saveFileDialog1.FileName);
                Kaydet.Write(rtbEkran.Text);
                Kaydet.Close();
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
                //
                myPane = zedGraphControl1.GraphPane;
                myPane.YAxis.Scale.Min = 0;
                myPane.YAxis.Scale.Max = 1100;
            

            myPane.XAxis.MajorGrid.IsVisible = true;
                myPane.YAxis.MajorGrid.IsVisible = true;
            //1
            double x = Convert.ToDouble(label10.Text);
                double y1 = double.Parse(label17.Text, System.Globalization.CultureInfo.InvariantCulture);
                listPointsOne.Add(x, y1);
                myPane.Title.Text = "EKG Grafiği ";

                myPane.XAxis.Title.Text = " t(s) ";

                myPane.YAxis.Title.Text = "EKG";
                myCurveOne = myPane.AddCurve(null, listPointsOne, Color.Red, SymbolType.None);
                myCurveOne.Line.Width = 3;
            //Grafiği yenileme
                zedGraphControl1.Invalidate();
                zedGraphControl1.AxisChange();
                zedGraphControl1.Refresh();
            }

        }
    }
