using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

using Engine.Models;

namespace Engine.Utilities
{
    public class RoomSorter
    {
        public void SortRoomsOnFloorWithNewKey(Floor floor, bool shuffle)
        {
            var roomsWithKeys = floor.PredeterminedRooms.Where(c => c.HasItem).ToList();
            var roomsWithLockedDoor = floor.PredeterminedRooms.Where(r => r.HasLockedDoor).ToList();
            var otherRooms = floor.PredeterminedRooms.Except(roomsWithKeys).Except(roomsWithLockedDoor).ToList();
            var shuffling = new Random().Next(0, 2) == 1;

            if (shuffle)
            {
                var shuffledRooms = ShuffleRoomsWithKeysAndLocks(roomsWithKeys, roomsWithLockedDoor);
                floor.PredeterminedRooms = shuffledRooms;
            }
            else
            {
                floor.PredeterminedRooms = roomsWithKeys
                    .Concat(roomsWithLockedDoor)
                    .ToList();
            }

            RandomHelper.InsertListInList(otherRooms, floor.PredeterminedRooms);
        }

        public List<Room> ShuffleRoomsWithKeysAndLocks(List<Room> keys, List<Room> locks)
        {
            if (locks.Count == 0)
            {
                return RandomHelper.ShuffleList(keys);
            }

            // This will group Keys with Locks.
            // {"K1":[L1,L3]},{"K2":[L2,L4,L5]}
            var sections = keys
                .SelectMany(k => locks, (k, l) => (Key: k, Lock: l))
                .Where(pair => pair.Key.Item == pair.Lock.DoorKey)
                .GroupBy(pair => pair.Key, pair => pair.Lock)
                .ToDictionary(group => group.Key, group => group.ToList());

            sections = RandomHelper.ShuffleDictionary(sections);

            // This will make sure locks are placed after the key. Locks position are also randomize.
            // {"K1":[L3,L1]},{"K2":[L5,L2,L4]}
            var rooms = new List<Room>(sections.Keys.ToList());
            foreach (var section in sections)
            {
                var index = rooms.IndexOf(section.Key) + 1;

                foreach (var l in section.Value)
                {
                    var max = rooms.Count() + 1;
                    var pos = new Random().Next(index, max);
                    rooms.Insert(pos, l);
                }
            }

            return rooms;
        }
    }
}
