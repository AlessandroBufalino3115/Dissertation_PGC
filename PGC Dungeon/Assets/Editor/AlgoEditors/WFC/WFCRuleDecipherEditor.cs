

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using DS.Windows;
using System.IO;

[CustomEditor(typeof(WFCRuleDecipher))]
public class WFCRuleDecipherEditor : Editor
{

   



    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WFCRuleDecipher ruleDec = (WFCRuleDecipher)target;


     
        if (GUILayout.Button("Load rule Set"))
        {

            IDictionary<int, string> dictNameIdx = new Dictionary<int, string>();
            

            var fileName = "Assets/Resources/" + ruleDec.tileSetFileName;

            var info = new DirectoryInfo(fileName);
            var fileInfo = info.GetFiles();


            var currIdx = 0;

            foreach (var file in fileInfo)
            {
                if (file.Name.Contains("meta"))
                {
                    continue;
                }

                int index = file.Name.IndexOf(".");
                var manipString = file.Name.Substring(0, index);

                dictNameIdx.Add(currIdx, ruleDec.tileSetFileName + "/" + manipString);

                currIdx++;
            }



            ruleDec.ruleSet.Clear();

            var graphViewCont = Resources.Load<GraphViewDataCont>("WFC RuleSets/" + ruleDec.ruleSetFileName);



            foreach (var node in graphViewCont.nodeData)   // this creates all the rules 
            {
                if (node.dialogueType == DS.Enumerations.DSDialogueType.MultiChoice) 
                {
                    int idx = int.Parse(node.IndexTile);


                    bool present = false;

                    foreach (var rule in ruleDec.ruleSet)
                    {
                        if (rule.assetIdx == idx) 
                        {
                            present = true;
                            break;
                        }
                    }

                    if (!present) 
                    {
                        ruleDec.ruleSet.Add(new WFCTileRule() { assetIdx = idx, mainAsset = ruleDec.tileSet[idx] });

                        //var tileRef = new WFCTileRule();



                        //ruleDec.ruleSet.Add(tileRef);


                    }
                }
            }


            foreach (var edge in graphViewCont.nodeLinkData)
            {
                NodeData inputNode = null;
                NodeData outputNode = null;


                foreach (var node in graphViewCont.nodeData)
                {
                    if (node.nodeGuid == edge.TargetNodeGuid)
                    {
                        inputNode = node;
                    }
                    else if (node.nodeGuid == edge.BaseNodeGuid)
                    {

                        outputNode = node;
                    }
                }

                foreach(var rule in ruleDec.ruleSet)
                {

                    bool added = false;
                    if (rule.assetIdx == int.Parse(inputNode.IndexTile)) 
                    {
                        int idxToAdd = int.Parse(outputNode.IndexTile);

                        switch (edge.PortName)
                        {

                            case "Left Side":

                                if (!rule.allowedObjLeft.Contains(idxToAdd)) 
                                {
                                    rule.allowedObjLeft.Add(idxToAdd);
                                }

                                break;

                            case "Up Side":


                                if (!rule.allowedObjAbove.Contains(idxToAdd))
                                {
                                    rule.allowedObjAbove.Add(idxToAdd);
                                }
                                break ;

                            case "Right Side":

                                if (!rule.allowedObjRight.Contains(idxToAdd))
                                {
                                    rule.allowedObjRight.Add(idxToAdd);
                                }

                                break ;

                            case "Down Side":

                                if (!rule.allowedObjBelow.Contains(idxToAdd))
                                {
                                    rule.allowedObjBelow.Add(idxToAdd);
                                }

                                break ;

                            default:
                                break ;
                        }
                        
                        added = true;
                    }

                    if (added) { break; }

                }

                //ruleDec.tileSet = new GameObject[ruleDec.ruleSet.Count];


                //foreach (var rule in ruleDec.ruleSet)
                //{
                //    ruleDec.tileSet[rule.assetIdx] = rule.mainAsset;
                //}

            }
        }



        if (GUILayout.Button("Load rule tiel set"))
        {

            var namesList = new List<string>();


            var fileName = "Assets/Resources/" + ruleDec.tileSetFileName;

            var info = new DirectoryInfo(fileName);
            var fileInfo = info.GetFiles();


            var currIdx = 0;

            foreach (var file in fileInfo)
            {

                Debug.Log(file);
                if (file.Name.Contains("meta"))
                {
                    continue;
                }

                int index = file.Name.IndexOf(".");
                var manipString = file.Name.Substring(0, index);

                namesList.Add(ruleDec.tileSetFileName + "/" + manipString);

                currIdx++;
            }


            ruleDec.tileSet = new GameObject[ruleDec.ruleSet.Count];
            for (int i = 0; i < ruleDec.tileSet.Length; i++)
            {
                ruleDec.tileSet[i] = Resources.Load(namesList[i]) as GameObject;
            }

        }



    }

}
