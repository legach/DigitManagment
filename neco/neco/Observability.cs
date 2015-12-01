﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatrixLibrary;

namespace neco
{
    class Observability
    {
        static Matrix errorMatrix = new Matrix(new double[,] { { 0 } });

        List<List<Matrix>> A;
        List<int> L;//по состоянию
        List<int> P;//по наблюдению
        List<List<Matrix>> H;
        List<Matrix> x;//размерность каждого элемента списка n x 1. это отсчеты состояний системы.
        List<Matrix> y;

        public static Matrix tmpVr = new Matrix(new double[,] { { 0 } });
        public static Matrix tmpSr = new Matrix(new double[,] { { 0 } });
        public static Matrix tmpVa = new Matrix(new double[,] { { 0 } });
        public static Matrix tmpSa = new Matrix(new double[,] { { 0 } });
        public static int J = 0;

        int n { get { return x[0].NoRows; } }
        int a { get { return L.Count; } }
        int d { get { return P.Count; } }
        int p { get { return y[0].NoRows; } }

        private Observability(ref State system_state) { 
            H = new List<List<Matrix>>();
            H.Add(system_state.allH());
            P = new List<int>();
            //P.Add(0);
            }
        public static bool IsSystemObservability(ref State system_state, int k0 = 0)
        {
            bool result = false;
            int tempResult = 0;
            Observability node = new Observability(ref system_state);

            if (!system_state.stationarity)     //система не стационарная p.55
            {

                tempResult = node.IsObserv_NonStationarity(ref system_state, k0);
                if (tempResult < 0)
                    result = false;
                else
                {
                    //Выставляем биты в state.
                    // 0 - высавляем observersability в 0
                    // 1 - высавляем observersability в 1
                    // 2 - тоже выставляем observersability в 1...
                    result = true;
                    switch (tempResult)
                    {
                        case 0:
                            system_state.full_observersability = false;
                            system_state.part_observersability = false;
                            break;
                        case 1:
                            system_state.full_observersability = true;
                            break;
                        case 2:
                            system_state.part_observersability = true;
                            break;
                        default: break;
                    }
                }

            }
            else //система стационарна
            {

                tempResult = node.IsObserv_stationarity(ref system_state);
                if (tempResult < 0)
                    result = false;
                else
                {
                    //Выставляем биты в state.
                    // 0 - высавляем observersability в 0
                    // 1 - высавляем observersability в 1
                    // 2 - тоже выставляем observersability в 1...
                    result = true;
                    switch (tempResult)
                    {
                        case 0:
                            system_state.full_observersability = false;
                            system_state.part_observersability = false;
                            break;
                        case 1:
                            system_state.full_observersability = true;
                            break;
                        case 2:
                            system_state.part_observersability = true;
                            break;
                        default: break;
                    }
                }
            }
            return result;
        }

        private int IsObserv_NonStationarity(ref State system_state, int k0 = 0)
        {
            //Пункт 3.2.3
            //Вызываем нашу функцию, передаём ей значения из system_state
            //Первый аргумен k0 - начальный момент времени. Пока дефолтим как о.
            if (system_state.P.Count == 0)
            {
                P.Add(0);
                return MainFunction3_2(k0, system_state.x, system_state.y, system_state.L, this.P, system_state.get_A(), /*system_state.H*/this.H);
            }
            else
                return MainFunction3_2(k0, system_state.x, system_state.y, system_state.L, system_state.P, system_state.get_A(), /*system_state.H*/this.H);

        }

        private int IsObserv_stationarity(ref State system_state)
        {
            //Пункт 3.3.3
            //Вызываем нашу функцию, передаём ей значения из system_state
            if (system_state.P.Count == 0)
            {
                P.Add(0);
                return MainFunction3_3(system_state.get_A(), /*system_state.allH()*/this.H, system_state.L, this.P);
            }
            else
                return MainFunction3_3(system_state.get_A(), /*system_state.allH()*/this.H, system_state.L, system_state.P);
        }

        //***********************************3.36****************************
        Matrix Function_F(int l, int k)
        {
            if (l == k || l < k)
            {
                Matrix In = new Matrix(n, n);
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        if (i == j)
                        {
                            In[i, j] = 1;
                        }

                return In;
            }
            else
            {
                Matrix res = A[l - 1][0];
                for (int i = l - 2; i >= k; i--)
                {
                    res = res * A[i][0];
                }
                return res;
            }
        }

