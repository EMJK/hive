using System;
using Assets;
using Hive.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class MouseManager : MonoBehaviour
{
    private Unit selectedUnit;
    public Text displayedText;
    private List<GridCoords> movesForBug = null;
    private bool showTip = false;
    private float timer = 0;
    public float tipTime = 4;

    // Use this for initialization
    private void Start()
    {
        Engine.Reset();
        displayText();
        //Assets.Engine.
    }

    // Update is called once per frame
    private void Update()
    {
        // Is the mouse over a Unity UI Element?
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // It is, so let's not do any of our own custom
            // mouse stuff, because that would be weird.

            // NOTE!  We might want to ask the system WHAT KIND
            // of object we're over -- so for things that aren't
            // buttons, we might not actually want to bail out early.

            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject ourHitObject = hitInfo.collider.transform.parent.gameObject;

            // So...what kind of object are we over?
            if (ourHitObject.GetComponent<Hex>() != null)
            {
                // Ah! We are over a hex!
                MouseOver_Hex(ourHitObject);
            }
            else if (ourHitObject.GetComponent<Unit>() != null)
            {
                // We are over a unit!
                MouseOver_Unit(ourHitObject);
            }
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            DeselectUnit();
        }
    }

    private void MouseOver_Hex(GameObject ourHitObject)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedUnit != null)
            {
                try
                {
                    // If we have a unit selected, let's move it to this tile!
                    int x = ourHitObject.GetComponent<Hex>().x;
                    int y = ourHitObject.GetComponent<Hex>().y;

                    GridCoords destination = new GridCoords(ourHitObject.GetComponent<Hex>().x, ourHitObject.GetComponent<Hex>().y);

                    if (selectedUnit.isOutOfBoard && Engine.Client.GameState.CheckNewBugPlacement(selectedUnit.color, destination))
                    {
                        Engine.Client.PlaceNewBug(selectedUnit.color, selectedUnit.bug, destination);
                        //move
                        selectedUnit.destination = ourHitObject.transform.position;
                        selectedUnit.x = x;
                        selectedUnit.y = y;
                        selectedUnit.fillActualPosition(x, y);
                        selectedUnit.isOutOfBoard = false;
                        displayText();
                    }
                    else if (selectedUnit.isOutOfBoard == false && Engine.Client.GameState.CheckIfBugCanMove(selectedUnit.color, selectedUnit.actualPosition, destination))
                    {

                        //move

                        Engine.Client.MoveBug(Engine.Client.GameState.CurrentPlayer, selectedUnit.actualPosition, destination);

                        selectedUnit.destination = ourHitObject.transform.position;
                        selectedUnit.x = x;
                        selectedUnit.y = y;
                        selectedUnit.fillActualPosition(x, y);
                        if (selectedUnit.unitBelow != null)
                            selectedUnit.unitBelow.unitAbove = null;
                        selectedUnit.unitBelow = null;
                        displayText();
                    }

                    //deselect
                    DeselectUnit();
                    //reset hex color
                    ourHitObject.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
                }
                catch (Exception ex)
                {
                    DeselectUnit();
                    Debug.LogError(ex.Message);
                }
            }
        }
    }

    private void MouseOver_Unit(GameObject ourHitObject)
    {

        if (Input.GetMouseButtonDown(0))
        {
            // We have clicked on the unit
            if (selectedUnit != null && selectedUnit.isOutOfBoard == false)
            {

                //try to jump on the unit
                Unit targetUnit = ourHitObject.GetComponent<Unit>();
                
                while (targetUnit.unitAbove != null)
                {
                    targetUnit = targetUnit.unitAbove;
                }

                if (Engine.Client.GameState.CheckIfBugCanMove(selectedUnit.color, selectedUnit.actualPosition, targetUnit.actualPosition))
                {
                    //move

                    Engine.Client.MoveBug(selectedUnit.color, selectedUnit.actualPosition, targetUnit.actualPosition);
                    Vector3 dest = new Vector3(targetUnit.transform.position.x, targetUnit.transform.position.y+0.2f, targetUnit.transform.position.z);
                    selectedUnit.destination = dest;
                    if (selectedUnit.unitBelow != null)
                        selectedUnit.unitBelow.unitAbove = null;
                    targetUnit.unitAbove = selectedUnit;
                    selectedUnit.unitBelow = targetUnit;
                    selectedUnit.x = targetUnit.x;
                    selectedUnit.y = targetUnit.y;
                    selectedUnit.fillActualPosition(targetUnit.x, targetUnit.y);
                    displayText();
                }
                //deselect
                DeselectUnit();
            }
            else
            {
                //deselect
                DeselectUnit();
                //select
                selectedUnit = ourHitObject.GetComponent<Unit>();
                while (selectedUnit.unitAbove != null)
                {
                    selectedUnit = selectedUnit.unitAbove;
                }

                MeshRenderer mr = selectedUnit.GetComponentInChildren<MeshRenderer>();
                Material newMat = Resources.Load("HalfTransparent", typeof(Material)) as Material;
                Material[] mats = new Material[2];
                //  Fill in the materials array...
                mats[1] = mr.material;
                mats[0] = newMat;
                mr.materials = mats;

                movesForBug = Engine.Client.GameState.GetPossibleTargetsForBug(Engine.Client.GameState.CurrentPlayer, selectedUnit.actualPosition);

                foreach (GridCoords move in movesForBug)
                {
                    GameObject hex = GameObject.Find("Hex_" + move.OX + "_" + move.OY);
                    hex.GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;
                }


            }
        }
    }

    private void DeselectUnit()
    {
        if (selectedUnit != null)
        {
            if (movesForBug != null)
            {
                foreach (GridCoords move in movesForBug)
                {
                    GameObject hex = GameObject.Find("Hex_" + move.OX + "_" + move.OY);
                    hex.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
                }
                movesForBug = null;
            }
            MeshRenderer mrold = selectedUnit.GetComponentInChildren<MeshRenderer>();
            Material[] matsold = new Material[1];
            //  Fill in the materials array...
            matsold[0] = mrold.materials[1];
            mrold.materials = matsold;
            selectedUnit = null;
        }
    }


    private void displayText()
    {
        if (Engine.Client.GameState.CurrentPlayer == PlayerColor.White)
            displayedText.text = "Ruch białych";
        else if (Engine.Client.GameState.CurrentPlayer == PlayerColor.Black)
            displayedText.text = "Ruch czarnych";
        else
            displayedText.text = "Błąd silnika gry";

        switch ((int)Engine.Client.GameState.Winner)
        {
            case 1:
                displayedText.text = "Czarne wygrały!";
                break;
            case 2:
                displayedText.text = "Białe wygrały!";
                break;
            case 3:
                displayedText.text = "Remis!";
                break;
            default:
                break;
                
        }
    }
}