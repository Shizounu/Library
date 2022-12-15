using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shizounu.Library.ScriptableArchitecture
{
    public interface IScriptableEventListener
    {
        ScriptableEvent scriptableEvent {get; set;}
        void Response();

    }
}