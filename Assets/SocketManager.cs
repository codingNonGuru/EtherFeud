using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using WebSocketSharp;

namespace EtherGame
{
    public class SocketManager : MonoBehaviour
    {
        public class EventHandler
        {
            public event Action<SocketEvent> action;

            public EventHandler(Action<SocketEvent> action)
            {
                this.action = action;
            }

            public void Fire(SocketEvent socketEvent)
            {
                if(action == null)
                    return;

                action.Invoke(socketEvent);
            }
        }

        static SocketManager instance = null;

        Dictionary<string, EventHandler> eventHandlers = new Dictionary<string, EventHandler>();

        WebSocket socket = null;

        string host = "127.0.0.1";

        int port = 8787;

        bool isConnected = false;

        void Awake()
        {
            if(instance == null)
                instance = this;
        }

        void Start()
        {
            Connect();
        }

        static public void Connect()
        {
            var address = string.Format("ws://{0}:{1}", instance.host, instance.port);

            var uri = new System.Uri(address);
            instance.socket = new WebSocket(uri);

            instance.StartCoroutine(instance.ConnnectCoroutine());
        }

        static public void RegisterHandler(string eventName, Action<SocketEvent> action)
        {
            EventHandler handler = null;
            instance.eventHandlers.TryGetValue(eventName, out handler);
            if(handler == null)
            {
                instance.eventHandlers.Add(eventName, new EventHandler(action));
            }
            else
            {
                handler.action -= action;
                handler.action += action;
            }
        }

        static public void SendMessage(SocketEvent socketEvent)
        {
            if(instance.socket == null)
                return;

            instance.socket.SendString(socketEvent.Data);
        }

        static public void Disconnect()
        {
            if(instance.socket == null)
                return;

            if(!instance.isConnected)
                return;

            instance.socket.Close();

            instance.isConnected = false;
        }

        IEnumerator ConnnectCoroutine()
        {
            yield return StartCoroutine(socket.Connect());

            if(!string.IsNullOrEmpty(socket.error))
            {
                Debug.Log("Socket connection error: " + socket.error);
                yield break;
            }

            isConnected = true;

            while(true)
            {
                if(!isConnected)
                {
                    yield break;
                }

                var rawData = socket.RecvString();

                if(!string.IsNullOrEmpty(rawData))
                {
                    var socketEvent = new SocketEvent(rawData);

                    EventHandler handler = null;
                    eventHandlers.TryGetValue(socketEvent.Name, out handler);
                    if(handler != null)
                    {
                        handler.Fire(socketEvent);
                    }

                    Debug.Log(rawData);
                }

                yield return 0;
            }
        }
    }
}