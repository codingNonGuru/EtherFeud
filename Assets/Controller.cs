using System;
using System.Collections.Generic;

using UnityEngine;

namespace EtherGame
{
    public enum Actions
    {
        MOVE_FORWARD,
        MOVE_BACKWARDS,
        TURN_RIGHTWARDS,
        TURN_LEFTWARDS,
        SHOOT
    };

    public class Controller
    {
        protected List<Actions> actions = new List<Actions>();

        public void Refresh()
        {
            actions.Clear();

            OnRefresh();
        }

        public bool IsDoing(Actions otherAction)
        {
            foreach(var action in actions)
            {
                if(action == otherAction)
                    return true;
            }

            return false;
        }

        protected virtual void OnRefresh() {}
    }
}