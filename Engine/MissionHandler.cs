using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Engine
{
    public class MissionHandler
    {
        public Mission LoadMission1FromAsset()
        {
            var json = Resources.Mission1;
            var mission = JsonConvert.DeserializeObject<Mission>(json);
            return mission;
        }
    }
}
