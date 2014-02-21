/// <summary>
/// By Deven Smith
/// 2/12/2014
/// Entity script.
/// Abstract Class to layout the framework for other classes
/// </summary>

using UnityEngine;
using System.Collections;

public class EntityScript : MonoBehaviour 
{
	public struct Weapon
	{
		public string sName;
		public int iDamage;
	}

	public bool bIncorporeal;
	public Vector3 vSpawnPoint;
	public float fHealth;

	public virtual void Move()
	{

	}

	public virtual void Attack()
	{
	}

	public virtual void Hurt(int aiDamage)
	{
		fHealth -= aiDamage;
	}

	public virtual void Die()
	{
	}

	public virtual void ChangeFacing()
	{
	}

}
