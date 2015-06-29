using UnityEngine;
using System.Collections;

public class Cell :Object {


	public int _x;
	public int _y;
	public int[ ] Cords;
	public int  Score = 0;
	public Cell _parent = null;
	public bool Start = false;
	public bool End = false;
	public bool open = false;
	public bool fast = false;



}
