using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyLogic
{
    public class Matrix
    {

        private double[][] matrix;
        public int ROWS;
        public int COLUMNS;

        public Matrix(int rows, int columns)
        {
            double[,] matrixConvert = new double[ROWS,COLUMNS];
            ROWS = rows;
            COLUMNS = columns;

            matrix = new double[ROWS][];
           
        }

        public Matrix(double[][] m)
        {
            if ((m == null) || (m[0] == null))
                return;
            ROWS = m.Length;
            COLUMNS = m[0].Length;
            matrix = m;
        }

        public double[][] getMatrix()
        {
            return (double[][])matrix.Clone();
        }

        public Matrix multiply(Matrix M)
        {
            if (this.COLUMNS != M.ROWS)
                throw new Exception("Incorrect matrix");

            double[][] A = this.getMatrix();
            double[][] B = M.getMatrix();
            double[,] C1 = new double[ROWS,COLUMNS];

            double[][] C = new double[ROWS][];

            int thisRows = this.ROWS;
            int thisCol = this.COLUMNS;
            int mRows = M.ROWS;
            int mCol = M.COLUMNS;
            if (thisCol != mRows)
                throw new Exception("matrices don't match: " + thisCol + " != " + mRows);

            for (int i = 0; i < thisRows; i++)
                for (int j = 0; j < mCol; j++)
                    for (int k = 0; k < thisCol; k++)
                        C1[i,j] += A[i][k] * B[k][j];

            C = Matrix.ToJagged<double>(C1);

            return new Matrix(C);
        }

        public static Matrix findMin(Matrix[] Arr)
        {
            int rows = Arr[0].getMatrix().Length;
            int cols = Arr[0].getMatrix()[0].Length;

            //for (int i=0;i<Arr.length;i++) { System.out.println("D["+(i+1)+"]:"); Arr[i].output(); System.out.print("\n"); }

            for (int i = 0; i < Arr.Length; i++)
                if ((Arr[i].getMatrix().Length != rows) || (Arr[i].getMatrix()[0].Length != cols))
                    throw new Exception("Incorrect size of matrix");

            double[,] min = new double[rows,cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    min[i,j] = Double.MaxValue;

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    for (int k = 0; k < Arr.Length; k++)
                        if (Arr[k].getMatrix()[i][j] < min[i,j])
                            min[i,j] = Arr[k].getMatrix()[i][j];
            
            double[][] minJ;
            minJ = Matrix.ToJagged<double>(min);
            
            return new Matrix(minJ);
        }

        public double[] getRow(int row)
        {
            double[] temp = new double[this.COLUMNS];
            for (int i = 0; i < this.COLUMNS; i++)
                temp[i] = this.matrix[row][i];
            return temp;
        }

        public void output()
        {
            for (int i = 0; i < this.ROWS; i++)
            {
                for (int j = 0; j < this.COLUMNS; j++)
                    Console.Write(matrix[i][j] + "\t");
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public static T[][] ToJagged<T>(T[,] array)
        {
            var result = new T[array.GetLength(0)][];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new T[array.GetLength(1)];
                for (int j = 0; j < result[i].Length; j++)
                {
                    result[i][j] = array[i, j];
                }
            }
            return result;
        }

    }
}
