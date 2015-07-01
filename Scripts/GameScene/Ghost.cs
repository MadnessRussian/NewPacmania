using UnityEngine;
using System.Collections;
using System;


public class Ghost : MonoBehaviour {
	public int NumAtX = 0;
	public int NumAtY = 0;
	private int[] Direction; // Текущее направление движения,включает направление по x,y

	/*
	 *  Направление движения зависит от клетки,в которой находится игрок
	 *  Куда может идти игрок определяет массив,внутри класса клетки,который меняется ( сдвигается ) 
	 *  в зависимости от угла поворота ( по часовой стрелке ) 
	 *              __
	 * например :  |         - фигура угол, по часовой стрелке определяем : сверху закрыто,справа свободно,внизу свободно,слева закрыто
	 * массив это фигуры будет выглядеть так : [1,0,0,1] , с помощью этого мы определяем куда можем двигаться
	 */

	public int oldDirection ; // Старое направление,чтобы не возвращаться туда,откуда пришли,если это не тупик
	public int PossibleDirection ; // Возможное направление

	private  float Delta = 0.0f; // Счетчик для передвижения
	private  int deltaMove = 0; // Счетчик ходов



	private float SpeedParametr ; // Параметр для установки движения, 0 - стоит, 1 - идет
	private float ObjectSpeed = 0 ; // Скорость обьекта

	public int State = 0; // Состояния обьекта : 0 - гуляет (синий),1 - ищет игрока(зеленый) , 2 - бежит на базу(красный),3 - убегает рандомно от игрока(розовый)


	public bool FindPlayer = false;
	public Finder Find;
	public bool Boost = false;
	
	public bool findend = false;

	void Start () {
		
		Find =GetComponent<Finder>() ;
		ObjectSpeed = Menu.GhostSpeed ;
		gameObject.transform.localScale = new Vector3 (Menu.CubeSize /2 , Menu.CubeSize/2, Menu.CubeSize/2);
		Direction = new int[] {0,0}; // Направление движения в этот момент
		SpeedParametr  = 0;
		State = 0;
		Move(); 
	}

	void Update () {
		gameObject.transform.position = new Vector3(transform.position.x + (Direction[0] * SpeedParametr  * ObjectSpeed),transform.position.y + (Direction[1]   * SpeedParametr  * ObjectSpeed),transform.position.z);
		Delta += SpeedParametr *ObjectSpeed  ;

		// Сначала считаем сумму - расстояние за которое мы перемешаемся к другой ячейке

		if (Math.Round(Delta) >=  Menu.CubeSize ) {

			// переместились,меняем координату и начинаем проверять
			NumAtY+=Direction[1];
			NumAtX+=Direction[0];
			Delta = 0.0f; 

			if( State == 0 || State == 1  ) { 
				deltaMove ++ ;
			}

			if(State == 3 && NumAtX == Convert.ToInt32 (Game.Spawns [2]) && NumAtY == Convert.ToInt32 (Game.Spawns [3] )  ) {
				findend = true;
				FindPlayer = false;
				State = 0;
			}  

			if(deltaMove == 15 ) {
				if( FindPlayer == false  ) {
					FindPlayer = true;
					Find.returnPath(PlayerClass.NumAtX,PlayerClass.NumAtY) ;	
					State = 1;
				}
			} 
			if( deltaMove == 25 ) {
				deltaMove = 0;
				FindPlayer = false;
				State = 0;
			}

			Move();
			if( Game.Walls[NumAtX,NumAtY ].GetComponent<Wall>().ObjectArray[ PossibleDirection] == 0) {
				SpeedParametr  = 0;
			} 
		}
	}

