using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System;
using System.Xml;
using static Unity.VisualScripting.Metadata;
using Random = UnityEngine.Random;
using Unity.VisualScripting;
using Newtonsoft.Json.Linq;
using System.Linq;
using static UnityEditor.Progress;



/*
 * the mian issue seems to be with the right and left sometimes they dont agree but you could alos say its because there isnt anough tiles variance or maybe i cant do
 * my left and right
 */


public class WaveFunctionCollaps : MonoBehaviour
{
    //true randomness
    // apperance %
    // user set %


    public static WaveFunctionCollaps instance;
    public WFCRuleBuilder ruleBuilder;




    [SerializeField]
    public List<WFCTileRule> arrayOfModulesRules = new List<WFCTileRule>();





    [SerializeField]
    public List<WFCTileRule> arrayOfModulesRulesCopy = new List<WFCTileRule>();

    [SerializeField]
    public List<GameObject> ListOfAssets = new List<GameObject>();


    public List<PlacedModule> ListOfPlacedAssets = new List<PlacedModule>();

    [SerializeField]
    public WFCTile[][] arrayOfWFCTiles = new WFCTile[1][];




    public bool WFCon = false;

    public bool WFCEditingMode = false;
    public bool WFCMapMode = false;

    public Vector2 currentHead = Vector2.zero;
    public int mouseScrollWheel = 0;

    [Range(10, 150)]
    public int maxXsize = 10;
    [Range(10, 150)]
    public int maxYsize = 10;

    public GameObject ghostModule = null;
    // an algo to make everything a bit more efficient 


