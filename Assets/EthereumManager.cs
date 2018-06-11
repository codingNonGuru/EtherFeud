using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.Encoders;
using Nethereum.RPC.Eth.Transactions;

namespace EtherGame.Networking
{
	public class TransactionData
	{
		public HexBigInteger Gas {get; set;}
		public HexBigInteger GasPrice {get; set;}
		public HexBigInteger Value {get; set;}
	}

	public class EthereumManager : MonoBehaviour 
	{
		static EthereumManager instance = null;

		string applicationBinaryInterface = System.IO.File.ReadAllText("Assets/Abi.txt");

		string contractAddress = "0x6fC9a76F0D4Dd5FC9cC582307fe69A854E390c37";

		string accountAddress = "0x7500cec88057819c123f6ba26cb8e1fd405cdb06";

		string privateKey = "b5b8e5169794acde9d92403a95ba7c06f48c8670e21e005f5c64af69920ca22e";

		Contract contract;

		string requestNode = "https://rinkeby.infura.io";

		List<string> transactionHashes = new List<string>();

		float transactionCheckTimer = 0.0f;

		const float transactionCheckInterval = 3.0f;

		public static Contract Contract
		{
			get {return instance.contract;}
		}

		public static string AccountAddress
		{
			get {return instance.accountAddress;}
		}

		void Awake () 
		{
			if(instance == null)
			{
				instance = this;
			}

			contract = new Nethereum.Contracts.Contract(null, applicationBinaryInterface, contractAddress);
		}

		void Update ()
		{
			transactionCheckTimer += Time.deltaTime;

			if(transactionCheckTimer < transactionCheckInterval)
				return;

			transactionCheckTimer = 0.0f;

			foreach(var hash in transactionHashes)
			{
				StartCoroutine(TransactionCheckCoroutine(hash));
			}
		}

		public static void SendRequest(CallInput requestParameters, System.Action<UnityRequest<string>> requestCallback)
		{
			instance.StartCoroutine(instance.SendRequestCoroutine(requestParameters, requestCallback));
		}

		public static void PrepareTransaction(TransactionInput transactionInput, System.Action<UnityRequest<string>> requestCallback)
		{
			instance.StartCoroutine(instance.SendTransactionCoroutine(transactionInput, requestCallback));
		}

		static void SendTransaction(TransactionInput requestParameters, System.Action<UnityRequest<string>> requestCallback)
		{
			instance.StartCoroutine(instance.SendTransactionCoroutine(requestParameters, requestCallback));
		}

		IEnumerator SendRequestCoroutine(CallInput requestParameters, System.Action<UnityRequest<string>> requestCallback)
		{
			var request = new EthCallUnityRequest(requestNode);

			yield return request.SendRequest(requestParameters, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());

			requestCallback.Invoke(request);
		}

		IEnumerator SendTransactionCoroutine(TransactionInput requestParameters, System.Action<UnityRequest<string>> requestCallback)
		{
			var request = new TransactionSignedUnityRequest(requestNode, privateKey, accountAddress);

			yield return request.SignAndSendTransaction(requestParameters);

			requestCallback.Invoke(request);

			transactionHashes.Add(request.Result);
		}

		IEnumerator TransactionCheckCoroutine(string hash)
		{
			var request = new EthGetTransactionByHashUnityRequest(requestNode);

			yield return request.SendRequest(hash);

			if(request.Result.BlockNumber != null)
			{
				transactionHashes.Remove(hash);
			}
		}
	}
}