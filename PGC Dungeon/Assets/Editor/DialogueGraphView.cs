using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using PlasticPipe.PlasticProtocol.Messages;
using System.Linq;
using System.Net.Sockets;

public class DialogueGraphView : GraphView
{


    public readonly Vector2 DefaultNodeSize = new Vector2(200, 150);

    public DialogueGraphView()
    {


        styleSheets.Add(Resources.Load<StyleSheet>("UIGraphViewStyle"));  //loads the style sheets that changes the background

        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);


        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());      // inbuilt things from unity fur ux
        this.AddManipulator(new RectangleSelector());


        var grid = new GridBackground();   //sets up the grid and sizes it
        Insert(0, grid);
        grid.StretchToParentSize();

        AddElement(GenerateEntryPointNode());
        
    }


    private Port GeneratePort(DialogueNode node, Direction portDir, Port.Capacity capacity = Port.Capacity.Single) 
    {
        return node.InstantiatePort(Orientation.Horizontal, portDir, capacity,typeof(int));
    }




    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdpater) 
    {
        var compatiblePorts = new List<Port>();

        ports.ForEach((port) =>
        {
            if (startPort != port && startPort.node != port.node)
                compatiblePorts.Add(port);

        });



        return compatiblePorts;
    
    }


    private DialogueNode GenerateEntryPointNode()
    {

        var node = new DialogueNode() { title = "start",  GUID = Guid.NewGuid().ToString(), DialogueText = "EntryPoint", EntryPoint = true };

        var generatedPort = GeneratePort(node, Direction.Output);

        generatedPort.portName = "Next";

        node.outputContainer.Add(generatedPort);


        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(100, 200, 150, 200));
        

        return node;
    }


    public void CreateNode(string nodeName) 
    {
        AddElement(CreateDialogueNode(nodeName));
    }

    public DialogueNode CreateDialogueNode(string nodeName) 
    {
        var dialogueNode = new DialogueNode
        {
            title = nodeName,
            DialogueText = nodeName,
            GUID = Guid.NewGuid().ToString()
        };


        var inputPort = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);

        inputPort.portName = "Input";
        dialogueNode.inputContainer.Add(inputPort);


        var button = new Button(() => { AddChoicePort(dialogueNode); });

        button.text = "new choice";
        dialogueNode.titleContainer.Add(button);

        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();

        dialogueNode.SetPosition(new Rect(Vector2.zero, DefaultNodeSize));

        return dialogueNode;
    
    }




    public void AddChoicePort(DialogueNode dialogueNode, string overriddenPortName = "") 
    {
        var generatedPort = GeneratePort(dialogueNode, Direction.Output);
        var portLabel = generatedPort.contentContainer.Q<Label>("type");
        generatedPort.contentContainer.Remove(portLabel);

        var outputPortCount = dialogueNode.outputContainer.Query("connector").ToList().Count();
        var outputPortName = string.IsNullOrEmpty(overriddenPortName)
            ? $"Option {outputPortCount + 1}"
            : overriddenPortName;


        var textField = new TextField()
        {
            name = string.Empty,
            value = outputPortName
        };
        textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
        generatedPort.contentContainer.Add(new Label("  "));
        generatedPort.contentContainer.Add(textField);
        var deleteButton = new Button(() => RemovePort(dialogueNode, generatedPort))
        {
            text = "X"
        };
        generatedPort.contentContainer.Add(deleteButton);
        generatedPort.portName = outputPortName;
        dialogueNode.outputContainer.Add(generatedPort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();

    }


    private void RemovePort(DialogueNode dialogueNode, Port generatedPort) 
    {
        var targetEdge = edges.ToList()
                .Where(x => x.output.portName == generatedPort.portName && x.output.node == generatedPort.node);
        if (targetEdge.Any())
        {
            var edge = targetEdge.First();
            edge.input.Disconnect(edge);
            RemoveElement(targetEdge.First());
        }

        dialogueNode.outputContainer.Remove(generatedPort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }




}