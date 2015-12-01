using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace neco
{
    partial class SysInfo : Form
    {
        ESystem sys;
        public SysInfo(ESystem sys)
        {
            this.sys = sys;
            //пустышка аднака - пустой блок для отображения матриц
            this.UniversalMatrixBox = new TBMatrixDrawer(new MatrixLibrary.Matrix(10, 10));
            UniversalMatrixBox.Visible = false;
            UniversalMatrixBox.Location = new Point(209, 56);
            this.Controls.Add(UniversalMatrixBox);

            InitializeComponent();
        }
        //обновление информации на форме
        public void UpdateInfo(ESystem sys)
        {
            this.sys = sys;
            //обновление типа системы
            if (sys.current_state.stationarity)
                Stationarity.ForeColor = Color.Green;
            else
                Stationarity.ForeColor = Color.Red;

            if (sys.current_state.synchronism)
                this.Syncronism.ForeColor = Color.Green;
            else
                Syncronism.ForeColor = Color.Red;

            if (sys.current_state.delay_by_state)
                this.delay_by_state.ForeColor = Color.Green;
            else
                delay_by_state.ForeColor = Color.Red;

            if (sys.current_state.delay_by_control)
                this.delay_by_control.ForeColor = Color.Green;
            else
                delay_by_control.ForeColor = Color.Red;

            if (sys.current_state.full_controllability)
                this.full_controllability.ForeColor = Color.Green;
            else
                full_controllability.ForeColor = Color.Red;

            if (sys.current_state.part_controllability)
                this.part_controllability.ForeColor = Color.Green;
            else
                part_controllability.ForeColor = Color.Red;

            if (sys.current_state.full_observersability)
                this.full_observability.ForeColor = Color.Green;
            else
                full_observability.ForeColor = Color.Red;

            if (sys.current_state.part_observersability)
                this.part_observability.ForeColor = Color.Green;
            else
                part_observability.ForeColor = Color.Red;

            if (sys.current_state.determinancy)
                this.Determinancy.ForeColor = Color.Green;
            else
                Determinancy.ForeColor = Color.Red;

            if (sys.current_state.perturbation)
                this.Perturbation.ForeColor = Color.Green;
            else
                Perturbation.ForeColor = Color.Red;
            //строчичка с размерами
            this.Sizes.Text = "n = " + sys.current_state.n + " m = " + sys.current_state.m +" p = " + sys.current_state.p +" a = " + sys.current_state.a +" c = " + sys.current_state.c ;
            //протирка матриц.. а ВДРУК!
            //скрываем все, что было до этого
            UniversalMatrixBox.Visible = false;
            this.TimeBox.Enabled = false;
            this.TimeBox.Items.Clear();
            this.DelayBox.Enabled = false;
            this.DelayBox.Items.Clear();
            this.LoadMatrix.Enabled = false;
        }

        private void MatrixBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //скрываем все, что было до этого
            UniversalMatrixBox.Visible = false;
            this.TimeBox.Enabled = false;
            this.TimeBox.Items.Clear();
            this.DelayBox.Enabled = false;
            this.DelayBox.Items.Clear();
            this.LoadMatrix.Enabled = false;
            //ветвление по выбору
            String[] Times;
            String[] Delays;
            switch (this.MatrixBox.Text)
            {
                case "An":
                    {
                        //о, времена
                        Times = new String[sys.current_state.analog_A().Count];
                        for (int i = 0; i < sys.current_state.analog_A().Count; i++)
                            Times[i] = i.ToString();
                        this.TimeBox.Items.AddRange(Times);
                        this.TimeBox.Text = Times[0];
                        this.TimeBox.Enabled = true;
                        //о, нравы
                        Delays = new String[sys.current_state.tau.Count];
                        for (int i = 0; i < sys.current_state.tau.Count; i++)
                            Delays[i] = i.ToString();
                        this.DelayBox.Items.AddRange(Delays);
                        this.DelayBox.Text = Delays[0];
                        this.DelayBox.Enabled = true;
                        //ну и кнопоча
                        LoadMatrix.Enabled = true;
                        break;
                    }
                case "Bn":
                    {
                        //о, времена
                        Times = new String[sys.current_state.analog_B().Count];
                        for (int i = 0; i < sys.current_state.analog_B().Count; i++)
                            Times[i] = i.ToString();
                        this.TimeBox.Items.AddRange(Times);
                        this.TimeBox.Text = Times[0];
                        this.TimeBox.Enabled = true;
                        //о, нравы
                        Delays = new String[sys.current_state.teta.Count];
                        for (int i = 0; i < sys.current_state.teta.Count; i++)
                            Delays[i] = i.ToString();
                        this.DelayBox.Items.AddRange(Delays);
                        this.DelayBox.Text = Delays[0];
                        this.DelayBox.Enabled = true;
                        //ну и кнопоча
                        LoadMatrix.Enabled = true;
                        break;
                    }
                case "A":
                    {
                        //о, времена
                        Times = new String[sys.current_state.get_A().Count];
                        for (int i = 0; i < sys.current_state.get_A().Count; i++)
                            Times[i] = i.ToString();
                        this.TimeBox.Items.AddRange(Times);
                        this.TimeBox.Text = Times[0];
                        this.TimeBox.Enabled = true;
                        //о, нравы
                        Delays = new String[sys.current_state.a];
                        for (int i = 0; i < sys.current_state.a; i++)
                            Delays[i] = i.ToString();
                        this.DelayBox.Items.AddRange(Delays);
                        this.DelayBox.Text = Delays[0];
                        this.DelayBox.Enabled = true;
                        //ну и кнопоча
                        LoadMatrix.Enabled = true;
                        break;
                    }
                case "B":
                    {
                        //о, времена
                        Times = new String[sys.current_state.B().Count];
                        for (int i = 0; i < sys.current_state.B().Count; i++)
                            Times[i] = i.ToString();
                        this.TimeBox.Items.AddRange(Times);
                        this.TimeBox.Text = Times[0];
                        this.TimeBox.Enabled = true;
                        //о, нравы
                        Delays = new String[sys.current_state.c];
                        for (int i = 0; i < sys.current_state.c; i++)
                            Delays[i] = i.ToString();
                        this.DelayBox.Items.AddRange(Delays);
                        this.DelayBox.Text = Delays[0];
                        this.DelayBox.Enabled = true;
                        //ну и кнопоча
                        LoadMatrix.Enabled = true;
                        break;
                    }
                case "H":
                    {
                        //о, времена
                        Times = new String[sys.current_state.allH().Count];
                        for (int i = 0; i < sys.current_state.allH().Count; i++)
                            Times[i] = i.ToString();
                        this.TimeBox.Items.AddRange(Times);
                        this.TimeBox.Text = Times[0];
                        this.TimeBox.Enabled = true;
                        //безнравственность
                        //ну и кнопоча
                        LoadMatrix.Enabled = true;
                        break;
                    }
                case "L":
                    {
                        //безвременность
                         //безнравственность
                        //ну и кнопоча
                        LoadMatrix.Enabled = true;
                        break;
                    }
                case "M":
                    {
                        //безвременность
                        //безнравственность
                        //ну и кнопоча
                        LoadMatrix.Enabled = true;
                        break;
                    }
                case "tau":
                    {
                        //безвременность
                        //безнравственность
                        //ну и кнопоча
                        LoadMatrix.Enabled = true;
                        break;
                    }
                case "teta":
                    {
                        //безвременность
                        //безнравственность
                        //ну и кнопоча
                        LoadMatrix.Enabled = true;
                        break;
                    }
                case "Fi":
                    {
                        //временность
                        Times = new String[sys.current_state.allFi().Count];
                        for (int i = 0; i < sys.current_state.allFi().Count; i++)
                            Times[i] = i.ToString();
                        this.TimeBox.Items.AddRange(Times);
                        this.TimeBox.Text = Times[0];
                        this.TimeBox.Enabled = true;
                        //безнравственность
                        //ну и кнопоча
                        LoadMatrix.Enabled = true;
                        break;
                    }
                case "Psi":
                    {
                        //безвременность
                        Times = new String[sys.current_state.allPsi().Count];
                        for (int i = 0; i < sys.current_state.allPsi().Count; i++)
                            Times[i] = i.ToString();
                        this.TimeBox.Items.AddRange(Times);
                        this.TimeBox.Text = Times[0];
                        this.TimeBox.Enabled = true;
                        //безнравственность
                        //ну и кнопоча
                        LoadMatrix.Enabled = true;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void LoadMatrix_Click(object sender, EventArgs e)
        {
            //матрицца для вывода
            MatrixLibrary.Matrix DownloadedForFall = null;
            //флажок
            bool notFailed = true;
            switch (this.MatrixBox.Text)
            {
                case "An":
                    {
                        DownloadedForFall = sys.current_state.analog_A(Convert.ToInt32(TimeBox.Text), Convert.ToInt32(DelayBox.Text));
                        break;
                    }
                case "Bn":
                    {
                        DownloadedForFall = sys.current_state.analog_B(Convert.ToInt32(TimeBox.Text), Convert.ToInt32(DelayBox.Text));
                        break;
                    }
                case "A":
                    {
                        DownloadedForFall = sys.current_state.get_A(Convert.ToInt32(TimeBox.Text), Convert.ToInt32(DelayBox.Text));
                        break;
                    }
                case "B":
                    {
                        DownloadedForFall = sys.current_state.B(Convert.ToInt32(TimeBox.Text), Convert.ToInt32(DelayBox.Text));
                        break;
                    }
                case "H":
                    {
                        DownloadedForFall = sys.current_state.H(Convert.ToInt32(TimeBox.Text));
                        break;
                    }
                case "L":
                    {
                        DownloadedForFall = new MatrixLibrary.Matrix(1, sys.current_state.a);
                        for (int i = 0; i < sys.current_state.a; i++)
                            DownloadedForFall[0, i] = sys.current_state.L[i];
                        break;
                    }
                case "M":
                    {
                        DownloadedForFall = new MatrixLibrary.Matrix(1, sys.current_state.c);
                        for (int i = 0; i < sys.current_state.c; i++)
                            DownloadedForFall[0, i] = sys.current_state.M[i];
                        break;
                    }
                case "tau":
                    {
                        DownloadedForFall = new MatrixLibrary.Matrix(1, sys.current_state.tau.Count);
                        for (int i = 0; i < sys.current_state.tau.Count; i++)
                            DownloadedForFall[0, i] = sys.current_state.tau[i];
                        break;
                    }
                case "teta":
                    {
                        DownloadedForFall = new MatrixLibrary.Matrix(1, sys.current_state.teta.Count);
                        for (int i = 0; i < sys.current_state.teta.Count; i++)
                            DownloadedForFall[0, i] = sys.current_state.teta[i];
                        break;
                    }
                case "Fi":
                    {
                        DownloadedForFall = sys.current_state.Fi();
                        break;
                    }
                case "Psi":
                    {
                        DownloadedForFall = sys.current_state.Psi();
                        break;
                    }
                default:
                    {
                        notFailed = false;
                        break;
                    }
            }
            //инициализация Матрицорисователя
            if (notFailed)//если конечно ктото не перепорол
            {
                this.UniversalMatrixBox.ReloadMatrix(DownloadedForFall);
                UniversalMatrixBox.Visible = true;
                if(this.Size.Width < UniversalMatrixBox.Location.X + UniversalMatrixBox.Size.Width || this.Size.Height < UniversalMatrixBox.Location.Y + UniversalMatrixBox.Size.Height)
                    this.Size = new Size(UniversalMatrixBox.Location) + UniversalMatrixBox.Size + new Size(10,30);
                if (UniversalMatrixBox.Location.X + UniversalMatrixBox.Size.Width < 450 && UniversalMatrixBox.Location.Y + UniversalMatrixBox.Size.Height <= 200)
                    this.Size = new Size(450, 200);
            }

        }

        private void Hider_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
