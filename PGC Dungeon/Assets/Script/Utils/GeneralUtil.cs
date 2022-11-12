using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class GeneralUtil 
{

    public static int[,] childPosArry4Side = { { 0, -1 }, { -1, 0 }, { 1, 0 }, { 0, 1 } };
    public static int[,] childPosArry8Side = { { 0, -1 }, { 1, -1 }, { -1, -1 },  { -1, 0 }, { 1, 0 },    { 0, 1 }, { 1, 1 }, { -1, 1 } };





    /// <summary>
    /// from 0 
    /// </summary>
    /// <param name="maxX"></param>
    /// <param name="maxY"></param>
    /// <returns></returns>
    public static Vector2Int RanVector2Int(int maxX,int maxY) 
    {
        int ranX = Random.Range(0,maxX);
        int ranY = Random.Range(0, maxY);


        return new Vector2Int(ranX, ranY);
    }


    public static Vector2 RanVector2Float(float maxX, float maxY)
    {
        float ranX = Random.Range(0f, maxX);
        float ranY = Random.Range(0f, maxY);

        return new Vector2(ranX, ranY);
    }







    public static float EuclideanDistance2D(Vector2 point1, Vector2 point2)
    {
        return MathF.Sqrt(MathF.Pow((point1.x - point2.x), 2) + MathF.Pow((point1.y - point2.y), 2));
    }

    public static float ManhattanDistance2D(Vector2 point1, Vector2 point2)
    {
        return Mathf.Abs((point1.x - point2.x)) + Mathf.Abs((point1.y - point2.y));
    }


    public static List<Tile> SolveA_StarPathfinding2D4Side(Tile[][] gridArray2D, int startX, int startY, int endX, int endY)
    {
        List<AStar_Node> openList = new List<AStar_Node>();
        List<AStar_Node> closedList = new List<AStar_Node>();


        AStar_Node start_node = new AStar_Node(gridArray2D[startY][startX]);
        start_node.parent = null;

        AStar_Node end_node = new AStar_Node(gridArray2D[endY][endX]);


        openList.Add(start_node);



        while (openList.Count > 0)
        {


            AStar_Node currNode = openList[0];
            int currIndex = 0;
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].f < currNode.f)
                {
                    currNode = openList[i];
                    currIndex = i;
                }
            }

            openList.RemoveAt(currIndex);
            closedList.Add(currNode);

            if ((currNode.refToGameObj.position.x == end_node.refToGameObj.position.x && currNode.refToGameObj.position.y == end_node.refToGameObj.position.y))
            {

                List<AStar_Node> path = new List<AStar_Node>();

                AStar_Node current = currNode;

                while (current.parent != null)
                {
                    path.Add(current);
                    current = current.parent;
                }

                List<Tile> tiles = new List<Tile>();

                foreach (var tile in path)
                {
                    int xCord = tile.refToGameObj.position.x;
                    int yCord = tile.refToGameObj.position.y;

                    tiles.Add(gridArray2D[yCord][xCord]);
                }


                return tiles;

            }
            else
            {
                List<AStar_Node> children = new List<AStar_Node>();


                for (int i = 0; i < childPosArry4Side.Length / 2; i++)
                {
                    int x_buff = childPosArry4Side[i, 0];
                    int y_buff = childPosArry4Side[i, 1];

                    int[] node_position = { currNode.refToGameObj.position.x + x_buff, currNode.refToGameObj.position.y + y_buff };


                    if (node_position[0] < 0 || node_position[1] < 0 || node_position[0] >= TileVolumeGenerator.Instance.x_Length || node_position[1] >= TileVolumeGenerator.Instance.y_Height)
                    {
                        continue;
                    }
                    else
                    {
                        //here an if statment also saying that walkable 
                        AStar_Node new_node = new AStar_Node(gridArray2D[node_position[1]][node_position[0]]);
                        children.Add(new_node);
                    }
                }

                foreach (var child in children)
                {
                    foreach (var closedListItem in closedList)
                    {
                        if (child.refToGameObj.position.x == closedListItem.refToGameObj.position.x && child.refToGameObj.position.y == closedListItem.refToGameObj.position.y)
                        {
                            continue;
                        }
                    }


                    float addedCost = 0;
                    
                    child.g = currNode.g + 0.3f;
                    child.h = GeneralUtil.EuclideanDistance2D(new Vector2(end_node.refToGameObj.position.x, end_node.refToGameObj.position.y), new Vector2(child.refToGameObj.position.x, child.refToGameObj.position.y));
                    child.f = child.g + child.h + addedCost;
                    child.parent = currNode;


                    foreach (var openListItem in openList)
                    {
                        if (child.refToGameObj.position.x == openListItem.refToGameObj.position.x && child.refToGameObj.position.y == openListItem.refToGameObj.position.y && child.g > openListItem.g)
                        {
                            continue;
                        }
                    }

                    openList.Add(child);

                }
            }
        }

        return null;

    }

}




//do some ineritance

public class AStar_Node
{

    public Tile refToGameObj;
    public AStar_Node parent;

    public float g = 0;
    public float f = 0;
    public float h = 0;

    public AStar_Node(Tile gameobject)
    {
        refToGameObj = gameobject;
    }

}


public class Tile
{
    public GameObject tileObj;
    public Vector3Int position = new Vector3Int();

    public enum TileType
    {
        VOID,
        FLOORROOM,
        WALL,
        ROOF,
        FLOORCORRIDOR,
        AVOID

    }
    public TileType tileType = 0;

    public Tile(GameObject _arrayTileObj, Vector2Int _pos)
    {
        tileObj = _arrayTileObj;
        position = new Vector3Int(_pos.x, 0, _pos.y);
    }
    public Tile(GameObject _arrayTileObj, Vector3Int _pos)
    {
        tileObj = _arrayTileObj;
        position = _pos;
    }
    public Tile(GameObject _arrayTileObj, int _x, int _y, int _z)
    {
        tileObj = _arrayTileObj;

        position = new Vector3Int(_x, _y, _z);
    }
    public Tile(GameObject _arrayTileObj, int _x, int _y)
    {
        tileObj = _arrayTileObj;
        position = new Vector3Int(_x, 0, _y);
    }


    public void SetTileWorldPos() => tileObj.transform.position = position;
}


[System.Serializable]
public class Identifier 
{
    public Color color = Color.black;

    public enum TileType
    {
        VOID,
        FLOORROOM,
        WALL,
        ROOF,
        FLOORCORRIDOR,
        AVOID
    }

    public TileType tileType = 0;
    public float cost = 0;
}

