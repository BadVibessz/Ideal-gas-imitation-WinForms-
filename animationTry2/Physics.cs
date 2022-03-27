using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Threading;

namespace animationTry2
{
    public class Physics
    {
        private static Animator _animator;
        private static List<Ball> _balls;
        private static Statistics _statisticsForm;

        public Physics(Animator animator)
        {
            Physics._animator = animator;
            Physics._balls = _animator.Balls;
        }

        public Statistics StatisticsForm
        {
            set { _statisticsForm = value; }
        }

        public void AddBall(Ball ball)
        {
            _balls.Add(ball);
        }

        public static bool MoveBall(Ball ball)
        {
            var p = new PointF(ball.Origin.X + ball.vX, ball.Origin.Y + ball.vY);

            if (Math.Sign(ball.vX) == 1)
                p.X += ball.Radius * 2;
            if (Math.Sign(ball.vY) == 1)
                p.Y += ball.Radius * 2;

            if (ball.ContainerRect.Contains(Point.Ceiling(p)))
            {
                ball.Origin.X += ball.vX;
                ball.Origin.Y += ball.vY;
            }
            else
            {
                if (p.X <= 0)
                    p.X = 1;
                if (p.Y <= 0)
                    p.Y = 1;
                if (p.X >= ball.ContainerSize.Width)
                    p.X = ball.ContainerSize.Width - 1;
                if (p.Y >= ball.ContainerSize.Height)
                    p.Y = ball.ContainerSize.Height - 1;


                if (Math.Sign(ball.vX) == 1)
                    p.X -= ball.Radius * 2;
                if (Math.Sign(ball.vY) == 1)
                    p.Y -= ball.Radius * 2;

                ball.Origin.X = p.X;
                ball.Origin.Y = p.Y;
            }

            if (BounceBallFromEdge(ball, ball.ContainerSize))
            {
                ball.vX *= 0.95f;
                ball.vY *= 0.95f;

                if (Math.Abs(ball.vX) < 3 && Math.Abs(ball.vY) < 3)
                {
                    (ball.vX, ball.vY) = (0, 0);
                    ball.IsAlive = false; //todo: remove ball from screen
                    _balls.Remove(ball);
                    _animator.RemoveBall(ball);
                }
                
                if (_statisticsForm != null) //todo: trouble with threads
                    _statisticsForm.DataGridRefresh();
            }

            // var collisions = Physics.GetCollisions(_balls); //todo
            // if (collisions != null)
            //     BounceBallFromBall(collisions);
            
            ball.vX *= 0.995f;
            ball.vY *= 0.995f;

            //todo: чтобы шарик темнел при замедлении
            ball.Color = ball.ChangeBrightness(-0.8f); //todo: calculate factor


            if (Math.Abs(ball.vX) <= 0.5 && Math.Abs(ball.vY) <= 0.5)
            {
                ball.IsAlive = false;
                _balls.Remove(ball);
                _animator.RemoveBall(ball);
            }
            
            return true;
        }

        private static bool BounceBallFromEdge(Ball ball, Size containerSize)
        {
            if (ball.Origin.X <= 1)
            {
                ball.vX = -ball.vX;
                return true;
            }
            else if (ball.Origin.X + ball.Radius * 2 >= containerSize.Width - 1)
            {
                ball.vX = -ball.vX;
                return true;
            }

            if (ball.Origin.Y <= 1)
            {
                ball.vY = -ball.vY;
                return true;
            }
            else if (ball.Origin.Y + ball.Radius * 2 >= containerSize.Height - 1)
            {
                ball.vY = -ball.vY;
                return true;
            }

            return false;
        }

        private static bool HasCollision(Ball b1, Ball b2) //todo: wrong
        {
            var o1 = new PointF(b1.Origin.X + b1.Radius, b1.Origin.Y + b1.Radius);
            var o2 = new PointF(b2.Origin.X + b2.Radius, b2.Origin.Y + b2.Radius);

            float distSquared = (float)Math.Pow(o1.X - o2.X, 2) + (float)Math.Pow(o1.Y - o2.Y, 2);

            if ((b1.Radius + b2.Radius) * (b1.Radius + b2.Radius) > 0)
                return true;
            return false;
        }


        private static IEnumerable<Tuple<Ball, Ball>> GetCollisions(IEnumerable<Ball> balls)
        {
            //TODO: idk, может из-за потоков просто считает не актуальный _balls??
            var collisions = new List<Tuple<Ball, Ball>>();

            int i = 0;
            for (int k = 0; k < balls.Count(); k++)
            {
                var b = _balls.ElementAt(k);
                for (int j = i; j < balls.Count(); j++)
                {
                    var c = balls.ElementAt(i);
                    if (b != c && HasCollision(b, c))
                        collisions.Add(new Tuple<Ball, Ball>(b, c));
                }

                if (i == balls.Count() - 1)
                    break;
                i++;
            }

            if (collisions.Count() != 0)
                return collisions;
            return null;
        }

        private static bool BounceBallFromBall(IEnumerable<Tuple<Ball, Ball>> collisions)
        {
            foreach (var collision in collisions)
            {
                var b1 = collision.Item1;
                var b2 = collision.Item2;

                b1.vX = 0;
                b1.vY = 0;
                b2.vX = 0;
                b2.vY = 0;
                // b1.Reverse();
                // b2.Reverse();
            }

            return true;
        }
    }
}