using System;
using System.Numerics;

using UnityEngine;

using Nethereum.JsonRpc.UnityClient;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Hex.HexTypes;

namespace EtherGame.Networking
{
    [FunctionOutput]
    public class GetArmyCountOutput 
    {
        [Parameter ("uint256", "count", 1)]
        public System.Numerics.BigInteger ArmyCount { get; set; }
        [Parameter ("string", "name", 2)]
        public string ArmyName { get; set; }
    }

    public class EthereumModule : MonoBehaviour
    {
        static EthereumModule instance = null;

        public static event Action<GetArmyCountOutput> GetArmyCountResponse;
        public static event Action AddArmyResponse;

        Nethereum.Contracts.Function GetArmyCountFunction = null;
        Nethereum.Contracts.Function AddArmyFunction = null;

        void Start()
        {
            if(instance == null)
            {
                instance = this;
            }

            GetArmyCountFunction = EthereumManager.Contract.GetFunction("GetArmyCount");
            AddArmyFunction = EthereumManager.Contract.GetFunction("AddArmy");
        }

        public static void GetArmyCount()
        {
            EthereumManager.SendRequest(instance.GetArmyCountFunction.CreateCallInput(), OnGetArmyCountDone);
        }

        public static void AddArmy()
        {
            var function = instance.AddArmyFunction;

            var gas = new HexBigInteger(300000);
            var gasPrice = new HexBigInteger(1);
            var value = new HexBigInteger(0);

			var transactionInput = function.CreateTransactionInput(EthereumManager.AccountAddress, gas, value);

            EthereumManager.PrepareTransaction(transactionInput, OnAddArmyDone);
        }

        static void OnGetArmyCountDone(UnityRequest<string> response)
        {
            if (string.IsNullOrEmpty (response.Result)) 
                return;

            var function = instance.GetArmyCountFunction;                

            var output = function.DecodeDTOTypeOutput<GetArmyCountOutput>(response.Result);

            Debug.Log("OnGetArmyCountDone Response: " + response.Result);

            if(GetArmyCountResponse != null)
            {
                GetArmyCountResponse.Invoke(output);
            }
        }

        static void OnAddArmyDone(UnityRequest<string> response)
        {
            if (response.Exception != null) 
            {
                Debug.Log("Exception: " + response.Exception.Message);
                return;
            }

            Debug.Log("OnAddArmyDone Response: " + response.Result);

            if(AddArmyResponse != null)
            {
                AddArmyResponse.Invoke();
            }
        }
    }
}