/// <summary>
/// By Deven Smith
/// 2/20/2014
/// Melee attack script.
/// sets Damage and sends it to enemies
/// </summary>

using UnityEngine;
using System.Collections;

public class MeleeAttackScript : MonoBehaviour {

	public int iMeleeDamage = 100;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SetMeleeDamage(int a_iNewDamage)
	{
		iMeleeDamage = a_iNewDamage;
	}

	void OnTriggerEnter(Collider c)
	{
		if(c.tag == "Enemy")
		{
			c.gameObject.SendMessage("Hurt", iMeleeDamage, SendMessageOptions.DontRequireReceiver);
		}
	}
}
