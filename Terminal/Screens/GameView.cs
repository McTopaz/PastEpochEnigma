using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Engine.Enums;
using Engine.Models;
using Terminal.Icons;

namespace Terminal.Screens
{
    internal class GameView : ScreenBase
    {
        private List<Room> _rooms = new List<Room>();

        public override void Show()
        {
            Console.Clear();

            DisplayGameHeader();
            DisplayRooms(_rooms);
            Console.WriteLine();
            TakeInput();
        }

        public void Show(List<Room> rooms)
        {
            _rooms = rooms;
            Show();
        }

        protected override void ExecuteCommand(ConsoleKeyInfo input)
        {
            
        }

        private void DisplayGameHeader()
        {
            var column1Margin = "   ";
            var title = $"{column1Margin}Past Epoch Enigma{column1Margin}";
            var version = ApplicationVersion();

            var column1Count = title.Length;
            var column1Filler = string.Concat(Enumerable.Repeat(BoxIcons.HorizontalLine, title.Length));

            var column2Count = Width - (OuterMargin.Length * 2) - 3 - title.Length;
            var column2Filler = string.Concat(Enumerable.Repeat(BoxIcons.HorizontalLine, column2Count));


            // Header.
            DisplayCustomLine
            (
                OuterMargin,
                BoxIcons.LeftUpperCorner,
                column1Filler,
                BoxIcons.TopTCrossing,
                column2Filler,
                BoxIcons.RightUpperCorner,
                OuterMargin
            );

            var column1Spaces = new string(' ', column1Count);
            var column2Spaces = new string(' ', column2Count);

            // Empty row.
            DisplayCustomLine
            (
                OuterMargin,
                BoxIcons.VerticalLine,
                column1Spaces,
                BoxIcons.VerticalLine,
                column2Spaces,
                BoxIcons.VerticalLine,
                OuterMargin
            );

            // Title.
            DisplayCustomLine
            (
                OuterMargin,
                BoxIcons.VerticalLine,
                title,
                BoxIcons.VerticalLine,
                column2Spaces,
                BoxIcons.VerticalLine,
                OuterMargin
            );

            var versionSpacingCount = column1Count - (version.Length + column1Margin.Length + 1);
            var versionSpacing = new string(' ', versionSpacingCount);
            var versionLine = $"{versionSpacing}V{version}{column1Margin}";

            // Version.
            DisplayCustomLine
            (
                OuterMargin,
                BoxIcons.VerticalLine,
                versionLine,
                BoxIcons.VerticalLine,
                column2Spaces,
                BoxIcons.VerticalLine,
                OuterMargin
            );

            // Footer.
            DisplayCustomLine
            (
                OuterMargin,
                BoxIcons.LeftLowerCorner,
                column1Filler,
                BoxIcons.BottomTCrossing,
                column2Filler,
                BoxIcons.RightLowerCorner,
                OuterMargin
            );
        }

        private void DisplayRooms(List<Room> rooms)
        {
            var cellLine = string.Concat(Enumerable.Repeat(BoxIcons.HorizontalLine, 5));
            var topLine = string.Join(BoxIcons.TopTCrossing, Enumerable.Repeat(cellLine, 16));
            var bottomLine = string.Join(BoxIcons.BottomTCrossing, Enumerable.Repeat(cellLine, 16));

            // Top line.
            DisplayCustomLine
            (
                OuterMargin,
                BoxIcons.LeftUpperCorner,
                topLine,
                BoxIcons.RightUpperCorner,
                OuterMargin
            );

            var cellSpace = cellLine.Replace(BoxIcons.HorizontalLine, " ");
            var emptyCellsLine = string.Join(BoxIcons.VerticalLine, Enumerable.Repeat(cellSpace, 16));
            var itemCellsLine = string.Join("#", Enumerable.Repeat(cellSpace, 16));
            var lineDivider = topLine.Replace(BoxIcons.TopTCrossing, BoxIcons.Cross);

            for (int i = 0; i < 5; i++)
            {
                // Empty top line.
                DisplayCustomLine
                (
                    OuterMargin,
                    BoxIcons.VerticalLine,
                    emptyCellsLine,
                    BoxIcons.VerticalLine,
                    OuterMargin
                );

                var items = GetItemsForLineNumber(i, rooms);
                var cellItems = items.Select(s => $"  {s} ");
                var itemLine = string.Join(BoxIcons.VerticalLine, cellItems);

                // Item line.
                DisplayCustomLine
                (
                    OuterMargin,
                    BoxIcons.VerticalLine,
                    itemLine,
                    BoxIcons.VerticalLine,
                    OuterMargin
                );

                // Empty bottom line.
                DisplayCustomLine
                (
                    OuterMargin,
                    BoxIcons.VerticalLine,
                    emptyCellsLine,
                    BoxIcons.VerticalLine,
                    OuterMargin
                );

                if (i < 4)
                {
                    // Lower cell line
                    DisplayCustomLine
                    (
                        OuterMargin,
                        BoxIcons.LeftTCrossing,
                        lineDivider,
                        BoxIcons.RightTCrossing,
                        OuterMargin
                    );
                }
            }

            // Bottom line.
            DisplayCustomLine
            (
                OuterMargin,
                BoxIcons.LeftLowerCorner,
                bottomLine,
                BoxIcons.RightLowerCorner,
                OuterMargin
            );
        }

        private List<string> GetItemsForLineNumber(int lineNumber, List<Room> rooms)
        {
            var roomsOnLine = rooms.Where(r => r.Position.Y == lineNumber);
            var list = new List<string>();
            for (int i = 0; i < 16; i++)
            {
                var room = roomsOnLine.FirstOrDefault(r => r.Position.X == i, new Room());
                var icon = "  ";

                if (room.IsStart || room.IsEnd)
                {
                    icon = GetIconForFlaggedRoom(room);
                }
                else if (room.HasItem)
                {
                    icon = GetIconForItem(room.Item);
                }

                list.Add(icon);
            }
            return list;
        }

        private string GetIconForFlaggedRoom(Room room)
        {
            var icon = string.Empty;
            if (room.IsStart) icon = "\U0001F6AA";
            else icon = ArrowsIcons.AngledUpRight;

            icon = icon.Length == 2 ? icon : icon.PadRight(2);
            return icon;
        }

        private string GetIconForItem(Item item)
        {
            var icon = string.Empty;
            if (item == Item.KeycardOperator) return "\U0001F511";

            icon = icon.Length == 2 ? icon : icon.PadRight(1);
            return icon;
        }
    }
}
