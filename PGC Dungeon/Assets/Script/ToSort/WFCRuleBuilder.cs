using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static WaveFunctionCollaps;

public class WFCRuleBuilder : MonoBehaviour
{
    public GameObject MainTile;

    public List<GameObject> AllowedRight = new List<GameObject>();
    public List<GameObject> AllowedLeft = new List<GameObject>();
    public List<GameObject> AllowedBackwards = new List<GameObject>();
    public List<GameObject> AllowedForwards = new List<GameObject>();




    public void DeleteRule(int dir, int idx, int mainIdx)
    {
        switch (dir)
        {
            case 0:

                if (idx < AllowedLeft.Count)
                {
                    //Destroy(AllowedLeft[idx]);
                    //AllowedLeft.RemoveAt(idx);
                    WaveFunctionCollaps.instance.arrayOfModulesRules[mainIdx].allowedObjLeft.RemoveAt(idx);
                }

                break;

            case 1:

                if (idx < AllowedRight.Count)
                {
                    //Destroy(AllowedRight[idx]);
                    //AllowedRight.RemoveAt(idx);
                    WaveFunctionCollaps.instance.arrayOfModulesRules[mainIdx].allowedObjRight.RemoveAt(idx);
                }

                break;

            case 2:

                if (idx < AllowedForwards.Count)
                {
                    //Destroy(AllowedForwards[idx]);
                    //AllowedForwards.RemoveAt(idx);
                    WaveFunctionCollaps.instance.arrayOfModulesRules[mainIdx].allowedObjForward.RemoveAt(idx);
                }

                break;

            case 3:

                if (idx < AllowedBackwards.Count)
                {
                    //Destroy(AllowedBackwards[idx]);
                    //AllowedBackwards.RemoveAt(idx);
                    WaveFunctionCollaps.instance.arrayOfModulesRules[mainIdx].allowedObjBackwards.RemoveAt(idx);
                }

                break;


            default:
                break;
        }

        ChangeMainTile(mainIdx);

    }






    public void InsertRule(int dir, int idx, int mainIdx)
    {
        switch (dir)
        {
            case 0:

                if (!WaveFunctionCollaps.instance.arrayOfModulesRules[mainIdx].allowedObjLeft.Contains(idx))
                {
                    WaveFunctionCollaps.instance.arrayOfModulesRules[mainIdx].allowedObjLeft.Add(idx);
                }
                else
                {
                    Debug.Log($"ddwwiwdw {mainIdx} {idx} ");
                }

                break;

            case 1:

                if (!WaveFunctionCollaps.instance.arrayOfModulesRules[mainIdx].allowedObjRight.Contains(idx))
                {
                    WaveFunctionCollaps.instance.arrayOfModulesRules[mainIdx].allowedObjRight.Add(idx);
                }
                else
                {
                    Debug.Log($"ddwwiwdw {mainIdx} {idx} ");
                }
                break;

            case 2:

                if (!WaveFunctionCollaps.instance.arrayOfModulesRules[mainIdx].allowedObjForward.Contains(idx))
                {
                    WaveFunctionCollaps.instance.arrayOfModulesRules[mainIdx].allowedObjForward.Add(idx);
                }
                else
                {
                    Debug.Log($"ddwwiwdw {mainIdx} {idx} ");
                }
                break;

            case 3:

                if (!WaveFunctionCollaps.instance.arrayOfModulesRules[mainIdx].allowedObjBackwards.Contains(idx))
                {
                    WaveFunctionCollaps.instance.arrayOfModulesRules[mainIdx].allowedObjBackwards.Add(idx);
                }
                else
                {
                    Debug.Log($"ddwwiwdw {mainIdx} {idx} ");
                }
                break;


            default:
                break;
        }

        ChangeMainTile(mainIdx);

    }

    public void ChangeMainTile(int idx) 
    {
        var wFC = WaveFunctionCollaps.instance;

        Destroy(MainTile);


        MainTile = Instantiate(wFC.ListOfAssets[idx], this.transform);
        MainTile.transform.position = Vector3.zero;


        foreach (var item in AllowedBackwards)
        {
            Destroy(item);
        }
        AllowedBackwards.Clear();


        foreach (var item in AllowedLeft)
        {
            Destroy(item);
        }
        AllowedLeft.Clear();


        foreach (var item in AllowedRight)
        {
            Destroy(item);
        }
        AllowedRight.Clear();


        foreach (var item in AllowedForwards)
        {
            Destroy(item);
        }
        AllowedForwards.Clear();


        for (int i = 0; i < wFC.arrayOfModulesRules[idx].allowedObjBackwards.Count; i++)
        {
            AllowedBackwards.Add(Instantiate(wFC.ListOfAssets[wFC.arrayOfModulesRules[idx].allowedObjBackwards[i]], this.transform));
            AllowedBackwards[AllowedBackwards.Count - 1].transform.position = new Vector3(0, 0, -2 - (i * 1.25f));
        }

        for (int i = 0; i < wFC.arrayOfModulesRules[idx].allowedObjRight.Count; i++)
        {
            AllowedRight.Add(Instantiate(wFC.ListOfAssets[wFC.arrayOfModulesRules[idx].allowedObjRight[i]], this.transform));
            AllowedRight[AllowedRight.Count - 1].transform.position = new Vector3(2 + (i * 1.25f), 0, 0 );
        }

        for (int i = 0; i < wFC.arrayOfModulesRules[idx].allowedObjLeft.Count; i++)
        {
            AllowedLeft.Add(Instantiate(wFC.ListOfAssets[wFC.arrayOfModulesRules[idx].allowedObjLeft[i]], this.transform));
            AllowedLeft[AllowedLeft.Count - 1].transform.position = new Vector3( -2 - (i * 1.25f ), 0, 0);
        }

        for (int i = 0; i < wFC.arrayOfModulesRules[idx].allowedObjForward.Count; i++)
        {
            AllowedForwards.Add(Instantiate(wFC.ListOfAssets[wFC.arrayOfModulesRules[idx].allowedObjForward[i]], this.transform));
            AllowedForwards[AllowedForwards.Count - 1].transform.position = new Vector3(0, 0, 2 + (i * 1.25f ));
        }

        Destroy(WaveFunctionCollaps.instance.ghostModule);

    }











}
