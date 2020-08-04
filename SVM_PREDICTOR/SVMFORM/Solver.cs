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

namespace SVMFORM
{
    class Solver
    {
        private Data data;
        private SupportVectorMachine<Gaussian> learnedSVM;
        private Random random;
        private DataReader dataReader;
        public Solver()
        {
            //Randomizer
            random = new Random();
            //Resp for reading data
            dataReader = new DataReader(@"data");
            GetLearnedSVM(@"train_data\1data.xlsx");

        }
        public void GetLearnedSVM(string pathToTrainData)
        {
            //Create new svm and teach it with train data
            Teacher teacher = new Teacher();
            teacher.GetTrainData(pathToTrainData);
            learnedSVM = teacher.Learn();
        }
        public string HandleNextData()
        {
            var data = dataReader.GetNextData();
            //Normalizing
            var normalizedData = data;
            //Predict result of normalized Data vector
            var result = PredictState(normalizedData);
            return result ? "NORMAL" : "ANOMALY";
        }
        private Data NormalizeData(Data data)
        {
            //Nomalizing
            Data normalizedData = data.Normalize();
            return normalizedData;
        }
        private bool PredictState(Data data)
        {
            //SVM doing it's job
            return learnedSVM.Decide(data.ToArray());
        }

    }
}
