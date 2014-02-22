/// <summary>
/// Burrower script by Jason Ege
/// Determines the behavior of the burrower, a creature which hides underground and attacks from beneath the player.
/// Last edited by Jason Ege on February 21, 2014 @ 4:00pm
/// </summary>

using UnityEngine;
using System.Collections;

public class BurrowerScript : EnemyScript {

	public float fMoveSpeed; //The speed of the burrower
	public float fWaitToAttack; //The wait time between when the burrower reaches the player and when he attacks.
	public float fWaitToChase; //Determines how long after an attack the enemy should wait before chasing down the player again.
	public GameObject gPlayer; //The player to go after
	
	float fCurMoveSpeed;
	float fAttackTimer;
	float fCurChaseTimer;
	bool bAttacking;
	bool bGoingDown;

	// Use this for initialization
	void Start () {
		fAttackTimer = 0;
		bAttacking = false;
		bGoingDown = false;
		fCurMoveSpeed = fMoveSpeed;
	}

	public override void Move ()
	{
		fCurChaseTimer += Time.deltaTime;
		if (fCurChaseTimer > fWaitToChase)
		{
			if (transform.position.x < gPlayer.transform.position.x)
			{
				transform.Translate (fCurMoveSpeed * Time.deltaTime, 0.0f, 0.0f);
			}
			if (transform.position.x > gPlayer.transform.position.x)
			{
				transform.Translate (-fCurMoveSpeed * Time.deltaTime, 0.0f, 0.0f);
			}
		}
		if (transform.position.x < gPlayer.transform.position.x + 0.5f && transform.position.x > gPlayer.transform.position.x - 0.5f)
		{
			bAttacking = true;
			Attack ();
		}
	}

	public override void Attack()
	{
		if (bAttacking == true && bGoingDown == false)
		{
			fAttackTimer += Time.deltaTime;
			if (fAttackTimer > fWaitToAttack)
			{
				if (transform.position.y < vSpawnPoint.y + 2)
				{
					fCurMoveSpeed = 0.0f;
					transform.Translate (0.0f, (fMoveSpeed*Time.deltaTime)/3, 0.0f);
				}
				else if (transform.position.y >= vSpawnPoint.y + 2)
				{
					fAttackTimer = 0.0f;
					bGoingDown = true;
				}
			}
		}
		else if (bAttacking == true && bGoingDown == true)
		{
			if (transform.position.y > vSpawnPoint.y)
			{
				fCurMoveSpeed = 0.0f;
				transform.Translate (0.0f, ((-fMoveSpeed*Time.deltaTime)/3), 0.0f);
				fCurChaseTimer = 0.0f;
			}
			if (transform.position.y <= vSpawnPoint.y + 0.01f)
			{
				bAttacking = false;
				bGoingDown = false;
				fAttackTimer = 0.0f;
				if (fCurChaseTimer > fWaitToChase)
				{
					fCurMoveSpeed = fMoveSpeed;
					fCurChaseTimer = 0.0f;
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {
		Move ();
	}
}
