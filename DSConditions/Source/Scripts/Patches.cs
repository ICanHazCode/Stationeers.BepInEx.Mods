using Assets.Scripts;
using Assets.Scripts.Objects;
using Assets.Scripts.Serialization;
using Assets.Scripts.UI;
using Assets.Scripts.Util;

using Cysharp.Threading.Tasks;

using HarmonyLib;

using JetBrains.Annotations;

using System;
using System.Text.RegularExpressions;

using TMPro;

using UI;

using UnityEngine;

namespace DSConditions.Source.Scripts
{
	//This is just an example structure that you can use to organize your patchers
	class WorldConfigurationMenuPatches
	{
		[HarmonyPatch(typeof(WorldConfigurationMenu))]
		[HarmonyPatch("StartGame")]
		public class StartGamePatch
		{

			private static bool TestStartConditions(string saveName)
			{
				if (string.IsNullOrEmpty(saveName))
				{
					PromptPanel.Instance.ShowPrompt("Failure",
						"Can not create a new save with empty name.",
						"Ok", StartGameDelegates.EmptyAction);
					return false;
				}
				Match match_saves = Regex.Match(saveName, "^saves");
				Match match_backup = Regex.Match(saveName, "^Backup");
				Match match_autosave = Regex.Match(saveName, "^AutoSave");
				if (match_saves.Success || match_backup.Success || match_autosave.Success)
				{
					PromptPanel.Instance.ShowPrompt("Failure",
						string.Format("Can not create a new save with invalid word: {0} {1} {2}", 
							match_saves.Value, match_backup.Value, match_autosave.Value),
						"Ok", StartGameDelegates.EmptyAction);
					return false;
				}
				if (StationSaveUtils.IsSaveExist(saveName))
				{
					PromptPanel.Instance.ShowPrompt("Failure",
						"Can not create a new save with the same name as existing save.",
						"Ok", StartGameDelegates.EmptyAction);
					return false;
				}
				return true;
			}


			[UsedImplicitly]
			private static bool Prefix(WorldConfigurationMenu __instance, TMP_InputField ____worldNameInput, DifficultyButtonItem ____selectedDifficulty)
			{
				StartGame(____worldNameInput.text, ____selectedDifficulty.DifficultySetting, __instance.gameObject);
				return false;

			}
			/// <summary>
			/// All the synchronus parts of the <see cref="WorldConfigurationMenu.StartGame">StartGame</see> function.
			/// </summary>
			/// <param name="saveName">Name of the World save.</param>
			/// <param name="difficultySetting">Standard and Custom difficulties reference</param>
			/// <param name="gameObject">the WorldConfigurationMenu Unity handle</param>
			private static void StartGame(string saveName, DifficultySetting difficultySetting, GameObject gameObject)
			{
				//string saveName = _worldNameInput.text;
				if (TestStartConditions(saveName))
				{

					StartGameDelegates gameDelegates = new StartGameDelegates
					{
						worldSetting = NewWorldMenu.SelectedWorld.WorldSetting,
						saveName = saveName,
						difficultySetting = difficultySetting,
						_thisObject = gameObject
					};
					if (DSConditionsPlugin.DEBUG)
						DSConditionsPlugin.Log.LogDebug($"World conditions:{gameDelegates.worldSetting.StartingCondition,20} Difficulty conditions:{gameDelegates.difficultySetting.StartingCondition}");
					bool ddf = gameDelegates.difficultySetting.StartingCondition == "Default";
					if (ddf || gameDelegates.worldSetting.StartingCondition == "Default")
					{

						gameDelegates.startCondition = ddf ? gameDelegates.worldSetting.StartingCondition : gameDelegates.difficultySetting.StartingCondition;
						if (ddf)
						{
							gameDelegates.StartGameWorld();
						}
						else
						{
							gameDelegates.StartDifficulty();
						}
					}
					else
					{
#pragma warning disable CS0618 // Type or member is obsolete
						string worldKey = WorldManager.StartingConditions.Find(gameDelegates.FindWorldCondition)?.StringKey;
						string settingKey = WorldManager.StartingConditions.Find(gameDelegates.FindDifficultyCondition)?.StringKey;
#pragma warning restore CS0618 // Type or member is obsolete
						Singleton<ConfirmationPanel>.Instance.SetUpPanel("Warning: Conflicting Starting Conditions",
							string.Concat(
								new string[]
								{
						"Choose the Starting Conditions:",
						"\nWorld Start: ", Localization.InterfaceExists(worldKey)   ? Localization.GetInterface(worldKey)   : gameDelegates.worldSetting.StartingCondition,
						"\nDifficulty : ", Localization.InterfaceExists(settingKey) ? Localization.GetInterface(settingKey) : gameDelegates.difficultySetting.StartingCondition
								}),
							worldKey,
							gameDelegates.StartGameWorld,
							settingKey,
							gameDelegates.StartDifficulty,
						"ButtonCancel", StartGameDelegates.EmptyAction);
					}
				}
			}
		}
	}
}
