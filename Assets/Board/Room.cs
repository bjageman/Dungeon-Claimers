using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {

	public GameObject tile;
	public bool isVisited = false;
	public List<Room> pathWays = new List<Room>();
	public int x;
	public int y;

}
