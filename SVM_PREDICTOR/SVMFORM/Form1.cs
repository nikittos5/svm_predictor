using System;
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
            this.label1.Text = "     SVM";
            this.timer1.Enabled = false;
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            //Teach model and issues predicts
            solver = new Solver();
            this.timer1.Enabled = true;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (solver == null)
                this.textBox1.Text = "Stats don't exist";
            else
                this.textBox1.Text = solver.qualityCheking.GetStats();
        }
        private async void timer1_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("Читаем из базы");
            var result = await Task.Run(solver.HandleNextData);
            label1.Text = result;
        }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void Form1_Load(object sender, EventArgs e) { }
    }
}
