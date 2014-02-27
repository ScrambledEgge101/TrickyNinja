//Enemy Script
//Last edited by Jason Ege on 02/20/2014 @ 9:26am
//Handles things that all enemies do. All enemies derive from this class, and this class
//derives from the Entity Class.

using UnityEngine;
using System.Collections;

public class EnemyScript : EntityScript {

	// Use this for initialization
	void Start () {
	
	}

	public void ChasePlayer(GameObject gPlayer, float fChaseSpeed)
	{
		//if (gPlayer != null)
		//{
			if (gPlayer.transform.position.x < transform.position.x)
			{
				transform.rotation = Quaternion.Euler (0.0f, 90.0f, 0.0f);
			}
			else if (gPlayer.transform.position.x > transform.position.x)
			{
				transform.rotation = Quaternion.Euler(0.0f,180.0f,0.0f);
			}
			transform.Translate (transform.right*fChaseSpeed);
		//}
	}
	
	public bool CollidingWithPlayer(GameObject gPlayer)
	{
		if (gPlayer != null)
		{
			if (gPlayer.transform.position.x+1.0f > this.gameObject.transform.position.x && gPlayer.transform.position.x-1.0f < this.gameObject.transform.position.x)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		return false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
