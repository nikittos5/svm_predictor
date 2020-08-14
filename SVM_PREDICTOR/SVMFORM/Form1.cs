using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SVMFORM
{
    public partial class Form1 : Form
    {
        private Solver solver;
        public int k = 1;
        public Form1()
        {
            InitializeComponent();
            this.label1.Text = "Normal0";
            //Teach model and issues predicts
            solver = new Solver();
            Data d = new SqlReader(@"Data Source=DESKTOP-OIVS035; Initial Catalog=TestData; Integrated Security=True", "RW1A0").GetNextData();
            }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            solver.qualityCheking.PrintStats();
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("Читаем из базы");
            var result = await Task.Run(solver.HandleNextData) + (k++).ToString();
            label1.Text = result;
        }
    }
}
