using Accord.Statistics.Kernels;
using Accord.MachineLearning.VectorMachines;
using System.Collections.Generic;

namespace SVMFORM
{
    class Solver
    {
        private SupportVectorMachine<Gaussian> learnedSVM;
        private SqlReader sqlReader;
        public QualityChecking qualityCheking;
        private double[] mean;
        private double[] rmsd;
        public Solver(string connectionStringTest, string dataTableNameTest, string connectionStringTrain, string dataTableNameTrain, HashSet<int> permit)
        {
            //Resp for reading data
            sqlReader = new SqlReader(connectionStringTest, dataTableNameTest, permit);
            //Checks quality of predictions
            qualityCheking = new QualityChecking();
            //Gets learned SVM
            GetLearnedSVM(connectionStringTrain, dataTableNameTrain, permit);
        }
        public void GetLearnedSVM(string connectionString, string dataTableName, HashSet<int> permit)
        {
            //Create new svm and teach it with train data
            Teacher teacher = new Teacher();
            teacher.GetTrainData(connectionString, dataTableName, permit);
            learnedSVM = teacher.Learn();
            mean = teacher.mean;
            rmsd = teacher.rmsd;
        }
        public string HandleNextData()
        {
            //Gets new data vector
            var data = sqlReader.GetNextData();
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
            //SVM doing it's job :)
            return learnedSVM.Decide(data.ToArray());
        }
    }
}
