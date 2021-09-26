using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Shizounu.Library.Editor
{

    [CustomPropertyDrawer(typeof(Shizounu.Library.Editor.ReadOnlyAttribute))]
    public class ReadOnlyPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = true;
        }
    }
}
