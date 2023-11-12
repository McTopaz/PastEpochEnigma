using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Terminal.Screens
{
    internal class MainMenu : MenuBase
    {
        const ConsoleKey NewGameKey = ConsoleKey.N;
        const ConsoleKey LoadGameKey = ConsoleKey.L;
        const ConsoleKey OptionsKey = ConsoleKey.O;
        const ConsoleKey IntroductionKey = ConsoleKey.I;
        const ConsoleKey ExitKey = ConsoleKey.X;

        public override void Show()
        {
            Console.Clear();
            base.DisplayHeader();
            base.DisplayLogo(Resources.MenuLogo);
            base.DisplayEmptyLine();
            base.DisplayEmptyLine();
            base.DisplayLeftAlignedContent("Select option:");
            base.DisplayEmptyLine();
            base.DisplayAlignedContentAtSides("1) New game", $"({ NewGameKey })");
            base.DisplayEmptyLine();
            base.DisplayAlignedContentAtSides("2) Load game", $"({ LoadGameKey })");
            base.DisplayEmptyLine();
            base.DisplayAlignedContentAtSides("3) Options", $"({ OptionsKey })");
            base.DisplayEmptyLine();
            base.DisplayAlignedContentAtSides("4) Introduction", $"({ IntroductionKey })");
            base.DisplayEmptyLine();
            base.DisplayAlignedContentAtSides("5) Exit", $"({ ExitKey })");
            base.DisplayEmptyLine();
            base.DisplayEmptyLine();
            base.DisplayEmptyLine();
            base.DisplayAuthorAndVersion();
            base.DisplayFooter();
            Console.WriteLine();
            base.TakeInput();
        }

        protected override void ExecuteCommand(ConsoleKeyInfo input)
        {
            if (input.Key == NewGameKey || input.Key == ConsoleKey.D1 || input.Key == ConsoleKey.NumPad1)
            {
            }
            else if (input.Key == LoadGameKey || input.Key == ConsoleKey.D2 || input.Key == ConsoleKey.NumPad2)
            {
            }
            else if (input.Key == OptionsKey || input.Key == ConsoleKey.D3 || input.Key == ConsoleKey.NumPad3)
            {
                Program.Container.GetInstance<Options>().Show();
            }
            else if (input.Key == IntroductionKey || input.Key == ConsoleKey.D4 || input.Key == ConsoleKey.NumPad4)
            {
                Program.Container.GetInstance<Introduction>().Show();
            }
            else if (input.Key == ExitKey || input.Key == ConsoleKey.Escape || input.Key == ConsoleKey.D5 || input.Key == ConsoleKey.NumPad5)
            {
                Environment.Exit(0);
            }
            else
            {
                base.DisplayUnknownInput();
            }
        }
    }
}
