using UnityEngine;
using System.Collections;
using Hive.IpcClient;
using System;

public class Map : MonoBehaviour {

	public GameObject hexPrefab;

	// Size of the map in terms of number of hex tiles
	// This is NOT representative of the amount of 
	// world space that we're going to take up.
	// (i.e. our tiles might be more or less than 1 Unity World Unit)
	int width = 21;
	int height = 21;

	float xOffset = 0.882f;
	float zOffset = 0.764f;

	// Use this for initialization
	void Start () {

		for (int x = 0, xbis = -10; x < width; x++, xbis++) {
         
			for (int y = 0, z = 10; y < height; y++, z--) {
                
                float xPos = x * xOffset;

				// Are we on an odd row?
				if( y % 2 == 1 ) {
					xPos += xOffset/2f;
				}

				GameObject hex_go = (GameObject)Instantiate(hexPrefab, new Vector3( xPos,0, y * zOffset  ), Quaternion.identity  );

				// Name the gameobject something sensible.
				hex_go.name = "Hex_" + x + "_" + y;

				// Make sure the hex is aware of its place on the map
				hex_go.GetComponent<Hex>().x = x;
				hex_go.GetComponent<Hex>().y = y;
                hex_go.GetComponent<Hex>().c = z;
                hex_go.GetComponent<Hex>().a = xbis - (z - (z & 1)) / 2;
                hex_go.GetComponent<Hex>().b = -(hex_go.GetComponent<Hex>().a) - (hex_go.GetComponent<Hex>().c);

                if (x == width / 2 && y == height / 2)
                    hex_go.GetComponentInChildren<MeshRenderer>().material.color = Color.cyan;
                // For a cleaner hierachy, parent this hex to the map
                hex_go.transform.SetParent(this.transform);

				// TODO: Quill needs to explain different optimization later...
				hex_go.isStatic = true;

			}
           
		}

    }

    // Update is called once per frame
    void Update () {
	
	}
}
