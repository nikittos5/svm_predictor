using Accord.Statistics.Kernels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVMFORM
{
    class Data
    {
        private double[] dataVector;
        public Data() { }
        public Data(int n)
        {
            //create n-dim vector 
            dataVector = new double[n];
        }
        public Data(double[] dVector)
        {
            //create vector Data from double array 
            dataVector = new double[dVector.Length];
            Array.Copy(dVector, dataVector, dVector.Length);
        }
        public int Length
        {
            get
            {
                return dataVector.Length;
            }
        }
        public double this[int n]
        {
            //realization of indexing of class object
            get { return dataVector[n]; }
            set { dataVector[n] = value; }
        }
        public double[] ToArray()
        {
            //create double array from vector Data
            double[] resVector = new double[dataVector.Length];
            Array.Copy(dataVector, resVector, dataVector.Length);
            return resVector;
        }
    }
}
