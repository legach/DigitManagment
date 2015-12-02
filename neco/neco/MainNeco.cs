using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GraphLib;
using MatrixLibrary;

namespace neco
{
    public partial class MainNeco : Form
    {
        static ESystem sys;
        /// <summary>
        /// разблокировать все вкладки
        /// </summary>
        private bool debug = true; 
        /// <summary>
        /// успешно выполненный этап расчета управления (1..4)
        /// </summary>
        private int step = 0; 
        private bool isSpline = false;
        private bool isNoiseEnabled = false;

        public MainNeco()
        {
            GUI = new GUI_Container();
            InitializeComponent();
            #if DEBUG
                debug = false;
            #endif
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //отлавливаем исключения инициализации
            try
            {
                //загрузка sys из файла
                sys = GUI.OpenJsonFile(isSpline, isNoiseEnabled);
            }
            catch (Exception excption)//если при определении следующего шага чтото пошло нетак
            {
                MessageBox.Show(excption.Message + "\n" + excption.StackTrace.ToString(), "Внимание!",MessageBoxButtons.OK,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button1);//вываливаем сообщение     
            }
            //если файл был успешно загружен
            if (sys != null)
            {
                //грузим полученный sys в формочку
                if (SI == null && sys != null)//проверка на открытие файла снова
                {
                    SI = new SysInfo(sys);
                }
                else
                    SI.UpdateInfo(sys);
                //разблокировка элементов
                this.showStateInfoToolStripMenuItem.Enabled = true;
                this.startEmulationToolStripMenuItem.Enabled = true;

                tabControl1.SelectTab(0);
                if (!debug) step = 0;   else step = 4;
                tabPage2.Controls.Clear();
                tabPage2.Controls.Add(button2);

                tabPage1.Controls.Clear();
                tabPage1.Controls.Add(button1);
                tabPage1.Controls.Add(label1);
                tabPage1.Controls.Add(lAnalogEquation);
                lAnalogEquation.Text = "x'(t) = ";

                for (int i = 0; i < sys.current_state.analog_A()[0].Count; i++)
                {
                    if (i != 0) this.lAnalogEquation.Text += " + ";
                    this.lAnalogEquation.Text += "Ан" + i.ToString() + "x(t";
                    if (i == 0) this.lAnalogEquation.Text += ")";
                    else this.lAnalogEquation.Text += " - " + sys.current_state.tau[i] + ")";


                }

                for (int i = 0; i < sys.current_state.analog_B()[0].Count; i++)
                {
                    this.lAnalogEquation.Text += " + Bн" + i.ToString() + "u(t";
                    if (i == 0) this.lAnalogEquation.Text += ")";
                    else this.lAnalogEquation.Text += " - " + sys.current_state.teta[i] + ")";

                }


                UniversalMatrixBox = new List<MatrixDrawer>();
                LabelList = new List<Label>();

                int loctemp = 20;
                int j = 0;
                for (int i = 0; i < sys.current_state.analog_B()[0].Count + sys.current_state.analog_A()[0].Count; i++)
                {

                    MatrixDrawer tmp = new MatrixDrawer(new MatrixLibrary.Matrix(10, 10));

                    this.UniversalMatrixBox.Add(new MatrixDrawer(new MatrixLibrary.Matrix(10, 10)));
                    this.LabelList.Add(new Label());


                    UniversalMatrixBox[i].Visible = false;
                    UniversalMatrixBox[i].Location = new Point(loctemp, 130);
                    this.LabelList[i].Location = new Point(loctemp, 115);
                    this.LabelList[i].Font = new Font(this.Font, FontStyle.Bold);


                    this.tabPage1.Controls.Add(UniversalMatrixBox[i]);
                    this.tabPage1.Controls.Add(LabelList[i]);


                    if (i < sys.current_state.analog_A()[0].Count)
                    {
                        this.UniversalMatrixBox[i].ReloadMatrix(sys.current_state.analog_A()[0][i]);
                        this.LabelList[i].Text = "Ан" + i.ToString() + ": ";

                    }
                    else
                    {
                        this.UniversalMatrixBox[i].ReloadMatrix(sys.current_state.analog_B()[0][j]);
                        this.LabelList[i].Text = "Bн" + j.ToString() + ": ";
                        j++;
                    }
                    UniversalMatrixBox[i].Visible = true;
                    loctemp += UniversalMatrixBox[i].Size.Width + 20;
                }

                button1.Location = new Point(20, UniversalMatrixBox[0].Location.Y + 20 + UniversalMatrixBox[0].Size.Height);
                button1.Visible = true;
            }
        }

