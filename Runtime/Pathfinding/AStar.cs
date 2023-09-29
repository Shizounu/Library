using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shizounu.Library.AI
{
    public static class AStar{
        public static List<IAStarTile> generatePath(List<IAStarTile> tiles, IAStarTile startTile, IAStarTile goalTile)
        {
            //Outer reached edge
            PriorityQueue<IAStarTile> frontier = new PriorityQueue<IAStarTile>();
            frontier.Enqueue(startTile, 0);

            //From which tile this tile has been reached
            Dictionary<IAStarTile, IAStarTile> cameFrom = new();
            cameFrom[startTile] = null;

            //which cost you need to reach this tile
            Dictionary<IAStarTile, float> costSoFar = new();
            costSoFar[startTile] = 0;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();
                if (current == goalTile)
                    break;

                foreach (var next in current.Adjacencies)
                {
                    float newCost = costSoFar[current] + next.TraversalCost;
                    if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        frontier.Enqueue(next, newCost + next.Heuristic(goalTile));
                        cameFrom[next] = current;
                    }
                }
            }

            List<IAStarTile> Path = new();
            var cur = goalTile;
            while (cur != startTile)
            {
                Path.Add(cur);
                cur = cameFrom[cur];
            }
            Path.Add(startTile);
            Path.Reverse();

            return Path;
        }
    }

    public interface IAStarTile{
        Transform transform {get;}
        float TraversalCost {get; set;}
        List<IAStarTile> Adjacencies {get; set;}
        float Heuristic(IAStarTile goal);
    }
    
}