    public GameObject pointer;
    public GameObject WFCparent;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        WFCEditingMode = true;
    }




    //up and down cycle in the index direction
    //left and right in the decisions 
    //mouse wheel the possible curr thing
    //backspace or return to select

    // keypar enter to cycle the main



    public enum Dir
    {
        LEFT,
        RIGHT,
        FORWARD,
        BACKWARD
    }

    public Dir ruleModeDir;
   
    public int ruleModeIdx;
    public int ruleModeMainIdx;






    public void PositionRuleBuilder()
    {

        switch (ruleModeDir)
        {
            case Dir.LEFT:

                pointer.transform.position = new Vector3(-(ruleBuilder.AllowedLeft.Count * 1.25f) - 2, 1.1f, 0);

                ruleModeIdx = ruleBuilder.AllowedLeft.Count;
                break;

            case Dir.RIGHT:

                pointer.transform.position = new Vector3((ruleBuilder.AllowedRight.Count * 1.25f) + 2, 1.1f, 0);

                ruleModeIdx = ruleBuilder.AllowedRight.Count;
                break;

            case Dir.FORWARD:

                pointer.transform.position = new Vector3(0, 1.1f, (ruleBuilder.AllowedForwards.Count * 1.25f) + 2);

                ruleModeIdx = ruleBuilder.AllowedForwards.Count;

                break;

            case Dir.BACKWARD:

                pointer.transform.position = new Vector3(0, 1.1f, -(ruleBuilder.AllowedBackwards.Count * 1.25f) - 2);

                ruleModeIdx = ruleBuilder.AllowedBackwards.Count;

                break;

            default:
                break;
        }

    }



    public void IDXRuleBuilder()
    {

        switch (ruleModeDir)
        {
            case Dir.LEFT:


                if (ruleModeIdx > ruleBuilder.AllowedLeft.Count)
                    ruleModeIdx = 0;
                else if (ruleModeIdx < 0)
                    ruleModeIdx = ruleBuilder.AllowedLeft.Count;

                pointer.transform.position = new Vector3(-(ruleModeIdx * 1.25f) - 2, 1.1f, 0);

                if (ruleModeIdx == ruleBuilder.AllowedLeft.Count)
                {
                    ghostModule = Instantiate(ListOfAssets[mouseScrollWheel]);
                    ghostModule.transform.position = new Vector3(-(ruleModeIdx * 1.25f) - 2, 0, 0);
                }
                else
                {
                    Destroy(ghostModule);
                }

                break;

            case Dir.RIGHT:


                if (ruleModeIdx > ruleBuilder.AllowedRight.Count)
                    ruleModeIdx = 0;
                else if (ruleModeIdx < 0)
                    ruleModeIdx = ruleBuilder.AllowedRight.Count;


                pointer.transform.position = new Vector3((ruleModeIdx * 1.25f) + 2, 1.1f, 0);

                if (ruleModeIdx == ruleBuilder.AllowedRight.Count)
                {
                    ghostModule = Instantiate(ListOfAssets[mouseScrollWheel]);
                    ghostModule.transform.position = new Vector3((ruleModeIdx * 1.25f) + 2, 0, 0);

                }
                else
                {
                    Destroy(ghostModule);
                }

                break;

            case Dir.FORWARD:

                if (ruleModeIdx > ruleBuilder.AllowedForwards.Count)
                    ruleModeIdx = 0;
                else if (ruleModeIdx < 0)
                    ruleModeIdx = ruleBuilder.AllowedForwards.Count;

                pointer.transform.position = new Vector3(0, 1.1f, (ruleModeIdx * 1.25f) + 2);


                if (ruleModeIdx == ruleBuilder.AllowedForwards.Count)
                {
                    ghostModule = Instantiate(ListOfAssets[mouseScrollWheel]);
                    ghostModule.transform.position = new Vector3(0, 0, (ruleModeIdx * 1.25f) + 2);
                }
                else
                {
                    Destroy(ghostModule);
                }
                break;

            case Dir.BACKWARD:


                if (ruleModeIdx > ruleBuilder.AllowedBackwards.Count)
                    ruleModeIdx = 0;
                else if (ruleModeIdx < 0)
                    ruleModeIdx = ruleBuilder.AllowedBackwards.Count;


                pointer.transform.position = new Vector3(0, 1.1f, -(ruleModeIdx * 1.25f) - 2);

                if (ruleModeIdx == ruleBuilder.AllowedBackwards.Count)
                {
                    ghostModule = Instantiate(ListOfAssets[mouseScrollWheel]);
                    ghostModule.transform.position = new Vector3(0, 0, -(ruleModeIdx * 1.25f) - 2);
                }
                else
                {
                    Destroy(ghostModule);
                }


                break;

            default:
                break;
        }

    }




    // Update is called once per frame
    void Update()
    {
        if (WFCEditingMode)
        {

            if (!WFCMapMode)
            {
                if (!WFCparent.activeSelf)
                {
                    WFCparent.SetActive(true);

                    ruleBuilder.ChangeMainTile(0);

                    IDXRuleBuilder();
                }
            }
            else
            {

                ResetGhost(currentHead, mouseScrollWheel);

                if (ghostModule != null)
                {
                    if (!ghostModule.activeSelf)
                    {
                        ghostModule.SetActive(true);
                    }
                }

                pointer.transform.position = new Vector3(currentHead.x, 1.1f, currentHead.y);
                if (WFCparent.activeSelf) { WFCparent.SetActive(false); }
            }

            if (!pointer.activeSelf)
            {
                pointer.SetActive(true);

            }




            if (Input.GetKeyDown(KeyCode.UpArrow))
            {

                if (!WFCMapMode)
                {

                    if (ruleModeDir == Dir.BACKWARD)
                    {
                        ruleModeDir = Dir.LEFT;
                    }
                    else
                    {
                        ruleModeDir = ruleModeDir + 1;
                    }
                    Destroy(ghostModule);
                    PositionRuleBuilder();
                    IDXRuleBuilder();


                }
                else
                {
                    if (currentHead.y + 1 >= maxYsize)
                    {
                    }
                    else { currentHead = new Vector2(currentHead.x, currentHead.y + 1); }


                    ResetGhost(currentHead, mouseScrollWheel);
                }


            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {


                if (!WFCMapMode)
                {
                    if (ruleModeDir == Dir.LEFT)
                    {
                        ruleModeDir = Dir.BACKWARD;
                    }
                    else
                    {
                        ruleModeDir = ruleModeDir - 1;
                    }
                    Destroy(ghostModule);
                    PositionRuleBuilder();
                    IDXRuleBuilder();


                }
                else
                {
                    if (currentHead.y - 1 < 0)
                    {
                    }
                    else { currentHead = new Vector2(currentHead.x, currentHead.y - 1); }


                    ResetGhost(currentHead, mouseScrollWheel);
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {

                if (!WFCMapMode)
                {
                    ruleModeIdx++;
                    IDXRuleBuilder();
                }
                else
                {
                    if (currentHead.x - 1 < 0)
                    {
                    }
                    else { currentHead = new Vector2(currentHead.x - 1, currentHead.y); }

                    ResetGhost(currentHead, mouseScrollWheel);

                }



            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {

                if (!WFCMapMode)
                {
                    ruleModeIdx--;

                    IDXRuleBuilder();
                }
                else
                {
                    if (currentHead.x + 1 >= maxXsize)
                    {
                    }
                    else { currentHead = new Vector2(currentHead.x + 1, currentHead.y); }


                    ResetGhost(currentHead, mouseScrollWheel);
                }


            }



            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                mouseScrollWheel++;
                if (mouseScrollWheel >= ListOfAssets.Count)
                {
                    mouseScrollWheel = 0;
                }

                if (!WFCMapMode)
                {
                    if (ghostModule != null)
                    {
                        var pos = ghostModule.transform.position;
                        Destroy(ghostModule);
                        ghostModule = Instantiate(ListOfAssets[mouseScrollWheel]);
                        ghostModule.transform.position = pos;
                    }
                }
                else
                {
                    ResetGhost(currentHead, mouseScrollWheel);
                }


            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {

                mouseScrollWheel--;
                if (mouseScrollWheel < 0)
                {
                    mouseScrollWheel = ListOfAssets.Count - 1;
                }


                if (!WFCMapMode)
                {
                    if (ghostModule != null)
                    {
                        var pos = ghostModule.transform.position;
                        Destroy(ghostModule);
                        ghostModule = Instantiate(ListOfAssets[mouseScrollWheel]);
                        ghostModule.transform.position = pos;
                    }
                }
                else
                {
                    ResetGhost(currentHead, mouseScrollWheel);
                }

            }




            if (Input.GetKeyDown(KeyCode.Return))
            {


                if (!WFCMapMode)
                {
                    ruleBuilder.InsertRule((int)ruleModeDir, mouseScrollWheel, mouseScrollWheel);
                }
                else
                {

                    bool free = true;
                    int idx = 0;

                    for (int i = 0; i < ListOfPlacedAssets.Count; i++)
                    {
                        if (ListOfPlacedAssets[i].xCords == currentHead.x && ListOfPlacedAssets[i].yCords == currentHead.y)
                        {
                            idx = i;
                            free = false;
                        }
                    }

                    if (!free)
                    {
                        Destroy(ListOfPlacedAssets[idx].moduleAsset);

                        ListOfPlacedAssets.RemoveAt(idx);

                    }


                    ListOfPlacedAssets.Add(new PlacedModule());

                    PlacedModule newRef = ListOfPlacedAssets[ListOfPlacedAssets.Count - 1];

                    newRef.xCords = (int)currentHead.x;
                    newRef.yCords = (int)currentHead.y;

                    newRef.moduleAssetIndex = mouseScrollWheel;
                    newRef.moduleAsset = Instantiate(ListOfAssets[mouseScrollWheel], this.transform);

                    newRef.moduleAssetName = newRef.moduleAsset.name;

                    newRef.moduleAsset.transform.position = new Vector3(currentHead.x, 0, currentHead.y);

                }

            }



            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (!WFCMapMode)
                {
                    ruleModeMainIdx++;
                    if (ruleModeMainIdx >= ListOfAssets.Count)
                    {
                        ruleModeMainIdx = 0;
                    }
                    ruleBuilder.ChangeMainTile(ruleModeMainIdx);
                }
                else
                {
                    for (int y = 0; y < maxYsize; y++)
                    {
                        for (int x = 0; x < maxXsize; x++)
                        {
                            ListOfPlacedAssets.Add(new PlacedModule());

                            PlacedModule newRef = ListOfPlacedAssets[ListOfPlacedAssets.Count - 1];

                            newRef.xCords = x;
                            newRef.yCords = y;

                            newRef.moduleAssetIndex = ListOfAssets.Count - 1;
                            newRef.moduleAsset = Instantiate(ListOfAssets[newRef.moduleAssetIndex], this.transform);
                            newRef.moduleAssetName = newRef.moduleAsset.name;

                            newRef.moduleAsset.transform.position = new Vector3(x, 0, y);
                        }
                    }
                }
            }




            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                if (!WFCMapMode)
                {
                    ruleBuilder.DeleteRule((int)ruleModeDir, ruleModeIdx, mouseScrollWheel);


                    PositionRuleBuilder();
                    IDXRuleBuilder();

                }
                else
                {
                    CreateRuleSet();
                }
            }



            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                RunWFCVar();
            }

        }
        else
        {
            if (WFCparent.activeSelf) { pointer.SetActive(false); }
            if (pointer.activeSelf) { pointer.SetActive(false); }
            if (ghostModule != null)
            {
                if (ghostModule.activeSelf)
                {
                    ghostModule.SetActive(false);
                }
            }
        }
    }





    public void CreateRuleSet()
    {

        arrayOfModulesRules = new List<WFCTileRule>();


        for (int i = 0; i < ListOfAssets.Count; i++)
        {
            arrayOfModulesRules.Add(new WFCTileRule());
            arrayOfModulesRules[i].assetIdx = i;
            arrayOfModulesRules[i].mainAsset = ListOfAssets[i];
        }





        // gor thoruhg the placed list     get the and then go rhgout te placed list again 

        foreach (var mainPlacedModule in ListOfPlacedAssets)
        {

            int idxMain = mainPlacedModule.moduleAssetIndex;

            int selfX = mainPlacedModule.xCords;
            int selfY = mainPlacedModule.yCords;


            foreach (var comparingPlacedModule in ListOfPlacedAssets)
            {



                if (comparingPlacedModule == mainPlacedModule) { continue; }



                int idxCompare = comparingPlacedModule.moduleAssetIndex;


                //right

                if (selfX + 1 >= maxXsize)
                { }
                else
                {
                    if (selfX + 1 == comparingPlacedModule.xCords && selfY == comparingPlacedModule.yCords)
                    {
                        if (arrayOfModulesRules[idxMain].allowedObjRight.Contains(idxCompare)) { }
                        else { arrayOfModulesRules[idxMain].allowedObjRight.Add(idxCompare); }
                    }
                }




                //left

                if (selfX - 1 < 0)
                {

                }
                else
                {
                    if (selfX - 1 == comparingPlacedModule.xCords && selfY == comparingPlacedModule.yCords)
                    {
                        if (arrayOfModulesRules[idxMain].allowedObjLeft.Contains(idxCompare)) { }
                        else { arrayOfModulesRules[idxMain].allowedObjLeft.Add(idxCompare); }
                    }
                }


                //forw

                if (selfY + 1 >= maxYsize)
                { }
                else
                {
                    if (selfX == comparingPlacedModule.xCords && selfY + 1 == comparingPlacedModule.yCords)
                    {
                        if (arrayOfModulesRules[idxMain].allowedObjForward.Contains(idxCompare)) { }
                        else { arrayOfModulesRules[idxMain].allowedObjForward.Add(idxCompare); }
                    }
                }




                //back

                if (selfY - 1 < 0)
                {

                }
                else
                {
                    if (selfX == comparingPlacedModule.xCords && selfY - 1 == comparingPlacedModule.yCords)
                    {
                        if (arrayOfModulesRules[idxMain].allowedObjBackwards.Contains(idxCompare)) { }
                        else { arrayOfModulesRules[idxMain].allowedObjBackwards.Add(idxCompare); }
                    }
                }
            }



        }



    }

    public void ResetGhost(Vector2 _currentHead, int idx)
    {
        Destroy(ghostModule);

        ghostModule = Instantiate(ListOfAssets[idx]);
        ghostModule.transform.position = new Vector3(_currentHead.x, 0.05f, _currentHead.y);

        ghostModule.GetComponent<MeshRenderer>().material.color = new Color(ghostModule.GetComponent<MeshRenderer>().material.color.r, ghostModule.GetComponent<MeshRenderer>().material.color.g, ghostModule.GetComponent<MeshRenderer>().material.color.b, 0.4f);


    }


    public void RunWFCVar()
    {
        // set up the superposition on all of the tiles 

        arrayOfWFCTiles = new WFCTile[maxYsize][];

        for (int y = 0; y < arrayOfWFCTiles.Length; y++)
        {
            arrayOfWFCTiles[y] = new WFCTile[maxXsize];

            for (int x = 0; x < arrayOfWFCTiles[y].Length; x++)
            {
                arrayOfWFCTiles[y][x] = new WFCTile();

                arrayOfWFCTiles[y][x].gridPos = new Vector2(x, y);

                foreach (var modules in arrayOfModulesRules)
                {

                    arrayOfWFCTiles[y][x].allowedObjRight.Add(modules.assetIdx);
                    arrayOfWFCTiles[y][x].allowedObjLeft.Add(modules.assetIdx);
                    arrayOfWFCTiles[y][x].allowedObjBackwards.Add(modules.assetIdx);
                    arrayOfWFCTiles[y][x].allowedObjForward.Add(modules.assetIdx);
                }
            }
        }


        Vector2 ranStart = GeneralUtil.RanVector2Int(maxXsize, maxYsize);

        arrayOfWFCTiles[(int)ranStart.y][(int)ranStart.x].solved = true;
        SetNeighbourAllowed(ranStart);

        arrayOfWFCTiles[(int)ranStart.y][(int)ranStart.x].RunSimilarityChecks();
        arrayOfWFCTiles[(int)ranStart.y][(int)ranStart.x].DecideIndex();

        arrayOfWFCTiles[(int)ranStart.y][(int)ranStart.x].spawnedAsset = Instantiate(ListOfAssets[arrayOfWFCTiles[(int)ranStart.y][(int)ranStart.x].decidedIndex], this.transform);

        arrayOfWFCTiles[(int)ranStart.y][(int)ranStart.x].AssetPosition();


        bool WFCSolved = false;


        while (WFCSolved == false)
        {

            WFCSolved = true;
            int lowestEntropy = arrayOfModulesRules.Count + 2;

            WFCTile lowEntropyTile = null;

            for (int y = 0; y < arrayOfWFCTiles.Length; y++)        // loop
            {
                for (int x = 0; x < arrayOfWFCTiles[y].Length; x++)
                {

                    if (arrayOfWFCTiles[y][x].solved == false) // if this tile is not solved then
                    {

                        WFCSolved = false;    // dont end the while loop

                        SetNeighbourAllowed(new Vector2(x, y));  //give the 

                        if (arrayOfWFCTiles[y][x].possibleAssetsIDX.Count < lowestEntropy)
                        {
                            lowestEntropy = arrayOfWFCTiles[y][x].possibleAssetsIDX.Count;
                            lowEntropyTile = arrayOfWFCTiles[y][x];

                        }
                    }

                }

            }

            if (WFCSolved == true)
            {
                break;
            }
            else
            {
                lowEntropyTile.solved = true;
                lowEntropyTile.DecideIndex();
                lowEntropyTile.spawnedAsset = Instantiate(ListOfAssets[lowEntropyTile.decidedIndex], this.transform);
                lowEntropyTile.AssetPosition();
            }
        }


    }



    public void SetNeighbourAllowed(Vector2 coordinate)
    {
        WFCTile mainTile = arrayOfWFCTiles[(int)coordinate.y][(int)coordinate.x];



        mainTile.allowedObjRight = new List<int>();
        mainTile.allowedObjLeft = new List<int>();
        mainTile.allowedObjBackwards = new List<int>();
        mainTile.allowedObjForward = new List<int>();




        if (coordinate.x + 1 >= maxXsize)
        {
            if (arrayOfWFCTiles[(int)coordinate.y][0].solved)
            {
                int indexOfSolvedNeighbourTile = arrayOfWFCTiles[(int)coordinate.y][0].decidedIndex;

                foreach (var item in arrayOfModulesRules[indexOfSolvedNeighbourTile].allowedObjLeft)
                {
                    mainTile.allowedObjRight.Add(item);
                }
            }
            else
            {

                foreach (var modules in arrayOfModulesRules)
                {
                    arrayOfWFCTiles[(int)coordinate.y][(int)coordinate.x].allowedObjRight.Add(modules.assetIdx);
                }
            }
        }
        else
        {

            if (arrayOfWFCTiles[(int)coordinate.y][(int)coordinate.x + 1].solved)
            {
                int indexOfSolvedNeighbourTile = arrayOfWFCTiles[(int)coordinate.y][(int)coordinate.x + 1].decidedIndex;


                foreach (var item in arrayOfModulesRules[indexOfSolvedNeighbourTile].allowedObjLeft)
                {
                    mainTile.allowedObjRight.Add(item);
                }
            }
            else
            {
                foreach (var modules in arrayOfModulesRules)
                {
                    arrayOfWFCTiles[(int)coordinate.y][(int)coordinate.x].allowedObjRight.Add(modules.assetIdx);
                }
            }
        }

        //the tile on the left
        if (coordinate.x - 1 < 0)
        {

            if (arrayOfWFCTiles[(int)coordinate.y][maxXsize - 1].solved)
            {
                int indexOfSolvedNeighbourTile = arrayOfWFCTiles[(int)coordinate.y][maxXsize - 1].decidedIndex;


                foreach (var item in arrayOfModulesRules[indexOfSolvedNeighbourTile].allowedObjRight)
                {
                    mainTile.allowedObjLeft.Add(item);
                }
            }
            else
            {
                foreach (var modules in arrayOfModulesRules)
                {
                    arrayOfWFCTiles[(int)coordinate.y][(int)coordinate.x].allowedObjLeft.Add(modules.assetIdx);
                }
            }
        }
        else
        {

            if (arrayOfWFCTiles[(int)coordinate.y][(int)coordinate.x - 1].solved)
            {
                int indexOfSolvedNeighbourTile = arrayOfWFCTiles[(int)coordinate.y][(int)coordinate.x - 1].decidedIndex;


                foreach (var item in arrayOfModulesRules[indexOfSolvedNeighbourTile].allowedObjRight)
                {
                    mainTile.allowedObjLeft.Add(item);
                }
            }
            else
            {
                foreach (var modules in arrayOfModulesRules)
                {
                    arrayOfWFCTiles[(int)coordinate.y][(int)coordinate.x].allowedObjLeft.Add(modules.assetIdx);
                }
            }
        }

        //the tile on the for
        if (coordinate.y + 1 >= maxYsize)
        {

            if (arrayOfWFCTiles[0][(int)coordinate.x].solved)
            {
                int indexOfSolvedNeighbourTile = arrayOfWFCTiles[0][(int)coordinate.x].decidedIndex;

                mainTile.allowedObjForward = new List<int>();

                foreach (var item in arrayOfModulesRules[indexOfSolvedNeighbourTile].allowedObjBackwards)
                {
                    mainTile.allowedObjForward.Add(item);
                }
            }
            else
            {
                foreach (var modules in arrayOfModulesRules)
                {
                    arrayOfWFCTiles[(int)coordinate.y][(int)coordinate.x].allowedObjForward.Add(modules.assetIdx);
                }
            }
        }
        else
        {
            if (arrayOfWFCTiles[(int)coordinate.y + 1][(int)coordinate.x].solved)
            {
                int indexOfSolvedNeighbourTile = arrayOfWFCTiles[(int)coordinate.y + 1][(int)coordinate.x].decidedIndex;

                mainTile.allowedObjForward = new List<int>();

                foreach (var item in arrayOfModulesRules[indexOfSolvedNeighbourTile].allowedObjBackwards)
                {
                    mainTile.allowedObjForward.Add(item);
                }
            }
            else
            {
                foreach (var modules in arrayOfModulesRules)
                {
                    arrayOfWFCTiles[(int)coordinate.y][(int)coordinate.x].allowedObjForward.Add(modules.assetIdx);
                }
            }
        }

        //the tile on the back
        if (coordinate.y - 1 < 0)
        {

            if (arrayOfWFCTiles[maxYsize - 1][(int)coordinate.x].solved)
            {

                int indexOfSolvedNeighbourTile = arrayOfWFCTiles[maxYsize - 1][(int)coordinate.x].decidedIndex;

                mainTile.allowedObjBackwards = new List<int>();

                foreach (var item in arrayOfModulesRules[indexOfSolvedNeighbourTile].allowedObjForward)
                {
                    mainTile.allowedObjBackwards.Add(item);
                }
            }
            else
            {
                foreach (var modules in arrayOfModulesRules)
                {
                    arrayOfWFCTiles[(int)coordinate.y][(int)coordinate.x].allowedObjBackwards.Add(modules.assetIdx);
                }
            }
        }
        else
        {

            if (arrayOfWFCTiles[(int)coordinate.y - 1][(int)coordinate.x].solved)
            {

                int indexOfSolvedNeighbourTile = arrayOfWFCTiles[(int)coordinate.y - 1][(int)coordinate.x].decidedIndex;

                foreach (var item in arrayOfModulesRules[indexOfSolvedNeighbourTile].allowedObjForward)
                {
                    mainTile.allowedObjBackwards.Add(item);
                }
            }
            else
            {
                foreach (var modules in arrayOfModulesRules)
                {
                    arrayOfWFCTiles[(int)coordinate.y][(int)coordinate.x].allowedObjBackwards.Add(modules.assetIdx);
                }
            }
        }

        mainTile.RunSimilarityChecks();

    }



}



