using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Drawing;
using MatrixLibrary;
using System.Windows.Forms;
using ZedGraph;

namespace neco
{
    public class NewGraphics
    {

        #region математика
        //текущий массив точек
        List<PointF> Points;
        //пределы Y
        public double Ymax
        {
            get
            {
                double[] tmp = new double[Points.Count];
                for (int i = 0; i <= Points.Count - 1; i++)
                    tmp[i] = Points[i].Y;
                return tmp.Max();
            }
        }
        public double Ymin
        {
            get
            {
                double[] tmp = new double[Points.Count];
                for (int i = 0; i <= Points.Count - 1; i++)
                    tmp[i] = Points[i].Y;
                return tmp.Min();
            }
        }
        public double Xmax
        {
            get
            {
                return Points[Points.Count - 1].X;
            }
        }
        public double Xmin
        {
            get
            {
                return Points[0].X;
            }
        }

        #endregion


        //Заливка какого-либо массива отсчетов из sys сюда
        public List<PointF> LoadPointsFromSys(List<Matrix> A, int GraphNum)
        {
            this.Points = new List<PointF>();
            if (A != null)
            {
                PointF tmp;
                for (int j = A.Count - 1; j >= 0; j--)
                {
                    tmp = new PointF(A.Count - 1 - j, (float)A[j][GraphNum, 0]);
                    this.Points.Add(tmp);
                }
            }
            return this.Points;
        }
        //загрузка точек из листа double
        public List<PointF> LoadPointsFromDouble(List<double> A)
        {
            this.Points = new List<PointF>();
            if (A != null)
            {
                PointF tmp;
                for (int j = A.Count - 1; j >= 0; j--)
                {
                    tmp = new PointF(A.Count - 1 - j, (float)A[j]);
                    this.Points.Add(tmp);
                }
            }
            return this.Points;
        }

        /// <summary>
        /// Построение графиков zgraph
        /// </summary>
        /// <param name="zgc">Объект на форме</param>
        /// <param name="func">Массив значений</param>
        /// <param name="name">Имя</param>
        public void CreateGraph(ZedGraphControl zgc, string name, double[] func)
        {
            // get a reference to the GraphPane    
            GraphPane myPane = zgc.GraphPane;

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            myPane.CurveList.Clear();

            // Set the Titles    
            myPane.Title.Text = name;
            myPane.XAxis.Title.Text = "X";
            myPane.YAxis.Title.Text = "Y";

            // Make up some data arrays based on the Sine function    
            double x, y1;
            PointPairList list1 = new PointPairList();
            for (int i = 0; i < func.Length; i++)
            {
                x = (double)i;
                y1 = func[i];
                list1.Add(x, y1);

            }

            // Generate a red curve with diamond    // symbols, and "Porsche" in the legend    
            LineItem myCurve1 = myPane.AddCurve("Line",
                 list1, Color.Red, SymbolType.None);

            int xMax = func.Length,
                xMin = 0;
            myPane.XAxis.Scale.Max = xMax + 20;
            myPane.XAxis.Scale.Min = xMin - 20;
            // Вызываем метод AxisChange (), чтобы обновить данные об осях.
            // В противном случае на рисунке будет показана только часть графика,
            // которая умещается в интервалы по осям, установленные по умолчанию
            zgc.AxisChange();

            // Обновляем график
            zgc.Invalidate();
        }

        /// <summary>
        /// Построение графиков zgraph
        /// </summary>
        /// <param name="zgc">Объект на форме</param>
        /// <param name="listFunc">Список значений PointF</param>
        /// <param name="name">Имя</param>
        public void CreateGraph(ZedGraphControl zgc, string name, List<PointF> listFunc)
        {
            // get a reference to the GraphPane    
            GraphPane myPane = zgc.GraphPane;

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            myPane.CurveList.Clear();

            // Set the Titles    
            myPane.Title.Text = name;
            myPane.XAxis.Title.Text = "X";
            myPane.YAxis.Title.Text = "Y";

            // Make up some data arrays based on the Sine function    
            double pointX, pointY;
            PointPairList listPoint = new PointPairList();
            foreach(var pair in listFunc)
            {
                pointX = pair.X;
                pointY = pair.Y;
                listPoint.Add(pointX, pointY);

            }

            // Generate a red curve with diamond    // symbols, and "Porsche" in the legend    
            //LineItem myCurve1 = myPane.AddCurve("Line",
            //     list1, Color.Red, SymbolType.None);

            LineItem myCurve1 = myPane.AddCurve("Line",
                 listPoint, Color.Red, SymbolType.Diamond);

            int xMax = (int)listFunc.Max(l=>l.X),
                xMin = 0;
            myPane.XAxis.Scale.Max = xMax + 20;
            myPane.XAxis.Scale.Min = xMin - 20;
            // Вызываем метод AxisChange (), чтобы обновить данные об осях.
            // В противном случае на рисунке будет показана только часть графика,
            // которая умещается в интервалы по осям, установленные по умолчанию
            zgc.AxisChange();

            // Обновляем график
            zgc.Invalidate();
        }
    }
}
