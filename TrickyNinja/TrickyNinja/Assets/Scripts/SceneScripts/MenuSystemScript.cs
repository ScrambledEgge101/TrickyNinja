
//Built by: Steven Hoover
//Last Edited by: Steven Hoover 2/19/2014


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GamepadInput;

public delegate void ButtonFunc();

public class sButton
{
	string sName;
	Vector2 vPosition;		//location on screen
	Vector2 vSize;			//realestate taken on screen
	public ButtonFunc BFexecute;	//function executed when button is pressed
	string sLevel;

	public void Init(string asName, Vector2 avPosition, Vector2 avSize, ButtonFunc aButtonFunc, string asLevel)
	{
		sName = asName;
		vPosition = avPosition;
		vSize = avSize;
		BFexecute = aButtonFunc;
		sLevel = asLevel;
	}

	public bool Draw()
	{
		if(GUI.Button(new Rect(vPosition.x,vPosition.y,vSize.x,vSize.y/2), sName) )
		{
			BFexecute();
			return true;
		}
		return false;
	}

	public void LoadLevel()
	{
		Application.LoadLevel(sLevel);
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public Vector2 GetPos()
	{
		return vPosition;
	}

	public Vector2 GetSize()
	{
		return vSize;
	}

	public string GetLevel()
	{
		return sLevel;
	}
}


public class ControllerMenuInput
{
	public struct Dot
	{
		public Vector2 vPosition;
		public Vector2 vSize;
	}
	Dot dotLeft;
	Dot dotRight;
	int iIndex =0;
	int iMaxIndex;
	public sButton active;
	Texture texture;
	public bool bActiveButtonConstantPressed = false;

	//variables for preventing continuos circling between options
	float fTimeSinceLastMove =0;
	public float fTimeBetweenMoves = 2;

	public void Init( Texture aTexture , int aiMaxIndex, int iDotSize, float afTimeBetweenMoves)
	{
		texture = aTexture;
		iMaxIndex = aiMaxIndex;

		dotLeft = new Dot();
		dotLeft.vSize = new Vector2(iDotSize,iDotSize);
		dotLeft.vPosition = Vector2.zero;

		dotRight = new Dot();
		dotRight.vSize = new Vector2(iDotSize,iDotSize);
		dotRight.vPosition = Vector2.zero;

		fTimeBetweenMoves = afTimeBetweenMoves;
	}
	public void Update()
	{
		fTimeSinceLastMove += Time.deltaTime;
		if( fTimeSinceLastMove > fTimeBetweenMoves)
		{
			fTimeSinceLastMove = 0;

			if( GamePad.GetAxis(GamePad.Axis.LeftStick , GamePad.Index.One).y < 0) //if( Input.GetAxis("Player1Vertical") > 0)
			{
				iIndex++;
			}
			if( GamePad.GetAxis(GamePad.Axis.LeftStick , GamePad.Index.One ).y > 0)
			{
				iIndex--;
			}

			if( iIndex > iMaxIndex)
			{
				iIndex = 0;
			}
			if( iIndex < 0 )
			{
				iIndex = iMaxIndex;
			}
		}

		if( GamePad.GetButtonDown(GamePad.Button.A , GamePad.Index.One) || bActiveButtonConstantPressed)
		{
			active.BFexecute();
		}
	}

	public void Draw()
	{
		GUI.DrawTexture(new Rect( dotLeft.vPosition.x, dotLeft.vPosition.y, dotLeft.vSize.x, dotLeft.vSize.y ), texture , ScaleMode.ScaleToFit) ;
		GUI.DrawTexture(new Rect( dotRight.vPosition.x, dotRight.vPosition.y, dotRight.vSize.x, dotRight.vSize.y ), texture , ScaleMode.ScaleToFit) ;
	}

	public int GetIndex()
	{
		return iIndex;
	}

