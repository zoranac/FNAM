using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Room StartingRoom;
    public Room CurrentRoom;
    public float MovementCheckSpacing = 0;
    public int MoveChance = 0;
    public bool paused = false;
    public int MoveType = 0;
    private float elapsed = 0f;
    private int stage = 0;
    public List<GameObject> CameraImages = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        TimeController.Instance.RestartDayEvent.AddListener(RestartDay);
        RestartDay();
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeController.Instance.TimeStarted && !ScareController.Instance.DoingScare)
        {
            if (!CurrentRoom.MonstersInMe.Contains(this))
            {
                CurrentRoom.EnterRoom(this);
                ShowCurrentImage();
            }

            elapsed += Time.deltaTime;

            if (stage >= 3)
            {
                if (elapsed >= MovementCheckSpacing)
                {
                    elapsed = elapsed % MovementCheckSpacing;
                    MovementCheck();
                }
            }

            else if (elapsed >= MovementCheckSpacing)
            {
                elapsed = elapsed % MovementCheckSpacing;
                MovementCheck();
            }
        }
    }

    void MovementCheck()
    {
        if (stage >= 3)
        {
            Move();
        }
        else if (UnityEngine.Random.Range(0, 20) < MoveChance)
        {
            Move();
        }
        else
        {
            Debug.Log("I am not moving");
        }
    }

    void Move()
    {
        /*
        foreach (var room in CurrentRoom.ConnectedRooms)
        {

        }*/

        if (MoveType == 0)
        {
            Room temp = CurrentRoom.ConnectedRooms[UnityEngine.Random.Range(0, CurrentRoom.ConnectedRooms.Count)];

            //Check if door is closed
            if (CurrentRoom.DistanceFromMainRoom == 0)
            {
                if (DoorManager.Instance.HoldingDoor)
                {
                    Debug.Log("The door is closed, so I cannot enter");
                    CurrentRoom.LeaveRoom(this);
                    CurrentRoom = temp;
                    CurrentRoom.EnterRoom(this);
                    ShowCurrentImage();
                }
                else
                {
                    ScareController.Instance.DoScare(this);
                }
            }
            else
            {
                CurrentRoom.LeaveRoom(this);
                CurrentRoom = temp;
                CurrentRoom.EnterRoom(this);
                ShowCurrentImage();
            }

            Debug.Log("I am now in room " + CurrentRoom.gameObject.name);

        }
        else if (MoveType == 1)
        {
            if (stage >= 4)
            {
                ScareController.Instance.DoScare(this);
            }
            else if (stage == 3)
            {
                
                CurrentRoom.LeaveRoom(this);
                CurrentRoom = RoomManager.Instance.allRooms[stage+1];
                RoomManager.Instance.allRooms[stage+1].EnterRoom(this);
                ShowCurrentImage();
                stage++;
            }
            else if (CameraManager.Instance.ViewingScreen != 2)
            {
                stage++;
                ShowCurrentImage();
                if (CameraScreenManager.Instance.CurrentViewingRoom == CurrentRoom)
                    CameraFeedEffectManager.Instance.StartTransition = true;
            }
            else
            {
                Debug.Log("I am getting attention");
            }

        }
    }

    public void RestartDay()
    {
        CurrentRoom = StartingRoom;
        HideImages();
        stage = 0;
    }

    public void ShowCurrentImage()
    {
        HideImages();
        if (CameraScreenManager.Instance.CurrentViewingRoom == CurrentRoom)
        {
            if (MoveType == 0)
            {
                CameraImages[RoomManager.Instance.allRooms.IndexOf(CurrentRoom)].SetActive(true);
            }
            else if (MoveType == 1)
            {
                CameraImages[stage].SetActive(true);
                if (stage == 3)
                {
                    CameraImages[stage].GetComponent<WifeHallway>().StartAnim = true;
                }
            }
        }
        else if (CurrentRoom.DistanceFromMainRoom == 0 && MoveType == 0)
        {
            CameraImages[5].SetActive(true);
        }
    }

    public void HideImages()
    {
        foreach (var item in CameraImages)
        {
            if (item != null)
            {
                item.SetActive(false);
            }
        }
    }
}
