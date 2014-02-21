//Built by: Steven Hoover
//Last Edited by: Steven Hoover

using UnityEngine;
using System.Collections;
using GamepadInput;

public class InputCharScript : MonoBehaviour {
	
	public struct PlayerContInputs
	{
		public GamePad.Button bAttack;
		public GamePad.Button bJump;
		public GamePad.Button bPause;
		public GamePad.Button bSwap1;
		public GamePad.Button bSwap2;
		public GamePad.Button bSwap3;
		public GamePad.Button bSwap4;

	}
	
	public PlayerContInputs[] strctPlayerInputs = new PlayerContInputs[4];
	
	public GameObject[] gPlayer;
	public GameObject[] agShadows;
	
	
	//assigns all the players default controls
	void Start()
	{
		strctPlayerInputs[0].bJump = GamePad.Button.LeftShoulder;
		strctPlayerInputs[0].bAttack = GamePad.Button.RightShoulder;
		strctPlayerInputs[1].bPause = GamePad.Button.Start;
	}
	
	// Update is called once per frame
	//just looks for the player inputs and handles them accordingly 
	void Update () 
	{
		for(int i = 0; i < gPlayer.Length; i++)
		{
			if( GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.One ).x < 0 )
			{
				gPlayer[i].SendMessage("MoveRight", SendMessageOptions.DontRequireReceiver);
			}
			
			if( GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.One  ).x > 0 )
			{
				gPlayer[i].SendMessage("MoveLeft", SendMessageOptions.DontRequireReceiver);
			}
			if( GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.One  ).y > 0 )
			{
				gPlayer[i].SendMessage("LookUp", SendMessageOptions.DontRequireReceiver);
			}
			if( GamePad.GetAxis(GamePad.Axis.LeftStick , GamePad.Index.One ).y < 0 )
			{
				gPlayer[i].SendMessage("Crouch", SendMessageOptions.DontRequireReceiver);
			}
			
			if( GamePad.GetButtonDown( strctPlayerInputs[i].bAttack , GamePad.Index.One ) )
			{
				gPlayer[i].SendMessage("Attack", SendMessageOptions.DontRequireReceiver);
			}
			
			
			if( GamePad.GetButtonDown( strctPlayerInputs[i].bJump , GamePad.Index.One ) )
			{
				gPlayer[i].SendMessage("Jump", SendMessageOptions.DontRequireReceiver);
			}
			
			if( GamePad.GetButtonUp( strctPlayerInputs[i].bJump , GamePad.Index.One) )
			{
				gPlayer[i].SendMessage("StoppedJumping", SendMessageOptions.DontRequireReceiver);
			}

		}
	}
}
