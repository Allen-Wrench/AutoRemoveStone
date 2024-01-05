using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using VRage;
using VRage.Game;
using VRage.Game.Entity;

namespace AutoRemoveStone
{
	public static class StoneRemover
	{
		public static void EnableMode(Mode voidMode = Mode.Off)
		{
			if (!Enabled && voidMode != Mode.Off)
			{
				VoidMode = voidMode;
				Enabled = true;
				MySession.Static.AddUpdateCallback(new MyUpdateCallback(new Func<bool>(Update)));
				return;
			}
			Enabled = false;
			VoidMode = Mode.Off;
		}

		public static bool Update()
		{
			if (!Enabled)
			{
				VoidMode = Mode.Off;
				Counter = 0;
				return true;
			}
			if (Counter > 0)
			{
				Counter--;
				return false;
			}
			Counter = 300;
			if (MySession.Static.ControlledEntity is MyCubeBlock)
			{
				HashSet<MyCubeBlock> inventories = (MySession.Static.ControlledEntity as MyCubeBlock).CubeGrid.Inventories;
				for (int i = 0; i < inventories.Count; i++)
				{
					MyInventory inventory = inventories.ElementAt(i).GetInventory(0);
					List<MyPhysicalInventoryItem> items = inventory.GetItems();
					if (items.Count != 0)
					{
						for (int j = 0; j < items.Count; j++)
						{
							if (VoidMode == Mode.Stone || VoidMode == Mode.Both)
							{
								if (items[j].Content.TypeId.ToString().Contains("Ore") && items[j].Content.SubtypeName.Contains("Stone"))
								{
									inventory.RemoveItemsAt(inventory.GetItemIndexById(items[j].ItemId), new MyFixedPoint?(items[j].Amount), true, false, null);
								}
							}
							if (VoidMode == Mode.Ice || VoidMode == Mode.Both)
							{
								if (items[j].Content.TypeId.ToString().Contains("Ore") && items[j].Content.SubtypeName.Contains("Ice"))
								{
									inventory.RemoveItemsAt(inventory.GetItemIndexById(items[j].ItemId), new MyFixedPoint?(items[j].Amount), true, false, null);
								}
							}
						}
					}
				}
				return false;
			}
			Enabled = false;
			return true;
		}
		public static bool Enabled = false;
		public static Mode VoidMode { get; private set; } = Mode.Off;
		public static int Counter { get; private set; } = 0;
		public enum Mode
        	{
			Off,
			Stone,
			Ice,
			Both
        	};
	}
}
