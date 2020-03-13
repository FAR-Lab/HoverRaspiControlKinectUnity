using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GesturePresent : MonoBehaviour 
{
	public GameObject obj;
	public float posx = 0.0f, posy = 0.5f, posz = 0.0f;
	[Tooltip("Speed of movement after execution of gesture.")]
	public int moveSpeed;
	public int moveDist;

	private bool isMoving = false;
	private int dir = 0; //1 left, 2 right, 3 up.
	private GestureListener gestureListener;
	private int stepsToGo = 0;

	void Start()
	{
		isMoving = false;
		dir = 0;
		gestureListener = GestureListener.Instance;
	}
	
	void Update()
	{
		// dont run Update() if there is no gesture listener
		if(!gestureListener)
			return;
		
		if(!isMoving) {
			if(Input.GetKeyDown(KeyCode.RightArrow)) {
				MoveLeft();
			} else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
				MoveRight();
			} else if(Input.GetKeyDown(KeyCode.UpArrow)) {
				MoveUp();
			}
			if(gestureListener) {
				if(gestureListener.IsSwipeLeft())
					MoveLeft();
				else if(gestureListener.IsSwipeRight())
					MoveRight();
				else if(gestureListener.IsSwipeUp())
					MoveUp();
			}
		} else {
			if(stepsToGo > 0) {
				if (dir == 1) {
					posx += 0.2f;
				} else if (dir == 2) {
					posx -= 0.2f;
				} else if (dir == 3) {
					posz += 0.2f;
				}
				stepsToGo--;
			} else {
				isMoving = false;
				dir = 0;
			}
		}
		obj.gameObject.transform.position = new Vector3(posx, posy, posz);
	}
	
	private void MoveLeft()
	{
		//posx += 2.0f;
		dir = 1;
		stepsToGo = moveDist / moveSpeed;
		isMoving = true;
	}
	
	private void MoveRight()
	{
		//posx -= 2.0f;
		dir = 2;
		stepsToGo = moveDist / moveSpeed;
		isMoving = true;
	}

	private void MoveUp()
	{
		//posz += 2.0f;
		dir = 3;
		stepsToGo = moveDist / moveSpeed;
		isMoving = true;
	}
}
