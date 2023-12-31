﻿using Engine.Enums;
using Engine.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utilities
{
    public class RoomGenerator
    {
        private const int MaxNumberOfIntermediateRooms = 4;

        private RoomSorter _sorter;

        public RoomGenerator(RoomSorter sorter)
        {
            _sorter = sorter;
        }

        public void CreateRoomsForMissionFloors(Mission mission, DifficultLevel difficultLevel)
        {
            var floor = mission.Floors.First(f => f.IsStart);
            CreateRoomsForFloor(floor, difficultLevel);
        }

        private void CreateRoomsForFloor(Floor floor, DifficultLevel difficultLevel)
        {
            floor.Rooms.Add(new Room { IsStart = true, Name = "Start", IsTerminal = true }) ;
            SortPredeterminedRooms(floor);
            AddIntermediateRoomsToPredeterminedRooms(floor);
            floor.Rooms.Add(new Room { IsEnd = true, Name = "End", IsTerminal = true });
            LinkRoomsTogether(floor);
        }

        private void SortPredeterminedRooms(Floor floor)
        {
            if (floor.HasKeyForLockedDoors)
            {
                var shuffle = new Random().Next(0, 2) == 1;
                _sorter.SortRoomsOnFloorWithNewKey(floor, shuffle);
            }
            else
            {
                floor.PredeterminedRooms = RandomHelper.ShuffleList(floor.PredeterminedRooms);
            }
        }

        private void AddIntermediateRoomsToPredeterminedRooms(Floor floor)
        {
            foreach (var room in floor.PredeterminedRooms)
            {
                floor.Rooms.AddRange(GenerateIntermediateRooms());
                floor.Rooms.Add(room);
            }

            floor.Rooms.AddRange(GenerateIntermediateRooms());
        }

        private List<Room> GenerateIntermediateRooms()
        {
            var rnd = new Random();
            var count = rnd.Next(0, MaxNumberOfIntermediateRooms);
            var list = Enumerable.Range(0, count).Select(_ => new Room { IsIntermediate = true, Name = "Intermediate" }).ToList();
            return list;
        }

        private void LinkRoomsTogether(Floor floor)
        {
            floor.Rooms.Reverse<Room>().Aggregate((r1, r2) => r1.Previous = r2);
            floor.Rooms.Aggregate((r1, r2) => r1.Next = r2);
        }
    }
}
