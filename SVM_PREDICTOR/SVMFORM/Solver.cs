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
using Accord;
using Accord.Math.Optimization;

namespace SVMFORM
{
    class Solver
    {
        private SupportVectorMachine<Gaussian> learnedSVM;
        private Random random;
        private DataReader dataReader;
        public QualityChecking qualityCheking;
        private double[] mean;
        private double[] rmsd;
        public Solver()
        {
            //Randomizer
            random = new Random();
            //Resp for reading data
            dataReader = new DataReader(@"data");
            //Checks quality of predictions
            qualityCheking = new QualityChecking();
            GetLearnedSVM(@"train_data\1data.xlsx");

        }
        public void GetLearnedSVM(string pathToTrainData)
        {
            //Create new svm and teach it with train data
            Teacher teacher = new Teacher();
            teacher.GetTrainData(pathToTrainData);
            learnedSVM = teacher.Learn();
            mean = teacher.mean;
            rmsd = teacher.rmsd;
        }
        public string HandleNextData()
        {
            var data = dataReader.GetNextData();
            //Normalizing
            var normalizedData = NormalizeData(data);
            //Predict result of normalized Data vector
            var result = PredictState(normalizedData);
            qualityCheking.UpdateStats(result, true);
            return result ? "NORMAL" : "ANOMALY";
        }
        private Data NormalizeData(Data data)
        {
            //Normalize Data vector
            Data normalizedData = new Data(data.Length);
            for (int i = 0; i < normalizedData.Length; i++)
                normalizedData[i] = (data[i] - mean[i]) / rmsd[i];
            return normalizedData;
        }
        private bool PredictState(Data data)
        {
            //SVM doing it's job
            return learnedSVM.Decide(data.ToArray());
        }
    }
}
