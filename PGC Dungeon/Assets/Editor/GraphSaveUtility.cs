using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphSaveUtility 
{


    private DialogueGraphView _targetGraphView;
    private DialogueContainer _containerCache;

    private List<Edge> edges => _targetGraphView.edges.ToList();
    private List<DialogueNode> nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();


    public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView) 
    {
        return new GraphSaveUtility { _targetGraphView = targetGraphView };
    }




    // th sizing and the deletiong o the ports dont work


    public void SaveGraph(string fileName) 
    {
        if (!edges.Any())
            return;



        var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();

        var connectedPorts = edges.Where(x => x.input.node != null).ToArray();

        for (int i = 0; i < connectedPorts.Length; i++)
        {
            var outputNode = connectedPorts[i].output.node as DialogueNode;
            var inputNode = connectedPorts[i].input.node as DialogueNode;

            dialogueContainer.nodeLinks.Add(new NodeLinkData
            {
                BaseNodeGuid = outputNode.GUID,
                PortName = connectedPorts[i].output.portName,
                TargetNodeGuid = inputNode.GUID
            });
        }




        foreach (var nodes in nodes.Where(node=> !node.EntryPoint))
        {
            dialogueContainer.dialogueNodeData.Add(new DialogueNodeData
            {
                NodeGUID = nodes.GUID,
                DialogueText = nodes.DialogueText,
                Position = nodes.GetPosition().position
            });
        }



        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");


        AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Resources/{fileName}.asset");
        AssetDatabase.SaveAssets();


    }


    public void LoadGraph(string fileName) 
    {
        _containerCache = Resources.Load<DialogueContainer>(fileName);


        if (_containerCache == null)
        {
            EditorUtility.DisplayDialog("File not found", "target file not found", "ok");
            return;
        }



        ClearGraph();
        CreateNodes();
        ConnectNodes();

    }



    private void ClearGraph() 
    {
        nodes.Find(x => x.EntryPoint).GUID = _containerCache.nodeLinks[0].BaseNodeGuid;

        foreach (var node in nodes)
        {
            if (node.EntryPoint)
            {
                continue;

            }


            edges.Where(x => x.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));

            _targetGraphView.RemoveElement(node);

        }
    }


    private void CreateNodes()
    {
        foreach (var nodedata in _containerCache.dialogueNodeData)
        {
            var tempNode = _targetGraphView.CreateDialogueNode(nodedata.DialogueText);

            tempNode.GUID = nodedata.NodeGUID;

            _targetGraphView.AddElement(tempNode);

            var nodePorts = _containerCache.nodeLinks.Where(x => x.BaseNodeGuid == nodedata.NodeGUID).ToList();
            nodePorts.ForEach(x => _targetGraphView.AddChoicePort(tempNode, x.PortName));
        }
    }



    private void ConnectNodes()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            var connections = _containerCache.nodeLinks.Where(x => x.BaseNodeGuid == nodes[i].GUID).ToList();

            for (int w = 0; w < connections.Count; w++)
            {
                var targetNodeGuid = connections[w].TargetNodeGuid;


                var targetNode = nodes.First(x => x.GUID == targetNodeGuid);
                LinkNodes(nodes[i].outputContainer[w].Q<Port>(), (Port)targetNode.inputContainer[0]);


                targetNode.SetPosition(new Rect(_containerCache.dialogueNodeData.First(x => x.NodeGUID == targetNodeGuid).Position, new Vector2(50,50)));
            }
        }
    }

    private void LinkNodes(Port output, Port input)
    {
        var tempEdge = new Edge
        {
            input = input,
            output = output
        };



        tempEdge?.input.Connect(tempEdge);
        tempEdge?.output.Connect(tempEdge);
        _targetGraphView.Add(tempEdge);
    }
}
