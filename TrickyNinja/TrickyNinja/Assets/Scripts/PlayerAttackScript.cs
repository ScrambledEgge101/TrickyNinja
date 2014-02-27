/// <summary>
/// By Deven Smith
/// 2/5/2014
/// Player attack script.
/// receives a direction from the player and travels in it
/// </summary>

using UnityEngine;
using System.Collections;

public class PlayerAttackScript : BulletScript {

	public int iDamage = 100;
	Vector3 vDirection;
	public float fMoveSpeed = 1.0f;
	public float fLifeTime = 3.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerStay(Collider c)
	{
		if (c.gameObject.tag == "Enemy")
		{
			c.gameObject.SendMessage("Hurt", iDamage, SendMessageOptions.DontRequireReceiver);
			Destroy (gameObject);
		}
	}
	/*void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.tag == "Enemy")
		{
			c.gameObject.SendMessage("Hurt", 1, SendMessageOptions.DontRequireReceiver);
			Destroy (gameObject);
		}
	}*/
	
	// Update is called once per frame
	//moves it forward and removes time from its life and if its life is up deletes it
	void Update () {
		transform.Translate (vDirection * fMoveSpeed * Time.deltaTime);

		fLifeTime -= Time.deltaTime;

		if (fLifeTime <= 0.0f)
			Destroy (gameObject);
	}


	//a function to call to tell the attack in waht direction to travel
	void SetDirection(Vector3 newDirection)
	{
		vDirection = newDirection;
	}
}
