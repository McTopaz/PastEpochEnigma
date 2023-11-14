using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Objective
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsFinished { get; set; } = false;

        public override string ToString() => Name;
    }
}
