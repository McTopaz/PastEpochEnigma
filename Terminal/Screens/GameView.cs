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
        private const int RoomWidth = 5;
        private const int RoomHeight = 3;

        private Floor _floor;
        private Dictionary<Point, Room> _rooms = new Dictionary<Point, Room>();
        private Point Start = new Point();
        private Point End = new Point();
        private Size Size = new Size();
        private string LeftMargin;
        private (Point position, bool presence)[][] Grid;

        public override void Show()
        {
            Console.Clear();

            DisplayGameHeader();
            //DisplayRooms(_rooms);
            DisplayRoomGrid();
            Console.WriteLine();
            TakeInput();
        }

        public void Show(Floor floor, List<Room> rooms)
        {
            _floor = floor;
            _rooms = rooms.ToDictionary(k => k.Position, v => v);
            CalculateActualFloorSize();
            Show();
        }

        protected override void ExecuteCommand(ConsoleKeyInfo input)
        {
            
        }

        private void CalculateActualFloorSize()
        {
            var minX = _rooms.Values.Min(r => r.Position.X);
            var minY = _rooms.Values.Min(r => r.Position.Y);
            var maxX = _rooms.Values.Max(r => r.Position.X);
            var maxY = _rooms.Values.Max(r => r.Position.Y);

            Start = new Point(minX, minY);
            End = new Point(maxX, maxY);
            Size = new Size((maxX - minX) + 1, (maxY - minY) + 1);

            CalculateLeftMargin();
            CalculateGrid();
            AddRoomPresenceToGrid();
        }

        private void CalculateLeftMargin()
        {
            var numberOfRooms = Size.Width;
            var totalRoomsWidth = numberOfRooms * RoomWidth;
            var numberOfRoomDividers = numberOfRooms - 1;
            var ends = 2;
            var count = (Width - totalRoomsWidth - numberOfRoomDividers - ends) / 2;
            LeftMargin = new string('*', count);
        }

        private void CalculateGrid()
        {
            Grid = new (Point, bool)[Size.Height][];

            for (int y = 0; y < Size.Height; y++)
            {
                Grid[y] = new (Point, bool)[Size.Width];

                for (int x = 0; x < Size.Width; x++)
                {
                    Grid[y][x].position = new Point(x, y);
                    Grid[y][x].presence = false;
                }
            }
        }

        private void AddRoomPresenceToGrid()
        {
            foreach (var room in _rooms.Values)
            {
                var x = room.Position.X - Start.X;
                var y = room.Position.Y - Start.Y;
                Grid[y][x].presence = true;
            }
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

        private void DisplayRoomGrid()
        {
            DrawTopLine();

            for (int y = 0; y < Size.Height; y++)
            {
                DrawRoomsForRow(y);
            }

            DrawBottomLine();
        }

        private void DrawTopLine()
        {
            Console.Write(LeftMargin);

            for(int x = 0; x < Size.Width; x++)
            {
                var cell = Grid[0][x];

                if (cell.presence)
                {
                    DrawCellTop(x);
                }
                else
                {
                    DrawNoCellTop(x);
                }
            }

            DrawTopEnd();
        }

        private bool HasNeighbourCell(int y, int x)
        {
            return x >= 0 && x < Grid[y].Length && Grid[y][x].presence;
        }


        private void DrawCellTop(int x)
        {
            var hasLeftNeighbour = HasNeighbourCell(0, x - 1);
            var left = hasLeftNeighbour ? BoxIcons.TopTCrossing : BoxIcons.LeftUpperCorner;
            var line = string.Concat(Enumerable.Repeat(BoxIcons.HorizontalLine, RoomWidth));
            Console.Write(left + line);
        }

        private void DrawNoCellTop(int x)
        {
            var hasLeftNeighbour = HasNeighbourCell(0, x - 1);
            var left = hasLeftNeighbour ? BoxIcons.RightUpperCorner: " ";
            var line = new string(' ', RoomWidth);
            Console.Write(left + line);
        }

        private void DrawTopEnd()
        {
            var end = Grid[0][Size.Width - 1].presence ? BoxIcons.RightUpperCorner : " ";
            Console.WriteLine(end);
        }

        private void DrawRoomsForRow(int y)
        {
            DrawRoomEmptyLine(y);
            DrawRoomItemLine(y);
            DrawRoomEmptyLine(y);

            // Divier.
            if (y >= 0 && y < Size.Height - 1)
            {
                DrawDividerLine(y);
            }
        }

        private void DrawRoomEmptyLine(int y)
        {
            Console.Write(LeftMargin);

            for (int x = 0; x < Size.Width; x++)
            {
                var hasLeftNeighbour = HasNeighbourCell(y, x - 1);
                var isPresent = Grid[y][x].presence;
                var left = !isPresent && !hasLeftNeighbour ? " " : BoxIcons.VerticalLine;
                var line = new string(' ', RoomWidth);
                Console.Write(left + line);
            }

            var end = Grid[y][Size.Width - 1].presence ? BoxIcons.VerticalLine : " ";
            Console.WriteLine(end);
        }

        private void DrawRoomItemLine(int y)
        {
            Console.Write(LeftMargin);

            for (int x = 0; x < Size.Width; x++)
            {
                var hasLeftNeighbour = HasNeighbourCell(y, x - 1);
                var isPresent = Grid[y][x].presence;
                var left = !isPresent && !hasLeftNeighbour ? " " : BoxIcons.VerticalLine;
                var item = isPresent ? "r" : "n";
                var count = (RoomWidth - item.Length) / 2;
                var margin = new string(' ', count);
                var line = $"{margin}{item}{margin}";
                line.PadRight(RoomWidth);
                Console.Write(left + line);
            }

            var end = Grid[y][Size.Width - 1].presence ? BoxIcons.VerticalLine : " ";
            Console.WriteLine(end);
        }

        private void DrawDividerLine(int y)
        {
            Console.WriteLine(LeftMargin + "Divider");
        }

        private void DrawBottomLine()
        {
            Console.Write(LeftMargin);

            var y = Size.Height - 1;
            for (int x = 0; x < Size.Width; x++)
            {
                var cell = Grid[y][x];

                if (cell.presence)
                {
                    DrawCellBottom(x);
                }
                else
                {
                    DrawNoCellBottom(x);
                }
            }

            DrawBottomEnd();
        }

        private void DrawCellBottom(int x)
        {
            var y = Size.Height - 1;
            var hasLeftNeighbour = HasNeighbourCell(y, x - 1);
            var left = hasLeftNeighbour ? BoxIcons.BottomTCrossing : BoxIcons.LeftLowerCorner;
            var line = string.Concat(Enumerable.Repeat(BoxIcons.HorizontalLine, RoomWidth));
            Console.Write(left + line);
        }

        private void DrawNoCellBottom(int x)
        {
            var y = Size.Height - 1;
            var hasLeftNeighbour = HasNeighbourCell(y, x - 1);
            var left = hasLeftNeighbour ? BoxIcons.RightLowerCorner : " ";
            var line = new string(' ', RoomWidth);
            Console.Write(left + line);
        }

        private void DrawBottomEnd()
        {
            var y = Size.Height - 1;
            var end = Grid[y][Size.Width - 1].presence ? BoxIcons.RightLowerCorner : " ";
            Console.WriteLine(end);
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
            if (room.IsStart) icon = RoomIcons.GoalFlag;
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
