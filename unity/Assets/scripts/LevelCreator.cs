using UnityEngine;
using System.Collections;

public class LevelCreator : MonoBehaviour 
{
	public int levelWidth;
	public int levelHeight;
	public int maxObjects;

	public GameObject[] obstacles;
	
	void Start()
	{
		GenerateLevel();
	}

	void GenerateLevel()
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
	}

	void GenerateObstacles(ref int[][] pLevelGrid)
	{
		for(int x = 0; x < pLevelGrid.Length; x++)
		{
			for (int y = 0; y < pLevelGrid[0].Length; y++)
			{
				if(pLevelGrid[x][y] == (int)EObject.None && maxObjects > 0)
				{
					int randInt = Random.Range((int)EObject.None, (int)EObject.Obstacle);

					if(randInt == (int)EObject.Obstacle)
					{
						GameObject go = obstacles[Random.Range(0, obstacles.Length - 1)];
						Instantiate(go, new Vector3(x,1,y), Quaternion.identity);
						maxObjects--;
					}
				}
			}
		}
	}
}
