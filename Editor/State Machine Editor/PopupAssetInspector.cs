using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Shizounu.Library.AI;

namespace Shizounu.Library.Editor
{
    ///https://forum.unity.com/threads/custom-editor-open-new-inspector-window-and-show-asset.1167818/
    public class PopUpAssetInspector : EditorWindow
    {

        public UnityEngine.Object asset;
        public UnityEditor.Editor assetEditor;
        

        public static PopUpAssetInspector Create(UnityEngine.Object asset)
        {
            var window = CreateWindow<PopUpAssetInspector>("State Editor Window");
            window.asset = asset;
            window.assetEditor = UnityEditor.Editor.CreateEditor(asset);
            return window;
        }
        public void ChangeEditor(UnityEngine.Object _asset){
            asset = _asset;
            assetEditor = UnityEditor.Editor.CreateEditor(asset);
            GUI.changed = true;
        }
        private void OnGUI()
        {
            if(asset == null)
                return;
            GUI.enabled = false;
            asset = EditorGUILayout.ObjectField("Asset", asset, asset.GetType(), false);
            GUI.enabled = true;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            assetEditor.OnInspectorGUI();
            EditorGUILayout.EndVertical();
        }
    }
}