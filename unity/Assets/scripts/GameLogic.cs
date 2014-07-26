using UnityEngine;

public class GameLogic : GameLogicImplementationBase
{
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
		throw new System.NotImplementedException ();
	}

	public override SGameState GetCurrentGameState ()
	{
		return Application.loadedLevel;
	}

	public override bool OnBeforeRestart ()
	{
		throw new System.NotImplementedException ();
	}

	public override bool OnBeforePause ()
	{
		throw new System.NotImplementedException ();
	}

	#endregion


}

