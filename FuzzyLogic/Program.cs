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
            //ВВОДНЫЕ ДАННЫЕ
            int col = 5;
            int row = 5;

            Console.WriteLine("Enter DATA:");

            double[,] enterArray =
            {
                {0.8, 0.6, 0.5, 0.1, 0.3}, 
                {0.5, 1, 0, 0.5, 1},
                { 0.6, 0.9, 1, 0.7, 1 },
                {1, 0.3, 1, 0, 0 },
                {0, 0.5, 1, 0.8, 0.1 }
            };

            double[][] enterArray2 = 
            {
                new double[] {0.8, 0.6, 0.5, 0.1, 0.3},
                new double[] {0.5, 1, 0, 0.5, 1},
                new double[] { 0.6, 0.9, 1, 0.7, 1 },
                new double[] {1, 0.3, 1, 0, 0 },
                new double[] {0, 0.5, 1, 0.8, 0.1 }
            };

            PrintMatrix(enterArray, col, row);

           
            

            Facade f = new Facade(true, false, true);

            Input inp;




            //Rule rl = new Rule(tut, Function func);
            
            //Matrix M = f.calcD();
            //double[] C = f.calcC(M);

            //Console.WriteLine("THE BEST:{0}", f.getBest(C));
            



            Console.ReadKey();

        }

        public static void PrintMatrix(double[,] mass, int col, int row)
        {
            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    Console.Write(mass[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        //НЕЧЕТКИЕ ФУНКЦИИ

        //Более, чем удовлетворяющий
        public class mMS : Function
        {
            public mMS() { }

            public override double getY(double x)
            {
                return Math.Sqrt(Math.Pow(x, 2));
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
    // **********РАЗМЕР МАТРИЦЫ [D]**********
    public class Variables
        {
            private static int sizeD = 11;
            private static int stepD = 1 / (sizeD - 1);

            public static int SIZE_OF_D
            {
                get { return sizeD; }
                set { sizeD = value; }
            }

            public static int STEP_OF_D
            {
                get { return stepD; }
                set { stepD = value; }
            }

        }
        //Облегчаем хитрую сортировкую
        public class Num
        {
            public double x;
            public double u;

            public Num(double x, double u) { }
        }

        // КЛАСС SKILL

        public class Skill
        {
            public double[] skill;

            public Skill(double[] skill) { }
        }

        public class Matrix
        {
            private double[,] matrix;                       //двумерный массив
            private double[] toArrayDoubles;                //одномерный массив
            public int ROWS;
            public int COLUMNS;
            public Matrix(int rows, int columns) { }        //конструктор для пустой матрицы
            public Matrix(double[,] m) { }                 //конструктор для готового массива

            public double[,] getMatrix()                   // возврат матрицы в виде массива
            {
                for (int i = 0; i < ROWS; i++)
                    for (int j = 0; j < COLUMNS; j++)
                    {
                        toArrayDoubles[i] = matrix[i,j];
                    }
                return matrix;

            }

            public static Matrix findMin(Matrix[] arr)      /* поиск минимальных значений в массиве матриц, если размеры не совпадают — выкидывается исключение*/
            {
                int rows = arr[0].getMatrix().Length;
                int cols = arr[0].getMatrix().Length;

                for (int i = 0; i < arr.Length; i++)
                    if ((arr[i].getMatrix().Length != rows) || (arr[i].getMatrix().Length != cols))
                        throw new Exception("Incorrect size of matrix");

                double[,] min = new double[rows,cols];
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        min[i,j] = double.MaxValue;

                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        for (int k = 0; k < arr.Length; k++)
                            if (arr[k].getMatrix()[i,j] < min[i,j])
                                min[i,j] = arr[k].getMatrix()[i,j];

                return new Matrix(min);
            }

            public double[] getRow(int row)                     //возврат нужной строки матрицы
            {
                for (int i = 0; i < matrix.Length; i++)
                {
                    toArrayDoubles[i] = matrix[row, i];
                }
                return toArrayDoubles;
            }
            

            public void output()                                // вывод матрицы в консоль
            {
                for (int i = 0; i < ROWS; i++)
                {
                    for (int j = 0; j < COLUMNS; j++)
                    {
                        Console.Write(matrix[i, j] + "\t");
                    }
                    Console.WriteLine();
                }
            }
        }
        public abstract class Function
        {
            public string ID;

            public abstract double getY(double x);
        }

        //ОПИСАНИЕ ПРАВИЛ
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

            public double[] findMin(Skill[] arr)  /*вычисление минимальных значений одного из навыков каждого  объекта (пересечение нечетких множеств — нахождение минимума их функций принадлежности)*/
            {
                int size = arr[0].skill.Length;
                for (int i = 0; i<arr.Length;i++)
                    if (arr[i].skill.Length != size)
                        throw new Exception("Incorrect skill");

                double[] min = new double[size];
                for (int i = 0; i<min.Length;i++)
                    min[i] = int.MaxValue;

                for (int i = 0; i<arr.Length;i++)
                    for (int j = 0; j<size;j++)
                        if (arr[i].skill[j]< min[j])
                            min[j] = arr[i].skill[j];

                return min;
            }

            public double getF(double x)                /*возврат функции, соответствующей данному правилу*/  
            { 
                return function.getY(x);
            }
    }

        //ФОРМИРОВАНИЕ ПРАВИЛ
        public class SubsetD
        {
            private bool DEBUG = false;

            public double[,] d_arr;

            public SubsetD(Rule rule) 
            {
                d_arr = makeD(rule);
            }

            /* если правила неверны - исключение*/

            public double[,] makeD(Rule rule)
            {
                d_arr = new double [rule.m.Length,Variables.SIZE_OF_D];

                for (int i = 0; i<rule.m.Length;i++)
                    for (int j = 0; j<Variables.SIZE_OF_D;j++)
                    { /*вычисление элементов матрицы D, результат не может быть больше единицы — для контроля этого используется метод findMin*/
                        d_arr[i,j]=findMin(1-rule.m[i]+rule.getF(j* Variables.STEP_OF_D));
                        if (DEBUG) Console.WriteLine("m["+i+"]="+rule.m[i]+"\t j*Variables.STEP_OF_D()="+j* Variables.STEP_OF_D+"\tY="+rule.getF(j* Variables.STEP_OF_D)+"\n");
                    }
                
                return d_arr;
            }

            private double findMin(double x)
            {
                if (x > 1) return 1;
                return x;
            }
            
        }

        public abstract class Input
        {

            protected Rule[] rl = null;
            protected Function[] func = null;
            public int PARAMS_CNT = -1;
            public int FUNC_CNT = -1;

            public abstract Rule[] makeRules(double[][] arr);        // инициализация правил
            public abstract void initFunc(Function[] func);          // инициализация функций

            public Function[] getFunctions()                         // возврат массива функций
            {
                return func;
            }                     
        }

        //ФАСАД

        public class Facade
        {

            bool DEBUG_D = false;
            bool DEBUG_C = false;
            bool DEBUG_LM = false;

            
            public Facade(bool debug_d, bool debug_c, bool debug_lm) { } /*конструктор с возможностью выставить нужные флаги для просмотра тех или иных результатов промежуточных вычислений*/

            public Matrix calcD(Rule[] rules)
            { /* метод цепляет массив используемый правил *///page 97

                Rule [] M = rules;

                //page 98
                SubsetD [] sD = new SubsetD[M.Length];
                for (int i = 0; i<M.Length;i++)
                    sD[i] = new SubsetD(M[i]);

                Matrix[] matr = new Matrix[sD.Length];
                for (int i = 0; i<sD.Length;i++)
                {
                    matr[i]=new Matrix(sD[i].d_arr);
                    if (DEBUG_D); /*вывод матриц D(i)*/

                }
                Matrix min = Matrix.findMin(matr); /*вычисление общей матрицы D*/
                if (DEBUG_D) min.output();

                return min;
            }

            public double[] calcC(Matrix D)
            { /*подсчет точечных оценок удовлетворительности*/
                int len = D.COLUMNS;
                double[] c = new double[D.ROWS]; /* пустой массив оценок, размером соответствующий количеству альтернатив*/
                double[] x = getX(); /*получение значений X на протяжении единичного интервала*/

                for (int i = 0; i < D.ROWS; i++)
                {
                     double[] u = D.getRow(i);
                     Num[] arr = new Num[len];
                    for (int j = 0; j < len; j++)
                        arr[j] = new Num(x[j], u[j]);
                    for (int j = 0; j < len; j++) /*сортировка массива элементов общего функционального решения, соответствующей i-ой альтернативе по возрастанию, при этом в такой же порядок перемещаются и соответсвующие точки единичного интервала — для удобного вычисления промежуточных данных при подсчете последнего интеграла*/
                        for (int k = j; k < len; k++)
                            if (arr[k].u < arr[j].u)
                                {
                                    Num temp = arr[k];
                                    arr[k] = arr[j];
                                    arr[j] = temp;
                                }
                    c[i] = calcS(arr); /*вычисление точечной оценки удовлетворительности для i-ой альтернативы*/
                }
                return c;
    }
            /// <summary>
            /// тутутуту
            /// </summary>
            /// <param name="C"></param>
            /// <returns></returns>
            /// 
            public double getBest(double[] C) // возврат номера лучшей альтернативы
            {
                double best = 0;
                for (int i = 0; i < C.Length; i++)
                {
                    if (C[i] > best)
                        best = C[i];
                }
                return best;
            } 

    private double[] getX()
    { /*вычисление значений X в соответствии с размерами матрицы D*/
        double[] temp = new double[Variables.SIZE_OF_D];
        for (int i = 0; i < temp.Length; i++)
            temp[i] = Variables.STEP_OF_D * i;
        return temp;
    }

    private double calcS(Num[] arr)
    { /*вычисление точечной оценки удовлетворительности для данной альтернативы*/
        int size = arr.Length;
        double[] x = new double[size];
        double[] u = new double[size];
        for (int i = 0; i < size; i++)
        { /* более удобное представление пары элемента общего функционального решения и соответствующей ему точки единичного интервала*/
            x[i] = arr[i].x;
            u[i] = arr[i].u;
        }

        double max = findMax(u); /* поиск максимального элемента общего функционального решения для текущей альтернативы */
        double s = 0.0;
        double[] m = new double[size];
        double[] lbd = new double[size];

        for (int i = 0; i < size; i++)
        { /*перебор уровневых множеств с сопутствующими вычислениями*/
            if (i == 0)
            {
                lbd[i] = u[0];
                m[i] = calcM(x, 0);
            }
            else {
                lbd[i] = u[i] - u[i - 1];
                m[i] = calcM(x, i);
            }
        }

        for (int i = 0; i < size; i++) /*вычисление интеграла для случая с дискретными величинами*/
            s += (double)lbd[i] * m[i];

        return (double)s / max; //возврат точечной оценки
    }

            private double findMax(double[] arr) /*поиск максимального элемента массива*/
            {
                double maxArr = 0;
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i] > maxArr)
                        maxArr = arr[i];
                }
                return maxArr;
            } 

    private double calcM(double[] x, int first)
    { /*вычисление мощности уровневого множества*/
        double s = 0.0;
        for (int i = first; i < x.Length; i++)
            s += x[i];
        return (double)s / (x.Length - first);
    }

    }

        //ПРАВИЛА
        public class input : Input
        {
            private Skill A, B, C, D, E, A_M5, B_M5, A_M6, C_M6;
            public input()
            { /* переменные для контроля количества параметров и функций */
                PARAMS_CNT = 5;
                FUNC_CNT = 6;
            }

        public override void initFunc(Function[] func)
        {
            this.func = func;
        }

        private void makeSkill(double[][] arr)
        { /* подсчет навыков для создания правил; так как их объекты еще не созданы, использовать реализации Function нельзя и удобнее пересчитывать их значения вручную */

            rl = new Rule[FUNC_CNT];

            A = new Skill(arr[0]);
            B = new Skill(arr[1]);
            C = new Skill(arr[2]);
            D = new Skill(arr[3]);
            E = new Skill(arr[4]);

            double[][] u = arr;

            double[] a_m5 = (double[])u[0].Clone();
            for (int i = 0; i < u[0].Length; i++)
                a_m5[i] *= a_m5[i];       /*очень удовлетворяющий*/
            A_M5 = new Skill(a_m5);

            double[] b_m5 = (double[])u[1].Clone();
            for (int i = 0; i < u[1].Length; i++)
                b_m5[i] = 1 - b_m5[i];  /* не удовлетворяющий */
            B_M5 = new Skill(b_m5);

            double[] a_m6 = (double[])u[0].Clone();
            for (int i = 0; i < u[0].Length; i++)
                a_m6[i] = 1 - a_m6[i]; /* не удовлетворяющий */
            A_M6 = new Skill(a_m6);

            double[] c_m6 = (double[])u[2].Clone();
            for (int i = 0; i < u[2].Length; i++)
                c_m6[i] = 1 - c_m6[i]; /* не удовлетворяющий */
            C_M6 = new Skill(c_m6);

        }

        public override Rule[] makeRules(double[][] arr)
        {
            makeSkill(arr);
            rl[0] = new Rule(new Skill[]{A,B,C}          ,func[0]);  // высказывание d1
            rl[1] = new Rule(new Skill[]{A,B,C,D}        ,func[1]);  // высказывание d2
            rl[2] = new Rule(new Skill[]{A,B,C,D,E}      ,func[2]);  // высказывание d3
            rl[3] = new Rule(new Skill[]{A,B,C,E}        ,func[3]);  // высказывание d4
            rl[4] = new Rule(new Skill[]{A_M5,B_M5,C,E}  ,func[4]);  // высказывание d5
            rl[5] = new Rule(new Skill[]{A_M6,C_M6}      ,func[5]);  // высказывание d6
        
            return rl;
        }
    }
        
    }
}
