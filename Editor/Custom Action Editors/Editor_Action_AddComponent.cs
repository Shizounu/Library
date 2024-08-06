using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Shizounu.Library.Editor.StateMachineEditor
{
    [CustomEditor(typeof(Action_AddComponent))]
    public class Editor_Action_AddComponent : UnityEditor.Editor {
        private void DrawDropdown(Action_AddComponent so){
            TypeCache.TypeCollection collection = TypeCache.GetTypesDerivedFrom<Component>();
        
            List<string> names = new();
            foreach (System.Type type in collection)
                names.Add(type.ToString());

            int index = (so.typeToAdd != null ? collection.IndexOf(so.typeToAdd) : 0); 
            int newIndex = EditorGUILayout.Popup(index,names.ToArray());
            so.typeToAdd = collection[newIndex];
        }
        public override void OnInspectorGUI() {
            Action_AddComponent SO = target as Action_AddComponent; 
            DrawDropdown(SO);
        
            base.OnInspectorGUI();
        }
    }

}
