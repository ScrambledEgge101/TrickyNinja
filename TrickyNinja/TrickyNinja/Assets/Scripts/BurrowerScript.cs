/// <summary>
/// Burrower script by Jason Ege
/// Determines the behavior of the burrower, a creature which hides underground and attacks from beneath the player.
/// Last edited by Jason Ege on February 26, 2014 @ 4:22pm
/// </summary>

using UnityEngine;
using System.Collections;

public class BurrowerScript : EnemyScript {

	public float fMoveSpeed; //The initial (master) speed of the burrower
	public float fWaitToAttack; //The total wait time between when the burrower reaches the player and when he attacks.
	public float fWaitToChase; //Determines how long after an attack the enemy should wait before chasing down the player again.
	public GameObject gPlayer; //The player that the burrower should go after
	
	float fCurMoveSpeed; //The current moving speed of the burrower. This becomes zero when the burrower reaches the player and remains zero throughout the attack process.
	float fAttackTimer; //The timer that counts up to how long it should be between the time the burrower reaches the player and the time when the burrower actually pops up.
	float fCurChaseTimer; //The wait time  between the burrower spawn/attack event and the burrower starts chasing the player event.
	bool bAttacking; //Is the burrower currently "attacking" the player, meaning either moving up or down.
	bool bGoingDown; //If true, that means the burrower is moving back down after missing the player.

	// Use this for initialization
	void Start () {
		fAttackTimer = 0;  //Initialize the attack timer to 0.
		bAttacking = false; //The burrower is not currently attacking the player.
		bGoingDown = false;  //The burrower is not going down.
		fCurMoveSpeed = fMoveSpeed; //The burrower's current speed is the master speed (defined by the Unity interface).
	}

	//The Move method.
	//This controls burrower movement and movement between the two states ("attack" and "move").
	public override void Move ()
	{
		fCurChaseTimer += Time.deltaTime; //Add time to the chase timer to determine when the burrower should start chasing the player.
		if (fCurChaseTimer >= fWaitToChase && bAttacking == false) //If it is time to chase the player and the burrower is not already in attack state...
		{
			fCurMoveSpeed = fMoveSpeed; //Set the current move speed to the initial move speed so it chases the player.
			if (transform.position.x < gPlayer.transform.position.x) //If the burrower is to the left of the player...
			{
				transform.Translate (fCurMoveSpeed * Time.deltaTime, 0.0f, 0.0f); //Set the burrower's speed positive so it goes right.
			}
			if (transform.position.x > gPlayer.transform.position.x) //If the burrower is to the right of the player...
			{
				transform.Translate (-fCurMoveSpeed * Time.deltaTime, 0.0f, 0.0f); //Set the burrower's speed negative so it goes left.
			}
		}
		else if (fCurChaseTimer < fWaitToChase) //Otherwise, if it is not yet time to chase the player...
		{
			fCurMoveSpeed = 0.0f; //The current move speed is still 0.
		} //End of check to see if it is time to chase the player or not.
		
		if (transform.position.x < gPlayer.transform.position.x + 0.35f && transform.position.x > gPlayer.transform.position.x - 0.35f) //If the player is close to on top of the burrower...
		{
			bAttacking = true; //The burrower is currently attacking the player.
		}
		if (bAttacking == true) //If the burrower is supposed to be attacking...
		{
			fCurMoveSpeed = 0.0f; //The current moving speed of the burrower is zero.
			Attack(); //Call the attack method.
		}
	}

	//This function tells the burrower how to attack the player.
	//In other words, what defines an "attack" for the burrower.
	public override void Attack()
	{
		if (bAttacking == true && bGoingDown == false) //If the burrower is currently attacking and is not going down.
		{
			fCurMoveSpeed = 0.0f; //Change the burrower's current speed to prevent it from further chasing the player while it's attacking.
			fAttackTimer += Time.deltaTime;	//Add time to the attack timer so the burrower does not attack the player immediately after reaching him. This gives
											//the player a chance to escape at the last minute.
			if (fAttackTimer > fWaitToAttack) //If the time to wait has added up and it is time to attack..
			{
				if (transform.position.y < vSpawnPoint.y + 2) //If the burrower's y position is less than its spawn point position y (meaning the burrower is still underground).
				{
					//fCurMoveSpeed = 0.0f; //Change the burrower's current speed to prevent it from further chasing the player while it's attacking.
					transform.Translate (0.0f, (fMoveSpeed*Time.deltaTime)/3, 0.0f); //Move burrower up slowly.
				}
				else if (transform.position.y >= vSpawnPoint.y + 2) //If the burrower is above its spawn point + 2.
				{
					fAttackTimer = 0.0f; //Reset the attack timer to 0.
					bGoingDown = true; //The burrower is going down.
				}
			}
		}
		else if (bAttacking == true && bGoingDown == true) //Otherwise, if the burrower is attacking and going down.
		{
			if (transform.position.y > vSpawnPoint.y) //If the burrower is above its spawn point
			{
				fCurMoveSpeed = 0.0f; //Its current move speed is sett to zero
				transform.Translate (0.0f, ((-fMoveSpeed*Time.deltaTime)/3), 0.0f); //Move the burrower back down.
				fCurChaseTimer = 0.0f; //Reset the current chase timer.
			}
			if (transform.position.y <= vSpawnPoint.y + 0.01f) //If the player's y position is less than slightly above its initial spawn position y.
			{
				bGoingDown = false; //The burrower is no longer moving down.
				fCurChaseTimer = 0.0f; //Reset the chase timer to give the player a little head start.
				fAttackTimer = 0.0f; //Reset the attack timer so the enemy will wait to attack even after he finds the player again.
				transform.position = new Vector3(transform.position.x, vSpawnPoint.y+0.02f, transform.position.z); //The burrower's position y is the spawn point y plus 0.02.
				bAttacking = false; //The burrower is no longer attacking.
			}
		}
	}

	// Update is called once per frame
	void Update () {
		Move (); //Call the move method that controls basically everything about the burrower.
	}
}
