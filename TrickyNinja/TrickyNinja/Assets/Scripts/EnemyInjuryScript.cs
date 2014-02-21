/// <summary>
/// By Deven Smith
/// Enemy injury script.
/// temparray injury script till for testing out melee attacks without the actual enemies
/// </summary>

using UnityEngine;
using System.Collections;

public class EnemyInjuryScript : EntityScript {

	public float fHealTime = 0.0f;
	public int iDamageResistance = 10;

	// Use this for initialization
	void Start () {
		renderer.material.color = Color.green;
		fHealth = 100.0f;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(fHealTime > 0.0f)
		{
			fHealTime -= Time.deltaTime;
			if(fHealTime <= 0.0f)
			{
				fHealth = 100;
				renderer.material.color = Color.green;
			}
		}
	}

	public override void Hurt(int aiDamage)
	{
		if(aiDamage - iDamageResistance > 0)
			fHealth -= aiDamage - iDamageResistance;
		else
			print("Attacks do not deal enough damage to hurt this enemy!");

		if(fHealth  < 66)
		{
			renderer.material.color = Color.yellow;
			if(fHealth < 33)
			{
				renderer.material.color = Color.red;
				fHealTime = 3.0f;
			}
		}
	}
}
