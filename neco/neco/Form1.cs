using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MatrixLibrary;


namespace neco
{
    partial class CreateF1 : Form
    {
        List<TBMatrixDrawer> MatrixBoxListA;
        List<Label> LabelListA, LabelListTau;
        List<TextBox> TextBoxListTau;
        List<TBMatrixDrawer> MatrixBoxListB;
        List<Label> LabelListB, LabelListTeta;
        List<TextBox> TextBoxListTeta;
        List<TBMatrixDrawer> MatrixBoxListH;
        List<Label> LabelListH;
        public static ESystem sys1;
        int m, n, p, a, c;
        double Tx, Tu;
        List<double> tau, teta;
        List<List<Matrix>> A, B;
        List<Matrix> x,u,H;
        Boolean debug = true;

        public CreateF1()
        {

            InitializeComponent();
        }

        

        private void next_button1_Click_1(object sender, EventArgs e)
        {
            Done_button1.Visible = true;
            splitContainer1.Panel2.Controls.Clear();
            splitContainer1.Panel2.Controls.Add(Done_button1);
            int locationX = 20;
            int locationY = 60;

         try
           {
                n = Convert.ToInt32(textBox_n.Text);
                m = Convert.ToInt32(textBox_m.Text);
                p = Convert.ToInt32(textBox_p.Text);
                a = Convert.ToInt32(textBox_a.Text);
                c = Convert.ToInt32(textBox_c.Text);
                Tx = Convert.ToDouble(textBox_Tx.Text);
                Tu = Convert.ToDouble(textBox_Tu.Text);


                // Вывод матриц А и tau


                MatrixBoxListA = new List<TBMatrixDrawer>();
                LabelListA = new List<Label>();
                LabelListTau = new List<Label>();
                TextBoxListTau = new List<TextBox>();

                for (int i = 0; i <= a; i++)
                {
                    LabelListA.Add(new Label());
                    LabelListA[i].Text = "A"+i+": ";
                    LabelListA[i].Location = new Point(locationX, locationY);
                    splitContainer1.Panel2.Controls.Add(LabelListA[i]);

                    MatrixBoxListA.Add(new TBMatrixDrawer(n, n));
                    MatrixBoxListA[i].Location = new Point(locationX, locationY+20);
                    splitContainer1.Panel2.Controls.Add(MatrixBoxListA[i]);
                    
                    if (i != 0)
                    {
                        LabelListTau.Add(new Label());
                        LabelListTau[i-1].Text = "Tau" + i + ": ";
                        LabelListTau[i-1].Location = new Point(locationX, locationY + MatrixBoxListA[i].Size.Height + 40);
                        LabelListTau[i - 1].Size = new Size(50, 25);
                        splitContainer1.Panel2.Controls.Add(LabelListTau[i-1]);

                        TextBoxListTau.Add(new TextBox());
                        TextBoxListTau[i - 1].Text = "0";
                        TextBoxListTau[i - 1].Location = new Point(locationX + 60, locationY + MatrixBoxListA[i].Size.Height + 37);
                        TextBoxListTau[i - 1].Size = new Size(50, 20);
                        splitContainer1.Panel2.Controls.Add(TextBoxListTau[i - 1]);
                    }
                    locationX += MatrixBoxListA[i].Size.Width + 20;
                }

                locationY += MatrixBoxListA[0].Size.Height + 70;
                locationX = 20;

                 // Вывод матриц B и teta


                MatrixBoxListB = new List<TBMatrixDrawer>();
                LabelListB = new List<Label>();
                LabelListTeta = new List<Label>();
                TextBoxListTeta = new List<TextBox>();

                for (int i = 0; i <= c; i++)
                {
                    LabelListB.Add(new Label());
                    LabelListB[i].Text = "B"+i+": ";
                    LabelListB[i].Location = new Point(locationX, locationY);
                    splitContainer1.Panel2.Controls.Add(LabelListB[i]);

                    MatrixBoxListB.Add(new TBMatrixDrawer(n, m));
                    MatrixBoxListB[i].Location = new Point(locationX, locationY+20);
                    splitContainer1.Panel2.Controls.Add(MatrixBoxListB[i]);
                    
                    if (i != 0)
                    {
                        LabelListTeta.Add(new Label());
                        LabelListTeta[i-1].Text = "Teta" + i + ": ";
                        LabelListTeta[i-1].Location = new Point(locationX, locationY + MatrixBoxListB[i].Size.Height + 40);
                        LabelListTeta[i - 1].Size = new Size(50, 25);
                        splitContainer1.Panel2.Controls.Add(LabelListTeta[i-1]);

                        TextBoxListTeta.Add(new TextBox());
                        TextBoxListTeta[i - 1].Text = "0";
                        TextBoxListTeta[i - 1].Location = new Point(locationX + 60, locationY + MatrixBoxListB[i].Size.Height + 37);
                        TextBoxListTeta[i - 1].Size = new Size(50, 20);
                        splitContainer1.Panel2.Controls.Add(TextBoxListTeta[i - 1]);
                    }
                    locationX += MatrixBoxListB[i].Size.Width + 20;
                }

                locationY = MatrixBoxListB[0].Location.Y + MatrixBoxListB[0].Size.Height + 60;
                locationX = 20;

                // Вывод матрицы H



                MatrixBoxListH = new List<TBMatrixDrawer>();
                LabelListH = new List<Label>();

                LabelListH.Add(new Label());
                LabelListH[0].Text = "H: ";
                LabelListH[0].Location = new Point(locationX, locationY);
                splitContainer1.Panel2.Controls.Add(LabelListH[0]);

                MatrixBoxListH.Add(new TBMatrixDrawer(p, n));
                MatrixBoxListH[0].Location = new Point(locationX, locationY + 20);
                splitContainer1.Panel2.Controls.Add(MatrixBoxListH[0]);


          }
         catch (Exception ex)
          {
           
              MessageBox.Show(this, ex.Message, "Ошибка!",MessageBoxButtons.OK, MessageBoxIcon.Error);
          }

        }

