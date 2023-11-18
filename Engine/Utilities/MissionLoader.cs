using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Engine.Utilities
{
    public class MissionLoader
    {
        List<Mission> _missions { get; set; } = new List<Mission>();

        public void LoadMissionsFromAssets()
        {
            _missions.Add(LoadFirstMission());
        }

        private Mission LoadFirstMission()
        {
            var json = Resources.InfiltrationHeadquarters;
            var mission = JsonConvert.DeserializeObject<Mission>(json);
            return mission;
        }

        public List<Mission> RandomizedMissions()
        {
            var list = new List<Mission>
            {
                _missions.First()
            };

            var shuffled = RandomHelper.ShuffleList(_missions.Skip(1).ToList());
            list.AddRange(shuffled);
            return list;
        }
    }
}
