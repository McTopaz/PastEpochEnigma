using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        const ConsoleKey EnterKey = ConsoleKey.Enter;
        const ConsoleKey EscapeKey = ConsoleKey.Escape;

        protected Dictionary<int, string[]> Pages { get; set; } = new Dictionary<int, string[]>();
        private int _pageCounter  = 0;
        private int _index = 0;

        protected void NewBodyText(string[] lines)
        {
            Pages.Add(_pageCounter, lines);
            _pageCounter += 1;
        }

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

            Pages.Add(_pageCounter, lines.ToArray());
            _pageCounter += 1;
        }

        protected void NewNotesPage(List<string> notes)
        {
            var lines = new List<string>
            {
                "Notes:"
            };
            lines.AddRange(notes.Select(n => $"{ GeneralPunctuationIcons.Bullet } {n}"));
            lines.Add(string.Empty);

            Pages.Add(_pageCounter, lines.ToArray());
            _pageCounter += 1;
        }

        protected string[] CurrentPage()
        {
            return Pages[_index];
        }

        protected void ScrollInput(string actionCommand)
        {
            DisplayCustomLine
            (
                OuterMargin,
                OuterMargin,
                $"Page {_index + 1} of {Pages.Count}.\t\t",
                $"Press the {ArrowsIcons.Up} or the {ArrowsIcons.Down} arrow keys to scroll.\t\t",
                actionCommand
            );

            var input = Console.ReadKey();
            ExecuteCommand(input);
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
            else if (input.Key == EnterKey)
            {
                OnEnterPressed();
            }

            Show();
        }

        protected void NextPage()
        {
            _index = _index < Pages.Count() - 1 ? _index += 1 : _index;
        }

        protected void PreviousPage()
        {
            _index = _index > 0 ? _index -= 1 : _index;
        }

        protected virtual void OnEnterPressed()
        {
        }
    }
}
