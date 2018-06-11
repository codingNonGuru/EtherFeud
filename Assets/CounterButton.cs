using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using EtherGame.Networking;

public class CounterButton : MonoBehaviour 
{
	[SerializeField]
	Text countLabel = null;

	void Start()
	{
		EthereumModule.GetArmyCountResponse -= OnGetArmyCountResponded;
		EthereumModule.GetArmyCountResponse += OnGetArmyCountResponded;
	}

	public void Press()
	{
		EthereumModule.GetArmyCount();
	}

	void OnGetArmyCountResponded(GetArmyCountOutput output)
	{
		if(countLabel == null)
			return;

		countLabel.text = string.Format("Armies: {0}", output.ArmyCount);
	}
}
