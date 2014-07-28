using UnityEngine;

public class GameLogic : GameLogicImplementationBase
{
	public int playerOneCount {get; private set;}
	public int playerTwoCount {get; private set;}
	public int[][] level { get { return levelGrid;}}
	int[][] levelGrid;

	UI mUI;
	LevelCreator mLevelCreator;

	#region implemented abstract members of GameLogicImplementationBase

	public override void Start ()
	{
		SGameState.Add((int)EGameState.Ingame, EGameState.Ingame.ToString());

		mUI = Game.instance.GetComponentInChildren<UI>();
		mLevelCreator = Game.instance.GetComponent<LevelCreator>();


	}

	public override void GameSetupReady ()
	{
		Game.instance.mSceneTransition.OnSceneTransitionIsDone += HandleOnSceneTransitionIsDone;
		Game.instance.mSceneTransition.mFadeTexture = UIHelpers.blackTexture;
		Game.instance.mSceneTransition.LoadScene((int)EGameState.Ingame);
	}

	void HandleOnSceneTransitionIsDone (int pSceneID)
	{
		if(pSceneID == (int)EGameState.Ingame)
		{
			mLevelCreator.GenerateLevel((level) => {
				levelGrid = level;
			});
		}
	}

	public override void GameStateChanged (SGameState pOldState, SGameState pCurrentGameState)
	{
//		if(pCurrentGameState == (int)EGameState.Ingame)
//		{
//			mLevelCreator.GenerateLevel((level) => {
//				levelGrid = level;
//			});
//		}
	}

	public override SGameState GetCurrentGameState ()
	{
		return Application.loadedLevel;
	}

	public override bool OnBeforeRestart ()
	{
		return true;
	}

	public override bool OnBeforePause ()
	{
		return true;
	}

	public void IncreaseItemCount(int pPlayerId)
	{
		if(pPlayerId == 1)
		{
			playerOneCount++;
		}
		else if(pPlayerId == 2)
		{
			playerTwoCount++;
		}

		mUI.IncreaseTimer(10.0f, pPlayerId);
		mLevelCreator.SpawnCollectable(ref levelGrid);
	}

	#endregion

	public void RestartLevel()
	{
		mLevelCreator.DestroyLevel();
		Game.instance.mSceneTransition.LoadScene((int)EGameState.Ingame, true);
	}


}

