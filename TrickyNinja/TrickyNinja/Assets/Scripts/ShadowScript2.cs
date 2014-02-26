/// <summary>
/// By Deven Smith
/// 1/29/2014
/// ShadowScript2
/// made the shadows only move to positions the player has been 
/// instead of tyring to treat it like a player recieving delayed inputs
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum Facings {Crouch, Up, Right, Left};
public class ShadowScript2 : EntityScript {
	public Facings eFacing = Facings.Right;
	public float fMoveSpeed = 5.0f;
	public GameObject gPlayerAttackPrefab;
	public GameObject goCharacter;
	public int iDelay = 60;

	List<Vector3> lvPositions = new List<Vector3>();

	// Use this for initialization
	void Start () 
	{

		for(int i = iDelay; i > 0; i--)
			lvPositions.Add(transform.position);
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale = new Vector3(1, 1, 1);
		if(eFacing == Facings.Left)
		{
			goCharacter.animation.Play("Walk");
			transform.eulerAngles = new Vector3(0, 180, 0);
		}
		if(eFacing == Facings.Right)
		{
			goCharacter.animation.Play("Walk");
			transform.eulerAngles = new Vector3(0, 0, 0);
		}
		if(eFacing == Facings.Crouch)
		{
			goCharacter.animation.Play("Duck");
			//transform.localScale = new Vector3(1, .5f, 1);
		}
		if(eFacing == Facings.Up)
		{
			goCharacter.animation.Play("LookUp");
			//transform.localScale = new Vector3(1, 1.5f, 1);
		}
	
	}

	public override void Move()
	{
		Vector3 vectorToPosition = lvPositions[0] - transform.position;
		transform.position = lvPositions[0];
		lvPositions.RemoveAt(0);
	}

	void AddPosition(Vector3 newPosition)
	{
		lvPositions.Add(newPosition);
	}

	void ChangeFacing(int newFacing)
	{
		switch(newFacing)
		{
		case 0:
			eFacing = Facings.Right;
			break;
		case 1:
			eFacing = Facings.Left;
			break;
		case 2:
			eFacing = Facings.Up;
			break;
		case 3:
			eFacing = Facings.Crouch;
			break;
		default:
			eFacing = Facings.Right;
			break;
		}
	}

	public override void Attack()
	{
		Vector3 direction = Vector3.zero;
		
		if(eFacing == Facings.Left)
		{
			direction = new Vector3(-1.0f, 0, 0);
		}
		if(eFacing == Facings.Right)
		{
			direction = new Vector3(1.0f, 0, 0);
		}
		if(eFacing == Facings.Crouch)
		{
			direction = new Vector3(0, -1.0f, 0);
		}
		if(eFacing == Facings.Up)
		{
			direction = new Vector3(0, 1.0f, 0);
		}
		
		GameObject attack = (GameObject)Instantiate (gPlayerAttackPrefab, transform.position, gPlayerAttackPrefab.transform.rotation);
		attack.SendMessage ("SetDirection", direction, SendMessageOptions.DontRequireReceiver);
		direction = Vector3.zero;
	}
}
