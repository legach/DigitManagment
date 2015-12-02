using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatrixLibrary;
using System.Runtime.Serialization;

namespace neco
{
    /// <summary>
    /// класс состояния системы.
    /// </summary>
    class State
    {
#region переменные
        //во вложенных списках внешний - временная шкала, внутренний - шкала запаздываний. 
        //в списках 0-й элемент считается текущим, 1-й - предыдущим и т.д. Т.е. для добавления следующего элемента необходимо вставить нулевой элемент в список.


        //отсчеты векторов состояния и управления
        /// <summary>
        /// отсчеты векторов состояния и управления
        /// размерность каждого элемента списка n x 1. это отсчеты состояний системы.
        /// </summary>
        public List<Matrix> x;
        /// <summary>
        /// отсчеты векторов состояния и управления
        /// размерность каждого элемента списка m x 1. это отсчеты управляющих воздействий. 0-й элемент занулен, кроме регулятора, где он считается, а потом сдвигается время
        /// </summary>
        public List<Matrix> u;

        
        /// <summary>
        /// матрицы перевода x в y - матрицы p x n
        /// </summary>
        List<Matrix> int_H;
        
        
        /// <summary>
        /// непрерывные матрицы-описатели системы
        /// размерность матриц - n x n (n - размерность х), запаздывание длинной L, время длинной от L и возрастает по мере рассчета
        /// </summary>
        List<List<Matrix>> int_analog_A;
        /// <summary>
        /// непрерывные матрицы-описатели системы
        /// размерность матриц - m x m (m - размерность u), запаздывание длинной M, время длинной от M и возрастает по мере рассчета
        /// </summary>
        List<List<Matrix>> int_analog_B;

        
        /// <summary>
        /// дискретезированные матрицы - формат как у непрерывных, только они являются результатом работы класса Digitizer
        /// </summary>
        List<List<Matrix>> int_A;
        /// <summary>
        /// дискретезированные матрицы - формат как у непрерывных, только они являются результатом работы класса Digitizer
        /// </summary>
        List<List<Matrix>> int_B;

        
        /// <summary>
        /// массивы непрерывных задержек. НЕ лист листов, т.к. при изменении А задержки все те-же, т.е. другой массив А накладывается на так-же отдаленные от текущего t отсчеты состояний.
        /// по состоянию
        /// </summary>
        public List<double> tau;
        /// <summary>
        /// по управлению
        /// </summary>
        public List<double> teta;

        /// <summary>
        /// массивы дискретных задержек.
        /// по состоянию
        /// </summary>
        public List<int> L;
        /// <summary>
        /// массивы дискретных задержек.
        /// по управлени
        /// </summary>
        public List<int> M;
        //############################################
        /// <summary>
        /// массивы дискретных задержек.
        /// по наблюдению
        /// </summary>
        public List<int> P;
        //############################################
        
        
        /// <summary>
        /// периоды дискретизации
        /// //по состоянию
        /// </summary>
        public double T;
        public double sx;
        public double su;
        /// <summary>
        /// периоды дискретизации
        /// по состоянию
        /// </summary>
        public double Tx;
        /// <summary>
        /// периоды дискретизации
        /// по управлению
        /// </summary>
        public double Tu;

        /// <summary>
        /// значения критерия оптимальности. рассчитывается в Regulator
        /// </summary>
        public List<double> quality_f;

        public bool isSplainAppr;
        public bool isRandomEffect;

        
        /// <summary>
        /// матрицы для рассчета критерия оптимальности (для теста брать единичные, предусмотреть изменение в процессе работы)
        ///оказывацца зависят от времени
        /// </summary>
        List<Matrix> int_Fi;
        /// <summary>
        /// матрицы для рассчета критерия оптимальности (для теста брать единичные, предусмотреть изменение в процессе работы)
        ///оказывацца зависят от времени
        /// </summary>
        List<Matrix> int_Psi;
        #endregion
#region свойства
        /// <summary>
        /// Выходные данные системы y размерности p x 1
        /// </summary>
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

        /// <summary>
        /// отношение Tx/Tu. если =1 - синхронная система, если <1 - система с периодической выдачей управляющих воздействий, если >1 - система с периодическим режимом съёма информации.
        /// </summary>
        public double s
        {
            get { return this.Tx / this.Tu; }
        }
        #region функции вытаскивания/затаскивания приватных матриц Fi, Psi
        
        /// <summary>
        /// Fi
        ///путтеры
        /// </summary>
        /// <param name="new_Fi"></param>
        /// <param name="k"></param>
        public void Fi(Matrix new_Fi, int k)
        {
            if (stationarity)
                int_Fi[0] = new_Fi;
            else
                int_Fi[k] = new_Fi;
        }

