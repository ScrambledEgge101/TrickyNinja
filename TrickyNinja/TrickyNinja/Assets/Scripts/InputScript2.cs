/// <summary>
/// By Deven Smith 
/// 1/29/2014
/// Input script2.
/// Remade the input script to be more streamlined 
/// last edited 2/6/2014 Deven Smith
/// </summary>

using UnityEngine;
using System.Collections;

public class InputScript2 : MonoBehaviour {

	//konami code up up down down left right left right B A 
	public struct KonamiCode
	{
		public int iCorrectKeys;
		
		public KeyCode kKey1;
		public KeyCode kKey2;
		public KeyCode kKey3;
		public KeyCode kKey4;
		public KeyCode kKey5;
		public KeyCode kKey6;
		public KeyCode kKey7;
		public KeyCode kKey8;
		public KeyCode kKey9;
		public KeyCode kKey10;
	}

	public struct PlayerInputs
	{
		public KeyCode kAttackButton;
		public KeyCode kJumpButton;
		public KeyCode kPauseButton;
		public KeyCode kSwapButton;

		public KeyCode kRightKey;
		public KeyCode kLeftKey;
		public KeyCode kUpKey;
		public KeyCode kCrouchKey;
		
		public KeyCode kJumpKey;
		public KeyCode kAttackKey;

		public KeyCode kSwap1Key;
		public KeyCode kSwap2Key;
		public KeyCode kSwap3key;
		public KeyCode kSwap4key;

		public KeyCode kPauseKey;
	}

	public PlayerInputs[] strctPlayerInputs = new PlayerInputs[4];
	public KonamiCode kCode;

	bool bNextKeyInCode;

	public GameObject[] gPlayer;
	public GameObject[] agShadows;


	//assigns all the players default controls
	void Start()
	{
		kCode.iCorrectKeys = 0;
		kCode.kKey1 = KeyCode.UpArrow;
		kCode.kKey2 = KeyCode.UpArrow;
		kCode.kKey3 = KeyCode.DownArrow;
		kCode.kKey4 = KeyCode.DownArrow;
		kCode.kKey5 = KeyCode.LeftArrow;
		kCode.kKey6 = KeyCode.RightArrow;
		kCode.kKey7 = KeyCode.LeftArrow;
		kCode.kKey8 = KeyCode.RightArrow;
		kCode.kKey9 = KeyCode.B;
		kCode.kKey10 = KeyCode.A;

		strctPlayerInputs[0].kJumpButton = KeyCode.Joystick1Button0;
		strctPlayerInputs[0].kAttackButton = KeyCode.Joystick1Button1;
		strctPlayerInputs[0].kPauseButton = KeyCode.Joystick1Button7;
		strctPlayerInputs[0].kSwapButton = KeyCode.Joystick1Button5;

		strctPlayerInputs[0].kRightKey = KeyCode.LeftArrow;
		strctPlayerInputs[0].kLeftKey = KeyCode.RightArrow;
		strctPlayerInputs[0].kUpKey = KeyCode.UpArrow;
		strctPlayerInputs[0].kCrouchKey = KeyCode.DownArrow;
		strctPlayerInputs[0].kJumpKey = KeyCode.Space;
		strctPlayerInputs[0].kAttackKey = KeyCode.A;
		strctPlayerInputs[0].kSwap1Key = KeyCode.Tab;

		strctPlayerInputs[1].kJumpButton = KeyCode.Joystick2Button0;
		strctPlayerInputs[1].kAttackButton = KeyCode.Joystick2Button1;
		strctPlayerInputs[1].kPauseButton = KeyCode.Joystick2Button7;

		strctPlayerInputs[1].kRightKey = KeyCode.LeftArrow;
		strctPlayerInputs[1].kLeftKey = KeyCode.RightArrow;
		strctPlayerInputs[1].kUpKey = KeyCode.UpArrow;
		strctPlayerInputs[1].kCrouchKey = KeyCode.DownArrow;
		strctPlayerInputs[1].kJumpKey = KeyCode.Space;
		strctPlayerInputs[1].kAttackKey = KeyCode.A;
	}

