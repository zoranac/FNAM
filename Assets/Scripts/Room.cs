using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<Room> ConnectedRooms = new List<Room>();
    public int DistanceFromMainRoom = 0;
    public List<Monster> MonstersInMe = new List<Monster>();
    public Sprite EmptyRoomImage;

    private void Start()
    {
        TimeController.Instance.RestartDayEvent.AddListener(RestartDay);
    }

    public void LeaveRoom(Monster monsterLeaving)
    {
        MonstersInMe.Remove(monsterLeaving);
        if (CameraScreenManager.Instance.CurrentViewingRoom == this)
        {
            CameraFeedEffectManager.Instance.StartTransition = true;
        }
    }

    public void EnterRoom(Monster monsterEntering)
    {
        MonstersInMe.Add(monsterEntering);
        if (CameraScreenManager.Instance.CurrentViewingRoom == this)
        {
            CameraFeedEffectManager.Instance.StartTransition = true;
        }
    }



    public void RestartDay()
    {
        MonstersInMe.Clear();
    }
}
