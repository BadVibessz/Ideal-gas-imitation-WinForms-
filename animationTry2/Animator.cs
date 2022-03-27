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
        private List<Ball> _balls = new List<Ball>();
        private Rectangle _containerRect;
        private Size _containerSize;
        private bool _isAnimating = false;
        private BufferedGraphics _bg;
        private Random _rand = new Random();
        private Physics _physics;
        private object _obj = new object();
        private Thread _thread;
        private const int Max = 100;


        private Form1 _mainForm; //todo: спросить нормальное ли решение
        private NumericUpDown _numeric;

        public List<Ball> Balls => _balls;

        public Physics Physics => this._physics;

        public Animator(Graphics g, Rectangle rectangle, Form1 form, NumericUpDown numeric)
        {
            Update(g, rectangle);
            this._physics = new Physics(this);
            this._numeric = numeric;
            this._mainForm = form;

            //this._bg.Graphics.Clear(Color.White);
        }

        public void Update(Graphics g, Rectangle rectangle) //todo:
        {
            this._graphics = g;
            this._containerRect = rectangle;
            this._containerSize = new Size(rectangle.Width, rectangle.Height);

            lock (_obj)
            {
                // if (_bg != null)
                //     this._bg.Dispose();
                this._bg = BufferedGraphicsManager.Current.Allocate(this._graphics,
                    new Rectangle(0, 0, _containerSize.Width, _containerSize.Height));
            }

            // if (_bg != null) //todo:
            // {
            //    
            //         //Monitor.Enter(_bg);
            //         this._bg.Dispose();
            //         this._bg = BufferedGraphicsManager.Current.Allocate(this._graphics,
            //             new Rectangle(0, 0, _containerSize.Width, _containerSize.Height));
            //         //Monitor.Exit(_bg);
            //     
            // }
            // else
            // {
            //     this._bg = BufferedGraphicsManager.Current.Allocate(this._graphics,
            //         new Rectangle(0, 0, _containerSize.Width, _containerSize.Height));
            // }


            // lock (_bg)
            // {
            //     this._bg = BufferedGraphicsManager.Current.Allocate(this._graphics,
            //         new Rectangle(0, 0, _containerSize.Width, _containerSize.Height));
            // }

            lock (_balls)
                foreach (var b in _balls)
                    b.Update(rectangle);

            // lock (_bg)
            //     this._bg.Graphics.Clear(Color.White);
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

        public void AddBall(PointF location, Rectangle rectangle)
        {
            if (_balls.Count() <= Max)
            {
                Ball b = new Ball(location, rectangle);
                this._balls.Add(b);
                this._physics.AddBall(b);
                b.Start();
            }
        }

        public event EventHandler BallDied;

        public bool RemoveBall(Ball ball) // todo: decreaseNumeric
        {
            this._balls.Remove(ball);
            //lock (_mainForm)
            //this._mainForm.Invoke(DecreaseNumeric());

            return true;
        }

        private Delegate DecreaseNumeric()
        {
            this._numeric.Value--;
            return null;
        }

        public void Stop()
        {
            this._isAnimating = false;
        }

        public void Start()
        {
            if (_thread == null || !_thread.IsAlive)
            {
                _thread = new Thread(() =>
                    {
                        lock (_bg)
                        {
                            Graphics g = _bg.Graphics;

                            while (true)
                            {
                                g.Clear(Color.White);

                                for (int i = 0; i < _balls.Count; i++)
                                    _balls[i]?.Paint(g);
                                lock (_graphics)
                                    _bg.Render(_graphics); //todo: Exception outta nowhere
                            }
                        }
                    }
                );
                _thread.IsBackground = true;
                _thread.Start();
            }
        }
    }
}