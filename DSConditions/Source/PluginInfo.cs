using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSConditions.Source
{
	public static class PluginInfo
	{
		public const string PLUGIN_AUTHOR    = "X_Blaster";
		public const string PLUGIN_NAME      = "Difficulty Starting Conditions";
		public const string PLUGIN_SHORTNAME = "DSConditions";
		public const string PLUGIN_VERSION   = "1.0.2";
		// This is your workshop id for the mod.
		public const uint   PLUGIN_ID        = 2929937198;
		// This is what Harmony uses to distinguish different plugins and patchers from each other.
		// This string can really be anything, but this format is the most used and easy to recognise.
		public const string PLUGIN_GUID      = "org.stationeers." + PLUGIN_AUTHOR + "." + PLUGIN_SHORTNAME;
	}
}