        /// <summary>
        /// Fi
        ///путтеры
        /// </summary>
        /// <param name="new_Fi"></param>
        public void Fi(Matrix new_Fi)
        {
            if (stationarity)
                int_Fi[0] = new_Fi;
            else
                throw new InvalidOperationException("Система нестационарна!");
        }

        /// <summary>
        /// геттеры
        /// </summary>
        /// <returns></returns>
        public List<Matrix> allFi()
        {
            return int_Fi;
        }

        /// <summary>
        /// геттеры
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public Matrix Fi(int k)
        {
            if (stationarity)
                return int_Fi[0];
            else
                return int_Fi[k];
        }
        /// <summary>
        /// геттеры
        /// </summary>
        /// <returns></returns>
        public Matrix Fi()
        {
            if (stationarity)
                return int_Fi[0];
            else
                throw new InvalidOperationException("Система нестационарна!");
        }
        
        /// <summary>
        /// Psi
        /// путтеры
        /// </summary>
        /// <param name="new_Psi"></param>
        /// <param name="k"></param>
        public void Psi(Matrix new_Psi, int k)
        {
            if (stationarity)
                int_Psi[0] = new_Psi;
            else
                int_Psi[k] = new_Psi;
        }
        /// <summary>
        /// Psi
        /// путтеры
        /// </summary>
        /// <param name="new_Psi"></param>
        public void Psi(Matrix new_Psi)
        {
            if (stationarity)
                int_Psi[0] = new_Psi;
            else
                throw new InvalidOperationException("Система нестационарна!");
        }
        
        /// <summary>
        /// геттеры
        /// </summary>
        /// <returns></returns>
        public List<Matrix> allPsi()
        {
            return int_Psi;
        }
        /// <summary>
        /// геттеры
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public Matrix Psi(int k)
        {
            if (stationarity)
                return int_Psi[0];
            else
                return int_Psi[k];
        }
        /// <summary>
        /// геттеры
        /// </summary>
        /// <returns></returns>
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
        
        /// <summary>
        /// матрица analog_A
        ///геттеры
        ///целиком
        /// </summary>
        /// <returns></returns>
        public List<List<Matrix>> analog_A()
        {
            return int_analog_A;
        }
        //
        /// <summary>
        /// матрица analog_A
        /// геттеры
        /// нестац.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public Matrix analog_A(int time, int delay)
        {
            if (!this.stationarity)
                return int_analog_A[time][delay];
            else
                return int_analog_A[0][delay];
        }
        
        /// <summary>
        /// матрица analog_A
        /// геттеры
        /// стац.
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        public Matrix analog_A(int delay)
        {
            if (this.stationarity)
                return int_analog_A[0][delay];
            else
                throw new InvalidOperationException("Система нестационарна!");
        }
        
        /// <summary>
        /// матрица analog_A
        /// путтеры
        /// нестац.
        /// </summary>
        /// <param name="new_A"></param>
        /// <param name="time"></param>
        /// <param name="delay"></param>
        public void analog_A(Matrix new_A, int time, int delay)
        {
            if (!this.stationarity)
                this.int_analog_A[time][delay] = new_A;
            else
                this.int_analog_A[0][delay] = new_A;
        }

        /// <summary>
        /// матрица analog_A
        /// путтеры
        /// стац
        /// </summary>
        /// <param name="new_A"></param>
        /// <param name="delay"></param>
        public void analog_A(Matrix new_A, int delay)
        {
            if (this.stationarity)
                this.int_analog_A[0][delay] = new_A;
            else
                throw new InvalidOperationException("Система нестационарна!");
        }
        
        /// <summary>
        /// матрица analog_B
        ///геттеры
        ///целиком
        /// </summary>
        /// <returns></returns>
        public List<List<Matrix>> analog_B()
        {
            return this.int_analog_B;
        }
        
        /// <summary>
        /// матрица analog_B
        /// геттеры
        /// нестац.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public Matrix analog_B(int time, int delay)
        {
            if (!this.stationarity)
                return int_analog_B[time][delay];
            else
                return int_analog_B[0][delay];
        }
        
        /// <summary>
        /// матрица analog_B
        /// геттеры
        /// стац.
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        public Matrix analog_B(int delay)
        {
            if (this.stationarity)
                return int_analog_B[0][delay];
            else
                throw new InvalidOperationException("Система нестационарна!");
        }
        
        /// <summary>
        /// матрица analog_B
        /// путтеры
        /// нестац.
        /// </summary>
        /// <param name="new_B"></param>
        /// <param name="time"></param>
        /// <param name="delay"></param>
        public void analog_B(Matrix new_B, int time, int delay)
        {
            if (!this.stationarity)
                this.int_analog_B[time][delay] = new_B;
            else
                this.int_analog_B[0][delay] = new_B;
        }
        
