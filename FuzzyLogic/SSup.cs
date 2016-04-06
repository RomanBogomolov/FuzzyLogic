using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyLogic
{
    public class SSup
    {
        public int[] flag;
        public int[] vecNum;

        public SSup(int[] vecNum, int[] flag)
        {
            this.flag = flag;
            this.vecNum = vecNum;
        }

    }
}
