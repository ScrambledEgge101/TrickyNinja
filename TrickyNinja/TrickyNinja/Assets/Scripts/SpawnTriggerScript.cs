//Spawn Trigger Script
//Detects when the player enters the bounds of the Spawn Trigger and then spawns
//the enemy specified

using UnityEngine;
using System.Collections;

public class SpawnTriggerScript : EntityScript {

	public GameObject gEnemyToSpawn;
	public Vector3[] vOffset;

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.tag == "Player")
		{
			for (int i = 0; i < vOffset.Length; i++)
			{
				Instantiate (gEnemyToSpawn, vSpawnPoint + vOffset[i], Quaternion.Euler (0.0f,90.0f,0.0f));
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
