using UnityEngine;
using System.Collections;

public class UI : GameComponent
{
	float timer1 = 100.0f;
	float timer2 = 100.0f;

	string winner = "";

	void OnGUI()
	{
		GUI.color = Color.black;
		GUI.Label(new Rect(0,0,100,50), "Player 1: " + (GLogic as GameLogic).playerOneCount.ToString() + "  -  " + timer1.ToString("0.##"));
		GUI.Label(new Rect(Screen.width - 100, 0, 100, 50), "Player 2: " + (GLogic as GameLogic).playerTwoCount.ToString() + "  -  " + timer2.ToString("0.##"));

		if (!string.IsNullOrEmpty(winner))
		{
			GUI.Label(new Rect(Screen.width * 0.5f - 50.0f, Screen.height * 0.5f, 100.0f, 50.0f), winner + " wins the game!");
		}
	}

	void Update()
	{
		if(timer1 > 0 && timer2 > 0)
		{
			timer1 -= Time.deltaTime;
			timer2 -= Time.deltaTime;

		}

		if(timer1 <= 0)
		{
			//Player 1 wins
			winner = "Player 2";

		}
		else if(timer1 <= 0)
		{
			//Player 2 wins
			winner = "Player 1";
		}

	}

	public void IncreaseTimer(float pValue, int pPlayerId)
	{
		if (pPlayerId == 1)
		{
			timer1 += pValue;
		}
		else if(pPlayerId == 2)
		{
			timer2 += pValue;
		}
	}
}

