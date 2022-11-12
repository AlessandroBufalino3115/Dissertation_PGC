using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiHomeState : UiBaseState
{


    public Vector2 scrollPosition = Vector2.zero;

    private int scrollX = 0;
    private int scrollY = 0;
    private int scrollZ = 0;

    private bool scaleToggle;
    private bool ghostToggle;


    public override void onExit(StateUIManager currentMenu)
    {

    }

    public override void onGUI(StateUIManager currentMenu)
    {
        //scrollPosition = GUI.BeginScrollView(new Rect(10, 300, 300, 300), scrollPosition, new Rect(0, 0, 220, 200));

        //scaleToggle = GUI.Toggle(new Rect(10, 10, 100, 30), scaleToggle, "toggle scale");
        //ghostToggle = GUI.Toggle(new Rect(10, 90, 100, 30), ghostToggle, "toggle ghost");

        //scrollX = (int)GUI.HorizontalSlider(new Rect(25, 25, 100, 30), scrollX, 3, 75);

        //scrollY = (int)GUI.HorizontalSlider(new Rect(25, 50, 100, 30), scrollY, 3, 15);

        //scrollZ = (int)GUI.HorizontalSlider(new Rect(25, 75, 100, 30), scrollZ, 3, 75);

        //if (GUI.Button(new Rect(0, 0, 100, 20), "basic"))
        //    currentMenu.ChangeState(1);
        //if (GUI.Button(new Rect(120, 0, 100, 20), "voroni"))
        //    currentMenu.ChangeState(2);
        //if (GUI.Button(new Rect(0, 180, 100, 20), "path"))
        //    currentMenu.ChangeState(3);

        //if (GUI.Button(new Rect(120, 180, 100, 20), "GEnDAta")) 
        //{
        //    currentMenu.DestroyAllTiles();
        //    currentMenu.Gen2DVolume(scrollX, scrollZ, ghostToggle, scaleToggle);
        //}
        //// End the scroll view that we began above.
        //GUI.EndScrollView();


        GUI.Box(new Rect(10, 10, 200, 350), "");

        if (GUI.Button(new Rect(20, 20, 150, 20), "Generate 2D volume"))
        {
            currentMenu.DestroyAllTiles();
            currentMenu.Gen2DVolume(scrollX, scrollZ, ghostToggle, scaleToggle);
        }

        scaleToggle = GUI.Toggle(new Rect(20, 50, 140, 30), scaleToggle, "toggle scale");
        ghostToggle = GUI.Toggle(new Rect(20, 80, 140, 30), ghostToggle, "toggle ghost");

        scrollX = (int)GUI.HorizontalSlider(new Rect(20, 120, 100, 30), scrollX, 3, 75);
        GUI.Label(new Rect(150, 115, 100, 20), scrollX.ToString());

        scrollY = (int)GUI.HorizontalSlider(new Rect(20, 160, 100, 30), scrollY, 3, 15);
        GUI.Label(new Rect(150, 155, 100, 20), scrollY.ToString());

        scrollZ = (int)GUI.HorizontalSlider(new Rect(20, 200, 100, 30), scrollZ, 3, 75);
        GUI.Label(new Rect(150, 195, 100, 20), scrollZ.ToString());

        if (GUI.Button(new Rect(20, 220, 140, 20), "Destroy Volume"))
            currentMenu.DestroyAllTiles();
       

        if (GUI.Button(new Rect(20, 260, 140, 20), "LineRoomMaker"))
            currentMenu.ChangeState(1);
        if (GUI.Button(new Rect(20, 290, 140, 20), "voroni"))
            currentMenu.ChangeState(2);
        if (GUI.Button(new Rect(20, 320, 140, 20), "path"))
            currentMenu.ChangeState(3);


    }

    public override void onStart(StateUIManager currentMenu)
    {
    }

    public override void onUpdate(StateUIManager currentMenu)
    {
    }
}
