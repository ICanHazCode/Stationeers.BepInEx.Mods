using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using HarmonyLib;
using HarmonyLib.Tools;
using UnityEngine;

namespace ModLoadOrder.Source.Scripts
{
	//This is just an example class structure that you can use to organize your patchers
	public class WorldManagerPatches
	{
		[HarmonyPatch(typeof(WorldManager), "LoadDataFiles")]
		public class LoadDataFilesPatch
		{
			static readonly Action<string> LDFAtPath = AccessTools.MethodDelegate<Action<string>>(
														AccessTools.Method(typeof(WorldManager), "LoadDataFilesAtPath"), null, false);
			public static bool Prefix()
			{
				
				List<ModData> mods = WorkshopMenu.ModsConfig.Mods;
				for (int i = mods.Count - 1; i >= 0; i--)
				{
					ModData modData = mods[i];
					if (MLOrderPlugin.DEBUG)
						MLOrderPlugin.Log.LogDebug($"{i}\t:{modData.GetAboutData().Name, -40}:Enabled:{modData.IsEnabled}");
					//This check must be first, as Core path is an empty string
					//Check if it is Core, it's always enabled.
					if (modData.IsCore)
					{
						LDFAtPath(Application.streamingAssetsPath + "/Data");
					}//Check if mod is enabled
					else if (modData.IsEnabled)
					{
						LDFAtPath(modData.LocalPath + "/GameData");
					}
					// disabled mods fall through and get skipped
				}
				//Complete replacement
				return false;
			}
		}
	}
}
