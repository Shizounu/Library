using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shizounu.Library
{
    public static class ObjectExtension
    {
        public static T GetInterface<T>(System.Object obj) {
            return (obj is T t) ? t : default;
        }
    }
}
