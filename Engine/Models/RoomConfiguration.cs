using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engine.Enums;

namespace Engine.Models
{
    public class RoomConfiguration
    {
        public bool IsDark { get; set; } = false;
        public bool HasLockedDoor { get; set; } = false;
        public bool IsTerminal { get; set; } = false;
        public Item? Item { get; set; }
        public Item? DoorKey { get; set; }

    }
}
