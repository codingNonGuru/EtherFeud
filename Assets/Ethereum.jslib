//edge mountain nice ticket maple screen must fiction zero forest gravity brisk

var EthereumPlugin = 
{
  	Initialize: function () 
  	{
		if (typeof web3 !== 'undefined') 
		{
            web3 = new Web3(web3.currentProvider);
        } 
		else 
		{
            web3 = new Web3(new Web3.providers.HttpProvider("http://localhost:8545"));
        }

		contractAddress = "0xd45f58ab5055b2cbfe14a738e9aa265f530615db";

		var abi = [
			{
				"constant": false,
				"inputs": [],
				"name": "AddArmy",
				"outputs": [],
				"payable": false,
				"stateMutability": "nonpayable",
				"type": "function"
			},
			{
				"constant": true,
				"inputs": [],
				"name": "GetArmyCount",
				"outputs": [
					{
						"name": "",
						"type": "uint256"
					}
				],
				"payable": false,
				"stateMutability": "view",
				"type": "function"
			}
		];

		contract = web3.eth.contract(abi);

		instance = contract.at(contractAddress);

		message = "ALOHAAAAAAAAAAAAAAA",

		console.log("CONTRACT ADDRESS!!!!!: " + contractAddress);
  	},

	GetArmyCount: function()
	{
		var callData = 
		{
			from: web3.eth.accounts[0],
			to: contractAddress
		};

		console.log("MESSAGE 2: " + message);

		instance.GetArmyCount.call(
			callData, 
			function(error, hash) 
			{
				console.log(hash.c[0]);
			}
		);
	},

	AddArmy: function()
	{
		var callData = 
		{
			from: web3.eth.accounts[0],
			to: contractAddress
		};

		instance.AddArmy.sendTransaction(
			callData,
			function(error, hash)
			{
				console.log("Army was added!");
			}
		);
	}
};

mergeInto(LibraryManager.library, EthereumPlugin);