using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using CGG.Core;

namespace CGG.Sixth
{
    public partial class SixthForm : Form
    {
        private readonly PlotInfo plotInfo = new PlotInfo(
            new CombinedTriangleProvider(
                new CubeTriangleProvider(new Point3D(0, 0, 0), 1f),
                new CubeTriangleProvider(new Point3D(4, 4, 0), 2f),
                new CubeTriangleProvider(new Point3D(-8, 8, 0), 3f),
                new FunctionTriangleProvider(-10, 10, -10, 10, 0.5f, (x, y) => (float)(10 + Math.Sin(Math.Sqrt(x * x + y * y))))
                ),
            new Point3D(15, 15, 15),
            1f,
            new Vector3D(-1, -1, -1),
            new Vector3D(1, -1, 0));

        private readonly Timer timer;

        public SixthForm()
        {
            InitializeComponent();
            Redraw();
            timer = new Timer { Interval = Settings.TickInterval, Enabled = true };
            timer.Tick += TimerTick;

            var old = Settings.TurnAngle;
            Settings.TurnAngle = (float) (Math.PI / 4);
            plotInfo.Turn(TurnDirection.Left);
            Settings.TurnAngle = old;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Left))
                plotInfo.Turn(TurnDirection.Left);
            if (Keyboard.IsKeyDown(Key.Right))
                plotInfo.Turn(TurnDirection.Right);
            if (Keyboard.IsKeyDown(Key.Up))
                plotInfo.Turn(TurnDirection.Up);
            if (Keyboard.IsKeyDown(Key.Down))
                plotInfo.Turn(TurnDirection.Down);
            if (Keyboard.IsKeyDown(Key.W))
                Settings.ZoomCoefficient += Settings.ZoomStep;
            if (Keyboard.IsKeyDown(Key.S))
                Settings.ZoomCoefficient -= Settings.ZoomStep;
            Redraw();
        }

        void Redraw()
        {
            if (canvas.Width == 0 || canvas.Height == 0)
                return;
            var bmp = new Bitmap(canvas.Width, canvas.Height);
            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.FillRectangle(Settings.BackgroundBrush, 0, 0, canvas.Width, canvas.Height);
                DrawTriangles(graphics);
            }
            canvas.Image = bmp;
        }

        private void DrawTriangles(Graphics graphics)
        {
            var triangles = plotInfo.TriangleProvider
                .Produce()
                .Select(t => new {Triangle = t, Distance = t.DistanceFrom(plotInfo.ViewPoint), MaxDistance = t.MaxDistanceTo(plotInfo.ViewPoint)})
                .Where(o => o.Distance > plotInfo.ViewDistance)
                .ToList();
            triangles.Sort((left, right) => Math.Abs(left.Distance - right.Distance) < float.Epsilon
                ? right.MaxDistance.CompareTo(left.MaxDistance)
                : right.Distance.CompareTo(left.Distance));
            foreach (var triangle in triangles.Select(o => o.Triangle))
            {
                var triangle2D = triangle
                    .ProjectOn(plotInfo.ViewPoint, plotInfo.ViewDistance, plotInfo.Forward, plotInfo.Right, plotInfo.Up);
                var canvasPoints = triangle2D
                    .Select(ToCanvasCoords)
                    .ToArray();
                graphics.FillPolygon(new SolidBrush(AdjustColor(Settings.ShapeColor, triangle, plotInfo.Forward)), canvasPoints);
            }
        }

        private PointF ToCanvasCoords(PointF point)
        {
            return new PointF(
                canvas.Width * 0.5f + point.X * Settings.ZoomCoefficient,
                canvas.Height * 0.5f - point.Y * Settings.ZoomCoefficient);
        }

        private Color AdjustColor(Color color, Triangle triangle, Vector3D forward)
        {
            var coef = 0.4 + 0.6 * (Math.Abs(triangle.Normal.DotProduct(forward)));
            return Color.FromArgb((int)(color.R * coef), (int)(color.G * coef), (int)(color.B * coef));
        }

        private void SixthForm_ResizeEnd(object sender, EventArgs e)
        {
            canvas.Size = new Size(Size.Width - 40, Size.Height - 63);
            Redraw();
        }

        private void ExecuteFor<TSender>(object sender, Action<TSender> action)
            where TSender : class
        {
            var senderTyped = sender as TSender;
            if (senderTyped == null)
                return;
            action(senderTyped);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern short GetKeyState(int keyCode);
    }
}
