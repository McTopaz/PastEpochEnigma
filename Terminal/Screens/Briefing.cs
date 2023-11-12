using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Terminal.Icons;

namespace Terminal.Screens
{
    internal class Briefing : PageMenu
    {
        private Mission _mission;

        public override void Show()
        {
            Console.Clear();
            
            var descriptions = _mission.Description.Split('.', StringSplitOptions.RemoveEmptyEntries).Select(s => $"{s.Trim()}.");

            base.DisplayHeader();
            base.DisplayLogo(Resources.BriefingLogo);
            base.DisplayEmptyLine();
            base.DisplayContent($"Mission: {_mission.Title}");
            base.DisplayEmptyLine();
            base.DisplayBodyText(descriptions);
            base.DisplayEmptyLine();

            var lines = CurrentPage();
            base.DisplayBodyText(lines);

            base.DisplayFooter();
            base.ScrollInput($"Press {ButtonIcons.Enter} to start.");
        }

        public void Show(Mission mission)
        {
            _mission = mission;
            NewObjectivesPage(mission.Objectives);
            NewNotesPage(mission.Notes);
            Show();
        }

        protected override void OnEnterPressed()
        {
            
        }
    }
}