	void Move(){
		// Определяем,где мы были раньше,чтобы туда не идти
		switch( PossibleDirection) {
		case 0:
			oldDirection = 2;
			break;
		case 1:
			oldDirection = 3;
			break;
		case 2:
			oldDirection = 0;
			break;
		case 3:
			oldDirection = 1;
			break;
		}
		if( State == 0  ||   State == 2 ) {
			if( State == 2 ) {
				GetComponent<Renderer>().material.color = Color.magenta;
			} else {
				GetComponent<Renderer>().material.color = Color.blue;
			}
			int Proverka = randomCheck();
			if( Game.Walls[NumAtX,NumAtY ].GetComponent<Wall>().ObjectArray[Proverka] == 1) {
				PossibleDirection = Proverka;
				SetDirection(Proverka);
			}
		}

		if( State == 1 ) {
			GetComponent<Renderer>().material.color = Color.green;
			PossibleDirection =  FastCheck();
			SetDirection( PossibleDirection);
		}
		ObjectSpeed = Menu.GhostSpeed;

		if( State == 3 ) {
				ObjectSpeed = Menu.GhostSpeed*2;
				GetComponent<Renderer>().material.color = Color.red;
				Find.returnPath(Convert.ToInt32 (Game.Spawns [2]) , Convert.ToInt32 (Game.Spawns [3])) ;	
				PossibleDirection =  FastCheck();
				SetDirection( PossibleDirection);

		}

	}
	// Функция для движения по короткому пути,просто проверяем соседние ячейки и ищем ту,которая нам нужна
	// работает просто - находим путь до нужной ячейки ( до базы или игрока ) и идем по этому пути
	int FastCheck() {
		int fastnapr = 0;
		Cell childElement = Find.Elements[NumAtX,NumAtY]._parent;

		for ( int count = 0 ; count <=3 ; count ++ ) {
			if( Game.Walls[NumAtX,NumAtY ].GetComponent<Wall>().ObjectArray[count] == 1 ) {
				switch (count) {
				case 0:
					childElement = Find.Elements[NumAtX,NumAtY+1];
					fastnapr = 0;
					break;
				case 1:
					childElement = Find.Elements[NumAtX+1,NumAtY];
					fastnapr = 1;
					break;
				case 2:
					childElement = Find.Elements[NumAtX,NumAtY-1];
					fastnapr = 2;
					break;
				case 3:
					childElement = Find.Elements[NumAtX-1,NumAtY];
					fastnapr = 3;
					break;
				}
			}
			if( Game.Walls[NumAtX,NumAtY ].GetComponent<Wall>().ObjectArray[fastnapr] == 1 ) { 
				if ( childElement.fast == true) {
					break;
				}
			}
		}
		if( childElement.End == true  ) {
				deltaMove = 0;
				FindPlayer = false;
		} 
		Find.Elements[NumAtX,NumAtY].fast = false;
		return fastnapr; 
	}

	// Задаем направление движения
	void SetDirection( int p) {
		SpeedParametr  = 1.0f;
		if ( p == 0 ) {
			Direction[0] = 0;
			Direction[1] = 1;
		}
		if ( p == 2 ) {
			Direction[0] = 0;
			Direction[1] = -1;
		}

		if ( p == 1 ) {
			Direction[0] = 1;
			Direction[1] = 0;
		}
		if ( p == 3 ) {
			Direction[0] = -1;
			Direction[1] = 0;
		}

	}
	
	// функция для рандомного движения, проверяем,не находимся ли мы в тупике и идем куда возможно
	int randomCheck(){
		bool may = true;
		int ch = 0 ;
		System.Random rnd = new System.Random();
		if(Game.Walls[NumAtX,NumAtY ].GetComponent<Wall>().Type_number != 5 ) {
			while(may) {
				ch = rnd.Next(4);
				if( ch != oldDirection && Game.Walls[NumAtX,NumAtY ].GetComponent<Wall>().ObjectArray[ch] == 1 ) {
					may = false;
				}
			}
	  } else {

			for( int pr = 0 ; pr <= 3; pr ++ ) {
				if ( Game.Walls[NumAtX,NumAtY ].GetComponent<Wall>().ObjectArray[pr] == 1 )  {
					ch = pr;
				}
			}
		}
		return ch;
	}


	int Check ( int x , int y ) {
		int CountProverka = 0 ;
		if(x == 1 || x ==  -1  ) {
			CountProverka =  2 - x;
		} else {
			CountProverka = 1 - y;
		}
		return CountProverka;
	}
}
