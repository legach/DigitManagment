using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatrixLibrary;

namespace neco
{
    class Controllability
    {
        public static Matrix GR;
        public static Matrix G_;
        public static Matrix temp_GR;
        public static Matrix temp_G_;


        /// <summary>
        /// конструктор принимает system_state и инициализирует класс ниже
        /// </summary>
        /// <param name="system_state"></param>
        private Controllability(ref State system_state)
        {
            
        }
        
        /// <summary>
        /// главная функция рассчета управляемости системы. возвращает нашелся ли метод на тип системы, описанной в sysytem_state
        ///!!!Конструкторы не описаны - АБСТРАКТНЫЙ КОД!
        /// </summary>
        /// <param name="system_state"></param>
        /// <returns></returns>
        public static bool IsSystemControllability(ref State system_state)
        {
            //была ли определена управляемоть
            bool flag = false;
            //объект для инициализации в зависимости от типа системы
            Controllability node = new Controllability(ref system_state);

            if (system_state.synchronism)           //система синхронна
            {
                if (!system_state.stationarity)     //система не стационарная
                {
                    if (system_state.delay_by_state && system_state.delay_by_control)   //есть запаздывание по состоянию и управлению
                    {
                        node.IsContr_NonPerm_XU(ref system_state);                      //запись результата в State - состояние системы
                        flag = true;
                    }
                    if (system_state.delay_by_state && !system_state.delay_by_control)  //есть запаздывание по состоянию и НЕТ по управлению
                    {
                        node.IsContr_NonPerm_X(ref system_state);
                        flag = true;
                    }
                    if (!system_state.delay_by_state && system_state.delay_by_control)  //НЕТ запаздывания по состоянию и есть по управлению
                    {
                        node.IsContr_NonPerm_U(ref system_state);
                        flag = true;
                    }
                }

                else //система стационарна
                {
                    if (system_state.delay_by_state && system_state.delay_by_control)   //есть запаздывание по состоянию и по управлению
                    {
                        node.IsContr_Perm_XU(ref system_state);
                        flag = true;
                    }
                    if (system_state.delay_by_state && !system_state.delay_by_control)  //есть запаздывание по состоянию и НЕТ по управлению
                    {
                        node.IsContr_Perm_X(ref system_state);
                        flag = true;
                    }
                    if (!system_state.delay_by_state && system_state.delay_by_control) //НЕТ запаздывания по состоянию и есть по управлению
                    {
                        node.IsContr_Perm_U(ref system_state);
                        flag = true;
                    }
                }
            }
            else    //система асинхронна - пока хз. в книге написано что приводятся к синхронным.
            {
                flag = false;
            }
            return flag;
        }

        
        /// <summary>
        /// Управляемость стац системы с обоими запаздываниями
        /// </summary>
        /// <param name="system_state"></param>
        private void IsContr_Perm_XU(ref State system_state)
        {
            // реализация на основе нестационарной системы
            //State NonPermState = new State(system_state.x, system_state.u, system_state.analog_A(), system_state.analog_B(), system_state.allH(), system_state.tau, system_state.teta, system_state.allFi(), system_state.allPsi(), system_state.Tx, system_state.Tu, false, system_state.determinancy, system_state.perturbation);
            //IsContr_NonPerm_XU(ref NonPermState);
            IsContr_NonPerm_XU(ref system_state);
            //system_state.full_controllability = NonPermState.full_controllability;
            //system_state.part_controllability = NonPermState.part_controllability;
        }

        /// <summary>
        /// Управляемость стац. системы с запаздыванием по состоянию
        /// </summary>
        /// <param name="system_state"></param>
        private void IsContr_Perm_X(ref State system_state)
        {
            //реализация на основе общего случая по задержкам
            this.IsContr_Perm_XU(ref system_state);
        }

        /// <summary>
        /// Управляемость стац. системы с запаздыванием по управлению
        /// </summary>
        /// <param name="system_state"></param>
        private void IsContr_Perm_U(ref State system_state)
        {
            //реализация на основе общего случая по задержкам
            this.IsContr_Perm_XU(ref system_state);
        }