        private void Done_button1_Click(object sender, EventArgs e)
        {
             try
           {
            List<Matrix> tmp;
            Matrix tmpM;
            int J;
            if (a>c)
                J=a+1;
            else
                J=c+1;



            if (debug)
            {
                x = new List<Matrix>();
                for (int i = 0; i < J*2; i++)
                {
                    tmpM = new Matrix(n, 1);
                    for (int j = 0; j < n; j++)
                        tmpM[j, 0] = 1;
                    x.Add(tmpM);
                }

               
                u = new List<Matrix>();
                for (int i = 0; i < J*2; i++)
                {
                    tmpM = new Matrix(m, 1);
                    for (int j = 0; j < m; j++)
                        tmpM[j, 0] = 0;
                    u.Add(tmpM);
                }

            }

            A = new List<List<Matrix>>();
            tmp = new List<Matrix>();
            for (int i = 0; i <= a; i++)
            {
                tmp.Add(new Matrix(n, n));
                for (int j = 0; j < n; j++)
                    for (int k = 0; k < n; k++)
                    {
                        tmp[i][j, k] = Convert.ToDouble(MatrixBoxListA[i].getVal(j, k));
                    }
            }

            A.Add(tmp);

            B = new List<List<Matrix>>();
            tmp = new List<Matrix>();
            for (int i = 0; i <= c; i++)
            {
                tmp.Add(new Matrix(n, m));
                for (int j = 0; j < n; j++)
                    for (int k = 0; k < m; k++)
                    {
                        tmp[i][j, k] = Convert.ToDouble(MatrixBoxListB[i].getVal(j, k));
                    }
            }

            B.Add(tmp);

            H = new List<Matrix>();

                H.Add(new Matrix(p, n));
                for (int j = 0; j < p; j++)
                    for (int k = 0; k < n; k++)
                    {
                        H[0][j, k] = Convert.ToDouble(MatrixBoxListH[0].getVal(j, k));
                    }


            tau = new List<double>();
            tau.Add(0);
            for (int i = 0; i < a; i++)
                tau.Add(Convert.ToDouble(TextBoxListTau[i].Text));


            teta = new List<double>();
            teta.Add(0);
            for (int i = 0; i < c; i++)
                teta.Add(Convert.ToDouble(TextBoxListTeta[i].Text));

            List<Matrix> Fi = new List<Matrix>();
            if (debug)
                Fi.Add(new Matrix(Matrix.Identity(n)));

            List<Matrix> Psi = new List<Matrix>();
            if (debug)
                Psi.Add(new Matrix(Matrix.Identity(m)));

            if (Tu <= 0 && Tx > 0)//если задали только Tx
                Tu = Tx;
            if (Tx <= 0 && Tu > 0)//а если только Tu
                Tx = Tu;
            if (Tu <= 0 || Tx <= 0)//и если оне по-прежнему некорректны...
                Tx = Tu = 1;
            
            Boolean new_determinancy = true;
            Boolean new_perturbation = false;




            State newState = new State(
                x,
                u,
                A,
                B,
                H,
                tau,
                teta,
                Fi,
                Psi,
                Tx,
                Tu,
                FlagStationary.Checked,
                new_determinancy,
                new_perturbation);

            newState.isRandomEffect = false;
            newState.isSplainAppr = false;
            sys1 = new ESystem(newState);

            this.Close();
           }
             catch (Exception ex)
             {

                 MessageBox.Show(this, ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
             }
        }




       
 
    }
}
