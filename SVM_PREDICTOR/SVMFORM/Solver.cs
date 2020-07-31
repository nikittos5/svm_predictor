using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Accord.IO;
using Accord.Math;
using Accord.Statistics.Kernels;
using Accord.MachineLearning.VectorMachines;

namespace SVMFORM
{
    class Solver
    {
        private double[][] dataTable;
        private int numberOfNextData = 0;
        private SupportVectorMachine<Linear> learnedSVM;
        private Random random;
        public Solver()
        {
            random = new Random();//оставил на всякий
        }
        public void GetNextDataTable(string pathToNextTable, string sheet="sheet1")
        {
            dataTable = new ExcelReader(pathToNextTable).GetWorksheet(sheet).ToJagged();//читает из Excell в dataTable
        }
        public void GetLearnedSVM(string pathToTrainData)
        {
            Teacher teacher = new Teacher();
            teacher.GetTrainData(pathToTrainData);//Загружает тренировочные данные для обучения модели
            learnedSVM = teacher.Learn();//Обучает 
        }
        public string HandleNextData()
        {
            var data = ReadNextData();//Получает вектор Data из dataTable
            var normalizedData = NormalizeData(data);//Нормализация вектора
            var result = PredictState(normalizedData);//Для вектора предсказывает "аномальность"
            return result ? "NORMAL" : "ANOMALY";
        }
        private Data ReadNextData()
        {
            if (numberOfNextData > dataTable.GetLength(0)) numberOfNextData = 0;
            return new Data(dataTable[numberOfNextData++]);
            //Из матрицы dataTable получает вектор Data под номером numberOfNextData
        }
        private Data NormalizeData(Data data)
        {
            //Нормализация вектора
            Data normalizedData = data.Normalize();
            return normalizedData;
        }
        private bool PredictState(Data data)
        {
            //Работа SVM над одним вектором
            return learnedSVM.Decide(data.ToArray());
        }

    }
}
