using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyLogic
{
    public class Rule
    {

        public Skill[] u;
        private Function function;
        public double[] m;

        public Rule(Skill[] u, Function function)
        {
            this.u = u;
            this.function = function;
            m = findMin(u);
        }

        public double[] findMin(Skill[] arr)
        {
            int size = arr[0].skill.Length;

            for (int i = 0; i < arr.Length; i++)
                if (arr[i].skill.Length != size)
                    throw new Exception("Incorrect skill");

            double[] min = new double[size];
            for (int i = 0; i < min.Length; i++)
                min[i] = Int32.MaxValue;

            for (int i = 0; i < arr.Length; i++)
                for (int j = 0; j < size; j++)
                    if (arr[i].skill[j] < min[j])
                        min[j] = arr[i].skill[j];

            m = min;

            return min;
        }

        public double getF(double x)
        {
            return function.getY(x);
        }
    }
}
