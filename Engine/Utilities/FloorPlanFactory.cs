using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engine.Enums;
using Engine.Models;

namespace Engine.Utilities
{
    public class FloorPlanFactory
    {
        private RandomPathGenerator _randomPathGenerator;

        public FloorPlanFactory(RandomPathGenerator randomPathGenerator)
        {
            _randomPathGenerator = randomPathGenerator;
        }

        public void CreateFloorPlans(Mission mission)
        {
            var floor = mission.Floors.First(f => f.IsStart);
            CreateFloorPlan(floor);
        }

        private void CreateFloorPlan(Floor floor)
        {
            RandomStartPosition(floor);
            FloorPlanGeneration(floor);
            MergeRoomsWithPath(floor);
            floor.Position = floor.Rooms.First().Position;
        }

        private void RandomStartPosition(Floor floor)
        {
            var room = floor.Rooms.First();
            var side = RandomHelper.GetEnum<Side>();
            var rnd = new Random();

            if (side == Side.Left)
            {
                var y = rnd.Next(floor.Size.Height);
                room.Position = new Point(0, y);
                floor.ForbiddenDirection = Direction.Left;
            }
            else if (side == Side.Right)
            {
                var y = rnd.Next(floor.Size.Height);
                room.Position = new Point(floor.Size.Width - 1, y);
                floor.ForbiddenDirection = Direction.Right;
            }
            else if (side == Side.Top)
            {
                var x = rnd.Next(floor.Size.Width);
                room.Position = new Point(x, 0);
                floor.ForbiddenDirection = Direction.Up;
            }
            else if (side == Side.Bottom)
            {
                var x = rnd.Next(floor.Size.Width);
                room.Position = new Point(x, floor.Size.Height - 1);
                floor.ForbiddenDirection = Direction.Down;
            }
        }

        private void FloorPlanGeneration(Floor floor)
        {
            do
            {
                GenerateRandomPath(floor);

                if (floor.Path.Count < floor.Rooms.Count)
                {
                    RemoveAnyIntermediateRoom(floor);
                }
            }
            while (floor.Path.Count < floor.Rooms.Count);
        }

        private void GenerateRandomPath(Floor floor)
        {
            var size = floor.Size;
            var firstRoom = floor.Rooms.First();
            var start = firstRoom.Position;
            var steps = floor.Rooms.Count - 1;
            var path = _randomPathGenerator.Generate(size, start, steps, floor.ForbiddenDirection);

            floor.Path.Add(new(Direction.None, firstRoom.Position));
            floor.Path.AddRange(path);
        }

        private void RemoveAnyIntermediateRoom(Floor floor)
        {
            var count = floor.Rooms.Count - floor.Path.Count;

            if (floor.Rooms.Where(r => r.IsIntermediate).Count() < count)
            {
                var msg = "Failed to remove enough intermediate rooms to fix path. There are too few Intermediate rooms";
                Console.WriteLine(msg);
                throw new Exception(msg);
            }

            for (int i = 0; i < count; i++)
            {
                var room = floor.Rooms.First(r => r.IsIntermediate);
                var index = floor.Rooms.IndexOf(room);
                room.Previous.Next = room.Next;
                room.Next.Previous = room.Previous;
                floor.Rooms.RemoveAt(index);
            }
        }

        private void MergeRoomsWithPath(Floor floor)
        {
            for (int i = 1; i < floor.Rooms.Count; i++)
            {
                var room = floor.Rooms[i];
                var path = floor.Path[i];

                room.Position = path.Position;
                room.Into = OppositeDirection(path.Direction);
                floor.Rooms[i - 1].OutOf = path.Direction;
            }
        }

        private Direction OppositeDirection(Direction direction)
        {
            if (direction == Direction.Left) return Direction.Right;
            else if (direction == Direction.Up) return Direction.Down;
            else if (direction == Direction.Right) return Direction.Left;
            else return Direction.Up;
        }
    }
}
