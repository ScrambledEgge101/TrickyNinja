using UnityEngine;
using System.Collections;

public class SpawnScript : EntityScript {

	public GameObject gNinja;
	public GameObject Player;
	GameObject gPlayer;

	// Use this for initialization
	void Start () {
		gPlayer = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.N))
		{
			Instantiate (gNinja, gPlayer.transform.position, gPlayer.transform.rotation);
		}
		if (Input.GetKey (KeyCode.M) && Input.GetKeyDown (KeyCode.O))
		{
			gPlayer.transform.position = new Vector3(-108.0f, -65, 25.0f);
		}
		if (Input.GetKey (KeyCode.M) && Input.GetKeyDown (KeyCode.V))
		{
			gPlayer.transform.position = new Vector3(-150.0f, -73.0f, 25.0f);
		}
		/*if (Input.GetKey (KeyCode.LeftArrow))
		{
			Player.transform.rotation = Quaternion.Euler (0.0f, 90.0f, 0.0f);
			Player.transform.Translate (Player.transform.right*Time.deltaTime*8);
		}
		else if (Input.GetKey (KeyCode.RightArrow))
		{
			Player.transform.rotation = Quaternion.Euler (0.0f, 180.0f, 0.0f);
			Player.transform.Translate (Player.transform.right*Time.deltaTime*8);
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			Player.rigidbody.AddForce (0.0f, 700.0f, 0.0f, ForceMode.Force);
		}*/
	}
}
