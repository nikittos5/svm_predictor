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

namespace SVMFORM
{
    public partial class Form1 : Form
    {
        private Solver solver;
        private DataReader dataReader;
        public int k = 1;
        public Form1()
        {
            InitializeComponent();
            this.label1.Text = "Normal0";
            //Teach model and issues predicts
            solver = new Solver();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
  
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {

            Console.WriteLine("Читаем из базы");
            var result = await Task.Run(solver.HandleNextData) + (k++).ToString();
            label1.Text = result;
        }
    }
}
