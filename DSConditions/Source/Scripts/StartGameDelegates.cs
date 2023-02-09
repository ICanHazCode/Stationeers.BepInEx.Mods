using System;
using System.Runtime.CompilerServices;

using Assets.Scripts;
using Assets.Scripts.Objects;
using Assets.Scripts.Serialization;

using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.Events;

namespace DSConditions.Source.Scripts
{
	internal class StartGameDelegates
	{
		private _StartGame startMachine;
		internal string saveName;
		internal string startCondition;
		internal GameObject _thisObject;
		internal WorldSetting worldSetting;
		internal DifficultySetting difficultySetting;

		public static UnityAction EmptyAction = EmptyFn;
		public static Func<bool> TickWait = TickCount;
		private static void EmptyFn() { }
		private static bool TickCount() => GameManager.GameTickCount > 5;
		internal bool FindWorldCondition(WorldManager.ConditionData c) => c.Key == worldSetting.StartingCondition;
		internal bool FindDifficultyCondition(WorldManager.ConditionData c) => c.Key == difficultySetting.StartingCondition;

		internal void StartGameWorld()
		{
			Start(worldSetting.StartingCondition);

		}

		internal void StartDifficulty()
		{
			Start(difficultySetting.StartingCondition);

		}
		private void Start(string condition)
		{
			startCondition = condition;
			startMachine._this = this;
			startMachine._builder = AsyncVoidMethodBuilder.Create();
			startMachine._state = -1;
			startMachine._builder.Start(ref startMachine);
		}

		internal struct _StartGame : IAsyncStateMachine
		{
			private UniTask.Awaiter _awaiter;
			internal AsyncVoidMethodBuilder _builder;
			internal int _state;
			internal StartGameDelegates _this;

			public void MoveNext()
			{
				int currentState = _state;
				try
				{
					UniTask.Awaiter awaiter;
					if (currentState != 0) // -1 or 1
					{
						if (currentState == 1) //await UniTask.WaitUntil(StartGameDelegates.TickCountAction);

						{
							awaiter = _awaiter;
							_awaiter = default;
							_state = -1;
							awaiter.GetResult();
						}
						else // currentState == -1 (first start)
						{
							XmlSaveLoad.ClearAll();
							_this._thisObject.SetActive(false);
							string name = _this.worldSetting.Name;
							_this.worldSetting.DifficultySetting = _this.difficultySetting;
							WorldManager.CurrentWorldSetting = _this.worldSetting;
							WorldManager.CurrentWorldName = name;

							WorldManager.SetStartCondition(_this.startCondition);
							WorldManager.SetRespawnCondition(_this.difficultySetting.RespawnCondition);
							//await World.NewOrContinue(true, name);
							awaiter = World.NewOrContinue(true, name).GetAwaiter();
							if (!awaiter.IsCompleted)
							{
								_state = 0;
								_awaiter = awaiter;
								_builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
								return;
							}
							//await UniTask.WaitUntil(StartGameDelegates.TickCountAction);
							//XmlSaveLoad.Instance.SaveGame(saveName);
						}
					}
					else // currentState = 0
					{ //await World.NewOrContinue(true, name);
						awaiter = _awaiter;
						_awaiter = default;
						_state = -1;
						awaiter.GetResult();
						awaiter = UniTask.WaitUntil(TickWait).GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							_state = 1;
							_awaiter = awaiter;
							_builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
					}
					//await UniTask.WaitUntil(StartGameDelegates.TickCountAction);
					XmlSaveLoad.Instance.SaveGame(_this.saveName);
				}
				catch (Exception ex)
				{
					_state = -2;
					_this.saveName = null;
					_builder.SetException(ex);
				}
				_state = -2;
				_this.saveName = null;
				_builder.SetResult();
			}
			public void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_builder.SetStateMachine(stateMachine);
			}
		}
	}
}
