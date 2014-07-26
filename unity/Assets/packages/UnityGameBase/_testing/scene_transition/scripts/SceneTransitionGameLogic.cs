using UnityEngine;
using System.Collections;

public class SceneTransitionGameLogic : GameLogicImplementationBase{
	#region implemented abstract members of GameLogicImplementationBase

	public override void Start ()
	{

	}

	public override void GameSetupReady ()
	{
		Game.instance.mSceneTransition.mLoadingScreenController = Game.instance.gameObject.GetComponent<CLoadingSceneController>();
		Game.instance.mSceneTransition.LoadScene(2);
	}

	public override void GameStateChanged (SGameState pOldState, SGameState pCurrentGameState)
	{

	}

	public override SGameState GetCurrentGameState ()
	{
		return SGameState.invalid;
	}

	public override bool OnBeforeRestart ()
	{
		return true;
	}

	public override bool OnBeforePause ()
	{
		return true;
	}

	#endregion


}
