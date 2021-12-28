using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using VRage;
using VRage.Game.Entity;

namespace AutoRemoveStone
{
	public static class StoneRemover
	{
		public static void AutoStoneRemoval(bool? enabled = null)
		{
			if (enabled != null)
			{
				StoneRemover.AutoRemoveStone = enabled.Value;
			}
			else
			{
				StoneRemover.AutoRemoveStone = !StoneRemover.AutoRemoveStone;
			}
			if (MySession.Static.ControlledEntity is MyCubeBlock)
			{
				if (StoneRemover.AutoRemoveStone)
				{
					StoneRemover.RemoveStone();
					MySession.Static.AddUpdateCallback(new MyUpdateCallback(new Func<bool>(StoneRemover.Update)));
					return;
				}
				StoneRemover.AutoRemoveStone = false;
			}
		}

		public static void RemoveStone()
		{
			float num = 0f;
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
							if (items[j].Content.GetObjectId().SubtypeName.Contains("Stone"))
							{
								num += (float)items[j].Amount;
								inventory.RemoveItemsAt(inventory.GetItemIndexById(items[j].ItemId), new MyFixedPoint?(items[j].Amount), true, false, null);
							}
						}
					}
				}
				MyAPIGateway.Utilities.ShowNotification(string.Format("Removed {0:N0} stone.", num), 2000, "White");
			}
		}

		public static bool Update()
		{
			if (!StoneRemover.AutoRemoveStone)
			{
				return true;
			}
			if (StoneRemover.Counter < 500)
			{
				StoneRemover.Counter++;
				return false;
			}
			StoneRemover.Counter = 0;
			if (MySession.Static.ControlledEntity is MyCubeBlock)
			{
				StoneRemover.RemoveStone();
				return false;
			}
			StoneRemover.AutoRemoveStone = false;
			return true;
		}

		public static bool AutoRemoveStone { get; private set; }

		public static int Counter { get; private set; }
	}
}