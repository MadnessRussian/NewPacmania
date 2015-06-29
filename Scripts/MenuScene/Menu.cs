using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class Menu : MonoBehaviour {
	
	static  public int CubeNumX =  10;
	static  public int CubeNumY = 15;

	
	static public int selGridInt = 0;
	static public int CubeSize = 28 ;
	static public float PlayerSpeed = 1.0f;
	static public float GhostSpeed = 1.0f;
	static public int GhostCols = 3;

	static public string[] NamesArr;
	static public string[] NamesArr2;



	static public float buttonwidth = 100.0f ;
	static public float buttonheight = 24.0f ;
	static public int fontsize = 12;
//	public TextAsset myText;
	
	void Start () {
		
		if(Screen.width < 480) {
			buttonwidth = buttonwidth / (480f/Screen.width);
			buttonheight = buttonheight / (480f/Screen.height);
			fontsize = Convert.ToInt32( 12f / (480f/Screen.width) );
		}

		Parse () ;
	}
	
	void Update () {
	}

	void Parse () {

		String levels = PlayerPrefs.GetString("levels");
		if ( levels == "") {
			PlayerPrefs.DeleteAll();

			
			// Грузим первый пробный уровень
			levels="level1";
			PlayerPrefs.SetString("levels",levels);

			PlayerPrefs.SetString("level1","123311323335121112222112221113522144122222112344122234334311222225222252231222221122112222234334123341233132222222225332112252223122222123323311233111");
			PlayerPrefs.SetString("angleslevel1","180,90.00001,180,180,90.00002,180,180,90.00001,180,180,180,90.00001,180,90.00001,90.00001,270,90.00001,0,0,0,180,270,90.00002,0,0,0,180,0,180,90.00001,180,0,0,270,0,0,90.00002,0,0,0,0,0,180,6.465943E-05,0,270,0,0,90.00002,0,0,0,270,0,90.00001,270,0,90.00002,180,1.001791E-05,0,0,0,0,0,0,180,0,0,0,0,180,0,270,90.00002,0,180,0,0,0,180,6.465943E-05,0,0,270,90.00002,1.001791E-05,0,0,0,0,270,0,90.00001,270,0,90.00001,0,270,180,0,0,0,270,90.00001,270,90.00001,0,0,0,0,0,0,0,0,0,180,270,90.00001,180,180,0,0,0,0,0,0,0,270,0,0,0,0,0,1.001791E-05,270,90.00001,0,0,90.00001,0,0,1.001791E-05,270,90.00001,0,0,1.001791E-05,270,1.001791E-05,");
			PlayerPrefs.SetString("spawnslevel1","2,5,8,6");	

				selGridInt = 0;
		} 
		NamesArr = levels.Split(',');

	}
		
	void OnGUI () {
		selGridInt = GUI.SelectionGrid(new Rect(25, 20, 150, 40*NamesArr.Length), selGridInt, NamesArr, 1);
		
		if (GUI.Button (new Rect (Screen.width/2 -50 , 20,100,40), "Builder")) {
			Application.LoadLevel("Builder");
		}
		if (GUI.Button (new Rect (Screen.width/2 -50 , 70,100,40), "Play")) {
			Application.LoadLevel("Game");

		} 
		if (GUI.Button (new Rect (Screen.width/2 -50 , 120,100,40), "Del My Levels")) {
			PlayerPrefs.DeleteAll();
			Parse () ;
			selGridInt = GUI.SelectionGrid(new Rect(25, 20, 150, 40), selGridInt, NamesArr, 1);
		} 
	}
}