        //***********************************3.40****************************
        Matrix Function_Sr(int k0, int N, int l)
        {
            try
            {
                int dl = d;

                for (int s = 0; s < d; s++)
                {
                    if (((l >= k0 + P[s]) && (l <= k0 + P[s + 1] - 1)) || ((l <= k0 + P[s]) && (l >= k0 + P[s + 1] - 1)))
                    {
                        if (N == k0 + P[s + 1] - 1)
                        {
                            dl = s;
                            break;
                        }
                    }
                }

                Matrix res = H[k0][0];

                Matrix mx1, mx2, mx3, mx4;

                mx1 = H[l][0] * Function_F(l - P[0], k0);

                for (int i = 1; i < dl; i++)
                {
                    mx1 += H[l][i] * Function_F(l - P[i], k0);
                }

                res = neco._3rdparty.Concatenate_Vert(res, mx1);

                mx2 = H[k0 + P[d - 2]][0] * Function_F(k0 + P[d - 1] + P[0] - 1, k0);

                for (int i = 1; i < d - 1; i++)
                {
                    mx2 += H[k0 + P[d - 2]][i] * Function_F(k0 + P[d - 1] + P[i] - 1, k0);
                }

                res = neco._3rdparty.Concatenate_Vert(res, mx2);

                mx3 = H[k0 + P[d - 1]][0] * Function_F(k0 + P[d - 1] + P[0], k0);

                for (int i = 1; i < d; i++)
                {
                    mx3 += H[k0 + P[d - 1]][i] * Function_F(k0 + P[d - 1] + P[i], k0);
                }

                res = neco._3rdparty.Concatenate_Vert(res, mx3);

                mx4 = H[N][0] * Function_F(N - P[0], k0);

                for (int i = 1; i < d; i++)
                {
                    mx4 = H[N][i] * Function_F(N - P[i], k0);
                }

                res = neco._3rdparty.Concatenate_Vert(res, mx4);

                tmpSr = res;
                return res;
            }
            catch (Exception ex)
            {
                return errorMatrix;
            }
        }

        //***********************************3.46****************************
        Matrix Function_Vr(int k0, int N, int l)
        {
            try
            {
                Matrix res = Matrix.Transpose(Function_Sr(k0, N, l));
                res = res * Function_Sr(k0, N, l);
                tmpVr = res;
                return res;
            }
            catch (Exception ex)
            {
                return errorMatrix;
            }
        }

        //***********************************3.70****************************
        Matrix Function_G(int l, int k0, int N)
        {
            try
            {
                int dl = d;

                for (int s = 0; s < d; s++)
                {
                    if (((l >= k0 + P[s]) && (l <= k0 + P[s + 1] - 1)) || ((l <= k0 + P[s]) && (l >= k0 + P[s + 1] - 1)))
                    {
                        if (N == k0 + P[s + 1] - 1)
                        {
                            dl = s;
                            break;
                        }
                    }
                }

                int ak = 0;

                for (int s = 0; s < a; s++)
                {
                    if (((l >= k0 - L[s]) && (l <= k0 - L[s - 1] - 1)) || ((l <= k0 - L[s]) && (l >= k0 - L[s - 1] - 1)))
                    {
                        ak = s;
                        break;
                    }
                }

                Matrix res = null;

                Matrix mx1, mx2, mx3;

                mx1 = H[l][0] * Function_F(l - P[0], k0 + 1) * A[k0][0];

                for (int j = 1; j < dl; j++)
                {
                    mx1 += H[l][j] * Function_F(l - P[j], k0 + 1) * A[k0][0];
                }

                mx1 = Matrix.Transpose(mx1);

                res = mx1;

                mx2 = H[l][0] * Function_F(l - P[0], k0 + 1) * A[k0 + L[ak]][0];

                for (int i = ak; i < a; i++)
                {
                    for (int j = 0; j < dl; j++)
                    {
                        if (i == ak && j == 0)
                        {
                            continue;
                        }
                        mx2 += H[l][j] * Function_F(l - P[j], k0 + 1) * A[k0 + L[i]][0];
                    }
                }

                mx2 = Matrix.Transpose(mx2);

                res = neco._3rdparty.Concatenate_Vert(res, mx2);

                mx3 = H[l][0] * Function_F(l - P[0], k0 + L[0]) * A[k0 + L[1] - 1][0];

                for (int i = 1; i < a; i++)
                {
                    for (int j = 0; j < dl; j++)
                    {
                        if (i == 1 && j == 0)
                        {
                            continue;
                        }
                        mx3 += H[l][j] * Function_F(l - P[j], k0 + L[j]) * A[k0 + L[i] - 1][0];
                    }
                }

                mx3 = Matrix.Transpose(mx3);

                res = neco._3rdparty.Concatenate_Vert(res, mx3);

                res = Matrix.Transpose(res);
                return res;
            }
            catch (Exception ex)
            {
                return errorMatrix;
            }
        }

