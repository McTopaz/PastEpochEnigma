using Engine.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Terminal.Icons;
using Terminal.Models;

namespace Terminal.Screens
{
    internal abstract class ScreenBase
    {
        protected const string OuterMargin = " ";
        protected const string InnerMargin = "    ";
        protected int Width;
        protected int Height;

        public ScreenBase()
        {
            Width = Console.WindowWidth;
            Height = Console.WindowHeight;
        }

        public abstract void Show();
        protected abstract void ExecuteCommand(ConsoleKeyInfo input);

        protected virtual void DisplayCustomLine(params string[] parts)
        {
            var line = string.Join("", parts);
            Console.WriteLine(line);
        }

        protected virtual void DisplayContent(string content)
        {
            var count = Width - LengthOfMargins() - content.Length;
            var filler = new string(' ', count);
            DisplayCustomLine
            (
                OuterMargin,
                BoxIcons.VerticalLine,
                InnerMargin,
                content,
                filler,
                InnerMargin,
                BoxIcons.VerticalLine,
                OuterMargin
            );
        }

        protected virtual void DisplayHeader()
        {
            var count = Width - (OuterMargin.Length * 2) - 2;
            var filler = string.Concat(Enumerable.Repeat(BoxIcons.HorizontalLine, count));

            DisplayCustomLine
            (
                OuterMargin,
                BoxIcons.LeftUpperCorner,
                filler,
                BoxIcons.RightUpperCorner,
                OuterMargin
            );
        }

        protected virtual void DisplayFooter()
        {
            var count = Width - (OuterMargin.Length * 2) - 2;
            var filler = string.Concat(Enumerable.Repeat(BoxIcons.HorizontalLine, count));

            DisplayCustomLine
            (
                OuterMargin,
                BoxIcons.LeftLowerCorner,
                filler,
                BoxIcons.RightLowerCorner,
                OuterMargin
            );
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

            DisplayCustomLine
            (
                OuterMargin,
                BoxIcons.VerticalLine,
                filler,
                BoxIcons.VerticalLine,
                OuterMargin
            );
        }

        protected virtual void DisplayLeftAlignedContent(string content)
        {
            var count = Width - (OuterMargin.Length * 2) - (InnerMargin.Length * 2) - 2 - content.Length;
            var filler = new string(' ', count);

            DisplayCustomLine
            (
                OuterMargin,
                BoxIcons.VerticalLine,
                InnerMargin,
                content,
                filler,
                InnerMargin,
                BoxIcons.VerticalLine,
                OuterMargin
            );
        }

        protected virtual void DisplayAlignedContentAtSides(string leftContent, string rightContent)
        {
            var count = Width - (OuterMargin.Length * 2) - (InnerMargin.Length * 2) - 2 - leftContent.Length - rightContent.Length;
            var filler = new string(' ', count);

            DisplayCustomLine
            (
                OuterMargin,
                BoxIcons.VerticalLine,
                InnerMargin,
                leftContent,
                filler,
                rightContent,
                InnerMargin,
                BoxIcons.VerticalLine,
                OuterMargin
            );
        }

        protected virtual void TakeInput()
        {
            Console.Write($"{OuterMargin}{OuterMargin}Enter input: ");
            var input = Console.ReadKey();
            ExecuteCommand(input);
        }

        protected virtual void DisplayUnknownInput()
        {
            Console.WriteLine();
            Console.WriteLine();
            DisplayInformation("Unknown key input");
            Console.WriteLine();
            Thread.Sleep(1500);
        }

        protected virtual void DisplayInformation(string information)
        {
            Console.WriteLine($"{OuterMargin}{OuterMargin}{information}");
        }

        private int LengthOfMargins()
        {
            return (OuterMargin.Length * 2) + (BoxIcons.VerticalLine.Length * 2) + (InnerMargin.Length * 2);
        }

        protected virtual void DisplayEnum(string text, Enum e, ConsoleKey key)
        {
            var paddedText = text.PadRight(30);
            var options = Enum.GetNames(e.GetType());
            var value = Enum.GetName(e.GetType(), e);
            var content = EnumLineWithSelection(options, value);
            var keySelector = $"({key})";

            var count = Width - LengthOfMargins() - paddedText.Length - content.Length - keySelector.Length;
            var filler = new string(' ', count);

            DisplayCustomLine(
                OuterMargin,
                BoxIcons.VerticalLine,
                InnerMargin,
                paddedText,
                content,
                filler,
                keySelector,
                InnerMargin,
                BoxIcons.VerticalLine,
                OuterMargin
            );
        }

        private string EnumLineWithSelection(string[] items, string value)
        {
            var line = "";

            foreach (var item in items)
            {
                var box = item.Equals(value) ? CheckBoxIcons.CheckedBox : CheckBoxIcons.EmptyBox;
                var section = $"{box} {item}";
                line += section.PadRight(20);
            }

            return line;
        }

        protected virtual void DisplayBodyText(IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                var text = line.Replace("*", GeneralPunctuationIcons.Bullet);
                DisplayContent(text);
            }
        }

        protected virtual void DisplayBulletList(IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                DisplayContent($"{GeneralPunctuationIcons.Bullet} {line}");
            }
        }
    }
}
