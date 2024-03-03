using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleInjector;

using Engine.Utilities;

namespace Engine.Models
{
    public class Game
    {
        private static readonly Container Container;

        static Game()
        {
            Container = new Container();
            InitContainer();
        }

        static void InitContainer()
        {
            Container.Register<Settings>(Lifestyle.Singleton);
            Container.Register<MissionLoader>(Lifestyle.Singleton);
            Container.Register<MissionFactory>(Lifestyle.Singleton);
            Container.Register<RoomGenerator>(Lifestyle.Singleton);
            Container.Register<RoomSorter>(Lifestyle.Singleton);
            Container.Register<FloorPlanFactory>(Lifestyle.Singleton);
            Container.Register<RandomPathGenerator>(Lifestyle.Singleton);
        }

        public Settings Settings { get; set; }
        public Awarness Awarness { get; set; } = new Awarness();
        public List<Mission> Missions { get; set; } = new List<Mission>();

        private int _index = 0;

        public Mission GetCurrentMission()
        {
            return Missions[_index];
        }

        public ActionController GetActionController(Floor floor)
        {
            return new ActionController(floor);
        }

        public void Init()
        {
            var loader = Container.GetInstance<MissionLoader>();

            loader.LoadMissionsFromAssets();
            Missions = loader.RandomizedMissions();
        }

        public void Setup()
        {
            var mission = GetCurrentMission();
            var missionFactory = Container.GetInstance<MissionFactory>();
            var roomGenerator = Container.GetInstance<RoomGenerator>();
            var floorPlanFactory = Container.GetInstance<FloorPlanFactory>();

            missionFactory.InitFloors(mission, Settings.DifficultLevel);
            roomGenerator.CreateRoomsForMissionFloors(mission, Settings.DifficultLevel);
            floorPlanFactory.CreateFloorPlans(mission);
        }
    }
}
