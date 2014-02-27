//Built by: Steven Hoover
//Last Edited by: Steven Hoover

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GamepadInput;
using System.Xml;
using System.Xml.Serialization;

//public enum ProfileSceneState {CreateLoad, Edit};
public enum Button { A, B, Y, X, RightShoulder, LeftShoulder, RightStick, LeftStick, Back, Start, LeftTrigger, RightTrigger , None};

public class Profile
{
	[XmlAttribute("name")]
	public string name;
	[XmlAttribute("atttack")]
	public Button kAttack;
	[XmlAttribute("jump")]
	public Button kJump;
	[XmlAttribute("pause")]
	public Button kPause;
	[XmlAttribute("Swap1")]
	public Button kSwap1;
	[XmlAttribute("Swap2")]
	public Button kSwap2;
	[XmlAttribute("Swap3")]
	public Button kSwap3;
	[XmlAttribute("Swap4")]
	public Button kSwap4;

	public void SwapKeys( string asWhichKeyCode , Button akNewCode )
	{
		switch( asWhichKeyCode )
		{
		case "Attack":
			kAttack = akNewCode;
			break;
		case "Jump":
			kJump = akNewCode;
			break;
		case "Pause":
			kPause = akNewCode;
			break;
		case "Swap1":
			kSwap1 = akNewCode;
			break;
		case "Swap2":
			kSwap2 = akNewCode;
			break;
		case "Swap3":
			kSwap3 = akNewCode;
			break;
		case "Swap4":
			kSwap4 = akNewCode;
			break;
		}
	}

	public void ChangeName( string sNewName )
	{
		name = sNewName;
	}
}

public class ProfilesSceneScript : MonoBehaviour {

	ControllerMenuInput controllerMenuInput;
	//ProfileSceneState state;
	Profile loadedProfile;
	Vector2 vSize;
	bool started = false;
	int mainColumn = 0;
	//varibales set by editor
	public float colWidth = 200;
	public float rowHeight = 50;
	public Texture texture;
	public int iDotSize = 50;
	public float fTimeBetweenMoves = 2;

	//rects used for textfields
	public Rect rNameText;
	public Rect rAttackDis;
	public Rect rJumpDis;
	public Rect rPauseDis;
	public Rect rSwap1Dis;
	public Rect rSwap2Dis;
	public Rect rSwap3Dis;
	public Rect rSwap4Dis;
	public Rect rExit;


	//buttons
		//createload
	sButton createProfileButton;
	sButton loadProfileButton;
	
