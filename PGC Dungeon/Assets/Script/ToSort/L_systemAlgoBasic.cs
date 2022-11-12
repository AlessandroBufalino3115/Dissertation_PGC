using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Xml;

[Serializable]
public class KeyValuePair
{
    public char key;
    public string val;


    public KeyValuePair (char key, string val) { this.key = key; this.val = val; }
}



public class L_systemAlgoBasic : MonoBehaviour
{



    public List<KeyValuePair> ListToSee = new List<KeyValuePair>();



    public static L_systemAlgoBasic instance;
    public string axium;


    public string proposedRuleName;
    public string proposedAction;

    public Dictionary<char, string> rulesDict = new Dictionary<char, string>();

    [Range(2, 10)]
    public int iterations;



    public string solution;

    private void Awake()
    {
        instance = this;
    }


    public void AddRule() 
    {
        if (proposedRuleName.Length != 1) 
        {
            Debug.Log($"<color=red>This RuleName is not 1 character</color>");

            proposedRuleName = "";

            return;
        }

        proposedRuleName.ToUpper();
        char ruleName = proposedRuleName[0];

        if (rulesDict.ContainsKey(ruleName))
        {
            Debug.Log($"<color=red>This RuleName is already there</color>");
        }

        if (proposedAction.Length > 0)
        {
            proposedAction.ToUpper();
        }
        else 
        {
            Debug.Log($"<color=red>Action not valid</color>");

            proposedAction = "";
            return;
        }

        string actionRule = proposedAction;


        rulesDict.Add(ruleName, actionRule);

    }


    public void PrintDict()
    {
        ListToSee.Clear();

        foreach (KeyValuePair<char, string> entry in rulesDict)
        {
            ListToSee.Add(new KeyValuePair(entry.Key, entry.Value));
        }
    }



    public void RunLSystemAlgo() 
    {
        solution = axium;


        for (int i = 0; i < iterations; i++)
        {
            string presolution = "";
            for (int z = 0; z < solution.Length; z++)
            {
                presolution += rulesDict[(char)solution[z]];
            }

            solution = presolution;
            

        }


        Debug.Log($"<color=green>{solution}</color>");

    }




}




[CustomEditor(typeof(L_systemAlgoBasic))]
public class InspectorEditing : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        L_systemAlgoBasic newRef = (L_systemAlgoBasic)target;
        //EditorGUILayout.LabelField("This is a labe;");
        

        if (GUILayout.Button("add a Rule")) 
        {
            newRef.AddRule();
        }

        if (GUILayout.Button("run algo"))
        {
            newRef.RunLSystemAlgo();
        }

        if (GUILayout.Button("print rules"))
        {
            newRef.PrintDict();
        }
    }





}