using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engine.Models;
using Terminal.Icons;
using Terminal.Models;
using static System.Collections.Specialized.BitVector32;

namespace Terminal.Screens
{
    internal abstract class PageMenu : MenuBase
    {
        const ConsoleKey UpArrowKey = ConsoleKey.UpArrow;
        const ConsoleKey DownArrowKey = ConsoleKey.DownArrow;
        const ConsoleKey EscapeKey = ConsoleKey.Escape;

        protected Dictionary<int, string[]> Pages { get; set; } = new Dictionary<int, string[]>();
        private int Index { get; set; } = 0;
        protected int CurrentPage { get; private set; } = 0;

        protected void NewObjectivesPage(List<Objective> objectives)
        {
            var lines = new List<string>
            {
                "Objectives:"
            };

            foreach (var objective in objectives)
            {
                var mark = objective.IsFinished ? CheckBoxIcons.CheckedBox : CheckBoxIcons.EmptyBox;
                lines.Add($"{mark}   {objective.Name}");
                lines.Add($"    {objective.Description}");
                lines.Add(string.Empty);
            }

            Pages.Add(Index, lines.ToArray());
            Index += 1;
        }

        protected void NewNotesPage(List<string> notes)
        {
            var lines = new List<string>
            {
                "Notes:"
            };
            lines.AddRange(notes.Select(n => $"{ GeneralPunctuationIcons.Bullet } {n}"));
            lines.Add(string.Empty);

            Pages.Add(Index, lines.ToArray());
            Index += 1;
        }

        protected override void ExecuteCommand(ConsoleKeyInfo input)
        {
            if (input.Key == EscapeKey)
            {
                return;
            }

            if (input.Key == UpArrowKey)
            {
                PreviousPage();
            }
            else if (input.Key == DownArrowKey)
            {
                NextPage();
            }

            Show();
        }

        protected void NextPage()
        {
            CurrentPage = CurrentPage < Pages.Count() - 1 ? CurrentPage += 1 : CurrentPage;
        }

        protected void PreviousPage()
        {
            CurrentPage = CurrentPage > 0 ? CurrentPage -= 1 : CurrentPage;
        }
    }
}
