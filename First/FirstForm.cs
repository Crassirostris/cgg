using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace First
{
    public partial class FirstForm : Form
    {
        private readonly Dictionary<string, object> oldValues = new Dictionary<string, object>(); 

        public FirstForm()
        {
            InitializeComponent();
            Redraw();
        }

        #region Drawing

        private void Redraw()
        {
            var bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            var plotInfo = GatherPlotInfo();
            using (var g = Graphics.FromImage(bmp))
            {
                g.FillRectangle(new SolidBrush(Settings.BackgroundColor),
                    0, 0, 
                    pictureBox1.Width - 1, pictureBox1.Height - 1);
                DrawGrid(g, plotInfo);
                DrawAxes(g, plotInfo);
                DrawPlot(g, plotInfo);
            }
            pictureBox1.Image = bmp;
        }

        private void DrawAxes(Graphics graphics, PlotInfo plotInfo)
        {
            graphics.DrawLine(Settings.AxesPen,
                ToBitmapPoint(plotInfo, plotInfo.From, 0),
                ToBitmapPoint(plotInfo, plotInfo.To, 0));
            graphics.DrawLine(Settings.AxesPen,
                ToBitmapPoint(plotInfo, 0, -plotInfo.ValuesSegmentLength / 2),
                ToBitmapPoint(plotInfo, 0, plotInfo.ValuesSegmentLength / 2));
        }

        private void DrawGrid(Graphics graphics, PlotInfo plotInfo)
        {
            var gridTick = DeduceGridTick(plotInfo);
            var signigicantDigits = Math.Max(0, (int)Math.Ceiling(-Math.Log10(gridTick)));
            var marksFormat = string.Format("{{0:F{0}}}", signigicantDigits);
            for (int i = 1; i * gridTick < plotInfo.ValuesSegmentLength; ++ i)
            {
                graphics.DrawLine(Settings.GridPen, 
                    ToBitmapPoint(plotInfo, plotInfo.From, i * gridTick),
                    ToBitmapPoint(plotInfo, plotInfo.To, i * gridTick));
                graphics.DrawString(string.Format(marksFormat, i * gridTick), Settings.GridMarksFont, Settings.GridMarksBrush,
                    ToBitmapPoint(plotInfo, 0, i * gridTick));
                graphics.DrawLine(Settings.GridPen, 
                    ToBitmapPoint(plotInfo, plotInfo.From, -i * gridTick),
                    ToBitmapPoint(plotInfo, plotInfo.To, -i * gridTick));
                graphics.DrawString(string.Format(marksFormat, i * gridTick), Settings.GridMarksFont, Settings.GridMarksBrush,
                    ToBitmapPoint(plotInfo, 0, -i * gridTick));
            }
            for (int i = (int) (Math.Ceiling(plotInfo.From / gridTick + Settings.Epsilon)); i < plotInfo.To / gridTick; i++)
            {
                graphics.DrawLine(Settings.GridPen,
                    ToBitmapPoint(plotInfo, i * gridTick, -plotInfo.ValuesSegmentLength / 2),
                    ToBitmapPoint(plotInfo, i * gridTick, plotInfo.ValuesSegmentLength / 2));
                graphics.DrawString(string.Format(marksFormat, i * gridTick), Settings.GridMarksFont, Settings.GridMarksBrush,
                    ToBitmapPoint(plotInfo, i * gridTick, 0));
            }
            var rightmostPoint = ToBitmapPoint(plotInfo, plotInfo.To, 0);
            graphics.DrawString("x", Settings.AxisLegendFont, Settings.AxisLegendBrush, new PointF(rightmostPoint.X - 20, rightmostPoint.Y - 30));
            var topmostPoint = ToBitmapPoint(plotInfo, 0, plotInfo.ValuesSegmentLength / 2);
            graphics.DrawString("y", Settings.AxisLegendFont, Settings.AxisLegendBrush, new PointF(topmostPoint.X - 20, topmostPoint.Y - 5));
        }
        private void DrawPlot(Graphics graphics, PlotInfo plotInfo)
        {
            var gridTick = DeduceGridTick(plotInfo);
            var firstPoint = true;
            float prevX = 0, prevY = 0;
            for (float i = 0; i < pictureBox1.Width; ++i)
            {
                var x = plotInfo.From + i / GetScale(plotInfo);
                try
                {
                    var y = Settings.Function(x, plotInfo.A, plotInfo.B, plotInfo.C);
                    if (!firstPoint)
                    {
                        if (Math.Abs(prevY) > plotInfo.ValuesSegmentLength / 2 &&
                            Math.Abs(y) > plotInfo.ValuesSegmentLength && Math.Sign(prevY) != Math.Sign(y))
                            firstPoint = true;
                        if (!firstPoint)
                            graphics.DrawLine(Settings.PlotPen,
                                ToBitmapPoint(plotInfo, prevX, prevY),
                                ToBitmapPoint(plotInfo, x, y));
                    }
                    else
                        firstPoint = false;
                    prevX = x;
                    prevY = y;
                }
                catch (Exception)
                {
                    firstPoint = true;
                }
            }
        }

        #endregion


        #region Event Handlers

        private void FirstForm_ResizeEnd(object sender, EventArgs e)
        {
            pictureBox1.Size = new Size(Size.Width - 203, Size.Height - 63);
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

        private PlotInfo GatherPlotInfo()
        {
            return new PlotInfo(@from: float.Parse(textBox1.Text),
                to: float.Parse(textBox2.Text),
                a: float.Parse(textBox3.Text),
                b: float.Parse(textBox4.Text),
                c: float.Parse(textBox5.Text),
                ratio: ((float) pictureBox1.Width) / pictureBox1.Height);
        }

        private PointF ToBitmapPoint(PlotInfo plotInfo, float x, float y)
        {
            return new PointF((x - plotInfo.From) * GetScale(plotInfo),
                -y * GetScale(plotInfo) + pictureBox1.Height / 2.0f);
        }

        private float GetScale(PlotInfo plotInfo)
        {
            return pictureBox1.Width / plotInfo.SegmentLength;
        }

        private float DeduceGridTick(PlotInfo plotInfo)
        {
            var currentTick = 1f;
            while (true)
            {
                var ticksCount = (int)(plotInfo.SegmentLength / currentTick);
                if (ticksCount <= Settings.MaxGrids && ticksCount >= Settings.MinGrids)
                    return currentTick;
                currentTick *= ticksCount < Settings.MinGrids ? 0.1f : 10;
            }
        }

        #endregion


    }
}
