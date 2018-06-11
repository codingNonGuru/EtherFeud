using System;
using System.Collections.Generic;

using UnityEngine;

namespace EtherGame
{
    public enum FighterActions
    {
        MOVE_FORWARD,
        TURN_RIGHTWARDS,
        TURN_LEFTWARDS,
        SHOOT
    };

    public class Controller
    {
        List<FighterActions> actions = new List<FighterActions>();

        public void Refresh()
        {
            actions.Clear();

            if(Input.GetKey(KeyCode.W))
            {
                actions.Add(FighterActions.MOVE_FORWARD);
            }
            
            if(Input.GetKey(KeyCode.A))
            {
                actions.Add(FighterActions.TURN_LEFTWARDS);
            }
            else if(Input.GetKey(KeyCode.D))
            {
                actions.Add(FighterActions.TURN_RIGHTWARDS);
            }

            if(Input.GetKey(KeyCode.Space))
            {
                actions.Add(FighterActions.SHOOT);
            }
        }

        public bool IsDoing(FighterActions otherAction)
        {
            foreach(var action in actions)
            {
                if(action == otherAction)
                    return true;
            }

            return false;
        }
    }
}