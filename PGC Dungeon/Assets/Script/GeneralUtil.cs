using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralUtil 
{
    

    public static Vector2 RanVector2Int(int maxX,int maxY) 
    {
        int ranX = Random.Range(0,maxX);
        int ranY = Random.Range(0, maxY);


        return new Vector2(ranX, ranY);
    }


}

