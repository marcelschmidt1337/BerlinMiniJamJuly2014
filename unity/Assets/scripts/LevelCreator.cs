using UnityEngine;
using System.Collections;

public class LevelCreator : MonoBehaviour 
{
	public int levelWidth;
	public int levelHeight;

	void Start()
	{
		for(int x = 0; x < levelWidth; x++)
		{
			for (int y = 0; y < levelHeight; y++)
			{
				var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
				go.transform.position = new Vector3(x,y, 0);
			}
		}


	}
}
