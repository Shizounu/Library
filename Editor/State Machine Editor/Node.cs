using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Shizounu.Library.AI;

namespace Shizounu.Library.Editor.StateMachineEditor
{
    public class Node
    {
        public State nodeState;


        public Rect rect;
        public bool isDragged;
        public bool isSelected;
        public ConnectionPoint inPoint;
        public ConnectionPoint outPoint;
        
        public GUIStyle currentStyle;

        public GUIStyle Style_CachedDefault;
        public GUIStyle Style_CachedSelected;

        private System.Action<Node> removeNode;
        private System.Action<Node> popupAssetInspector;

        public Node(Vector2 position, float width, float height, NodeBasedEditor editor)
        {
            
            rect = new Rect(position.x, position.y, width * 1.5f, height * 1.5f);
            inPoint = new ConnectionPoint(this, ConnectionPointType.In, editor.Style_Inpoint, editor.OnClickInPoint);
            outPoint = new ConnectionPoint(this, ConnectionPointType.Out, editor.Style_Outpoint, editor.OnClickOutPoint);

            currentStyle = editor.Style_Node;
            Style_CachedDefault = editor.Style_Node;
            Style_CachedSelected = editor.Style_SelectedNode;

            removeNode = editor.OnClickRemoveNode;
            popupAssetInspector = editor.doAssetInspector;
        }

        public void Drag(Vector2 delta)
        {
            rect.position += delta;
        }

        public void Draw()
        {
            inPoint.Draw();
            outPoint.Draw();
            GUI.Box(rect, "", currentStyle);

            ///TODO: MAKE PRETTY
            Rect stateRect = new Rect(rect.position + rect.size * 0.5f - rect.size * 0.35f, rect.size * 0.7f);
            nodeState = (State)EditorGUI.ObjectField(stateRect, nodeState, typeof(State), false);
        }

        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (rect.Contains(e.mousePosition))
                        {
                            isDragged = true;
                            GUI.changed = true;
                            isSelected = true;
                            currentStyle = Style_CachedSelected;
                            popupAssetInspector(this);
                            
                        }
                        else
                        {
                            GUI.changed = true;
                            isSelected = false;
                            currentStyle = Style_CachedDefault;
                        }
                    }

                    if (e.button == 1 && isSelected && rect.Contains(e.mousePosition))
                    {
                        ProcessContextMenu();
                        e.Use();
                    }
                    break;

                case EventType.MouseUp:
                    isDragged = false;
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && isDragged)
                    {
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }
                    break;
            }
            return false;
        }

        private void ProcessContextMenu()
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
            genericMenu.ShowAsContext();
        }

        private void OnClickRemoveNode()
        {
            removeNode(this);
        }
    }
}