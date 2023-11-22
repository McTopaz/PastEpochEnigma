using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Mission
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Awarness Awarness { get; set; } = new Awarness();
        public List<Objective> Objectives { get; set; }
        public List<string> Notes { get; set; } = new List<string>();
        public List<Floor> Floors { get; set; }
        public Floor CurrentFloor { get; set; }
    }
}
