//Enemy Bullet Script
//Last edited by Jason Ege on 02/20/2014
//Handles the enemies' bullets

using UnityEngine;
using System.Collections;

public class EnemyBulletScript : BulletScript {

	public float fSpeed;
	public float fAliveTime;

	float fKillTimer;

	// Use this for initialization
	void Start () {
	
	}
	
	void BulletCantLastForever()
	{
		fKillTimer += Time.deltaTime;
		if (fKillTimer > fAliveTime)
		{
			KillBullet (this.gameObject);
		}
	}
	
	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.tag == "Player")
		{
			Destroy (gameObject); //Destroy the bullet object
			c.gameObject.renderer.enabled = false; //Disable the player's renderer
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (transform.right*Time.deltaTime*fSpeed,Space.Self);
		BulletCantLastForever ();
	}
}
