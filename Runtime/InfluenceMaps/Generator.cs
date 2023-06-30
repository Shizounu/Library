using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shizounu.Library.InfluenceMaps
{
    public class Generator : MonoBehaviour
    {
        
        public Vector2Int mapScale;
        public List<Faction> Factions;
        public ITile[,] map; 


        public Dictionary<Faction, float[,]> GenerateInfluenceMap(){
            Dictionary<Faction, float[,]> influenceMaps = new();
            foreach (Faction faction in Factions)
                influenceMaps.Add(faction, GenerateInfluenceMap(faction));
            return influenceMaps;
        }  
        public float[,] GenerateInfluenceMap(Faction faction){
            float[,] influenceMap = new float[mapScale.x, mapScale.y];

            for (int x = 0; x < mapScale.x; x++){
                for (int y = 0; y < mapScale.y; y++){

                    if(map[x,y].owningFaction == faction){
                        //push to compute shader
                        for (int dx = -faction.influenceRadius; dx < faction.influenceRadius; dx++){
                            for (int dy = -faction.influenceRadius; dy < faction.influenceRadius; dx++){
                                int wholeDistance = Mathf.Abs(dx) + Mathf.Abs(dy);

                                if(wholeDistance <= faction.influenceRadius){
                                    int currentX = x + dx;
                                    int currentY = y + dy;

                                    if(isPointInBound(currentX, currentY)){
                                        float distancePercent = wholeDistance / (float)faction.influenceRadius;
                                        influenceMap[currentX, currentY] += 1.0f * distancePercent;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return influenceMap;
        }
        bool isPointInBound(int _x, int _y){
            return _x >= 0 && _x < mapScale.x && _y >= 0 && _y < mapScale.y; 
        }
    }
}

