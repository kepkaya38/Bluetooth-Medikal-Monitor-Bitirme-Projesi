using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Bluetooth_Medikal_Monitör
{
    public partial class Form3 : Form
    {
        Form1 frm1 = new Form1();
        Form2 frm2 = new Form2();

        public Form3()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frm1.Show(); //Button'a tıkladığımız zaman form2'ye geçmesini sağlıyoruz
            this.Hide(); //Form2 açıldıktan sonra FORM1'ri gizliyoruz.

        }

        private void button2_Click(object sender, EventArgs e)
        {
            frm2.Show(); //Button'a tıkladığımız zaman form2'ye geçmesini sağlıyoruz
            this.Hide(); //Form2 açıldıktan sonra FORM1'ri gizliyoruz.
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
