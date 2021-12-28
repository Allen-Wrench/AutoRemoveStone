using System;
using System.Reflection;
using HarmonyLib;
using VRage.Plugins;

namespace AutoRemoveStone
{
	public class Plugin : IDisposable, IPlugin
	{
		public void Dispose()
		{
		}

		public void Init(object gameInstance)
		{
			new Harmony("AutoRemoveStone").PatchAll(Assembly.GetExecutingAssembly());
		}

		public void Update()
		{
		}
	}
}