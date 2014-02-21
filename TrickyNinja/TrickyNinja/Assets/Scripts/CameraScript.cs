/// <summary>
/// By Deven Smith
/// Camera script.
/// matches the character up and down movement
/// </summary>

using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	public GameObject Player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, transform.position.z);	
	}
}
