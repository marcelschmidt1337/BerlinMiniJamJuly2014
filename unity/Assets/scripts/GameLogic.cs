using UnityEngine;

public class GameLogic : GameLogicImplementationBase
{
	public int playerOneCount {get; private set;}
	public int playerTwoCount {get; private set;}
	public int[][] level { get { return levelGrid;}}
	int[][] levelGrid;

	public event System.Action<int[][]> OnItemCollected;

	#region implemented abstract members of GameLogicImplementationBase

	public override void Start ()
	{
		SGameState.Add((int)EGameState.Ingame, EGameState.Ingame.ToString());
	}

	public override void GameSetupReady ()
	{
		Game.instance.mSceneTransition.mFadeTexture = UIHelpers.blackTexture;
		Game.instance.mSceneTransition.LoadScene((int)EGameState.Ingame);
	}

	public override void GameStateChanged (SGameState pOldState, SGameState pCurrentGameState)
	{
		if(pCurrentGameState == (int)EGameState.Ingame)
		{
			Game.instance.GetComponent<LevelCreator>().GenerateLevel((level) => {
				levelGrid = level;
			});
		}
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

		if(OnItemCollected != null)
		{
			Game.instance.GetComponent<LevelCreator>().SpawnCollectable(ref levelGrid);
		}
	}

	#endregion


}

