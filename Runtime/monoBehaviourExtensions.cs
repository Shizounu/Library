using UnityEngine;

namespace Shizounu.Library
{
    public static class MonoBehaviourExtension
    {
        public static bool CanSee(this MonoBehaviour source, MonoBehaviour target, float maxDist = 2){                                                    // invert layermask
            RaycastHit raycastHit = new RaycastHit();
            if(!Physics.Raycast(source.transform.position, (target.transform.position - source.transform.position).normalized, out raycastHit, maxDist)){
                return false;
            }
            return (raycastHit.transform.gameObject == target.gameObject);
        }
    }


}
