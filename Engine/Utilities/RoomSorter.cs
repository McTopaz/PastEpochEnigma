using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engine.Models;

namespace Engine.Utilities
{
    public class RoomSorter
    {
        internal void SortRoomsOnFloorWithNoNewKeys(Floor floor)
        {
            var rnd = new Random();
            floor.PredeterminedRooms = floor.PredeterminedRooms
                .OrderBy(r => rnd.Next())
                .ToList();

            //floor.PredeterminedRooms = floor.PredeterminedRooms
            //    .OrderBy(c => c.HasLockedDoor)
            //    .ToList();
        }

        internal void SortRoomsOnFloorWithNewKey(Floor floor)
        {
            var roomsWithNewKeys = floor.PredeterminedRooms.Where(r => r.HasDoorKey).ToList();
            
            var roomsWithKeys = floor.PredeterminedRooms.Where(c => c.HasItem);
            var roomsWithLockedDoor = floor.PredeterminedRooms.Where(r => r.HasLockedDoor);
            var otherRooms = floor.PredeterminedRooms.Except(roomsWithKeys).Except(roomsWithLockedDoor);

            var sections = roomsWithKeys
                .SelectMany(k => roomsWithLockedDoor, (k, l) => (k, l))
                .Where(pair => pair.k.Item.Value == pair.l.DoorKey.Value);

            // Keys -> Locks

            // Key -> lock -> Key -> lock

            // KeyA, KeyB, LockA, KeyC, LockB, LockC

        }
    }
}
