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


public enum Facings {Crouch, Up, Right, Left, Idle};
public class ShadowScript2 : EntityScript {
	public Facings eFacing = Facings.Right;
	public float fMoveSpeed = 5.0f;
	public GameObject gPlayerAttackPrefab;
	public GameObject goCharacter;
	public int iDelay = 60;
	public bool bGrounded = false;
	float fHeight = 0.0f;
	float fWidth = 0.0f;
	public float fGroundDistance = 0.2f;
	Vector3 direction = Vector3.zero;

	List<Vector3> lvPositions = new List<Vector3>();
	public LayerMask lmGroundLayer;

	// Use this for initialization
	void Start () 
	{
		for(int i = iDelay; i > 0; i--)
			lvPositions.Add(transform.position);

		CapsuleCollider myCollider = GetComponent<CapsuleCollider>();
		fHeight = myCollider.height;
		fWidth = myCollider.radius;
	}
	
	// Update is called once per frame
	void Update () {

		//transform.localScale = new Vector3(1, 1, 1);
		RaycastHit hit;
		Debug.DrawLine(transform.position, transform.position + (-transform.up * (fGroundDistance + fHeight/2)));
		if(Physics.Raycast(transform.position, -transform.up, out hit, fGroundDistance + fHeight/2, lmGroundLayer.value))
		{
			if(hit.collider.tag != "Ground")
			{
				bGrounded = false;
			}
			else 
			{
				bGrounded = true;
			}
		}
		else
		{
			if(!goCharacter.animation.IsPlaying("Jump") && !goCharacter.animation.IsPlaying("Duck")  && !goCharacter.animation.IsPlaying("LookUp") )
			{
				goCharacter.animation.Play("Jump");
				ChangeFacing(4);
			}
			bGrounded = false;
		}

		if(eFacing == Facings.Left)
		{
			if(bGrounded)
				goCharacter.animation.Play("Walk");
			transform.eulerAngles = new Vector3(0, 180, 0);
		}
		if(eFacing == Facings.Right)
		{
			if(bGrounded)
				goCharacter.animation.Play("Walk");
			transform.eulerAngles = new Vector3(0, 0, 0);
		}
		if(eFacing == Facings.Crouch)
		{
			goCharacter.animation.Play("Duck");
		}
		if(eFacing == Facings.Up)
		{
			goCharacter.animation.Play("LookUp");
		}
		if(eFacing == Facings.Idle)
		{
			if(bGrounded)
				goCharacter.animation.Play("Idle");
			else
			{
				if(goCharacter.animation.IsPlaying("Duck") || goCharacter.animation.IsPlaying("LookUp"))
					goCharacter.animation.Play("Jump");
			}
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
			direction = new Vector3(1.0f, 0, 0);
			break;
		case 1:
			eFacing = Facings.Left;
			direction = new Vector3(-1.0f, 0, 0);
			break;
		case 2:
			eFacing = Facings.Up;
			direction = new Vector3(0, 1.0f, 0);
			break;
		case 3:
			eFacing = Facings.Crouch;
			direction = new Vector3(0, -1.0f, 0);
			break;
		case 4:
			eFacing = Facings.Idle;

			if(transform.eulerAngles == new Vector3(0, 180, 0))
				direction = new Vector3(1.0f, 0, 0);
			else
				direction = new Vector3(-1.0f, 0, 0);

			break;
		default:
			eFacing = Facings.Right;
			break;
		}
	}

	public override void Attack()
	{
		GameObject attack = (GameObject)Instantiate (gPlayerAttackPrefab, transform.position, gPlayerAttackPrefab.transform.rotation);
		attack.SendMessage ("SetDirection", direction, SendMessageOptions.DontRequireReceiver);
	}
}
