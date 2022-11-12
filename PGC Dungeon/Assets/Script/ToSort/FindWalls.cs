using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindWalls : MonoBehaviour
{
    public static FindWalls instance;

    List<FloodFillTile> allTiles = new List<FloodFillTile>();

    List<FloodFillTile> wallsTiles = new List<FloodFillTile>();
    List<FloodFillTile> floorTiles = new List<FloodFillTile>();



    private void Start()
    {
        instance = this;
    }


    public void FindWalls2D(Tile[][] _gridArray2d) 
    {

        Debug.Log("this is where it starts");
        var ran = GeneralUtil.RanVector2Int(_gridArray2d.Length, _gridArray2d[0].Length);

        while (true) 
        {
            if (_gridArray2d[ran.y][ran.x].tileObj.GetComponent<MeshRenderer>().material.color != Color.white) 
            {

                Debug.Log("the ran generated is good");
                break;
            }
            else 
            {
                ran = GeneralUtil.RanVector2Int(_gridArray2d.Length, _gridArray2d[0].Length);
                
                Debug.Log("the ran generated is bad");
            }
        }



        var floodFillArr = new FloodFillTile[_gridArray2d.Length][];

        for (int y = 0; y < floodFillArr.Length; y++)
        {
            floodFillArr[y] = new FloodFillTile[_gridArray2d[0].Length];

            for (int x = 0; x < floodFillArr[y].Length; x++)
            {

                floodFillArr[y][x] = new FloodFillTile();

                floodFillArr[y][x].position =  _gridArray2d[y][x].position;
                floodFillArr[y][x].tileObj = _gridArray2d[y][x].tileObj;


            }
        }









        Flood2DWallsFinder(floodFillArr, ran.x, ran.y, Color.white);



        Debug.Log($"this is the size of the tile after floodfill {allTiles.Count}");

        DistinguishTile(floodFillArr, allTiles, Color.gray);
        Debug.Log($"this is the size of the floor after floodfill {floorTiles.Count}");
        Debug.Log($"this is the size of the walls after floodfill {wallsTiles.Count}");
    }



    private void Flood2DWallsFinder(FloodFillTile[][] gridArray2D, int x, int y, Color colorCheck)
    {
        if (y >= 0 && x >= 0 && y < gridArray2D.Length && x < gridArray2D[y].Length)
        {

            if (gridArray2D[y][x].passed == false && gridArray2D[y][x].tileObj.GetComponent<MeshRenderer>().material.color != colorCheck)
            {
                gridArray2D[y][x].passed = true;
                allTiles.Add(gridArray2D[y][x]);

                Flood2DWallsFinder( gridArray2D,   x + 1, y, colorCheck);
                Flood2DWallsFinder(gridArray2D, x - 1, y, colorCheck);
                Flood2DWallsFinder(gridArray2D, x, y + 1, colorCheck);
                Flood2DWallsFinder(gridArray2D, x, y - 1, colorCheck);
            }
        }

    }


    private void DistinguishTile(FloodFillTile[][] gridArray2D, List<FloodFillTile> allTilesArray, Color floorColor) 
    {
        for (int i = allTilesArray.Count; i-- > 0;)
        {
            bool wallTrue = true;

            //need to check the dimesion in the array
            // need to check the color


            Debug.Log($"{allTilesArray[i].position.z}   ,   {allTilesArray[i].position.x}");
            if (allTilesArray[i].position.z + 1 < gridArray2D.Length) 
            {
                if (gridArray2D[allTilesArray[i].position.z + 1][allTilesArray[i].position.x].tileObj.GetComponent<MeshRenderer>().material.color == floorColor) 
                {
                    wallTrue = true;
                }
                else 
                {
                    wallTrue = false; 
                }
            }
            else { wallTrue = false; }





            if (allTilesArray[i].position.z - 1 >= 0 && wallTrue == true)
            {
                if (gridArray2D[allTilesArray[i].position.z - 1][allTilesArray[i].position.x].tileObj.GetComponent<MeshRenderer>().material.color == floorColor)
                {
                    wallTrue = true;
                }
                else
                {
                    wallTrue = false;
                }
            }
            else { wallTrue = false; }







            if (allTilesArray[i].position.x - 1 >= 0 && wallTrue == true)
            {
                if (gridArray2D[allTilesArray[i].position.z][allTilesArray[i].position.x - 1].tileObj.GetComponent<MeshRenderer>().material.color == floorColor)
                {
                    wallTrue = true;
                }
                else
                {
                    wallTrue = false;
                }
            }
            else { wallTrue = false; }



            // its getting reset over here
            if (allTilesArray[i].position.x + 1 < gridArray2D[0].Length && wallTrue == true)
            {
                if (gridArray2D[allTilesArray[i].position.z][allTilesArray[i].position.x + 1].tileObj.GetComponent<MeshRenderer>().material.color == floorColor)
                {
                    wallTrue = true;
                }
                else
                {
                    wallTrue = false;
                }
            }
            else { wallTrue = false; }




            if (!wallTrue) 
            {
                wallsTiles.Add(allTilesArray[i]);
            }
            else 
            {
                floorTiles.Add(allTilesArray[i]);
            }


            allTilesArray.RemoveAt(i);


        }








        foreach (var item in wallsTiles)
        {
            item.tileObj.GetComponent<MeshRenderer>().material.color = Color.red;
        }


    }



}




public class FloodFillTile 
{
    public GameObject tileObj;
    public Vector3Int position = new Vector3Int();
    public bool passed = false;
}
