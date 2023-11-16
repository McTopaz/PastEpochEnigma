using Engine.Enums;
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
            CreateRoomsForFLoor(floor, difficultLevel);
        }

        private void CreateRoomsForFLoor(Floor floor, DifficultLevel difficultLevel)
        {
            floor.Rooms.Add(new Room { IsStart = true });
            SortPresetRooms(floor);
            AddIntermediateRoomsToPredeterminedRooms(floor);
            floor.Rooms.Add(new Room { IsEnd = true });
            LinkRoomsTogether(floor);
        }

        private void SortPresetRooms(Floor floor)
        {
            if (floor.HasKeyForLockedDoors)
            {
                //SortAccordingToKeyOnFloor(floor);
                _sorter.SortRoomsOnFloorWithNewKey(floor);
            }
            else
            {
                _sorter.SortRoomsOnFloorWithNoNewKeys(floor);
            }
        }

        private void SortAccordingToKeyOnFloor(Floor floor)
        {
            var roomsWithKeys = floor.PredeterminedRooms.Where(c => c.HasItem);
            var roomsWithLockedDoor = floor.PredeterminedRooms.Where(r => r.HasLockedDoor);
            var sections = roomsWithKeys
                .SelectMany(k => roomsWithLockedDoor, (k, l) => (k, l))
                .Where(pair => pair.k.Item.Value == pair.l.DoorKey.Value);

            if (sections.Count() > 0)
            {

            }
            else
            {

            }

            var otherRooms = floor.PredeterminedRooms.Except(roomsWithKeys).Except(roomsWithLockedDoor);
        }

        private void AddIntermediateRoomsToPredeterminedRooms(Floor floor)
        {
            var list = new List<Room>();
            List<Room>? intermediateRooms;

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
            var list = Enumerable.Repeat(new Room { IsIntermediate = true }, count).ToList();
            return list;
        }

        private void LinkRoomsTogether(Floor floor)
        {
            floor.Rooms.Reverse<Room>().Aggregate((r1, r2) => r1.Previous = r2);
            floor.Rooms.Aggregate((r1, r2) => r1.Next = r2);
        }

        private void PrintRooms(List<Room> rooms)
        {
            foreach (var room in rooms)
            {
                if (room.IsStart) Console.WriteLine("Start");
                if (room.IsIntermediate) Console.WriteLine("\tIntermediate");
                if (room.IsDark) Console.WriteLine("Dark");
                if (room.HasItem) Console.WriteLine(room.Item?.ToString());
                if (room.HasLockedDoor) Console.WriteLine("Locked door");
                if (room.IsEnd) Console.WriteLine("End");
            }
        }
    }
}
