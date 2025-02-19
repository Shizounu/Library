using UnityEngine;
using Shizounu.Library.ScriptableArchitecture;

namespace Shizounu.Library.Dialogue.Data
{
    public class Information : DialogueElement
    {
        public Blackboard Blackboard;
        public ComparisonOperator Operator;
        public string FactKey;
        public IntReference Value;
        public override bool CanEnter()
        {
            return true;
        }

        public override void OnEnter(DialogueManager manager)
        {
            if (!Blackboard.Facts.ContainsKey(FactKey))
            {
                Debug.LogError($"Key ({FactKey}) is not present in Blackboard {Blackboard.name}");
                throw new System.Exception($"Key ({FactKey}) is not present in Blackboard {Blackboard.name}");
            }

            switch (Operator)
            {
                case ComparisonOperator.Addition:
                    Blackboard.Facts[FactKey] += Value;
                    break;
                case ComparisonOperator.Subtraction:
                    Blackboard.Facts[FactKey] -= Value;
                    break;
                case ComparisonOperator.Set:
                    Blackboard.Facts[FactKey] = Value;
                    break;
                default:
                    throw new System.NotImplementedException();
            }
            manager.NodeHasCompleted = true;
        }
    }

    public enum ComparisonOperator
    {
        Addition,
        Subtraction,
        Set
    }
}
