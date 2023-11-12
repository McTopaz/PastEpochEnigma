using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Game
    {
        public GameSettings GameSettings { get; set; }
        public MissionLoader MissionHandler { get; set; }

        public Awarness Awarness { get; set; } = new Awarness();
        public List<Mission> Missions { get; set; } = new List<Mission>();
        public int CurrentMission { get; set; } = 0;


        public Game(GameSettings gameSettings, MissionLoader missionHandler)
        {
            GameSettings = gameSettings;
            MissionHandler = missionHandler;
        }

        public void Init()
        {
        }

        public void Start()
        {

        }
    }
}
