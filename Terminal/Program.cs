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

            InitContainer();
        }

        private static void InitContainer()
        {
            // Engine
            Container.Register<Game>(Lifestyle.Singleton);
            Container.Register<GameSettings>(Lifestyle.Singleton);
            Container.Register<MissionHandler>(Lifestyle.Singleton);

            // Screens.
            Container.Register<Options>();
            Container.Register<Introduction>();

            Container.Verify();
        }

        private static void ShowSplash()
        {
            var splash = new Splash();
            splash.Show();
        }

        private static void Run()
        {
            var mainMenu = new MainMenu();

            while(true)
            {
                mainMenu.Show();
            }
        }

        public static void NewGame()
        {
            var game = Container.GetInstance<Game>();
            game.Init();
            game.Start();
        }
    }
}