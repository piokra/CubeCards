using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class BoardCreatorScript : MonoBehaviour
{

	public Vector2[] Walls = { new Vector3(-5, -5), new Vector3(5, -5), new Vector3(5,5), new Vector3(-5, 5)};

	public Material Material;
	public PhysicMaterial PhysicMaterial;
	
	// Use this for initialization
	void Start ()
	{
		var board = new GameObject("Board");
		float minx = float.MaxValue, miny = float.MaxValue, maxx = float.MinValue, maxy = float.MinValue;
		for (int cur = 0; cur < Walls.Length; ++cur)
		{
		
			int next = (cur + 1) % Walls.Length;

			BoardUtil.CreateWall(Walls[cur], Walls[next], board, Material);
		}

		BoardUtil.CreateFloor(Walls, board, Material, PhysicMaterial);
	
		
	}
}
