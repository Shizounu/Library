using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using Shizounu.Library.Editor.DialogueEditor.Elements;

namespace Shizounu.Library.Editor.DialogueEditor.Windows
{
    public class GraphSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        public void Initialize(DialogueGraphView graphView)
        {
            this.graphView = graphView;
            indentationIcon = new Texture2D(1, 1);
            indentationIcon.SetPixel(0, 0, Color.clear);
            indentationIcon.Apply();
        }
        private DialogueGraphView graphView;
        private Texture2D indentationIcon;

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> entries = new List<SearchTreeEntry>()
            {
                
                new SearchTreeGroupEntry(new GUIContent("Create Element")),
                new SearchTreeGroupEntry(new GUIContent("Dialogue Node"), 1),
                new SearchTreeEntry(new GUIContent("Sentence Node", indentationIcon))
                {
                    level = 2,
                    userData = NodeType.SentenceNode
                },
                new SearchTreeEntry(new GUIContent("Condition Node", indentationIcon))
                {
                    level = 2,
                    userData = NodeType.Condition
                },
                new SearchTreeEntry(new GUIContent("Information Node", indentationIcon))
                {
                    level = 2,
                    userData = NodeType.Information
                },
                new SearchTreeEntry(new GUIContent("Event Trigger Node", indentationIcon))
                {
                    level = 2,
                    userData = NodeType.EventTrigger
                },




                new SearchTreeGroupEntry(new GUIContent("Dialogue Group"), 1),
                new SearchTreeEntry(new GUIContent("Single Group", indentationIcon))
                {
                    level = 2,
                    userData = new Group()
                }
            };
            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Vector2 localPos = graphView.GetLocalMousePosition(context.screenMousePosition, true);
            switch (SearchTreeEntry.userData)
            {
                case NodeType.SentenceNode:
                    {
                        SentenceNode node = (SentenceNode)graphView.CreateNode(NodeType.SentenceNode, localPos);
                        graphView.AddElement(node);
                        return true;
                    }
                case NodeType.Condition:
                    {
                        ConditionalNode node = (ConditionalNode)graphView.CreateNode(NodeType.Condition, localPos);
                        graphView.AddElement(node);
                        return true; 
                    }
                case NodeType.Information:
                    {
                        InformationNode node = (InformationNode)graphView.CreateNode(NodeType.Information, localPos);
                        graphView.AddElement(node);
                        return true;
                    }

                case NodeType.EventTrigger:
                    {
                        EventTriggerNode node = (EventTriggerNode)graphView.CreateNode(NodeType.EventTrigger, localPos);
                        graphView.AddElement(node);
                        return true;
                    }
                case Group _:
                    {
                        Group group = (Group)graphView.CreateGroup("DialogueGroup", localPos);
                        graphView.AddElement(group);
                        return true;
                    }
                default: return false;
            }
        }
    }
}