	public void SetActiveButton( sButton aActive)
	{
		active = aActive;
		
		Vector2 pos = active.GetPos(); 	//variable to hold new pos for dot, set to active buttons's pos
		//dots need to move due to both being drawn from upper left
			//both need to move negativily in the y to center on button
		//right dot needs to move rigth by length of button
		dotRight.vPosition = new Vector2(pos.x + active.GetSize().x, pos.y - 20);
		//
		dotLeft.vPosition = new Vector2(pos.x - dotLeft.vSize.x, pos.y -20);
	}

	public sButton GrabActiveButton()
	{
		return active;
	}
}

public class MenuSystemScript : MonoBehaviour {

	ControllerMenuInput controller;
	public Texture texture;
	public int iDotSize = 50;
	public float fTimeBetweenMoves = 2;
	float fScreenWidth;
	float fScreenHeight;
	Vector2 fSpaceForButton; // button uses percentage of this space
	float fNumOfButtons = 5;
	public string PlayLevel = "level";
	public string ProfilesLevel = "Profiles";
	public string OptionsLevel = "Options";
	public string CreditsLevel = "Credits";
	List<sButton> lButtons;
	// Use this for initialization
	void Start () 
	{
		controller = new ControllerMenuInput();
		controller.Init( texture , (int)(fNumOfButtons -1) , iDotSize, fTimeBetweenMoves);

		float fButtonCount =0;
		lButtons = new List<sButton>();
		fScreenWidth = Screen.width;
		fScreenHeight = Screen.height;
		fSpaceForButton.y = (fScreenHeight / 2) / fNumOfButtons;
		fSpaceForButton.x = fScreenWidth / 4;

		sButton b = new sButton();
		b.Init("Play", new Vector2( (fScreenWidth/2) - (fScreenWidth/8) , (fScreenHeight/2) +  fSpaceForButton.y*fButtonCount) , fSpaceForButton, b.LoadLevel , PlayLevel);
		lButtons.Add( b );
		fButtonCount++;

//		Button b2 = new Button();
//		b2.Init("Multiplayer",new Vector2( fScreenWidth/2 - fScreenWidth/8 , fScreenHeight/2 +  fSpaceForButton.y*fButtonCount) , fSpaceForButton, b2.LoadLevel , "level");
//		lButtons.Add( b2 );
//		fButtonCount++;

		sButton b3 = new sButton();
		b3.Init("Profiles",new Vector2( fScreenWidth/2 - fScreenWidth/8 , fScreenHeight/2 +  fSpaceForButton.y*fButtonCount) , fSpaceForButton, b3.LoadLevel , ProfilesLevel);
		lButtons.Add( b3 );
		fButtonCount++;

		sButton b4 = new sButton();
		b4.Init("Options",new Vector2( fScreenWidth/2 - fScreenWidth/8 , fScreenHeight/2 +  fSpaceForButton.y*fButtonCount) , fSpaceForButton, b4.LoadLevel , OptionsLevel);
		lButtons.Add( b4 );
		fButtonCount++;

		sButton b5 = new sButton();
		b5.Init("Credits",new Vector2( fScreenWidth/2 - fScreenWidth/8 , fScreenHeight/2 +  fSpaceForButton.y*fButtonCount) , fSpaceForButton, b5.LoadLevel , CreditsLevel);
		lButtons.Add( b5 );
		fButtonCount++;

		sButton b6 = new sButton();
		b6.Init("Exit",new Vector2( fScreenWidth/2 - fScreenWidth/8 , fScreenHeight/2 +  fSpaceForButton.y*fButtonCount) , fSpaceForButton, b5.ExitGame , "Credits");
		lButtons.Add( b6 );
		fButtonCount++;
	}
	
	// Update is called once per frame
	void Update () 
	{
		controller.SetActiveButton( lButtons[controller.GetIndex()] );
		controller.Update();
	}

	public void LoadLevel(string levelName)
	{
		Application.LoadLevel(levelName);
	}

	void OnGUI()
	{
		foreach( sButton b in lButtons )
		{
			b.Draw();
		}

		controller.Draw();
	}
}