        /// <summary>
        /// матрица analog_B
        /// путтеры
        /// стац
        /// </summary>
        /// <param name="new_B"></param>
        /// <param name="delay"></param>
        public void analog_B(Matrix new_B, int delay)
        {
            if (this.stationarity)
                this.int_analog_B[0][delay] = new_B;
            else
                throw new InvalidOperationException("Система нестационарна!");
        }
// ----------------------------------------------------------------------------------
        
        /// <summary>
        /// матрица A
        /// </summary>
        /// <returns></returns>
        public List<List<Matrix>> get_A()
        {
            return this.int_A;
        }
        
        /// <summary>
        /// матрица A
        /// нестац.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public Matrix get_A(int time, int delay)
        {
            if (!this.stationarity)
                return int_A[time][delay];
            else
                return int_A[0][delay];
        }
        
        /// <summary>
        /// //матрица A
        /// стац.
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
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
        
        /// <summary>
        ///матрица B
        ///геттеры
        ///целиком
        /// </summary>
        /// <returns></returns>
        public List<List<Matrix>> B()
        {
            return this.int_B;
        }
        
        /// <summary>
        /// матрица B
        ///геттеры
        ///нестац.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public Matrix B(int time, int delay)
        {
            if (!this.stationarity)
                return int_B[time][delay];
            else
                return int_B[0][delay];
        }
        
        /// <summary>
        /// матрица B
        /// геттеры
        /// стац.
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// матрица H
        ///геттеры
        ///целиком
        /// </summary>
        /// <returns></returns>
        public List<Matrix> allH()
        {
            return int_H;
        }
        
        /// <summary>
        /// //матрица H
        ///геттеры
        ///нестац.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public Matrix H(int time)
        {
            if (!this.stationarity)
                return int_H[time];
            else
                return int_H[0];

        }
        
        /// <summary>
        /// матрица H
        ///геттеры
        ///стац.
        /// </summary>
        /// <returns></returns>
        public Matrix H()
        {
            if (this.stationarity)
                return int_H[0];
            else
                throw new InvalidOperationException("Система нестационарна!");
        }
       
        /// <summary>
        /// матрица H
        ///путтеры
        ///нестац.
        /// </summary>
        /// <param name="new_H"></param>
        /// <param name="time"></param>
        public void H(Matrix new_H, int time)
        {
            if (!this.stationarity)
                this.int_H[time] = new_H;
            else
                this.int_H[0] = new_H;
        }
        
        /// <summary>
        /// матрица H
        ///путтеры
        ///стац
        /// </summary>
        /// <param name="new_H"></param>
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
        /// <summary>
        /// размерность матрицы H из формулы y = Hx
        /// </summary>
        public int p { get { return int_H[0].NoRows; } }
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
        /// <summary>
        /// стационарность
        /// </summary>
        public Boolean stationarity
        {
            private set { sys_type[0] = value; }
            get { return sys_type[0]; }
        }
        /// <summary>
        /// синхронность
        /// </summary>
        public Boolean synchronism
        {
            private set { sys_type[1] = value; }
            get { return sys_type[1]; }
        }
        /// <summary>
        /// запаздываение по состоянию
        /// </summary>
        public Boolean delay_by_state
        {
            private set { sys_type[2] = value; }
            get { return sys_type[2]; }
        }
        /// <summary>
        /// запаздываение по управлению
        /// </summary>
        public Boolean delay_by_control
        {
            private set { sys_type[3] = value; }
            get { return sys_type[3]; }
        }
        /// <summary>
        /// полная управляемость
        /// </summary>
        public Boolean full_controllability
        {
            set { sys_type[4] = value; }
            get { return sys_type[4]; }
        }
        /// <summary>
        /// частичная управляемость
        /// </summary>
        public Boolean part_controllability
        {
            set { sys_type[5] = value; }
            get { return sys_type[5]; }
        }
        /// <summary>
        /// полная наблюдаемость
        /// </summary>
        public Boolean full_observersability
        {
            set { sys_type[6] = value; }
            get { return sys_type[6]; }
        }
        /// <summary>
        /// частичная наблюдаемость
        /// </summary>
        public Boolean part_observersability
        {
            set { sys_type[7] = value; }
            get { return sys_type[7]; }
        }
        /// <summary>
        /// детерменорованность
        /// </summary>
        public Boolean determinancy
        {
            private set { sys_type[8] = value; }
            get { return sys_type[8]; }
        }
        /// <summary>
        /// возмущения
        /// </summary>
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
