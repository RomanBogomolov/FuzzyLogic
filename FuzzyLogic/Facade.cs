using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FuzzyLogic
{
    public class Facade
    {

        bool DEBUG_D = false;
        bool DEBUG_C = false;
        bool DEBUG_LM = false;

        public Facade(bool debug_d, bool debug_c, bool debug_lm)
        {
            DEBUG_D = debug_d;
            DEBUG_C = debug_c;
            DEBUG_LM = debug_lm;
        }

        public Matrix calcD(Rule[] rules)
        {

            //page 97

            Rule[] M = rules;

            //page 98
            SubsetD[] sD = new SubsetD[M.Length];
            for (int i = 0; i < M.Length; i++)
                sD[i] = new SubsetD(M[i]);

            Matrix[] matr = new Matrix[sD.Length];
            for (int i = 0; i < sD.Length; i++)
            {
                matr[i] = new Matrix(sD[i].d_arr);
                if (DEBUG_D)
                {
                    Console.WriteLine("D[" + i + "]:");
                    matr[i].output();
                }
            }

            Matrix min = Matrix.findMin(matr);
            if (DEBUG_D)
            {
                Console.WriteLine("D:");
                min.output();
            }
            return min;
        }

        public double[] calcC(Matrix D)
        {
            int len = D.COLUMNS;
            double[] c = new double[D.ROWS];
            double[] x = getX();

            for (int i = 0; i < D.ROWS; i++)
            {
                double[] u = D.getRow(i);

                Num[] arr = new Num[len];

                for (int j = 0; j < len; j++)
                    arr[j] = new Num(x[j], u[j]);

                //sorting arr[] by u
                for (int j = 0; j < len; j++)
                    for (int k = j; k < len; k++)
                        if (arr[k].u < arr[j].u)
                        {
                            Num temp = arr[k];
                            arr[k] = arr[j];
                            arr[j] = temp;
                        }

                if (DEBUG_C) Console.WriteLine("Object #" + i + ":");

                c[i] = calcS(arr);

                if (DEBUG_C) Console.WriteLine("\nPoint estimate :" + c[i]);

            }

            return c;
        }

        public int getBest(double[] C)
        {
            double max = Double.MinValue;
            int max_ind = -1;

            for (int i = 0; i < C.Length; i++)
                if (C[i] > max)
                {
                    max = C[i];
                    max_ind = i;
                }
            return max_ind + 1;
        }

        private double[] getX()
        {
            double[] temp = new double [Variables.SIZE_OF_D];
            for (int i = 0; i < temp.Length; i++)
                temp[i] = Variables.STEP_OF_D * i;
            return temp;
        }

        private double calcS(Num[] arr)
        {
            int size = arr.Length;
            double[] x = new double[size];
            double[] u = new double[size];
            for (int i = 0; i < size; i++)
            {
                x[i] = arr[i].x;
                u[i] = arr[i].u;
            }

            if (DEBUG_C)
            {
                Console.WriteLine("E:");
                for (int i = 0; i < u.Length; i++)
                    Console.WriteLine("\t" + u[i]);

                Console.WriteLine("\nX:");
                for (int i = 0; i < x.Length; i++)
                    Console.WriteLine("\t" + x[i]);
            }

            double max = findMax(u);
            double s = 0.0;
            double[] m = new double[size];
            double[] lbd = new double[size];

            for (int i = 0; i < size; i++)
            {
                if (i == 0)
                {
                    lbd[i] = u[0];
                    m[i] = calcM(x, 0);
                }
                else
                {
                    lbd[i] = u[i] - u[i - 1];
                    m[i] = calcM(x, i);
                }
            }

            for (int i = 0; i < size; i++)
                s += (double)lbd[i] * m[i];

            return (double)s / max;
        }

        private double findMax(double[] arr)
        {
            double max = arr[0];
            for (int i = 1; i < arr.Length; i++)
                if (arr[i] > max)
                    max = arr[i];
            return max;
        }

        private double calcM(double[] x, int first)
        {
            double s = 0.0;
            for (int i = first; i < x.Length; i++)
                s += x[i];
            return (double)s / (x.Length - first);
        }
    }
}
