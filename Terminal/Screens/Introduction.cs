using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal.Screens
{
    internal class Introduction : MenuBase
    {
        public override void Show()
        {
            Console.Clear();
            base.DisplayHeader();
            base.DisplayLogo(Resources.IntroductionLogo);
            base.DisplayEmptyLine();
            base.DisplayEmptyLine();

            base.TakeInput();
        }

        protected override void ExecuteCommand(ConsoleKeyInfo input)
        {
            throw new NotImplementedException();
        }
    }
}
