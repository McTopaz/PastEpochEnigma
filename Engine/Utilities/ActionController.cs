using Engine.Enums;
using Engine.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utilities
{
    public class ActionController
    {
        readonly Floor _floor;

        public ActionController(Floor floor)
        {
            _floor = floor;
        }

        public MoveResult Move(Direction direction)
        {
            var canMove = CanMove(direction);

            if (canMove)
            {
                DoMove(direction);
                return OkMoveResult();
            }
            else
            {
                return NOkMoveResult($"No adjacent room at specified direction {direction}");
            }
        }

        private bool CanMove(Direction direction)
        {
            var room = _floor.CurrentRoom;
            var canGoDirectionInto = room.Into == direction;
            var canGoDirectionOutOf = room.OutOf == direction;
            return canGoDirectionInto || canGoDirectionOutOf;
        }

        private void DoMove(Direction direction)
        {
            var position = _floor.Position;
            Point newDirection = Point.Empty;

            switch (direction)
            {
                case Direction.Left:
                    newDirection = new Point(position.X - 1, position.Y);
                    break;
                case Direction.Right:
                    newDirection = new Point(position.X + 1, position.Y);
                    break;
                case Direction.Up:
                    newDirection = new Point(position.X, position.Y - 1);
                    break;
                case Direction.Down:
                    newDirection = new Point(position.X, position.Y + 1);
                    break;
                default:
                    return;
            }

            UpdateCurrentRoom(newDirection);
        }

        private void UpdateCurrentRoom(Point newPosition)
        {
            var newRoom = _floor.Rooms.First(r => r.Position == newPosition);
            _floor.CurrentRoom = newRoom;
        }

        private MoveResult OkMoveResult()
        {
            return new MoveResult
            {
                CanMove = true
            };
        }

        private MoveResult NOkMoveResult(string reason)
        {
            return new MoveResult
            {
                CanMove = false,
                Reason = reason
            };
        }
    }
}
