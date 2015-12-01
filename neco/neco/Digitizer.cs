using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatrixLibrary;

namespace neco
{
    class Digitizer
    {
        State state;
        int eN = 15; // Длина ряда экспоненты
        int iStep = 100; // Шаг интегрирования

        private Digitizer() { ;}
        private Digitizer(State _state) {
            state = _state;
        }
        public static bool DigitizeSystem(ref State system_state)
        {
            bool flag = false;//флаг успешности выполнения
            Digitizer node = new Digitizer(system_state);//конструктор надо дописать по ходу дела...
            //ветвление по типам системы из State
            if (system_state.stationarity)//если система стационарна
            {
                if (system_state.synchronism)//система синхронная
                {
                    system_state = system_state.isSplainAppr ? node.Digit_Perm_Sync_Splain(ref system_state) 
                                                             : node.Digit_Perm_Sync(ref system_state);
                    flag = true;
                }
                else//асинхронная
                {
                    if (Convert.ToInt32(system_state.s) > 1)//с периодическим режимом съёма информации
                    {
                        system_state = system_state.isSplainAppr ? node.Digit_Perm_Async_X_Splain(ref system_state) 
                                                                 : node.Digit_Perm_Async_X(ref system_state);
                        flag = true;
                    }
                    if (Convert.ToInt32(system_state.s) < 1)//с периодической выдачей управляющих воздействий
                    {
                        system_state = system_state.isSplainAppr ? node.Digit_Perm_Async_U_Splain(ref system_state)
                                                                 : node.Digit_Perm_Async_U(ref system_state);
                        flag = true;
                    }
                }
            }
            return flag;//можно понять нашелся ли в ветвлении метод


        }

// ================================================================================================================== //

        //здесь была ваша реклама. сейчас она в _3rdparty

// ------------------------------------------------------------------------------------------------------- //

