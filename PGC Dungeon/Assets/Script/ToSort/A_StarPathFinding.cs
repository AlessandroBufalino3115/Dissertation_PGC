using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class A_StarPathFinding : MonoBehaviour
{
   
    /// <summary>
    /// / this is techinally useless
    /// </summary>

    static public A_StarPathFinding instance;



    [SerializeField]
    public List<Identifier> rulesList = new List<Identifier>();

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    
    //possibly to delete
    public List<AStar_Node> SolveA_StarPathfinding2DTest(Tile[][] tileArray2D) 
    {
        var start = GeneralUtil.RanVector2Int(tileArray2D[0].Length, tileArray2D.Length);
        var end = GeneralUtil.RanVector2Int(tileArray2D[0].Length, tileArray2D.Length);


        var path = APathUtils.A_StarPathfinding2DNorm(tileArray2D, start, end);
       
        return  path.Item1;
    }

}

