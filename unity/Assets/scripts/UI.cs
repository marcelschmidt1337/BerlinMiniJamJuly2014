using UnityEngine;
using System.Collections;

public class UI : GameComponent
{
	void OnGUI()
	{
		GUI.Label(new Rect(0,0,100,50), "Player 1: " + (GLogic as GameLogic).playerOneCount.ToString());
		GUI.Label(new Rect(Screen.width - 100, 0, 100, 50), "Player 2: " + (GLogic as GameLogic).playerTwoCount.ToString());
	}
}

