using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyLogic
{

    class Program
    {
        static void Main(string[] args)
        {
            double[][] u =
            {
                new[] {0.8, 0.6, 0.5, 0.1, 0.3},
                new[] {0.5, 1, 0, 0.5, 1},
                new[] {0.6, 0.9, 1, 0.7, 1},
                new[] {1, 0.3, 1, 0, 0 },
                new[] {0, 0.5, 1, 0.8, 0.1}
            };

            Calc(u);
            Console.ReadKey();

        }

        //НЕЧЕТКИЕ ФУНКЦИИ
        public static int Calc(double[][] arr)
        {
            Input inp = new inputs();
            //Function[] func;
            Function[] func = new Function[6] { new mS(), new mMS(), new mP(), new mVS(), new mS(), new mUS() };
            int STATUS = -1;
            try
            {
                Facade f = new Facade(true, true, true);
                
                inp.initFunc(func);

                Matrix M = f.calcD(inp.makeRules(arr));
                double[] C = f.calcC(M);

                STATUS = f.getBest(C);

                Console.WriteLine("THE BEST:{0}", STATUS);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return STATUS;
        }

        //Более, чем удовлетворяющий
        public class mMS : Function
        {
            public mMS() { }

            public override double getY(double x)
            {
                return Math.Round(Math.Sqrt(Math.Pow(x, 3)), 2);
            }
        }

        //Безупречный
        public class mP : Function
        {
            public mP() { }
            public override double getY(double x)
            {
                if (x == 1)
                    return 1;
                return 0;
            }
        }

        //Очень удовлетворяющий
        public class mVS : Function
        {
            public mVS() { }
            public override double getY(double x)
            {
                return x * x;
            }
        }

        //Удовлетворяющий
        public class mS : Function
        {
            public mS() { }
            public override double getY(double x)
            {
                return x;
            }
        }
        public class mUS : Function
        {
            public mUS() { }
            public override double getY(double x)
            {
                return 1 - x;
            }
        }

        //ПРАВИЛА
        public class inputs : Input
        {

            private Skill A, B, C, D, E, A_M5, B_M5, A_M6, C_M6;

            public inputs()
            {
                PARAMS_CNT = 5;
                FUNC_CNT = 6;
            }

            public override void initFunc(Function[] func)
            {
                this.func = func;
            }

            private void makeSkill(double[][] arr)
            {

                rl = new Rule[FUNC_CNT];

                A = new Skill(arr[0]);
                B = new Skill(arr[1]);
                C = new Skill(arr[2]);
                D = new Skill(arr[3]);
                E = new Skill(arr[4]);

                double[][] u = arr;

                double[] a_m5 = (double[])u[0].Clone();
                for (int i = 0; i < u[0].Length; i++)
                    a_m5[i] *= a_m5[i];
                A_M5 = new Skill(a_m5);

                double[] b_m5 = (double[])u[1].Clone();
                for (int i = 0; i < u[1].Length; i++)
                    b_m5[i] = 1 - b_m5[i];
                B_M5 = new Skill(b_m5);

                double[] a_m6 = (double[])u[0].Clone();
                for (int i = 0; i < u[0].Length; i++)
                    a_m6[i] = 1 - a_m6[i];
                A_M6 = new Skill(a_m6);

                double[] c_m6 = (double[])u[2].Clone();
                for (int i = 0; i < u[2].Length; i++)
                    c_m6[i] = 1 - c_m6[i];
                C_M6 = new Skill(c_m6);

            }

            public override Rule[] makeRules(double[][] arr)
            {

                makeSkill(arr);

                rl[0] = new Rule(new Skill[] { A, B, C }, func[0]);
                rl[1] = new Rule(new Skill[] { A, B, C, D }, func[1]);
                rl[2] = new Rule(new Skill[] { A, B, C, D, E }, func[2]);
                rl[3] = new Rule(new Skill[] { A, B, C, E }, func[3]);
                rl[4] = new Rule(new Skill[] { A_M5, B_M5, C, E }, func[4]);
                rl[5] = new Rule(new Skill[] { A_M6, C_M6 }, func[5]);

                return rl;
            }

        }

    }
}
