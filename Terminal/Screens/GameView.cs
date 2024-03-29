﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

using Engine.Enums;
using Engine.Models;
using Engine.Utilities;
using Terminal.Icons;

namespace Terminal.Screens
{
    internal class GameView : ScreenBase
    {
        const ConsoleKey UpKey = ConsoleKey.UpArrow;
        const ConsoleKey DownKey = ConsoleKey.DownArrow;
        const ConsoleKey LeftKey = ConsoleKey.LeftArrow;
        const ConsoleKey RightKey = ConsoleKey.RightArrow;

        private const int RoomWidth = 5;
        private const int RoomHeight = 3;
        private const string Doorway = " ";
        private ConsoleColor Selected = ConsoleColor.Blue;
        private ConsoleColor Default = ConsoleColor.White;

        private Floor _floor;
        private ActionController _actionController;

        private List<Room> _rooms = new List<Room>();
        private Point Start = new Point();
        private Point End = new Point();
        private Size Size = new Size();
        private string LeftMargin = string.Empty;
        private (Room Room, bool IsPresent)[][] Grid;

        public override void Show()
        {
            Console.Clear();

            DisplayGameHeader();
            DisplayFloorName();
            DisplayVerticalMargin();
            DisplayRoomGrid();
            DisplayVerticalMargin();
            TakeInput();
        }

        public void Show(Floor floor, ActionController actionController)
        {
            _floor = floor;
            _actionController = actionController;
            _rooms = floor.Rooms;

            CalculateActualFloorSize();
            Show();
        }

        private void ShowInvalidInput(string reason)
        {
            Console.Clear();

            DisplayGameHeader();
            DisplayFloorName();
            DisplayVerticalMargin();
            DisplayRoomGrid();
            DisplayVerticalMargin();
            DisplayInformation(reason);

            Thread.Sleep(1500);
        }

        protected override void ExecuteCommand(ConsoleKeyInfo input)
        {
            var direction = GetInputDirection(input.Key);
            
            if (direction == Direction.None)
            {
                ShowInvalidInput("Unknwn input");
                return;
            }

            var result = _actionController.Move(direction);

            if (!result.CanMove)
            {
                ShowInvalidInput(result.Reason);
            }

            Show();
        }

        private Direction GetInputDirection(ConsoleKey input)
        {
            switch (input)
            {
                case UpKey:
                    return Direction.Up;
                case DownKey:
                    return Direction.Down;
                case LeftKey:
                    return Direction.Left;
                case RightKey:
                    return Direction.Right;
                default:
                    return Direction.None;
            }
        }

        private void CalculateActualFloorSize()
        {
            var minX = _rooms.Min(r => r.Position.X);
            var minY = _rooms.Min(r => r.Position.Y);
            var maxX = _rooms.Max(r => r.Position.X);
            var maxY = _rooms.Max(r => r.Position.Y);

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
            LeftMargin = new string(' ', count);
        }

        private void CalculateGrid()
        {
            Grid = new (Room, bool)[Size.Height][];

            for (int y = 0; y < Size.Height; y++)
            {
                Grid[y] = new (Room, bool)[Size.Width];

                for (int x = 0; x < Size.Width; x++)
                {
                    Grid[y][x].IsPresent = false;
                }
            }
        }

        private void AddRoomPresenceToGrid()
        {
            foreach (var room in _rooms)
            {
                var x = room.Position.X - Start.X;
                var y = room.Position.Y - Start.Y;
                Grid[y][x].IsPresent = true;
                Grid[y][x].Room = room;
            }
        }

