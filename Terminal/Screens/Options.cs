﻿using Engine.Models;
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
        const ConsoleKey ModeKey = ConsoleKey.M;
        const ConsoleKey ExitKey = ConsoleKey.X;

        readonly Settings _gameSettings;

        public Options(Settings gameSettings)
        {
            _gameSettings = gameSettings;
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
            base.DisplayEnum("Difficult level", _gameSettings.DifficultLevel, DifficultLevelKey);
            base.DisplayEmptyLine();
            base.DisplayEnum("Game mode", _gameSettings.Mode, ModeKey);
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
            if (_gameSettings.DifficultLevel == DifficultLevel.Easy) _gameSettings.DifficultLevel = DifficultLevel.Medium;
            else if (_gameSettings.DifficultLevel == DifficultLevel.Medium) _gameSettings.DifficultLevel = DifficultLevel.Hard;
            else if (_gameSettings.DifficultLevel == DifficultLevel.Hard) _gameSettings.DifficultLevel = DifficultLevel.Easy;
        }

        private void ToggleMode()
        {
            _gameSettings.Mode = _gameSettings.Mode == Mode.Normal ? Mode.Speedrun : Mode.Normal;
        }
    }
}