	// Update is called once per frame
	//just looks for the player inputs and handles them accordingly 
	void Update () 
	{
		for(int i = 0; i < gPlayer.Length; i++)
		{
			if(Input.GetKey(strctPlayerInputs[i].kRightKey) || Input.GetAxis("Player"+(i+1)+"Horizontal") == -1)
			{
				gPlayer[i].SendMessage("MoveRight", SendMessageOptions.DontRequireReceiver);
			}
			
			if(Input.GetKey(strctPlayerInputs[i].kLeftKey) ||  Input.GetAxis("Player"+(i+1)+"Horizontal") == 1)
			{
				gPlayer[i].SendMessage("MoveLeft", SendMessageOptions.DontRequireReceiver);
			}
			if(Input.GetKey(strctPlayerInputs[i].kUpKey) || Input.GetAxis("Player"+(i+1)+"Vertical") == 1)
			{
				gPlayer[i].SendMessage("LookUp", SendMessageOptions.DontRequireReceiver);
			}
			if(Input.GetKey(strctPlayerInputs[i].kCrouchKey) || Input.GetAxis("Player"+(i+1)+"Vertical") == -1)
			{
				gPlayer[i].SendMessage("Crouch", SendMessageOptions.DontRequireReceiver);
			}

			if(Input.GetKeyDown(strctPlayerInputs[i].kAttackKey) || Input.GetKeyDown(strctPlayerInputs[i].kAttackButton))
			{
				gPlayer[i].SendMessage("Attack", SendMessageOptions.DontRequireReceiver);
			}
			

			if(Input.GetKey(strctPlayerInputs[i].kJumpKey) || Input.GetKey(strctPlayerInputs[i].kJumpButton))
			{
				gPlayer[i].SendMessage("Jump", SendMessageOptions.DontRequireReceiver);
			}
			
			if(Input.GetKeyUp(strctPlayerInputs[i].kJumpKey) || Input.GetKeyUp(strctPlayerInputs[i].kJumpButton))
			{
				gPlayer[i].SendMessage("StoppedJumping", SendMessageOptions.DontRequireReceiver);
			}
			if(Input.GetKeyDown(strctPlayerInputs[i].kSwap1Key) || Input.GetKeyDown(strctPlayerInputs[i].kSwapButton))
			{
				gPlayer[i].SendMessage("ChangeWeapon", SendMessageOptions.DontRequireReceiver);
			}

			gPlayer[i].SendMessage("SetYAxis", Input.GetAxis("Player"+(i+1)+"Vertical"), SendMessageOptions.DontRequireReceiver);
			gPlayer[i].SendMessage("SetXAxis", Input.GetAxis("Player"+(i+1)+"Horizontal"), SendMessageOptions.DontRequireReceiver);
		}
	}

	//checks for events and if a key event is registered checks wether the konami code is being entered
	void OnGUI()
	{
		Event e = Event.current;
		if(e.isKey)
		{
			if(e.keyCode != KeyCode.None && bNextKeyInCode)
			{
				//Debug.Log("Detected key code: " + e.keyCode);
				CheckKonamiCode(e.keyCode);
				bNextKeyInCode = false;
			}
			else
				bNextKeyInCode = true;
		}
	}

	//determines which part of code on and wether the correct key was entered or not
	void CheckKonamiCode(KeyCode a_kKey)
	{
		if(kCode.iCorrectKeys == 9 && a_kKey == kCode.kKey10)
		{
			print ("Konami Code Success");
			kCode.iCorrectKeys = 0;
		}
		else if(kCode.iCorrectKeys == 8 && a_kKey == kCode.kKey9)
		{
			kCode.iCorrectKeys ++;
		}
		else if(kCode.iCorrectKeys == 7 && a_kKey == kCode.kKey8)
		{
			kCode.iCorrectKeys ++;
		}
		else if(kCode.iCorrectKeys == 6 && a_kKey == kCode.kKey7)
		{
			kCode.iCorrectKeys ++;
		}
		else if(kCode.iCorrectKeys == 5 && a_kKey== kCode.kKey6)
		{
			kCode.iCorrectKeys ++;
		}
		else if(kCode.iCorrectKeys == 4 && a_kKey == kCode.kKey5)
		{
			kCode.iCorrectKeys ++;
		}
		else if(kCode.iCorrectKeys == 3 && a_kKey == kCode.kKey4)
		{
			kCode.iCorrectKeys ++;
		}
		else if(kCode.iCorrectKeys == 2 && a_kKey == kCode.kKey3)
		{
			kCode.iCorrectKeys ++;
		}
		else if(kCode.iCorrectKeys == 1 && a_kKey == kCode.kKey2)
		{
			kCode.iCorrectKeys ++;
		}
		else if(kCode.iCorrectKeys == 0 && a_kKey == kCode.kKey1)
		{
			kCode.iCorrectKeys ++;
		}
		else
		{
			kCode.iCorrectKeys = 0;
		}
		//print("correct code keys = " + kCode.iCorrectKeys);
	}
}
