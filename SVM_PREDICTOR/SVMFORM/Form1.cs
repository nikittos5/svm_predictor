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
        private Solver solver;
        public Form1()
        {
            InitializeComponent();
            this.label1.Text = "Normal";
            solver = new Solver();
            solver.GetLearnedSVM("..\\..\\data\\1data.xlsx");//обучает модель
            solver.GetNextDataTable("..\\..\\data\\1data.xlsx");//тестирует модель
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //var teacher = new Teacher();
            //teacher.Learn();
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("Читаем из базы");
            var result = await Task.Run(solver.HandleNextData);
            label1.Text = result;
        }
    }
}
