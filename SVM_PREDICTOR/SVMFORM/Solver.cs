using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVMFORM
{
    class Solver
    {
        private Random random;
        public Solver()
        {
            random = new Random();
        }
        public string HandleNextData()
        {
            var data = ReadNextData();
            var normalizedData = NormalizeData(data);
            var result = PredictState(normalizedData);
            return result ? "NORMAL" : "ANOMALY";
        }

        private Data ReadNextData()
        {
            //TODO: чтение из Excel/BD
            var data = new Data();
            return data;
        }

        private Data NormalizeData(Data data)
        {
            //TODO: нормировать данные
            var normalizedData = new Data();
            return normalizedData;
        }

        private bool PredictState(Data data)
        {
            //TODO: работа SVM
            return random.Next() % 2 == 0;
        }

    }
}
