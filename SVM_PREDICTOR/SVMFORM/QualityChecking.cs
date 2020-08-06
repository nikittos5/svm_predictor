using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void PrintStats()
        {
            Console.WriteLine("Количество проверенных записей: {0}", generalCount);
            Console.WriteLine("Количество нормальных записей: {0}", posAndRight+negButWrong);
            Console.WriteLine("Количество аномальных записей: {0}", posButWrong+negAndRight);
            Console.WriteLine("Количетсво верно выявленных нормальных записей: {0}", posAndRight);
            Console.WriteLine("Количество верно выявленных аномальных записей: {0}", negAndRight);
            Console.WriteLine("Доля выявленных нормальных записей: {0}%", (posAndRight+posButWrong)*100/generalCount);
            Console.WriteLine("Доля верно выявленных нормальных записей: {0}%", posAndRight*100/generalCount);
            Console.WriteLine("Доля выявленных аномальных записей: {0}%", (negAndRight+negButWrong)*100/generalCount);
            Console.WriteLine("Доля верно выявленных аномальных записей: {0}%", negAndRight*100/generalCount);
        }
    }
}
