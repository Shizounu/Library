using UnityEngine;
using UnityEngine.AI;

namespace Shizounu.Library{
    public static class NavMeshPathExtension{
		public static float PathLength(this NavMeshPath path){
			float length = 0;
			for (int i = 1; i < path.corners.Length; i++){
				length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
			}
			return length;
		}   
    }
}
