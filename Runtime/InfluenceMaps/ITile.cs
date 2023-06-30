using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shizounu.Library.InfluenceMaps
{
    public interface ITile
    {
        bool isPassable {get; set;}
        
        Faction owningFaction {get; set;}
    }
}