        private void DisplayGameHeader()
        {
            var column1Margin = "   ";
            var title = $"{column1Margin}{Engine.Resources.Title}{column1Margin}";
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

        private void DisplayFloorName()
        {
            DisplayCustomLine
            (
                OuterMargin,
                OuterMargin,
                "Floor: ",
                _floor.Name
            );
        }

        private void DisplayVerticalMargin()
        {
            var iterations = 1 + ((_floor.Size.Height - Size.Height) * 2);
            for (int i = 0; i < iterations; i++)
            {
                Console.WriteLine();
            }
        }

        private void DrawLineSelectedOrDefault(string line, bool drawSelected)
        {
            if (drawSelected)
            {
                Console.ForegroundColor = Selected;
                Console.Write(line);
                Console.ForegroundColor = Default;
            }
            else
            {
                Console.Write(line);
            }
        }

        private bool HasNeighbourCell(int y, int x)
        {
            return x >= 0 && x < Grid[y].Length && Grid[y][x].IsPresent;
        }

        private bool IsPlayerAtPosition(int y, int x)
        {
            return (_floor.Position.Y - Start.Y) == y && (_floor.Position.X - Start.X) == x;
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

        #region Top line

        private void DrawTopLine()
        {
            Console.Write(LeftMargin);

            for(int x = 0; x < Size.Width; x++)
            {
                var cell = Grid[0][x];
                if (cell.IsPresent)
                {
                    DrawCellTop(x);
                }
                else
                {
                    DrawNoCellTop(x);
                }
            }

            DrawTopEnd();
            Console.WriteLine();
        }

        private void DrawCellTop(int x)
        {
            var hasLeftNeighbour = HasNeighbourCell(0, x - 1);
            var isSelectedLeft = IsPlayerAtPosition(0, x) || IsPlayerAtPosition(0, x - 1);
            var isSelectedLine = IsPlayerAtPosition(0, x);
            var left = hasLeftNeighbour ? BoxIcons.TopTCrossing : BoxIcons.LeftUpperCorner;
            var line = string.Concat(Enumerable.Repeat(BoxIcons.HorizontalLine, RoomWidth));
            DrawLineSelectedOrDefault(left, isSelectedLeft);
            DrawLineSelectedOrDefault(line, isSelectedLine);
        }

        private void DrawNoCellTop(int x)
        {
            var hasLeftNeighbour = HasNeighbourCell(0, x - 1);
            var isSelected = IsPlayerAtPosition(0, x - 1);
            var left = hasLeftNeighbour ? BoxIcons.RightUpperCorner: " ";
            var line = new string(' ', RoomWidth);
            DrawLineSelectedOrDefault(left + line, isSelected);
        }

        private void DrawTopEnd()
        {
            var end = Grid[0][Size.Width - 1].IsPresent ? BoxIcons.RightUpperCorner : " ";
            var isSelected = IsPlayerAtPosition(0, Size.Width - 1);
            DrawLineSelectedOrDefault(end, isSelected);
        }

        #endregion

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
            var isSelected = false;
            Console.Write(LeftMargin);

            for (int x = 0; x < Size.Width; x++)
            {
                var hasLeftNeighbour = HasNeighbourCell(y, x - 1);
                var isPresent = Grid[y][x].IsPresent;
                var left = !isPresent && !hasLeftNeighbour ? " " : BoxIcons.VerticalLine;
                var line = new string(' ', RoomWidth);
                isSelected = IsPlayerAtPosition(y, x) || IsPlayerAtPosition(y, x - 1);
                DrawLineSelectedOrDefault(left + line, isSelected);
            }

            var end = Grid[y][Size.Width - 1].IsPresent ? BoxIcons.VerticalLine : " ";
            isSelected = IsPlayerAtPosition(y, Size.Width - 1);
            DrawLineSelectedOrDefault(end, isSelected);
            Console.WriteLine();
        }

        private void DrawRoomItemLine(int y)
        {
            var isSelected = false;
            Console.Write(LeftMargin);

            for (int x = 0; x < Size.Width; x++)
            {
                var cell = Grid[y][x];
                var hasLeftNeighbour = HasNeighbourCell(y, x - 1);
                var isPresent = cell.IsPresent;
                var left = BoxIcons.VerticalLine;

                if (!isPresent && !hasLeftNeighbour) left = Doorway;
                else if (isPresent && hasLeftNeighbour)
                {
                    if (cell.Room.OutOf == Direction.Left) left = Doorway;
                    else if (cell.Room.Into == Direction.Left) left = Doorway;
                }
                var line = GetContentLine(cell.Room);
                isSelected = IsPlayerAtPosition(y, x) || IsPlayerAtPosition(y, x - 1);
                DrawLineSelectedOrDefault(left + line, isSelected);
            }

            var end = Grid[y][Size.Width - 1].IsPresent ? BoxIcons.VerticalLine : " ";
            isSelected = IsPlayerAtPosition(y, Size.Width - 1);
            DrawLineSelectedOrDefault(end, isSelected);
            Console.WriteLine();
        }

        private string GetContentLine(Room? room)
        {
            if (room == null) return new string(' ', RoomWidth);

            var content = GetRoomContent(room);
            if (content.Length == 1) return $"  {content}  ";
            if (content.Length == 2) return $"  {content} ";
            if (content.Length == 3) return $" {content} ";
            if (content.Length == 4) return $" {content}";
            if (content.Length == 5) return $"{content}";
            else return new string(' ', RoomWidth);
        }

        private string GetRoomContent(Room room)
        {
            var content = "";
            if (room.HasItem)
            {
                if (room.Item == Item.KeycardOperator) content = "\U0001F511";
            }
            else if (room.IsStart) content = RoomIcons.Start;
            else if (room.IsEnd) content = RoomIcons.Up;
            else if (room.IsDark) content = RoomIcons.QuestionMark;
            else content = "";

            return content;
        }

        private void DrawDividerLine(int y)
        {
            Console.Write(LeftMargin);
            var dashes = string.Concat(Enumerable.Repeat(BoxIcons.HorizontalLine, RoomWidth));
            var empty = new string(' ', RoomWidth);

            for (int x = 0; x < Size.Width; x++)
            {
                var hasLeftNeighbourAbove = HasNeighbourCell(y, x - 1);
                var hasLeftNeighbourBelow = HasNeighbourCell(y + 1, x - 1);
                var above = Grid[y][x].IsPresent;
                var below = Grid[y + 1][x].IsPresent;
                var left = "";
                var line = "";

                if (hasLeftNeighbourAbove && hasLeftNeighbourBelow)
                {
                    if (!above && !below) left = BoxIcons.RightTCrossing;
                    else left = BoxIcons.Cross;
                    line = !above && !below ? empty : dashes;
                }
                else if (hasLeftNeighbourAbove && !hasLeftNeighbourBelow)
                {
                    if (!above && !below) left = BoxIcons.RightLowerCorner;
                    else if (above && !below) left = BoxIcons.BottomTCrossing;
                    else left = BoxIcons.Cross;
                    line = !above && !below ? empty : dashes;
                }
                else if (!hasLeftNeighbourAbove && hasLeftNeighbourBelow)
                {
                    if (!above && !below) left = BoxIcons.RightUpperCorner;
                    else if (!above && below) left = BoxIcons.TopTCrossing;
                    else left = BoxIcons.Cross;
                    line = !above && !below ? empty : dashes;
                }
                else
                {
                    if (!above && !below) left = Doorway;
                    else if (!above && below) left = BoxIcons.LeftUpperCorner;
                    else if (above && !below) left = BoxIcons.LeftLowerCorner;
                    else if (above && below) left = BoxIcons.LeftTCrossing;
                    line = above || below ? dashes : empty;
                }

                var cell = Grid[y][x];

                if (cell.Room?.Into == Direction.Down || cell.Room?.OutOf == Direction.Down)
                {
                    var index = dashes.Length / 2;
                    var sb = new StringBuilder(line);
                    sb[index - 1] = ' ';
                    sb[index] = ' ';
                    sb[index + 1] = ' ';
                    line = sb.ToString();
                }

                //var yx = IsPlayerAtPosition(y, x);
                //var y1x = IsPlayerAtPosition(y + 1, x);
                //var yx1 = IsPlayerAtPosition(y, x - 1);
                //var y1x1 = IsPlayerAtPosition(y, x);
                var isSelectedLeft = (IsPlayerAtPosition(y, x) || IsPlayerAtPosition(y + 1, x) || IsPlayerAtPosition(y, x - 1) || IsPlayerAtPosition(y + 1, x - 1));// && (above || below);
                var isSelectedLine = (IsPlayerAtPosition(y, x) || IsPlayerAtPosition(y + 1, x)) && (above || below);
                DrawLineSelectedOrDefault(left, isSelectedLeft);
                DrawLineSelectedOrDefault(line, isSelectedLine);
            }

            var end = "";
            var endAbove = Grid[y][Size.Width - 1].IsPresent;
            var endBelow = Grid[y + 1][Size.Width - 1].IsPresent;

            if (!endAbove && endBelow) end = BoxIcons.RightUpperCorner;
            else if (endAbove && !endBelow) end = BoxIcons.RightLowerCorner;
            else if (endAbove && endBelow) end = BoxIcons.RightTCrossing;

            var isSelected = (IsPlayerAtPosition(y, Size.Width - 1) || IsPlayerAtPosition(y + 1, Size.Width - 1)) && (endAbove || endBelow);
            DrawLineSelectedOrDefault(end, isSelected);
            Console.WriteLine();
        }

        #region Bottom line

        private void DrawBottomLine()
        {
            Console.Write(LeftMargin);

            var y = Size.Height - 1;
            for (int x = 0; x < Size.Width; x++)
            {
                var cell = Grid[y][x];

                if (cell.IsPresent)
                {
                    DrawCellBottom(x);
                }
                else
                {
                    DrawNoCellBottom(x);
                }
            }

            DrawBottomEnd();
            Console.WriteLine();
        }

        private void DrawCellBottom(int x)
        {
            var y = Size.Height - 1;
            var hasLeftNeighbour = HasNeighbourCell(y, x - 1);
            var isSelectedLeft = IsPlayerAtPosition(y, x) || IsPlayerAtPosition(y, x - 1);
            var isSelectedLine = IsPlayerAtPosition(y, x);
            var left = hasLeftNeighbour ? BoxIcons.BottomTCrossing : BoxIcons.LeftLowerCorner;
            var line = string.Concat(Enumerable.Repeat(BoxIcons.HorizontalLine, RoomWidth));
            DrawLineSelectedOrDefault(left, isSelectedLeft);
            DrawLineSelectedOrDefault(line, isSelectedLine);
        }

        private void DrawNoCellBottom(int x)
        {
            var y = Size.Height - 1;
            var hasLeftNeighbour = HasNeighbourCell(y, x - 1);
            var isSelected = IsPlayerAtPosition(y, x - 1);
            var left = hasLeftNeighbour ? BoxIcons.RightLowerCorner : " ";
            var line = new string(' ', RoomWidth);
            DrawLineSelectedOrDefault(left + line, isSelected);
        }

        private void DrawBottomEnd()
        {
            var y = Size.Height - 1;
            var end = Grid[y][Size.Width - 1].IsPresent ? BoxIcons.RightLowerCorner : " ";
            var isSelected = IsPlayerAtPosition(y, Size.Width - 1);
            DrawLineSelectedOrDefault(end, isSelected);
        }

        #endregion
    }
}
