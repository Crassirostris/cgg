using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CGG.Core;

namespace CGG.Second
{
    public partial class SecondForm : Form
    {
        private readonly Dictionary<string, object> oldValues = new Dictionary<string, object>(); 

        public SecondForm()
        {
            InitializeComponent();
            Redraw();
        }

        #region Drawing

        void Redraw()
        {
            var plotInfo = CollectPlotInfo();
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.FillRectangle(Settings.BackgroundBrush, 0, 0, pictureBox1.Width, pictureBox1.Height);
                DrawPlot(plotInfo, graphics);
            }
            pictureBox1.Image = bmp;
        }

        private void DrawPlot(PlotInfo plotInfo, Graphics graphics)
        {
            DrawPixel(graphics, plotInfo.F1.NearestIntegerPoint(), Settings.FocusBrush);
            DrawPixel(graphics, plotInfo.F2.NearestIntegerPoint(), Settings.FocusBrush);
            Bresenham(plotInfo, graphics);
        }

        private void Bresenham(PlotInfo plotInfo, Graphics graphics)
        {
            var initialPoint = plotInfo.InitialPoint.NearestIntegerPoint();
            var currentPoint = initialPoint;
            var previousPoint = plotInfo.InitialPoint.Add(plotInfo.Direction).NearestIntegerPoint();
            do
            {
                DrawPixel(graphics, currentPoint, Settings.PlotBrush);
                var nextPoint = currentPoint
                    .Neighbors()
                    .Where(p => previousPoint.Substract(p).LengthSquared() > 1)
                    .OrderBy(p => BresenhamError(plotInfo, p))
                    .First();
                previousPoint = currentPoint;
                currentPoint = nextPoint;
            } while (currentPoint.Substract(initialPoint).LengthSquared() > 4 || previousPoint.Substract(initialPoint).LengthSquared() <= 4);
            DrawPixel(graphics, currentPoint, Settings.PlotBrush);
        }

        private void DrawPixel(Graphics graphics, Point point, Brush brush)
        {
            var horizontalPixelsCount = pictureBox1.Width / Settings.PixelSize;
            var verticalPixelsCount = pictureBox1.Height / Settings.PixelSize;
            var biasedPoint = new Point(point.X, -point.Y).Add(new Point(horizontalPixelsCount / 2, verticalPixelsCount / 2));
            if (biasedPoint.X < 0 || biasedPoint.X >= horizontalPixelsCount || biasedPoint.Y < 0 || biasedPoint.Y >= verticalPixelsCount)
                return;
            var horizontalOffset = (pictureBox1.Width - horizontalPixelsCount * Settings.PixelSize) / 2;
            var verticalOffset = (pictureBox1.Height - verticalPixelsCount * Settings.PixelSize) / 2;
            graphics.FillRectangle(brush,
                horizontalOffset + biasedPoint.X * Settings.PixelSize,
                verticalOffset + biasedPoint.Y * Settings.PixelSize,
                Settings.PixelSize,
                Settings.PixelSize);
        }

        private float BresenhamError(PlotInfo plotInfo, Point point)
        {
            return Math.Abs(plotInfo.F1.Substract(point).Length() + plotInfo.F2.Substract(point).Length() - 2 * plotInfo.A);
        }

        private PlotInfo CollectPlotInfo()
        {
            return new PlotInfo(
                new PointF(float.Parse(textBox1.Text), float.Parse(textBox2.Text)),
                new PointF(float.Parse(textBox3.Text), float.Parse(textBox4.Text)),
                int.Parse(textBox5.Text));
        }

        #endregion


        #region Event Handlers

        private void FirstForm_ResizeEnd(object sender, EventArgs e)
        {
            pictureBox1.Size = new Size(Size.Width - panel1.Width - 46, Size.Height - 63);
            panel1.Location = new Point(pictureBox1.Location.X + pictureBox1.Size.Width + 6, 12);
            Redraw();
        }

        private void NumberBoxOnGetFocus(object sender, EventArgs e)
        {
            ExecuteFor<TextBox>(sender, textBox =>
            {
                oldValues[textBox.Name] = textBox.Text;
            });
        }

        private void NumberBoxOnLostFocus(object sender, EventArgs e)
        {
            ExecuteFor<TextBox>(sender, textBox =>
            {
                float value;
                if (!float.TryParse(textBox.Text, out value))
                    textBox.Text = (string) oldValues[textBox.Name];
                Redraw();
            });
        }

        private void ExecuteFor<TSender>(object sender, Action<TSender> action)
            where TSender : class
        {
            var senderTyped = sender as TSender;
            if (senderTyped == null)
                return;
            action(senderTyped);
        }

        #endregion


        #region Drawing Ulitity

        #endregion
    }
}