        //дискретизация стационарной синхронной системы
        private State Digit_Perm_Sync(ref State system_state)
        {
            //throw new NotImplementedException();
            int t = 0; // Система стационарная, значения не зависят от времени

            // Дискретизируем analog_A
            List<Matrix> tmpA = new List<Matrix>();
            List<int> tmpL = new List<int>();
            int sizeA = state.analog_A(0).NoRows;

            // A0
            tmpA.Add(_3rdparty.mExp(state.analog_A(0) * state.Tx, eN)); // A0
            tmpL.Add(0);
            for(int n = 1; n < state.analog_A()[t].Count; n++)
            {
                tmpL.Add(Convert.ToInt32((state.tau[n] / state.Tx) - 1)); // A1 (1.13)
                tmpL.Add(Convert.ToInt32(state.tau[n] / state.Tx)); // A2

                // A1
                Matrix A1 = new Matrix(sizeA, sizeA);
                double h = state.Tx / iStep;
                for (double v = 0; v < state.Tx - h; v += h)
                {
                    A1 = A1 + _3rdparty.mExp(state.analog_A(t, 0) * (state.Tx - v),eN) * v;
                }
                A1 = ( 1.0 / state.Tx) * A1 * h  * state.analog_A(t, n);
                tmpA.Add(A1);

                // A2
                Matrix A2 = new Matrix(sizeA, sizeA);
                for (double v = 0; v < state.Tx - h; v += h)
                {
                    A2 = A2 + _3rdparty.mExp(state.analog_A(t, 0) * (state.Tx - v),eN);
                }
                A2 = (A2 * h * state.analog_A(t, n)) - A1;
                tmpA.Add(A2);
            }
            // TODO: Складываем все A с одинаковыми L
            for (int i = 0; i < tmpA.Count; i++)
            {
                for (int j = i + 1; j < tmpA.Count; j++)
                {
                    if (tmpL[i] == tmpL[j])
                    {
                        tmpA[i] = tmpA[i] + tmpA[j];
                        tmpA.RemoveAt(i);
                        tmpL.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
            state.set_A(tmpA);
            state.L = tmpL;

            // Дискретизируем analog_B
            List<Matrix> tmpB = new List<Matrix>();
            List<int> tmpM = new List<int>();
            int sizeB = state.analog_B(t, 0).NoRows;

            // B0
            Matrix B0 = new Matrix(sizeB, sizeB);
            double hU = state.Tu / iStep;
            for (double v = 0; v < state.Tu - hU; v += hU)
            {
                B0 = B0 + _3rdparty.mExp(state.analog_A(t, 0) * (state.Tu - v), eN);
            }
            B0 = B0 * hU * state.analog_B(t, 0);
            tmpB.Add(B0);
            tmpM.Add(0);

            // B1
            Matrix B1 = new Matrix(sizeB, sizeB);
            for (int n = 1; n < state.analog_B()[t].Count; n++)
            {
                tmpM.Add(Convert.ToInt32(state.teta[n] / state.Tu)); // B1 (1.13)
                for (double v = 0; v < state.Tu - hU; v += hU)
                {
                    B1 = B1 + _3rdparty.mExp(state.analog_A(t, 0) * (state.Tu - v), eN);
                }
                B1 = B1 * hU * state.analog_B(t, n);
                tmpB.Add(B1);
            }
            state.set_B(tmpB);
            state.M = tmpM;
            return state;
        }

        //дискретизация стационарной асинхронной системы с периодическим режимом съёма информации
        private State Digit_Perm_Async_X(ref State system_state)
        {
    // ===================================== Копипаста ===================================== //
            int t = 0; // Система стационарная, значения не зависят от времени

            // Дискретизируем analog_A
            List<Matrix> tmpA = new List<Matrix>();
            List<int> tmpL = new List<int>();
            int sizeA = state.analog_A(t, 0).NoRows;

            // A0
            tmpA.Add( _3rdparty.mExp(state.analog_A(t, 0) * state.Tx, eN)); // A0
            tmpL.Add(0);
            for (int n = 1; n < state.analog_A()[t].Count; n++)
            {
                tmpL.Add(Convert.ToInt32((state.tau[n] / state.Tx) - 1)); // A1 (1.13)
                tmpL.Add(Convert.ToInt32(state.tau[n] / state.Tx)); // A2

                double hX = state.Tx / iStep;
                // A1
                Matrix A1 = new Matrix(sizeA, sizeA);
                for (double v = 0; v < state.Tx - hX; v += hX)
                {
                    A1 = A1 + _3rdparty.mExp(state.analog_A(t, 0) * (state.Tx - v),eN);
                }
                A1 = (A1 * hX / state.Tx) * state.analog_A(t, n);
                tmpA.Add(A1);

                // A2
                Matrix A2 = new Matrix(sizeA, sizeA);
                for (double v = 0; v < state.Tx - hX; v += hX)
                {
                    A2 = A2 + _3rdparty.mExp(state.analog_A(t, 0) * (state.Tx - v), eN);
                }
                A2 = (A2 * hX * state.analog_A(t, n)) - A1;
                tmpA.Add(A2);
            }
            // TODO: Складываем все A с одинаковыми L
            for (int i = 0; i < tmpA.Count; i++)
            {
                for (int j = i + 1; j < tmpA.Count; j++)
                {
                    if (tmpL[i] == tmpL[j])
                    {
                        tmpA[i] = tmpA[i] + tmpA[j];
                        tmpA.RemoveAt(i);
                        tmpL.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
            state.set_A(tmpA);
            state.L = tmpL;
    // ===================================== Копипаста ===================================== //
            // Дискретизируем analog_B
            List<Matrix> tmpB = new List<Matrix>();
            List<int> tmpM = new List<int>();
            int sizeB = state.B(t, 0).NoRows;

            // B0
            for (int r = 0; r < state.su - 1; r++)
            {
                double hU = ((r + 1) * state.Tu / state.su) / iStep;
                Matrix B0 = new Matrix(sizeB, sizeB);
                tmpM.Add(Convert.ToInt32(r / state.su)); // B1 (1.13)
                for (double v = r * state.Tu / state.su; v <= (r + 1) * state.Tu / state.su; v += hU)
                {
                    B0 = B0 + _3rdparty.mExp(state.analog_A(t, 0) * (state.Tu - v), eN);
                }
                B0 = B0 * hU * state.analog_B(t, 0);
                tmpB.Add(B0);
            }

            // B1
            Matrix B1 = new Matrix(sizeB, sizeB);
            for (int n = 1; n < state.analog_B()[t].Count; n++)
            {
                for (int r = 0; r < state.su - 1; r++)
                {
                    double hU = ((r + 1) * state.Tu / state.su) / iStep;
                    tmpM.Add(Convert.ToInt32((state.teta[n] / state.Tu) - r / state.su)); // B1 (1.32)
                    for (double v = r * state.Tu / state.su; v <= (r + 1) * state.Tu / state.su; v += hU)
                    {
                        B1 = B1 + _3rdparty.mExp(state.analog_A(t, 0) * (state.Tu - v), eN);
                    }
                    B1 = B1 * hU * state.analog_B(t, n);
                    tmpB.Add(B1);
                }
            }
            state.set_B(tmpB);
            state.M = tmpM;
            return state;
        }

        //дискретизация стационарной асинхронной системы с периодической выдачей управления
        private State Digit_Perm_Async_U(ref State system_state)
        {
            int t = 0; // Система стационарная, значения не зависят от времени
            int r = 0; // Шаг

            // Дискретизируем analog_A
            List<Matrix> tmpA = new List<Matrix>();
            List<int> tmpL = new List<int>();
            int sizeA = state.analog_A(t, 0).NoRows;

            // A0
            tmpA.Add(_3rdparty.mExp(state.analog_A(t, 0) * ((state.Tx * r) / state.sx), eN)); // A0
            tmpL.Add(0);
            for (int n = 1; n < state.analog_A()[t].Count; n++)
            {
                tmpL.Add(Convert.ToInt32(state.tau[n] / state.Tx) - 1); // A1 (1.13)
                tmpL.Add(Convert.ToInt32(state.tau[n] / state.Tx)); // A2

                double hX = ((r + 1) * state.Tx / state.sx) / iStep;
                // A1
                Matrix A1 = new Matrix(sizeA, sizeA);
                for (double v = r * state.Tx / state.sx; v <= (r + 1) * state.Tx / state.sx; v += hX)
                {
                    A1 = A1 + _3rdparty.mExp(state.analog_A(t, 0) * (((r + 1) * state.Tx / state.sx) - v), eN);
                }
                A1 = ((A1 * hX * state.sx) / (state.Tx * (r + 1)) ) * state.analog_A(t, n);
                tmpA.Add(A1);

                // A2
                Matrix A2 = new Matrix(sizeA, sizeA);
                for (double v = (r * state.Tx / state.sx); v <= (r + 1) * state.Tx / state.sx; v += hX)
                {
                    A2 = A2 + _3rdparty.mExp(state.analog_A(t, 0) * (((r + 1) * state.Tx / state.sx) - v), eN);
                }
                A2 = (A2 * hX * state.analog_A(t, n)) - A1;
                tmpA.Add(A2);
            }
            // TODO: Складываем все A с одинаковыми L
            for (int i = 0; i < tmpA.Count; i++)
            {
                for (int j = i + 1; j < tmpA.Count; j++)
                {
                    if (tmpL[i] == tmpL[j])
                    {
                        tmpA[i] = tmpA[i] + tmpA[j];
                        tmpA.RemoveAt(i);
                        tmpL.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
            state.set_A(tmpA);
            state.L = tmpL;

            // Дискретизируем analog_B
            List<Matrix> tmpB = new List<Matrix>();
            List<int> tmpM = new List<int>();
            int sizeB = state.analog_B(t, 0).NoRows;

            Matrix B1 = new Matrix(sizeB, sizeB);
            for (int n = 0; n < state.analog_B()[t].Count; n++)
            {
                if (n == 0)
                    tmpM.Add(Convert.ToInt32(0));
                else
                    tmpM.Add(Convert.ToInt32(state.teta[n] / state.Tu)); // B1 (1.13)
                double hU = ((r + 1) * state.Tu / state.su) / iStep;
                for (double v = r * state.Tu / state.sx; v <= (r + 1) * state.Tu / state.su; v += hU)
                {
                    B1 = B1 + _3rdparty.mExp(state.analog_A(t, 0) * (state.Tu - v), eN);
                }
                B1 = B1 * hU * state.analog_B(t, n);
                tmpB.Add(B1);
            }
            state.set_B(tmpB);
            state.M = tmpM;
            return state;
        }

        //дискретизация стационарной синхронной системы
        private State Digit_Perm_Sync_Splain(ref State system_state)
        {
            int t = 0; // Система стационарная, значения не зависят от времени

            // Дискретизируем analog_A
            List<Matrix> tmpA = new List<Matrix>();
            List<int> tmpL = new List<int>();
            int sizeA = state.analog_A(0).NoRows;

            // A0
            tmpA.Add(_3rdparty.mExp(state.analog_A(0) * state.Tx, eN)); // A0
            tmpL.Add(0);
            for (int z = 1; z < state.analog_A()[t].Count; z++)
            {
                tmpL.Add(Convert.ToInt32((state.tau[z] / state.Tx) - 1)); // A1 (1.13)
                tmpL.Add(Convert.ToInt32(state.tau[z] / state.Tx)); // A2

                int i;
                // A1

                double h = state.Tx / iStep;
                Matrix[] f = new Matrix[iStep];
                int k = 0;

                double v = 0;
                while (k != iStep) // тут ручками переписан фикс тани для сплайнов
                {
                    v += h;
                    f[k] = _3rdparty.mExp(state.analog_A(t, 0) * (state.Tx - v), eN) * v;
                    k++;
                }
                Matrix A1 = new Matrix(sizeA, sizeA);
                Matrix A2 = new Matrix(sizeA, sizeA);
                Matrix tmp = new Matrix(sizeA, sizeA);
                int N = f.Length - 1;
                int n = N - 1;
                double[] alpha = new double[n];
                Matrix[] beta = new Matrix[n];
                Matrix[] c = new Matrix[n];
                alpha[0] = -1 / 4;
                beta[0] = f[2] - 2 * f[1] + f[0];
                //метод прогонки, прямой ход
                for (i = 1; i < n; i++)
                {
                    alpha[i] = -1 / (alpha[i - 1] + 4);
                    beta[i] = (f[i + 2] - 2 * f[i + 1] + f[i] - beta[i - 1]) / (alpha[i - 1] + 4);
                }

                c[n - 1] = (f[N] - 2 * f[N - 1] + f[N - 2] - beta[n - 1]) / (4 + alpha[n - 1]);
                //обратный ход
                for (i = n - 2; i >= 0; i--)
                {
                    c[i] = alpha[i + 1] * c[i + 1] + beta[i + 1];
                }

                for (i = 0; i < n; i++) c[i] = c[i] * 3 / (state.tau[z] * state.tau[z]);

                //считаем приближенное значение интеграла по формуле (9):
                A2 = (5 * f[0] + 13 * f[1] + 13 * f[N - 1] + 5 * f[N]) / 12;
                for (i = 2; i < N - 1; i++) tmp = tmp + f[i];
                A2 = (A2 + tmp) * state.tau[z] - (c[0] + c[n - 1]) * state.tau[z] * state.tau[z] * state.tau[z] / 36;

                //считаем приближенное значение интеграла по формуле (6):
                tmp = new Matrix(sizeA, sizeA);
                A2 = (f[0] + f[N]) / 2;
                for (i = 1; i < N; i++) tmp = tmp + f[i];
                A2 = (A2 + tmp) * state.tau[z];
                tmp = new Matrix(sizeA, sizeA);
                for (i = 0; i < n; i++) tmp = tmp + c[i];
                tmp = tmp * state.tau[z] * state.tau[z] * state.tau[z] / 6;
                A1 = (1.0 / state.Tx) * A2 * h * state.analog_A(t, z);
                tmpA.Add(A1);

                // A2
                A2 = (A2 * h * state.analog_A(t, z)) - A1;
                tmpA.Add(A2);
            }
            // TODO: Складываем все A с одинаковыми L
            for (int i = 0; i < tmpA.Count; i++)
            {
                for (int j = i + 1; j < tmpA.Count; j++)
                {
                    if (tmpL[i] == tmpL[j])
                    {
                        tmpA[i] = tmpA[i] + tmpA[j];
                        tmpA.RemoveAt(i);
                        tmpL.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
            state.set_A(tmpA);
            state.L = tmpL;

            // Дискретизируем analog_B
            List<Matrix> tmpB = new List<Matrix>();
            List<int> tmpM = new List<int>();
            int sizeB = state.analog_B(t, 0).NoRows;

            // B0
            Matrix B0 = new Matrix(sizeB, sizeB);
            double hU = state.Tu / iStep;
            for (double v = 0; v < state.Tu - hU; v += hU)
            {
                B0 = B0 + _3rdparty.mExp(state.analog_A(t, 0) * (state.Tu - v), eN);
            }
            B0 = B0 * hU * state.analog_B(t, 0);
            tmpB.Add(B0);
            tmpM.Add(0);

            // B1
            Matrix B1 = new Matrix(sizeB, sizeB);
            for (int n = 1; n < state.analog_B()[t].Count; n++)
            {
                tmpM.Add(Convert.ToInt32(state.teta[n] / state.Tu)); // B1 (1.13)
                for (double v = 0; v < state.Tu - hU; v += hU)
                {
                    B1 = B1 + _3rdparty.mExp(state.analog_A(t, 0) * (state.Tu - v), eN);
                }
                B1 = B1 * hU * state.analog_B(t, n);
                tmpB.Add(B1);
            }
            state.set_B(tmpB);
            state.M = tmpM;
            return state;
        }

        //дискретизация стационарной асинхронной системы с периодическим режимом съёма информации
        private State Digit_Perm_Async_X_Splain(ref State system_state)
        {
            // ===================================== Копипаста ===================================== //
            int t = 0; // Система стационарная, значения не зависят от времени

            // Дискретизируем analog_A
            List<Matrix> tmpA = new List<Matrix>();
            List<int> tmpL = new List<int>();
            int sizeA = state.analog_A(t, 0).NoRows;

            // A0
            tmpA.Add(_3rdparty.mExp(state.analog_A(t, 0) * state.Tx, eN)); // A0
            tmpL.Add(0);
            for (int z = 1; z < state.analog_A()[t].Count; z++)
            {
                tmpL.Add(Convert.ToInt32((state.tau[z] / state.Tx) - 1)); // A1 (1.13)
                tmpL.Add(Convert.ToInt32(state.tau[z] / state.Tx)); // A2

                double hX = state.Tx / iStep;
                int i;
                // A1
                Matrix[] f = new Matrix[iStep];
                int k = 0;

                double v = 0;
                while (k != iStep)   // Аналогично Танин фикс ручками (for --> while)
                {
                    v += hX;
                    f[k] = _3rdparty.mExp(state.analog_A(t, 0) * (state.Tx - v), eN);
                    k++;
                }
                Matrix A1 = new Matrix(sizeA, sizeA);
                Matrix A2 = new Matrix(sizeA, sizeA);
                Matrix tmp = new Matrix(sizeA, sizeA);
                int N = f.Length - 1;
                int n = N - 1;
                double[] alpha = new double[n];
                Matrix[] beta = new Matrix[n];
                Matrix[] c = new Matrix[n];
                alpha[0] = -1 / 4;
                beta[0] = f[2] - 2 * f[1] + f[0];
                //метод прогонки, прямой ход
                for (i = 1; i < n; i++)
                {
                    alpha[i] = -1 / (alpha[i - 1] + 4);
                    beta[i] = (f[i + 2] - 2 * f[i + 1] + f[i] - beta[i - 1]) / (alpha[i - 1] + 4);
                }

                c[n - 1] = (f[N] - 2 * f[N - 1] + f[N - 2] - beta[n - 1]) / (4 + alpha[n - 1]);
                //обратный ход
                for (i = n - 2; i >= 0; i--)
                {
                    c[i] = alpha[i + 1] * c[i + 1] + beta[i + 1];
                }

                for (i = 0; i < n; i++) c[i] = c[i] * 3 / (state.tau[z] * state.tau[z]);

                //считаем приближенное значение интеграла по формуле (9):
                A2 = (5 * f[0] + 13 * f[1] + 13 * f[N - 1] + 5 * f[N]) / 12;
                for (i = 2; i < N - 1; i++) tmp = tmp + f[i];
                A2 = (A2 + tmp) * state.tau[z] - (c[0] + c[n - 1]) * state.tau[z] * state.tau[z] * state.tau[z] / 36;

                //считаем приближенное значение интеграла по формуле (6):
                tmp = new Matrix(sizeA, sizeA);
                A2 = (f[0] + f[N]) / 2;
                for (i = 1; i < N; i++) tmp = tmp + f[i];
                A2 = (A2 + tmp) * state.tau[z];
                tmp = new Matrix(sizeA, sizeA);
                for (i = 0; i < n; i++) tmp = tmp + c[i];
                tmp = tmp * state.tau[z] * state.tau[z] * state.tau[z] / 6;
                A1 = (A2 * hX / state.Tx) * state.analog_A(t, z);
                tmpA.Add(A1);

                // A2
                A2 = (A2 * hX * state.analog_A(t, z)) - A1;
                tmpA.Add(A2);
            }
            // TODO: Складываем все A с одинаковыми L
            for (int i = 0; i < tmpA.Count; i++)
            {
                for (int j = i + 1; j < tmpA.Count; j++)
                {
                    if (tmpL[i] == tmpL[j])
                    {
                        tmpA[i] = tmpA[i] + tmpA[j];
                        tmpA.RemoveAt(i);
                        tmpL.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
            state.set_A(tmpA);
            state.L = tmpL;
            // ===================================== Копипаста ===================================== //
            // Дискретизируем analog_B
            List<Matrix> tmpB = new List<Matrix>();
            List<int> tmpM = new List<int>();
            int sizeB = state.B(t, 0).NoRows;

            // B0
            for (int r = 0; r < state.su - 1; r++)
            {
                double hU = ((r + 1) * state.Tu / state.su) / iStep;
                Matrix B0 = new Matrix(sizeB, sizeB);
                tmpM.Add(Convert.ToInt32(r / state.su)); // B1 (1.13)
                for (double v = r * state.Tu / state.su; v <= (r + 1) * state.Tu / state.su; v += hU)
                {
                    B0 = B0 + _3rdparty.mExp(state.analog_A(t, 0) * (state.Tu - v), eN);
                }
                B0 = B0 * hU * state.analog_B(t, 0);
                tmpB.Add(B0);
            }

            // B1
            Matrix B1 = new Matrix(sizeB, sizeB);
            for (int n = 1; n < state.analog_B()[t].Count; n++)
            {
                for (int r = 0; r < state.su - 1; r++)
                {
                    double hU = ((r + 1) * state.Tu / state.su) / iStep;
                    tmpM.Add(Convert.ToInt32((state.teta[n] / state.Tu) - r / state.su)); // B1 (1.32)
                    for (double v = r * state.Tu / state.su; v <= (r + 1) * state.Tu / state.su; v += hU)
                    {
                        B1 = B1 + _3rdparty.mExp(state.analog_A(t, 0) * (state.Tu - v), eN);
                    }
                    B1 = B1 * hU * state.analog_B(t, n);
                    tmpB.Add(B1);
                }
            }
            state.set_B(tmpB);
            state.M = tmpM;
            return state;
        }

        //дискретизация стационарной асинхронной системы с периодической выдачей управления
        private State Digit_Perm_Async_U_Splain(ref State system_state)
        {
            int t = 0; // Система стационарная, значения не зависят от времени
            int r = 0; // Шаг

            // Дискретизируем analog_A
            List<Matrix> tmpA = new List<Matrix>();
            List<int> tmpL = new List<int>();
            int sizeA = state.analog_A(t, 0).NoRows;

            // A0
            tmpA.Add(_3rdparty.mExp(state.analog_A(t, 0) * ((state.Tx * r) / state.sx), eN)); // A0
            tmpL.Add(0);
            for (int z = 1; z < state.analog_A()[t].Count; z++)
            {
                tmpL.Add(Convert.ToInt32((state.tau[z] / state.Tx) - 1)); // A1 (1.13)
                tmpL.Add(Convert.ToInt32(state.tau[z] / state.Tx)); // A2

                double hX = ((r + 1) * state.Tx / state.sx) / iStep;
                int i;
                // A1
                Matrix[] f = new Matrix[iStep];
                int k = 0;
  
                double v = r * state.Tx / state.sx;
                while (k != iStep)
                {
                    v += hX;
                    f[k] = _3rdparty.mExp(state.analog_A(t, 0) * (((r + 1) * state.Tx / state.sx) - v), eN);
                    k++;
                }
                Matrix A1 = new Matrix(sizeA, sizeA);
                Matrix A2 = new Matrix(sizeA, sizeA);
                Matrix tmp = new Matrix(sizeA, sizeA);
                int N = f.Length - 1;
                int n = N - 1;
                double[] alpha = new double[n];
                Matrix[] beta = new Matrix[n];
                Matrix[] c = new Matrix[n];
                alpha[0] = -1 / 4;
                beta[0] = f[2] - 2 * f[1] + f[0];
                //метод прогонки, прямой ход
                for (i = 1; i < n; i++)
                {
                    alpha[i] = -1 / (alpha[i - 1] + 4);
                    beta[i] = (f[i + 2] - 2 * f[i + 1] + f[i] - beta[i - 1]) / (alpha[i - 1] + 4);
                }

                c[n - 1] = (f[N] - 2 * f[N - 1] + f[N - 2] - beta[n - 1]) / (4 + alpha[n - 1]);
                //обратный ход
                for (i = n - 2; i >= 0; i--)
                {
                    c[i] = alpha[i + 1] * c[i + 1] + beta[i + 1];
                }

                for (i = 0; i < n; i++) c[i] = c[i] * 3 / (state.tau[z] * state.tau[z]);

                //считаем приближенное значение интеграла по формуле (9):
                A2 = (5 * f[0] + 13 * f[1] + 13 * f[N - 1] + 5 * f[N]) / 12;
                for (i = 2; i < N - 1; i++) tmp = tmp + f[i];
                A2 = (A2 + tmp) * state.tau[z] - (c[0] + c[n - 1]) * state.tau[z] * state.tau[z] * state.tau[z] / 36;

                //считаем приближенное значение интеграла по формуле (6):
                tmp = new Matrix(sizeA, sizeA);
                A2 = (f[0] + f[N]) / 2;
                for (i = 1; i < N; i++) tmp = tmp + f[i];
                A2 = (A2 + tmp) * state.tau[z];
                tmp = new Matrix(sizeA, sizeA);
                for (i = 0; i < n; i++) tmp = tmp + c[i];
                tmp = tmp * state.tau[z] * state.tau[z] * state.tau[z] / 6;
                A1 = (A2 * hX * state.sx) / (state.Tx * (r + 1)) * state.analog_A(t, z);
                tmpA.Add(A1);

                // A2
                A2 = (A2 * hX * state.analog_A(t, z)) - A1;
                tmpA.Add(A2);
            }
            // TODO: Складываем все A с одинаковыми L
            for (int i = 0; i < tmpA.Count; i++)
            {
                for (int j = i + 1; j < tmpA.Count; j++)
                {
                    if (tmpL[i] == tmpL[j])
                    {
                        tmpA[i] = tmpA[i] + tmpA[j];
                        tmpA.RemoveAt(i);
                        tmpL.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
            state.set_A(tmpA);
            state.L = tmpL;

            // Дискретизируем analog_B
            List<Matrix> tmpB = new List<Matrix>();
            List<int> tmpM = new List<int>();
            int sizeB = state.analog_B(t, 0).NoRows;

            Matrix B1 = new Matrix(sizeB, sizeB);
            for (int n = 0; n < state.analog_B()[t].Count; n++)
            {
                if (n == 0)
                    tmpM.Add(Convert.ToInt32(0));
                else
                    tmpM.Add(Convert.ToInt32(state.teta[n] / state.Tu)); // B1 (1.13)
                double hU = ((r + 1) * state.Tu / state.su) / iStep;
                for (double v = r * state.Tu / state.sx; v <= (r + 1) * state.Tu / state.su; v += hU)
                {
                    B1 = B1 + _3rdparty.mExp(state.analog_A(t, 0) * (state.Tu - v), eN);
                }
                B1 = B1 * hU * state.analog_B(t, n);
                tmpB.Add(B1);
            }
            state.set_B(tmpB);
            state.M = tmpM;
            return state;
        }
    }
}
