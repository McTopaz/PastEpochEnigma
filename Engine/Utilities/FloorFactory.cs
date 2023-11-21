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
    }
}
