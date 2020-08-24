using System;

namespace SVMFORM
{
    class QualityChecking
    {
        private int posAndRight=0, posButWrong=0, negAndRight=0, negButWrong=0, generalCount=0;
        public void UpdateStats(bool receivedResult, bool expectedResult)
        {
            generalCount++;
            switch (receivedResult)
            {
                case true:
                    switch(expectedResult)
                    {
                        case true:
                            posAndRight++;
                            break;
                        case false:
                            posButWrong++;
                            break;
                    }
                    break;
                case false:
                    switch(expectedResult)
                    {
                        case true:
                            negButWrong++;
                            break;
                        case false:
                            negAndRight++;
                            break;
                    }
                    break;
            }

        }
        public string GetStats()
        {
            return String.Format("Количество проверенных записей: {0}" + Environment.NewLine +
              "Количетсво верно выявленных нормальных записей: {1}" + Environment.NewLine +
              "Количество верно выявленных аномальных записей: {2}" + Environment.NewLine +
              "Доля верно выявленных записей (отн. всех записей): {3}" + Environment.NewLine +
              "Доля нормальных записей, выявленных верно (отн. всех нормальных): {4}%" + Environment.NewLine +
              "Доля аномальных записей, выявленных верно (отн. всех аномальных): {5}%",
              generalCount, posAndRight,  negAndRight, (posAndRight+negAndRight) * 100 / (generalCount),
              (posAndRight+negButWrong!=0) ? posAndRight * 100 / (posAndRight+negButWrong) : 0, 
              (negAndRight + posButWrong!=0) ? negAndRight * 100 / (negAndRight+posButWrong) : 0);;          
        }
    }
}
