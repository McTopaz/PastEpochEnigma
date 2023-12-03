using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector;

using Engine.Models;
using Engine.Utilities;

using Terminal.Screens;
using SimpleInjector.Lifestyles;

namespace Terminal
{
    public class Program
    {
        public static readonly Container Container;

        static Program()
        {
            Container = new Container();
        }

        public static void Main(string[] args)
        {
            Init();
            //ShowSplash();
            Run();
        }

        private static void Init()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.Title = $"{Engine.Resources.Title } - {Engine.Resources.SubTitle }";

            InitContainer();
            InitMissions();
        }

        private static void InitContainer()
        {
            // Engine
            Container.Register<Game>(Lifestyle.Singleton);
            Container.Register<Settings>(Lifestyle.Singleton);
            Container.Register<MissionLoader>(Lifestyle.Singleton);
            Container.Register<FloorFactory>(Lifestyle.Singleton);
            Container.Register<RoomGenerator>(Lifestyle.Singleton);
            Container.Register<RoomSorter>(Lifestyle.Singleton);
            Container.Register<FloorPlanFactory>(Lifestyle.Singleton);
            Container.Register<RandomPathGenerator>(Lifestyle.Singleton);

            // Screens.
            Container.Register<Splash>();
            Container.Register<MainMenu>();
            Container.Register<Options>();
            Container.Register<Introduction>();
            Container.Register<Briefing>();
            Container.Register<GameView>();

            Container.Verify();
        }

        private static void InitMissions()
        {
            var loader = Container.GetInstance<MissionLoader>();
            var game = Container.GetInstance<Game>();

            loader.LoadMissionsFromAssets();
            game.Missions = loader.RandomizedMissions();
        }

        private static void ShowSplash()
        {
            var splash = Container.GetInstance<Splash>();
            splash.Show();
        }

        private static void Run()
        {
            var mainMenu = Container.GetInstance<MainMenu>();

            while(true)
            {
                mainMenu.Show();
            }
        }

        public static void NewGame()
        {
            var game = Container.GetInstance<Game>();
            var screen = Container.GetInstance<Briefing>();
            screen.Show(game.Missions.First());

            if (screen.StartGame)
            {
                SetupGame(game);
                MainGameLoop(game);
            }
        }

        private static void SetupGame(Game game)
        {
            var mission = game.GetCurrentMission();
            var floorFactory = Container.GetInstance<FloorFactory>();
            var roomGenerator = Container.GetInstance<RoomGenerator>();
            var floorPlanFactory = Container.GetInstance<FloorPlanFactory>();

            floorFactory.InitFloors(mission, game.Settings.DifficultLevel);
            roomGenerator.CreateRoomsForMissionFloors(mission, game.Settings.DifficultLevel);
            floorPlanFactory.CreateFloorPlans(mission);
        }

        private static void MainGameLoop(Game game)
        {
            var gameView = Container.GetInstance<GameView>();

            while (true)
            {
                var mission = game.GetCurrentMission();
                var floor = mission.CurrentFloor;
                var rooms = floor.Rooms;

                gameView.Show(floor, rooms);
            }
        }
    }
}