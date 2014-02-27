//Builted by: Steven Hoover
//Last Edited by: Steven Hoover

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionSceneScript : MonoBehaviour {

	ControllerMenuInput menuInput;

	//set by editor
	public string sFirstLevelName;
	public Texture texture;
	public int iDotSize = 50;
	public float fTimeBetweenMoves = 2;
		//rects
	public Rect	standardButton;
	List<sButton> buttons;


	// Use this for initialization
	void Start () {
		FileIO.LoadProfiles();
		menuInput = new ControllerMenuInput();
		menuInput.Init( texture , FileIO.profileContainer.profiles.Count -1 , iDotSize , fTimeBetweenMoves );
		buttons = new List<sButton>();

		//print ( FileIO.profileContainer.profiles.Count ); //debug
		//add buttons
		int count = 0; // temp way to move buttons
		foreach( Profile p in FileIO.profileContainer.profiles )
		{
			sButton b = new sButton();
			b.Init( p.name , new Vector2( (Screen.width / 2) - standardButton.width/2 , standardButton.y - count*30) , new Vector2( standardButton.width, standardButton.height ) , LoadProfile , p.name );
			buttons.Add( b );
			count++;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		menuInput.SetActiveButton( buttons[ menuInput.GetIndex() ] );
		menuInput.Update();
	}

	void OnGUI()
	{
		menuInput.Draw();
		foreach( sButton b in buttons )
		{
			b.Draw();
		}
	}

	void LoadProfile()
	{
		Profile p = FileIO.GetProfileByName( menuInput.active.GetLevel() );
		if( p.name == "" )
		{
			print( "Load failed" );
			return;
		}
		GameObject profile = new GameObject();
		profile.tag = "Profile";
		profile.AddComponent<ProfileObjectScript>();
		profile.GetComponent<ProfileObjectScript>().profile = p;
		DontDestroyOnLoad( profile );

		Application.LoadLevel( sFirstLevelName );
	}
}
