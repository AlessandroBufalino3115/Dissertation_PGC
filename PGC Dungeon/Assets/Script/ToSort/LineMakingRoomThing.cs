using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static CellularAutomataBasicAlgo;
using static UnityEditor.Progress;

public class LineMakingRoomThing : MonoBehaviour
{

    [SerializeField]
    public List<Identifier> rulesList = new List<Identifier>();


    private Dictionary<int,float> rulesDict = new Dictionary<int,float>();


    public static LineMakingRoomThing instance;

    [Range(4, 25)]
    public int linesX;

    [Range(4, 25)]
    public int linesY;

    [Range(4, 24)]
    public int roomsNum;


    int[,] childPosArry = { { 0, -1 }, { -1, 0 }, { 1, 0 }, { 0, 1 } };
    // it is possible that the above thigns completly break everything 


    public int test_x = 0;
    public int test_y = 0;

    Tile[][] gridArray2D = new Tile[1][];

    List<LineMakerRoom2D> roomsList = new List<LineMakerRoom2D>();

    private void Awake()
    {
        instance = this;


        foreach (var item in rulesList)
        {
            rulesDict.Add((int)item.tileType, item.cost);
        }


    }

    public void GenRoomLines2D(Tile[][] _gridArray)
    {
        int lengthY = _gridArray.Length;
        int lengthX = _gridArray[0].Length;



        for (int i = 0; i < lengthX; i++)
        {
            _gridArray[0][i].tileType = Tile.TileType.AVOID;

            _gridArray[lengthY - 1][i].tileType = Tile.TileType.AVOID;
        }

        for (int i = 0; i < lengthY; i++)
        {
            _gridArray[i][0].tileType = Tile.TileType.AVOID;

            _gridArray[i][lengthX - 1].tileType = Tile.TileType.AVOID;
        }


        List<int> coloumsX = new List<int>();
        List<int> coloumsY = new List<int>();


        int tries = 0;


        while (coloumsX.Count <= linesX)
        {
            tries++;


            if (tries == 400)
            {

                break;
            }


            int ran = Random.Range(4, lengthX - 5);

            if (coloumsX.Contains(ran)) { continue; }

            bool tooClose = false;


            for (int i = 0; i < coloumsX.Count; i++)
            {
                if (Mathf.Abs(ran - coloumsX[i]) <= 4)
                {
                    tooClose = true;
                    break;
                }
            }

            if (tooClose == false)
            {
                coloumsX.Add(ran);

            }

        }


        tries = 0;


        while (coloumsY.Count <= linesY)
        {
            tries++;


            if (tries == 400)
            {

                break;
            }



            int ran = Random.Range(4, lengthY - 5);

            if (coloumsY.Contains(ran)) { continue; }

            bool tooClose = false;


            for (int i = 0; i < coloumsY.Count; i++)
            {
                if (Mathf.Abs(ran - coloumsY[i]) <= 4)
                {
                    tooClose = true;
                    break;
                }
            }

            if (tooClose == false)
            {
                coloumsY.Add(ran);
            }

        }


        foreach (var item in coloumsX)
        {
            for (int i = 1; i < lengthY; i++)
            {
                if (_gridArray[i][item].tileType == Tile.TileType.AVOID)
                {
                    break;
                }
                else
                {
                    _gridArray[i][item].tileType = Tile.TileType.AVOID;

                }
            }

        }

        foreach (var item in coloumsY)
        {

            float start = Random.Range(0f, 1f);

            if (start > 0.5)
            {
                for (int i = 1; i < lengthX; i++)
                {
                    if (_gridArray[item][i].tileType == Tile.TileType.AVOID)
                    {
                        float ran = Random.Range(0f, 1f);

                        if (ran >= 0.10f)
                        {
                            _gridArray[item][i].tileType = Tile.TileType.AVOID;
                        }
                        else
                        {
                            break;
                        }

                    }
                    else
                    {
                        _gridArray[item][i].tileType = Tile.TileType.AVOID;
                    }
                }



            }
            else
            {


                for (int i = 1; i < lengthX; i++)
                {
                    if (_gridArray[item][lengthX - i - 1].tileType == Tile.TileType.AVOID)
                    {
                        float ran = Random.Range(0f, 1f);

                        if (ran >= 0.10f)
                        {
                            _gridArray[item][lengthX - i - 1].tileType = Tile.TileType.AVOID;

                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        _gridArray[item][lengthX - i - 1].tileType = Tile.TileType.AVOID;

                    }
                }
            }
        }

        DecideRooms2D(_gridArray, lengthY, lengthX);
    }

    public void DecideRooms2D(Tile[][] _gridArray, int lengthY, int lengthX)
    {

        gridArray2D = _gridArray;


        List<string> takenPositions = new List<string>();


        for (int y = 0; y < lengthY; y++)
        {
            for (int x = 0; x < lengthX; x++)
            {
                if (_gridArray[y][x].tileType == Tile.TileType.AVOID)
                {
                    takenPositions.Add(_gridArray[y][x].tileObj.transform.name);
                }
            }
        }

        for (int i = 0; i < roomsNum; i++)
        {

            int x = 0;
            int y = 0;

            //int tries = 400;

            while (true)
            {
                //tries = tries + 1;

                //if (tries == 400)
                //{
                //    break;
                //}

                int ranX = Random.Range(1, lengthX - 1);
                int ranY = Random.Range(1, lengthY - 1);

                string pos = ranX + " " + ranY;

                if (!takenPositions.Contains(pos))
                {

                    y = ranY;
                    x = ranX;

                    takenPositions.Add(pos);
                    break;
                }
            }

            int count = roomsList.Count;
            roomsList.Add(new LineMakerRoom2D(count));
            Flood2D(x, y, Color.grey, i);

            foreach (var item in roomsList[i].position)
            {
                string pos = item.x + " " + item.y;
                takenPositions.Add(pos);
            }

        }


        ConnectTheRooms();

    }



    public void ConnectTheRooms()
    {
        List<int> roomID = new List<int>();




        for (int i = 0; i < roomsList.Count; i++)
        {
            roomID.Add(i);
        }

        int tries = 0;
        while (roomID.Count > 0)
        {
            tries++;


            if (tries > 200) { break; }


            int roomOne = Random.Range(0, roomID.Count);
            roomOne = roomID[roomOne];
            roomID.Remove(roomOne);


            int roomTwo = Random.Range(0, roomID.Count);
            roomTwo = roomID[roomTwo];
            roomID.Remove(roomTwo);

            int roomThree = Random.Range(0, roomsList.Count);


            int startX = roomsList[roomOne].position[Random.Range(0, roomsList[roomOne].position.Count - 1)][0];
            int startY = roomsList[roomOne].position[Random.Range(0, roomsList[roomOne].position.Count - 1)][1];

            int endX = roomsList[roomTwo].position[Random.Range(0, roomsList[roomTwo].position.Count - 1)][0];
            int endY = roomsList[roomTwo].position[Random.Range(0, roomsList[roomTwo].position.Count - 1)][1];


            var corridor = APathUtils.A_StarPathfinding2DWeight(gridArray2D, new Vector2Int(startX, startY), new Vector2Int(endX, endY), rulesDict);
            foreach (var node in corridor.Item1)
            {

                node.refToGameObj.tileObj.GetComponent<MeshRenderer>().material.color = Color.grey;

                if (node.refToGameObj.tileType != Tile.TileType.FLOORROOM)
                    node.refToGameObj.tileType = Tile.TileType.FLOORCORRIDOR;

            }

            endX = roomsList[roomThree].position[Random.Range(0, roomsList[roomThree].position.Count - 1)][0];
            endY = roomsList[roomThree].position[Random.Range(0, roomsList[roomThree].position.Count - 1)][1];

            corridor = APathUtils.A_StarPathfinding2DWeight(gridArray2D, new Vector2Int(startX, startY), new Vector2Int(endX, endY), rulesDict);
            foreach (var node in corridor.Item1)
            {

                node.refToGameObj.tileObj.GetComponent<MeshRenderer>().material.color = Color.grey;

                if (node.refToGameObj.tileType != Tile.TileType.FLOORROOM)
                    node.refToGameObj.tileType = Tile.TileType.FLOORCORRIDOR;

            }
        }

    }



    //this will change
    private void Flood2D(int x, int y, Color col, int i)
    {
        if (y >= 0 && x >= 0 && y < gridArray2D.Length && x < gridArray2D[y].Length)
        {
            if (gridArray2D[y][x].tileType != Tile.TileType.AVOID && gridArray2D[y][x].tileType != Tile.TileType.FLOORROOM)
            {

                roomsList[i].position.Add(new Vector2Int( x, y ));

                gridArray2D[y][x].tileObj.GetComponent<MeshRenderer>().material.color = col;
                gridArray2D[y][x].tileType = Tile.TileType.FLOORROOM;

                Flood2D(x + 1, y, col, i);
                Flood2D(x - 1, y, col, i);
                Flood2D(x, y + 1, col, i);
                Flood2D(x, y - 1, col, i);
            }
        }
    }

}




public class LineMakerRoom2D
{
    public List<Vector2Int> position = new List<Vector2Int>();
    public int indexRoom;



    public LineMakerRoom2D( int indexRoom)
    {
        this.indexRoom = indexRoom;
    }
}
