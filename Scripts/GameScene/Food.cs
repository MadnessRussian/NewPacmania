using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {

	public bool boost  = false;
	// Use this for initialization
	void Start() {
		if( boost == false ) { 
		GetComponent<Renderer>().material.color = Color.yellow;
		} else {
			GetComponent<Renderer>().material.color = Color.green;
		}
	}

}
