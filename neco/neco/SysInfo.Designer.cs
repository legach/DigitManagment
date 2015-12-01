namespace neco
{
    partial class SysInfo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Stationarity = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.part_observability = new System.Windows.Forms.Label();
            this.Perturbation = new System.Windows.Forms.Label();
            this.Determinancy = new System.Windows.Forms.Label();
            this.full_observability = new System.Windows.Forms.Label();
            this.delay_by_control = new System.Windows.Forms.Label();
            this.full_controllability = new System.Windows.Forms.Label();
            this.part_controllability = new System.Windows.Forms.Label();
            this.delay_by_state = new System.Windows.Forms.Label();
            this.Syncronism = new System.Windows.Forms.Label();
            this.MatrixL = new System.Windows.Forms.Label();
            this.TimeL = new System.Windows.Forms.Label();
            this.DelayL = new System.Windows.Forms.Label();
            this.MatrixBox = new System.Windows.Forms.ComboBox();
            this.TimeBox = new System.Windows.Forms.ComboBox();
            this.DelayBox = new System.Windows.Forms.ComboBox();
            this.Sizes = new System.Windows.Forms.Label();
            this.LoadMatrix = new System.Windows.Forms.Button();
            this.Hider = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Stationarity
            // 
            this.Stationarity.AutoSize = true;
            this.Stationarity.Location = new System.Drawing.Point(6, 16);
            this.Stationarity.Name = "Stationarity";
            this.Stationarity.Size = new System.Drawing.Size(73, 13);
            this.Stationarity.TabIndex = 0;
            this.Stationarity.Text = "Стационарна";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.part_observability);
            this.groupBox1.Controls.Add(this.Perturbation);
            this.groupBox1.Controls.Add(this.Determinancy);
            this.groupBox1.Controls.Add(this.full_observability);
            this.groupBox1.Controls.Add(this.delay_by_control);
            this.groupBox1.Controls.Add(this.full_controllability);
            this.groupBox1.Controls.Add(this.part_controllability);
            this.groupBox1.Controls.Add(this.delay_by_state);
            this.groupBox1.Controls.Add(this.Syncronism);
            this.groupBox1.Controls.Add(this.Stationarity);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(190, 152);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Тип системы";
            // 
            // part_observability
            // 
            this.part_observability.AutoSize = true;
            this.part_observability.Location = new System.Drawing.Point(6, 107);
            this.part_observability.Name = "part_observability";
            this.part_observability.Size = new System.Drawing.Size(122, 13);
            this.part_observability.TabIndex = 11;
            this.part_observability.Text = "Частично наблюдаема";
            // 
            // Perturbation
            // 
            this.Perturbation.AutoSize = true;
            this.Perturbation.Location = new System.Drawing.Point(6, 133);
            this.Perturbation.Name = "Perturbation";
            this.Perturbation.Size = new System.Drawing.Size(98, 13);
            this.Perturbation.TabIndex = 10;
            this.Perturbation.Text = "Есть возмущения";
            // 
            // Determinancy
            // 
            this.Determinancy.AutoSize = true;
            this.Determinancy.Location = new System.Drawing.Point(6, 120);
            this.Determinancy.Name = "Determinancy";
            this.Determinancy.Size = new System.Drawing.Size(107, 13);
            this.Determinancy.TabIndex = 9;
            this.Determinancy.Text = "Детерменированна";
            // 
            // full_observability
            // 
            this.full_observability.AutoSize = true;
            this.full_observability.Location = new System.Drawing.Point(6, 94);
            this.full_observability.Name = "full_observability";
            this.full_observability.Size = new System.Drawing.Size(131, 13);
            this.full_observability.TabIndex = 8;
            this.full_observability.Text = "Полностью наблюдаема";
            // 
            // delay_by_control
            // 
            this.delay_by_control.AutoSize = true;
            this.delay_by_control.Location = new System.Drawing.Point(6, 55);
            this.delay_by_control.Name = "delay_by_control";
            this.delay_by_control.Size = new System.Drawing.Size(161, 13);
            this.delay_by_control.TabIndex = 7;
            this.delay_by_control.Text = "Запаздывание по управлению";
            // 
            // full_controllability
            // 
            this.full_controllability.AutoSize = true;
            this.full_controllability.Location = new System.Drawing.Point(6, 68);
            this.full_controllability.Name = "full_controllability";
            this.full_controllability.Size = new System.Drawing.Size(128, 13);
            this.full_controllability.TabIndex = 6;
            this.full_controllability.Text = "Полностью управляема";
            // 
            // part_controllability
            // 
            this.part_controllability.AutoSize = true;
            this.part_controllability.Location = new System.Drawing.Point(6, 81);
            this.part_controllability.Name = "part_controllability";
            this.part_controllability.Size = new System.Drawing.Size(119, 13);
            this.part_controllability.TabIndex = 5;
            this.part_controllability.Text = "Частично управляема";
            // 
            // delay_by_state
            // 
            this.delay_by_state.AutoSize = true;
            this.delay_by_state.Location = new System.Drawing.Point(6, 42);
            this.delay_by_state.Name = "delay_by_state";
            this.delay_by_state.Size = new System.Drawing.Size(181, 13);
            this.delay_by_state.TabIndex = 4;
            this.delay_by_state.Text = "Есть запаздывание по состоянию";
            // 
            // Syncronism
            // 
            this.Syncronism.AutoSize = true;
            this.Syncronism.Location = new System.Drawing.Point(6, 29);
            this.Syncronism.Name = "Syncronism";
            this.Syncronism.Size = new System.Drawing.Size(61, 13);
            this.Syncronism.TabIndex = 3;
            this.Syncronism.Text = "Синхронна";
            // 
            // MatrixL
            // 
            this.MatrixL.AutoSize = true;
            this.MatrixL.Location = new System.Drawing.Point(209, 12);
            this.MatrixL.Name = "MatrixL";
            this.MatrixL.Size = new System.Drawing.Size(51, 13);
            this.MatrixL.TabIndex = 3;
            this.MatrixL.Text = "Матрица";
            // 
            // TimeL
            // 
            this.TimeL.AutoSize = true;
            this.TimeL.Location = new System.Drawing.Point(266, 12);
            this.TimeL.Name = "TimeL";
            this.TimeL.Size = new System.Drawing.Size(40, 13);
            this.TimeL.TabIndex = 4;
            this.TimeL.Text = "Время";
            // 
            // DelayL
            // 
            this.DelayL.AutoSize = true;
            this.DelayL.Location = new System.Drawing.Point(312, 12);
            this.DelayL.Name = "DelayL";
            this.DelayL.Size = new System.Drawing.Size(58, 13);
            this.DelayL.TabIndex = 5;
            this.DelayL.Text = "Задержка";
            // 
            // MatrixBox
            // 
            this.MatrixBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MatrixBox.FormattingEnabled = true;
            this.MatrixBox.Items.AddRange(new object[] {
            "An",
            "Bn",
            "A",
            "B",
            "H",
            "L",
            "M",
            "tau",
            "teta",
            "Fi",
            "Psi"});
            this.MatrixBox.Location = new System.Drawing.Point(212, 28);
            this.MatrixBox.Name = "MatrixBox";
            this.MatrixBox.Size = new System.Drawing.Size(48, 21);
            this.MatrixBox.TabIndex = 6;
            this.MatrixBox.SelectedIndexChanged += new System.EventHandler(this.MatrixBox_SelectedIndexChanged);
            // 
            // TimeBox
            // 
            this.TimeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TimeBox.Enabled = false;
            this.TimeBox.FormattingEnabled = true;
            this.TimeBox.Location = new System.Drawing.Point(266, 28);
            this.TimeBox.Name = "TimeBox";
            this.TimeBox.Size = new System.Drawing.Size(48, 21);
            this.TimeBox.TabIndex = 7;
            // 
            // DelayBox
            // 
            this.DelayBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DelayBox.Enabled = false;
            this.DelayBox.FormattingEnabled = true;
            this.DelayBox.Location = new System.Drawing.Point(320, 28);
            this.DelayBox.Name = "DelayBox";
            this.DelayBox.Size = new System.Drawing.Size(48, 21);
            this.DelayBox.TabIndex = 8;
            // 
            // Sizes
            // 
            this.Sizes.AutoSize = true;
            this.Sizes.Location = new System.Drawing.Point(12, 167);
            this.Sizes.Name = "Sizes";
            this.Sizes.Size = new System.Drawing.Size(0, 13);
            this.Sizes.TabIndex = 10;
            // 
            // LoadMatrix
            // 
            this.LoadMatrix.Enabled = false;
            this.LoadMatrix.Location = new System.Drawing.Point(374, 28);
            this.LoadMatrix.Name = "LoadMatrix";
            this.LoadMatrix.Size = new System.Drawing.Size(62, 21);
            this.LoadMatrix.TabIndex = 11;
            this.LoadMatrix.Text = "Поехали!";
            this.LoadMatrix.UseVisualStyleBackColor = true;
            this.LoadMatrix.Click += new System.EventHandler(this.LoadMatrix_Click);
            // 
            // Hider
            // 
            this.Hider.Location = new System.Drawing.Point(376, -1);
            this.Hider.Name = "Hider";
            this.Hider.Size = new System.Drawing.Size(60, 23);
            this.Hider.TabIndex = 12;
            this.Hider.Text = "Скрыть";
            this.Hider.UseVisualStyleBackColor = true;
            this.Hider.Click += new System.EventHandler(this.Hider_Click);
            // 
            // SysInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 189);
            this.ControlBox = false;
            this.Controls.Add(this.Hider);
            this.Controls.Add(this.LoadMatrix);
            this.Controls.Add(this.Sizes);
            this.Controls.Add(this.DelayBox);
            this.Controls.Add(this.TimeBox);
            this.Controls.Add(this.MatrixBox);
            this.Controls.Add(this.DelayL);
            this.Controls.Add(this.TimeL);
            this.Controls.Add(this.MatrixL);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimumSize = new System.Drawing.Size(450, 214);
            this.Name = "SysInfo";
            this.Text = "SysInfo";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Stationarity;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label Perturbation;
        private System.Windows.Forms.Label Determinancy;
        private System.Windows.Forms.Label full_observability;
        private System.Windows.Forms.Label delay_by_control;
        private System.Windows.Forms.Label full_controllability;
        private System.Windows.Forms.Label part_controllability;
        private System.Windows.Forms.Label delay_by_state;
        private System.Windows.Forms.Label Syncronism;
        private System.Windows.Forms.Label MatrixL;
        private System.Windows.Forms.Label TimeL;
        private System.Windows.Forms.Label DelayL;
        private System.Windows.Forms.ComboBox MatrixBox;
        private System.Windows.Forms.ComboBox TimeBox;
        private System.Windows.Forms.ComboBox DelayBox;
        private System.Windows.Forms.Label Sizes;
        //коробка для рисования матрицы
        private TBMatrixDrawer UniversalMatrixBox;
        private System.Windows.Forms.Button LoadMatrix;
        private System.Windows.Forms.Button Hider;
        private System.Windows.Forms.Label part_observability;
    }
}