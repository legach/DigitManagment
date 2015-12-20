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
    //класс рисования графика на области
    class plotter : Panel
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
                for (int i = 0; i <= Points.Count-1; i++)
                    tmp[i] = Points[i].Y;
                return tmp.Max();
            }
        }
        public double Ymin
        {
            get
            {
                double[] tmp = new double[Points.Count];
                for (int i = 0; i <= Points.Count-1; i++)
                    tmp[i] = Points[i].Y;
                return tmp.Min();
            }
        }
        public double Xmax
        {
            get
            {
                return Points[Points.Count-1].X;
            }
        }
        public double Xmin
        {
            get
            {
                return Points[0].X;
            }
        }

        //пределы X и Y в видимости
        public double YVisMax
        {
            get
            {
                double[] tmp = new double[VisiblePoints.Length];
                for (int i = 0; i < VisiblePoints.Length; i++)
                    tmp[i] = VisiblePoints[i].Y;
                return tmp.Max();
            }
        }
        public double YVisMin
        {
            get
            {
                double[] tmp = new double[VisiblePoints.Length];
                for (int i = 0; i < VisiblePoints.Length; i++)
                    tmp[i] = VisiblePoints[i].Y;
                return tmp.Min();
            }
        }
        public double XVisMax
        {
            get
            {
                return VisiblePoints[VisiblePoints.Length - 1].X;
            }
        }
        public double XVisMin
        {
            get
            {
                return VisiblePoints[0].X;
            }
        }

#endregion
#region графика
        //число точек на экране
        uint PCountOnPlotter;
        //первая точка на экране
        uint FirstPointOnPlotter;
        //точки на экране
        PointF[] VisiblePoints
        {
            get
            {
                PointF[] tmp = new PointF[PCountOnPlotter];
                for (int i = 0; i < PCountOnPlotter; i++)
                {
                    tmp[i] = this.Points[(int)FirstPointOnPlotter + i];
                }
                return tmp;
            }
        }
        //массив точек на форме
        Point[] VisibleFPoints
        {
            get
            {
                Point[] tmp = new Point[VisiblePoints.Length];
                for (int i = 0; i < VisiblePoints.Length; i++)
                    tmp[i] = VisibleFPoint(i);
                return tmp;
            }
        }
        //точка на форме с индексом i
        Point VisibleFPoint(int i)
        {
            int x = Convert.ToInt32(this.Width * (this.VisiblePoints[i].X - XVisMin) / (XVisMax - XVisMin));
            int y;
            if (Ymax == Ymin)
                y = Convert.ToInt32(this.Height / 2);
            else
                y = Convert.ToInt32(this.Height * (1 - (this.VisiblePoints[i].Y - Ymin) / (Ymax - Ymin)));

            return new Point(x, y);
        }
        //Ручка, которой рисуем график
        Pen PPen;
        //координаты около курсора
        System.Windows.Forms.Label CoordL;
