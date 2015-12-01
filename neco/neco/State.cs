using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatrixLibrary;
using System.Runtime.Serialization;

namespace neco
{
    //класс состояния системы.
    class State
    {
#region переменные
        //во вложенных списках внешний - временная шкала, внутренний - шкала запаздываний. 
        //в списках 0-й элемент считается текущим, 1-й - предыдущим и т.д. Т.е. для добавления следующего элемента необходимо вставить нулевой элемент в список.


        //отсчеты векторов состояния и управления
        public List<Matrix> x;//размерность каждого элемента списка n x 1. это отсчеты состояний системы.
        public List<Matrix> u;//размерность каждого элемента списка m x 1. это отсчеты управляющих воздействий. 0-й элемент занулен, кроме регулятора, где он считается, а потом сдвигается время

        //матрицы перевода x в y - матрицы p x n
        List<Matrix> int_H;
        
        //непрерывные матрицы-описатели системы
        List<List<Matrix>> int_analog_A;//размерность матриц - n x n (n - размерность х), запаздывание длинной L, время длинной от L и возрастает по мере рассчета
        List<List<Matrix>> int_analog_B;//размерность матриц - m x m (m - размерность u), запаздывание длинной M, время длинной от M и возрастает по мере рассчета
        //дискретезированные матрицы - формат как у непрерывных, только они являются результатом работы класса Digitizer
        List<List<Matrix>> int_A;
        List<List<Matrix>> int_B;

        //массивы непрерывных задержек. НЕ лист листов, т.к. при изменении А задержки все те-же, т.е. другой массив А накладывается на так-же отдаленные от текущего t отсчеты состояний.
        public List<double> tau;//по состоянию
        public List<double> teta;//по управлению

        //массивы дискретных задержек.
        public List<int> L;//по состоянию
        public List<int> M;//по управлени
        //############################################
        public List<int> P;//по наблюдению
        //############################################
        
        //периоды дискретизации
        public double T;//по состоянию
        public double sx;
        public double su;
        public double Tx;//по состоянию
        public double Tu;//по управлению

        //значения критерия оптимальности. рассчитывается в Regulator
        public List<double> quality_f;

        public bool isSplainAppr;
        public bool isRandomEffect;

        //матрицы для рассчета критерия оптимальности (для теста брать единичные, предусмотреть изменение в процессе работы)
        //оказывацца зависят от времени
        List<Matrix> int_Fi;
        List<Matrix> int_Psi;
        #endregion
#region свойства
        //Выходные данные системы y размерности p x 1
        public List<Matrix> y
        {
            get
            {
                List<Matrix> tmp = new List<Matrix>();
                for (int i = 0; i < this.x.Count; i++)
                {
                    Matrix aaa = H(i) * x[i];
                    Matrix m = _3rdparty.AWGN_generator(aaa.NoCols, aaa.NoRows);

                    if (isRandomEffect)
                        tmp.Add(H(i) * x[i] + m); // ИСПРАВИТЬ ДЛЯ НОВОГО H (строчка, а не матрица) !!!
                    else
                        tmp.Add(H(i) * x[i]);
                }
                return tmp;
            }
        }

        //массивы дискретных задержек, являются результатом работы класса Digitizer
        //массивы дискретных задержек
        //public List<int> L//по состоянию
        //{
        //    get
        //    {
        //        List<int> tmp = new List<int>();
        //        foreach (double tau_i in tau)
        //            tmp.Add(Convert.ToInt32(tau_i / Tx));
        //        return tmp;
        //    }
        //}
        //public List<int> M//по управлению
        //{
        //    get
        //    {
        //        List<int> tmp = new List<int>();
        //        foreach (double teta_i in teta)
        //            tmp.Add(Convert.ToInt32(teta_i / Tu));
        //        return tmp;
        //    }
        //}

