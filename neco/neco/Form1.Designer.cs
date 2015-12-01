namespace neco
{
    partial class CreateF1
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.FlagNoStationary = new System.Windows.Forms.RadioButton();
            this.FlagStationary = new System.Windows.Forms.RadioButton();
            this.next_button1 = new System.Windows.Forms.Button();
            this.textBox_Tu = new System.Windows.Forms.TextBox();
            this.textBox_Tx = new System.Windows.Forms.TextBox();
            this.lTu = new System.Windows.Forms.Label();
            this.lTx = new System.Windows.Forms.Label();
            this.textBox_c = new System.Windows.Forms.TextBox();
            this.textBox_a = new System.Windows.Forms.TextBox();
            this.textBox_p = new System.Windows.Forms.TextBox();
            this.textBox_m = new System.Windows.Forms.TextBox();
            this.textBox_n = new System.Windows.Forms.TextBox();
            this.lc = new System.Windows.Forms.Label();
            this.la = new System.Windows.Forms.Label();
            this.lp = new System.Windows.Forms.Label();
            this.lm = new System.Windows.Forms.Label();
            this.ln = new System.Windows.Forms.Label();
            this.Done_button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.FlagNoStationary);
            this.splitContainer1.Panel1.Controls.Add(this.FlagStationary);
            this.splitContainer1.Panel1.Controls.Add(this.next_button1);
            this.splitContainer1.Panel1.Controls.Add(this.textBox_Tu);
            this.splitContainer1.Panel1.Controls.Add(this.textBox_Tx);
            this.splitContainer1.Panel1.Controls.Add(this.lTu);
            this.splitContainer1.Panel1.Controls.Add(this.lTx);
            this.splitContainer1.Panel1.Controls.Add(this.textBox_c);
            this.splitContainer1.Panel1.Controls.Add(this.textBox_a);
            this.splitContainer1.Panel1.Controls.Add(this.textBox_p);
            this.splitContainer1.Panel1.Controls.Add(this.textBox_m);
            this.splitContainer1.Panel1.Controls.Add(this.textBox_n);
            this.splitContainer1.Panel1.Controls.Add(this.lc);
            this.splitContainer1.Panel1.Controls.Add(this.la);
            this.splitContainer1.Panel1.Controls.Add(this.lp);
            this.splitContainer1.Panel1.Controls.Add(this.lm);
            this.splitContainer1.Panel1.Controls.Add(this.ln);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.Controls.Add(this.Done_button1);
            this.splitContainer1.Size = new System.Drawing.Size(1088, 627);
            this.splitContainer1.SplitterDistance = 157;
            this.splitContainer1.TabIndex = 0;
            // 
            // FlagNoStationary
            // 
            this.FlagNoStationary.AutoSize = true;
            this.FlagNoStationary.Location = new System.Drawing.Point(665, 67);
            this.FlagNoStationary.Name = "FlagNoStationary";
            this.FlagNoStationary.Size = new System.Drawing.Size(156, 17);
            this.FlagNoStationary.TabIndex = 35;
            this.FlagNoStationary.Text = "Нестационарная система";
            this.FlagNoStationary.UseVisualStyleBackColor = true;
            // 
            // FlagStationary
            // 
            this.FlagStationary.AutoSize = true;
            this.FlagStationary.Checked = true;
            this.FlagStationary.Location = new System.Drawing.Point(665, 43);
            this.FlagStationary.Name = "FlagStationary";
            this.FlagStationary.Size = new System.Drawing.Size(143, 17);
            this.FlagStationary.TabIndex = 34;
            this.FlagStationary.TabStop = true;
            this.FlagStationary.Text = "Стационарная система";
            this.FlagStationary.UseVisualStyleBackColor = true;
            // 
            // next_button1
            // 
            this.next_button1.Location = new System.Drawing.Point(336, 92);
            this.next_button1.Name = "next_button1";
            this.next_button1.Size = new System.Drawing.Size(75, 23);
            this.next_button1.TabIndex = 33;
            this.next_button1.Text = "Далее";
            this.next_button1.UseVisualStyleBackColor = true;
            this.next_button1.Click += new System.EventHandler(this.next_button1_Click_1);
            // 
            // textBox_Tu
            // 
            this.textBox_Tu.Location = new System.Drawing.Point(532, 41);
            this.textBox_Tu.MaxLength = 5;
            this.textBox_Tu.Name = "textBox_Tu";
            this.textBox_Tu.Size = new System.Drawing.Size(38, 20);
            this.textBox_Tu.TabIndex = 32;
            this.textBox_Tu.Text = "1";
            // 
            // textBox_Tx
            // 
            this.textBox_Tx.Location = new System.Drawing.Point(452, 41);
            this.textBox_Tx.MaxLength = 5;
            this.textBox_Tx.Name = "textBox_Tx";
            this.textBox_Tx.Size = new System.Drawing.Size(38, 20);
            this.textBox_Tx.TabIndex = 31;
            this.textBox_Tx.Text = "1";
            // 
            // lTu
            // 
            this.lTu.AutoSize = true;
            this.lTu.Location = new System.Drawing.Point(502, 44);
            this.lTu.Name = "lTu";
            this.lTu.Size = new System.Drawing.Size(29, 13);
            this.lTu.TabIndex = 30;
            this.lTu.Text = "Tu =";
            // 
            // lTx
            // 
            this.lTx.AutoSize = true;
            this.lTx.Location = new System.Drawing.Point(422, 44);
            this.lTx.Name = "lTx";
            this.lTx.Size = new System.Drawing.Size(31, 13);
            this.lTx.TabIndex = 29;
            this.lTx.Text = "Tx = ";
            // 
            // textBox_c
            // 
            this.textBox_c.Location = new System.Drawing.Point(372, 41);
            this.textBox_c.MaxLength = 5;
            this.textBox_c.Name = "textBox_c";
            this.textBox_c.Size = new System.Drawing.Size(38, 20);
            this.textBox_c.TabIndex = 28;
            this.textBox_c.Text = "0";
            // 
            // textBox_a
            // 
            this.textBox_a.Location = new System.Drawing.Point(293, 41);
            this.textBox_a.MaxLength = 5;
            this.textBox_a.Name = "textBox_a";
            this.textBox_a.Size = new System.Drawing.Size(38, 20);
            this.textBox_a.TabIndex = 26;
            this.textBox_a.Text = "0";
            // 
            // textBox_p
            // 
            this.textBox_p.Location = new System.Drawing.Point(213, 41);
            this.textBox_p.MaxLength = 5;
            this.textBox_p.Name = "textBox_p";
            this.textBox_p.Size = new System.Drawing.Size(38, 20);
            this.textBox_p.TabIndex = 25;
            this.textBox_p.Text = "1";
            // 
            // textBox_m
            // 
            this.textBox_m.Location = new System.Drawing.Point(133, 41);
            this.textBox_m.MaxLength = 5;
            this.textBox_m.Name = "textBox_m";
            this.textBox_m.Size = new System.Drawing.Size(38, 20);
            this.textBox_m.TabIndex = 24;
            this.textBox_m.Text = "1";
            // 
            // textBox_n
            // 
            this.textBox_n.Location = new System.Drawing.Point(53, 41);
            this.textBox_n.MaxLength = 5;
            this.textBox_n.Name = "textBox_n";
            this.textBox_n.Size = new System.Drawing.Size(38, 20);
            this.textBox_n.TabIndex = 23;
            this.textBox_n.Text = "1";
            // 
            // lc
            // 
            this.lc.AutoSize = true;
            this.lc.Location = new System.Drawing.Point(347, 44);
            this.lc.Name = "lc";
            this.lc.Size = new System.Drawing.Size(25, 13);
            this.lc.TabIndex = 22;
            this.lc.Text = "c = ";
            // 
            // la
            // 
            this.la.AutoSize = true;
            this.la.Location = new System.Drawing.Point(268, 44);
            this.la.Name = "la";
            this.la.Size = new System.Drawing.Size(25, 13);
            this.la.TabIndex = 20;
            this.la.Text = "a = ";
            // 
            // lp
            // 
            this.lp.AutoSize = true;
            this.lp.Location = new System.Drawing.Point(188, 44);
            this.lp.Name = "lp";
            this.lp.Size = new System.Drawing.Size(25, 13);
            this.lp.TabIndex = 19;
            this.lp.Text = "p = ";
            // 
            // lm
            // 
            this.lm.AutoSize = true;
            this.lm.Location = new System.Drawing.Point(108, 44);
            this.lm.Name = "lm";
            this.lm.Size = new System.Drawing.Size(27, 13);
            this.lm.TabIndex = 18;
            this.lm.Text = "m = ";
            // 
            // ln
            // 
            this.ln.AutoSize = true;
            this.ln.Location = new System.Drawing.Point(28, 44);
            this.ln.Name = "ln";
            this.ln.Size = new System.Drawing.Size(25, 13);
            this.ln.TabIndex = 17;
            this.ln.Text = "n = ";
            // 
            // Done_button1
            // 
            this.Done_button1.Location = new System.Drawing.Point(336, 3);
            this.Done_button1.Name = "Done_button1";
            this.Done_button1.Size = new System.Drawing.Size(75, 23);
            this.Done_button1.TabIndex = 0;
            this.Done_button1.Text = "Завершить";
            this.Done_button1.UseVisualStyleBackColor = true;
            this.Done_button1.Visible = false;
            this.Done_button1.Click += new System.EventHandler(this.Done_button1_Click);
            // 
            // CreateF1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1088, 627);
            this.Controls.Add(this.splitContainer1);
            this.Name = "CreateF1";
            this.Text = "Сведения о системе";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button next_button1;
        private System.Windows.Forms.TextBox textBox_Tu;
        private System.Windows.Forms.TextBox textBox_Tx;
        private System.Windows.Forms.Label lTu;
        private System.Windows.Forms.Label lTx;
        private System.Windows.Forms.TextBox textBox_c;
        private System.Windows.Forms.TextBox textBox_a;
        private System.Windows.Forms.TextBox textBox_p;
        private System.Windows.Forms.TextBox textBox_m;
        private System.Windows.Forms.TextBox textBox_n;
        private System.Windows.Forms.Label lc;
        private System.Windows.Forms.Label la;
        private System.Windows.Forms.Label lp;
        private System.Windows.Forms.Label lm;
        private System.Windows.Forms.Label ln;
        private System.Windows.Forms.Button Done_button1;
        private System.Windows.Forms.RadioButton FlagNoStationary;
        private System.Windows.Forms.RadioButton FlagStationary;


    }
}