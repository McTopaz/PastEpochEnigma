using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Room
    {
        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }
        public bool IsDark { get; set; }
        public bool HasLockedDoor { get; set; }
        public bool HasItem { get; set; }
    }
}
