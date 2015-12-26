using System.Collections.Generic;
namespace neco
{
    partial class ViewerG
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
            this.SuspendLayout();
            // 
            // ViewerG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(828, 217);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ViewerG";
            this.Text = "Матрицы G";
            this.ResumeLayout(false);

        }

        #endregion


        //коробка для рисования матрицы
        private List<MatrixDrawer> UniversalMatrixBox_gx;
        private List<MatrixDrawer> UniversalMatrixBox_gu;
        
        private List<System.Windows.Forms.Label> LabelList_gx;
        private List<System.Windows.Forms.Label> LabelList_gu;
    }
}
