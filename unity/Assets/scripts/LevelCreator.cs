using UnityEngine;
using System.Collections;

public class LevelCreator : MonoBehaviour 
{
	public int levelWidth;
	public int levelHeight;

	public int playerAmount = 2;
	public GameObject[] players;
	public int waterMaxAmount = 1;
	public GameObject waterSource;
	public int maxObjects;
	public GameObject[] obstacles;
	
	void Start()
	{
		GenerateLevel((level) => {

		});
	}

	void GenerateLevel(System.Action<int[][]> pOnDone)
	{
		//Init level grid
		int[][] levelGrid = new int[levelWidth][];

		for(int i = 0; i < levelWidth; i++)
		{
			levelGrid[i] = new int[levelHeight];
		}

		var levelParent = new GameObject();
		levelParent.name = "Ground";
		levelParent.transform.position = Vector3.zero;
		for(int x = 0; x < levelWidth; x++)
		{
			for (int y = 0; y < levelHeight; y++)
			{
				var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
				go.transform.position = new Vector3(x,0, y);
				go.transform.parent = levelParent.transform;
				levelGrid[x][y] = (int)EObject.None;
				if(x == 0 || x == levelWidth - 1 || y == 0 || y == levelHeight - 1)
				{
					var border = GameObject.CreatePrimitive(PrimitiveType.Cube);
					border.name = "Border";
					border.renderer.enabled = false;
					border.transform.position = new Vector3(x,1, y);
					border.transform.parent = levelParent.transform;

					levelGrid[x][y] = (int)EObject.Obstacle;
				}
			}
		}

		Vector3 camPos = Camera.main.transform.position;
		camPos.x = (float)levelWidth * 0.5f;
		Camera.main.transform.position = camPos;

		GenerateObstacles(ref levelGrid);
		SpawnPlayers(ref levelGrid);
		SpawnCollectable(ref levelGrid);

		if(pOnDone != null)
		{
			pOnDone(levelGrid);
		}
	}

	void GenerateObstacles(ref int[][] pLevelGrid)
	{
		for(int x = 0; x < pLevelGrid.Length; x++)
		{
			for (int y = 0; y < pLevelGrid[0].Length; y++)
			{
				if(pLevelGrid[x][y] == (int)EObject.None && maxObjects > 0)
				{
					float randFloat = Random.Range(0,1.0f);

					if(randFloat < (float)maxObjects / ((float)levelWidth * (float)levelHeight))
					{
						GameObject go = obstacles[Random.Range(0, obstacles.Length - 1)];
						Instantiate(go, new Vector3(x,1,y), Quaternion.identity);
						pLevelGrid[x][y] = (int)EObject.Obstacle;
						maxObjects--;
					}
				}
			}
		}
	}

	void SpawnPlayers(ref int[][] pLevelGrid)
	{
		while(playerAmount > 0)
		{
			for(int x = 0; x < pLevelGrid.Length; x++)
			{
				for (int y = 0; y < pLevelGrid[0].Length; y++)
				{
					if(pLevelGrid[x][y] == (int)EObject.None && playerAmount > 0)
					{
						float randFloat = Random.Range(0,1.0f);
						
						if(randFloat < (float)playerAmount / ((float)levelWidth * (float) levelHeight))
						{
							var player = players[playerAmount - 1];
							Instantiate(player, new Vector3(x,2,y), Quaternion.identity);
							pLevelGrid[x][y] = (int)EObject.Player;
							playerAmount--;
						}
					}
				}
			}
		}
	}

	public void SpawnCollectable(ref int[][] pLevelGrid)
	{
		int amount = waterMaxAmount;

		while(amount > 0)
		{
			for(int x = 0; x < pLevelGrid.Length; x++)
			{
				for (int y = 0; y < pLevelGrid[0].Length; y++)
				{
					if(pLevelGrid[x][y] == (int)EObject.None && amount > 0)
					{
						float randFloat = Random.Range(0,1.0f);
						
						if(randFloat < (float)waterMaxAmount / ((float)levelWidth * (float) levelHeight))
						{
							Instantiate(waterSource, new Vector3(x,1,y), Quaternion.identity);
							pLevelGrid[x][y] = (int)EObject.Item;
							amount--;
						}
					}
				}
			}
		}
	}
}
