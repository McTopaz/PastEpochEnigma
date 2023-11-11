using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Terminal.Screens
{
    internal abstract class MenuBase
    {
        protected const string OuterMargin = " ";
        protected const string InnerMargin = "    ";
        protected int Width;
        protected int Height;

        public MenuBase()
        {
            Width = Console.WindowWidth;
            Height = Console.WindowHeight;
        }

        public abstract void Run();

        protected virtual void DisplayHeader()
        {
            var count = Width - (OuterMargin.Length * 2) - 2;
            var filler = string.Concat(Enumerable.Repeat(BoxParts.HorizontalLine, count));
            Console.WriteLine($"{OuterMargin}{BoxParts.LeftUpperCorner}{filler}{BoxParts.RightUpperCorner}{OuterMargin}");
            DisplayEmptyLine();
        }

        protected virtual void DisplayFooter()
        {
            DisplayEmptyLine();
            var count = Width - (OuterMargin.Length * 2) - 2;
            var filler = string.Concat(Enumerable.Repeat(BoxParts.HorizontalLine, count));
            Console.WriteLine($"{OuterMargin}{BoxParts.LeftLowerCorner}{filler}{BoxParts.RightLowerCorner}{OuterMargin}");
        }


        protected virtual void DisplayLogo(string logo)
        {
            var lines = logo.Split(Environment.NewLine);
            foreach (var part in lines)
            {
                DisplayLeftAlignedContent(part);
            }
        }

        protected virtual void DisplayAuthorAndVersion()
        {
            var author = $"Created by: {Resources.Author}";
            var version = $"V{ApplicationVersion()}";
            DisplayAlignedContentAtSides(author, version);
        }
        private string ApplicationVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var info = FileVersionInfo.GetVersionInfo(assembly.Location);
            return info.FileVersion;
        }

        protected virtual void DisplayEmptyLine()
        {
            var count = Width - (OuterMargin.Length * 2) - 2;
            var filler = new string(' ', count);
            Console.WriteLine($"{OuterMargin}{BoxParts.VerticalLine}{filler}{BoxParts.VerticalLine}{OuterMargin}");
        }

        protected virtual void TakeInput()
        {
            Console.Write($"{OuterMargin}{OuterMargin}Enter input: ");
            var input = Console.ReadKey();
            ExecuteCommand(input);
        }

        protected virtual void DisplayLeftAlignedContent(string content)
        {
            var count = Width - (OuterMargin.Length * 2) - (InnerMargin.Length * 2) - 2 - content.Length;
            var filler = new string(' ', count);
            Console.WriteLine($"{OuterMargin}{BoxParts.VerticalLine}{InnerMargin}{content}{filler}{InnerMargin}{BoxParts.VerticalLine}{OuterMargin}");
        }

        protected virtual void DisplayAlignedContentAtSides(string leftContent, string rightContent)
        {
            var count = Width - (OuterMargin.Length * 2) - (InnerMargin.Length * 2) - 2 - leftContent.Length - rightContent.Length;
            var filler = new string(' ', count);
            Console.WriteLine($"{OuterMargin}{BoxParts.VerticalLine}{InnerMargin}{leftContent}{filler}{rightContent}{InnerMargin}{BoxParts.VerticalLine}{OuterMargin}");
        }

        protected abstract void ExecuteCommand(ConsoleKeyInfo input);
    }
}
