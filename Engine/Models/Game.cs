using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Game
    {
        public Settings Settings { get; set; }
        public Awarness Awarness { get; set; } = new Awarness();
        public List<Mission> Missions { get; set; } = new List<Mission>();

        private int _index = 0;

        public Game(Settings settings)
        {
            Settings = settings;
        }

        public Mission GetCurrentMission()
        {
            return Missions[_index];
        }
    }
}
