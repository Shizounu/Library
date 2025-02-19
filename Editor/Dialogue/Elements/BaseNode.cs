using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

using Shizounu.Library.Dialogue.Data;
using Shizounu.Library.Editor.DialogueEditor.Windows;
using Shizounu.Library.Editor.DialogueEditor.Utilities;
namespace Shizounu.Library.Editor.DialogueEditor.Elements 
{
    public abstract class BaseNode : Node
    {
        public string UID;

        public string SlideName;
        public List<PriorityPort> BranchPorts = new();
        public Port inputPort;
        protected DialogueGraphView graphView;

        public virtual void Initialize(Vector2 position, DialogueGraphView graphView)
        {
            SlideName = "SlideName";
            BranchPorts = new();
            SetPosition(new Rect(position, Vector2.zero));

            CreatePriorityPort();

            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");

            this.graphView = graphView;
        }

        public virtual void Draw()
        {
            MakeTitle();

            MakeMain();

            MakeInput();

            MakeOutput();

            MakeExtension();

            //Redraws visuals
            RefreshExpandedState();
        }

        #region Section Constructors
        protected virtual void MakeTitle()
        {
            TextField slideNameTextField = ElementUtility.CreateTextField(SlideName);
            titleContainer.Insert(0, slideNameTextField);

            slideNameTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__text-field__hidden",
                "ds-node__filename-text-field"
            );
        }

        protected virtual void MakeInput()
        {
            Port inputPort = this.CreatePort("Incoming", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            inputContainer.Add(inputPort);
        }
        protected virtual void MakeMain() { }
        protected virtual void MakeOutput() { }
        protected virtual void MakeExtension() { }

        public abstract DialogueElement GetElement();

        public PriorityPort GetPortWithPriority(int prio)
        {
            bool containsPrio = BranchPorts.Any(ctx => ctx.priority == prio);
            if (containsPrio) {
                return BranchPorts.Find(ctx => ctx.priority == prio);
            } else {
                return CreatePriorityPort(prio);
            }
        }

        protected PriorityPort CreatePriorityPort(int basePriority = 0)
        {
            Port choicePort = this.CreatePort("", Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);
            PriorityPort prioPort = new(basePriority, choicePort);

            Button deleteChoiceButton = ElementUtility.CreateButton("X", () => {
                if (choicePort.connected)
                    graphView.DeleteElements(choicePort.connections);


                BranchPorts.Remove(prioPort);
                graphView.RemoveElement(choicePort);
            });
            deleteChoiceButton.AddToClassList("ds-node__button");

            IntegerField choiceTextField = ElementUtility.CreateIntField(prioPort.priority, null, ctx => prioPort.priority = ctx.newValue);
            choiceTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__text-field__hidden",
                "ds-node__choice-text-field"
            );
            choicePort.Add(choiceTextField);
            choicePort.Add(deleteChoiceButton);

            BranchPorts.Add(prioPort);
            outputContainer.Add(choicePort);
            return prioPort;
        }

        public abstract void LoadData(DialogueElement element);
        #endregion
    }

    public class PriorityPort {
        public PriorityPort(int priority, Port port) {
            this.priority = priority;
            this.port = port;
        }
        public int priority;
        public Port port;
    }
    
}
