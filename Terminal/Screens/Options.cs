using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal.Screens
{
    internal class Options : MenuBase
    {
        const ConsoleKey DifficultLevelKey = ConsoleKey.D;
        const ConsoleKey GameModeKey = ConsoleKey.G;
        const ConsoleKey ExitKey = ConsoleKey.X;

        public override void Show()
        {
            base.DisplayHeader();
            base.DisplayLogo(Resources.OptionsLogo);
            base.DisplayEmptyLine();
            base.DisplayEmptyLine();
            base.DisplayLeftAlignedContent("Select option:");
            base.DisplayEmptyLine();

            Engine.Models.DifficultLevel dl = Engine.Models.DifficultLevel.Easy;
            Engine.Models.GameMode gm = Engine.Models.GameMode.Speedrun;

            base.DisplayEnum("Difficult level", dl, DifficultLevelKey);
            base.DisplayEmptyLine();
            base.DisplayEnum("Game mode", gm, GameModeKey);
            base.DisplayEmptyLine();
            base.DisplayEmptyLine();
            base.DisplayAlignedContentAtSides("Back", $"({ExitKey})");
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
            if (input.Key == DifficultLevelKey)
            {
            }
            else if (input.Key == GameModeKey)
            {
            }
            else if (input.Key == ExitKey || input.Key == ConsoleKey.Escape)
            {
            }
        }
    }
}
