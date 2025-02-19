using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;

using Shizounu.Library.Dialogue.Data;
using Shizounu.Library.Editor.DialogueEditor.Elements;
using Shizounu.Library.Editor.DialogueEditor.Utilities;

namespace Shizounu.Library.Editor.DialogueEditor.Windows
{
    public class DialogueGraphView : GraphView {
        
        private DialogueEditorWindow editorWindow;
        private GraphSearchWindow searchWindow;
        public EntryNode entryNode;
        public Dictionary<string, BaseNode> NodeCache;
        public List<Group> groups;

        public DialogueGraphView(DialogueEditorWindow editorWindow) {
            this.editorWindow = editorWindow;
            graphViewChanged += ctx => RemoveDeletedNodes(ctx);
            Init();
        }
        public void Init()
        {
            NodeCache = new();
            groups = new();

            AddManipulators();
            AddGridBackground();
            AddStyles();
            AddSearchWindow();

            AddStartNode();
        }

        private void AddStartNode()
        {
            entryNode = (EntryNode)CreateNode(NodeType.StartNode, new Vector2(100, 300));
            AddElement(entryNode);
            //AddElement(CreateNode(NodeType.ExitNode, new Vector2(500, 300)));
        }

        #region Context Menu
        private void AddSearchWindow()
        {
            if (searchWindow == null)
                searchWindow = ScriptableObject.CreateInstance<GraphSearchWindow>();
            searchWindow.Initialize(this);
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
        }
        private void AddManipulators()
        {
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger()); //needs to be before selector... for some reason
            this.AddManipulator(new RectangleSelector());

            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            
            this.AddManipulator(CreateContextualMenu("Add Slide", NodeType.SentenceNode));
            this.AddManipulator(CreateGroupContextualMenu());
        }
        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new(
                menuEvent => menuEvent.menu.AppendAction(
                    "Add Group",
                    actionEvent => AddElement(CreateGroup("Dialogue Group", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))
                ));

            return contextualMenuManipulator;
        }

        private IManipulator CreateContextualMenu(string actionTitle, NodeType type)
        {
            ContextualMenuManipulator contextualMenuManipulator = new(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(type, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
                );

            return contextualMenuManipulator;
        }


        #endregion

        #region Create Element Functions 
        public GraphElement CreateGroup(string title, Vector2 localMousePosition)
        {
            Group group = new()
            {
                title = title
            };
            group.SetPosition(new Rect(localMousePosition, Vector2.zero));

            foreach (GraphElement selectedElement in selection)
            {
                if (selectedElement is BaseNode)
                {
                    BaseNode node = selectedElement as BaseNode;
                    group.AddElement(node);
                }
            }


            groups.Add(group);
            return group;
        }

        public BaseNode CreateNode(NodeType type, Vector2 pos, DialogueElement elementToLoad = null) {
            BaseNode node = GetNode(type);
            node.Initialize(pos, this);
            if (elementToLoad != null)
                node.LoadData(elementToLoad);
            node.Draw();

            if (elementToLoad == null)
                node.UID = DialogueData.GetID();
            else
                node.UID = elementToLoad.ID;
            NodeCache.Add(node.UID, node);

            return node;
        }

        private void AddGridBackground()
        {
            GridBackground grid = new GridBackground();

            grid.StretchToParentSize(); //sets the size
            Insert(0, grid); //adds it to the graph view
        }
        #endregion

        #region Helpers
        private BaseNode GetNode(NodeType type)
        {
            switch (type)
            {
                case NodeType.StartNode: 
                    return new EntryNode();
                case NodeType.SentenceNode:
                    return new SentenceNode();
                case NodeType.Condition:
                    return new ConditionalNode();
                case NodeType.Information:
                    return new InformationNode();
                case NodeType.EventTrigger:
                    return new EventTriggerNode();
                default: return null;
            }
        }

        private void AddStyles()
        {
            this.AddStyleSheets(
                "Packages/Library/Editor/Dialogue/Style Sheets/ViewStyles.uss",
                "Packages/Library/Editor/Dialogue/Style Sheets/ViewStyles.uss"
            );
        }


        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new();

            ports.ForEach(port => {
                if (port.direction != startPort.direction)
                {
                    compatiblePorts.Add(port);
                }
            });

            return compatiblePorts;
        }

        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;
            if (searchWindow)
            {
                worldMousePosition -= editorWindow.position.position;
            }

            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);
            return localMousePosition;
        }

        public GraphViewChange RemoveDeletedNodes(GraphViewChange change)
        {
            if (change.elementsToRemove == null)
                return change;
            foreach (var item in change.elementsToRemove) {
                if(item is BaseNode node) {
                    NodeCache.Remove(node.UID);
                }
                if(item is Group group) {
                    groups.Remove(group);
                }
            }

            return change; 
        }

        #endregion
    }
}
