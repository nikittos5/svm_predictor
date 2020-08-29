using System;
using System.Collections.Generic;
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
           if (this.textBox2.Text == "" || this.textBox3.Text == "" || this.textBox4.Text == "")
                this.textBox1.Text = "Some of the fields required to fill are empty";
            else
            {
                this.textBox1.Text = "";
                string[] splitString = this.textBox2.Text.Split(new char[] { ';' });
                string connectionStringTest = "Data Source="+splitString[0]+";Initial Catalog="+splitString[1]+ ";Integrated Security=true;",
                dataTableNameTest = splitString[2];
                splitString = this.textBox3.Text.Split(new char[] { ';' });
                string connectionStringTrain = "Data Source=" + splitString[0] + ";Initial Catalog=" + splitString[1] + ";Integrated Security=true;",
                dataTableNameTrain = splitString[2];
                splitString = this.textBox4.Text.Split(new char[] { ';' });
                HashSet<int> permit = new HashSet<int>();
                foreach (var num in splitString)
                    permit.Add(Convert.ToInt32(num));
                solver = new Solver(connectionStringTest, dataTableNameTest, connectionStringTrain, dataTableNameTrain, permit);
                this.timer1.Enabled = true;
            }
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
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
    }
}
