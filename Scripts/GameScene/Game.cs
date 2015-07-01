using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class Game : MonoBehaviour
{

	// Обьекты для организации дочерних в иерархии
	public GameObject  Foods;
	public GameObject WallsObjects;

	// Основной массив для хранения обьектов стен
	static public GameObject[,]  Walls;

	// Загрузка обьектов стены,призраков,еды,игрока и т.д
	public GameObject Player;
	public GameObject GhostObj;
	public GameObject FoodObj;
	static public GameObject[] Ghosts ;
	public GameObject Ugol;
	public GameObject Stena;
	public GameObject Per4;
	public GameObject Per3;
	public GameObject tup;
	public GameObject Cube;

	// Координаты точек спавна
	static public string[] Spawns;



	static public  float BoostCount = 0.0f ;
	static public bool Boost = false;

	int Count = 0 ;
	float deltaTime = 0 	;


	void Start ()
	{

		Application.targetFrameRate = 30; 

		deltaTime = Time.time;
		Count = 0;

		// Грузим из prefs все нужные данные : тип обьекта,угол поворота и координаты спавнов
		String Filename = Menu.NamesArr [Menu.selGridInt];
		string LevelArray = PlayerPrefs.GetString (Filename);	
		string[] LevelAngles = PlayerPrefs.GetString ("angles" + Filename).Split (',');  // массив для хранения углов поворота обьектов стены
		Spawns = PlayerPrefs.GetString ("spawns" + Filename).Split (','); // координаты спавнов
		Walls = new GameObject[Menu.CubeNumX, Menu.CubeNumY]; // массив для хранения стен на сцене
		var ArrayCount = 0;

		// Добавляем обьекты на сцену в зависимости от типа обьекта
		for (var i = 0; i< Menu.CubeNumX; i ++) {
			for (var j = 0; j < Menu.CubeNumY; j++) {
				AddToStage (i, j, (int)Char.GetNumericValue (LevelArray [ArrayCount]), Convert.ToSingle (LevelAngles [ArrayCount]));
				ArrayCount++;
			}
		} 

	
		PlayerClass.NumAtX = Convert.ToInt32 (Spawns [0]) ;
		PlayerClass.NumAtY = Convert.ToInt32 (Spawns [1]);
		
		Player.transform.position = new Vector3 (PlayerClass.NumAtX * Menu.CubeSize,PlayerClass.NumAtY * Menu.CubeSize, Menu.CubeSize / 4);
		Ghosts = new GameObject[Menu.GhostCols];


	}


	void Update () {

		if ( Time.time- deltaTime  > 0.7f  && Count< Menu.GhostCols ) 
		{
			deltaTime = Time.time;
			newGhost();
			Count++;
		
		}
			
		// Расчет режима "страха" для призраков
		if( Boost == true && Time.time-BoostCount >= 6.0f  ) {
			Game.Ghosts  =  GameObject.FindGameObjectsWithTag("Ghost");
			foreach( GameObject GhostObj in Game.Ghosts) { 
				if( 	GhostObj .GetComponent<Ghost>().State == 2 ) {
					GhostObj .GetComponent<Ghost>().State = 0;
					
				}
			}
			Boost = false;
			BoostCount = 0.0f;
		}
	}

	void newGhost() {

		GameObject newGhostObj = GhostObj as GameObject;
		newGhostObj.GetComponent<Ghost>().NumAtX = Convert.ToInt32 (Spawns [2]) ;
		newGhostObj.GetComponent<Ghost>().NumAtY = Convert.ToInt32 (Spawns [3]);
		newGhostObj.transform.position  = new Vector3 (  Convert.ToInt32 (Spawns [2])  * Menu.CubeSize,Convert.ToInt32 (Spawns [3]) * Menu.CubeSize, Menu.CubeSize / 4);
		newGhostObj.name = "Ghost";
		Instantiate(newGhostObj) ;
	}

	public void AddToStage (int TargetI, int TargetJ, int CubeType, float ug)
	{

		switch (CubeType) {
		case 0:
			//AttachToArr (TargetI, TargetJ, 0, Cube, ug);
			break;
		case 1:
			AttachToArr (TargetI, TargetJ, 1, Ugol, ug);
			break;
		case 2:
			AttachToArr (TargetI, TargetJ, 2, Stena, ug);
			break;
		case 3:
			AttachToArr (TargetI, TargetJ, 3, Per3, ug);
			break;
		case 4:
			AttachToArr (TargetI, TargetJ, 4, Per4, ug);
			break;
		case 5:
			AttachToArr (TargetI, TargetJ, 5, tup, ug);
			break;	
		}

	}

	public void AttachToArr (int k, int m, int typen, GameObject obj, float ug)
	{
		FoodObj.GetComponent<Food>().boost = false;
		if( k % 3 == 0 && m % 3 == 0) {
			FoodObj.GetComponent<Food>().boost = true;
		} 
		FoodObj.transform.position = new Vector3 (+ k * Menu.CubeSize, m * Menu.CubeSize, Menu.CubeSize / 4);

		FoodObj.transform.localScale = new Vector3 (Menu.CubeSize /6, Menu.CubeSize/6, Menu.CubeSize/6);
		GameObject ChildObject =  Instantiate(FoodObj) as GameObject ;
		ChildObject.name = "Food";
		ChildObject.transform.parent = Foods.transform;

		obj.GetComponent<Wall> ().i = k;
		obj.GetComponent<Wall> ().j = m; 
		obj.GetComponent<Wall> ().Type_number = typen;
		obj.transform.rotation = Quaternion.Euler (0, 0, transform.rotation.eulerAngles.z + ug);
		Walls [k, m] = Instantiate (obj) as GameObject;
		Walls[k,m].name = "WallObject";
		Walls[k,m].transform.parent = WallsObjects.transform;
	}
	
	void OnGUI ()
	{
		int buttonWidth = 100;
		GUIStyle style = GUI.skin.GetStyle ("Button");
		style.fontSize = 12;
			
		if (GUI.Button (new Rect (Screen.width / 2 + 10 + buttonWidth / 2, Screen.height - Menu.CubeSize - 10, buttonWidth, Menu.CubeSize + 5), "Exit")) {		
			Application.LoadLevel ("Menu");
		}

		if (GUI.Button (new Rect (Screen.width / 2 + 10 + buttonWidth / 2, Screen.height - Menu.CubeSize *2 - 20, buttonWidth, Menu.CubeSize + 5), "find")) {		
		
		}
	}
}