#endregion
        //конструктор
        public plotter(Point newLocation, Size newSize, Color C, List<Matrix> Arr, int LineInArrNum) :base()
        {
            Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            Name = "123";
            BackColor = Color.DodgerBlue;
            Location = newLocation;
            Size = newSize;
            LoadPointsFromSys(Arr, LineInArrNum);
            SetVisibleRangeAll();
            //графика
            PPen = new Pen(C, 2);
            //элементы формы
            CoordL = new System.Windows.Forms.Label();
            CoordL.Visible = false;
            CoordL.BackColor = Color.LightGoldenrodYellow;
            CoordL.AutoSize = true;
            CoordL.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Controls.Add(CoordL);
            //события
            this.MouseDown += SetCoordOnTip;
            this.MouseUp += ClearTip;
        }
        //конструктор для загрузки из списка double
        public plotter(Point newLocation, Size newSize, Color C, List<double> Arr) : base()
        {
            Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            Name = "123";
            BackColor = Color.DodgerBlue;
            Location = newLocation;
            Size = newSize;
            LoadPointsFromDouble(Arr);
            SetVisibleRangeAll();
            //графика
            PPen = new Pen(C, 2);
            //элементы формы
            CoordL = new System.Windows.Forms.Label();
            CoordL.Visible = false;
            CoordL.BackColor = Color.LightGoldenrodYellow;
            CoordL.AutoSize = true;
            CoordL.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Controls.Add(CoordL);
            //события
            this.MouseDown += SetCoordOnTip;
            this.MouseUp += ClearTip;
        }
        //выдача координат ближайшей к мышке точке
        void SetCoordOnTip(object sender, EventArgs e)
        {
            for (int i = 1; i < this.VisibleFPoints.Length; i++)
                if ((VisibleFPoints[i - 1].X - ((MouseEventArgs)e).X > 0 && VisibleFPoints[i].X - ((MouseEventArgs)e).X < 0) || (VisibleFPoints[i - 1].X - ((MouseEventArgs)e).X < 0 && VisibleFPoints[i].X - ((MouseEventArgs)e).X > 0))
                    if (_3rdparty.mod(VisibleFPoints[i - 1].X - ((MouseEventArgs)e).X) < _3rdparty.mod(VisibleFPoints[i].X - ((MouseEventArgs)e).X))
                        this.CoordL.Text = "x:" + Points[i - 1 + (int)FirstPointOnPlotter].X.ToString() + " y:" + Points[i - 1 + (int)FirstPointOnPlotter].Y.ToString();
                    else
                        this.CoordL.Text = "x:" + Points[i + (int)FirstPointOnPlotter].X.ToString() + " y:" + Points[i + (int)FirstPointOnPlotter].Y.ToString();
            CoordL.Location = ((MouseEventArgs)e).Location - CoordL.Size;// new Point(((MouseEventArgs)e).X - CoordL.Width, ((MouseEventArgs)e).Y - CoordL.Height);
            CoordL.Visible = true;
        }
        void ClearTip(object sender, EventArgs e)
        {
            CoordL.Visible = false;
        }
        public void addP(PointF newP)
        {
            Points.Add(newP);
        }
        //задать рамки во все точки
        public void SetVisibleRangeAll()
        {
            this.PCountOnPlotter = (uint)Points.Count;
            this.FirstPointOnPlotter = 0;
        }
        //изменяет рамки отображения точек на графике
        public bool SetVisibleRange(uint Start, uint Count)
        {
            bool flag;
            if (Start >= this.Points.Count)
            {
                this.PCountOnPlotter = 0;
                flag = false;
            }
            else
                if (Start + Count >= this.Points.Count)
                {
                    this.PCountOnPlotter = (uint)this.Points.Count - Start;
                    this.FirstPointOnPlotter = Start;
                    flag = false;
                }
                else
                {
                    this.PCountOnPlotter = Count;
                    this.FirstPointOnPlotter = Start;
                    flag = true;
                }
            this.redraw();
            return flag;
        }
        public void redraw()
        {
            //перерисовка только если есть точки
            if (this.Points.Count >= 2)
            {
                (CreateGraphics()).Clear(this.BackColor);
                //(CreateGraphics()).DrawBizies(PPen, VisibleFPoints);
                (CreateGraphics()).DrawLines(PPen, VisibleFPoints);

            }
        }

        

        //Заливка какого-либо массива отсчетов из sys сюда
        public void LoadPointsFromSys(List<Matrix> A, int GraphNum)
        {
            this.Points = new List<PointF>();
            if (A !=null)
            {
                PointF tmp;
                for (int j = A.Count-1; j >=0; j--)
                {
                    tmp = new PointF(A.Count - 1 - j, (float)A[j][GraphNum, 0]);
                    this.Points.Add(tmp);
                }
            }
        }
        //загрузка точек из листа double
        public void LoadPointsFromDouble(List<double> A)
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
        }
    }
    //класс вертикального стека плоттеров
    class StackedPlotters : Panel
    {
        //сами плоттеры
        plotter[] Plot;
        //кисточка для рисования сеток
        Pen GridPen;
        #region всякие оффсеты
        //процент ширины ...
        double WidthOffsetPercent;
        //процент высоты "окна" плоттера, отводящийся на "ничто"
        double HeightOffsetPercent;
        //оффсеты в реальных пискелях
        int HeightOffset
        {
            get { return Convert.ToInt32(HeightOffsetPercent * Convert.ToDouble(Height) / Convert.ToDouble(Plot.Length)); }
        }
        int WidthOffset
        {
            get { return Convert.ToInt32(WidthOffsetPercent * Convert.ToDouble(Width)); }
        }
        #endregion
        //Всякие надписи
        System.Windows.Forms.Label Y;
        System.Windows.Forms.Label[] X;
        public StackedPlotters(Point newLocation, Size newPSize, List<Matrix> Arr, Color C) : base()
        {
            Location = newLocation;
            Size = newPSize;
            BackColor = Color.LightSteelBlue;
            //оффсеты
            HeightOffsetPercent = 0.1;
            WidthOffsetPercent = 0.1;
            //Ручка
            GridPen = new Pen(Color.Black, 1);
            //плоттеры
            Plot = new plotter[Arr[0].NoRows];
            for (int i = 0; i < Plot.Length; i++)
            {
                Plot[i] = new plotter(new Point(WidthOffset/2, i * Size.Height / Plot.Length + HeightOffset/2), new Size(Width - WidthOffset, Size.Height / Plot.Length  - HeightOffset), C, Arr, i);
                this.Controls.Add(Plot[i]);
                this.BringToFront();
            }
            //Метки
            Y = new System.Windows.Forms.Label();
            Y.Text = "Y";
            Y.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            Y.Location = new Point(Convert.ToInt32(0.75 * WidthOffset), 0);
            Y.AutoSize = true;
            this.Controls.Add(Y);
            Y.SendToBack();
            X = new System.Windows.Forms.Label[this.Plot.Length];
            for (int i = 0; i < X.Length; i++)
            {
                X[i] = new System.Windows.Forms.Label();
                X[i].Text = "X";
                X[i].Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                X[i].Location = new Point(Width - WidthOffset / 2 - X[i].Width, Plot[i].Location.Y + Plot[i].Height + Convert.ToInt32(GridPen.Width));
                X[i].AutoSize = true;
                this.Controls.Add(X[i]);
                X[i].SendToBack();
            }

            
            //events
            this.SizeChanged += OnSizeChanged;
            
        }
        public void redraw()
        {
            //чистка фона
            (CreateGraphics()).Clear(this.BackColor);
            //рисовка оси Y
            (this.CreateGraphics()).DrawLine(GridPen, new Point(WidthOffset/2 - Convert.ToInt32(GridPen.Width), Height), new Point(WidthOffset/2 - Convert.ToInt32(GridPen.Width), 0));
            (this.CreateGraphics()).DrawLine(GridPen, new Point(Convert.ToInt32(0.75 * WidthOffset) - Convert.ToInt32(GridPen.Width), HeightOffset / 2), new Point(WidthOffset / 2 - Convert.ToInt32(GridPen.Width), 0));
            (this.CreateGraphics()).DrawLine(GridPen, new Point(Convert.ToInt32(0.25 * WidthOffset) - Convert.ToInt32(GridPen.Width), HeightOffset / 2), new Point(WidthOffset / 2 - Convert.ToInt32(GridPen.Width), 0));
            //Метка Y
            Y.Text = "Y; X = " + Plot[0].Xmin.ToString();
            Y.Location = new Point(Convert.ToInt32(0.75 * WidthOffset), 0);
            //проход по плоттерам
            for (int i = 0; i < this.Plot.Length; i++)
            {
                //установка всех точек видимыми
                Plot[i].SetVisibleRangeAll();
                //перерисовка графиков
                Plot[i].redraw();
                //Метки X
                X[i].Text = "X; Y = " + Plot[0].Ymin.ToString();
                X[i].Location = new Point(Convert.ToInt32(Width - Convert.ToDouble(WidthOffset) / 2.0 - X[i].Width), Plot[i].Location.Y + Plot[i].Height + Convert.ToInt32(GridPen.Width + 1));
                //Оси Х
                (this.CreateGraphics()).DrawLine(GridPen, new Point(0, Plot[i].Location.Y + Plot[i].Height + Convert.ToInt32(GridPen.Width)), new Point(Width, Plot[i].Location.Y + Plot[i].Height + Convert.ToInt32(GridPen.Width)));
                (this.CreateGraphics()).DrawLine(GridPen, new Point(Width - WidthOffset / 2, Plot[i].Location.Y + Plot[i].Height + Convert.ToInt32(GridPen.Width) - HeightOffset / 4), new Point(Width, Plot[i].Location.Y + Plot[i].Height + Convert.ToInt32(GridPen.Width)));
                (this.CreateGraphics()).DrawLine(GridPen, new Point(Width - WidthOffset / 2, Plot[i].Location.Y + Plot[i].Height + Convert.ToInt32(GridPen.Width) + HeightOffset / 4), new Point(Width, Plot[i].Location.Y + Plot[i].Height + Convert.ToInt32(GridPen.Width)));
            }
        }
        public void OnSizeChanged(object sender, EventArgs e)
        {
            //Метка Y
            Y.Location = new Point(Convert.ToInt32(0.75 * WidthOffset), 0);
            //Проход по плоттерам
            for (int i = 0; i < Plot.Length; i++)
            {
                //Метки Х
                X[i].Location = new Point(Convert.ToInt32(Width - Convert.ToDouble(WidthOffset) / 2.0 - X[i].Width), Plot[i].Location.Y + Plot[i].Height + Convert.ToInt32(GridPen.Width + 1));
                //Плоттеры
                Plot[i].Size = new Size(Width - WidthOffset, Size.Height / Plot.Length - HeightOffset);
                Plot[i].Location = new Point(Convert.ToInt32(Convert.ToDouble(WidthOffset) / 2.0), Convert.ToInt32(i * Convert.ToDouble(Size.Height) / Convert.ToDouble(Plot.Length) + Convert.ToDouble(HeightOffset) / 2.0));
            }
        }
        public void LoadPlottersFromSys(List<Matrix> A)
        {
            for (int i = 0; i < this.Plot.Length; i++)
            {
                Plot[i].LoadPointsFromSys(A, i);
            }
        }
        
            
    }
    
    /// <summary>
    /// класс функционала графической формы интерфейса
    /// </summary>
    class GUI_Container
    {
        /// <summary>
        /// открытый на чтение (или не совсем) файл .json
        /// </summary>
        public FileStream JsonFile;
        
        /// <summary>
        /// 3 поверхности для рисования p, p и m графиков соответственно
        /// </summary>
        public StackedPlotters[] Panes;
        
        /// <summary>
        /// график критерия качества
        /// </summary>
        public plotter quality;
        
        /// <summary>
        /// метки для плоттеров
        /// </summary>
        public System.Windows.Forms.Label[] PanesLabels;
        
        /// <summary>
        /// открывашка файлов json и инициализирует объект ESystem им.
        /// </summary>
        /// <param name="isSpline"></param>
        /// <param name="isNoise"></param>
        /// <returns></returns>
        public ESystem OpenJsonFile(bool isSpline, bool isNoise)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                JsonFile = (FileStream)OFD.OpenFile();
                JasonsYandere OpenJson = new JasonsYandere(JsonFile);
                State newState = OpenJson.get_yandere_careful();
                newState.isRandomEffect = isNoise;
                newState.isSplainAppr = isSpline;
                return new ESystem(newState);
            }
            else return null;
        }

        /// <summary>
        /// инициализатор плоттеров для рисования и накидываетль их на форму
        /// </summary>
        /// <param name="PlottersCount"></param>
        /// <param name="LeftTopCorner"></param>
        /// <param name="sys"></param>
        public void InitPlotters(int PlottersCount, Point LeftTopCorner, ESystem sys)
        {
            //метки панелей
            PanesLabels = new System.Windows.Forms.Label[PlottersCount + 1];
            for (int k = 0; k < PlottersCount + 1; k++)
            {
                PanesLabels[k] = new System.Windows.Forms.Label();
                PanesLabels[k].Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            }
            PanesLabels[0].Text = "X";
            PanesLabels[1].Text = "U";
            PanesLabels[2].Text = "Y";
            PanesLabels[3].Text = "F (Quality)";
            //Создание панелей
            Panes = new StackedPlotters[PlottersCount];
            int i = 0;
            Panes[0] = new StackedPlotters(new Point(LeftTopCorner.X, LeftTopCorner.Y - PanesLabels[i].Height), new Size(100,100),sys.current_state.x,Color.Green);
            i = 1;
            Panes[1] = new StackedPlotters(new Point(LeftTopCorner.X + (100 + 10) * i, LeftTopCorner.Y - PanesLabels[i].Height), new Size(100, 100), sys.current_state.u, Color.Blue);
            i = 2;
            Panes[2] = new StackedPlotters(new Point(LeftTopCorner.X + (100 + 10) * i, LeftTopCorner.Y - PanesLabels[i].Height), new Size(100, 100), sys.current_state.y, Color.Red);
            // И панели качества
            quality = new plotter(new Point(LeftTopCorner.X, LeftTopCorner.Y + 110), new Size(100, 100), Color.Red, sys.current_state.quality_f);
        }

        /// <summary>
        ///масштабирование панелей под размер формы 
        /// </summary>
        /// <param name="FormSize"></param>
        public void ResizePaneAsForm(Size FormSize)
        {
            for (int i = 0; i < Panes.Length; i++)
            {
                Panes[i].Size = new Size(Convert.ToInt32(FormSize.Width / Panes.Length) - 15, Convert.ToInt32(0.85*FormSize.Height )- Panes[i].Location.Y - 35);
                Panes[i].Location = new Point(Panes[0].Location.X + i * (Panes[i].Width + 10), Panes[i].Location.Y);
                PanesLabels[i].Location = new Point(Panes[i].Location.X + Panes[i].Width / 2, Panes[i].Location.Y - PanesLabels[i].Height / 2);
            }
            quality.Size = new Size(Convert.ToInt32(FormSize.Width) - 25, Convert.ToInt32(0.15*FormSize.Height ) - 15);
            quality.Location = new Point(Panes[0].Location.X, Panes[0].Location.Y + Panes[0].Height + 15);
            PanesLabels[3].Location = new Point(quality.Location.X + quality.Width/2 - PanesLabels[3].Width/2, quality.Location.Y - PanesLabels[3].Height/2);
        }

        /// <summary>
        /// перерисовка массива панелей
        /// </summary>
        public void redraw()
        {
            for (int i = 0; i < this.Panes.Length;i++ )
                Panes[i].redraw();
            quality.redraw();
        }
        
        /// <summary>
        /// перегружает панели из системы
        /// </summary>
        /// <param name="sys"></param>
        public void LoadPanes(ESystem sys)
        {
            Panes[0].LoadPlottersFromSys(sys.current_state.x);
            Panes[1].LoadPlottersFromSys(sys.current_state.u);
            Panes[2].LoadPlottersFromSys(sys.current_state.y);
            quality.LoadPointsFromDouble(sys.current_state.quality_f);
            quality.SetVisibleRangeAll();
        }
    };

    
    /// <summary>
    /// класс рисовки матриц в коробочках
    /// </summary>
    class MatrixDrawer : GroupBox
    {
        System.Windows.Forms.Label[,] Vals;
        int RoundCount;

        public MatrixDrawer(Matrix M)
        {
            RoundCount = 2;
            Vals = new System.Windows.Forms.Label[M.NoRows, M.NoCols];
            for (int i = 0; i < M.NoRows; i++)
                for (int j = 0; j < M.NoCols; j++)
                {
                    Vals[i, j] = new System.Windows.Forms.Label();
                    Vals[i, j].Text = Math.Round(M[i, j], RoundCount).ToString("N3");
                    //Vals[i, j].Text = M[i, j].ToString();
                    Vals[i, j].AutoSize = true;
                    //Vals[i, j].Font = new System.Drawing.Font("SansMono", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                    if (i != 0 || j != 0)
                        Vals[i, j].Location = new Point(j * Vals[0, 0].Width + 5, i * Vals[0, 0].Height + 15);
                    else
                        Vals[i, j].Location = new Point(5, 15);
                    this.Controls.Add(Vals[i,j]);
                }
            this.Size = new Size(1, 1);
            this.AutoSize = true;
        }

        public void ReloadMatrix(Matrix M)
        {
            //чистка
            this.Controls.Clear();
            //загрузка заново
            Vals = new System.Windows.Forms.Label[M.NoRows, M.NoCols];
            for (int i = 0; i < M.NoRows; i++)
                for (int j = 0; j < M.NoCols; j++)
                {
                    Vals[i, j] = new System.Windows.Forms.Label();
                    Vals[i, j].Text = Math.Round(M[i, j], RoundCount).ToString("N3");
                    //Vals[i, j].Text = M[i, j].ToString();
                    Vals[i, j].AutoSize = true;
                    //Vals[i, j].Font = new System.Drawing.Font("SansMono", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                    if (i != 0 || j != 0)
                        Vals[i, j].Location = new Point(j * Vals[0, 0].Width + 5, i * Vals[0, 0].Height + 15);
                    else
                        Vals[i, j].Location = new Point(5, 15);
                    this.Controls.Add(Vals[i,j]);
                  }
            this.Size = new Size(1, 1);
        }
    }

    class TBMatrixDrawer : GroupBox
    {
         TextBox [,] Vals;
        int RoundCount;
        public TBMatrixDrawer(int rows, int cols)
        {
            Vals = new TextBox[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                {
                    Vals[i, j] = new TextBox();
                    Vals[i, j].Text = "0";
                    Vals[i, j].AutoSize = true;
                    //Vals[i, j].Font = new System.Drawing.Font("SansMono", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                    if (i != 0 || j != 0)
                        Vals[i, j].Location = new Point(j * Vals[0, 0].Width + 5, i * Vals[0, 0].Height + 15);
                    else
                        Vals[i, j].Location = new Point(5, 15);
                    this.Controls.Add(Vals[i, j]);
                }
            this.Size = new Size(1, 1);
            this.AutoSize = true;
        }
        public TBMatrixDrawer(Matrix M)
        {
            RoundCount = 2;
            Vals = new TextBox[M.NoRows, M.NoCols];
            for (int i = 0; i < M.NoRows; i++)
                for (int j = 0; j < M.NoCols; j++)
                {
                    Vals[i, j] = new TextBox ();
                    Vals[i, j].Text = Math.Round(M[i, j], RoundCount).ToString("N3");
                    //Vals[i, j].Text = M[i, j].ToString();
                    Vals[i, j].AutoSize = true;
                    //Vals[i, j].Font = new System.Drawing.Font("SansMono", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                    if (i != 0 || j != 0)
                        Vals[i, j].Location = new Point(j * Vals[0, 0].Width + 5, i * Vals[0, 0].Height + 15);
                    else
                        Vals[i, j].Location = new Point(5, 15);
                    this.Controls.Add(Vals[i, j]);
                }
            this.Size = new Size(1, 1);
            this.AutoSize = true;
        }
        public void ReloadMatrix(Matrix M)
        {
            //чистка
            this.Controls.Clear();
            //загрузка заново
            Vals = new TextBox [M.NoRows, M.NoCols];
            for (int i = 0; i < M.NoRows; i++)
                for (int j = 0; j < M.NoCols; j++)
                {
                    Vals[i, j] = new TextBox();
                    Vals[i, j].Text = Math.Round(M[i, j], RoundCount).ToString("N3");
                    //Vals[i, j].Text = M[i, j].ToString();
                    Vals[i, j].AutoSize = true;
                    //Vals[i, j].Font = new System.Drawing.Font("SansMono", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                    if (i != 0 || j != 0)
                        Vals[i, j].Location = new Point(j * Vals[0, 0].Width + 5, i * Vals[0, 0].Height + 15);
                    else
                        Vals[i, j].Location = new Point(5, 15);
                    this.Controls.Add(Vals[i, j]);
                }
            this.Size = new Size(1, 1);
        }
        public string getVal(int i, int j)
        {
            return Vals[i, j].Text;
        }
    }
}
