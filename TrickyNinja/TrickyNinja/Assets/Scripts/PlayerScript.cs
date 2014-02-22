/// <summary>
/// By Deven Smith
/// 2/13/2014
/// Player script.
/// Currently handles the players movement and his ability to attack
/// Camera Flipped now all references right are left
/// </summary>

using UnityEngine;
using System.Collections;
using System.Linq;

public class PlayerScript : EntityScript {

	Facings eFacing = Facings.Right;

	InputScript2 scrptInput;

	public bool bRangedAttack = false;
	public bool bMeleeAttack = false; //remove public when done testing
	public bool bRopeAttack = false;
	bool bGoingRight = true;
	bool bGrounded = true;
	bool bMoved = false;
	bool bCanJump = false;
	bool bStoppedJump = true;

	float fCurJumpTime = 0.0f;
	float fHeight = 0.0f;
	float fWidth = 0.0f;
	float fCurAttackTime = 0.0f;
	float fXAxis;
	float fYAxis;

	public bool bPlayer1;
	public float fMoveSpeed;
	public float fMaxAttackTime = 0.5f;
	public float fMaxJumpTime = 1.0f;
	public float fGroundDistance = 0.2f;
	public GameObject gPlayerAttackPrefab;
	public GameObject goHorAttackBox;
	public GameObject goUpAttackBox;
	public GameObject goDownAttackBox;
	public GameObject goRopePivotPoint;
	public GameObject goRopeAttackBox;
	public LayerMask lmGroundLayer;
	public GameObject goCharacter;
	
	// Use this for initialization
	// gets the input script from the main camera and figures out how tall the character is for movement
	void Start () {
		goCharacter.animation.Play("Idle");
		//disable the attack boxes 
		goRopeAttackBox.SetActive(false);
		goUpAttackBox.SetActive(false);
		goDownAttackBox.SetActive(false);
		goHorAttackBox.SetActive(false);

		scrptInput = Camera.main.GetComponent<InputScript2>();
		
		CapsuleCollider myCollider = GetComponent<CapsuleCollider>();
		fHeight = myCollider.height;
		fWidth = myCollider.radius;
	}

	void Update()
	{
		print(fXAxis + ", " + fYAxis);

		if(!goCharacter.animation.IsPlaying("Idle"))
		{
			if(bGrounded && fXAxis != 1 && fXAxis != 1 && fYAxis != 1 && fYAxis != -1)
				goCharacter.animation.Play("Idle");

			transform.eulerAngles = new Vector3(0, 0, 0);
			bGoingRight = true;
			eFacing = Facings.Right;
			SendShadowMessage("ChangeFacing" , 0);
		}

		//if currently attacking resolve it
		if(fCurAttackTime > 0)
		{
			if(bRopeAttack)
				RopeHandler();

			fCurAttackTime -= Time.deltaTime;
			if(fCurAttackTime <= 0)
			{
				goRopeAttackBox.SetActive(false);
				goDownAttackBox.SetActive(false);
				goUpAttackBox.SetActive(false);
				goHorAttackBox.SetActive(false);
			}
		}
	}
	
	// Update is called once per frame
	//checks to handle if the player has moved or if he was grounded but now is not or if he was not grounded but now is
	void LateUpdate () 
	{
		if(bGrounded)
		{
			RaycastHit hit;
			if(Physics.Raycast(transform.position, -transform.up, out hit, fGroundDistance + fHeight/2, lmGroundLayer.value))
			{
				if(hit.collider.tag != "Ground")
				{
					if(!goCharacter.animation.IsPlaying("Fall"))
						goCharacter.animation.Play("Fall");
					bGrounded = false;
					bCanJump = false;
				}
			}
			else
			{
				if(!goCharacter.animation.IsPlaying("Fall"))
					goCharacter.animation.Play("Fall");
				bGrounded = false;
				bCanJump = false;
			}
		}
		
		if(!bGrounded)
		{
			if(!bCanJump)
			{
				if(!goCharacter.animation.IsPlaying("Fall"))
					goCharacter.animation.Play("Fall");
				transform.Translate(-transform.up * fMoveSpeed * Time.deltaTime, Space.World);
				bMoved = true;
			}
		}
		
		if(bMoved)
		{
			SendShadowMessage("AddPosition" , transform.position);
			SendShadowMessage("Move");
		}
		bMoved = false;
	}
	
	//handles if the player needs to change facing and moving right
	void MoveRight()
	{
		if(fYAxis != -1)
		{
			if(!goCharacter.animation.IsPlaying("Jump") && !goCharacter.animation.IsPlaying("Fall"))
				goCharacter.animation.Play("Walk");
			if(!bGoingRight)
			{
				transform.eulerAngles = new Vector3(0, 0, 0);
				bGoingRight = true;
				eFacing = Facings.Right;
			}
			RaycastHit hit;
			if(Physics.Raycast(transform.position, transform.right, out hit, fWidth, lmGroundLayer.value))
			{
				if(hit.collider.tag != "Wall")
				{
					transform.Translate(transform.right * fMoveSpeed * Time.deltaTime,Space.World);
				}
			}
			else
			{
				transform.Translate(transform.right * fMoveSpeed * Time.deltaTime,Space.World);
			}
			SendShadowMessage("ChangeFacing" , 0);
			bMoved = true;
		}
	}
	
