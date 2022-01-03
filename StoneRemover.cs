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
		public static void AutoStoneRemoval(bool enabled)
		{
			if (enabled == AutoRemoveStone)
				return;
			AutoRemoveStone = enabled;
			if (AutoRemoveStone && !TaskRunning && MySession.Static.ControlledEntity is MyCubeBlock)
			{
				TaskRunning = true;
				MySession.Static.AddUpdateCallback(new MyUpdateCallback(new Func<bool>(Update)));
				return;
			}
		}

		public static bool Update()
		{
			if (!AutoRemoveStone)
			{
				TaskRunning = false;
				return true;
			}
			if (Counter < 500)
			{
				Counter++;
				return false;
			}
			Counter = 0;
			if (MySession.Static.ControlledEntity is MyCubeBlock)
			{
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
								if (items[j].Content.SubtypeName.Contains("Stone"))
								{
									inventory.RemoveItemsAt(inventory.GetItemIndexById(items[j].ItemId), new MyFixedPoint?(items[j].Amount), true, false, null);
								}
							}
						}
					}
				}
				return false;
			}
			AutoRemoveStone = false;
			TaskRunning = false;
			return true;
		}
		public static bool AutoRemoveStone { get; private set; }

		public static bool TaskRunning { get; private set; }

		public static int Counter { get; private set; }
	}
}