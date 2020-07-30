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
            double[][] traintable = new double[4000][];
            double[][] testtable = new double[966][];
            Console.WriteLine(jtable[0][0]);
            for (int i = 0; i < 4000; i++)
                traintable[i] = jtable[i];
            for (int i = 4000; i < 4966; i++)
                testtable[i - 4000] = jtable[i];
            var learn = new OneclassSupportVectorLearning<Gaussian>()
            {
                UseKernelEstimation = true
            };
            var svm = learn.Learn(traintable);
            bool[] predicts = svm.Decide(testtable);
            int countOfTrue = 0;
            foreach (bool pred in predicts)
            {
                Console.Write("{0} ", pred);
                if (pred == true)
                    countOfTrue++;
            }
            Console.WriteLine(countOfTrue * .1 / 966);
            Console.ReadLine();
    }
    }
}