        /// <summary>
        /// Условие проверки на существование задержки по управлению в массиве. (j == system_state.M[s] + 1)
        /// </summary>
        /// <param name="system_state"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        private bool condition_1(State system_state, int j)
        {
            for (int s = 1; s < system_state.c; s++)
            {
                if (j == system_state.M[s] + 1)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Усправляемость нестац системы с обоими запаздываниями
        /// </summary>
        /// <param name="system_state"></param>
        private void IsContr_NonPerm_XU(ref State system_state)
        {
            int a = system_state.a;
            int c = system_state.c;
            int n = system_state.n;
            int m = system_state.m;
            int La = system_state.L[system_state.a - 1];//последяя задержка по состоянию
            int Mc = system_state.M[system_state.c - 1];//последняя задержка по управлению
            int nu = n * (La + 1);
            int size_i = La + 1;
            int size_j = n * (La + 1) + Mc;
            Matrix res = new Matrix(n, m);
            //Список всех необходимых выржений дальше
            List<List<Matrix>> array_V = new List<List<Matrix>>();
            // Забиваем нулями
            for (int i = 0; i < size_i; i++)
            {
                List<Matrix> tmpI = new List<Matrix>();
                for (int j = 0; j < size_j; j++)
                {
                    tmpI.Add(new Matrix(system_state.n, system_state.m));
                }
                array_V.Add(tmpI);
            }
            //2.120
            for (int j = 0; j < size_j; j++)
            {
                for (int i = 0; i < size_i; i++)
                {
                    // Условие 1
                    if (i >= 0 && i <= La - 1 && j == 0)
                    {
                        array_V[i][0] = new Matrix(n, m);
                    }
                    // Условие 2
                    if (i == (La + 1) - 1 && j == 0)
                    {
                        array_V[i][0] = system_state.B(0);
                    }
                    // Условие 3
                    if (i >= 0 && i <= La - 1 && j >= 1 && j <= nu + Mc - 1)
                    {
                        array_V[i][j] = array_V[i + 1][j - 1];
                    }
                    // Условие 4
                    if (i == (La + 1) - 1 && j >= 1 && j <= nu + Mc - 1 && condition_1(system_state, j))
                    {
                        Matrix tmpV = new Matrix(n, m);
                        for (int k = 0; k < a; k++)
                        {
                            if (condition_1(system_state, j))
                            {
                                tmpV += system_state.get_A(k) * array_V[(La - system_state.L[k] + 1) - 1][j - 1];
                            }
                        }
                        array_V[(La + 1) - 1][j] = tmpV;
                    }
                    // Условие 5
                    if (system_state.M.Count > 1)
                    {
                        if (i == (La + 1) - 1)
                        {
                            for (int s = 0; s < c; s++)
                            {
                                if (j == (system_state.M[s] + 1))
                                {
                                    Matrix tmpV = new Matrix(n, m);
                                    for (int k = 0; k < a; k++)
                                    {
                                        tmpV += system_state.get_A(k) * array_V[(La + system_state.L[k] + 1) - 1][j - 1];
                                    }
                                    array_V[(La + 1) - 1][j] = tmpV + system_state.B(s);
                                }
                            }
                        }
                    }
                }
            }

            //Vr определяетс по выражениям (2.121) и (2.120).
            Matrix Vr = array_V[(La + 1) - 1][0];
            for (int i = 1; i <= nu + Mc - 1; i++)
            {
                Vr = _3rdparty.Concatenate_Horiz(Vr, array_V[(La + 1) - 1][i]);
            }

            //Va определена как (2.127)
            Matrix Va = array_V[0][0];
            for (int j = 1; j < n * (La + 1); j++)
            {
                Va = _3rdparty.Concatenate_Horiz(Va, array_V[0][j]);
            }
            for (int i = 1; i < La + 1; i++)
            {
                Matrix tmp = array_V[i][0];
                for (int j = 1; j < n * (La + 1); j++)
                {
                    tmp = _3rdparty.Concatenate_Horiz(tmp, array_V[i][j]);
                }
                Va = _3rdparty.Concatenate_Vert(Va, tmp);
            }


            //Проверка условий
            if (Matrix.Rank(Va) == n * (La + 1))//(2.126)
            {
                system_state.full_controllability = true;
            }
            else
            {
                system_state.full_controllability = false;
                if (Matrix.Rank(Vr) == n)//(2.122)
                {
                    system_state.part_controllability = true;
                }
                else
                {
                    system_state.part_controllability = false;
                }
            }
            GR = Vr;
            G_ = Va;
        }


        //Усправляемость нестац системы с обоими запаздываниями
        //private void IsContr_NonPerm_XU(ref State system_state)
        //{
        //    //больше брать нельзя из-за F
        //    int N = system_state.L[system_state.a - 1];//+k0\
        //    //условие относительной управляемости 2.61 стр.50
        //    GR = Gr(ref system_state, N);
        //    G_ = _G(ref system_state, N);
        //    var tempGrQr = _3rdparty.Concatenate_Horiz(G(ref system_state, N, 0), Qr(ref system_state, N));
        //    temp_GR = _3rdparty.Concatenate_Horiz(tempGrQr, Gamma(ref system_state, N));
        //    if (Matrix.Rank(Gr(ref system_state, N).toArray) == Matrix.Rank(_3rdparty.Concatenate_Horiz(_3rdparty.Concatenate_Horiz(G(ref system_state, N, 0), Qr(ref system_state, N)), Gamma(ref system_state, N))))
        //        system_state.part_controllability = true;
        //    else
        //        system_state.part_controllability = false;
        //    //system_state.part_controllability = true; // TODO: Temporal stub

        //    //условие полной управляемости 2.69 стр.52
        //    temp_G_ = _3rdparty.Concatenate_Horiz(_3rdparty.Concatenate_Horiz(_G(ref system_state, N), Qa(ref system_state, N)), GammaA(ref system_state, N));

        //    if (Matrix.Rank(_G(ref system_state, N).toArray) == Matrix.Rank(_3rdparty.Concatenate_Horiz(_3rdparty.Concatenate_Horiz(_G(ref system_state, N), Qa(ref system_state, N)), GammaA(ref system_state, N))))
        //        system_state.full_controllability = true;
        //    else
        //        system_state.full_controllability = false;
        //}

        /// <summary>
        /// Управляемость нестац. системы с запаздыванием по состоянию
        /// </summary>
        /// <param name="system_state"></param>
        private void IsContr_NonPerm_X(ref State system_state)
        {
            IsContr_NonPerm_XU(ref system_state);
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Управляемость нестац. системы с запаздыванием по управлению
        /// </summary>
        /// <param name="system_state"></param>
        private void IsContr_NonPerm_U(ref State system_state)
        {
            IsContr_NonPerm_XU(ref system_state);
            //throw new NotImplementedException();
        }

        /// <summary>
        /// рассчет матрицы формула 2.8 стр. 36                                           РЕКУРСИЯ, НЕ ОТЛАЖЕНО
        /// </summary>
        /// <param name="sys_state"></param>
        /// <param name="N"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        static Matrix F(ref State sys_state, int N, int k)
        {
            if (N == k)
                return new Matrix(Matrix.Identity(sys_state.n));
            else
            {
                Matrix tmp = new Matrix(sys_state.n,sys_state.n);//матрица для суммирования на выход.
                for (int i = 1; i < sys_state.a; i++)//проход по L с поиском на каком промежутке k
                    if (k >= N - sys_state.L[i] && k <= N - sys_state.L[i - 1] - 1)
                    {

                        for (int j = 0; j < i; j++)
                            tmp += F(ref sys_state, N, k + sys_state.L[j] + 1) * sys_state.get_A(k,j);
                    }
                return tmp;
            }
        }
        
        /// <summary>
        /// рассчет всего для Qa Qr(N; k0)
        ///формула 2.12, стр 37 - дикая неразбериха с индексами. Циклы перенесены с формулы 2.9
        /// </summary>
        /// <param name="sys_state"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        static Matrix Qr(ref State sys_state, int N)//k0 = 0
        {
            Matrix rez = F(ref sys_state, N, 0);
            for (int k = -sys_state.L[sys_state.a - 1]; k < 0; k++)
            {
                Matrix tmp = new Matrix(sys_state.n,sys_state.n);
                for (int ak=1;ak<sys_state.a;ak++)
                    if (k >= - sys_state.L[ak] && k <= sys_state.L[ak - 1] - 1)
                    {
                        for (int i = ak; i < sys_state.a; i++)
                        {
                            tmp += F(ref sys_state, N, k + sys_state.L[i] + 1) * sys_state.get_A(k + sys_state.L[i], i);
                        }
                        break;
                    }
                rez = _3rdparty.Concatenate_Horiz(rez, tmp);
            }
            return rez;
        }

        /// <summary>
        /// формула 2.21, стр. 40 Qa(N; k0)
        /// </summary>
        /// <param name="sys_state"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        static Matrix Qa(ref State sys_state, int N)//k0 = 0
        {
            Matrix rez = Qr(ref sys_state, N - sys_state.L[sys_state.a-1]);
            for (int i = sys_state.L[sys_state.a-1] - 1; i >= 0 ;i--)
            {
                rez = _3rdparty.Concatenate_Vert(rez, Qr(ref sys_state, N - i));
            }
            return rez;
        }
        /// <summary>
        /// рассчет всего для GammaA Г(N; k0)
        ///формула 2.37, стр.44 - дикая неразбериха с индексами. Циклы перенесены с формулы 2.34
        /// </summary>
        /// <param name="sys_state"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        static Matrix Gamma(ref State sys_state, int N)
        {
            Matrix rez = F(ref sys_state, N, 1) * sys_state.B(0, sys_state.c - 1);
            for (int k = -sys_state.M[sys_state.c - 1] + 1; k < 0; k++)
            {
                Matrix tmp = new Matrix(sys_state.n, sys_state.m);
                for (int j = 1; j < sys_state.c; j++)
                {
                    if (k <= -sys_state.M[j] && k >= -sys_state.M[j - 1] - 1)
                    {
                        for (int i = j; i < sys_state.c; i++)
                            tmp += F(ref sys_state, N, k + sys_state.M[i] + 1) * sys_state.B(k + sys_state.M[i], i);
                        break;
                    }
                }
                rez = _3rdparty.Concatenate_Horiz(rez, tmp);
            }
            return rez;
        }
        /// <summary>
        /// формула 2.67, стр.51 Гa(N; k0)
        /// </summary>
        /// <param name="sys_state"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        static Matrix GammaA(ref State sys_state, int N)
        {
            Matrix rez = Gamma(ref sys_state, N);
            for (int i = sys_state.L[sys_state.a - 1] - 1; i >= 0; i--)
            {
                rez = _3rdparty.Concatenate_Vert(rez, Gamma(ref sys_state, N - i));
            }
            return rez;
        }

        /// <summary>
        /// рассчет всего для G Gr(N; k0)
        ///формула 2.38, стр.45 - дикая неразбериха с индексами. Циклы перенесены с формулы 2.34
        /// </summary>
        /// <param name="sys_state"></param>
        /// <param name="N">N</param>
        /// <returns></returns>
        static Matrix Gr(ref State sys_state, int N)
        {
            Matrix rez = new Matrix(sys_state.n,sys_state.n);
            for (int i = 0; i < sys_state.c; i++)
            {
                rez += F(ref sys_state, N, sys_state.M[i] + 1);
            }
            for (int k = 0; k <= N - 2; k++)
            {
                Matrix tmp = new Matrix(sys_state.n, sys_state.m);
                for (int j = 0; j < sys_state.c; j++)
                    if ( sys_state.c == 1 || (k >= N - sys_state.M[j + 1] && k <= N - sys_state.M[j] - 1))
                        for (int i = 0; i < j; i++)
                            tmp += F(ref sys_state, N, k + sys_state.M[i] + 1) * sys_state.B(k + sys_state.M[i],i);
                rez = _3rdparty.Concatenate_Horiz(rez, tmp);
            }

            return rez;
        }
        /// <summary>
        /// формула 2.22, стр.40 - Здесь N и l - 2 ОТдельных паарметра. G(N - l; k0)
        /// </summary>
        /// <param name="sys_state"></param>
        /// <param name="N">N</param>
        /// <param name="l">l</param>
        /// <returns></returns>
        static Matrix G(ref State sys_state, int N, int l)
        {
            //if (N == l)
            //    return Gr(ref sys_state, N - l);
            //// Вызов от таких параметров не имеет смысла, конкатенация выполняется не верно.
            //else
//            if(0<=l && l<=sys_state.a)
                return _3rdparty.Concatenate_Horiz(Gr(ref sys_state, N-l), new Matrix(sys_state.n, sys_state.m * l));

        }
        /// <summary>
        /// формула 2.23, стр.40 Ga(N; k0)
        /// </summary>
        /// <param name="sys_state"></param>
        /// <param name="N">N</param>
        /// <returns></returns>
        static Matrix Ga(ref State sys_state, int N)
        {
            Matrix rez = G(ref sys_state, N, sys_state.L[sys_state.a - 1]);
            for (int i = sys_state.L[sys_state.a - 1] - 1; i >= 0; i--)
                rez = _3rdparty.Concatenate_Vert(rez, G(ref sys_state, N - i, i));//не проверены размерности сливания. но это всплывет если что сразу.
            return rez;
        }
        /// <summary>
        /// формулы нет, стр.52
        /// </summary>
        /// <param name="sys_state"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        static Matrix _G(ref State sys_state, int N)
        {
            return _3rdparty.Cut(Ga(ref sys_state, N), sys_state.n * (sys_state.L[sys_state.a - 1] + 1), sys_state.m * (N - sys_state.M[sys_state.c - 1]));
        }
    }
}
