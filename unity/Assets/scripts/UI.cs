using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : GameComponent 
{	
	public float timer = 100.0f;

	private float timer1, timer2;
	private int score1, score2;

	private Slider p1, p2;
	private Text t1, t2, gameOverMessage;
	bool mGameOver = false;
	GameObject mGameOverScreen;

	
	void Start()
	{
		Game.instance.mSceneTransition.OnSceneTransitionIsDone += HandleOnSceneTransitionIsDone;

		timer1 = timer2 = -1;
		score1 = score2 = 0;

		p1 = transform.Find("Ingame/P1").GetComponentInChildren<Slider>();
		p2 = transform.Find("Ingame/P2").GetComponentInChildren<Slider>();
		t1 = transform.Find("Ingame/P1").GetComponentInChildren<Text>();
		t2 = transform.Find("Ingame/P2").GetComponentInChildren<Text>();

		mGameOverScreen = transform.Find("Ingame/GameOver").gameObject;
		gameOverMessage = mGameOverScreen.transform.Find("Message").GetComponent<Text>();

		p1.maxValue = p2.maxValue = timer;

		t1.text = "Player 1 - Score: 0";
		t2.text = "Player 2 - Score: 0";
	}

	void HandleOnSceneTransitionIsDone (int pSceneID)
	{
		timer1 = timer2 = timer;
	}

	void Update()
	{
		if(timer1 > 0 && timer2 > 0)
		{
			timer1 -= Time.deltaTime;
			timer2 -= Time.deltaTime;

			p1.value = timer1;
			p2.value = timer2;
		}

		if(timer1 <= 0 && timer1 != -1)
		{
			GameOver(2);
		}
		else if(timer2 <= 0 && timer2 != -1)
		{
			GameOver(1);
		}

		if(mGameOver)
		{
			if(Input.anyKeyDown)
			{
				timer1 = timer2 = -1;
				mGameOverScreen.SetActive(false);
				(GLogic as GameLogic).RestartLevel();
			}
		}
	}

	void GameOver(int pWinnerId)
	{
		mGameOver = true;
		mGameOverScreen.SetActive(mGameOver);
		gameOverMessage.text = "Player " + pWinnerId + " wins!";
	}

	public void IncreaseTimer(float pValue, int pPlayerId)
	{
		switch(pPlayerId)
		{
			case 1: timer1 += pValue; t1.text = "Player 1 - Score: " + ++score1; break;
			case 2: timer2 += pValue; t2.text = "Player 2 - Score: " + ++score2; break;
		}
	}
}
