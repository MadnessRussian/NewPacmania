
using UnityEngine;
using System.Collections;
using System;

public class PlayerClass : MonoBehaviour {


	static public int NumAtX = 0;
	static public int NumAtY = 0;
	// Дельта перемешения персонажа,для расчета шага	
	public float Delta = 0.0f;
	/// Направление движение в данный момент и возможное
	public int[] Direction;
	public int[] possibleDirection;	

	private float ObjectSpeed;
	private float playerspeedcoef ;
	int Proverka;


	public bool buttonsPress = false;

	void Start () {

		playerspeedcoef =   Menu.PlayerSpeed;
		ObjectSpeed= 0;
		Proverka = 0;
		buttonsPress = false;
		gameObject.transform.localScale = new Vector3 (Menu.CubeSize /2 , Menu.CubeSize/2, Menu.CubeSize/2);
		
										
		possibleDirection = new int[] {0,0}; // Возможное направление
		Direction = new int[] {0,0}; // Направление движения в этот момент
	}




	void Update () {

		gameObject.transform.position = new Vector3(transform.position.x + (Direction[0] *   playerspeedcoef *ObjectSpeed ),transform.position.y + (Direction[1]   *  playerspeedcoef  * ObjectSpeed ),transform.position.z);
		Delta += playerspeedcoef  *  ObjectSpeed   ;

	
		if (Math.Round(Delta) >=  Menu.CubeSize ) {
			NumAtY+=Direction[1];
			NumAtX+=Direction[0];
			Delta = 0.0f; 
	

			////// Нужно для того,чтобы не останавливаться об стены во время движения ///////
			Proverka = Check(possibleDirection[0],possibleDirection[1]);
			if( Game.Walls[NumAtX,NumAtY ].GetComponent<Wall>().ObjectArray[Proverka] == 1) {
				Direction[0] = possibleDirection[0];
				Direction[1] = possibleDirection[1];
				ObjectSpeed=1.0f;
			} else {
				ObjectSpeed=0;
			}

			Proverka = Check(Direction[0],Direction[1]);
			if( Game.Walls[NumAtX,NumAtY ].GetComponent<Wall>().ObjectArray[Proverka] == 1) {
				ObjectSpeed=1.0f;
			} else {
				ObjectSpeed=0;
			}
		

		}
		// Если остановились  у стены и нажали кнопку,меняем направление
		if(ObjectSpeed== 0 && buttonsPress == true ){
			int Proverka = Check(possibleDirection[0],possibleDirection[1]);
			Wall findObject =  Game.Walls[NumAtX,NumAtY ].GetComponent<Wall>();
			if ( findObject.ObjectArray[Proverka] == 1 ) {
							Direction[0] = possibleDirection[0];
							Direction[1] = possibleDirection[1];
						ObjectSpeed= 1.0f	;
						}
		}


		if (Input.GetKeyDown ("down")) {
			possibleDirection[1] = -1;
			possibleDirection[0] = 0; 
			buttonsPress = true;
		} else 
		if (Input.GetKeyDown ("up")) {
			possibleDirection[1] = 1;
			possibleDirection[0] = 0;
			buttonsPress = true;
		} else 
		if (Input.GetKeyDown ("right") ) {
			possibleDirection[0] = 1;
			possibleDirection[1] = 0; 
			buttonsPress = true;
		} else 
		if (Input.GetKeyDown ("left") ) {
			possibleDirection[0] = -1;
			possibleDirection[1] = 0; 
			buttonsPress = true;
		}  
		
	}

	////// вовзращаем число - нужная нам сторона блока ( для проверки элемента его массива ) ///////
	int Check ( int x , int y ) {
		int CountProverka = 0 ;
		if(x == 1 || x ==  -1  ) {
			CountProverka =  2 - x;
		} else {
			CountProverka = 1 - y;
		}
		return CountProverka;
	}


	void OnTriggerEnter(Collider myTrigger) 
	{
		if (myTrigger.gameObject.tag == "Food")
		{

			if( myTrigger.gameObject.GetComponent<Food>().boost == true ) { 

				Game.Ghosts  =  GameObject.FindGameObjectsWithTag("Ghost");
				foreach( GameObject GhostObj in Game.Ghosts) { 
					if( GhostObj.GetComponent<Ghost>().State != 3) { 
						GhostObj.GetComponent<Ghost>().State = 2;
					}
				}
				Game.BoostCount = Time.time;
				Game.Boost = true;
			}
			Destroy(myTrigger.gameObject);
		}
		if( myTrigger.gameObject.tag == "Ghost" ) {
			if( myTrigger.gameObject.GetComponent<Ghost>().State == 1 ) { 
				myTrigger.gameObject.GetComponent<Ghost>().State = 0;
			}
			if( myTrigger.gameObject.GetComponent<Ghost>().State == 2 ) { 
				myTrigger.gameObject.GetComponent<Ghost>().State = 3;
			}
		
		}
	}

}
