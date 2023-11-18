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
        }

        private void SetStartRoomPosition(Floor floor)
        {
            var room = floor.Rooms.First();
            var side = RandomHelper.GetEnum<Side>();
            var rnd = new Random();

            if (side == Side.Left)
            {
                var y = rnd.Next(floor.Size.Height + 1);
                room.Location = (0, y);
                room.Directions.Add(Side.Right);
            }
            else if (side == Side.Right)
            {
                var y = rnd.Next(floor.Size.Height + 1);
                room.Location = (floor.Size.Width, y);
                room.Directions.Add(Side.Left);
            }
            else if (side == Side.Top)
            {
                var x = rnd.Next(floor.Size.Width + 1);
                room.Location= (x, 0);
                room.Directions.Add(Side.Bottom);
            }
            else if (side == Side.Bottom)
            {
                var x = rnd.Next(floor.Size.Width + 1);
                room.Location = (x, floor.Size.Height);
                room.Directions.Add(Side.Top);
            }
        }
    }
}
