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
        /// <param name="func">Массив значений</param>
        /// <param name="name">Имя</param>
        public void CreateGraph(ZedGraphControl zgc, string name, List<List<PointF>> func, int numOfGraph)
        {
            // get a reference to the GraphPane    
            GraphPane myPane = zgc.GraphPane;

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            myPane.CurveList.Clear();

            // Set the Titles    
            myPane.Title.Text = name;
            myPane.XAxis.Title.Text = "X";
            myPane.YAxis.Title.Text = "Y";

            double pointX, pointY;
            PointPairList listPoint1 = new PointPairList();
            PointPairList listPoint2 = new PointPairList();
            PointPairList listPoint3 = new PointPairList();
            PointPairList listPoint4 = new PointPairList();

            if (func.Count > 0)
            {
                for (int j = 0; j < func[0].Count; j++)
                {
                    pointX = (double)func[0][j].X;
                    pointY = (double)func[0][j].Y;
                    listPoint1.Add(pointX, pointY);
                }
                myPane.AddCurve(name + " 01", listPoint1, Color.DarkCyan, SymbolType.Diamond);
            }
            if (func.Count > 1)
            {
                for (int j = 0; j < func[0].Count; j++)
                {
                    pointX = (double)func[1][j].X;
                    pointY = (double)func[1][j].Y;
                    listPoint2.Add(pointX, pointY);
                }
                myPane.AddCurve(name + " 02", listPoint2, Color.DarkBlue, SymbolType.Diamond);
            }
            if (func.Count > 2)
            {
                for (int j = 0; j < func[0].Count; j++)
                {
                    pointX = (double)func[2][j].X;
                    pointY = (double)func[2][j].Y;
                    listPoint3.Add(pointX, pointY);
                }
                myPane.AddCurve(name + " 03", listPoint3, Color.DarkGreen, SymbolType.Diamond);
            }
            if (func.Count > 3)
            {
                for (int j = 0; j < func[0].Count; j++)
                {
                    pointX = (double)func[3][j].X;
                    pointY = (double)func[3][j].Y;
                    listPoint4.Add(pointX, pointY);
                }
                myPane.AddCurve(name + " 04", listPoint4, Color.DarkMagenta, SymbolType.Diamond);
            }
            int xMax = func[0].Count,
                xMin = 0;
            myPane.XAxis.Scale.Max = xMax + 1;
            myPane.XAxis.Scale.Min = xMin - 1;
            zgc.AxisChange();
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
            foreach (var pair in listFunc)
            {
                pointX = pair.X;
                pointY = pair.Y;
                listPoint.Add(pointX, pointY);
            }

            LineItem myCurve1 = myPane.AddCurve(name,
                 listPoint, Color.DarkCyan, SymbolType.Diamond);
            int xMax = 0, xMin = 0;
            try
            {
                xMax = (int)listFunc.Max(l => l.X);
            }
            catch (Exception e)
            {
                xMax = 0;
            }
            myPane.XAxis.Scale.Max = xMax + 10;
            myPane.XAxis.Scale.Min = xMin - 10;
            // Вызываем метод AxisChange (), чтобы обновить данные об осях.
            // В противном случае на рисунке будет показана только часть графика,
            // которая умещается в интервалы по осям, установленные по умолчанию
            zgc.AxisChange();

            // Обновляем график
            zgc.Invalidate();
        }
    }
}
