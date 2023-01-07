using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

public class UIDrunkWalk : UiBaseState
{
    // ask if to fill in the holes
    // add more walk in the sesne of another run additivie
    //start from the center

    // there is an issue with the edge stuff if only two rooms




    private int iterations;
    private int iterationsLeft;

    private bool alreadyPassed;

    private int neighboursNeeded = 3;

    private bool wallDiag;
    private bool typeOfTri;
    private int minSize;

    private List<List<BasicTile>> rooms;

    //private bool hardStop;
    private List<Edge> primEdges = new List<Edge>();

    public override void onExit(StateUIManager currentMenu)
    {
    }

    public override void onGUI(StateUIManager currentMenu)
    {


        if (currentMenu.working)
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "LOADING");
        }
        else
        {
            GUI.Box(new Rect(5, 10, 230, 650), "");   //background 


            if (currentMenu.dimension == StateUIManager.Dimension.TWOD)
                iterations = (int)GUI.HorizontalSlider(new Rect(10, 50, 100, 20), iterations, 20, (int)((currentMenu.gridArrayObj2D.Length * currentMenu.gridArrayObj2D[0].Length)) * 0.9f);

            //else if (currentMenu.dimension == StateUIManager.Dimension.THREED)
            //    iterations = (int)GUI.HorizontalSlider(new Rect(10, 50, 100, 20), iterations, 20, (int)((currentMenu.gridArray3D.Length * currentMenu.gridArray3D[0].Length * currentMenu.gridArray3D[0][0].Length)) * 0.9f);

            else if (currentMenu.dimension == StateUIManager.Dimension.PLANE)
                iterations = (int)GUI.HorizontalSlider(new Rect(10, 50, 100, 20), iterations, 20, (int)((currentMenu.gridArray2D.Length * currentMenu.gridArray2D[0].Length)) * 0.9f);



            GUI.Label(new Rect(130, 45, 100, 30), "iterations: " + iterations);

            GUI.Label(new Rect(130, 70, 100, 30), "Iterations Left: " + iterationsLeft);


            alreadyPassed = GUI.Toggle(new Rect(10, 100, 120, 30), alreadyPassed, "Overwrite cells");

            if (GUI.Button(new Rect(10, 140, 120, 30), "Run Drunk Walk"))
            {
                if (currentMenu.dimension == StateUIManager.Dimension.TWOD)
                    DrunkWalkObj2D(currentMenu);

                else if (currentMenu.dimension == StateUIManager.Dimension.PLANE)
                    DrunkWalk2D(currentMenu);

            }

            if (GUI.Button(new Rect(10, 180, 120, 30), "Run CA cleanup"))
            {
                if (currentMenu.dimension == StateUIManager.Dimension.TWOD)
                    AlgosUtils.CleanUp2dCA(currentMenu.gridArrayObj2D, neighboursNeeded);

                else if (currentMenu.dimension == StateUIManager.Dimension.PLANE) 
                {
                    AlgosUtils.CleanUp2dCA(currentMenu.gridArray2D, neighboursNeeded);

                    currentMenu.plane.GetComponent<Renderer>().material.mainTexture = AlgosUtils.SetUpTextBiColAnchor(currentMenu.gridArray2D);
                }


            }

            if (GUI.Button(new Rect(10, 220, 120, 30), "Run CA iteration"))
            {
                if (currentMenu.dimension == StateUIManager.Dimension.TWOD)
                    AlgosUtils.RunCaIteration2D(currentMenu.gridArrayObj2D, neighboursNeeded);

                else if (currentMenu.dimension == StateUIManager.Dimension.PLANE) 
                {
                    AlgosUtils.RunCaIteration2D(currentMenu.gridArray2D, neighboursNeeded);
                    currentMenu.plane.GetComponent<Renderer>().material.mainTexture = AlgosUtils.SetUpTextBiColAnchor(currentMenu.gridArray2D);
                }

            }

            neighboursNeeded = (int)GUI.HorizontalSlider(new Rect(10, 250, 100, 20), neighboursNeeded, 3, 5);




            wallDiag = GUI.Toggle(new Rect(10, 320, 100, 30), wallDiag, wallDiag != true ? "diagonal" : "straight");

            typeOfTri = GUI.Toggle(new Rect(10, 600, 100, 30), typeOfTri, typeOfTri != true ? "delu" : "prims");


            if (GUI.Button(new Rect(10, 280, 120, 30), "Run Wall finding"))
            {
                if (currentMenu.dimension == StateUIManager.Dimension.TWOD)
                    AlgosUtils.SetUpTileTypes(currentMenu.gridArrayObj2D, wallDiag);

                else if (currentMenu.dimension == StateUIManager.Dimension.PLANE) 
                {
                    AlgosUtils.SetUpTileTypes(currentMenu.gridArray2D, wallDiag);
                    currentMenu.plane.GetComponent<Renderer>().material.mainTexture = AlgosUtils.SetUpTextBiColShade(currentMenu.gridArray2D, 0, 1,true);
                }

            }


            if (GUI.Button(new Rect(10, 360, 120, 30), "GEt All rooms"))
            {
                if (currentMenu.dimension == StateUIManager.Dimension.TWOD)
                    AlgosUtils.SetUpTileTypes(currentMenu.gridArrayObj2D, wallDiag);

                else if (currentMenu.dimension == StateUIManager.Dimension.PLANE)
                {
                    rooms = AlgosUtils.GetAllRooms(currentMenu.gridArray2D,true);
                    currentMenu.plane.GetComponent<Renderer>().material.mainTexture = AlgosUtils.SetUpTextSelfCol(currentMenu.gridArray2D);
                }
            }


            if (GUI.Button(new Rect(10, 400, 120, 30), "Connect all the rooms"))
            {
                //if (currentMenu.dimension == StateUIManager.Dimension.TWOD)
                //    AlgosUtils.SetUpTileTypes(currentMenu.gridArrayObj2D, wallDiag);

                //else if (currentMenu.dimension == StateUIManager.Dimension.PLANE)
                //{
                //    AlgosUtils.GetAllRooms(currentMenu.gridArray2D, true);
                //    currentMenu.plane.GetComponent<Renderer>().material.mainTexture = AlgosUtils.SetUpTextSelfCol(currentMenu.gridArray2D);
                //}

                var centerPoints = new List<Vector2>();
                var roomDict = new Dictionary<Vector2, List<BasicTile>>();
                foreach (var room in rooms)
                {
                    roomDict.Add(AlgosUtils.FindMiddlePoint(room), room);
                    centerPoints.Add(AlgosUtils.FindMiddlePoint(room));
                }


                

                if (typeOfTri)
                    primEdges = AlgosUtils.PrimAlgoNoDelu(centerPoints);
                else
                    primEdges = AlgosUtils.DelunayTriangulation2D(centerPoints).Item2;


                foreach (var edge in primEdges)
                {

                    //use where so we get soemthing its not the wall but not necessary
                    var tileA = roomDict[edge.edge[0]][Random.Range(0, roomDict[edge.edge[0]].Count)].position;
                    var tileB = roomDict[edge.edge[1]][Random.Range(0, roomDict[edge.edge[1]].Count)].position;


                    var path = AlgosUtils.A_StarPathfinding2DNorm(currentMenu.gridArray2D, new Vector2Int(tileA.x, tileA.y), new Vector2Int(tileB.x, tileB.y), false);


                    foreach (var tile in path.Item1)
                    {
                        if (tile.tileType != BasicTile.TileType.FLOORROOM) 
                            tile.tileType = BasicTile.TileType.FLOORCORRIDOR;

                        tile.tileWeight = 0.75f;
                    }
                }

                currentMenu.plane.GetComponent<Renderer>().material.mainTexture = AlgosUtils.SetUpTextBiColShade(currentMenu.gridArray2D, 0, 1, true);
            }

            if (GUI.Button(new Rect(10, 440, 120, 30), "Gen mesh"))
            {
                //var something  = AlgosUtils.ExtrapolateMarchingCubes(currentMenu.gridArray2D);
                currentMenu.FormObject(AlgosUtils.MarchingCubesAlgo(AlgosUtils.ExtrapolateMarchingCubes(currentMenu.gridArray2D), false));
            }


            if (GUI.Button(new Rect(10, 500, 120, 30), "check size"))
            {
                //var something  = AlgosUtils.ExtrapolateMarchingCubes(currentMenu.gridArray2D);
                foreach (var room in rooms)
                {
                    if(room.Count < minSize) 
                    {
                        foreach (var tile in room)
                        {
                            tile.tileWeight = 0;
                            tile.tileType = BasicTile.TileType.VOID;
                        }
                        
                    }
                }

                currentMenu.plane.GetComponent<Renderer>().material.mainTexture = AlgosUtils.SetUpTextBiColShade(currentMenu.gridArray2D, 0, 1, true);

            }



            minSize = (int)GUI.HorizontalSlider(new Rect(10, 480, 100, 20), minSize, 20, 100);

            if (GUI.Button(new Rect(10, 540, 100, 30), "Go back"))
                currentMenu.ChangeState(0);
        }

    }

    public override void onStart(StateUIManager currentMenu)
    {
    }

    public override void onUpdate(StateUIManager currentMenu)
    {
    }


   

    public override void onGizmos(StateUIManager currentMenu)
    {

        //foreach (var edge in primEdges)
        //{
        //    Debug.DrawLine(new Vector3(edge.edge[0].x, edge.edge[0].y, edge.edge[0].z), new Vector3(edge.edge[1].x, edge.edge[1].y, edge.edge[1].z), Color.green);
        //}
    }


    private void DrunkWalkObj2D(StateUIManager currentMenu) 
    {
        if (currentMenu.dimension == StateUIManager.Dimension.TWOD)
        {
            currentMenu.working = true;
            AlgosUtils.RandomWalk2DCol(currentMenu.gridArrayObj2D, iterations, alreadyPassed);    // might need to change this  

            currentMenu.working = false;
        }
    }


    private void DrunkWalk2D(StateUIManager currentMenu)
    {
        if (currentMenu.dimension == StateUIManager.Dimension.PLANE)
        {
            currentMenu.working = true;

            currentMenu.gridArray2D = AlgosUtils.RandomWalk2DCol(iterations, alreadyPassed, currentMenu.width, currentMenu.height);

            currentMenu.plane.GetComponent<Renderer>().material.mainTexture = AlgosUtils.SetUpTextBiColAnchor(currentMenu.gridArray2D);

            currentMenu.working = false;
        }
    }


}
