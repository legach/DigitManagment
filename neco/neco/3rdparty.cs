using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatrixLibrary;
using GraphLib;

namespace neco
{
    //Всякие разные методы для работы
    class _3rdparty
    {

        /// <summary>
        /// конкатенация матриц горизонтальная
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Matrix Concatenate_Horiz(Matrix A, Matrix B)
        {
            Matrix C = new Matrix(A.NoRows, A.NoCols + B.NoCols);
            for (int i = 0; i < C.NoRows; i++)
                for (int j = 0; j < C.NoCols; j++)
                {
                    if (j < A.NoCols)
                        C[i,j] = A[i,j];
                    else
                        C[i,j] = B[i,j - A.NoCols];
                }
            return C;
        }
        /// <summary>
        /// конкатенация матриц вертикальная
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Matrix Concatenate_Vert(Matrix A, Matrix B)
        {
            Matrix C = new Matrix(A.NoRows + B.NoRows, A.NoCols);
            for (int i = 0; i < C.NoRows; i++)
                for (int j = 0; j < C.NoCols; j++)
                {
                    if (i < A.NoRows)
                        C[i, j] = A[i, j];
                    else
                        C[i, j] = B[i - A.NoRows, j];
                }
            return C;
        }
        /// <summary>
        /// обрезание матрицы до заданных размеров
        /// </summary>
        /// <param name="A"></param>
        /// <param name="NoRows"></param>
        /// <param name="NoCols"></param>
        /// <returns></returns>
        public static Matrix Cut(Matrix A, int NoRows, int NoCols)
        {
            Matrix rez = new Matrix(Math.Min(A.NoRows, NoRows), Math.Min(A.NoCols, NoCols));
            for (int i = 0; i < rez.NoRows; i++)
                for (int j = 0; j < rez.NoCols; j++)
                    rez[i, j] = A[i, j];
            return rez;
        }
        /// <summary>
        /// расово-верный модуль
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double mod(double x)
        {
            if (x < 0)
                x = -x;
            return x;
        }
        /// <summary>
        /// символ Кронекера
        /// </summary>
        /// <param name="symb"></param>
        /// <param name="original"></param>
        /// <returns></returns>
        public static int δ(int symb, int original)
        {
            if (symb == original)
                return 1;
            else
                return 0;
        }

        /// <summary>
        /// возведение матрицы в целую степень
        /// </summary>
        /// <param name="M"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static Matrix mPow(Matrix M, int n)
        {
            Matrix res = new Matrix(Matrix.Identity(M.NoRows));
            for (int i = 0; i < n; i++)
            {
                res = res * M;
            }
            return res;
        }

        /// <summary>
        /// факториал числа
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static double Factorial(double i)
        {
            return ((i <= 1) ? 1 : (i * Factorial(i - 1)));
        }

        /// <summary>
        /// матричная экспонента - переделана методом "удвоения" аргумента
        /// </summary>
        /// <param name="M"></param>
        /// <param name="element_count"></param>
        /// <returns></returns>
        public static Matrix mExp(Matrix M, int element_count)
        {
            //поиск максимального числа в матрице
            int m_max = Convert.ToInt32(mod(Math.Round(M[0, 0])));
            foreach (double m_el in M.toArray)
                if (mod(m_el) > m_max) m_max = 1 + Convert.ToInt32(mod(Math.Round(m_el)));
            // деление всей матрицы на это число 
            if (m_max == 0) m_max = 1;
            M /= Convert.ToDouble(m_max);

            //Обычный рассчет экспоненты
            Matrix res = new Matrix(M.NoRows, M.NoRows);
            for (int i = 0; i < element_count; i++)
            {
                res = res + mPow(M, i) / Factorial(i);
            }
            //восстановление результата
            res = mPow(res, m_max);

            return res;
        }


        /// <summary>
        ///  Генерация аддитивного Гауссовского белого шума
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public static Matrix AWGN_generator(int column, int row)
        {
            Matrix result = new Matrix(row, column);
            Random rand = new Random();
            double u1 = rand.NextDouble(); //these are uniform(0,1) random doubles

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    double u2 = rand.NextDouble();
                    double temp1 = Math.Sqrt(-2.0 * Math.Log(u1));
                    double temp2 = 2.0 * Math.PI * u2;
                    result[i, j] = temp1 * Math.Cos(temp2);
                    u1 = u2;
                }
            }

            return result; // return the generated random sample to the caller
        }
 
    }
}
