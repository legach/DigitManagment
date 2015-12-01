using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using MatrixLibrary;
using System.Drawing;
using GraphLib;
using System.Windows.Forms;

namespace neco
{
    //класс инициализации из файла .json. 
    [DataContract]
    class JasonsYandere
    {
        //переменные для выгрузки из .json
        [DataMember]
        double[][] x;
        [DataMember]
        double[][] u;
        [DataMember]
        double Tx;
        [DataMember]
        double Tu;
        [DataMember]
        double[][][][] A;
        [DataMember]
        double[][][][] B;
        [DataMember]
        double[][][] H;
        [DataMember]
        double[] tau;
        [DataMember]
        double[] teta;
        [DataMember]
        double[][][] Fi;
        [DataMember]
        double[][][] Psi;
        [DataMember]
        bool determinancy;
        [DataMember]
        bool perturbation;
        //Функция аккуратного вызова яндере
        public State get_yandere_careful()
        {
            Matrix tmp;

            List<Matrix> new_x = new List<Matrix>();
            for (int i = 0; i < this.x.Length; i++)
            {
                tmp = new Matrix(this.x[i].Length, 1);
                for (int j = 0; j < this.x[i].Length; j++)
                    tmp[j,0] = this.x[i][j];
                new_x.Add(tmp);
            }
            
            //возможно неверная заливка - на 0-й момент времени нет управления.. или можно это заставить делать в .json
            List<Matrix> new_u = new List<Matrix>();
            for (int i = 0; i < this.u.Length; i++)
            {
                tmp = new Matrix(this.u[i].Length, 1);
                for (int j = 0; j < this.u[i].Length; j++)
                    tmp[j, 0] = this.u[i][j];
                new_u.Add(tmp);
            }

            List<List<Matrix>> new_analog_A = new List<List<Matrix>>();
            for (int i = 0; i < this.A.Length; i++)//цикл по временным точкам
            {
                new_analog_A.Add(new List<Matrix>());
                for (int j = 0; j < this.A[i].Length; j++)//цикл для данного времени по задержкам
                {
                    tmp = new Matrix(this.A[i][j].Length, this.A[i][j].Length);
                    for (int k = 0; k < this.A[i][j].Length; k++)//проход по строкам
                        for (int l = 0; l < this.A[i][j][k].Length; l++)
                        {
                            tmp[k,l] = this.A[i][j][k][l];
                        }
                    new_analog_A[i].Add(tmp);
                }
            }


            List<List<Matrix>> new_analog_B = new List<List<Matrix>>();
            for (int i = 0; i < this.B.Length; i++)//цикл по временным точкам
            {
                new_analog_B.Add(new List<Matrix>());
                for (int j = 0; j < this.B[i].Length; j++)//цикл для данного времени по задержкам
                {
                    tmp = new Matrix(this.B[i][j].Length, this.B[i][j][0].Length);
                    for (int k = 0; k < this.B[i][j].Length; k++)//проход по строкам
                        for (int l = 0; l < this.B[i][j][k].Length; l++)
                        {
                            tmp[k, l] = this.B[i][j][k][l];
                        }
                    new_analog_B[i].Add(tmp);
                }
            }

            List<Matrix> new_H = new List<Matrix>();
            if (this.H != null)
            {
                for (int i = 0; i < this.H.Length; i++)
                {
                    tmp = new Matrix(this.H[i].Length, this.H[i][0].Length);
                    for (int j = 0; j < this.H[i].Length; j++)
                    {
                        for (int k = 0; k < this.H[i][j].Length; k++)
                            tmp[j, k] = this.H[i][j][k];
                    }
                    new_H.Add(tmp);
                }
            }
            else
                foreach (Matrix some_x in new_x)
                    new_H.Add(new Matrix(Matrix.Identity(new_x[0].NoRows)));

            List<double> new_tau = new List<double>();
            for (int i = 0; i < this.tau.Length; i++)
                new_tau.Add(this.tau[i]);

            List<double> new_teta = new List<double>();
            for (int i = 0; i < this.teta.Length; i++)
                new_teta.Add(this.teta[i]);

            List<Matrix> new_Fi = new List<Matrix>();
            if (this.Fi == null)
                new_Fi.Add(new Matrix(Matrix.Identity(this.x[0].Length)));//пишем единичную как заглушку
            else//если Fi задана, то Fi передали из json и его надо писать
            {
                for (int i = 0; i < this.Fi.Length; i++)
                {
                    tmp = new Matrix(Matrix.Identity(this.x[0].Length));
                    for (int j = 0; j < this.Fi[i].Length; j++)
                    {
                        for (int k = 0; k < this.Fi[i][j].Length; k++)
                            tmp[j, k] = this.Fi[i][j][k];
                    }
                    new_Fi.Add(tmp);
                }
            }

            List<Matrix> new_Psi = new List<Matrix>();
            if (this.Psi == null)
                new_Psi.Add(new Matrix(Matrix.Identity(this.u[0].Length)));//пишем единичную как заглушку
            else//если Psi задана, то Psi передали из json и его надо писать
            {
                for (int i = 0; i < this.Psi.Length; i++)
                {
                    tmp = new Matrix(Matrix.Identity(this.u[0].Length));
                    for (int j = 0; j < this.Psi[i].Length; j++)
                    {
                        for (int k = 0; k < this.Psi[i][j].Length; k++)
                            tmp[j, k] = this.Psi[i][j][k];
                    }
                    new_Psi.Add(tmp);
                }
            }

            double new_Tx = this.Tx;
            double new_Tu = this.Tu;
            if (new_Tu <= 0 && new_Tx > 0)//если задали только Tx
                Tu = Tx;
            if (new_Tx <= 0 && new_Tu > 0)//а если только Tu
                Tx = Tu;
            if (Tu <= 0 || Tx <= 0)//и если оне по-прежнему некорректны...
                Tx = Tu = 1;

            Boolean new_stationarity = new bool();
            if (new_analog_A.Count == 1)
                new_stationarity = true;

            Boolean new_determinancy = this.determinancy;
            Boolean new_perturbation = this.perturbation;
            return new State(
                new_x,
                new_u,
                new_analog_A,
                new_analog_B,
                new_H,
                new_tau,
                new_teta,
                new_Fi,
                new_Psi,
                new_Tx,
                new_Tu,
                new_stationarity,
                new_determinancy,
                new_perturbation);
        }
        //конструктор из .json по пути к файлу.
        public JasonsYandere(FileStream F)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(JasonsYandere));
            JasonsYandere tmp = (JasonsYandere)ser.ReadObject(F);
            this.A = tmp.A;
            this.B = tmp.B;
            this.Fi = tmp.Fi;
            this.Psi = tmp.Psi;
            this.tau = tmp.tau;
            this.teta = tmp.teta;
            this.Tu = tmp.Tu;
            this.Tx = tmp.Tx;
            this.u = tmp.u;
            this.x = tmp.x;
            this.determinancy = tmp.determinancy;
            this.H = tmp.H;
        }
    }
    //Получатель данных для следующего шага. гдето от всевышнего, гдето от ктулху
    class DataGetter
    {
        //вертает тотже State пока
        public static State GetData(State sys_state)
        {
            return sys_state;
        }
        //вертает пустой State, но с массивами
        public static State GetNothing()
        {
            return new State(new List<Matrix>(),new List<Matrix>(),new List<List<Matrix>>(), new List<List<Matrix>>(), new List<Matrix>(), new List<double>(), new List<double>(), new List<Matrix>(), new List<Matrix>(), new double(), new double(), new bool(), new bool(), new bool());
        }
    }
}
