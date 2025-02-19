using System.Collections.Generic;
using UnityEngine;
using Shizounu.Library.Utility;

namespace Shizounu.Library.Dialogue.Data
{
    [CreateAssetMenu(fileName = "new Blackboard", menuName = "Dialogue/Blackboard")]
    public class Blackboard : ScriptableObject
    {
        public SerializableDictionary<string, int> Facts = new();
    }
}
