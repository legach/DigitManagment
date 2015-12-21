using System.Collections;
using System.Collections.Generic;


namespace neco
{
    partial class MainNeco
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CreateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startEmulationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showStateInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainCalcTimer = new System.Windows.Forms.Timer(this.components);
            this.TimerIntervalScrollBar = new System.Windows.Forms.HScrollBar();
            this.TimerIntervalLabel = new System.Windows.Forms.Label();
            this.TimerIntervalValue = new System.Windows.Forms.Label();
            this.IsTimerEnabled = new System.Windows.Forms.CheckBox();
            this.NextStepHandle = new System.Windows.Forms.Button();
            this.ControlPanel = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.lDEquation = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.button_observ = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.button_regulation = new System.Windows.Forms.Button();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.IsAccidentalExposure = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.IsSplainEnabled = new System.Windows.Forms.CheckBox();
            this.IsTrapezeEnabled = new System.Windows.Forms.CheckBox();
            this.label_FullObserv = new System.Windows.Forms.Label();
            this.label_PartObserv = new System.Windows.Forms.Label();
            this.label_Sa = new System.Windows.Forms.Label();
            this.label_Va = new System.Windows.Forms.Label();
            this.label_Sr = new System.Windows.Forms.Label();
            this.label_Vr = new System.Windows.Forms.Label();
            this.label_IsObserv = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lAnalogEquation = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.ControlPanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(0);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1116, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreateToolStripMenuItem,
            this.openToolStripMenuItem,
            this.startEmulationToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.fileToolStripMenuItem.Text = "Файл";
            // 
            // CreateToolStripMenuItem
            // 
            this.CreateToolStripMenuItem.Name = "CreateToolStripMenuItem";
            this.CreateToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.CreateToolStripMenuItem.Text = "Создать";
            this.CreateToolStripMenuItem.Click += new System.EventHandler(this.CreateToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "Открыть";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // startEmulationToolStripMenuItem
            // 
            this.startEmulationToolStripMenuItem.Name = "startEmulationToolStripMenuItem";
            this.startEmulationToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.startEmulationToolStripMenuItem.Text = "Start Emulation";
            this.startEmulationToolStripMenuItem.Visible = false;
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showStateInfoToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.editToolStripMenuItem.Text = "Вид";
            // 
            // showStateInfoToolStripMenuItem
            // 
            this.showStateInfoToolStripMenuItem.Enabled = false;
            this.showStateInfoToolStripMenuItem.Name = "showStateInfoToolStripMenuItem";
            this.showStateInfoToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.showStateInfoToolStripMenuItem.Text = "Состояние системы";
            this.showStateInfoToolStripMenuItem.Click += new System.EventHandler(this.showStateInfoToolStripMenuItem_Click);
            // 
            // MainCalcTimer
            // 
            this.MainCalcTimer.Interval = 500;
            this.MainCalcTimer.Tick += new System.EventHandler(this.MainCalcTimer_Tick);
            // 
            // TimerIntervalScrollBar
            // 
            this.TimerIntervalScrollBar.LargeChange = 1000;
            this.TimerIntervalScrollBar.Location = new System.Drawing.Point(100, 0);
            this.TimerIntervalScrollBar.Maximum = 10000;
            this.TimerIntervalScrollBar.Minimum = 500;
            this.TimerIntervalScrollBar.Name = "TimerIntervalScrollBar";
            this.TimerIntervalScrollBar.Size = new System.Drawing.Size(146, 14);
            this.TimerIntervalScrollBar.SmallChange = 100;
            this.TimerIntervalScrollBar.TabIndex = 2;
            this.TimerIntervalScrollBar.Value = 500;
            this.TimerIntervalScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.TimerIntervalScrollBar_Scroll);
            // 
            // TimerIntervalLabel
            // 
            this.TimerIntervalLabel.AutoSize = true;
            this.TimerIntervalLabel.Location = new System.Drawing.Point(3, 0);
            this.TimerIntervalLabel.Name = "TimerIntervalLabel";
            this.TimerIntervalLabel.Size = new System.Drawing.Size(102, 13);
            this.TimerIntervalLabel.TabIndex = 1;
            this.TimerIntervalLabel.Text = "Интервал таймера";
            // 
            // TimerIntervalValue
            // 
            this.TimerIntervalValue.AutoSize = true;
            this.TimerIntervalValue.Location = new System.Drawing.Point(249, 0);
            this.TimerIntervalValue.Name = "TimerIntervalValue";
            this.TimerIntervalValue.Size = new System.Drawing.Size(25, 13);
            this.TimerIntervalValue.TabIndex = 3;
            this.TimerIntervalValue.Text = "100";
            // 
            // IsTimerEnabled
            // 
            this.IsTimerEnabled.AutoSize = true;
            this.IsTimerEnabled.Location = new System.Drawing.Point(6, 17);
            this.IsTimerEnabled.Name = "IsTimerEnabled";
            this.IsTimerEnabled.Size = new System.Drawing.Size(111, 17);
            this.IsTimerEnabled.TabIndex = 5;
            this.IsTimerEnabled.Text = "Таймер включен";
            this.IsTimerEnabled.UseVisualStyleBackColor = true;
            this.IsTimerEnabled.CheckedChanged += new System.EventHandler(this.IsTimerEnabled_CheckedChanged);
            // 
            // NextStepHandle
            // 
            this.NextStepHandle.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.NextStepHandle.Location = new System.Drawing.Point(109, 17);
            this.NextStepHandle.Name = "NextStepHandle";
            this.NextStepHandle.Size = new System.Drawing.Size(165, 18);
            this.NextStepHandle.TabIndex = 6;
            this.NextStepHandle.Text = "следующий шаг";
            this.NextStepHandle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.NextStepHandle.UseVisualStyleBackColor = true;
            this.NextStepHandle.Click += new System.EventHandler(this.MainCalcTimer_Tick);
            // 
            // ControlPanel
            // 
            this.ControlPanel.AutoSize = true;
            this.ControlPanel.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ControlPanel.Controls.Add(this.NextStepHandle);
            this.ControlPanel.Controls.Add(this.IsTimerEnabled);
            this.ControlPanel.Controls.Add(this.TimerIntervalValue);
            this.ControlPanel.Controls.Add(this.TimerIntervalLabel);
            this.ControlPanel.Controls.Add(this.TimerIntervalScrollBar);
            this.ControlPanel.Location = new System.Drawing.Point(3, 34);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Size = new System.Drawing.Size(400, 47);
            this.ControlPanel.TabIndex = 3;
            this.ControlPanel.Visible = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1116, 490);
            this.tabControl1.TabIndex = 5;
            this.tabControl1.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.TabChanging);
            // 
            // tabPage1
            // 
            this.tabPage1.AutoScroll = true;
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.lDEquation);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1108, 464);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Дискретизация";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(37, 206);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(285, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Дискретное уравнение системы:";
            this.label2.Visible = false;
            // 
            // lDEquation
            // 
            this.lDEquation.AutoSize = true;
            this.lDEquation.Location = new System.Drawing.Point(334, 157);
            this.lDEquation.Name = "lDEquation";
            this.lDEquation.Size = new System.Drawing.Size(0, 13);
            this.lDEquation.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(334, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(145, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Выполнить дискретизацию";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.AutoScroll = true;
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1108, 464);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Управляемость";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(21, 47);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(177, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Проверить управляемость";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.AutoScroll = true;
            this.tabPage3.Controls.Add(this.button_observ);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1108, 464);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Наблюдаемость";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // button_observ
            // 
            this.button_observ.Location = new System.Drawing.Point(23, 47);
            this.button_observ.Name = "button_observ";
            this.button_observ.Size = new System.Drawing.Size(177, 23);
            this.button_observ.TabIndex = 0;
            this.button_observ.Text = "Проверить наблюдаемость";
            this.button_observ.UseVisualStyleBackColor = true;
            this.button_observ.Click += new System.EventHandler(this.button_observ_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.zedGraphControl1);
            this.tabPage4.Controls.Add(this.button_regulation);
            this.tabPage4.Controls.Add(this.ControlPanel);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1108, 464);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Регулирование";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Location = new System.Drawing.Point(704, 168);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(396, 287);
            this.zedGraphControl1.TabIndex = 5;
            // 
            // button_regulation
            // 
            this.button_regulation.Dock = System.Windows.Forms.DockStyle.Top;
            this.button_regulation.Location = new System.Drawing.Point(0, 0);
            this.button_regulation.Name = "button_regulation";
            this.button_regulation.Size = new System.Drawing.Size(1108, 31);
            this.button_regulation.TabIndex = 4;
            this.button_regulation.Text = "Начать управление";
            this.button_regulation.UseVisualStyleBackColor = true;
            this.button_regulation.Click += new System.EventHandler(this.button_regulation_Click);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.IsAccidentalExposure);
            this.tabPage5.Controls.Add(this.groupBox1);
            this.tabPage5.Location = new System.Drawing.Point(4, 23);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(1108, 463);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Настройки";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // IsAccidentalExposure
            // 
            this.IsAccidentalExposure.AutoSize = true;
            this.IsAccidentalExposure.Location = new System.Drawing.Point(15, 82);
            this.IsAccidentalExposure.Name = "IsAccidentalExposure";
            this.IsAccidentalExposure.Size = new System.Drawing.Size(142, 17);
            this.IsAccidentalExposure.TabIndex = 1;
            this.IsAccidentalExposure.Text = "Случайное воздейсвие";
            this.IsAccidentalExposure.UseVisualStyleBackColor = true;
            this.IsAccidentalExposure.CheckedChanged += new System.EventHandler(this.IsAccidentalExposure_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.IsSplainEnabled);
            this.groupBox1.Controls.Add(this.IsTrapezeEnabled);
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(396, 70);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Аппроксимация";
            // 
            // IsSplainEnabled
            // 
            this.IsSplainEnabled.AutoSize = true;
            this.IsSplainEnabled.Location = new System.Drawing.Point(7, 45);
            this.IsSplainEnabled.Name = "IsSplainEnabled";
            this.IsSplainEnabled.Size = new System.Drawing.Size(170, 17);
            this.IsSplainEnabled.TabIndex = 1;
            this.IsSplainEnabled.Text = "Сплайновая аппроксимация";
            this.IsSplainEnabled.UseVisualStyleBackColor = true;
            this.IsSplainEnabled.CheckedChanged += new System.EventHandler(this.IsSplainEnabled_CheckedChanged);
            // 
            // IsTrapezeEnabled
            // 
            this.IsTrapezeEnabled.AutoSize = true;
            this.IsTrapezeEnabled.Checked = true;
            this.IsTrapezeEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IsTrapezeEnabled.Location = new System.Drawing.Point(7, 20);
            this.IsTrapezeEnabled.Name = "IsTrapezeEnabled";
            this.IsTrapezeEnabled.Size = new System.Drawing.Size(108, 17);
            this.IsTrapezeEnabled.TabIndex = 0;
            this.IsTrapezeEnabled.Text = "Метод трапеций";
            this.IsTrapezeEnabled.UseVisualStyleBackColor = true;
            this.IsTrapezeEnabled.CheckedChanged += new System.EventHandler(this.IsTrapezeEnabled_CheckedChanged);
            // 
            // label_FullObserv
            // 
            this.label_FullObserv.AutoSize = true;
            this.label_FullObserv.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_FullObserv.Location = new System.Drawing.Point(64, 222);
            this.label_FullObserv.Name = "label_FullObserv";
            this.label_FullObserv.Size = new System.Drawing.Size(179, 19);
            this.label_FullObserv.TabIndex = 7;
            this.label_FullObserv.Text = "Полная наблюдаемость";
            // 
            // label_PartObserv
            // 
            this.label_PartObserv.AutoSize = true;
            this.label_PartObserv.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_PartObserv.Location = new System.Drawing.Point(64, 168);
            this.label_PartObserv.Name = "label_PartObserv";
            this.label_PartObserv.Size = new System.Drawing.Size(236, 19);
            this.label_PartObserv.TabIndex = 6;
            this.label_PartObserv.Text = "Относительная наблюдаемость";
            // 
            // label_Sa
            // 
            this.label_Sa.AutoSize = true;
            this.label_Sa.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_Sa.Location = new System.Drawing.Point(574, 233);
            this.label_Sa.Name = "label_Sa";
            this.label_Sa.Size = new System.Drawing.Size(59, 19);
            this.label_Sa.TabIndex = 5;
            this.label_Sa.Text = "SA.text";
            // 
            // label_Va
            // 
            this.label_Va.AutoSize = true;
            this.label_Va.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_Va.Location = new System.Drawing.Point(397, 222);
            this.label_Va.Name = "label_Va";
            this.label_Va.Size = new System.Drawing.Size(59, 19);
            this.label_Va.TabIndex = 4;
            this.label_Va.Text = "VA.text";
            // 
            // label_Sr
            // 
            this.label_Sr.AutoSize = true;
            this.label_Sr.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_Sr.Location = new System.Drawing.Point(516, 186);
            this.label_Sr.Name = "label_Sr";
            this.label_Sr.Size = new System.Drawing.Size(60, 19);
            this.label_Sr.TabIndex = 3;
            this.label_Sr.Text = "SR.text";
            // 
            // label_Vr
            // 
            this.label_Vr.AutoSize = true;
            this.label_Vr.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_Vr.Location = new System.Drawing.Point(420, 186);
            this.label_Vr.Name = "label_Vr";
            this.label_Vr.Size = new System.Drawing.Size(62, 19);
            this.label_Vr.TabIndex = 2;
            this.label_Vr.Text = "VR.text";
            // 
            // label_IsObserv
            // 
            this.label_IsObserv.AutoSize = true;
            this.label_IsObserv.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_IsObserv.ForeColor = System.Drawing.Color.Red;
            this.label_IsObserv.Location = new System.Drawing.Point(332, 51);
            this.label_IsObserv.Name = "label_IsObserv";
            this.label_IsObserv.Size = new System.Drawing.Size(57, 19);
            this.label_IsObserv.TabIndex = 1;
            this.label_IsObserv.Text = "label10";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(251, 51);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(76, 19);
            this.label9.TabIndex = 8;
            this.label9.Text = "Система:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(210, 118);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 19);
            this.label8.TabIndex = 7;
            this.label8.Text = "Ранг:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(410, 118);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 19);
            this.label7.TabIndex = 6;
            this.label7.Text = "Ранг:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(610, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 19);
            this.label5.TabIndex = 5;
            this.label5.Text = "Ранг:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(410, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(183, 19);
            this.label6.TabIndex = 4;
            this.label6.Text = "Полная управляемость:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(10, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 19);
            this.label4.TabIndex = 2;
            this.label4.Text = "Ранг:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(17, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(240, 19);
            this.label3.TabIndex = 0;
            this.label3.Text = "Относительная управляемость:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(17, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(261, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Непрерывное уравнение системы:";
            // 
            // lAnalogEquation
            // 
            this.lAnalogEquation.AutoSize = true;
            this.lAnalogEquation.Location = new System.Drawing.Point(8, 82);
            this.lAnalogEquation.Name = "lAnalogEquation";
            this.lAnalogEquation.Size = new System.Drawing.Size(41, 14);
            this.lAnalogEquation.TabIndex = 0;
            this.lAnalogEquation.Text = "x`(t) = ";
            // 
            // MainNeco
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(1116, 514);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 250);
            this.Name = "MainNeco";
            this.ShowIcon = false;
            this.Text = "Управление многосвязными объектами";
            this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
            this.SizeChanged += new System.EventHandler(this.Form1_ResizeEnd);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainNeco_Paint);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ControlPanel.ResumeLayout(false);
            this.ControlPanel.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private GUI_Container GUI;
        private SysInfo SI;
        private CreateF1 CF1; 
        private System.Windows.Forms.ToolStripMenuItem showStateInfoToolStripMenuItem;
        private System.Windows.Forms.Timer MainCalcTimer;
        private System.Windows.Forms.HScrollBar TimerIntervalScrollBar;
        private System.Windows.Forms.Label TimerIntervalLabel;
        private System.Windows.Forms.Label TimerIntervalValue;
        private System.Windows.Forms.CheckBox IsTimerEnabled;
        private System.Windows.Forms.Button NextStepHandle;
        private System.Windows.Forms.Panel ControlPanel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label lAnalogEquation;
        private System.Windows.Forms.Label label1;
        private List <MatrixDrawer> UniversalMatrixBox;
        private List<System.Windows.Forms.Label> LabelList;
        private List<MatrixDrawer> UniversalMatrixBox2;
        private List<System.Windows.Forms.Label> LabelList2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lDEquation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private MatrixDrawer UniversalMatrixBox3;
        private System.Windows.Forms.Button button2;
        private MatrixDrawer UniversalMatrixBox4; 
        private MatrixDrawer UniversalMatrixBox5; 
        private MatrixDrawer UniversalMatrixBox6;

        private MatrixDrawer VrMatrixBox;
        private MatrixDrawer SrMatrixBox;
        private MatrixDrawer VaMatrixBox;
        private MatrixDrawer SaMatrixBox;
    
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.CheckBox IsAccidentalExposure;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox IsSplainEnabled;
        private System.Windows.Forms.CheckBox IsTrapezeEnabled;
        private System.Windows.Forms.Label label_IsObserv;
        private System.Windows.Forms.Button button_observ;
        private System.Windows.Forms.Label label_Vr;
        private System.Windows.Forms.Label label_PartObserv;
        private System.Windows.Forms.Label label_Sa;
        private System.Windows.Forms.Label label_Va;
        private System.Windows.Forms.Label label_Sr;
        private System.Windows.Forms.Label label_FullObserv;
        private System.Windows.Forms.Button button_regulation;
        private System.Windows.Forms.ToolStripMenuItem CreateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startEmulationToolStripMenuItem;
        private ZedGraph.ZedGraphControl zedGraphControl1;

    }
}