using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shizounu.Library.InfluenceMaps
{
    using UnityEngine;
    
    [CreateAssetMenu(fileName = "new Faction", menuName = "Shizounu/Influence Maps/Faction", order = 0)]
    public class Faction : ScriptableObject {
        public Color factionColor;
        public int influenceRadius;
    }
}