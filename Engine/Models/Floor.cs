﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engine.Enums;

namespace Engine.Models
{
    public class Floor
    {
        public string Name { get; set; }
        public bool IsStart { get; set; } = false;
        public bool IsOptional { get; set; }
        public Awarness Awarness { get; set; } = new Awarness();
        public List<Room> Rooms { get; set; } = new List<Room>();
        public (int X, int Y) Position { get; set; }
        public DifficultLevel DifficultLevel { get; set; } = DifficultLevel.Easy;
        public Floor? Above { get; set; }
        public Floor? Below { get; set; }
        public List<RoomConfiguration> RoomConfigurations { get; set; }

        public override string ToString()
        {
            var above = Above?.Name ?? "None";
            var below = Below?.Name ?? "None";
            var optional = IsOptional ? $"Optional: { DifficultLevel }" : string.Empty;
            return $"{ Name }, Above: { above }, Below: { below }, { optional }";
        }
    }
}
