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
            InitConsole();
            InitContainer();
            InitGame();
        }

        private static void InitConsole()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.Title = $"{Engine.Resources.Title} - {Engine.Resources.SubTitle}";
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void InitContainer()
        {
            // Engine
            Container.Register<Game>(Lifestyle.Singleton);
            Container.Register<Settings>(Lifestyle.Singleton);

            // Screens.
            Container.Register<Splash>();
            Container.Register<MainMenu>();
            Container.Register<Options>();
            Container.Register<Introduction>();
            Container.Register<Briefing>();
            Container.Register<GameView>();

            Container.Verify();
        }

        private static void InitGame()
        {
            var game = Container.GetInstance<Game>();
            game.Settings = Container.GetInstance<Settings>();
            game.Init();
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
                game.Setup();
                MainGameLoop(game);
            }
        }

        private static void MainGameLoop(Game game)
        {
            var gameView = Container.GetInstance<GameView>();

            while (true)
            {
                var mission = game.GetCurrentMission();
                var floor = mission.CurrentFloor;
                var actionController = game.GetActionController(floor);
                gameView.Show(floor, actionController);
            }
        }
    }
}