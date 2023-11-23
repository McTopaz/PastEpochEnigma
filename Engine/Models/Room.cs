using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engine.Enums;

namespace Engine.Models
{
    public class Room
    {
        private Item? _doorKey;

        public string Name { get; set; } = string.Empty;
        public bool IsStart { get; set; } = false;
        public bool IsEnd { get; set; } = false;
        public bool IsDark { get; set; } = false;
        public bool IsTerminal { get; set; } = false;
        public bool IsIntermediate { get; set; } = false;
        public bool HasLockedDoor { get; set; } = false;
        public bool HasItem => Item != Item.None;
        public bool HasDoorKey => DoorKey != Item.None;
        public Room? Previous { get; set; }
        public Room? Next { get; set; }
        public Point Position { get; set; } = new Point();
        public Direction Direction { get; set; } = Direction.None;

        public Item Item { get; set; } = Item.None;
        public Item DoorKey { get; set; } = Item.None;

        public override string ToString() => $"{Name} [{Position.X}:{Position.Y}] {Direction}";
    }
}
