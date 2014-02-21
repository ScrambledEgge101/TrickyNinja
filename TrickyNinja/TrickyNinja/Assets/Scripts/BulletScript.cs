//Bullet Script
//Last edited by Jason Ege on 02/20/2014 at 9:23am
//Abstract class that handles bullet activities.

using UnityEngine;
using System.Collections;

public class BulletScript : EntityScript {

	// Use this for initialization
	void Start () {
	
	}
	
	public void KillBullet(GameObject bullettokill)
	{
		Destroy (bullettokill);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
