using System.Data;
using Accord.Math;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Statistics.Kernels;
using Accord.MachineLearning.VectorMachines;
using Accord.Statistics.Filters;
using System.Collections.Generic;
using System;

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
            //Gets train table from excell file to double matrix (svm learning method requires double matrix as argument)
            trainDataTable = new SqlReader(connectionString, dataTableName).GetLastNData(50000); //new DataReader(pathToTrainData).GetDataTable();
            trainDataJagged = NormalizeData(trainDataTable).ToJagged();
        }
        private DataTable NormalizeData(DataTable dataTable)
        {
            //Normalization
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
