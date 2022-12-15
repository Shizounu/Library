using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Shizounu.Library.ScriptableArchitecture
{
    [CreateAssetMenu(fileName = "ScriptableEvent", menuName = "Shizounu/ScriptableArchitecture/ScriptableEvent", order = -1)]
    public class ScriptableEvent : ScriptableObject {
        private readonly List<IScriptableEventListener> eventListeners = new();

        public void Invoke(){
            for (int i = 0; i < eventListeners.Count; i++){
                eventListeners[i].EventResponse();
            }
        }

        public void RegisterListener(IScriptableEventListener listener){
            if(!eventListeners.Contains(listener)){
                eventListeners.Add(listener);
            }    
        }
        public void RemoveListener(IScriptableEventListener listener){
            if(eventListeners.Contains(listener)){
                eventListeners.Remove(listener);
            }    
        }

        public static ScriptableEvent operator + (ScriptableEvent thisEvent, IScriptableEventListener listener){
            thisEvent.RegisterListener(listener);
            return thisEvent;
        }
        public static ScriptableEvent operator - (ScriptableEvent thisEvent, IScriptableEventListener listener){
            thisEvent.RemoveListener(listener);
            return thisEvent;
        }
    }
}