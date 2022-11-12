using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkWalkAlgo : MonoBehaviour
{

    public bool DWtype;     //false for just iter       true for actual count

    public int iterationNum;
    public int maxIterationsReset;
    [Range(0f, 0.5f)]
    public float fillDelay;

    public static DrunkWalkAlgo instance;


    private void Start()
    {
        instance = this;
    }



    public void DrunkWalk2D(Tile[][] _gridArray2d) => StartCoroutine(DrunkWalk2DCor(_gridArray2d));
    public void DrunkWalk3D(Tile[][][] _gridArray3d) => StartCoroutine(DrunkWalk3DCor(_gridArray3d));
    



    public IEnumerator DrunkWalk2DCor(Tile[][] _gridArray2d)
    {
        WaitForSeconds wait = new WaitForSeconds(fillDelay);
        int startX = Random.Range(0, TileVolumeGenerator.Instance.x_Length);
        int startY = Random.Range(0, TileVolumeGenerator.Instance.y_Height);


        Vector2 currentHead = new Vector2(startX, startY);


        int maxIter = 0;

        int iterCount = 0;

        while (iterationNum > 0)
        {
            yield return wait;
            maxIter++;


            if (maxIter >= 1000000) {  }


            if(iterCount >= maxIterationsReset) 
            {
                startX = Random.Range(0, TileVolumeGenerator.Instance.x_Length);
                startY = Random.Range(0, TileVolumeGenerator.Instance.y_Height);


                currentHead = new Vector2(startX, startY);
            }

            int ranDir = Random.Range(0, 4);

            switch (ranDir)
            {
                case 0:    //for

                    if (currentHead.y + 1 >= TileVolumeGenerator.Instance.y_Height) 
                    {
                        
                    }
                    else 
                    {
                        currentHead.y++;
                    }

                    break;

                case 1:    //back
                    if (currentHead.y - 1 <0)
                    {
                       
                    }
                    else
                    {
                        currentHead.y--;
                    }
                    break;

                case 2:    //left
                    if (currentHead.x - 1 < 0)
                    {
                       
                    }
                    else
                    {
                        currentHead.x--;
                    }
                    break;

                case 3:   //rigth
                    if (currentHead.x + 1 >= TileVolumeGenerator.Instance.y_Height)
                    {
                        
                    }
                    else
                    {
                        currentHead.x++;
                    }
                    break;


                default:
                    break;
            }


            if (DWtype) 
            {
                if (_gridArray2d[(int)currentHead.y][(int)currentHead.x].tileObj.GetComponent<MeshRenderer>().material.color != Color.grey) 
                {
                    _gridArray2d[(int)currentHead.y][(int)currentHead.x].tileObj.GetComponent<MeshRenderer>().material.color = Color.grey;
                    iterationNum--;
                }
            }
            else 
            {
                _gridArray2d[(int)currentHead.y][(int)currentHead.x].tileObj.GetComponent<MeshRenderer>().material.color = Color.grey;
                iterationNum--;
            }
        }
    }



    public IEnumerator DrunkWalk3DCor(Tile[][][] _gridArray3d)
    {
        WaitForSeconds wait = new WaitForSeconds(fillDelay);
        int startX = Random.Range(0, TileVolumeGenerator.Instance.x_Length);
        int startY = Random.Range(0, TileVolumeGenerator.Instance.y_Height);
        int startZ = Random.Range(0, TileVolumeGenerator.Instance.z_Width);


        Vector3 currentHead = new Vector3(startX, startY,startZ);


        int maxIter = 0;

        int iterCount = 0;

        while (iterationNum > 0)
        {
            yield return wait;
            maxIter++;


            if (maxIter >= 1000000) { }


            if (iterCount >= maxIterationsReset)
            {
                startX = Random.Range(0, TileVolumeGenerator.Instance.x_Length);
                startY = Random.Range(0, TileVolumeGenerator.Instance.y_Height);
                startZ = Random.Range(0, TileVolumeGenerator.Instance.z_Width);


                currentHead = new Vector3(startX, startY, startZ);
            }

            int ranDir = Random.Range(0, 6);

            switch (ranDir)
            {
                case 0:    //for

                    if (currentHead.y + 1 >= TileVolumeGenerator.Instance.y_Height)
                    {

                    }
                    else
                    {
                        currentHead.y++;
                    }

                    break;

                case 1:    //back
                    if (currentHead.y - 1 < 0)
                    {

                    }
                    else
                    {
                        currentHead.y--;
                    }
                    break;

                case 2:    //left
                    if (currentHead.x - 1 < 0)
                    {

                    }
                    else
                    {
                        currentHead.x--;
                    }
                    break;

                case 3:   //rigth
                    if (currentHead.x + 1 >= TileVolumeGenerator.Instance.y_Height)
                    {

                    }
                    else
                    {
                        currentHead.x++;
                    }
                    break;

                case 4:   //top
                    if (currentHead.z + 1 >= TileVolumeGenerator.Instance.z_Width)
                    {

                    }
                    else
                    {
                        currentHead.z++;
                    }
                    break;

                case 5:   //bot
                    if (currentHead.z - 1 < 0)
                    {

                    }
                    else
                    {
                        currentHead.z--;
                    }
                    break;


                default:
                    break;
            }


            if (DWtype)
            {
                if (_gridArray3d[(int)currentHead.z][(int)currentHead.y][(int)currentHead.x].tileObj.GetComponent<MeshRenderer>().material.color != Color.grey)
                {
                    _gridArray3d[(int)currentHead.z][(int)currentHead.y][(int)currentHead.x].tileObj.GetComponent<MeshRenderer>().material.color = Color.grey;
                    iterationNum--;
                }
            }
            else
            {
                _gridArray3d[(int)currentHead.z][(int)currentHead.y][(int)currentHead.x].tileObj.GetComponent<MeshRenderer>().material.color = Color.grey;
                iterationNum--;
            }
        }
    }




}