        //3.71
        Matrix Function_R1(int k0, int N, int l)
        {
            try
            {
                Matrix res = null;

                Matrix mx1;

                mx1 = new Matrix(n * L[a - 1], p);

                res = mx1;

                for (int i = k0 + 1; i <= N; i++)
                {
                    res = neco._3rdparty.Concatenate_Horiz(res, Matrix.Transpose(Function_G(i, k0, N)));
                }

                res = Matrix.Transpose(res);

                return res;
            }
            catch (Exception ex)
            {
                return errorMatrix;
            }
        }

        //3.72
        Matrix Function_R2(int k0, int N, int l)
        {
            try
            {
                int J = 0;
                if (L[a - 1] > P[d - 1])
                {
                    J = L[a - 1];
                }
                else
                {
                    J = P[d - 1];
                }

                if (J == L[a - 1])
                {
                    return Function_R1(k0, N, l);
                }
                else
                {
                    Matrix res = null;

                    Matrix mx1 = new Matrix(p * (N - k0 + 1), n * (J - L[a - 1]));

                    Matrix mx2 = Function_R1(k0, N, l);

                    res = neco._3rdparty.Concatenate_Horiz(mx1, mx2);

                    return res;
                }
            }
            catch (Exception ex)
            {
                return errorMatrix;
            }
        }


        //3.73
        Matrix Function_R3(int k0, int N, int l)
        {
            try
            {
                int J = 0;
                if (L[a - 1] > P[d - 1])
                {
                    J = L[a - 1];
                }
                else
                {
                    J = P[d - 1];
                }

                if (J == L[a - 1])
                {
                    Matrix res = null;

                    Matrix mx1 = new Matrix(p * (N - k0 + 1), n * (J - P[d - 1]));

                    Matrix mx2 = Function_R(k0, N, l);

                    res = neco._3rdparty.Concatenate_Horiz(mx1, mx2);

                    return res;
                }
                else
                {
                    return Function_R(k0, N, l);
                }
            }
            catch (Exception ex)
            {
                return errorMatrix;
            }
        }

        //3.42
        Matrix Function_R(int k0, int N, int l)
        {
            try
            {
                List<List<Matrix>> R = new List<List<Matrix>>(N - k0 + 1);

                for (int i = 0; i < N - k0 + 1; i++)
                {
                    R.Add(new List<Matrix>(P[d - 1]));
                }

                for (int i = 0; i < N - k0 + 1; i++)
                {
                    for (int j = 0; j < P[d - 1]; j++)
                    {
                        R[i].Add(new Matrix(p, n));
                    }
                }

                for (int i = 0; i < P[d - 1]; i++)
                {
                    int di = 0;
                    for (int v = 0; v < d; v++)
                    {
                        if (((i >= P[v - 2] + 1) && (i <= P[v - 1])) || ((i <= P[v - 2] + 1) && (i >= P[v - 1])))
                        {
                            di = v;
                            break;
                        }
                    }

                    for (int s = di - 1; s < d; s++)
                    {
                        R[i][P[d - 1] - P[s + i]] = H[k0 + i - 1][s];
                    }
                }

                Matrix newR = null;

                for (int i = 0; i < N - k0 + 1; i++)
                {
                    Matrix tempRow = R[i][0];
                    for (int j = 1; j < P[d - 1]; j++)
                    {
                        tempRow = neco._3rdparty.Concatenate_Horiz(tempRow, R[i][j]);
                    }
                    if (i == 0)
                    {
                        newR = tempRow;
                    }
                    else
                    {
                        newR = neco._3rdparty.Concatenate_Vert(newR, tempRow);
                    }
                }

                return newR;
            }
            catch (Exception ex)
            {
                return errorMatrix;
            }
        }

        //3.74
        Matrix Function_qp(int k0, int N, int l)
        {
            try
            {
                return Function_R2(k0, N, l) + Function_R3(k0, N, l);
            }
            catch (Exception ex)
            {
                return errorMatrix;
            }
        }