        private void showStateInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SI == null)
                SI = new SysInfo(sys);
            else
                SI.UpdateInfo(sys);
            this.SI.Show();
        }



        private void TabChanging(object sender, TabControlCancelEventArgs e)
        {

            if ((e.TabPageIndex > step && e.TabPageIndex != 4) || (e.TabPageIndex == 4 && sys != null))
            {
                e.Cancel = true;   // блокировка вкладки
                System.Text.StringBuilder messageBoxCS = new System.Text.StringBuilder();
                if (e.TabPageIndex == 4 && sys != null)
                {
                    messageBoxCS.AppendFormat("Тестовый пример уже загружен!!!");
                    messageBoxCS.AppendLine();

                }
                else if (step == 0)
                {
                    messageBoxCS.AppendFormat("Не выполнена дискретизация!!!");
                    messageBoxCS.AppendLine();
                    
                }
                else if (step == 1) 
                {
                    messageBoxCS.AppendFormat("Система не полностью управляема!!!");
                    messageBoxCS.AppendLine();

                }
                else if (step == 2)
                {
                    messageBoxCS.AppendFormat("Система не полностью наблюдаема!!!");
                    messageBoxCS.AppendLine();

                }

                
                MessageBox.Show(messageBoxCS.ToString(), "Selecting Event");
            }
        }

        private void startEmulationToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            if (this.startEmulationToolStripMenuItem.Text == "Start Emulation")
            {

                //удалем панели, если они уже есть
                if (GUI.Panes != null)
                    for (int i = 0; i < GUI.Panes.Length; i++)
                        this.tabPage4.Controls.Remove(GUI.Panes[i]);
                if(GUI.quality != null)
                    this.tabPage4.Controls.Remove(GUI.quality);
                //удалем метки панелей, если они уже есть
                if (GUI.PanesLabels != null)
                    for (int i = 0; i < GUI.PanesLabels.Length; i++)
                        this.tabPage4.Controls.Remove(GUI.PanesLabels[i]);
                //накидываем панели
                GUI.InitPlotters(3, new Point(10, 125), sys);
                for (int i = 0; i < GUI.Panes.Length; i++)
                    this.tabPage4.Controls.Add(GUI.Panes[i]);
                //график качества
                this.tabPage4.Controls.Add(GUI.quality);
                //накидываем метки для панелей
                for (int i = 0; i < GUI.PanesLabels.Length; i++)
                    this.tabPage4.Controls.Add(GUI.PanesLabels[i]);
                //вызываем событие изменения размера для перерисовки
                this.OnSizeChanged(e);
                //изменение текста кнопки
                this.startEmulationToolStripMenuItem.Text = "Stop Emulation";
                //принудительный стоп таймера, чтобы дать возможность поставить тип аппроксимации и случайные воздействия
                this.IsTimerEnabled.Checked = false;
                this.IsTimerEnabled_CheckedChanged(this,e);
                //включаем интерфейс
                this.ControlPanel.Visible = true;
                //отключаем открытие файла
                this.openToolStripMenuItem.Enabled = false;
            }
            else
            {
                //удаляем панели
                for (int i = 0; i < GUI.Panes.Length; i++)
                    this.tabPage4.Controls.Remove(GUI.Panes[i]);
                this.tabPage4.Controls.Remove(GUI.quality);
                //удаляем метки
                if (GUI.PanesLabels != null)
                    for (int i = 0; i < GUI.PanesLabels.Length; i++)
                        this.tabPage4.Controls.Remove(GUI.PanesLabels[i]);
                //останавливаем время
                this.IsTimerEnabled.Checked = false;
                //меняем название кнопки меню
                this.startEmulationToolStripMenuItem.Text = "Start Emulation";
                //убираем панель управления
                this.ControlPanel.Visible = false;
                //включаем открытие файла
                this.openToolStripMenuItem.Enabled = true;

            }

            Form1_ResizeEnd(sender, e);

        }
        //изменение размера
        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            if (startEmulationToolStripMenuItem.Text != "Start Emulation")
            {

                GUI.ResizePaneAsForm(this.Size);
                this.ControlPanel.Width = this.Width;
                GUI.redraw();
            }
        }

        private void TimerIntervalScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            this.TimerIntervalValue.Text = this.TimerIntervalScrollBar.Value.ToString();
            this.MainCalcTimer.Interval = this.TimerIntervalScrollBar.Value;
        }

        private void IsTimerEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (this.IsTimerEnabled.Checked && !this.MainCalcTimer.Enabled)
            {
                this.MainCalcTimer.Start();
                this.NextStepHandle.Enabled = false;
            }
            if (!this.IsTimerEnabled.Checked && this.MainCalcTimer.Enabled)
            {
                this.MainCalcTimer.Stop();
                this.NextStepHandle.Enabled = true;
            }
        }

        private void IsSplainEnabled_CheckedChanged(object sender, EventArgs e)
        {
            this.IsTrapezeEnabled.Checked = this.IsSplainEnabled.Checked ? false : true;

            if (this.IsSplainEnabled.Checked)
            {
                isSpline = true;
            }
            else
            {
                isSpline = false;
            }
        }

        private void IsTrapezeEnabled_CheckedChanged(object sender, EventArgs e)
        {
            this.IsSplainEnabled.Checked = this.IsTrapezeEnabled.Checked ? false : true;

            if (this.IsTrapezeEnabled.Checked)
            {
                isSpline = false;
            }
            else
            {
                isSpline = true;
            }
        }

        private void IsAccidentalExposure_CheckedChanged(object sender, EventArgs e)
        {
            if (this.IsAccidentalExposure.Checked)
            {
                isNoiseEnabled = true;
            }
            else
            {
                isNoiseEnabled = false;
            }
        }

        
        /// <summary>
        /// Следующий шаг и по кнопке и по таймеру
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainCalcTimer_Tick(object sender, EventArgs e)
        {
            //моргалка подписью таймера
            if (this.TimerIntervalValue.Enabled)
                this.TimerIntervalValue.Enabled = false;
            else
                this.TimerIntervalValue.Enabled = true;
            try//отлов событий из системы
            {
                //следующий шаг системы к светлому будующему. данные пока неоткуда.
                sys.next_stage(DataGetter.GetData(sys.current_state));
            }
            catch (Exception excption)//если при определении следующего шага чтото пошло нетак
            {
                IsTimerEnabled.Checked = false;//останавливаем время
                MessageBox.Show(excption.Message + "\n" + excption.StackTrace.ToString(), "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);//вываливаем сообщение     
            }
            if (SI == null)
                SI = new SysInfo(sys);
            else
                SI.UpdateInfo(sys);
            //грузим новые графики
            GUI.LoadPanes(sys);
            //обновляем графики
            GUI.redraw();
        }
        
        /// <summary>
        /// фиксит отрисувку в режиме ожидания - это действие при перерисовке окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainNeco_Paint(object sender, PaintEventArgs e)
        {
            if (this.startEmulationToolStripMenuItem.Text == "Stop Emulation")
                GUI.redraw();
        }

        private void MainNeco_Restart()
        {
            if (this.MainCalcTimer.Enabled)
            {
                this.MainCalcTimer.Stop();
                this.MainCalcTimer.Start();
            }
        }

        /// <summary>
        /// Дискретизация
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(lDEquation);
            int loctemp = UniversalMatrixBox[0].Location.Y + 60 + UniversalMatrixBox[0].Size.Height;
            label2.Location = new Point(20, loctemp);
            label2.Visible = true;
            this.lDEquation.Text = "x(k+1)= ";
            this.lDEquation.Location = new Point(10, loctemp + 45);

            for (int i = 0; i < sys.current_state.get_A()[0].Count; i++)
            {
                if (i != 0) this.lDEquation.Text += " + "; 
                this.lDEquation.Text += "А" + i.ToString() + "x(k";
                if (i == 0) this.lDEquation.Text += ")";
                else this.lDEquation.Text += " - " + sys.current_state.L[i] + ")";

            }

            for (int i = 0; i < sys.current_state.B()[0].Count; i++)
            {
                this.lDEquation.Text += " + B" + i.ToString() + "u(k";
                if (i == 0) this.lDEquation.Text += ")";
                else this.lDEquation.Text += " - " + sys.current_state.M[i] + ")";

            }


            loctemp += 70;
            int loctemp2 = 20;

            UniversalMatrixBox2 = new List<MatrixDrawer>();
            LabelList2 = new List<Label>();


            int j = 0;
            for (int i = 0; i < sys.current_state.B()[0].Count + sys.current_state.get_A()[0].Count; i++)
            {
                MatrixDrawer tmp = new MatrixDrawer(new MatrixLibrary.Matrix(10, 10));

                this.UniversalMatrixBox2.Add(new MatrixDrawer(new MatrixLibrary.Matrix(10, 10)));
                this.LabelList2.Add(new Label());


                UniversalMatrixBox2[i].Visible = false;
                UniversalMatrixBox2[i].Location = new Point(loctemp2, loctemp + 15);
                this.LabelList2[i].Location = new Point(loctemp2, loctemp);
                this.LabelList2[i].Font = new Font(this.Font, FontStyle.Bold);


                this.tabPage1.Controls.Add(UniversalMatrixBox2[i]);
                this.tabPage1.Controls.Add(LabelList2[i]);


                if (i < sys.current_state.get_A()[0].Count)
                {
                    this.UniversalMatrixBox2[i].ReloadMatrix(sys.current_state.get_A()[0][i]);
                    this.LabelList2[i].Text = "А" + i.ToString() + ": ";

                }
                else
                {
                    this.UniversalMatrixBox2[i].ReloadMatrix(sys.current_state.B()[0][j]);
                    this.LabelList2[i].Text = "B" + j.ToString() + ": ";
                    j++;
                }
                UniversalMatrixBox2[i].Visible = true;
                loctemp2 += UniversalMatrixBox2[i].Size.Width + 20;

                if (!debug)  step = 1; // дискретизция закончилась


            }

        
        }

        /// <summary>
        /// Управляемость
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {

            int locationX = 10;
            int locationY = 160;
            String Matrix1Name = "";
            String Matrix2Name = "";


            tabPage2.Controls.Clear();
            tabPage2.Controls.Add(button2);
            tabPage2.Controls.Add(label3);
            tabPage2.Controls.Add(label6);
            tabPage2.Controls.Add(label4);
            tabPage2.Controls.Add(label7);

            if (!sys.current_state.stationarity)
            {
                tabPage2.Controls.Add(label8);               
                tabPage2.Controls.Add(label5);
            }
            
            tabPage2.Controls.Add(label9);

            Controllability.IsSystemControllability(ref sys.current_state);

            if (sys.current_state.stationarity)
            {
                Matrix1Name = " Vr";
                Matrix2Name = " Va";
            } 


            label3.Location = new Point(locationX, locationY - 60);

            this.UniversalMatrixBox3 = new MatrixDrawer(new MatrixLibrary.Matrix(20, 20));
            label4.Text = "Ранг" + Matrix1Name + ": " + Matrix.Rank(Controllability.GR).ToString();
            UniversalMatrixBox3.Visible = false;
            UniversalMatrixBox3.Location = new Point(locationX, locationY);
            label4.Location = new Point(locationX, locationY - 25);
            this.tabPage2.Controls.Add(UniversalMatrixBox3);
            this.UniversalMatrixBox3.ReloadMatrix(Controllability.GR);
            UniversalMatrixBox3.Visible = true;

            if (sys.current_state.stationarity)
            {
                label4.Text += "    |    n = " + sys.current_state.n.ToString();
            }
            else
            {

                locationX += UniversalMatrixBox3.Width + 20;

                this.UniversalMatrixBox4 = new MatrixDrawer(new MatrixLibrary.Matrix(20, 20));
                label8.Text = "Ранг: " + Matrix.Rank(Controllability.temp_GR).ToString();
                UniversalMatrixBox4.Visible = false;
                UniversalMatrixBox4.Location = new Point(locationX, locationY);
                label8.Location = new Point(locationX, locationY - 25);
                this.tabPage2.Controls.Add(UniversalMatrixBox4);
                this.UniversalMatrixBox4.ReloadMatrix(Controllability.temp_GR);
                UniversalMatrixBox4.Visible = true;
            }
            locationX = 10;
            locationY += UniversalMatrixBox3.Height + 80;

            label6.Location = new Point(locationX, locationY - 60);

            this.UniversalMatrixBox5 = new MatrixDrawer(new MatrixLibrary.Matrix(20, 20));
            label7.Text = "Ранг" + Matrix2Name + ": " + Matrix.Rank(Controllability.G_).ToString();
            UniversalMatrixBox5.Visible = false;
            UniversalMatrixBox5.Location = new Point(locationX, locationY);
            label7.Location = new Point(locationX, locationY - 25);
            this.tabPage2.Controls.Add(UniversalMatrixBox5);
            this.UniversalMatrixBox5.ReloadMatrix(Controllability.G_);
            UniversalMatrixBox5.Visible = true;

            if (sys.current_state.stationarity)
            {
                int tmp = sys.current_state.n * (sys.current_state.L[sys.current_state.a - 1] + 1);
                label7.Text += "    |    n(La+1) = " + tmp.ToString();
            }
            else
            {
                locationX += UniversalMatrixBox5.Width + 20;

                this.UniversalMatrixBox6 = new MatrixDrawer(new MatrixLibrary.Matrix(20, 20));
                label5.Text = "Ранг: " + Matrix.Rank(Controllability.temp_G_).ToString();
                UniversalMatrixBox6.Visible = false;
                UniversalMatrixBox6.Location = new Point(locationX, locationY);
                label5.Location = new Point(locationX, locationY - 25);
                this.tabPage2.Controls.Add(UniversalMatrixBox6);
                this.UniversalMatrixBox6.ReloadMatrix(Controllability.temp_G_);
                UniversalMatrixBox6.Visible = true;
            }
            if (sys.current_state.full_controllability)
            {
                label9.ForeColor = Color.Green;
                label9.Text = "Система полностью управляема";
                if (!debug) step = 2;
            }
            else
            {
                label9.ForeColor = Color.Red;
                if (sys.current_state.part_controllability) label9.Text = "Система частично управляема";
                else label9.Text = "Система не управляема";
            }
        }


        /// <summary>
        /// Наблюдаемость
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_observ_Click(object sender, EventArgs e)
        {
            int locationX = 10;
            int locationY = 160;
            tabPage3.Controls.Clear();
            tabPage3.Controls.Add(button_observ);
            tabPage3.Controls.Add(label_IsObserv);
            tabPage3.Controls.Add(label_FullObserv);
            tabPage3.Controls.Add(label_PartObserv);

            if (!sys.current_state.stationarity)
            {
                tabPage3.Controls.Add(label_Sa);
                tabPage3.Controls.Add(label_Sr);
            }
                

            tabPage3.Controls.Add(label_Va);
            tabPage3.Controls.Add(label_Vr);


            Observability.IsSystemObservability(ref sys.current_state);

            label_PartObserv.Location = new Point(locationX, locationY - 60);

            this.VrMatrixBox = new MatrixDrawer(new MatrixLibrary.Matrix(20, 20));
            label_Vr.Text = "Ранг Vr: " + Matrix.Rank(Observability.tmpVr).ToString();
            VrMatrixBox.Location = new Point(locationX, locationY);
            label_Vr.Location = new Point(locationX, locationY - 25);
            this.tabPage3.Controls.Add(VrMatrixBox);
            this.VrMatrixBox.ReloadMatrix(Observability.tmpVr);
            if (sys.current_state.stationarity)
            {
                label_Vr.Text += "    |    n = " + sys.current_state.n.ToString();
            }
            else
            {

                locationX += VrMatrixBox.Width + 20;

                this.SrMatrixBox = new MatrixDrawer(new MatrixLibrary.Matrix(20, 20));
                label_Sr.Text = "Ранг Sr: " + Matrix.Rank(Observability.tmpSr).ToString();
                SrMatrixBox.Location = new Point(locationX, locationY);
                label_Sr.Location = new Point(locationX, locationY - 25);
                this.tabPage3.Controls.Add(SrMatrixBox);
                this.SrMatrixBox.ReloadMatrix(Observability.tmpSr);

            }

            locationX = 10;
            locationY += VrMatrixBox.Height + 80;

            label_FullObserv.Location = new Point(locationX, locationY - 60);

            this.VaMatrixBox = new MatrixDrawer(new MatrixLibrary.Matrix(20, 20));
            label_Va.Text = "Ранг Va: " + Matrix.Rank(Observability.tmpVa).ToString();
            VaMatrixBox.Location = new Point(locationX, locationY);
            label_Va.Location = new Point(locationX, locationY - 25);
            this.tabPage3.Controls.Add(VaMatrixBox);
            this.VaMatrixBox.ReloadMatrix(Observability.tmpVa);

            if (sys.current_state.stationarity)
            {
                int tmp = sys.current_state.n * (Observability.J + 1);
                label_Va.Text += "    |    n(J+1) = " +  tmp.ToString();
            }
            else
            {

                locationX += VaMatrixBox.Width + 20;

                this.SaMatrixBox = new MatrixDrawer(new MatrixLibrary.Matrix(20, 20));
                label_Sa.Text = "Ранг Sa: " + Matrix.Rank(Observability.tmpSa).ToString();
                SaMatrixBox.Location = new Point(locationX, locationY);
                label_Sa.Location = new Point(locationX, locationY - 25);
                this.tabPage3.Controls.Add(SaMatrixBox);
                this.SaMatrixBox.ReloadMatrix(Observability.tmpSa);

            }

            if (sys.current_state.full_observersability)
            {
                label_IsObserv.ForeColor = Color.Green;
                label_IsObserv.Text = "Система полностью наблюдаема";
                if (!debug) step = 3;
            }

            else
                
            {   label_IsObserv.ForeColor = Color.Red;
                if (sys.current_state.part_observersability) label_IsObserv.Text = "Система частично наблюдаема";
                else label_IsObserv.Text = "Система не наблюдаема";
            }
        }

        /// <summary>
        /// Начать регулирование
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_regulation_Click(object sender, EventArgs e)
        {
            if (this.startEmulationToolStripMenuItem.Text == "Start Emulation")
            {
                button_regulation.Text = "Остановить управление";

            }
            else button_regulation.Text = "Начать управление";

            startEmulationToolStripMenuItem_Click(sender, e);
            
        }

        private void CreateToolStripMenuItem_Click(object sender, EventArgs e)
        {
           Form CF1 = new CreateF1();
           CF1.Show();
           CF1.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.F1_Closed);
        }

        private void CreateSys()
        {
            sys = new ESystem(CreateF1.sys1.current_state);
        
            if (sys != null)
            {
                //грузим полученный sys в формочку
                if (SI == null && sys != null)//проверка на открытие файла снова
                {
                    SI = new SysInfo(sys);
                }
                else
                    SI.UpdateInfo(sys);
                //разблокировка элементов
                this.showStateInfoToolStripMenuItem.Enabled = true;
                this.startEmulationToolStripMenuItem.Enabled = true;

                tabControl1.SelectTab(0);
                if (!debug) step = 0; else step = 4;
                tabPage2.Controls.Clear();
                tabPage2.Controls.Add(button2);

                tabPage1.Controls.Clear();
                tabPage1.Controls.Add(button1);
                tabPage1.Controls.Add(label1);
                tabPage1.Controls.Add(lAnalogEquation);
                lAnalogEquation.Text = "x'(t) = ";

                for (int i = 0; i < sys.current_state.analog_A()[0].Count; i++)
                {
                    if (i != 0) this.lAnalogEquation.Text += " + ";
                    this.lAnalogEquation.Text += "Ан" + i.ToString() + "x(t";
                    if (i == 0) this.lAnalogEquation.Text += ")";
                    else this.lAnalogEquation.Text += " - " + sys.current_state.tau[i] + ")";


                }

                for (int i = 0; i < sys.current_state.analog_B()[0].Count; i++)
                {
                    this.lAnalogEquation.Text += " + Bн" + i.ToString() + "u(t";
                    if (i == 0) this.lAnalogEquation.Text += ")";
                    else this.lAnalogEquation.Text += " - " + sys.current_state.teta[i] + ")";

                }


                UniversalMatrixBox = new List<MatrixDrawer>();
                LabelList = new List<Label>();

                int loctemp = 20;
                int j = 0;
                for (int i = 0; i < sys.current_state.analog_B()[0].Count + sys.current_state.analog_A()[0].Count; i++)
                {

                    MatrixDrawer tmp = new MatrixDrawer(new MatrixLibrary.Matrix(10, 10));

                    this.UniversalMatrixBox.Add(new MatrixDrawer(new MatrixLibrary.Matrix(10, 10)));
                    this.LabelList.Add(new Label());


                    UniversalMatrixBox[i].Visible = false;
                    UniversalMatrixBox[i].Location = new Point(loctemp, 130);
                    this.LabelList[i].Location = new Point(loctemp, 115);
                    this.LabelList[i].Font = new Font(this.Font, FontStyle.Bold);


                    this.tabPage1.Controls.Add(UniversalMatrixBox[i]);
                    this.tabPage1.Controls.Add(LabelList[i]);


                    if (i < sys.current_state.analog_A()[0].Count)
                    {
                        this.UniversalMatrixBox[i].ReloadMatrix(sys.current_state.analog_A()[0][i]);
                        this.LabelList[i].Text = "Ан" + i.ToString() + ": ";

                    }
                    else
                    {
                        this.UniversalMatrixBox[i].ReloadMatrix(sys.current_state.analog_B()[0][j]);
                        this.LabelList[i].Text = "Bн" + j.ToString() + ": ";
                        j++;
                    }
                    UniversalMatrixBox[i].Visible = true;
                    loctemp += UniversalMatrixBox[i].Size.Width + 20;
                }

                button1.Location = new Point(20, UniversalMatrixBox[0].Location.Y + 20 + UniversalMatrixBox[0].Size.Height);
                button1.Visible = true;
            }
        }


        private void F1_Closed(object sender, EventArgs e)
        {
            if (CreateF1.sys1 != null)
            {
                CreateSys();
            }
        }


    }
}