		//edit
	sButton editNameButton;
	sButton editAttackButton;
	sButton editJumpButton;
	sButton editPauseButton;
	sButton editSwap1Button;
	sButton editSwap2Button;
	sButton editSwap3Button;
	sButton editSwap4Button;
	sButton exitButton;
	sButton saveButton;
	List<sButton> profileButtons;
	// Use this for initialization
	void Start () 
	{
		started = true;
		FileIO.LoadProfiles();
		controllerMenuInput = new ControllerMenuInput();
		controllerMenuInput.Init( texture , 9 , iDotSize , fTimeBetweenMoves );
		controllerMenuInput.bActiveButtonConstantPressed = true;
		//state = new ProfileSceneState();
		vSize = new Vector2( colWidth, rowHeight );

		createProfileButton = new sButton();
		createProfileButton.Init("Create Profile", new Vector2( colWidth / 2, rowHeight * 2 ) , vSize , CreateProfileFunc , "None" );

		loadProfileButton = new sButton();
		loadProfileButton.Init("Load Profile", new Vector2(200,200) , vSize , CreateProfileFunc , "None" );

		exitButton = new sButton();
		exitButton.Init("Exit" , new Vector2( colWidth /2 , rowHeight * 4 ) , vSize , ExitProfileButtonFunc , "Exit" );

		saveButton = new sButton();
		saveButton.Init("Save" , new Vector2( colWidth /2 , rowHeight * 3 ) , vSize , SaveProfileButtonFunc , "Save" );
		profileButtons = new List<sButton>();
		MakeProfileButtons(false);

		//editButtons = new List<sButton>();
		editNameButton = new sButton();
		editNameButton.Init("Name: " , new Vector2( rNameText.x , rNameText.y ) , new Vector2( rNameText.width, rNameText.height ) , SwitchButton , "Name" );
		editAttackButton = new sButton();
		editAttackButton.Init("Attack: " , new Vector2( rAttackDis.x , rAttackDis.y ) , new Vector2( rAttackDis.width, rAttackDis.height ), SwitchButton , "Attack" );
		editJumpButton = new sButton();
		editJumpButton.Init("Jump: " , new Vector2( rJumpDis.x , rJumpDis.y ) , new Vector2( rJumpDis.width, rJumpDis.height ), SwitchButton , "Jump" );
		editPauseButton = new sButton();
		editPauseButton.Init("Pause: " ,new Vector2( rPauseDis.x , rPauseDis.y ) , new Vector2( rPauseDis.width, rPauseDis.height ), SwitchButton , "Pause" );
		editSwap1Button = new sButton();
		editSwap1Button.Init("Swap1: " , new Vector2( rSwap1Dis.x , rSwap1Dis.y ) , new Vector2( rSwap1Dis.width, rSwap1Dis.height ) , SwitchButton , "Swap1" );
		editSwap2Button = new sButton();
		editSwap2Button.Init("Swap2: " ,new Vector2( rSwap2Dis.x , rSwap2Dis.y ) , new Vector2( rSwap2Dis.width, rSwap2Dis.height ) , SwitchButton , "Swap2" );
		editSwap3Button = new sButton();
		editSwap3Button.Init("Swap3: " ,new Vector2( rSwap3Dis.x , rSwap3Dis.y ) , new Vector2( rSwap3Dis.width, rSwap3Dis.height ), SwitchButton , "Swap3" );
		editSwap4Button = new sButton();
		editSwap4Button.Init("Swap4: " ,new Vector2( rSwap4Dis.x , rSwap4Dis.y ) , new Vector2( rSwap4Dis.width, rSwap4Dis.height ) , SwitchButton , "Swap4" );
	}
	
	// Update is called once per frame
	void Update ()
	{
		if( !started ) Start();

		SendActiveButton();

		controllerMenuInput.Update();

		if( loadedProfile != null) print ( loadedProfile.name );
	}

	void OnGUI()
	{
		if( !started ) Start();

		createProfileButton.Draw();
		//loadProfileButton.Draw();
		saveButton.Draw();

		controllerMenuInput.Draw();
		if(exitButton.Draw() ) ExitProfilePage();

		profileButtons.ForEach( delegate(sButton obj) 
		{
			obj.Draw();
		});

		//textboxes
		if( loadedProfile != null )
		{
			loadedProfile.name = GUI.TextField( rNameText , loadedProfile.name , 10 );
			GUI.Label( rAttackDis , loadedProfile.kAttack.ToString()  );
			GUI.Label( rJumpDis , loadedProfile.kJump.ToString()  );
			GUI.Label( rPauseDis , loadedProfile.kPause.ToString()  );
			GUI.Label( rSwap1Dis , loadedProfile.kSwap1.ToString()  );
			GUI.Label( rSwap2Dis , loadedProfile.kSwap2.ToString()  );
			GUI.Label( rSwap3Dis , loadedProfile.kSwap3.ToString()  );
			GUI.Label( rSwap4Dis , loadedProfile.kSwap4.ToString()  );
		}
	}

	void AddButtonsToList( List<sButton> buttonList , params sButton [] buttons )
	{
		foreach( sButton b in buttons )
		{
			buttonList.Add(b);
		}
	}

	void MakeProfileButtons(bool clearList)
	{
		if(clearList) FileIO.profileContainer.profiles.Clear();

		float f = 0;
		float a = 5; // point that these start at
		foreach( Profile p in FileIO.profileContainer.profiles )
		{
			sButton b = new sButton();
			b.Init( "Load: " + p.name , new Vector2( colWidth / 2 , rowHeight * (a + f ) ), vSize , LoadProfile , p.name );
			profileButtons.Add(b);
			f++;
		}
	}