        //3.82
        Matrix Function_Sa(int k0, int N, int l)
        {
            try
            {
                tmpSa = neco._3rdparty.Concatenate_Horiz(Function_qp(k0, N, l), Function_Sr(k0, N, l));
                return tmpSa;
            }
            catch (Exception ex)
            {
                return errorMatrix;
            }
        }

        //3.83
        Matrix Function_Va(int k0, int N, int l)
        {
            try
            {
                tmpVa = Matrix.Transpose(Function_Sa(k0, N, l)) * Function_Sa(k0, N, l);
                return tmpVa;
            }
            catch (Exception ex)
            {
                return errorMatrix;
            }
        }

        //3.86-3.87
        // -1 - ошибка
        // 0 - не наблюдаема
        // 1 - полностью наблюдаема
        // 2 - относительно наблюдаема
        public int MainFunction3_2(int k0, List<Matrix> x, List<Matrix> y, List<int> L, List<int> P, List<List<Matrix>> A, List<List<Matrix>> H)
        {
            try
            {
                this.x = x;
                this.y = y;
                this.L = L;
                this.P = P;
                this.A = A;
                this.H = H;

                int max_N = k0 + 1000;

                int J = 0;
                if (L[a - 1] > P[d - 1])
                {
                    J = L[a - 1];
                }
                else
                {
                    J = P[d - 1];
                }

                for (int N = k0 + 1; N <= max_N; N++)
                {
                    for (int l = k0 + 1; l <= N; l++)
                    {
                        if ((Matrix.Rank(Function_Sa(k0, N, l)) == n * (J + 1)) || (Matrix.Rank(Function_Va(k0, N, l)) == n * (J + 1)))
                        {
                            return 1;
                        }
                    }
                }

                for (int N = k0 + 1; N <= max_N; N++)
                {
                    for (int l = k0 + 1; l <= N; l++)
                    {
                        if ((Matrix.Rank(Function_Sr(k0, N, l)) == n) || (Matrix.Rank(Function_Vr(k0, N, l)) == n))
                        {
                            return 2;
                        }
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        ///************************************************************3.3*******************************************************

        // -1 - ошибка
        // 0 - не наблюдаема
        // 1 - полностью наблюдаема
        // 2 - относительно наблюдаема
        public int MainFunction3_3(List<List<Matrix>> A, List<List<Matrix>> H, List<int> L, List<int> P)
        {
            if (A[0].Count < L.Count || H[0].Count < P.Count)
            {
                return -1;
            }

            int n = H[0][0].NoCols;
            int p = H[0][0].NoRows;
            int a = L.Count - 1;
            int d = P.Count - 1;


            if (L[L.Count - 1] > P[P.Count - 1])
            {
                J = L[a];
            }
            else
            {
                J = P[d];
            }

            List<List<Matrix>> Va = new List<List<Matrix>>(n * (J + 1));

            for (int i = 0; i < n * (J + 1); i++)
            {
                Va.Add(new List<Matrix>(J + 1));
            }

            for (int i = 0; i < n * (J + 1); i++)
            {
                for (int j = 0; j < J + 1; j++)
                {
                    Va[i].Add(new Matrix(p, n));
                }
            }

            if (!(a == 0 && d == 0))
            {
                // ********* 3.125 ***************************************************
                if (J == L[a])
                {
                    // 1-я строка
                    for (int s = 0; s < P.Count; s++)
                    {
                        Va[0][J - P[s]] = H[0][s];
                    }

                    for (int i = 1; i < n * (J + 1); i++)
                    {
                        //2 строка
                        Va[i][0] = Va[i - 1][J] * A[0][a];

                        List<int> nonj = new List<int>();
                        for (int s = 0; s < a - 1; s++)
                        {
                            nonj.Add(J - L[s]);
                        }

                        //3 строка
                        for (int j = 1; j < J + 1; j++)
                        {
                            if (!nonj.Contains(j))
                            {
                                Va[i][j] = Va[i - 1][j - 1];
                            }
                        }

                        //4 строка
                        for (int s = 0; s < a; s++)
                        {
                            int j = J - L[s];

                            Va[i][j] = Va[i - 1][j - 1] + Va[i - 1][J] * A[0][s];
                        }
                    }
                }
                else // ********* 3.126 ***************************************************
                {
                    // 1-я строка
                    for (int s = 0; s < P.Count; s++)
                    {
                        Va[0][J - P[s]] = H[0][s];
                    }

                    //2 строка
                    //it is ok

                    List<int> nonj = new List<int>();
                    for (int s = 0; s < a - 1; s++)
                    {
                        nonj.Add(J - L[s]);
                    }

                    for (int i = 1; i < n * (J + 1); i++)
                    {
                        //3 строка
                        for (int j = 1; j < J + 1; j++)
                        {
                            if (!nonj.Contains(j))
                            {
                                Va[i][j] = Va[i - 1][j - 1];
                            }
                        }

                        //4 строка
                        for (int s = 0; s < a; s++)
                        {
                            int j = J - L[s];

                            Va[i][j] = Va[i - 1][j - 1] + Va[i - 1][J] * A[0][s];
                        }
                    }
                }
            }
            else
            {
                // ********* 3.129 ***************************************************
                if (J == L[a])
                {
                    // 1-я строка
                    // everithing is ok

                    //2 строка
                    Va[0][J - P[0]] = H[0][1];

                    //3 строка
                    Va[0][J] = H[0][0];

                    //4 строка
                    for (int i = 1; i < n * (J + 1); i++)
                    {
                        for (int j = 1; j < J; j++)
                        {
                            Va[i][j] = Va[i - 1][j - 1];
                        }

                        //5 строка
                        Va[i][J - L[0]] = Va[i - 1][J - 1] + Va[i - 1][J] * A[0][0];
                    }
                }
                else // ********* 3.130 ***************************************************
                {
                    // 1-я строка
                    Va[0][0] = H[0][1];
                    Va[0][J] = H[0][0];

                    // 2-я  и 3-я строка
                    // everithing is ok

                    for (int i = 1; i < n * (J + 1); i++)
                    {
                        //4 строка
                        if (J - L[0] > 1)
                        {
                            for (int j = 1; j < J - L[0]; j++)
                            {
                                Va[i][j] = Va[i - 1][j - 1];
                            }
                        }
                        else
                        {
                            Va[i][1] = Va[i - 1][0];
                        }

                        if (J > J - L[0] + 1)
                        {
                            for (int j = J - L[0] + 1; j < J; j++)
                            {
                                Va[i][j] = Va[i - 1][j - 1];
                            }
                        }
                        else
                        {
                            for (int j = J - 1; j < J - L[0] + 2; j++)
                            {
                                Va[i][j] = Va[i - 1][j - 1];
                            }
                        }

                        //5 строка
                        Va[i][J - 1] = Va[i - 1][J - L[0] - 1] + Va[i - 1][J] * A[0][1];

                        //6 строка
                        Va[i][J] = Va[i - 1][J - 1] + Va[i - 1][J] * A[0][0];
                    }
                }
            }

            Matrix newVa = null;

            for (int i = 0; i < n * (J + 1); i++)
            {
                
                Matrix tempRow = Va[i][0];
                for (int j = 1; j < J + 1; j++)
                {
                    tempRow = neco._3rdparty.Concatenate_Horiz(tempRow, Va[i][j]);
                }
                if (i == 0)
                {
                    newVa = tempRow;
                }
                else
                {
                    newVa = neco._3rdparty.Concatenate_Vert(newVa, tempRow);

                }
            }
            tmpVa = newVa;




            // ********* 3.131 ***************************************************
            List<List<Matrix>> Vr = new List<List<Matrix>>(n * (J + 1));

            for (int i = 0; i < n * (J + 1); i++)
            {
                Vr.Add(new List<Matrix>(1));
            }

            for (int i = 0; i < n * (J + 1); i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    Vr[i].Add(new Matrix(p, n));
                }
            }

            for (int i = 0; i < n * (J + 1); i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    Vr[i][j] = Va[i][J];
                }
            }

            Matrix newVr = null;

            for (int i = 0; i < n * (J + 1); i++)
            {
                Matrix tempRow = Vr[i][0];
                if (i == 0)
                {
                    newVr = tempRow;
                }
                else
                {
                    newVr = neco._3rdparty.Concatenate_Vert(newVr, tempRow);
                }
            }
            tmpVr = newVr;



            // ********* 3.127 ***************************************************
            if (Matrix.Rank(newVa) == n * (J + 1))
            {
                return 1;
            }
            // ********* 3.132 ***************************************************  
          


            if (Matrix.Rank(newVr) == n)
            {
                return 2;
            }

            return 0;
        }

    }
}
