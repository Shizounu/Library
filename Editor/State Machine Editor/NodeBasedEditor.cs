using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Shizounu.Library.AI;

namespace Shizounu.Library.Editor.StateMachineEditor
{
    ///https://gram.gs/gramlog/creating-node-based-editor-unity/
    public class NodeBasedEditor : EditorWindow
    {
        public PopUpAssetInspector popUpAssetInspector;
        public List<Node> nodes;
        public List<Connection> connections;

        //Styles
        public GUIStyle Style_Node;
        public GUIStyle Style_SelectedNode;
        public GUIStyle Style_Inpoint;
        public GUIStyle Style_Outpoint;


        //Interacting with the Editor
        private ConnectionPoint selectedInPoint;
        private ConnectionPoint selectedOutPoint;

        private Vector2 offset;
        private Vector2 drag;

        [MenuItem("Shizounu/State Machine Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<NodeBasedEditor>();
            window.titleContent = new GUIContent("NodeBased");
            window.Show();
        }
        private void OnEnable()
        {
            if(nodes == null)
                nodes = new();
            if(connections == null)
                connections = new();

            //Preparing Styles
            Style_Node = new GUIStyle();
            Style_Node.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
            Style_Node.border = new RectOffset(12, 12, 12, 12);

            Style_SelectedNode = new GUIStyle();
            Style_SelectedNode.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
            Style_SelectedNode.border = new RectOffset(12, 12, 12, 12);

            Style_Inpoint = new GUIStyle();
            Style_Inpoint.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
            Style_Inpoint.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
            Style_Inpoint.border = new RectOffset(4, 4, 12, 12);

            Style_Outpoint = new GUIStyle();
            Style_Outpoint.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
            Style_Outpoint.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
            Style_Outpoint.border = new RectOffset(4, 4, 12, 12);
        }     
        private void OnGUI()
        {

            DrawGrid(20, 0.2f, Color.gray);
            DrawGrid(100, 0.4f, Color.gray);

            DrawNodes();
            DrawConnections();

            DrawConnectionLine(Event.current);

            ProcessNodeEvents(Event.current);
            ProcessEvents(Event.current);

            if (GUI.changed) Repaint();
        }



        #region Editing Functions

        //Nodes
        public void OnDrag(Vector2 delta)
        {
            drag = delta;

            if (nodes != null)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].Drag(delta);
                }
            }

            GUI.changed = true; //raise flag for repainting canvas
        }

        public void OnClickAddNode(Vector2 mousePosition)
        {
            if (nodes == null)
                nodes = new List<Node>();
            nodes.Add(new Node(mousePosition, 200, 50, this));
        }

        public void OnClickRemoveNode(Node node)
        {
            if (connections != null)
            {
                List<Connection> connectionsToRemove = new List<Connection>();

                for (int i = 0; i < connections.Count; i++)
                    if (connections[i].inPoint == node.inPoint || connections[i].outPoint == node.outPoint)
                        connectionsToRemove.Add(connections[i]);

                for (int i = 0; i < connectionsToRemove.Count; i++)
                    connections.Remove(connectionsToRemove[i]);

                connectionsToRemove = null;
            }

            nodes.Remove(node);
        }

        //Connections
        public void CreateConnection()
        {
            if (connections == null)
                connections = new List<Connection>();

            connections.Add(new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
            selectedInPoint.node.nodeState.transitions.Add(new Transition(selectedOutPoint.node.nodeState)); //adds the transition to the underlying scriptable object
        }

        public void ClearConnectionSelection()
        {
            selectedInPoint = null;
            selectedOutPoint = null;
        }

        public void OnClickInPoint(ConnectionPoint inPoint)
        {
            selectedInPoint = inPoint;

            if (selectedOutPoint != null)
            {
                if (selectedOutPoint.node != selectedInPoint.node)
                {
                    CreateConnection();
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }

        public void OnClickOutPoint(ConnectionPoint outPoint)
        {
            selectedOutPoint = outPoint;

            if (selectedInPoint != null)
            {
                if (selectedOutPoint.node != selectedInPoint.node)
                {
                    CreateConnection();
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }

        public void OnClickRemoveConnection(Connection connection)
        {
            connections.Remove(connection);
            foreach (var item in connection.inPoint.node.nodeState.transitions)
                if (item.transitionState == connection.outPoint.node.nodeState)
                    connection.inPoint.node.nodeState.transitions.Remove(item);

            //copies list and removes all the conenctions with the node I am removing a connection from
            List<Transition> transitionsCopy = connection.inPoint.node.nodeState.transitions;
            for (int i = transitionsCopy.Count; i > 0; i--)
                if (transitionsCopy[i].transitionState == connection.outPoint.node.nodeState)
                    connection.inPoint.node.nodeState.transitions.Remove(transitionsCopy[i]);
            connection.inPoint.node.nodeState.transitions = transitionsCopy;
        }

        public void doAssetInspector(Node node){
            if(popUpAssetInspector == null){
                popUpAssetInspector = PopUpAssetInspector.Create(node.nodeState);
                return;
            }

            if(popUpAssetInspector.asset != node.nodeState)
                popUpAssetInspector.ChangeEditor(node.nodeState);
        }
        #endregion

        #region Process Functions
        void ProcessEvents(Event e)
        {
            drag = Vector2.zero;
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 1)
                    {
                        ProcessContextMenu(e.mousePosition);
                        ClearConnectionSelection();
                    }
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0)
                    {
                        OnDrag(e.delta);
                    }
                    break;
            }
        }
        private void ProcessNodeEvents(Event e)
        {
            if (nodes != null)
            {
                for (int i = nodes.Count - 1; i >= 0; i--)
                {
                    bool guiChanged = nodes[i].ProcessEvents(e);

                    if (guiChanged){
                        GUI.changed = true;
                    }
                }
            }
        }
        private void ProcessContextMenu(Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition));
            genericMenu.ShowAsContext();
        }

        #endregion

        #region Draw Functions
        private void DrawConnectionLine(Event e)
        {
            if (selectedInPoint != null && selectedOutPoint == null)
            {
                Handles.DrawBezier(
                    selectedInPoint.rect.center,
                    e.mousePosition,
                    selectedInPoint.rect.center + Vector2.left * 50f,
                    e.mousePosition - Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );

                GUI.changed = true;
            }

            if (selectedOutPoint != null && selectedInPoint == null)
            {
                Handles.DrawBezier(
                    selectedOutPoint.rect.center,
                    e.mousePosition,
                    selectedOutPoint.rect.center - Vector2.left * 50f,
                    e.mousePosition + Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );

                GUI.changed = true;
            }
        }

        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            offset += drag * 0.5f;
            Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
            }

            for (int j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        void DrawNodes()
        {
            foreach (Node node in nodes)
                node.Draw();
        }
        private void DrawConnections()
        {
            foreach (Connection connection in connections)
                connection.Draw();
        }
        #endregion
    }
}