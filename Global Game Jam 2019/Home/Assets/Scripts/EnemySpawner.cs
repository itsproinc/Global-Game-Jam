using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
	
	public float spawnPosX = 6;
	public EnemyController enemy;

	// Use this for initialization
	void Start () {
		StartCoroutine(Spawning());
	}
	
	IEnumerator Spawning()
	{
		while(true)
		{
			yield return new WaitForSeconds(4);
			Instantiate(enemy, new Vector2(Random.Range(0,2) == 1? -spawnPosX : spawnPosX, 0), Quaternion.identity);
		}
	}
}
