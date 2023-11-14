using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engine.Models;

namespace Engine
{
    public class FloorFactory
    {
        public void InitFloors(Mission mission, DifficultLevel gameDifficultLevel)
        {
            mission.CurrentFLoor = mission.Floors.First(f => f.IsStart);
            mission.Floors = mission.Floors.Where(f => IncludeForGameDifficultLevel(f.DifficultLevel, gameDifficultLevel)).ToList();
            mission.Floors.Reverse<Floor>().Aggregate((f1, f2) => f1.Above = f2);
            mission.Floors.Aggregate((f1, f2) => f1.Below = f2);
        }

        private bool IncludeForGameDifficultLevel(DifficultLevel floorDifficultLevel, DifficultLevel gameDifficultLevel)
        {
            if (floorDifficultLevel == DifficultLevel.Easy) return true;
            else if (floorDifficultLevel == DifficultLevel.Medium && (gameDifficultLevel == DifficultLevel.Medium || gameDifficultLevel == DifficultLevel.Hard)) return true;
            else if (floorDifficultLevel == DifficultLevel.Hard && gameDifficultLevel == DifficultLevel.Hard) return true;
            else return false;
        }
    }
}
