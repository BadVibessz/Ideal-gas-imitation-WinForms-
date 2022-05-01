using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace animationTry2
{
    public class Animator
    {
        private Graphics _graphics;
        private List<Ball> _balls = new();
        private Rectangle _containerRect;
        private Size _containerSize;
        private bool _isAnimating = false;

        private BufferedGraphics _bg;

        //private Random _rand = new Random();
        private Physics _physics;

        private object _obj = new object();
        private Thread _thread;

        private const int Max = 100;
        private bool _resized = false;

        public Color colorToClear { get; set; }
        public List<Ball> Balls => _balls;

        public Physics Physics => this._physics;

        public Animator(Graphics g, Rectangle rectangle)
        {
            Update(g, rectangle);
            this._physics = new Physics(this);
        }

        public void Update(Graphics g, Rectangle rectangle) //todo: delete black blinks
        {
            this._resized = true;
            this._graphics = g;
            this._containerRect = rectangle;
            this._containerSize = new Size(rectangle.Width, rectangle.Height);


            lock (_obj)
            {
                //lock (_graphics)
                    this._bg = BufferedGraphicsManager.Current.Allocate(this._graphics,
                        new Rectangle(0, 0, _containerSize.Width, _containerSize.Height));
            }


            lock (_balls)
                foreach (var b in _balls)
                    b.Update(rectangle);
        }

        public void AddBallAndStart(PointF location, Rectangle rectangle)
        {
            if (_thread == null || !_thread.IsAlive)
            {
                _isAnimating = true;
                _thread = new Thread(Animate);
                _thread.IsBackground = true;
                _thread.Start();
            }

            if (_balls.Count() <= Max)
            {
                Ball b = new Ball(location, rectangle);
                this._balls.Add(b);
                //this._physics.AddBall(b);
                b.Start();
            }
        }


        public void Animate()
        {
            while (_isAnimating)
            {
                Monitor.Enter(_obj);
                this._resized = false;
                Graphics g = _bg.Graphics;
                Monitor.Exit(_obj);

                g.Clear(colorToClear);

                //lock (_balls) //todo: why Monitor works faster than lock
                Monitor.Enter(_balls);
                for (int i = 0; i < _balls.Count(); i++)
                    _balls[i].Paint(g);
                Monitor.Exit(_balls);

                lock (_bg)
                    if (!_resized)
                        _bg.Render();

                Thread.Sleep(5);
            }
        }


        public event EventHandler BallDied;

        protected virtual void OnBallDied(EventArgs e)
        {
            EventHandler handler = BallDied;
            handler?.Invoke(this, e);
        }

        public bool RemoveBall(Ball ball) // todo: decreaseNumeric
        {
            this._balls.Remove(ball);
            OnBallDied(EventArgs.Empty);
            //lock (_mainForm)
            //this._mainForm.Invoke(DecreaseNumeric());

            return true;
        }

        public void Accelerate()
        {
            Monitor.Enter(_balls);
            foreach (var b in _balls)
            {
                b.vX = b.vX * 2f;
                b.vY = b.vY * 2f;
            }

            Monitor.Exit(_balls);
        }

        public void Slow()
        {
            Monitor.Enter(_balls);
            foreach (var b in _balls)
            {
                b.vX = b.vX * 0.5f;
                b.vY = b.vY * 0.5f;
            }

            Monitor.Exit(_balls);
        }

        public void Clear()
        {
            this._balls.Clear();
            this._bg.Dispose();
            this._graphics.Clear(colorToClear);
            this._graphics.Dispose();
            this._thread?.Abort();
            this._isAnimating = false;
        }
    }
}