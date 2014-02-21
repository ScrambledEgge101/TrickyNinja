//Ninja Script
//Last edited by Jason Ege on 02/20/2014 at 9:30am
//Handles the Ninja that jumps up, fires a fan of bullets, and then goes away.

using UnityEngine;
using System.Collections;

public class NinjaScript : EnemyScript {

	public bool bChase;
	public int iBulletsToFire;
	public float fBulletWait;
	public float fWaitToFire;
	public float fBulletDelta;
	public float fBulletDeltaLimit;
	public float fInitBulletRot;
	public float fAddToBulletRotation;
	public float fNinjaSpeed;
	public GameObject gEnemyBullet;
	public GameObject gPlayer;
	public float fTimeAliveLimit;
	
	int iCurrentBulletsToFire;
	bool bneedtofire = true;
	bool bOnGround = false;
	float fTimeAlive;

	// Use this for initialization
	void Start () {
		gPlayer = GameObject.FindGameObjectWithTag("Player");
		iCurrentBulletsToFire = iBulletsToFire;
		Jump ();
	}
	
	void Jump()
	{
		gameObject.rigidbody.AddForce(new Vector3(0,550.0f,0), ForceMode.Force);
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
	
	void OnCollisionStay(Collision c)
	{
		if (c.gameObject.name != "Player")
		{
			bOnGround = true;
		}
	}
	
	public override void Move()
	{
		if (bOnGround == true)
		{
			if (bChase == true)
			{
				ChasePlayer(gPlayer, fNinjaSpeed*Time.deltaTime);
			}
			else if (bChase == false)
			{
				Destroy (gameObject);
			}
		}
		if (fTimeAlive >= fTimeAliveLimit)
		{
			Destroy(gameObject);
		}
		fTimeAlive += Time.deltaTime;
	}
	
	public override void Attack ()
	{
		if (fBulletWait >= fWaitToFire)
		{
			if (iCurrentBulletsToFire > 0)
			{
				if (fBulletDelta > fBulletDeltaLimit)
				{
					Instantiate (gEnemyBullet, gameObject.transform.position, Quaternion.Euler (0,0,fInitBulletRot));
					fBulletDelta = 0;
					fInitBulletRot += fAddToBulletRotation;
					iCurrentBulletsToFire--;
				}
			}
			if (iCurrentBulletsToFire <= 0)
			{
				iCurrentBulletsToFire = iBulletsToFire;
				fInitBulletRot = fAddToBulletRotation;
				fBulletDelta = 0;
				bneedtofire = false;
			}
			fBulletDelta += Time.deltaTime;
		}
		fBulletWait += Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.rigidbody.velocity.y > -1.0f && gameObject.rigidbody.velocity.y < 1.0f && bneedtofire == true)
		{
			bneedtofire = true;
		}
		if (bneedtofire == true)
		{
			this.Attack ();
		}
		Move();
	}
}
