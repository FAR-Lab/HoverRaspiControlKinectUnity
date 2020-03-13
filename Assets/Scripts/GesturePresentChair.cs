using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GesturePresentChair : MonoBehaviour
{
    public WebSocketConnectionInteraction controller;

    public GameObject obj;
    public float posx = 0.0f, posy = 0.5f, posz = 0.0f;
    [Tooltip("Speed of movement after execution of gesture.")]
    public int moveSpeed;
    public int moveDist;

    private bool isMoving = false;
    private int dir = 0; //1 left, 2 right, 3 up, 4 down.
    private GestureListener gestureListener;
    private int stepsToGo = 0;

    void Start()
    {
        controller = GetComponent<WebSocketConnectionInteraction>();

        isMoving = false;
        dir = 0;
        gestureListener = GestureListener.Instance;
    }

    void Update()
    {
        // dont run Update() if there is no gesture listener
        if (!gestureListener)
            return;

        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveUp();
                controller.MoveUp();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveDown();
                controller.MoveDown();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveLeft();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveRight();
            }
            if (gestureListener)
            {
                if (gestureListener.IsSwipeRight())
                {
                    MoveUp();
                    controller.MoveUp();
                }
                else if (gestureListener.IsSwipeLeft())
                {
                    MoveDown();
                    controller.MoveDown();
                }
                else if (gestureListener.IsSwipeUp())
                {
                    //MoveUp();
                }
            }
        }
        else
        {
            if (stepsToGo > 0)
            {
                if (dir == 1)
                {
                    posx -= 0.1f;
                }
                else if (dir == 2)
                {
                    posx += 0.1f;
                }
                else if (dir == 3)
                {
                    posz += 0.1f;
                }
                else if (dir == 4)
                {
                    posz -= 0.1f;
                }
                stepsToGo--;
            }
            else
            {
                isMoving = false;
                dir = 0;
            }
        }
        obj.gameObject.transform.position = new Vector3(posx, posy, posz);
    }

    public void MoveLeft()
    {
        dir = 1;
        stepsToGo = moveDist / moveSpeed;
        isMoving = true;
    }

    public void MoveRight()
    {
        dir = 2;
        stepsToGo = moveDist / moveSpeed;
        isMoving = true;
    }

    public void MoveUp()
    {
        dir = 3;
        stepsToGo = moveDist / moveSpeed;
        isMoving = true;
    }

    public void MoveDown()
    {
        dir = 4;
        stepsToGo = moveDist / moveSpeed;
        isMoving = true;
    }
}
