using System.Text.RegularExpressions;

using UnityEngine;

namespace EtherGame
{
    public class SocketEvent
    {
        string name = null;

        string message = null;

        string data = null;

        public string Name 
        {
            get {return name;}
        }

        public string Message 
        {
            get {return message;}
        }

        public string Data
        {
            get {return data;}
        }

        public SocketEvent(string data)
        {
            this.data = data;

            var pattern = @"\[(.*), (\{.*\})\]";

            var match = Regex.Match(data, pattern);

            if(!match.Success)
            {
                Debug.Log("Event string is malformed.");
                return;
            }

            name = match.Groups[1].ToString();
            message = match.Groups[2].ToString();
        }

        public SocketEvent(string name, string message)
        {
            this.name = name;
            this.message = message;

            data = string.Format("[(0), (1)]", name, message);
        }
    }
}