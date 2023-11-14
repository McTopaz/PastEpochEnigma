using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Floor
    {
        public string Name { get; set; }
        public bool IsStart { get; set; } = false;
        public Awarness Awarness { get; set; } = new Awarness();
        public List<Room> Rooms { get; set; }
        public (int X, int Y) Position { get; set; }
        public bool IsOptional { get; set; }
        public DifficultLevel DifficultLevel { get; set; } = DifficultLevel.Easy;

        public override string ToString() =>  IsOptional ? $"{ Name } - Optional: { DifficultLevel }" : Name;
    }
}
