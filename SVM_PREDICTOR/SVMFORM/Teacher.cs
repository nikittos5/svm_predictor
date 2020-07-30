using System;
using System.Linq;
using Accord.Statistics.Models.Regression.Linear;
using Accord.Statistics.Analysis;
using Accord.IO;
using Accord.Math;
using System.Data;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Math.Optimization.Losses;
using Accord.Statistics.Kernels;
using Accord.Controls;

namespace SVMFORM
{
    class Teacher
    {
        public void Learn()
        {
            string testXlsxFilePath = "..\\..\\data\\1data.xlsx";
            DataTable table = new ExcelReader(testXlsxFilePath, false).GetWorksheet("sheet1");
            double[][] jtable = table.ToJagged();
            double[][] traintable = new double[4897][];
            double[][] testtable = new double[69][];
            Console.WriteLine(jtable[0][0]);
            for (int i = 0; i < 4897; i++)
            {
                traintable[i] = new double[5]; 
                for (int j = 0; j < 5; j++)
                    traintable[i][j] = jtable[i][j];
            }
            for (int i = 4897; i < 4966; i++)
            {
                testtable[i- 4897] = new double[5];
                for (int j = 0; j < 5; j++)
                    testtable[i - 4897][j] = jtable[i][j];
            }
            var learn = new OneclassSupportVectorLearning<Linear>()
            {
                Kernel = new Linear(),
                Nu = 0.01
            };
            var svm = learn.Learn(traintable);
            bool[] predicts = svm.Decide(testtable);
            int countOfTrue = 0;
            foreach (bool pred in predicts)
            {
                if (pred == true)
                    countOfTrue++;
            }
            Console.WriteLine(countOfTrue * 1.0 / 69 *100);
            Console.ReadLine();
    }
    }
}
