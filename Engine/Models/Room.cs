using Engine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Room
    {
        private Item? _item;
        private Item? _doorKey;

        public string Name { get; set; } = string.Empty;
        public bool IsStart { get; set; } = false;
        public bool IsEnd { get; set; } = false;
        public bool IsDark { get; set; } = false;
        public bool IsTerminal { get; set; } = false;
        public bool IsIntermediate { get; set; } = false;
        public bool HasLockedDoor { get; set; } = false;
        public bool HasItem { get; set; } = false;
        public bool HasDoorKey { get; set; } = false;
        public Room? Previous { get; set; }
        public Room? Next { get; set; }
        public (int X, int Y) Location { get; set; } = (0, 0);
        public List<Side> Directions { get; set; } = new List<Side>();
        public Side MainDirection { get; set; }

        public Item? Item
        {
            get => _item;
            set
            {
                _item = value;
                HasItem = _item != null;
            }
        }
        public Item? DoorKey
        {
            get => _doorKey;
            set
            {
                _doorKey = value;
                HasDoorKey = _doorKey != null;
            }
        }

        public override string ToString() => $"{Name} [{Location.X}:{Location.Y}]";
    }
}
