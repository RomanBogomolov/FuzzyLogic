using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyLogic
{
    public class Variables
    {
        private static int sizeD = 11;                  //horizontal size of D matrix
        private static double stepD = 1 / (sizeD - 1);  //step between elements of D matrix

        public static int SIZE_OF_D
        {
            get { return sizeD; }
        }

        public static double STEP_OF_D
        {
            get { return stepD; }
        }
    }
}
