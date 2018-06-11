using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using EtherGame.Networking;

public class AddButton : MonoBehaviour 
{
	public void Press()
	{
		EthereumModule.AddArmy();
	}
	
}
