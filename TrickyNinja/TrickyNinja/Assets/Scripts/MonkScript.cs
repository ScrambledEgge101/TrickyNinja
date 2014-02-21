//Monk script
//Last edited by Jason Ege on 02/20/2014 at 9:23am
//Handles the monk enemy. The guy that runs up to you and then jumps before landing on you.

using UnityEngine;
using System.Collections;

public class MonkScript : EnemyScript {

	public float fInitSpeed;
	public float fJumpHeight;
	public GameObject gPlayer;

	float fSpeed;
	float fVerticalSpeed;
	bool bInAir;

	// Use this for initialization
	void Start () {
		gPlayer = GameObject.FindGameObjectWithTag ("Player");
		fSpeed = fInitSpeed;
		fVerticalSpeed = fJumpHeight;
		bInAir = false;
	}
	
	public override void Move()
	{
		MonkChasePlayer ();
		MonkGravity();
	}
	
	public override void Die()
	{
		Destroy (gameObject);
	}
	
	public override void Hurt(int aiDamage)
	{
		fHealth -= aiDamage;
		if (fHealth < 0)
		{
			Die ();
		}
	}
	
	void MonkGravity()
	{
		transform.Translate (0.0f,fVerticalSpeed*Time.deltaTime,0.0f);
		fVerticalSpeed -=  20.0f*Time.deltaTime;
	}
	
	void OnCollisionStay(Collision c)
	{
		if (c.gameObject.tag == "Ground")
		{
			bInAir = false;
		}
	}
	
//	bool CollidingWithPlayer()
//	{
//		if (gPlayer.transform.position.x+1.0f > this.gameObject.transform.position.x && gPlayer.transform.position.x-1.0f < this.gameObject.transform.position.x)
//		{
//			return true;
//		}
//		else
//		{
//			return false;
//		}
//	}
	
	void MonkChasePlayer()
	{	
		if (gPlayer != null)
		{
			if (CollidingWithPlayer (gPlayer) && bInAir == false)
			{
				fSpeed = 0.0f;
				fVerticalSpeed = fJumpHeight;
				bInAir = true;
			}
			else if (!CollidingWithPlayer (gPlayer) && bInAir == false)
			{
				fSpeed = fInitSpeed;
				ChasePlayer(gPlayer, fSpeed*Time.deltaTime);
			}
			if (bInAir == false)
			{
				transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
				fVerticalSpeed = 0;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}
}
