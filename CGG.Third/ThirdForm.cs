using System;
using System.Drawing;
using System.Windows.Forms;
using CGG.Core;

namespace CGG.Third
{
    public partial class ThirdForm : Form
    {
        private readonly PlotInfo plotInfo;

        public ThirdForm()
        {
            InitializeComponent();
            plotInfo = new PlotInfo(canvas.Size);
            Redraw();
        }

        #region Drawing

        private void Redraw()
        {
            var bmp = new Bitmap(canvas.Width, canvas.Height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.FillRectangle(Settings.BackgroundBrush,
                    0, 0,
                    canvas.Width - 1, canvas.Height - 1);
                DrawHull(g);
                DrawPoints(g);
            }
            canvas.Image = bmp;
        }

        private void DrawPoints(Graphics graphics)
        {
            foreach (var point in plotInfo.Points)
                graphics.FillEllipse(Settings.PointBrush,
                    new RectangleF(point.Substract(new PointF(Settings.PointRadius, Settings.PointRadius).Divide(2f)),
                        new SizeF(Settings.PointRadius, Settings.PointRadius)));
        }

        private void DrawHull(Graphics graphics)
        {
            if (plotInfo.Hull == null)
                return;
            for (int i = 0; i < plotInfo.Hull.Count; i++)
            {
                var fromIndex = i == 0 ? plotInfo.Hull[plotInfo.Hull.Count - 1] : plotInfo.Hull[i - 1];
                var from = plotInfo.Points[fromIndex];
                var to = plotInfo.Points[plotInfo.Hull[i]];
                graphics.DrawLine(Settings.HullPen, from, to);
            }
        }

        #endregion


        #region Event Handlers

        private void FirstForm_ResizeEnd(object sender, EventArgs e)
        {
            canvas.Size = new Size(Size.Width - 203, Size.Height - 63);
            panel1.Location = new Point(canvas.Location.X + canvas.Size.Width + 6, 12);
            plotInfo.Resize(canvas.Size);
            Redraw();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            ExecuteFor<Button>(sender, button =>
            {
                plotInfo.Clear();
                Redraw();
            });
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            ExecuteFor<Button>(sender, button =>
            {
                if (!plotInfo.CanBuildHull)
                {
                    MessageBox.Show(
                        "Cannot build hull now!\nNumber of points should be grater than 9 and a multiple of 3",
                        "Cannot build hull");
                    return;
                }
                plotInfo.BuildHull();
                if (plotInfo.Hull == null)
                    MessageBox.Show("No hull was found!", "Fail");
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

        private void canvas_MouseClick(object sender, MouseEventArgs e)
        {
            ExecuteFor<PictureBox>(sender, pictureBox =>
            {
                plotInfo.AddPoint(e.Location);
                Redraw();
            });
        }

        private void canvas_MouseLeave(object sender, EventArgs e)
        {
            label1.Text = string.Empty;
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            label1.Text = canvas.PointToClient(Cursor.Position).ToString();
        }

        #endregion

        #region Drawing Ulitity

        #endregion


    }
}
