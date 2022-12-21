using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class DialogueGraph : EditorWindow
{

    private DialogueGraphView _graphView;

    private string _fileName = "New Narrative";



    [MenuItem("Graph/Dialogue Graph")]
    public static void OpenDialogueGraphWindow() 
    {
        var window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent("DIalogue Graph");
    }


    private void OnEnable()
    {
        ConstructGraph();
        GenerateToolBar();
    }


    private void GenerateToolBar()
    {
        var toolbar = new Toolbar();


        var fileNameTextField = new TextField("File name: ");
        fileNameTextField.SetValueWithoutNotify(_fileName);

        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);

        toolbar.Add(fileNameTextField);


        toolbar.Add(new Button(() => SaveData()) { text = "Save Data" });
        toolbar.Add(new Button(() => LoadData()) { text = "Load Data" });

        var nodeCreationButton = new Button(() => { _graphView.CreateNode("Dialogue Node"); });

        nodeCreationButton.text = "Create Node";

        toolbar.Add(nodeCreationButton);

        rootVisualElement.Add(toolbar);


    }



    private void ConstructGraph()
    {
        _graphView = new DialogueGraphView { name = "Dialogue Graph" };

        _graphView.StretchToParentSize();

        rootVisualElement.Add(_graphView);
    }


    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }










    private void SaveData() 
    {
        if (string.IsNullOrEmpty(_fileName)) 
        {
            EditorUtility.DisplayDialog("invalid filename", "Please enter somethign valid", "OK");
            return;
        }


        var saveUtility = GraphSaveUtility.GetInstance(_graphView);

        saveUtility.SaveGraph(_fileName);
    
    }


    private void LoadData()
    {

        if (string.IsNullOrEmpty(_fileName))
        {
            EditorUtility.DisplayDialog("invalid filename", "Please enter somethign valid", "OK");
            return;
        }


        var saveUtility = GraphSaveUtility.GetInstance(_graphView);

        saveUtility.LoadGraph(_fileName);

    }




}
