using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engine.Enums;

namespace Engine.Models
{
    public class Settings
    {
        public DifficultLevel DifficultLevel { get; set; } = DifficultLevel.Easy;
        public Mode Mode { get; set; } = Mode.Normal;
    }
}
