using UnityEngine;
using System;
using System.Collections.Generic;


public class Finder : MonoBehaviour {

	/// Реализуется упрощенная версия A*,использую открытый список для работы алгоритма 
	///  и массив для проверок и возврата карты пути, чистая работа с массивом показала низкую производительность.
	List<Cell> Opened;

	public Cell[,]  Elements;

	Cell EndPoint ; // конечная точка
	Cell StartCell; // начальная точка 

	public int[] coords ; 
	bool FindEnd = false; // флаг поиска конца 

	public  void returnPath(  int posx , int posy) {
	
		Opened  = new List<Cell>();
		Opened.Clear();
		FindEnd = false;

		Elements = new Cell[Menu.CubeNumX, Menu.CubeNumY];

		// Заполнение массива и обнуление цветов
		foreach( GameObject elementObject in Game.Walls ) {
			if(elementObject !=null) {
			//elementObject.GetComponent<Renderer>().material.color = Color.red;
			Wall element = elementObject.GetComponent<Wall>();
			Elements[ element.i ,element. j ] =  new Cell();
			Elements[ element.i ,element. j ] .open = false;
			Elements[ element.i ,element. j ] .fast = false;
			}
		}
						

		coords = new int[4];
		// Получение координат точек
		coords[0]  = GetComponent<Ghost>().NumAtX;
		coords[1]  = GetComponent<Ghost>().NumAtY;
		coords[2]  = posx;
		coords[3]  = posy;

		EndPoint = new Cell();
		EndPoint.Cords =  new int[2]{ coords[0] , coords[1] };
		EndPoint._parent = null;
		Elements[coords[2],coords[3]].fast = true;

		StartCell  = new Cell();
		StartCell.Cords =  new int[2];
		StartCell .Cords[0]= coords[0];
		StartCell.Cords[1] = coords[1];
		StartCell._parent = null;
		StartCell.Start = true;
		StartCell .Score = 0;
		Opened.Add(StartCell);


		// Основной цикл 
		while ( !FindEnd ) {
			// Поиск элемента с мин стоймостью в откр списке
			 Opened.Sort( 
			           delegate(Cell cell1, Cell cell2 ) {
				return cell1.Score.CompareTo(cell2.Score);
			}
			) ;  
			Cell CurrCell = Opened[0] ;
			Opened.Remove(Opened[0]);
			// Расчет элементов вокруг проверяемой точки
			FindArround(CurrCell); 
		} 
		// конечный поиск пути
		FindStartPoint();
	}

	void FindStartPoint() {
		Cell SuperParent = EndPoint;
		Elements[coords[2],coords[3]].End = true;
		while(SuperParent.Start == false) {
			Elements[SuperParent.Cords[0],SuperParent.Cords[1]].fast = true;
			SuperParent = SuperParent._parent;
				

		}
	}

	void FindArround( Cell Center )  {
		Wall MyObject  = Game.Walls[ Center.Cords[0],Center.Cords[1]].GetComponent<Wall>();
		// Проверяем куда нам можно идти и делаем расчет
		for( int StartCount = 0 ; StartCount <=3 ; StartCount++ ) {
			if( MyObject.ObjectArray[StartCount] == 1 ) {
				Cell Curr =  new Cell();
				Curr.Cords = new int[2];
				switch(StartCount) {
					case 0:
						Curr.Cords[0] = Center.Cords[0];
						Curr.Cords[1] = Center.Cords[1] + 1;
						break;
					case 1:
						Curr.Cords[0]  = Center.Cords[0] + 1;
						Curr.Cords[1]  = Center.Cords[1] ;
						break;
					case 2:
						Curr.Cords[0] =Center.Cords[0];
						Curr.Cords[1]  = Center.Cords[1] - 1;
						break;
					case 3:
						Curr.Cords[0]  = Center.Cords[0]-1;
						Curr.Cords[1] = Center.Cords[1] ;
						break;
				}
				if( Curr.Cords[0]  == coords[2] &&  Curr.Cords[1] == coords[3] ) {
					// Нашли конечную точку
					FindEnd = true;
					EndPoint._parent = Center;
				} else {
					// Расчитываем стоймость
					Curr.Score = Center.Score + 1 + ManhScore( Curr.Cords[0] ,Curr.Cords[1] ) ;
					Curr._parent = Center;

					if( Elements[Curr.Cords[0],Curr.Cords[1]].open == false) {
						// Добавляем в открытый список если такого элемента там нет
						Opened.Add(Curr);
						Curr.open = true;
						Elements[Curr.Cords[0],Curr.Cords[1]]  = Curr;

				} else {
						// Если есть такой же элемент,меняем его стоймость и родителя ( в данном варианте просто замена )
						if(Curr.Score < Elements[Curr.Cords[0],Curr.Cords[1]] .Score  )  {
							Opened.Remove(Opened.Find( item  => item =  Elements[Curr.Cords[0],Curr.Cords[1]]  )) ;
							Opened.Add(Curr);	
						}
					}
				}
			}
		}
	}

	// метод для расчета манхеттенского расстояния
	int ManhScore ( int mx , int my ) {
		return Mathf.Abs( mx -coords[2]  ) + Mathf.Abs( my -coords[3]  ) ;
	}
}
