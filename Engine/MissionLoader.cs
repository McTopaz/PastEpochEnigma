using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Engine
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

            var rnd = new Random();
            var shuffled = _missions.Skip(1).OrderBy(m => rnd.Next());
            list.AddRange(shuffled);
            return list;
        }
    }
}
