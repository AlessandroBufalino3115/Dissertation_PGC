using System.Collections;
using System.Collections.Generic;
using UnityEngine;




// get the boundso fthe whole map
// spawn a certain amount of objects

// get the closest
public class VoronoiDiagram : MonoBehaviour
{
    public static VoronoiDiagram instance;

    public float topRightCor_X;
    public float topRightCor_Y;


    public float botLeftCor_X;
    public float botLeftCor_Y;

    [Range(4, 20)]
    public int points = 10;


    [SerializeField]
    public List<Vector2> veronoiPoints2D = new List<Vector2>();


    [SerializeField]
    public List<Color> listColor = new List<Color>();


    private void Awake()
    {
        instance = this;
    }

    public void CallVoronoiGen2D(Tile[][] _gridArray2D) 
    {
        veronoiPoints2D = new List<Vector2>();
        listColor = new List<Color>();

        GameObject topRight = _gridArray2D[TileVolumeGenerator.Instance.y_Height - 1][TileVolumeGenerator.Instance.x_Length - 1].tileObj;
        GameObject botLeft = _gridArray2D[0][0].tileObj;


        topRightCor_X = topRight.transform.position.x;
        topRightCor_Y = topRight.transform.position.z;

        botLeftCor_X = botLeft.transform.position.x;
        botLeftCor_Y = botLeft.transform.position.z;


        for (int i = 0; i < points; i++)
        {

            float ran_r = Random.Range(0.01f, 0.99f);
            float ran_g = Random.Range(0.01f, 0.99f);
            float ran_b = Random.Range(0.01f, 0.99f);

            listColor.Add(new Color(ran_r, ran_g, ran_b));

            float ran_x = Random.Range(botLeftCor_X, topRightCor_X);
            float ran_y = Random.Range(botLeftCor_Y, topRightCor_Y);

            Debug.Log(ran_x + " for the x and " + ran_y + " for the y ");

            veronoiPoints2D.Add(new Vector2(ran_x,ran_y));
        }




        for (int y = 0; y < _gridArray2D.Length; y++)
        {
            for (int x = 0; x < _gridArray2D[y].Length; x++)
            {
                int closestIndex = 0;
                float closestDistance = -1;

                for (int i = 0; i < points; i++)
                {
                    if (closestDistance < 0)  //therefore minus therefoe we just started
                    {
                        closestDistance = GeneralUtil.EuclideanDistance2D(veronoiPoints2D[i], new Vector2( _gridArray2D[y][x].tileObj.transform.position.x, _gridArray2D[y][x].tileObj.transform.position.z));
                    
                    }
                    else
                    {
                        float newDist = GeneralUtil.EuclideanDistance2D(veronoiPoints2D[i], new Vector2(_gridArray2D[y][x].tileObj.transform.position.x, _gridArray2D[y][x].tileObj.transform.position.z));

                        if (closestDistance > newDist)
                        {
                            closestDistance = newDist;
                            closestIndex = i;

                        }
                    }
                }

                _gridArray2D[y][x].tileObj.GetComponent<MeshRenderer>().material.color = listColor[closestIndex];

            }
        }
    }
}
