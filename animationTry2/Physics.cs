using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Threading;
using System.Numerics;

namespace animationTry2
{
    public class Physics
    {
        private static Animator _animator;
        private static List<Ball> _balls;
        private static Statistics _statisticsForm;
        private static object _obj = new();
        private static bool _wasCollision = false;

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
                    //(ball.vX, ball.vY) = (0, 0);
                    ball.IsAlive = false;
                    _animator.RemoveBall(ball);
                }

                // if (_statisticsForm != null) //todo: trouble with threads
                //     _statisticsForm.DataGridRefresh();
            }

            var collisions = Physics.GetCollisions(_balls); //todo
            if (collisions != null)
                lock (_balls)
                {
                    BounceBallFromBall(collisions, ball);
                }


            ball.vX *= 0.995f;
            ball.vY *= 0.995f;

            ball.Color = ball.ChangeBrightness(-0.8f); //todo: calculate factor


            if (Math.Abs(ball.vX) <= 0.5 && Math.Abs(ball.vY) <= 0.5)
            {
                ball.IsAlive = false;
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

            if (ball.Origin.X + ball.Radius * 2 >= containerSize.Width - 1)
            {
                ball.vX = -ball.vX;
                return true;
            }

            if (ball.Origin.Y <= 1)
            {
                ball.vY = -ball.vY;
                return true;
            }

            if (ball.Origin.Y + ball.Radius * 2 >= containerSize.Height - 1)
            {
                ball.vY = -ball.vY;
                return true;
            }

            return false;
        }

        private static bool HasCollision(Ball b1, Ball b2)
        {
            var o1 = new PointF(b1.Origin.X + b1.Radius, b1.Origin.Y + b1.Radius);
            var o2 = new PointF(b2.Origin.X + b2.Radius, b2.Origin.Y + b2.Radius);

            float distSquared = (float)Math.Pow(o1.X - o2.X, 2) + (float)Math.Pow(o1.Y - o2.Y, 2);

            if ((b1.Radius + b2.Radius) * (b1.Radius + b2.Radius) >= distSquared) //to avoid overlapping
            {
                double angle = System.Math.Atan2(o2.Y - o1.Y, o2.X - o1.X);
                double distanceToMove = b1.Radius + b2.Radius - Math.Sqrt(distSquared);
                o2.X += (float)(Math.Cos(angle) * distanceToMove);
                o2.Y += (float)(Math.Sin(angle) * distanceToMove);

                b2.Origin.X = o2.X - b2.Radius;
                b2.Origin.Y = o2.Y - b2.Radius;

                return true;
            }

            return false;
        }


        private static IEnumerable<Tuple<Ball, Ball>> GetCollisions(IEnumerable<Ball> balls)
        {
            var collisions = new List<Tuple<Ball, Ball>>();

            int i = 0;
            for (int k = 0; k < balls.Count(); k++)
            {
                var b = _balls.ElementAt(k);
                for (int j = i; j < balls.Count(); j++)
                {
                    var c = balls.ElementAt(j);
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

        private static bool BounceBallFromBall(IEnumerable<Tuple<Ball, Ball>> collisions, Ball ball)
        {
            // TODO: у каждого шарика свой поток, каждый шарик из коллизий заходит сюда и обрабатывает себя, решить!
            if (collisions is null) return false;
            if (_wasCollision) return false;
            // foreach (var collision in collisions)
            // {
            //     Ball b1, b2;
            //
            //     if (collision.Item1 == ball)
            //     {
            //         b1 = collision.Item1;
            //         b2 = collision.Item2;
            //     }
            //     else if (collision.Item2 == ball)
            //     {
            //         b1 = collision.Item2;
            //         b2 = collision.Item1;
            //     }
            //     else
            //         return false;
            //
            //     //todo: calculate physics like impulse
            //     var o1 = new PointF(b1.Origin.X + b1.Radius, b1.Origin.Y + b1.Radius);
            //     var o2 = new PointF(b2.Origin.X + b2.Radius, b2.Origin.Y + b2.Radius);
            //
            //     Vector2 tangent = new Vector2(o2.Y - o1.Y, -(o2.X - o1.X));
            //     tangent = Vector2.Normalize(tangent);
            //
            //     Vector2 relativeVelocity = new Vector2(b2.vX - b1.vX, b2.vY - b1.vY);
            //     float lenght = Vector2.Dot(relativeVelocity, tangent);
            //
            //
            //     Vector2 velocityComponentOnTangent = Vector2.Multiply(lenght, tangent);
            //
            //     Vector2 velocityComponentPerpendicularToTangent =
            //         relativeVelocity - velocityComponentOnTangent;
            //
            //     // to make both circles move.  //todo: too fast bouncing
            //     b1.vX = -velocityComponentPerpendicularToTangent.X;
            //     b1.vY = -velocityComponentPerpendicularToTangent.Y;
            //
            //     // b2.vX = velocityComponentPerpendicularToTangent.X;
            //     // b2.vY = velocityComponentPerpendicularToTangent.Y;
            // }
            
            foreach (var collision in collisions)
            {
                var b1 = collision.Item1;
                var b2 = collision.Item2;
            
                //todo: calculate physics like impulse
                var o1 = new PointF(b1.Origin.X + b1.Radius, b1.Origin.Y + b1.Radius);
                var o2 = new PointF(b2.Origin.X + b2.Radius, b2.Origin.Y + b2.Radius);
            
                Vector2 tangent = new Vector2(o2.Y - o1.Y, -(o2.X - o1.X));
                tangent = Vector2.Normalize(tangent);
            
                Vector2 relativeVelocity = new Vector2(b2.vX - b1.vX, b2.vY - b1.vY);
                float lenght = Vector2.Dot(relativeVelocity, tangent);
            
            
                Vector2 velocityComponentOnTangent = Vector2.Multiply(lenght, tangent);
            
                Vector2 velocityComponentPerpendicularToTangent =
                    relativeVelocity - velocityComponentOnTangent;
            
                // to make both circles move.  //todo: too fast bouncing
                b1.vX = -velocityComponentPerpendicularToTangent.X;
                b1.vY = -velocityComponentPerpendicularToTangent.Y;
            
                b2.vX = velocityComponentPerpendicularToTangent.X;
                b2.vY = velocityComponentPerpendicularToTangent.Y;
            }

            _wasCollision = true;
            return true;
        }
    }
}