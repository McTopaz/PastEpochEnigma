using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Icons;

namespace Terminal.Screens
{
    internal class Introduction : PageMenu
    {
        const string LineBreak = "<br>";

        public Introduction()
        {
            var sections = Resources.Introduction.Split(LineBreak, StringSplitOptions.TrimEntries);
            foreach (var section in sections)
            {
                var lines = section.Split(Environment.NewLine);
                NewBodyText(lines);
            }
        }

        public override void Show()
        {
            Console.Clear();
            base.DisplayHeader();
            base.DisplayLogo(Resources.IntroductionLogo);
            base.DisplayEmptyLine();
            base.DisplayEmptyLine();

            var lines = CurrentPage();
            base.DisplayBodyText(lines);

            base.DisplayEmptyLine();
            base.DisplayFooter();

            base.ScrollInput($"Press {ButtonIcons.ESC} to exit.");
        }
    }
}
