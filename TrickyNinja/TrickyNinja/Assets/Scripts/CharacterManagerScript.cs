using UnityEngine;
using System.Collections;

public class CharacterManagerScript : MonoBehaviour {

	public GameObject[] agPlayers;
	public int iCorporealPlayer = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//checks for a new corporeal player to be selected then turns them solid and everyone else not
	void ChangeCorporeal(int a_iNewCorporeal)
	{
		//if a new corporeal player was selected
		if(a_iNewCorporeal != iCorporealPlayer)
		{
			for(int i = 0; i < agPlayers.Length; i++ )
			{
				//if new corporeal player
				if(i == a_iNewCorporeal-1)
				{
					//set player corporeal
				}
				else
				{
					//set incorporeal
				}
				
			}
		}
	}
}
