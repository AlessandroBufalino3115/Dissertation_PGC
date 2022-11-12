using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    public static PerlinNoise Instance;

    public float scale = 20f;
    public int offsetX = 0;
    public int offsetY = 0;
    public int offsetZ = 0;


    private void Start()
    {
        Instance = this;
    }

    public void GenPerlinNoise2D(Tile[][] _gridArray2D) 
    {

        for (int y = 0; y < TileVolumeGenerator.Instance.y_Height; y++)
        {
            for (int x = 0; x < TileVolumeGenerator.Instance.x_Length; x++)
            {
                _gridArray2D[y][x].tileObj.GetComponent<MeshRenderer>().material.color = CalcColour(x, y, _gridArray2D.Length, _gridArray2D[0].Length);
            }
        }
    }


    private Color CalcColour(int x, int y, int y_, int x_) 
    {
        float xCoord = (float)x / x_ * scale  + offsetX;
        float yCoord = (float)y / y_   * scale  + offsetY;


        float value = Mathf.PerlinNoise(xCoord, yCoord);

        return new Color(value, value, value);

    }





    public void GenPerlinNoise3D(Tile[][][] _gridArray3D)
    {
        for (int z = 0; z < TileVolumeGenerator.Instance.z_Width; z++)
        {
            for (int y = 0; y < TileVolumeGenerator.Instance.y_Height; y++)
            {
                for (int x = 0; x < TileVolumeGenerator.Instance.x_Length; x++)
                {

                    float noise = Perlin3D(z * scale, y * scale , x * scale );
                    
                    if (noise > 0.5f) 
                    {
                        _gridArray3D[z][y][x].tileObj.SetActive(true);
                    }
                    else 
                    {

                        _gridArray3D[z][y][x].tileObj.SetActive(false);
                    }
                  
                }
            }
        }
        
    }



    public static float Perlin3D(float x, float y, float z)
    {
        float ab = Mathf.PerlinNoise(x, y);
        float bc = Mathf.PerlinNoise(y, z);
        float ac = Mathf.PerlinNoise(x, z);

        float ba = Mathf.PerlinNoise(y, x);
        float cb = Mathf.PerlinNoise(z, y);
        float ca = Mathf.PerlinNoise(z, x);

        float abc = ab + bc + ac + ba + cb + ca;
        return abc / 6f;
    }




}
