using System;
using System.Drawing;
using System.Threading;

namespace animationTry2
{
    public class Ball
    {
        public Color Color { get; set; }
        public float Radius { get; set; }
        public float vX { get; set; }
        public float vY { get; set; }
        public PointF Origin;
        public bool IsAlive;
        public Rectangle ContainerRect => _containerRect;
        public Size ContainerSize => _containerSize;


        private Rectangle _containerRect;
        private Size _containerSize;
        private Thread _thread;

        


        private Random _rand = new Random();
        //private Thread _thread = null; //todo: why do we need to store thread here?


        public Ball(PointF origin, Rectangle rectangle)
        {
            this.Origin = origin;
            this._containerRect = rectangle;
            this._containerSize = new Size(rectangle.Width, rectangle.Height);

            this.Color = System.Drawing.Color.FromArgb(_rand.Next(0, 255), _rand.Next(0, 255), _rand.Next(0, 255)); //todo from 0 to 255
            this.Radius = _rand.Next(5, 50);
            this.vX = _rand.Next(-20, 20);
            this.vY = _rand.Next(-20, 20);
            this.IsAlive = true;
        }

        public void Start()
        {
            if (_thread == null || !_thread.IsAlive)
            {
                _thread = new Thread(() =>
                {
                    while (Move())
                        Thread.Sleep(5);
                });
                _thread.IsBackground = true;
                _thread.Start();
            }
            
        }

        public void Paint(Graphics g)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.DrawEllipse(new Pen(Color.Black, 2), Origin.X, Origin.Y, Radius * 2, Radius * 2);
            g.FillEllipse(new SolidBrush(Color), Origin.X, Origin.Y, Radius * 2, Radius * 2);
        }

        public void Update(Rectangle rectangle)
        {
            this._containerRect = rectangle;
            this._containerSize = new Size(rectangle.Width, rectangle.Height);
        }

        public bool Reverse()
        {
            this.vX = -vX;
            this.vY = -vY;
            return true;
        }

        public bool Move()
        {
            if (this.IsAlive)
                return Physics.MoveBall(this);
            return false;
        }

        public Color ChangeBrightness(float factor)
        {
            float R = (this.Color.R + factor > 255) ? 255 : Color.R + factor;
            float G = (this.Color.G + factor > 255) ? 255 : Color.G + factor;
            float B = (this.Color.B + factor > 255) ? 255 : Color.B + factor;

            if (R < 0) R = 0;
            if (G < 0) G = 0;
            if (B < 0) B = 0;

            return System.Drawing.Color.FromArgb((int)R, (int)G, (int)B);
        }
    }
}