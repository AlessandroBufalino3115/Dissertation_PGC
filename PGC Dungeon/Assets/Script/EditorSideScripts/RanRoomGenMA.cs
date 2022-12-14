using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RanRoomGenMA : MonoBehaviour
{
    private PCGManager pcgManager;
    public PCGManager PcgManager
    {
        get { return pcgManager; }
    }






    private int maxWidth;
    public int MaxWidth
    {
        get { return maxWidth; }
        set { maxWidth = value; }
    }


    private int minWidth;
    public int MinWidth
    {
        get { return minWidth; }
        set { minWidth = value; }
    }


    private int maxHeight;
    public int MaxHeight
    {
        get { return maxHeight; }
        set { maxHeight = value; }
    }


    private int minHeight;
    public int MinHeight
    {
        get { return minHeight; }
        set { minHeight = value; }
    }


    private int numOfRoom;
    public int NumOfRoom
    {
        get { return numOfRoom; }
        set { numOfRoom = value; }
    }


    private bool BPS;
    public bool BPSg
    {
        get { return BPS; }
        set { BPS = value; }
    }



    private bool additive;
    public bool Additive
    {
        get { return additive; }
        set { additive = value; }
    }


    public List<RoomsClassInUI> roomList = new List<RoomsClassInUI>();
    private List<BoundsInt> roomsListBPSAlgo = new List<BoundsInt>();

    public List<BoundsInt> RoomList
    {
        get { return roomsListBPSAlgo; }
    }






    public List<List<BasicTile>> rooms = new List<List<BasicTile>>();
    public List<Edge> edges = new List<Edge>();



    public enum PathFindingType
    {
        A_STAR,
        DJISTRA,
        BFS,
        DFS
    }
    private PathFindingType pathFindingType;
    public PathFindingType PathfindingType
    {
        get { return pathFindingType; }
        set { pathFindingType = value; }
    }






    public void InspectorAwake()
    {
        pcgManager = this.transform.GetComponent<PCGManager>();
        roomList.Clear();
        roomsListBPSAlgo.Clear();
    }





    public void SetUpWeights(BasicTile[][] gridArr)
    {
        foreach (var room in roomList)
        {
            for (int i = 0; i < room.tileCoords.Count; i++)
            {
                gridArr[room.tileCoords[i].y][room.tileCoords[i].x].tileWeight = 1;
                gridArr[room.tileCoords[i].y][room.tileCoords[i].x].tileType = BasicTile.TileType.FLOORROOM;
            }
        }
    }






    #region Random Room Gen

    public void RandomRoomGen(BasicTile[][] gridArray2D)
    {
        int tries = numOfRoom * 4;


        while (tries > 0)
        {
            tries--;


            if (roomList.Count >= numOfRoom) { break; }


            Vector2Int ranStartPoint = new Vector2Int(Random.Range(0, gridArray2D[0].Length), Random.Range(0, gridArray2D.Length));

            RoomsClassInUI currRoom = new RoomsClassInUI();

            int ranWidth = Random.Range(minWidth, maxWidth);
            if (Random.value > 0.5f)
                ranWidth *= -1;


            int ranHeight = Random.Range(minHeight, minWidth);
            if (Random.value > 0.5f)
                ranHeight *= -1;



            if (ranWidth < 0)   // so the added withd is negative so the ranPoint x is the most positve
            {
                if (ranStartPoint.x + ranWidth < 0)
                    continue;


                currRoom.maxX = ranStartPoint.x;
                currRoom.minX = ranStartPoint.x + ranWidth;
            }
            else
            {

                if (ranStartPoint.x + ranWidth > gridArray2D[0].Length - 1)
                    continue;

                currRoom.minX = ranStartPoint.x;
                currRoom.maxX = ranStartPoint.x + ranWidth;


            }



            if (ranHeight < 0)   // so the added withd is negative so the ranPoint x is the most positve
            {
                if (ranStartPoint.y + ranHeight < 0)
                    continue;

                currRoom.maxY = ranStartPoint.y;
                currRoom.minY = ranStartPoint.y + ranHeight;

            }
            else
            {

                if (ranStartPoint.y + ranHeight > gridArray2D.Length - 1)
                    continue;

                currRoom.minY = ranStartPoint.y;
                currRoom.maxY = ranStartPoint.y + ranHeight;

            }

            currRoom.WorkOutCoords();

            bool toAdd = true;

            for (int i = 0; i < roomList.Count; i++)
            {
                if (AABBCol(currRoom, roomList[i]))
                {
                    //Debug.Log($"DId this call {roomsList.Count}");
                    toAdd = false;
                    break;
                }
            }


            if (toAdd)
            {
                WorkoutRegion(currRoom);

                roomList.Add(currRoom);
            }
        }
    }


    /// <summary>
    /// returns true when something is hit ---- return false when nothing is hit
    /// </summary>
    /// <param name="newRoom"></param>
    /// <param name="oldRoom"></param>
    /// <returns></returns>
    public bool AABBCol(RoomsClassInUI newRoom, RoomsClassInUI oldRoom)
    {

        for (int i = 0; i < newRoom.verticesList.Count; i++)
        {
            if ((oldRoom.minX <= newRoom.verticesList[i].x && newRoom.verticesList[i].x <= oldRoom.maxX) && (oldRoom.minY <= newRoom.verticesList[i].y && newRoom.verticesList[i].y <= oldRoom.maxY))
            {
                return true;
            }
        }


        return false;
    }


    #endregion




    public void WorkoutRegion(RoomsClassInUI room)
    {
        room.tileCoords = new List<Vector2Int>();

        for (int y = room.minY; y < room.maxY + 1; y++)
        {
            for (int x = room.minX; x < room.maxX + 1; x++)
            {
                room.tileCoords.Add(new Vector2Int(x, y));
            }
        }
    }


    #region BPS




    public void BPSRoomGen(BasicTile[][] gridArray2D)
    {
        BoundsInt map = new BoundsInt();

        map.min = new Vector3Int(0, 0, 0);
        map.max = new Vector3Int(gridArray2D[0].Length - 1, 0, gridArray2D.Length - 1);

        roomsListBPSAlgo = BPSAlgo2d(map);

        while (roomsListBPSAlgo.Count > numOfRoom)
        {
            int ranIDX = Random.Range(0, roomsListBPSAlgo.Count - 1);

            roomsListBPSAlgo.RemoveAt(ranIDX);
        }


        BoundsToWeights();

    }

    public void BoundsToWeights()
    {
        roomList.Clear();

        foreach (var room in roomsListBPSAlgo)
        {
            roomList.Add(new RoomsClassInUI() { maxX = room.xMax, maxY = room.zMax, minX = room.xMin, minY = room.zMin });

            WorkoutRegion(roomList[roomList.Count - 1]);
        }
    }




    public List<BoundsInt> BPSAlgo2d(BoundsInt toSplit)
    {

        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();

        roomsQueue.Enqueue(toSplit);

        while (roomsQueue.Count > 0)
        {
            var room = roomsQueue.Dequeue();

            if (room.size.z >= minHeight && room.size.x >= minWidth)
            {

                if (Random.value < 0.5f)
                {
                    if (room.size.z >= minHeight * 2 + 1)
                    {
                        SplitHori(minHeight, room, roomsQueue);
                    }
                    else if (room.size.x >= minWidth * 2 + 1)
                    {
                        SplitVert(minWidth, room, roomsQueue);
                    }
                    else
                    {
                        roomsListBPSAlgo.Add(room);
                    }
                }
                else
                {
                    if (room.size.x >= minWidth * 2 + 1)
                    {
                        SplitVert(minWidth, room, roomsQueue);
                    }
                    else if (room.size.z >= minHeight * 2 + 1)
                    {
                        SplitHori(minHeight, room, roomsQueue);
                    }
                    else
                    {
                        roomsListBPSAlgo.Add(room);
                    }
                }
            }
        }

        return roomsListBPSAlgo;


    }

    public void SplitVert(int minWidth, BoundsInt room, Queue<BoundsInt> roomQue)
    {

        int minX = room.min.x;
        int maxX = room.max.x;

        int adjustedMinX = minX + minWidth;
        int adjustedMaxX = maxX - minWidth;

        var ranPosition = Random.Range(adjustedMinX, adjustedMaxX);

        BoundsInt roomLeft = new BoundsInt();

        roomLeft.min = new Vector3Int(room.min.x, 0, room.min.z);
        roomLeft.max = new Vector3Int(ranPosition, 0, room.max.z);


        BoundsInt roomRight = new BoundsInt();

        roomRight.min = new Vector3Int(ranPosition, 0, room.min.z);
        roomRight.max = new Vector3Int(room.max.x, 0, room.max.z);

        roomQue.Enqueue(roomRight);
        roomQue.Enqueue(roomLeft);

    }

    public void SplitHori(int minHeight, BoundsInt room, Queue<BoundsInt> roomQue)
    {
        int minY = room.min.z;
        int maxY = room.max.z;

        int adjustedMinY = minY + minHeight;
        int adjustedMaxY = maxY - minHeight;

        var ranPosition = Random.Range(adjustedMinY, adjustedMaxY);

        BoundsInt roomTop = new BoundsInt();

        roomTop.min = new Vector3Int(room.min.x, 0, ranPosition);
        roomTop.max = new Vector3Int(room.max.x, 0, room.max.z);

        BoundsInt roomBot = new BoundsInt();

        roomBot.min = new Vector3Int(room.min.x, 0, room.min.z);
        roomBot.max = new Vector3Int(room.max.x, 0, ranPosition);

        roomQue.Enqueue(roomBot);
        roomQue.Enqueue(roomTop);

    }

    #endregion




    public void RanRoom(bool usingBPS, bool usingCA)
    {
        pcgManager.Restart();


        if (!usingBPS)
        {

            foreach (var room in roomList)
            {
                roomsListBPSAlgo.Add(new BoundsInt() { xMin = room.minX, xMax = room.maxX, zMin = room.minY, zMax = room.maxY });
            }
        }



        foreach (var room in roomsListBPSAlgo)
        {

            var gridArrRoom = new BasicTile[0][];

            if (usingCA)
            {
                gridArrRoom = AlgosUtils.compartimentalisedCA(room);
            }
            else
            {
                gridArrRoom = AlgosUtils.CompartimentalisedRandomWalk(room);
            }



            for (int y = 0; y < gridArrRoom.Length; y++)
            {
                for (int x = 0; x < gridArrRoom[0].Length; x++)
                {
                    if (gridArrRoom[y][x].tileWeight == 1)
                    {
                        pcgManager.gridArray2D[y + room.zMin][x + room.xMin].tileWeight = 1;
                    }
                    else
                    {
                        pcgManager.gridArray2D[y + room.zMin][x + room.xMin].tileWeight = 0;
                    }
                }
            }
        }







        PcgManager.Plane.GetComponent<Renderer>().sharedMaterial.mainTexture = AlgosUtils.SetUpTextBiColAnchor(PcgManager.gridArray2D, true);


    }








    //for bps when in addivite and special it doesn add
    // for both when in additive and special it changed the type of gen for all

    //noraml additive works fine


}






public class RoomsClass
{
    public int minX;
    public int maxX;
    public int minY;
    public int maxY;

    public Vector2Int topLeftCoord;
    public Vector2Int topRightCoord;
    public Vector2Int botLeftCoord;
    public Vector2Int botRightCoord;
    public List<Vector2Int> verticesList = new List<Vector2Int>();

    public int width;
    public int height;

    public List<Vector2Int> tileCoords = new List<Vector2Int>();
    public GameObject tile;


    public void WorkOutCoords()
    {
        verticesList = new List<Vector2Int>();

        topLeftCoord = new Vector2Int(minX, maxY);
        topRightCoord = new Vector2Int(maxX, maxY);
        botLeftCoord = new Vector2Int(minX, minY);
        botRightCoord = new Vector2Int(maxX, minY);

        verticesList.Add(topLeftCoord);
        verticesList.Add(topRightCoord);
        verticesList.Add(botLeftCoord);
        verticesList.Add(botRightCoord);
    }
}