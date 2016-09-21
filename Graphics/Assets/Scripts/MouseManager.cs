using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour {

	Unit selectedUnit;

	// Use this for initialization
	void Start () {
        Assets.Engine.Reset();
        //Assets.Engine.
    }

    // Update is called once per frame
    void Update()
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
            if (selectedUnit != null)
            {
                //deselect
                MeshRenderer mrold = selectedUnit.GetComponentInChildren<MeshRenderer>();
                Material[] matsold = new Material[1];
                //  Fill in the materials array...
                matsold[0] = mrold.materials[1];
                mrold.materials = matsold;
                selectedUnit = null;
            }

        }

    }

        void MouseOver_Hex(GameObject ourHitObject) {

		if(Input.GetMouseButtonDown(0)) {

            MeshRenderer mr = ourHitObject.GetComponentInChildren<MeshRenderer>();
            // If we have a unit selected, let's move it to this tile!

            if (selectedUnit != null) {
                //Assets.Engine.Client.GameState.CheckNewBugPlacement(selectedUnit.color, );
                //move
                selectedUnit.destination = ourHitObject.transform.position;
                selectedUnit.x = ourHitObject.GetComponent<Hex>().x;
                selectedUnit.y = ourHitObject.GetComponent<Hex>().y;
                //deselect
                MeshRenderer mrold = selectedUnit.GetComponentInChildren<MeshRenderer>();
                Material[] matsold = new Material[1];
                //  Fill in the materials array...
                matsold[0] = mrold.materials[1];
                mrold.materials = matsold;
                selectedUnit = null;
                //reset hex color
                ourHitObject.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
            }
        }
	}

	void MouseOver_Unit(GameObject ourHitObject) {
		//Debug.Log("Raycast hit: " + ourHitObject.name );

		if(Input.GetMouseButtonDown(0)) {
			// We have click on the unit
            //deselect
            if (selectedUnit != null)
            {
                MeshRenderer mrold = selectedUnit.GetComponentInChildren<MeshRenderer>();
                Material[] matsold = new Material[1];
                //  Fill in the materials array...
                matsold[0] = mrold.materials[1];
                mrold.materials = matsold;
            }
            //select
            selectedUnit = ourHitObject.GetComponent<Unit>();
            MeshRenderer mr = ourHitObject.GetComponentInChildren<MeshRenderer>();
            Material newMat = Resources.Load("HalfTransparent", typeof(Material)) as Material;
            Material[] mats = new Material[2];
            //  Fill in the materials array...
            mats[1] = mr.material;
            mats[0] = newMat;
            mr.materials = mats;
           
        }

    }
}
