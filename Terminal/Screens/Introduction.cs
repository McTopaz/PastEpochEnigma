using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal.Screens
{
    internal class Introduction : MenuBase
    {
        const string LineBreak = "<br>";
        const ConsoleKey UpArrowKey = ConsoleKey.UpArrow;
        const ConsoleKey DownArrowKey = ConsoleKey.DownArrow;
        const ConsoleKey EscapeKey = ConsoleKey.Escape;

        private IEnumerable<string> Sections { get; set; } = Enumerable.Empty<string>();
        private int CurrentSection = 0;

        public Introduction()
        {
            Sections = Resources.Introduction.Split(LineBreak, StringSplitOptions.TrimEntries);
        }

        public override void Show()
        {
            Console.Clear();
            base.DisplayHeader();
            base.DisplayLogo(Resources.IntroductionLogo);
            base.DisplayEmptyLine();
            base.DisplayEmptyLine();
            var rows = Sections.ElementAt(CurrentSection).Split(Environment.NewLine);
            base.DisplayBodyText(rows);
            base.DisplayFooter();

            base.ScrollInput();
        }

        protected override void ExecuteCommand(ConsoleKeyInfo input)
        {
            if (input.Key == EscapeKey)
            {
                return;
            }

            if (input.Key == UpArrowKey)
            {
                CurrentSection = CurrentSection > 0 ? CurrentSection -= 1 : CurrentSection;
            }
            else if (input.Key == DownArrowKey)
            {
                CurrentSection = CurrentSection < Sections.Count() - 1 ? CurrentSection += 1 : CurrentSection;
            }

            Show();
        }
    }
}
