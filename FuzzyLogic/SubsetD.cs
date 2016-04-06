using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyLogic
{
    public class SubsetD
    {

        private readonly bool DEBUG = false;

        public double[][] d_arr;

        public SubsetD(Rule rule)
        {
            d_arr = makeD(rule);
        }

        public double[][] makeD(Rule rule)
        {
            double[,] d_arr1;
            d_arr1 = new double[rule.m.Length, Variables.SIZE_OF_D];
        
            for (int i=0; i < rule.m.Length; i++)
                for (int j=0; j < Variables.SIZE_OF_D; j++)
                {
                    
                    d_arr1[i,j] = Math.Min(1, 1-rule.m[i]+rule.getF(j*Variables.STEP_OF_D));
                    if (DEBUG) Console.WriteLine("m["+i+"]="+rule.m[i]+"\t j*Variables.STEP_OF_D()="+j*Variables.STEP_OF_D+"\tY="+rule.getF(j*Variables.STEP_OF_D)+"\n");
                }
           
            d_arr = d_arr1.Cast<double>().ToArray()
                            .Select((it, ix) => new { item = it, index = ix })
                            .GroupBy(it => it.index / (d_arr1.GetUpperBound(0) + 1), it => it.item)
                            .Select(it => it.ToArray()).ToArray();
        
            return d_arr;
        }

        private double findMin(double x)
        {
            if (x > 1) return 1;
            return x;
        }

    }
}
