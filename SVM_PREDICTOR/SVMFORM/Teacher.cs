using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Accord.IO;
using Accord.Math;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Statistics.Kernels;
using Accord.MachineLearning.VectorMachines;
using Accord.Statistics.Filters;
using Accord;

namespace SVMFORM
{
    class Teacher
    {
        private double[][] trainDataTable;
        public void GetTrainData(string pathToTrainData, string sheet = "sheet1")
        {
            //Загружает данные из Excell в trainDataTable
            trainDataTable = new ExcelReader(pathToTrainData).GetWorksheet(sheet).ToJagged();
            //Нормализация
            trainDataTable = NormalizeData(trainDataTable);
        }
        private double[][] NormalizeData(double[][] dataTable)
        {
            double[][] dTable = new double[dataTable.GetLength(0)][];
            for (int i = 0; i < dataTable.GetLength(0); i++)
            {
                dTable[i] = new double[dataTable[0].Length];
                Array.Copy(dataTable[i], dTable[i], dataTable[0].Length);
            }
            for (int i = 0; i < dTable.GetLength(0); i++)
            {
                double lamb = 0;
                for (int j = 0; j < dTable[0].Length; j++)
                    lamb += dTable[i][j] * dTable[i][j];
                lamb = Math.Sqrt(lamb);
                for (int j = 0; j < dTable[0].Length; j++)
                    dTable[i][j] /= lamb;
            }
            return dTable;
        }
        public SupportVectorMachine<Gaussian> Learn()
        {
            var teacher = new OneclassSupportVectorLearning<Gaussian>()
            {
                Kernel = new Gaussian(0.1), // Будем это ядро использовать
                Nu = 0.1//Вот этот коэффициент как-то влияет на точность, я до конца не понял
            };
            return teacher.Learn(trainDataTable);
        }
    }
}
