using HarmonyLib;
using Sandbox.Game.Gui;
using Sandbox.Game.Weapons;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using VRage.Utils;

namespace AutoRemoveStone
{
    [HarmonyPatch]
	public class TerminalPatches
	{
		public static void ShipDrillControls()
		{
			MyTerminalControlOnOffSwitch<MyShipDrill> myTerminalControlOnOffSwitch = new MyTerminalControlOnOffSwitch<MyShipDrill>("AutoVoidStone", MyStringId.GetOrCompute("Void Stone"));
			myTerminalControlOnOffSwitch.Getter = (MyShipDrill x) => StoneRemover.Enabled && StoneRemover.VoidMode == StoneRemover.Mode.Stone;
			myTerminalControlOnOffSwitch.Setter = delegate (MyShipDrill x, bool v)
			{
				StoneRemover.EnableMode(StoneRemover.Mode.Stone);
			};
			myTerminalControlOnOffSwitch.DynamicTooltipGetter = delegate (MyShipDrill drill)
			{
				return $"Toggle voiding of stone.";
			};
			myTerminalControlOnOffSwitch.SupportsMultipleBlocks = false;
			MyTerminalControlFactory.AddControl<MyShipDrill>(myTerminalControlOnOffSwitch);
			MyTerminalAction<MyShipDrill> myTerminalAction = new MyTerminalAction<MyShipDrill>("VoidStone", new StringBuilder("Toggle Stone voiding"), "");
			myTerminalAction.Action = delegate (MyShipDrill x) { StoneRemover.EnableMode(StoneRemover.Mode.Stone); } ;
			myTerminalAction.Writer = delegate (MyShipDrill block, StringBuilder builder)
			{
				builder.Append($"{StoneRemover.VoidMode} {StoneRemover.Counter / 10:N0}");
			};
			myTerminalAction.ValidForGroups = false;
			MyTerminalControlFactory.AddAction<MyShipDrill>(myTerminalAction);

			MyTerminalControlOnOffSwitch<MyShipDrill> myTerminalControlOnOffSwitch2 = new MyTerminalControlOnOffSwitch<MyShipDrill>("AutoVoidIce", MyStringId.GetOrCompute("Void Ice"));
			myTerminalControlOnOffSwitch2.Getter = (MyShipDrill x) => StoneRemover.Enabled && StoneRemover.VoidMode == StoneRemover.Mode.Ice;
			myTerminalControlOnOffSwitch2.Setter = delegate (MyShipDrill x, bool v)
			{
				StoneRemover.EnableMode(StoneRemover.Mode.Ice);
			};
			myTerminalControlOnOffSwitch2.DynamicTooltipGetter = delegate (MyShipDrill drill)
			{
				return $"Toggle voiding of Ice.";
			};
			myTerminalControlOnOffSwitch2.SupportsMultipleBlocks = false;
			MyTerminalControlFactory.AddControl<MyShipDrill>(myTerminalControlOnOffSwitch2);
			MyTerminalAction<MyShipDrill> myTerminalAction2 = new MyTerminalAction<MyShipDrill>("VoidIce", new StringBuilder("Toggle Ice voiding"), "");
			myTerminalAction2.Action = delegate (MyShipDrill x) { StoneRemover.EnableMode(StoneRemover.Mode.Ice); };
			myTerminalAction2.Writer = delegate (MyShipDrill block, StringBuilder builder)
			{
				builder.Append($"{StoneRemover.VoidMode} {StoneRemover.Counter/10:N0}");
			};
			myTerminalAction2.ValidForGroups = false;
			MyTerminalControlFactory.AddAction<MyShipDrill>(myTerminalAction2);

			MyTerminalControlOnOffSwitch<MyShipDrill> myTerminalControlOnOffSwitch3 = new MyTerminalControlOnOffSwitch<MyShipDrill>("AutoVoidBoth", MyStringId.GetOrCompute("Void Both"));
			myTerminalControlOnOffSwitch3.Getter = (MyShipDrill x) => StoneRemover.Enabled && StoneRemover.VoidMode == StoneRemover.Mode.Both;
			myTerminalControlOnOffSwitch3.Setter = delegate (MyShipDrill x, bool v)
			{
				StoneRemover.EnableMode(StoneRemover.Mode.Both);
			};
			myTerminalControlOnOffSwitch2.DynamicTooltipGetter = delegate (MyShipDrill drill)
			{
				return $"Toggle voiding of Stone and Ice.";
			};
			myTerminalControlOnOffSwitch3.SupportsMultipleBlocks = false;
			MyTerminalControlFactory.AddControl<MyShipDrill>(myTerminalControlOnOffSwitch3);
			MyTerminalAction<MyShipDrill> myTerminalAction3 = new MyTerminalAction<MyShipDrill>("VoidBoth", new StringBuilder("Toggle Stone and Ice voiding"), "");
			myTerminalAction3.Action = delegate (MyShipDrill x) { StoneRemover.EnableMode(StoneRemover.Mode.Both); };
			myTerminalAction3.Writer = delegate (MyShipDrill block, StringBuilder builder)
			{
				builder.Append($"{StoneRemover.VoidMode} {StoneRemover.Counter / 10:N0}");
			};
			myTerminalAction3.ValidForGroups = false;
			MyTerminalControlFactory.AddAction<MyShipDrill>(myTerminalAction3);
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