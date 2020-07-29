using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SVMFORM
{
    public partial class Form1 : Form 
    {
        string text;
        public Form1(string text="")
        {
            InitializeComponent();
            this.text = text;
            this.textBox1.Text = text;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
