using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Terminal.Screens
{
    internal class Splash
    {
        public void Run()
        {
            var random = new Random().Next(2);
            if (random == 0) SplashHorizontal();
            else if (random == 1) SplashVertical();
        }

        private void SplashHorizontal()
        {
            var width = Console.WindowWidth;
            var height = Console.WindowHeight;
            var logo = Resources.SplashLogoHorizontal.Split(Environment.NewLine);
            var verticalMargin = (height - logo.Length) / 2;

            DisplayNumberOfLines(verticalMargin, width);

            var logoLongestRow = logo.Aggregate("", (max, current) => max.Length > current.Length ? max : current);
            var logoHorizontalMargin = (width - (logoLongestRow.Length + 2)) / 2;

            foreach (var part in logo)
            {
                var horizontalMargin = new string('*', logoHorizontalMargin);
                var line = $"{horizontalMargin} {part} {horizontalMargin}";
                line = line.Length == width ? line : line + "*";
                Console.WriteLine(line);
            }

            DisplayNumberOfLines(verticalMargin - 1, width);
            DisplayFooterRow(width);
            DelaySplashAndClear();
        }

        private void SplashVertical()
        {
            var width = Console.WindowWidth;
            var height = Console.WindowHeight;
            var logo = Resources.SplashLogoVertical.Split(Environment.NewLine);
            var verticalMargin = (height - logo.Length) / 2;

            DisplayNumberOfLines(verticalMargin, width);

            var logoLongestRow = logo.Aggregate("", (max, current) => max.Length > current.Length ? max : current);
            var leftMarginCount = (width - (logoLongestRow.Length + 2)) / 2;
            var leftMargin = new string('*', leftMarginCount);

            foreach (var part in logo)
            {
                var line = $"{leftMargin} {part} ";
                var rightMarginCount = width - line.Length;
                var rightMargin = new string('*', rightMarginCount);
                line = $"{line}{rightMargin}";
                Console.WriteLine(line);
            }

            DisplayNumberOfLines(verticalMargin - 1, width);
            DisplayFooterRow(width);
            DelaySplashAndClear();
        }

        private void DisplayNumberOfLines(int rows, int width)
        {
            for (int i = 0; i < rows; i++)
            {
                var line = new string('*', width);
                Console.WriteLine(line);
            }
        }

        private void DisplayFooterRow(int width)
        {
            var author = $" Created by: {Resources.Author} ";
            var version = $" V{ApplicationVersion()} ";
            var margin = new string('*', 4);
            var fillerCount = width - author.Length - version.Length - (margin.Length * 2);
            var filler = new string('*', fillerCount);
            Console.Write($"{margin}{author}{filler}{version}{margin}");
        }

        private string ApplicationVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var info = FileVersionInfo.GetVersionInfo(assembly.Location);
            return info.FileVersion;
        }

        private void DelaySplashAndClear()
        {
            Thread.Sleep(1000);
            Console.Clear();
        }
    }
}
