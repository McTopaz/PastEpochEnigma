using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Terminal.Screens
{
    internal class Briefing : PageMenu
    {
        private Game _game;
        private Mission _mission;

        public Briefing(Game game)
        {
            _game = game;

            if (_game.Missions.Count == 0) return;      // Load missions earlier and remove this.

            _mission = _game.Missions[_game.CurrentMission];

            NewObjectivesPage(_mission.Objectives);
            NewNotesPage(_mission.Notes);
        }

        public override void Show()
        {
            Console.Clear();
            
            var descriptions = _mission.Description.Split('.', StringSplitOptions.RemoveEmptyEntries).Select(s => $"{s.Trim()}.");

            base.DisplayHeader();
            base.DisplayLogo(Resources.BriefingLogo);
            base.DisplayEmptyLine();
            base.DisplayContent($"{_mission.Title} - {_mission.SubTitle}");
            base.DisplayEmptyLine();
            base.DisplayBodyText(descriptions);
            base.DisplayEmptyLine();

            var lines = Pages[CurrentPage];
            base.DisplayBodyText(lines);

            base.DisplayFooter();
            base.ScrollInput(CurrentPage + 1, Pages.Count);
        }
    }
}
