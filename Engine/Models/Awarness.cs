using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Awarness
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsOver { get; set; } = false;
        public bool IsFinished { get; set; } = false;


        public void CalculateDuration()
        {
            Duration = End - Start;
        }
    }
}
