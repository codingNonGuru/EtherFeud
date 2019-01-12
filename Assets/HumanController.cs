using System;
using System.Collections.Generic;

using UnityEngine;

namespace EtherGame
{
    public class HumanController : Controller
    {
        protected override void OnRefresh()
        {
            if(Input.GetKey(KeyCode.W))
            {
                actions.Add(Actions.MOVE_FORWARD);
            }
            else if(Input.GetKey(KeyCode.S))
            {
                actions.Add(Actions.MOVE_BACKWARDS);
            }
            
            if(Input.GetKey(KeyCode.A))
            {
                actions.Add(Actions.TURN_LEFTWARDS);
            }
            else if(Input.GetKey(KeyCode.D))
            {
                actions.Add(Actions.TURN_RIGHTWARDS);
            }

            if(Input.GetKey(KeyCode.Space))
            {
                actions.Add(Actions.SHOOT);
            }
        }
    }
}