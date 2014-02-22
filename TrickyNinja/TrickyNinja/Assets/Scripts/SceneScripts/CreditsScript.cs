using UnityEngine;
using System.Collections;

public class CreditsScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		
		if(GUI.Button(new Rect(50,50,50,50), "Exit") || Input.GetKey(KeyCode.JoystickButton0 ) )
		{
			Application.LoadLevel("MainMenu");
		}
	}
}
