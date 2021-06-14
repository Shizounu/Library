using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shizounu.Library
{
    public static class GameObjectExtension
    {
        public static bool HasComponent<T>(this GameObject obj) where T : Component
        {
            return obj.GetComponent<T>() != null;
        }
    }
}