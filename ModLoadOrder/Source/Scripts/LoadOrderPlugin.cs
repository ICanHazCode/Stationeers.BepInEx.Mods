using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BepInEx;
using BepInEx.Logging;

using HarmonyLib;

using JetBrains.Annotations;

using ModLoadOrder.Source;
using ModLoadOrder.ModUtil;

namespace ModLoadOrder.Source.Scripts
{
	[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_SHORTNAME, PluginInfo.PLUGIN_VERSION)]
	class MLOrderPlugin : BaseUnityPlugin
	{
		public static ManualLogSource Log;
		public static bool DEBUG = true;
		private static Harmony harmony;
		public MLOrderPlugin ()
		{
			Log = Logger;
			ModSettings.Log = Logger;
			ModSettings.configFile = Config;
			ModSettings.LoadModSettings();
			DEBUG = ModSettings.Debug.Value;
		}
		[UsedImplicitly]
		private void Awake()
		{
			// This is where the mod starts.
			// Do your initialization here.
			// Run your patches in Patch()
			if (ModSettings.IsEnabled)
			{
				Log.LogMessage($"{PluginInfo.PLUGIN_NAME} {PluginInfo.PLUGIN_VERSION} loaded.");
				Patch();
			}
			else
				Log.LogInfo($"{PluginInfo.PLUGIN_NAME} is disabled, skipping.");
		}
		// This is whre your patch will happen
		private void Patch()
		{
			harmony = new Harmony(PluginInfo.PLUGIN_GUID);
			harmony.PatchAll();
			Log.LogMessage($"{PluginInfo.PLUGIN_NAME} patching finished.");
		}

	}
}
