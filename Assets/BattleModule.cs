using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using EtherGame.Networking;

namespace EtherGame
{
    [System.Serializable]
    public class PlayerJoinedMessage
    {
        public string key;
        public int positionX;
        public int positionZ;
    }

    [System.Serializable]
    public class CommandRefreshMessage
    {
        [System.Serializable]
        public class CommandData
        {
            public string key;
            public string[] commands;
        }

        public CommandData[] commandBuffer;
    }

    [System.Serializable]
    public class IssueCommandMessage
    {
        public string key;
        public string command;
    }

    public class BattleModule : MonoBehaviour
    {
        static public event Action<PlayerJoinedMessage> OnPlayerJoined;
        static public event Action<CommandRefreshMessage> OnCommandsRefreshed;

        void Start()
        {
            SocketManager.RegisterHandler("playerJoined", HandlePlayerJoined);
            SocketManager.RegisterHandler("commandsRefreshed", HandleCommandsRefreshed);
        }

        static public void IssueCommand(string command)
        {
            var messageObject = new IssueCommandMessage();

            messageObject.key = EthereumManager.AccountAddress;
            messageObject.command = command;

            var message = JsonUtility.ToJson(messageObject);

            var socketEvent = new SocketEvent("issueCommand", message);

            SocketManager.SendMessage(socketEvent);
        }

        void HandlePlayerJoined(SocketEvent socketEvent)
        {
            var messsage = JsonUtility.FromJson<PlayerJoinedMessage>(socketEvent.Message);

            if(OnPlayerJoined != null)
            {
                OnPlayerJoined.Invoke(messsage);
            }
        }

        void HandleCommandsRefreshed(SocketEvent socketEvent)
        {
            var messsage = JsonUtility.FromJson<CommandRefreshMessage>(socketEvent.Message);

            if(OnCommandsRefreshed != null)
            {
                OnCommandsRefreshed.Invoke(messsage);
            }
        }
    }
}