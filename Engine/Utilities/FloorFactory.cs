using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Engine.Enums;
using Engine.Models;

namespace Engine.Utilities
{
    public class FloorFactory
    {
        public void InitFloors(Mission mission, DifficultLevel gameDifficultLevel)
        {
            mission.CurrentFLoor = mission.Floors.First(f => f.IsStart);
            mission.Floors = mission.Floors.Where(f => IncludeFloorsForGameDifficultLevel(f.DifficultLevel, gameDifficultLevel)).ToList();
            mission.Floors.Reverse<Floor>().Aggregate((f1, f2) => f1.Above = f2);
            mission.Floors.Aggregate((f1, f2) => f1.Below = f2);
        }

        private bool IncludeFloorsForGameDifficultLevel(DifficultLevel floorDifficultLevel, DifficultLevel gameDifficultLevel)
        {
            if (floorDifficultLevel == DifficultLevel.Easy) return true;
            else if (floorDifficultLevel == DifficultLevel.Medium && (gameDifficultLevel == DifficultLevel.Medium || gameDifficultLevel == DifficultLevel.Hard)) return true;
            else if (floorDifficultLevel == DifficultLevel.Hard && gameDifficultLevel == DifficultLevel.Hard) return true;
            else return false;
        }

        public void CreateFloorPlans(Mission mission)
        {
            var floor = mission.CurrentFLoor;
            CreateFloorPlan(floor);
        }

        private void CreateFloorPlan(Floor floor)
        {
            SetStartRoomPosition(floor);
            GenerateMainPathway(floor);
        }

        private void SetStartRoomPosition(Floor floor)
        {
            var room = floor.MainRooms.First();
            var side = RandomHelper.GetEnum<Side>();
            var rnd = new Random();

            if (side == Side.Left)
            {
                var y = rnd.Next(floor.Size.Height + 1);
                room.Location = (0, y);
                room.Directions.Add(Side.Right);
                room.MainDirection = Side.Right;
            }
            else if (side == Side.Right)
            {
                var y = rnd.Next(floor.Size.Height + 1);
                room.Location = (floor.Size.Width - 1, y);
                room.Directions.Add(Side.Left);
                room.MainDirection = Side.Left;
            }
            else if (side == Side.Top)
            {
                var x = rnd.Next(floor.Size.Width + 1);
                room.Location= (x, 0);
                room.Directions.Add(Side.Bottom);
                room.MainDirection = Side.Bottom;
            }
            else if (side == Side.Bottom)
            {
                var x = rnd.Next(floor.Size.Width + 1);
                room.Location = (x, floor.Size.Height - 1);
                room.Directions.Add(Side.Top);
                room.MainDirection = Side.Top;
            }
        }

        private void GenerateMainPathway(Floor floor)
        {
            var rooms = floor.MainRooms.Skip(1);

            foreach (var room in rooms)
            {
                if (room.IsEnd || room.IsTerminal)
                {
                    TerminalRoomPathway(room);
                }
                else
                {
                    MainRoomPathway(room);
                }
            }

            foreach (var room in floor.MainRooms)
            {
                Console.WriteLine(room);
            }

            Console.ReadLine();
        }

        private void TerminalRoomPathway(Room room)
        {
            var previous = room.Previous;
            NextRoomLocation(room);
            room.MainDirection = GetOppositeDirection(previous.MainDirection);
        }

        private void MainRoomPathway(Room room)
        {
            NextRoomLocation(room);
            NextDirection(room);
        }

        private void NextRoomLocation(Room room)
        {
            var previous = room.Previous;

            if (previous.MainDirection == Side.Left)
            {
                room.Location = (previous.Location.X - 1, previous.Location.Y);
            }
            else if (previous.MainDirection == Side.Right)
            {
                room.Location = (previous.Location.X + 1, previous.Location.Y);
            }
            else if (previous.MainDirection == Side.Top)
            {
                room.Location = (previous.Location.X, previous.Location.Y - 1);
            }
            else
            {
                room.Location = (previous.Location.X, previous.Location.Y + 1);
            }
        }

        private Side GetOppositeDirection(Side direction)
        {
            if (direction == Side.Left) return Side.Right;
            else if (direction == Side.Right) return Side.Left;
            else if (direction == Side.Top) return Side.Bottom;
            else return Side.Top;
        }

        private void NextDirection(Room room)
        {
            var forbidden = new List<Side>
            {
                GetOppositeDirection(room.Previous.MainDirection)
            };
            var direction = RandomHelper.GetRandomDirection(forbidden);

            room.Directions.Add(direction);
            room.MainDirection = direction;
        }
    }
}
