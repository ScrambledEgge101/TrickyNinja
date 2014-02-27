using UnityEngine;
using System.Collections;

public class SwordGuy : EnemyScript {

	public float fSpeed;

	GameObject gPlayer;	
	bool bGoingUp;

	// Use this for initialization
	void Start () {
		gPlayer = GameObject.FindGameObjectWithTag("Player");
	}
	
	public override void Die()
	{
		Destroy (gameObject);
	}
	
	public override void Hurt(int aiDamage)
	{
		fHealth -= aiDamage;
		bGoingUp = true;
		if (fHealth < -2)
		{
			Die ();
		}
	}
	
	public override void Move ()
	{
		if (bGoingUp == true)
		{
			transform.Translate (0.0f, 20.0f*Time.deltaTime, 12.0f*Time.deltaTime);
			transform.position = new Vector3(transform.position.x, transform.position.y, 25.0f);
			if (transform.position.y > vSpawnPoint.y+1.75f)
			{
				bGoingUp = false;
			}
		}
		if (transform.position.y > vSpawnPoint.y && bGoingUp == false)
		{
			transform.Translate (0.0f, -6.0f * Time.deltaTime, 0.0f);
		}
		if (transform.position.y < vSpawnPoint.y + 0.02f && bGoingUp == false)
		{
			ChasePlayer (gPlayer, fSpeed*Time.deltaTime);
		}
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}
}
