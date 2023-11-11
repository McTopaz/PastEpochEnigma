using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terminal.Screens;

namespace Terminal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Init();
            //ShowSplash();
            Run();
        }

        private static void Init()
        {
            Console.OutputEncoding = Encoding.Unicode;
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

    }
}