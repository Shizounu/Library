using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

using Shizounu.Library.Dialogue.Data;
using Shizounu.Library.Editor.DialogueEditor.Utilities;

namespace Shizounu.Library.Editor.DialogueEditor.Windows
{
    public class DialogueEditorWindow : EditorWindow
    {
        private const string defaultEventName = "new Dialogue";
        private string curEventName = defaultEventName;
        private DialogueGraphView graphView;
        private DialogueData data;
        [MenuItem("Shizounu/Dialgoue Editor")]
        public static void Open()
        {
            GetWindow<DialogueEditorWindow>("Dialogue Editor");
        }

        private void OnEnable()
        {
            AddGraphView();
            AddToolbar();
            AddStyles();
        }

        private void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();

            toolbar.Add(ElementUtility.CreateTextField(curEventName, "FileName:", change => curEventName = change.newValue));
            toolbar.Add(ElementUtility.CreateButton("Save", () => DoSave()));

            toolbar.Add(ElementUtility.CreateSOField<DialogueData>("File to Load", null ,change => data = (DialogueData)change.newValue));
            toolbar.Add(ElementUtility.CreateButton("Load", () => DoLoad()));
            toolbar.AddStyleSheets("Packages/Library/Editor/Dialogue/Style Sheets/ToolbarStyle.uss");

            rootVisualElement.Add(toolbar);
        }
        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("Packages/Library/Editor/Dialogue/Style Sheets/Variables.uss");
        }

        private void AddGraphView()
        {
            graphView = new DialogueGraphView(this);

            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
        }
        private void DoSave()
        {
            SavingUtility.Save(curEventName, graphView);
        }
        private void DoLoad()
        {
            SavingUtility.Load(data, graphView);
        }
    }
}
