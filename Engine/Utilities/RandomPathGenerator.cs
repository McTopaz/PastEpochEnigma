using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engine.Enums;

namespace Engine.Utilities
{
    public class RandomPathGenerator
    {
        private Size Size { get; set; } = new Size();
        private Point Current { get; set; } = new Point();
        private readonly List<Direction> Allowed;

        public RandomPathGenerator()
        {
            Allowed = new List<Direction> { Direction.Left, Direction.Up, Direction.Right, Direction.Down };
        }

        public List<(Direction Direction, Point Position)> Generate(Size size, Point start, int steps, Direction forbidden)
        {
            Size = size;
            Current = start;
            Allowed.Remove(forbidden);

            var path = new List<(Direction Direction, Point Position)>();
            var previous = Direction.None;

            for (var i = 0; i < steps; i++)
            {
                var notAllowed = NotAllowedDirections(previous);
                var directions = Allowed.Except(notAllowed).ToList();

                if (previous == forbidden)
                {
                    i = -1;
                    Current = start;
                    path.Clear();
                    previous = Direction.None;
                    continue;
                }
                else if (directions.Count == 0)
                {
                    previous = forbidden;
                }
                else
                {
                    previous = RandomHelper.GetRandomInList(directions);
                }

                Move(previous);
                path.Add((previous, Current));
            }

            return path;
        }

        private List<Direction> NotAllowedDirections(Direction previous)
        {
            var forbiddenBoundryDirection = CheckBoundariesForForbiddenDirection();
            var forbiddenCornerDirections = CheckCornersForForbiddenDirections();
            var notAllowed = new List<Direction>
            {
                OppositeDirection(previous)
            };
            notAllowed.AddRange(forbiddenCornerDirections);

            if (forbiddenBoundryDirection != Direction.None)
            {
                notAllowed.Add(forbiddenBoundryDirection);
            }

            return notAllowed;
        }

        private Direction CheckBoundariesForForbiddenDirection()
        {
            if (Current.X == 0)
            {
                return Direction.Left;
            }
            else if (Current.Y == 0)
            {
                return Direction.Up;
            }
            else if (Current.X == Size.Width - 1)
            {
                return Direction.Right;
            }
            else if (Current.Y == Size.Height - 1)
            {
                return Direction.Down;
            }

            return Direction.None;
        }

        private List<Direction> CheckCornersForForbiddenDirections()
        {
            if (Current.X == 0 && Current.Y == 0)
            {
                return new List<Direction>() { Direction.Left, Direction.Up };
            }
            else if (Current.X == Size.Width - 1 && Current.Y == 0)
            {
                return new List<Direction>() { Direction.Right, Direction.Up };
            }
            else if (Current.X == 0 && Current.Y == Size.Height - 1)
            {
                return new List<Direction>() { Direction.Left, Direction.Down };
            }
            else if (Current.X == Size.Width - 1 && Current.Y == Size.Height - 1)
            {
                return new List<Direction>() { Direction.Right, Direction.Down };
            }

            return Enumerable.Empty<Direction>().ToList();
        }

        private Direction OppositeDirection(Direction direction)
        {
            if (direction == Direction.Left) return Direction.Right;
            else if (direction == Direction.Up) return Direction.Down;
            else if (direction == Direction.Right) return Direction.Left;
            else return Direction.Up;
        }

        private void Move(Direction direction)
        {
            if (direction == Direction.Left)
            {
                Current = new Point(Current.X - 1, Current.Y);
            }
            else if (direction == Direction.Up)
            {
                Current = new Point(Current.X, Current.Y - 1);
            }
            else if (direction == Direction.Right)
            {
                Current = new Point(Current.X + 1, Current.Y);
            }
            else if (direction == Direction.Down)
            {
                Current = new Point(Current.X, Current.Y + 1);
            }
        }

    }
}
