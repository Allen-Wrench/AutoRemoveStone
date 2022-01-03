using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using HarmonyLib;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using VRage.Utils;

namespace AutoRemoveStone
{
	[HarmonyPatch]
	public class TerminalPatches
	{
		public static void AutoRemoveStone(MyShipDrill drill)
		{
			StoneRemover.AutoStoneRemoval(!StoneRemover.AutoRemoveStone);
		}

		public static void ShipDrillControls()
		{
			MyTerminalControlOnOffSwitch<MyShipDrill> myTerminalControlOnOffSwitch = new MyTerminalControlOnOffSwitch<MyShipDrill>("AutoStoneRemove", MyStringId.GetOrCompute("Auto Stone Removal"), MyStringId.GetOrCompute("Automatically remove stone while drilling"), null, null, float.PositiveInfinity, false, false);
			myTerminalControlOnOffSwitch.Getter = (MyShipDrill x) => StoneRemover.AutoRemoveStone;
			myTerminalControlOnOffSwitch.Setter = delegate (MyShipDrill x, bool v)
			{
				StoneRemover.AutoStoneRemoval(v);
			};
			myTerminalControlOnOffSwitch.SupportsMultipleBlocks = false;
			MyTerminalControlFactory.AddControl<MyShipDrill>(myTerminalControlOnOffSwitch);
			MyTerminalAction<MyShipDrill> myTerminalAction = new MyTerminalAction<MyShipDrill>("AutoStoneRemove", new StringBuilder("Auto Remove Stone"), "");
			myTerminalAction.Action = new Action<MyShipDrill>(AutoRemoveStone);
			myTerminalAction.Writer = delegate (MyShipDrill block, StringBuilder builder)
			{
				if (StoneRemover.AutoRemoveStone)
				{
					builder.Append("No Stone");
					return;
				}
				builder.Append("Keep Stone");
			};
			myTerminalAction.ValidForGroups = false;
			MyTerminalControlFactory.AddAction<MyShipDrill>(myTerminalAction);
		}

		[HarmonyTranspiler]
		[HarmonyPatch(typeof(MyShipDrill), "CreateTerminalControls")]
		public static IEnumerable<CodeInstruction> ShipDrillTranspiler(IEnumerable<CodeInstruction> instructions)
		{
			int index = instructions.Count() - 1;
			int num;
			for (int i = 0; i < index; i = num + 1)
			{
				yield return instructions.ElementAt(i);
				num = i;
			}
			yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(TerminalPatches), "ShipDrillControls", null, null));
			yield return new CodeInstruction(OpCodes.Ret, null);
			yield break;
		}
	}
}