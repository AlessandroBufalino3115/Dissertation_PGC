using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileGeneration : MonoBehaviour
{

    public static TileGeneration Instance;

    public Tile[][][] gridArray3D = new Tile[1][][];
    public Tile[][] gridArray2D = new Tile[1][];

    [SerializeField] GameObject emptyBlock;
    [SerializeField] GameObject CubeBlock;
    [SerializeField] GameObject WorldTileParent;


    public TMP_Text xLengthText;
    public TMP_Text yHeightText;
    public TMP_Text zWidthText;

    public Slider xLengthSlider;
    public Slider yHeightSlider;
    public Slider zWidthSlider;

    public Toggle scaleToggle;
    public Toggle ghostToggle;


    private int xLength;
    private int yHeight;
    private int zWidth;

    private bool clearBlock;
    private bool scaleBool;



    public enum Dimension
    {
        NONE,
        TWOD,
        THREED,
        NOT_A
    }

    public Dimension dimension;


    void Start()
    {
        ValueChangeUI();
    }
    void Update()
    {

    }


    public void Gen3DVolume()
    {
        dimension = Dimension.THREED;
        int timerStart = Environment.TickCount & Int32.MaxValue;
        int blockNum = 0;

        gridArray3D = new Tile[zWidth][][];

        for (int z = 0; z < gridArray3D.Length; z++)
        {

            gridArray3D[z] = new Tile[yHeight][];
            for (int y = 0; y < gridArray3D[z].Length; y++)
            {

                gridArray3D[z][y] = new Tile[xLength];

                for (int x = 0; x < gridArray3D[z][y].Length; x++)
                {
                    Vector3 position = new Vector3(x, y, z);

                    GameObject newRef = null;

                    if (clearBlock)
                    {
                        newRef = Instantiate(emptyBlock, WorldTileParent.gameObject.transform);
                    }
                    else
                    {
                        newRef = Instantiate(CubeBlock, WorldTileParent.gameObject.transform);
                    }

                    newRef.transform.position = position;
                    if (scaleBool)
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


        int half_x = (xLength - 1) / 2;
        int half_y = (yHeight - 1) / 2;
        int half_z = (zWidth - 1) / 2;

        gridArray3D[half_z][half_y][half_x].tileObj.GetComponent<MeshRenderer>().material.color = Color.yellow;


        int timerEnd = Environment.TickCount & Int32.MaxValue;
        int totalTicks = timerEnd - timerStart;
        Debug.Log($"<color=yellow>Performance: The total time this has taken was {totalTicks} Ticks, to generate {blockNum} positions</color>");

    }

    public void Gen2DVolume()
    {
        dimension = Dimension.TWOD;
        int timerStart = Environment.TickCount & Int32.MaxValue;
        int blockNum = 0;

        gridArray2D = new Tile[zWidth][];

        for (int y = 0; y < gridArray2D.Length; y++)
        {
            gridArray2D[y] = new Tile[xLength];

            for (int x = 0; x < gridArray2D[y].Length; x++)
            {
                Vector3 position = new Vector3(x, 0, y);



                GameObject newRef = null;

                if (clearBlock)
                {
                    newRef = Instantiate(emptyBlock, WorldTileParent.gameObject.transform);
                }
                else
                {
                    newRef = Instantiate(CubeBlock, WorldTileParent.gameObject.transform);
                }

                newRef.transform.position = position;

                if (scaleBool)
                {
                    newRef.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                }
                newRef.transform.name = x.ToString() + " " + y.ToString();

                gridArray2D[y][x] = new Tile(newRef, x, y);
                gridArray2D[y][x].tileType = Tile.TileType.VOID;

                blockNum++;
            }
        }


        int half_x = (xLength - 1) / 2;
        int half_y = (zWidth - 1) / 2;

        gridArray2D[half_y][half_x].tileObj.GetComponent<MeshRenderer>().material.color = Color.yellow;

        int timerEnd = Environment.TickCount & Int32.MaxValue;

        int totalTicks = timerEnd - timerStart;
        Debug.Log($"<color=yellow>Performance: The total time this has taken was {totalTicks} Ticks, to generate {blockNum} positions</color>");
        // 1 tick seems to be 1 millisecond
    }

    public void DestroyAllTiles()
    {
        dimension = Dimension.NONE;

        int timerStart = Environment.TickCount & Int32.MaxValue;

        foreach (Transform child in WorldTileParent.transform)
            Destroy(child.gameObject);

        gridArray3D = new Tile[1][][];

        gridArray2D = new Tile[1][];

        int timerEnd = Environment.TickCount & Int32.MaxValue;

        int totalTicks = timerEnd - timerStart;


        Debug.Log($"<color=yellow>Performance: The total time that destorying all the children has taken was {totalTicks}</color>");
    }


    public void ValueChangeUI() 
    {

        xLength = (int)xLengthSlider.value;
        xLengthText.text = "X Length:" + xLength.ToString();

        yHeight = (int)yHeightSlider.value;
        yHeightText.text = "Y Height:" + yHeight.ToString();

        zWidth = (int)zWidthSlider.value;
        zWidthText.text = "Z Width:" + zWidth.ToString();


        scaleBool = scaleToggle.isOn;
        clearBlock = ghostToggle.isOn;


    }






}
