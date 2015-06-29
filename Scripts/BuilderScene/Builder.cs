using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class Builder : MonoBehaviour
{

	
		public GUIText SaveError ;
		public GameObject LevelObject ;

		public GameObject [,]  CubesOnScene; //для хранения обье22ктов
		public float [,] CubesAngle; // для сохранения углов
		public int [] SpawnCords; // для сохранения координат спавнов

		// имя уровня
		public string stringToEdit = "";  

		// Строки для сохранения в prefs
		public string LevelArray = "";
		public string LevelAngles = "";
		public string LevelSpawn = "";

		// Текущий тип куба
		public int CubeType = 0 ; 

		// флаги для спавнов
		public bool GhostSpawner = false;
		public bool PlayerSpawner = false;

		
		
		// Загрузка префабов с обьектами
		public GameObject Ugol;
		public GameObject Stena;
		public GameObject Per4;
		public GameObject Per3;
		public GameObject tup;
		public GameObject Cube;

		public int TargetI;
		public int TargetJ;

		void Start ()
		{
			
		stringToEdit = "Level" + (	Menu.NamesArr.Length + 1 ).ToString();
		gameObject.transform.position = new Vector3((Menu.CubeNumX/2 )*Menu.CubeSize - Menu.CubeSize/2,(Menu.CubeNumY/ 2 )*Menu.CubeSize,-32); 
		CubesOnScene = new GameObject[Menu.CubeNumX, Menu.CubeNumY];
		CubesAngle = new float[Menu.CubeNumX, Menu.CubeNumY];
		SpawnCords = new int[4];

		// Добаляем кубы на сцену
		for (var i = 0; i< Menu.CubeNumX; i ++) {
						for (var j = 0; j<Menu.CubeNumY; j++) {
								AttachToArr(i,j,0,Cube,0);							
						}
				}
		CubeType = 1;

		}

		void Update ()
		{

		
		}


		public void AddToStage (){

			Destroy(CubesOnScene [TargetI, TargetJ]);

					switch(CubeType) {
						case 1:
							AttachToArr(TargetI,TargetJ,1,Ugol,0);
							break;
						case 2:
							AttachToArr(TargetI,TargetJ,2,Stena,0);
							break;
						case 3:
							AttachToArr(TargetI,TargetJ,3,Per3,0);
							break;
						case 4:
							AttachToArr(TargetI,TargetJ,4,Per4,0);
							break;
						case 5:
							AttachToArr(TargetI,TargetJ,5,tup,0);
							break;	
					}

		}

		public void AttachToArr(int k , int m ,int typen , GameObject obj , float ug) {
			obj.GetComponent<Wall>().i = k;
			obj.GetComponent<Wall>().j = m; 
			obj.GetComponent<Wall>().Type_number = typen;
			obj.GetComponent<Wall>()._parent  = this;
			obj.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z+ug);
			CubesAngle [k, m] = transform.rotation.eulerAngles.z;
			CubesOnScene [k, m] = Instantiate(obj) as GameObject;
		}
		

		void ColorRemove ()
		{
				for (var i = 0; i< Menu.CubeNumX; i ++) {
						for (var j = 0; j<Menu.CubeNumY; j++) {
								Destroy(CubesOnScene[i,j]);
								AttachToArr(i,j,0,Cube,0);
						}
				}
		GhostSpawner = false;
		PlayerSpawner = false;
		CubeType = 1;
		}

		void SaveLevel ()
		{

				if (GhostSpawner == false || PlayerSpawner == false) {

					SaveError.text = "Set ghost/player spawners";
				} else {
						for (var i = 0; i<  Menu.CubeNumX; i ++) {
							for (var j = 0; j< Menu.CubeNumY; j++) {
								LevelAngles+= Convert.ToString(CubesAngle[i,j]) +",";
								LevelArray+= CubesOnScene [i, j].GetComponent<Wall>().Type_number.ToString();
						}
				}

				string spawns = "";
				spawns =  SpawnCords[0].ToString() +"," + SpawnCords[1].ToString()+"," + SpawnCords[2].ToString()+"," + SpawnCords[3].ToString() ;
				
				PlayerPrefs.SetString(stringToEdit,LevelArray);
				PlayerPrefs.SetString("angles" + stringToEdit,LevelAngles);
				PlayerPrefs.SetString("spawns" + stringToEdit,spawns);




				String levels = PlayerPrefs.GetString("levels");
				levels+=","+stringToEdit;
				PlayerPrefs.SetString("levels",levels);

				Application.LoadLevel("Menu");
				}
		}

		void OnGUI ()
		{
			
			GUIStyle style = GUI.skin.GetStyle ("Button");
			style.fontSize = Menu.fontsize;
			
			Menu.buttonwidth = 100.0f ;
			Menu.buttonheight = 24.0f ;
		
			
			if (GUI.Button (new Rect ( Screen.width / 2 - Menu.buttonwidth - 20  - Menu.buttonwidth/4 , 5  ,Menu.buttonwidth /2 ,Menu.buttonheight), "Ugol")) {
							CubeType = 1;
					}
					
			if (GUI.Button (new Rect (Screen.width / 2 - Menu.buttonwidth / 2 - 10 - Menu.buttonwidth/4 , 5  ,Menu.buttonwidth /2 , Menu.buttonheight), "Wall")) {
							CubeType = 2; 
					}
			if (GUI.Button (new Rect (Screen.width / 2   - Menu.buttonwidth/4,5,Menu.buttonwidth /2 ,  Menu.buttonheight), "3-per")) {
							CubeType = 3;
					}
			if (GUI.Button (new Rect (Screen.width / 2 + 10 + Menu.buttonwidth / 2 - Menu.buttonwidth/4 , 5  ,Menu.buttonwidth /2 , Menu.buttonheight), "4-per")) {
							CubeType = 4;
					}

			if (GUI.Button (new Rect (Screen.width / 2 + 20 + Menu.buttonwidth  -Menu.buttonwidth/4, 5  ,Menu.buttonwidth /2 , Menu.buttonheight), "tup")) {
							CubeType = 5;
					}  

			if (GUI.Button (new Rect (Screen.width / 2 - Menu.buttonwidth - 10 ,8 +   Menu.buttonheight, Menu.buttonwidth,Menu.buttonheight), "Player Spawn")) {
							CubeType = 6;
					}
			if (GUI.Button (new Rect (Screen.width / 2 + 10  ,  8 +   Menu.buttonheight , Menu.buttonwidth , Menu.buttonheight), "Ghost Spawn")) {
							CubeType = 7;
					} 
			

			if (GUI.Button (new Rect (Screen.width / 2 - 10- Menu.buttonwidth /2  +   Menu.buttonwidth /4, Screen.height - Menu.buttonheight-10 ,  Menu.buttonwidth /2 , Menu.buttonheight), "Save")) {
							SaveLevel();
					}

			if (GUI.Button (new Rect (Screen.width / 2  +   Menu.buttonwidth /4 , Screen.height - Menu.buttonheight-10 , Menu.buttonwidth /2, Menu.buttonheight), "Exit")) {		
				Application.LoadLevel("Menu");
			}

			if (GUI.Button (new Rect (Screen.width / 2   + 10 + Menu.buttonwidth/2   +   Menu.buttonwidth /4 ,  Screen.height - Menu.buttonheight-10 , Menu.buttonwidth /2 ,Menu.buttonheight), "Clear")) {
							ColorRemove ();
					}
			
			stringToEdit = GUI.TextField (new Rect (Screen.width / 2 -  Menu.buttonwidth*1.5f - 20  +   Menu.buttonwidth /4, Screen.height - Menu.buttonheight-10, Menu.buttonwidth, Menu.buttonheight),stringToEdit);
					

		}
	
	
}
