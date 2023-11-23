using Engine.Enums;
using Engine.Models;
using Engine.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;

namespace Test
{
    [TestClass]
    public class RoomSorterTest
    {
        private Item Key1 = Item.KeycardOperator;
        private Item Key2 = Item.KeycardManager;
        private Item Key3 = Item.KeycardAdministrator;

        [TestMethod]
        public void NoSuffle_Key_Lock_return_Key_Lock()
        {
            var floor = new Floor
            {
                PredeterminedRooms = new List<Room>
                {
                    new Room { Item = Key1, Name = "K" },
                    new Room { HasLockedDoor = true, DoorKey = Key1, Name = "L" }
                }
            };

            var sorter = new RoomSorter();
            sorter.SortRoomsOnFloorWithNewKey(floor, shuffle: false);
            var rooms = floor.PredeterminedRooms;
            
            Assert.IsTrue(rooms.First().HasDoorKey && rooms.First().Item == Key1);
            Assert.IsTrue(rooms.Last().HasLockedDoor && rooms.Last().DoorKey == Key1);
        }

        [TestMethod]
        public void NoSuffle_Lock_Key_return_Key_Lock()
        {
            var floor = new Floor
            {
                PredeterminedRooms = new List<Room>
                {
                    new Room { HasLockedDoor = true, DoorKey = Key1, Name = "L" },
                    new Room { Item = Key1, Name = "K" }
                }
            };

            var sorter = new RoomSorter();
            sorter.SortRoomsOnFloorWithNewKey(floor, shuffle: false);
            var rooms = floor.PredeterminedRooms;

            Assert.IsTrue(rooms.First().HasDoorKey && rooms.First().Item == Key1);
            Assert.IsTrue(rooms.Last().HasLockedDoor && rooms.Last().DoorKey == Key1);
        }

        [TestMethod]
        public void Shuffle_Key_Lock_returns_KeyBeforeLock()
        {
            var k = new Room { Item = Key1, Name = "K" };
            var l = new Room { HasLockedDoor = true, DoorKey = Key1, Name = "L" };

            var keys = new List<Room> { k };
            var locks = new List<Room> { l };

            var sorter = new RoomSorter();
            var rooms = sorter.ShuffleRoomsWithKeysAndLocks(keys, locks);
            var indices = (k: rooms.IndexOf(k), l: rooms.IndexOf(l));

            Assert.IsTrue(indices.k < indices.l);
        }

        [TestMethod]
        public void Shuffle_K1_K2_L1_L2_L3_L4_L5_returns_KeyBeforeLocks()
        {
            var k1 = new Room { Item = Key1, Name = "K1" };
            var k2 = new Room { Item = Key2, Name = "K2" };
            var l1 = new Room { HasLockedDoor = true, DoorKey = Key1, Name = "L1" };
            var l2 = new Room { HasLockedDoor = true, DoorKey = Key1, Name = "L2" };
            var l3 = new Room { HasLockedDoor = true, DoorKey = Key2, Name = "L3" };
            var l4 = new Room { HasLockedDoor = true, DoorKey = Key2, Name = "L4" };
            var l5 = new Room { HasLockedDoor = true, DoorKey = Key2, Name = "L5" };

            var keys = new List<Room> { k1, k2 };
            var locks = new List<Room> { l1, l2, l3, l4, l5 };

            var sorter = new RoomSorter();
            var rooms = sorter.ShuffleRoomsWithKeysAndLocks(keys, locks);
            var indices = 
            (
                k1: rooms.IndexOf(k1),
                k2: rooms.IndexOf(k2),
                l1: rooms.IndexOf(l1),
                l2: rooms.IndexOf(l2),
                l3: rooms.IndexOf(l3),
                l4: rooms.IndexOf(l4),
                l5: rooms.IndexOf(l5)
            );

            Assert.IsTrue(indices.k1 < indices.l1);
            Assert.IsTrue(indices.k1 < indices.l2);
            Assert.IsTrue(indices.k2 < indices.l3);
            Assert.IsTrue(indices.k2 < indices.l4);
            Assert.IsTrue(indices.k2 < indices.l5);
        }

        [TestMethod]
        public void Shuffle_K1_K2_K3_L1_L2_L3_L4_L5_L6_L7_returns_KeysBeforeLocks()
        {
            var k1 = new Room { Item = Key1, Name = "K1" };
            var k2 = new Room { Item = Key2, Name = "K2" };
            var k3 = new Room { Item = Key3, Name = "K3" };
            var l1 = new Room { HasLockedDoor = true, DoorKey = Key3, Name = "L1" };
            var l2 = new Room { HasLockedDoor = true, DoorKey = Key1, Name = "L2" };
            var l3 = new Room { HasLockedDoor = true, DoorKey = Key2, Name = "L3" };
            var l4 = new Room { HasLockedDoor = true, DoorKey = Key1, Name = "L4" };
            var l5 = new Room { HasLockedDoor = true, DoorKey = Key2, Name = "L5" };
            var l6 = new Room { HasLockedDoor = true, DoorKey = Key2, Name = "L6" };
            var l7 = new Room { HasLockedDoor = true, DoorKey = Key3, Name = "L7" };

            var keys = new List<Room> { k2, k3, k1 };
            var locks = new List<Room> { l2, l7, l1, l6, l4, l5, l3 };

            var sorter = new RoomSorter();
            var rooms = sorter.ShuffleRoomsWithKeysAndLocks(keys, locks);
            var indices =
            (
                k1: rooms.IndexOf(k1),
                k2: rooms.IndexOf(k2),
                k3: rooms.IndexOf(k3),
                l1: rooms.IndexOf(l1),
                l2: rooms.IndexOf(l2),
                l3: rooms.IndexOf(l3),
                l4: rooms.IndexOf(l4),
                l5: rooms.IndexOf(l5),
                l6: rooms.IndexOf(l6),
                l7: rooms.IndexOf(l7)
            );

            Assert.IsTrue(indices.k1 < indices.l2);
            Assert.IsTrue(indices.k1 < indices.l4);
            Assert.IsTrue(indices.k2 < indices.l3);
            Assert.IsTrue(indices.k2 < indices.l5);
            Assert.IsTrue(indices.k2 < indices.l6);
            Assert.IsTrue(indices.k3 < indices.l1);
            Assert.IsTrue(indices.k3 < indices.l7);
        }

        [TestMethod]
        public void Shuffle_Key_NoLock_return()
        {
            var k = new Room { Item = Key1, Name = "K" };

            var keys = new List<Room> { k };
            var locks = new List<Room>();

            var sorter = new RoomSorter();
            var rooms = sorter.ShuffleRoomsWithKeysAndLocks(keys, locks);
            Assert.AreEqual(1, rooms.Count());
        }
    }
}