	void SendActiveButton ()
	{
		if( mainColumn == 3 )
		{
			switch( controllerMenuInput.GetIndex() % 9 )
			{
			case 0:
				controllerMenuInput.SetActiveButton( editNameButton );
				break;
			case 1:
				controllerMenuInput.SetActiveButton( editAttackButton );
				break;
			case 2:
				controllerMenuInput.SetActiveButton( editJumpButton );
				break;
			case 3:
				controllerMenuInput.SetActiveButton( editPauseButton );
				break;
			case 4:
				controllerMenuInput.SetActiveButton( editSwap1Button );
				break;
			case 5:
				controllerMenuInput.SetActiveButton( editSwap2Button );
				break;
			case 6:
				controllerMenuInput.SetActiveButton( editSwap3Button );
				break;
			case 7:
				controllerMenuInput.SetActiveButton( editSwap4Button );
				break;
			case 8:
				controllerMenuInput.SetActiveButton( exitButton );
				break;
			}
		}
		else
		{
			switch( controllerMenuInput.GetIndex() % 9 )
			{
			case 0:
				controllerMenuInput.SetActiveButton( createProfileButton );
				break;
			case 1:
				controllerMenuInput.SetActiveButton( saveButton );
				break;
			case 2:
				controllerMenuInput.SetActiveButton( exitButton );
				break;
			case 3:
				controllerMenuInput.SetActiveButton( profileButtons[0] );
				break;
			case 4:
				controllerMenuInput.SetActiveButton( editSwap1Button );
				break;
			case 5:
				controllerMenuInput.SetActiveButton( editSwap2Button );
				break;
			case 6:
				controllerMenuInput.SetActiveButton( editSwap3Button );
				break;
			case 7:
				controllerMenuInput.SetActiveButton( editSwap4Button );
				break;
			case 8:
				controllerMenuInput.SetActiveButton( exitButton );
				break;
			default:
				break;
			}
		}
	}

	void CreateProfileFunc()
	{
		loadedProfile = new Profile();
		loadedProfile.name = "Default";
		loadedProfile.kAttack = Button.A;
		loadedProfile.kJump = Button.B;
		loadedProfile.kPause = Button.X;
		loadedProfile.kSwap1 = Button.Y;
		loadedProfile.kSwap2 = Button.LeftTrigger;
		loadedProfile.kSwap3 = Button.LeftShoulder;
		loadedProfile.kSwap4 = Button.RightShoulder;
	}

	void SwitchButton()
	{
		GamepadState state = GamePad.GetState( GamePad.Index.One );
		if( state.A || state.B || state.X || state.Y || state.LeftTrigger != 0 || state.RightTrigger != 0 || state.LeftShoulder || state.RightShoulder )
		{
			Button buttonPressed = PullButton( state );
			string activeButton = controllerMenuInput.GrabActiveButton().GetLevel();
			loadedProfile.SwapKeys( activeButton , buttonPressed );
		}
	}

	Button PullButton( GamepadState aState )
	{
		if( aState.A ) return Button.A;
		if( aState.B ) return Button.B;
		if( aState.X ) return Button.X;
		if( aState.Y ) return Button.Y;
		if( aState.LeftShoulder ) return Button.LeftShoulder;
		if( aState.RightShoulder ) return Button.RightShoulder;
		if( aState.LeftTrigger != 0 ) return Button.LeftTrigger;
		if( aState.RightTrigger != 0 ) return Button.RightTrigger;
		if( aState.Start ) return Button.Start;
		else
		{
			return Button.None;
		}
	}

	void LoadProfile()
	{
		//if( FileIO.profileContainer.profiles.Find( x => x.name == controllerMenuInput.active.GetLevel() ) == null ) print ("Its null");
		print(  controllerMenuInput.active.GetLevel() );
		loadedProfile = FileIO.profileContainer.profiles.Find( x => x.name == controllerMenuInput.active.GetLevel() );
	}

	void SaveProfileButtonFunc()
	{
		FileIO.AddToContainer(loadedProfile);
		FileIO.SaveProfiles();
	}

	void ExitProfileButtonFunc()
	{
		if( GamePad.GetButtonDown( GamePad.Button.A, GamePad.Index.One ) )
		{
			ExitProfilePage();
		}
	}

	void ExitProfilePage()
	{
		Application.LoadLevel("MainMenu");
	}

}
