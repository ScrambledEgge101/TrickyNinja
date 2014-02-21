using UnityEngine;
using System.Collections;

public class SwordGuy : EnemyScript {

	GameObject gPlayer;
	public float fSpeed;
	//justin comment
	

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
		if (fHealth < 0)
		{
			Die ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		ChasePlayer (gPlayer, fSpeed*Time.deltaTime);
	}
}
