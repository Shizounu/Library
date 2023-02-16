using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shizounu.Library.AI;
using UnityEditor;

[CreateAssetMenu(fileName = "new Action_AddComponent", menuName = "Shizounu/AI/Action/Add Component")]
public class Action_AddComponent : Action
{
    [HideInInspector] public System.Type typeToAdd;

    [Tooltip("Only add the components if its unique")] public bool Unique;
    public override void Act(StateMachine stateMachine)
    {
        stateMachine.gameObject.AddComponent(typeToAdd);
    }
}
