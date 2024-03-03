using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class MoveResult
    {
        public bool CanMove { get; internal set; } = false;
        public string Reason { get; internal set; } = string.Empty;
    }
}
