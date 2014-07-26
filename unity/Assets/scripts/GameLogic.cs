using UnityEngine;

public class GameLogic : GameLogicImplementationBase
{
	#region implemented abstract members of GameLogicImplementationBase

	public override void Start ()
	{
		throw new System.NotImplementedException ();
	}

	public override void GameSetupReady ()
	{
		throw new System.NotImplementedException ();
	}

	public override void GameStateChanged (SGameState pOldState, SGameState pCurrentGameState)
	{
		throw new System.NotImplementedException ();
	}

	public override SGameState GetCurrentGameState ()
	{
		throw new System.NotImplementedException ();
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

