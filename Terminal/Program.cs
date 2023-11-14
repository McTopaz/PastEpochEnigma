using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector;

using Engine;
using Engine.Models;

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
            Console.Title = $"{ Resources.Title } - { Resources.SubTitle }";

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

            // Screens.
            Container.Register<Splash>();
            Container.Register<MainMenu>();
            Container.Register<Options>();
            Container.Register<Introduction>();
            Container.Register<Briefing>();

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
                MainGameLoop(game);
            }
        }

        private static void MainGameLoop(Game game)
        {
            var mission = game.GetCurrentMission();
            var floorFactory = Container.GetInstance<FloorFactory>();

            floorFactory.InitFloors(mission, game.GameSettings.DifficultLevel);
            //floorFactory.CreateFloor()

            while(true)
            {

            }
        }
    }
}