public class PlacedModule
{
    public GameObject moduleAsset;
    public string moduleAssetName;
    public int moduleAssetIndex;

    public int xCords;
    public int yCords;
}

[Serializable]
public class WFCTileRule
{
    public GameObject mainAsset;

    public int assetIdx;

    public List<int> allowedObjLeft = new List<int>();
    public List<int> allowedObjRight = new List<int>();
    public List<int> allowedObjForward = new List<int>();
    public List<int> allowedObjBackwards = new List<int>();
}




[Serializable]
public class WFCTile
{
    public Vector2 gridPos = Vector2.zero;
    public GameObject spawnedAsset = null;
    public bool solved = false;
    public int decidedIndex = -1;
    public List<int> possibleAssetsIDX = new List<int>();

    public List<int> allowedObjLeft = new List<int>();
    public List<int> allowedObjRight = new List<int>();
    public List<int> allowedObjForward = new List<int>();
    public List<int> allowedObjBackwards = new List<int>();




    /// <summary>
    /// matches all of the directions to find a simlar value
    /// </summary>
    public void RunSimilarityChecks()
    {
        var commonElements1 = allowedObjRight.Intersect(allowedObjLeft).ToArray();
        var commonElements2 = allowedObjBackwards.Intersect(allowedObjForward).ToArray();

        var finalArray = commonElements2.Intersect(commonElements1).ToArray();

        this.possibleAssetsIDX = new List<int>();

        foreach (var item in finalArray)
        {
            this.possibleAssetsIDX.Add(item);
        }

        if (this.possibleAssetsIDX.Count == 0)
        {

            Debug.Log($"-------------------------------------");
            foreach (var item in allowedObjBackwards)
            {
                Debug.Log($"int he back {item}");
            }
            foreach (var item in allowedObjForward)
            {
                Debug.Log($"int he forward {item}");
            }
            foreach (var item in allowedObjRight)
            {
                Debug.Log($"int he right {item}");
            }
            foreach (var item in allowedObjLeft)
            {
                Debug.Log($"int he left {item}");
            }
            Debug.Log($"===========================================");


            Debug.Log($"<color=red>there is a fatal flaw as this tile {gridPos} has no similarities</color>");
        }
    }

    /// <summary>
    /// randomly picks from the available indexes
    /// </summary>
    public void DecideIndex() /*=> this.decidedIndex = this.possibleAssetsIDX[Random.Range(0, this.possibleAssetsIDX.Count)];*/
    {
        if (this.possibleAssetsIDX.Count == 0)
        {
            Debug.Log($"fddfsdfdsffdsfsdfds");
            this.decidedIndex = 0;   // this is meant to be teh base tile the basic one
        }
        else
        {

            this.decidedIndex = this.possibleAssetsIDX[Random.Range(0, this.possibleAssetsIDX.Count)];
        }
    }
    public void AssetPosition()
    {
        this.spawnedAsset.transform.position = new Vector3(gridPos.x, 0, gridPos.y);
        this.spawnedAsset.transform.name = gridPos.x + "  " + gridPos.y;
    }
}


