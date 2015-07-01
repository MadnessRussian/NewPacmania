using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class Wall : MonoBehaviour
{

		public int i = 0;
		public int j = 0;
	 	public int Type_number = 0;
		public int[] ObjectArray;
		public int colpov = 0;
		

		public Builder _parent ;
		public Builder Parent;
		// Use this for initialization
		void Start ()
		{
		ObjectArray = new int[] {0,0,0,0};
		////// Массив для проверки,в какую сторону блока мы можем идти ///////
		switch(Type_number) {
		case 1:
			ObjectArray = new int[]{0,0,1,1};
			break;
		case 2:
			ObjectArray = new int[]{0,1,0,1};
			break;
		case 3:
			ObjectArray = new int[]{1,0,1,1};
			break;
		case 4:
			ObjectArray = new int[]{1,1,1,1};
			break;
		case 5:
			ObjectArray = new int[]{0,0,0,1};
			break;	
		}
	
		int countPov = Convert.ToInt32(Math.Round(transform.eulerAngles.z / 90) ); // количество поворотов
		colpov = countPov;

		////// побитовый сдвиг с переносом,зависит от угла поворота блока ///////
		for( int k = 1;k <= countPov;k++){
			if( Type_number != 2 ) {
				GoToRight(ObjectArray,4);
			} else {
				Array.Reverse(ObjectArray);
			}
		} 

		//	gameObject.transform.position = new Vector3 (( (Menu.CubeNumX*Menu.CubeSize) / -2 + Menu.CubeSize / 2) + i * Menu.CubeSize, ( (Menu.CubeNumY*Menu.CubeSize) / -2 + Menu.CubeSize / 2) + j * Menu.CubeSize,0);
			
		
		gameObject.transform.position = new Vector3 (i * Menu.CubeSize, j * Menu.CubeSize,0);

			if(Type_number == 0) { 
				gameObject.transform.localScale = new Vector3 (Menu.CubeSize, Menu.CubeSize, Menu.CubeSize);
			} else {
				gameObject.transform.localScale = new Vector3 (Menu.CubeSize*0.375f, Menu.CubeSize*0.375f, Menu.CubeSize*0.375f);	
			}
			//GetComponent<Renderer>().material.color = Color.white;
			

			if(_parent!= null){
				Parent = _parent.GetComponent<Builder>();
				
			} else {
			//	GetComponent<Renderer>().material.color = Color.red;
			}

		}
	 
		void OnMouseDown ()
		{     

		////// далеее - события нажатия,либо меняем угол,меняем фигуру или устанавливаем точки спавнов ///////
		/// _parent нужен для прямого обращения к массиву обьектов,в особенности для замены фигуры напрямую у родителя

			if(_parent!= null){ 

				if(GetComponent<Renderer>().material.color == Color.white) {
					if(Parent.CubeType == 6 && Parent.PlayerSpawner == false ) {
							transform.GetComponent<Renderer>().material.color = Color.green;
							Parent.SpawnCords[0] = i;
							Parent.SpawnCords[1] = j;
							Parent.PlayerSpawner = true;
					}
					if(Parent.CubeType == 7 && Parent.GhostSpawner  == false) {
						transform.GetComponent<Renderer>().material.color = Color.red;
						Parent.SpawnCords[2] = i;
						Parent.SpawnCords[3] = j;
						Parent.GhostSpawner = true;
					}
				} else {
					
					if(Parent.CubeType == 6 &&  GetComponent<Renderer>().material.color == Color.green ) {
						Parent.PlayerSpawner = false;
						GetComponent<Renderer>().material.color = Color.white;
					}
					if(Parent.CubeType == 7 && GetComponent<Renderer>().material.color == Color.red ) {
						Parent.GhostSpawner = false;
						GetComponent<Renderer>().material.color = Color.white;
					}

					
				}
				if(  Parent.CubeType != 6  && Parent.CubeType != 7 ) {
					if(Type_number != Parent.CubeType ) {
						Parent.TargetI = i;
						Parent.TargetJ = j; 
						Parent.AddToStage();	
					} else {
						gameObject.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z+90);
						Parent.CubesAngle[i,j] = transform.rotation.eulerAngles.z;
					} 
				}
			}
		}

	void GoToRight(int[] array, int size) {
		int temp = array[0];        
		for(int i = 1; i < size; i++) { 
			array[i-1] = array[i] ;
		} ; 
		array[size-1] = temp;
	}
}
