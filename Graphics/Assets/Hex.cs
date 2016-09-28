using UnityEngine;
using System.Collections;

public class Hex : MonoBehaviour {

	// Our coordinates in the map array
	public int x;
	public int y;

    public int a, b, c;
  
	public Hex[] GetNeighbours() {

		// So if we are at x, y -- the neighbour to our left is at x-1, y
		GameObject leftNeighbour = GameObject.Find("Hex_" + (x-1) + "_" + y);

		// Right neighbour is also easy to find.
		GameObject right = GameObject.Find("Hex_" + (x+1) + "_" + y);

		return null;
	}

}