	//handles if the player needs to change facing and moving left
	void MoveLeft()
	{
		if(fYAxis != -1)
		{
			if(!goCharacter.animation.IsPlaying("Jump") && !goCharacter.animation.IsPlaying("Fall"))
				goCharacter.animation.Play("Walk");
			if(bGoingRight)
			{
				transform.eulerAngles = new Vector3(0, 180, 0);
				bGoingRight = false;
				eFacing = Facings.Left;
			}
			RaycastHit hit;
			if(Physics.Raycast(transform.position, transform.right, out hit, fWidth, lmGroundLayer.value))
			{
				print (hit.collider.tag);
				if(hit.collider.tag != "Wall")
				{
					transform.Translate(transform.right * fMoveSpeed * Time.deltaTime,Space.World);
				}
			}
			else
			{
				transform.Translate(transform.right * fMoveSpeed * Time.deltaTime,Space.World);
			}
			SendShadowMessage("ChangeFacing" , 1);
		bMoved = true;
		}
	}

	//ensures that the player is allowed to jump and then moves him up
	void Jump()
	{
		if(bCanJump)
		{
			goCharacter.animation.Play("Jump");
			bGrounded = false;
			transform.Translate(transform.up * fMoveSpeed * Time.deltaTime);
			fCurJumpTime += Time.deltaTime;
			
			if(fCurJumpTime >= fMaxJumpTime)
			{
				bCanJump = false;
			}
			bMoved = true;
		}
		bStoppedJump = false;
	}
	
	//stops the ability to jump 
	void StoppedJumping()
	{
		bCanJump = false;
		bStoppedJump = true;

		if(bGrounded)
		{
			RaycastHit hit;
			if(Physics.Raycast(transform.position, -transform.up, out hit, fGroundDistance + fHeight, lmGroundLayer.value))
			{
				if(hit.collider.tag == "Ground")
				{
					bCanJump = true;
				}
			}
		}
	}
	
	//handles the player looking up and informs the shadows to do the same
	void LookUp()
	{
		goCharacter.animation.Play("LookUp");
		eFacing = Facings.Up;
		SendShadowMessage("ChangeFacing" , 2);
	}
	
	//handles the player crouching and informs the shadows to do the same
	void Crouch()
	{
		eFacing = Facings.Crouch;
		goCharacter.animation.Play("Duck");
		SendShadowMessage("ChangeFacing" , 3);
	}

	//selects the direction to send to the attack based on the facing that is selected then creates and attack and gives it the direction to travel in
	public override void Attack()
	{
		Vector3 vDirection = Vector3.zero;
		
		if(eFacing == Facings.Left)
		{
			vDirection = new Vector3(-1.0f, 0, 0);
		}
		if(eFacing == Facings.Right)
		{
			vDirection = new Vector3(1.0f, 0, 0);
		}
		if(eFacing == Facings.Crouch)
		{
			vDirection = new Vector3(0, -1.0f, 0);
		}
		if(eFacing == Facings.Up)
		{
			vDirection = new Vector3(0, 1.0f, 0);
		}

		if(bRangedAttack)
		{
			GameObject attack = (GameObject)Instantiate (gPlayerAttackPrefab, transform.position, gPlayerAttackPrefab.transform.rotation);
			attack.SendMessage ("SetDirection", vDirection, SendMessageOptions.DontRequireReceiver);
		}
		if(bMeleeAttack)
		{
			if(eFacing == Facings.Left || eFacing == Facings.Right)
			{
				goUpAttackBox.SetActive(false);
				goDownAttackBox.SetActive(false);
				goHorAttackBox.SetActive(true);
				fCurAttackTime = fMaxAttackTime;
			}
			else if(eFacing == Facings.Up)
			{
				goUpAttackBox.SetActive(true);
				goDownAttackBox.SetActive(false);
				goHorAttackBox.SetActive(false);
				fCurAttackTime = fMaxAttackTime;
			}
			else
			{
				goUpAttackBox.SetActive(false);
				goDownAttackBox.SetActive(true);
				goHorAttackBox.SetActive(false);
				fCurAttackTime = fMaxAttackTime;
			}
		}

		if(bRopeAttack)
		{
			goRopeAttackBox.SetActive(true);
			fCurAttackTime = fMaxAttackTime;
		}

		SendShadowMessage("Attack");
		vDirection = Vector3.zero;
	}

	void RopeHandler()
	{
		float angle = Mathf.Atan2(fYAxis, -fXAxis) * Mathf.Rad2Deg;
		goRopePivotPoint.transform.eulerAngles = new Vector3(0,0,angle);
	}


	void SetYAxis(float a_fValue)
	{
		fYAxis = a_fValue;
	}

	void SetXAxis(float a_fValue)
	{
		fXAxis = a_fValue;
	}

	//sends a message to all shadows if player 1
	void SendShadowMessage(string a_sMessage)
	{
		if(bPlayer1)
		{
			foreach(GameObject shadow in scrptInput.agShadows)
			{
				shadow.SendMessage(a_sMessage, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	//sends a message and value to all shadows if player 1 
	void SendShadowMessage(string a_sMessage , int a_iValue)
	{
		if(bPlayer1)
		{
			foreach(GameObject shadow in scrptInput.agShadows)
			{
				shadow.SendMessage(a_sMessage, a_iValue ,SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	//sends a message and value to all shadows if player 1 
	void SendShadowMessage(string a_sMessage , Vector3 a_vValue)
	{
		if(bPlayer1)
		{
			foreach(GameObject shadow in scrptInput.agShadows)
			{
				shadow.SendMessage(a_sMessage, a_vValue ,SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	//right now only used to find the ground
	void OnCollisionStay(Collision c)
	{
		if(c.collider.tag == "Ground")
		{
			//if(c.collider.transform.position.y < transform.position.y)
			if(c.contacts[0].point.y < transform.position.y)
			{
				bGrounded = true;

				if(bStoppedJump)
					bCanJump = true;

				transform.position = new Vector3(transform.position.x, c.contacts[0].point.y  + fGroundDistance + fHeight/2, transform.position.z);
				fCurJumpTime = 0.0f;
			}
		}
	}
}
