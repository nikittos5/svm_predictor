using System;
using Accord.Statistics.Kernels;
using Accord.MachineLearning.VectorMachines;

namespace SVMFORM
{
    class Solver
    {
        private SupportVectorMachine<Gaussian> learnedSVM;
        private Random random;
        private SqlReader sqlReader;
        public QualityChecking qualityCheking;
        private double[] mean;
        private double[] rmsd;
        public Solver()
        {
            //Randomizer
            random = new Random();
            //Resp for reading data
            sqlReader = new SqlReader(@"Data Source=DESKTOP-OIVS035; Initial Catalog=TestData; Integrated Security=True", "RW1A0");
            //Checks quality of predictions
            qualityCheking = new QualityChecking();
            GetLearnedSVM(@"Data Source=DESKTOP-OIVS035; Initial Catalog=TestData; Integrated Security=True", "RW1A0");

        }
        public void GetLearnedSVM(string connectionString, string dataTableName)
        {
            //Create new svm and teach it with train data
            Teacher teacher = new Teacher();
            teacher.GetTrainData(connectionString, dataTableName);
            learnedSVM = teacher.Learn();
            mean = teacher.mean;
            rmsd = teacher.rmsd;
        }
        public string HandleNextData()
        {
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
            //SVM doing it's job
            return learnedSVM.Decide(data.ToArray());
        }
    }
}
