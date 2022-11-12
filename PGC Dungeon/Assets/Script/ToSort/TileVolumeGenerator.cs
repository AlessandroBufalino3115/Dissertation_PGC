using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UIElements;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TileVolumeGenerator : MonoBehaviour
{



    public bool scaleToggle;

    public int x_Length;
    public int y_Height;
    public int z_Width;

    [SerializeField] GameObject emptyBlock;
    [SerializeField] GameObject CubeBlock;
    public bool clearBlock = false;
    public bool Gizmos;

    /*
     *  void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
    }
     * 
     * 
     */




    public static TileVolumeGenerator Instance;


    public int acceptedRoomFailures;


    Tile[][][] gridArray3D = new Tile[1][][];

    Tile[][] gridArray2D = new Tile[1][];


    private void Awake()
    {
        Instance = this;
    }

    public void Gen3DVolume()
    {
        int timerStart = Environment.TickCount & Int32.MaxValue;
        int blockNum = 0;

        gridArray3D = new Tile[z_Width][][];

        for (int z = 0; z < gridArray3D.Length; z++)
        {

            gridArray3D[z] = new Tile[y_Height][];
            for (int y = 0; y < gridArray3D[z].Length; y++)
            {

                gridArray3D[z][y] = new Tile[x_Length];

                for (int x = 0; x < gridArray3D[z][y].Length; x++)
                {
                    Vector3 position = new Vector3(x, y, z);



                    GameObject newRef = null;

                    if (clearBlock)
                    {
                        newRef = Instantiate(emptyBlock, this.gameObject.transform);
                    }
                    else
                    {
                        newRef = Instantiate(CubeBlock, this.gameObject.transform);
                    }

                    newRef.transform.position = position;
                    if (scaleToggle)
                    {
                        newRef.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    }

                    newRef.transform.name = x.ToString() + " " + y.ToString();

                    gridArray3D[z][y][x] = new Tile(newRef, x, y, z);
                    gridArray3D[z][y][x].tileType = Tile.TileType.VOID;

                    blockNum++;
                }
            }
        }


        int half_x = (x_Length - 1) / 2;
        int half_y = (y_Height - 1) / 2;
        int half_z = (z_Width - 1) / 2;



        gridArray3D[half_z][half_y][half_x].tileObj.GetComponent<MeshRenderer>().material.color = Color.yellow;
        int timerEnd = Environment.TickCount & Int32.MaxValue;

        int totalTicks = timerEnd - timerStart;
        Debug.Log($"<color=yellow>Performance: The total time this has taken was {totalTicks} Ticks, to generate {blockNum} positions</color>");

    }



    public void Gen2DVolume()
    {
        int timerStart = Environment.TickCount & Int32.MaxValue;
        int blockNum = 0;

        gridArray2D = new Tile[y_Height][];

        for (int y = 0; y < gridArray2D.Length; y++)
        {
            gridArray2D[y] = new Tile[x_Length];

            for (int x = 0; x < gridArray2D[y].Length; x++)
            {
                Vector3 position = new Vector3(x, 0, y);



                GameObject newRef = null;

                if (clearBlock)
                {
                    newRef = Instantiate(emptyBlock, this.gameObject.transform);
                }
                else
                {
                    newRef = Instantiate(CubeBlock, this.gameObject.transform);
                }

                newRef.transform.position = position;

                if (scaleToggle)
                {
                    newRef.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                }
                newRef.transform.name = x.ToString() + " " + y.ToString();

                gridArray2D[y][x] = new Tile(newRef, x, y);
                gridArray2D[y][x].tileType = Tile.TileType.VOID;

                blockNum++;
            }
        }


        int half_x = (x_Length - 1) / 2;
        int half_y = (y_Height - 1) / 2;

        gridArray2D[half_y][half_x].tileObj.GetComponent<MeshRenderer>().material.color = Color.yellow;

        int timerEnd = Environment.TickCount & Int32.MaxValue;

        int totalTicks = timerEnd - timerStart;
        Debug.Log($"<color=yellow>Performance: The total time this has taken was {totalTicks} Ticks, to generate {blockNum} positions</color>");
        // 1 tick seems to be 1 millisecond
    }



    public void DestroyAllTiles()
    {
        int timerStart = Environment.TickCount & Int32.MaxValue;


        foreach (Transform child in transform)
            Destroy(child.gameObject);


        gridArray3D = new Tile[1][][];

        gridArray2D = new Tile[1][];

        int timerEnd = Environment.TickCount & Int32.MaxValue;

        int totalTicks = timerEnd - timerStart;



        Debug.Log($"<color=yellow>Performance: The total time that destorying all the children has taken was {totalTicks}</color>");

    }



    //testing
    public void CombineMeshes()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];


        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        transform.gameObject.SetActive(true);

    }







    public void CallWallsFind() => FindWalls.instance.FindWalls2D(gridArray2D);
    
    public void CallBasicPerlinNoise2D() => PerlinNoise.Instance.GenPerlinNoise2D(gridArray2D);
    public void CallBasicPerlinNoise3D() => PerlinNoise.Instance.GenPerlinNoise3D(gridArray3D);
    public void Init2DCa() => CellularAutomataBasicAlgo.instance.Init2DCallAuto(gridArray2D, x_Length, y_Height);

    public void InitA_StarPathFinding() => A_StarPathFinding.instance.SolveA_StarPathfinding2DTest(gridArray2D);

    public void CallForVoronoi() => VoronoiDiagram.instance.CallVoronoiGen2D(gridArray2D);

    public void CallGenLinesRoom2D() => LineMakingRoomThing.instance.GenRoomLines2D(gridArray2D);

    public void CallDrunkWalk2D() => DrunkWalkAlgo.instance.DrunkWalk2D(gridArray2D);
    public void CallDrunkWalk3D() => DrunkWalkAlgo.instance.DrunkWalk3D(gridArray3D);

    /*
    public void RoomSpawner()
    {

        int stratoSize = volumeLength * volumeWidth;

        int offsetIndex = 0;

        int currentFailures;


        for (int x = 0; x < volumeFloors; x++)
        {
            for (int i = 0; i < stratoSize; i++)
            {
                if (clearBlock == false)
                    Tiles[i + offsetIndex].transform.GetComponent<MeshRenderer>().material.color = Color.black;

                if (i == 140)
                    ActuallySpawnRoom(i + offsetIndex);



            }
            offsetIndex = stratoSize * 3;
        }


    }



    needs to pick a random start index on then call this 
    half way it all prinintg that should be the middle so use that as the main connection node



     i need the origin point to be 1 away from any other room
    to delete
    public void ActuallySpawnRoom(int originIndex)
    {

        int width = Random.Range(3, 9);
        int length = Random.Range(3, 9);

        int targetindex = originIndex;

        for (int i = 0; i < width; i++)
        {
            for (int x = 0; x < length; x++)
            {
                if (clearBlock == true)
                {
                    GameObject newRef = Instantiate(CubeBlock, this.gameObject.transform);
                    newRef.transform.position = new Vector3(Tiles[targetindex].gameObject.transform.position.x, Tiles[targetindex].gameObject.transform.position.y, Tiles[targetindex].gameObject.transform.position.z);
                }
                else
                    Tiles[targetindex].transform.GetComponent<MeshRenderer>().material.color = Color.red;
                targetindex--;
            }
            targetindex -= volumeWidth;
            targetindex += length;
        }
    }
    */
}




/*
public class Room
{
    public List<GameObject> roomTiles = new List<GameObject>();
    public Vector2 pos2D;
    public Vector3 pos3D;


    public Room() { }
}
*/