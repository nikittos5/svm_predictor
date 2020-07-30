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
            DataTable table = new ExcelReader(testXlsxFilePath).GetWorksheet("sheet1");
            double[][] jtable = table.ToJagged();
            Console.WriteLine(jtable);
            var learn = new OneclassSupportVectorLearning<Gaussian>();
            var svm = learn.Learn(jtable);
            Console.WriteLine(svm.Decide(new double[] {1000, 127673.09, -49.57, 127648.02, -169.58, 127723.24, 65.69, 605.91, -57.00, 626.79, -173.59, 602.43, 70.42, 127673.09, 0.00, 0.00, 0.00, 0.00, 65.01, 611.59, 118.57, 13.18, -100.87, 13.92, 60.00, 0.01, 6.39, 0.08, 0.00, 60.66, 124631.81, -59.30, 124484.36, -179.34, 124715.07, -119.55, 612.80, 117.73, 632.53, 0.86, 610.14, 60.68, 124611.98, 0.00, 0.00, 0.00, 0.00, -120.34, 618.30, -64.05, 12.77, 69.40, 12.83, 60.00, 0.02, 6.13, 3.14, 0.00, 60.66, 124187.91, -59.31, 124162.83, -179.30, 124212.98, -119.75, 610.12, 117.69, 628.25, 0.66, 606.83, 60.69, 124187.91, 0.00, 0.00, 0.00, 0.00, -120.49, 614.88, -64.81, 12.09, 70.39, 11.90, 60.00, 0.02, 6.11, 3.14, 0.00, 70.45, 127723.24, -49.54, 127096.41, -169.53, 127773.38, 65.64, 604.45, -56.87, 621.84, -173.87, 599.87, 70.46, 127522.65, 0.00, 0.00, 0.00, 0.00, 64.95, 608.47, 119.30, 12.27, -102.06, 11.72, 60.00, 0.01, 6.34, 0.08, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 41.00}));
    }
    }
}
