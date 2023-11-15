using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Engine.Enums;
using Engine.Models;

namespace Engine
{
    public class FloorFactory
    {
        private const int MaxNumberOfIntermediateRooms = 4;

        public void InitFloors(Mission mission, DifficultLevel gameDifficultLevel)
        {
            mission.CurrentFLoor = mission.Floors.First(f => f.IsStart);
            mission.Floors = mission.Floors.Where(f => IncludeForGameDifficultLevel(f.DifficultLevel, gameDifficultLevel)).ToList();
            mission.Floors.Reverse<Floor>().Aggregate((f1, f2) => f1.Above = f2);
            mission.Floors.Aggregate((f1, f2) => f1.Below = f2);
        }

        private bool IncludeForGameDifficultLevel(DifficultLevel floorDifficultLevel, DifficultLevel gameDifficultLevel)
        {
            if (floorDifficultLevel == DifficultLevel.Easy) return true;
            else if (floorDifficultLevel == DifficultLevel.Medium && (gameDifficultLevel == DifficultLevel.Medium || gameDifficultLevel == DifficultLevel.Hard)) return true;
            else if (floorDifficultLevel == DifficultLevel.Hard && gameDifficultLevel == DifficultLevel.Hard) return true;
            else return false;
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
            floor.PredeterminedRooms = floor.PredeterminedRooms
                .OrderBy(c => c.HasLockedDoor)
                .ToList();
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
