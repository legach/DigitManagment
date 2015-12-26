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
    partial class ViewerG : Form
    {
        ESystem sys;
        public ViewerG(ESystem sys)
        {
            this.sys = sys;
            DrawMatrix();
            InitializeComponent();
        }

        private void DrawMatrix()
        {
            String MatrixGxName = "Gx";
            String MatrixGuName = "Gu";

            int locationY_gx = 10;
            int locationX_gx = 10;
            int locationY_gu = 10;
            int locationX_gu = 0;
            //UniversalMatrixBox_new2 = new List<TBMatrixDrawer>();
            UniversalMatrixBox_gx = new List<MatrixDrawer>();
            LabelList_gx = new List<System.Windows.Forms.Label>();

            UniversalMatrixBox_gu = new List<MatrixDrawer>();
            LabelList_gu = new List<System.Windows.Forms.Label>();

            int j = 0;
            for (int i = 0; i < sys.current_state.get_Gx().Count + sys.current_state.get_Gu().Count; i++)
            {
                MatrixDrawer tmp = new MatrixDrawer(new MatrixLibrary.Matrix(10, 10));

                if (i < sys.current_state.get_Gx().Count)
                {
                    this.UniversalMatrixBox_gx.Add(new MatrixDrawer(new MatrixLibrary.Matrix(10, 10)));
                    this.LabelList_gx.Add(new System.Windows.Forms.Label());

                    UniversalMatrixBox_gx[i].Visible = false;
                    UniversalMatrixBox_gx[i].Location = new Point(locationX_gx, locationY_gx + 15);

                    this.LabelList_gx[i].Location = new Point(locationX_gx, locationY_gx);
                    this.LabelList_gx[i].Font = new Font(this.Font, FontStyle.Bold);

                    this.Controls.Add(UniversalMatrixBox_gx[i]);
                    this.Controls.Add(LabelList_gx[i]);

                    this.UniversalMatrixBox_gx[i].ReloadMatrix(sys.current_state.get_Gx()[i]);
                    //this.UniversalMatrixBox_new2[i].ReloadMatrix(sys.current_state.get_Gx()[i]);
                    this.LabelList_gx[i].Text = MatrixGxName + i.ToString() + ": ";

                    UniversalMatrixBox_gx[i].Visible = true;
                    locationX_gx += UniversalMatrixBox_gx[i].Size.Width + 20;
                }
                else if(sys.current_state.get_Gu().Any())
                {
                    this.UniversalMatrixBox_gu.Add(new MatrixDrawer(new MatrixLibrary.Matrix(10, 10)));
                    this.LabelList_gu.Add(new System.Windows.Forms.Label());

                    if (locationY_gu == 0 && UniversalMatrixBox_gx.Any())
                        locationY_gu = UniversalMatrixBox_gx[0].Height + 15;

                    UniversalMatrixBox_gu[j].Visible = false;
                    UniversalMatrixBox_gu[j].Location = new Point(locationX_gu, locationY_gu + 15);

                    this.LabelList_gu[j].Location = new Point(locationX_gu, locationY_gu);
                    this.LabelList_gu[j].Font = new Font(this.Font, FontStyle.Bold);

                    this.Controls.Add(UniversalMatrixBox_gu[j]);
                    this.Controls.Add(LabelList_gu[j]);

                    this.UniversalMatrixBox_gu[j].ReloadMatrix(sys.current_state.get_Gu()[j]);
                    this.LabelList_gu[j].Text = MatrixGuName + j.ToString() + ": ";
                    
                    UniversalMatrixBox_gu[j].Visible = true;
                    locationX_gu += UniversalMatrixBox_gu[j].Size.Width + 20;
                    j++;
                }

            }


            //пустышка аднака - пустой блок для отображения матриц
            //this.UniversalMatrixBox = new TBMatrixDrawer(new MatrixLibrary.Matrix(10, 10));
            //UniversalMatrixBox.Visible = false;
            //UniversalMatrixBox.Location = new Point(209, 56);
            //this.Controls.Add(UniversalMatrixBox);
        }

        public void UpdateInfo(ESystem sys)
        {
            DrawMatrix();
        }

    }
}
