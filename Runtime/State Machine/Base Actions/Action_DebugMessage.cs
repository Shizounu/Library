using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Shizounu.Library.AI;

[CreateAssetMenu(fileName = "new Action_DebugMessage", menuName = "Shizounu/AI/Action/Debug Message")]
public class Action_DebugMessage : Action
{
    [TextArea] public string LogMessage;
    public bool isWarning;
    public override void Act(StateMachine stateMachine)
    {
        if(isWarning)
            Debug.LogWarning(LogMessage);
        else 
            Debug.Log(LogMessage);
    }
}
