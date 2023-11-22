using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engine.Enums;
using Engine.Models;

namespace Terminal.Screens
{
    internal class Options : ScreenBase
    {
        const ConsoleKey DifficultLevelKey = ConsoleKey.D;
        const ConsoleKey ModeKey = ConsoleKey.M;
        const ConsoleKey ExitKey = ConsoleKey.X;

        readonly Settings _settings;

        public Options(Settings settings)
        {
            _settings = settings;
        }

        public override void Show()
        {
            Console.Clear();
            base.DisplayHeader();
            base.DisplayLogo(Resources.OptionsLogo);
            base.DisplayEmptyLine();
            base.DisplayEmptyLine();
            base.DisplayLeftAlignedContent("Select option:");
            base.DisplayEmptyLine();
            base.DisplayEnum("Difficult level", _settings.DifficultLevel, DifficultLevelKey);
            base.DisplayEmptyLine();
            base.DisplayEnum("Game mode", _settings.Mode, ModeKey);
            base.DisplayEmptyLine();
            base.DisplayEmptyLine();
            base.DisplayAlignedContentAtSides("Back", $"({ExitKey})");
            base.DisplayEmptyLine();
            base.DisplayEmptyLine();
            base.DisplayEmptyLine();
            base.DisplayFooter();
            Console.WriteLine();
            base.TakeInput();
        }

        protected override void ExecuteCommand(ConsoleKeyInfo input)
        {
            if (input.Key == ExitKey || input.Key == ConsoleKey.Escape)
            {
                return;
            }

            if (input.Key == DifficultLevelKey)
            {
                ToggleDifficultLevel();
            }
            else if (input.Key == ModeKey)
            {
                ToggleMode();
            }

            Show();
        }

        private void ToggleDifficultLevel()
        {
            if (_settings.DifficultLevel == DifficultLevel.Easy) _settings.DifficultLevel = DifficultLevel.Medium;
            else if (_settings.DifficultLevel == DifficultLevel.Medium) _settings.DifficultLevel = DifficultLevel.Hard;
            else if (_settings.DifficultLevel == DifficultLevel.Hard) _settings.DifficultLevel = DifficultLevel.Easy;
        }

        private void ToggleMode()
        {
            _settings.Mode = _settings.Mode == Mode.Normal ? Mode.Speedrun : Mode.Normal;
        }
    }
}
