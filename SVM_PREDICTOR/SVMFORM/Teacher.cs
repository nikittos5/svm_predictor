using System.Data;
using Accord.Math;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Statistics.Kernels;
using Accord.MachineLearning.VectorMachines;
using Accord.Statistics.Filters;

namespace SVMFORM
{
    class Teacher
    {
        private DataTable trainDataTable;
        private double[][] trainDataJagged;
        public double[] mean;
        public double[] rmsd;
        public void GetTrainData(string connectionString, string dataTableName, string sheet = "sheet1")
        {
            //Gets train data from sql database
            trainDataTable = new SqlReader(connectionString, dataTableName).GetLastNData(25000);
            //Normalizes gotten data
            trainDataJagged = NormalizeData(trainDataTable).ToJagged();
        }
        private DataTable NormalizeData(DataTable dataTable)
        {
            //Normalization of data
            Normalization normalization = new Normalization(dataTable);
            mean = new double[dataTable.Columns.Count];
            rmsd = new double[dataTable.Columns.Count];
            for(int i = 0; i< dataTable.Columns.Count; i++)
            {
                mean[i] = normalization[i].Mean;
                rmsd[i] = normalization[i].StandardDeviation;
            }
            return normalization.Apply(dataTable);
        }
        public SupportVectorMachine<Gaussian> Learn()
        {
            //Creates new SVM and teach it
            var teacher = new OneclassSupportVectorLearning<Gaussian>()
            {
                //Gaussian kernel
                Kernel = new Gaussian(0.1),
                Nu = 0.1
            };
            return teacher.Learn(trainDataJagged);
        }
    }
}