        //отношение Tx/Tu. если =1 - синхронная система, если <1 - система с периодической выдачей управляющих воздействий, если >1 - система с периодическим режимом съёма информации.
        public double s
        {
            get { return this.Tx / this.Tu; }
        }
        #region функции вытаскивания/затаскивания приватных матриц Fi, Psi
        //Fi
        //путтеры
        public void Fi(Matrix new_Fi, int k)
        {
            if (stationarity)
                int_Fi[0] = new_Fi;
            else
                int_Fi[k] = new_Fi;
        }
        public void Fi(Matrix new_Fi)
        {
            if (stationarity)
                int_Fi[0] = new_Fi;
            else
                throw new InvalidOperationException("Система нестационарна!");
        }
        //геттеры
        public List<Matrix> allFi()
        {
            return int_Fi;
        }
        public Matrix Fi(int k)
        {
            if (stationarity)
                return int_Fi[0];
            else
                return int_Fi[k];
        }
        public Matrix Fi()
        {
            if (stationarity)
                return int_Fi[0];
            else
                throw new InvalidOperationException("Система нестационарна!");
        }
        //Psi
        //путтеры
        public void Psi(Matrix new_Psi, int k)
        {
            if (stationarity)
                int_Psi[0] = new_Psi;
            else
                int_Psi[k] = new_Psi;
        }
        public void Psi(Matrix new_Psi)
        {
            if (stationarity)
                int_Psi[0] = new_Psi;
            else
                throw new InvalidOperationException("Система нестационарна!");
        }
        //геттеры
        public List<Matrix> allPsi()
        {
            return int_Psi;
        }
        public Matrix Psi(int k)
        {
            if (stationarity)
                return int_Psi[0];
            else
                return int_Psi[k];
        }
        public Matrix Psi()
        {
            if (stationarity)
                return int_Psi[0];
            else
                throw new InvalidOperationException("Система нестационарна!");
        }
        #endregion
        #region функции вытаскивания/затаскивания приватных матриц A,B,H.
        //политика такова - даже если дали больше паарметров (вместе со временем, а система стационарна), то вытаскивается что есть
        //матрица analog_A
        //геттеры
        //целиком
        public List<List<Matrix>> analog_A()
        {
            return int_analog_A;
        }
        //нестац.
        public Matrix analog_A(int time, int delay)
        {
            if (!this.stationarity)
                return int_analog_A[time][delay];
            else
                return int_analog_A[0][delay];
        }
        //стац.
        public Matrix analog_A(int delay)
        {
            if (this.stationarity)
                return int_analog_A[0][delay];
            else
                throw new InvalidOperationException("Система нестационарна!");
        }
        //путтеры
        //нестац.
        public void analog_A(Matrix new_A, int time, int delay)
        {
            if (!this.stationarity)
                this.int_analog_A[time][delay] = new_A;
            else
                this.int_analog_A[0][delay] = new_A;
        }
        //стац
        public void analog_A(Matrix new_A, int delay)
        {
            if (this.stationarity)
                this.int_analog_A[0][delay] = new_A;
            else
                throw new InvalidOperationException("Система нестационарна!");
        }
        //матрица analog_B
        //геттеры
        //целиком
        public List<List<Matrix>> analog_B()
        {
            return this.int_analog_B;
        }
        //нестац.
        public Matrix analog_B(int time, int delay)
        {
            if (!this.stationarity)
                return int_analog_B[time][delay];
            else
                return int_analog_B[0][delay];
        }
        //стац.
        public Matrix analog_B(int delay)
        {
            if (this.stationarity)
                return int_analog_B[0][delay];
            else
                throw new InvalidOperationException("Система нестационарна!");
        }
        //путтеры
        //нестац.
        public void analog_B(Matrix new_B, int time, int delay)
        {
            if (!this.stationarity)
                this.int_analog_B[time][delay] = new_B;
            else
                this.int_analog_B[0][delay] = new_B;
        }
        //стац
        public void analog_B(Matrix new_B, int delay)
        {
            if (this.stationarity)
                this.int_analog_B[0][delay] = new_B;
            else
                throw new InvalidOperationException("Система нестационарна!");
        }
// ----------------------------------------------------------------------------------
        //матрица A
        public List<List<Matrix>> get_A()
        {
            return this.int_A;
        }
        //нестац.
        public Matrix get_A(int time, int delay)
        {
            if (!this.stationarity)
                return int_A[time][delay];
            else
                return int_A[0][delay];
        }
        //стац.
        public Matrix get_A(int delay)
        {
            if (this.stationarity)
                return int_A[0][delay];
            else
                throw new InvalidOperationException("Система нестационарна!");
        }
    // ------------------------------------------------------------------------------
        public void set_A(List<Matrix> new_A)
        {
            if (this.int_A.Count == 0)
                    this.int_A.Add(new_A);
            else
                if (this.stationarity) // Стационарная, вставляем
                    this.int_A.Insert(0, new_A);
                else                    // Не стационарная, добавляем
                    this.int_A.Add(new_A);                
        }
// ----------------------------------------------------------------------------------
        //матрица B
        //геттеры
        //целиком
        public List<List<Matrix>> B()
        {
            return this.int_B;
        }
        //нестац.
        public Matrix B(int time, int delay)
        {
            if (!this.stationarity)
                return int_B[time][delay];
            else
                return int_B[0][delay];
        }
        //стац.
        public Matrix B(int delay)
        {
            if (this.stationarity)
                return int_B[0][delay];
            else
                throw new InvalidOperationException("Система нестационарна!");
        }
// ----------------------------------------------------------------------------------
        public void set_B(List<Matrix> new_B)
        {
            if (this.int_B.Count == 0)
                this.int_B.Add(new_B);
            else
                if (this.stationarity)
                    this.int_B.Insert(0, new_B);
                else
                    this.int_B.Add(new_B);
        }
// ----------------------------------------------------------------------------------
        //матрица H
        //геттеры
        //целиком
        public List<Matrix> allH()
        {
            return int_H;
        }
        //нестац.
        public Matrix H(int time)
        {
            if (!this.stationarity)
                return int_H[time];
            else
                return int_H[0];

        }
        //стац.
        public Matrix H()
        {
            if (this.stationarity)
                return int_H[0];
            else
                throw new InvalidOperationException("Система нестационарна!");
        }
        //путтеры
        //нестац.
        public void H(Matrix new_H, int time)
        {
            if (!this.stationarity)
                this.int_H[time] = new_H;
            else
                this.int_H[0] = new_H;
        }
        //стац
        public void H(Matrix new_H)
        {
            if (this.stationarity)
                this.int_H[0] = new_H;
            else
                throw new InvalidOperationException("Система нестационарна!");
        }
        #endregion
        #endregion
#region размерности векторов и матриц
        public int n { get { return x[0].NoRows; } }
        public int m { get { return u[0].NoRows; } }
        public int a { get { return L.Count; } }
        public int c { get { return M.Count; } }
        public int p { get { return int_H[0].NoRows; } }//размерность матрицы H из формулы y = Hx
        //############################################
        public int d { get { return P.Count; } }
        //############################################
        #endregion
#region переменная типа системы
        private BitArray sys_type;
        /*по битам :
         * 0 - стационарность                   [+]
         * 1 - синхронность                     [-]
         * 2 - запаздываение по состоянию       [-]
         * 3 - запаздываение по управлению      [-]
         * 4 - полная управляемость	            [ ]
         * 5 - частичная управляемость          [ ]
         * 6 - полная наблюдаемость             [ ]
         * 7 - частичная наблюдаемость          [ ]
         * 8 - детерменорованность              [+]
         * 9 - возмущения 		                [+]
         * метки:
         * [+] передача в конструктор от пользователя
         * [-] рассчет в этом классе (из размерностей и тп)
         * [ ] рассчитывается в соответствующих классах
         * Значения: 0 означает отсутствие свойства, 1 - наличие.
         * Например:0-1-2-3-4-5-6-7-8
         * 			0-1-1-0-1-1-0-1-1
         * 			нестационарна, синхронна, есть запаздывание по состоянию, нет запаздывания по управлению, полностью управляема, частично управляема, не наблюдаема, детерменирована, с вомущениями
         */
        public Boolean stationarity
        {
            private set { sys_type[0] = value; }
            get { return sys_type[0]; }
        }
        public Boolean synchronism
        {
            private set { sys_type[1] = value; }
            get { return sys_type[1]; }
        }
        public Boolean delay_by_state
        {
            private set { sys_type[2] = value; }
            get { return sys_type[2]; }
        }
        public Boolean delay_by_control
        {
            private set { sys_type[3] = value; }
            get { return sys_type[3]; }
        }
        public Boolean full_controllability
        {
            set { sys_type[4] = value; }
            get { return sys_type[4]; }
        }
        public Boolean part_controllability
        {
            set { sys_type[5] = value; }
            get { return sys_type[5]; }
        }
        public Boolean full_observersability
        {
            set { sys_type[6] = value; }
            get { return sys_type[6]; }
        }
        public Boolean part_observersability
        {
            set { sys_type[7] = value; }
            get { return sys_type[7]; }
        }
        public Boolean determinancy
        {
            private set { sys_type[8] = value; }
            get { return sys_type[8]; }
        }
        public Boolean perturbation
        {
            private set { sys_type[9] = value; }
            get { return sys_type[9]; }
        }
#endregion
        #region конструкторы
        //Общий конструктор
        public State(
            List<Matrix> new_x,
            List<Matrix> new_u,
            List<List<Matrix>> new_analog_A,
            List<List<Matrix>> new_analog_B,
            List<Matrix> new_H,
            List<double> new_tau,
            List<double> new_teta,
            //List<double> new_quality_f, - рассчитывается в regulator
            List<Matrix> new_Fi,
            List<Matrix> new_Psi,
            double new_Tx,
            double new_Tu,
            Boolean new_stationarity,
            Boolean new_determinancy,
            Boolean new_perturbation)
        {
            x = new_x;
            u = new_u;
            int_analog_A = new_analog_A;
            int_analog_B = new_analog_B;
            int_H = new_H;
            tau = new_tau;
            teta = new_teta;
            //quality_f = new_quality_f; - рассчитывается в regulator
            int_Fi = new_Fi;
            int_Psi = new_Psi;
            Tx = new_Tx;
            Tu = new_Tu;
            sys_type = new BitArray(16, false);
            this.stationarity = new_stationarity;
            if (Convert.ToInt32(s) == 1)
            {
                this.synchronism = true;
            }
            else
            {
                this.synchronism = false;
            }
            if (this.tau.Count == 1) // фикс db2577c
            {
                this.delay_by_state = false;
            }
            else
            {
                this.delay_by_state = true;
            }
            if (this.teta.Count == 1) // фикс db2577c
            {
                this.delay_by_control = false;
            }
            else
            {
                this.delay_by_control = true;
            }
            this.determinancy = new_determinancy;
            this.perturbation = new_perturbation;
            int_A = new List<List<Matrix>>();
            int_B = new List<List<Matrix>>();
            //int_A.Add(new List<Matrix>());
            //int_B.Add(new List<Matrix>());
            L = new List<int>();
            M = new List<int>();
            P = new List<int>();
        }
        #endregion
    }
}
