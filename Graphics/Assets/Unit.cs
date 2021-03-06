﻿using UnityEngine;
using System.Collections;
using Hive.Common;

public class Unit : MonoBehaviour {

	public Vector3 destination;

	float speed = 6;

    public BugType bug;
    public PlayerColor color;

    public int x = 999;
    public int y = 999;

    public bool isOutOfBoard = true;
    public GridCoords actualPosition;
    public Unit unitAbove = null;
    public Unit unitBelow = null;
    public void fillActualPosition (int x, int y)
    {
        this.actualPosition = new GridCoords(x, y);
    }

    // Use this for initialization
    void Start () {
		destination = transform.position;

        int positionof_ = name.IndexOf('_');
        string bugName = name.Substring(0, positionof_);

        if (name.Contains("light"))
            color = PlayerColor.White;
        else if (name.Contains("dark"))
            color = PlayerColor.Black;

        switch (bugName)
        {
            case "QueenBee":
                bug = BugType.QueenBee;
                break;
            case "Grasshopper":
                bug = BugType.Grasshopper;
                break;
            case "Ant":
                bug = BugType.SoldierAnt;
                break;
            case "Spider":
                bug = BugType.Spider;
                break;
            case "Beetle":
                bug = BugType.Beetle;
                break;
            case "Ladybird":
                bug = BugType.Ladybug;
                break;
            case "Millipede":
                bug = BugType.PillBug;
                break;
            case "Mosquito":
                bug = BugType.Mosquito;
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        // Move towards our destination

        if (destination != transform.position)
        {
            Vector3 dir = destination - transform.position;
            Vector3 velocity = dir.normalized * speed * Time.deltaTime;

            // Make sure the velocity doesn't actually exceed the distance we want.
            velocity = Vector3.ClampMagnitude(velocity, dir.magnitude);

            transform.Translate(velocity);
        }

	}
	
}
