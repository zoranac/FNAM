using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraScreenManager : Singleton<CameraScreenManager>
{
    public SpriteRenderer CameraScreenBGObject;
    public Sprite[] CameraImages = new Sprite[6];
    public List<GameObject> ButtonObjects = new List<GameObject>();
    public Room CurrentViewingRoom;
    // Start is called before the first frame update
    void Start()
    {
        TimeController.Instance.RestartDayEvent.AddListener(RestartDay);
        RestartDay();
    }

    private void RestartDay()
    {
        CameraScreenBGObject.sprite = CameraImages[0];
        CameraFeedEffectManager.Instance.ChangeSprite(RoomManager.Instance.allRooms[0].EmptyRoomImage);
        CurrentViewingRoom = RoomManager.Instance.allRooms[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (CameraManager.Instance.ViewingScreen == 2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.name == "Button1")
                    {
                        CameraScreenBGObject.sprite = CameraImages[0];
                        CameraFeedEffectManager.Instance.ChangeSprite(RoomManager.Instance.allRooms[0].EmptyRoomImage);
                        CurrentViewingRoom = RoomManager.Instance.allRooms[0];
                        MonsterManager.Instance.HideAllMonsters();

                        CameraFeedEffectManager.Instance.ScreenStatic.color = new Color(CameraFeedEffectManager.Instance.ScreenStatic.color.r,
                                                                                        CameraFeedEffectManager.Instance.ScreenStatic.color.g,
                                                                                        CameraFeedEffectManager.Instance.ScreenStatic.color.b,                  
                                                                                        CameraFeedEffectManager.Instance.startingStaticAlpha);
                        GetComponent<AudioSource>().Play();
                    }
                    else if (hit.transform.name == "Button2")
                    {
                        CameraScreenBGObject.sprite = CameraImages[1];
                        CameraFeedEffectManager.Instance.ChangeSprite(RoomManager.Instance.allRooms[1].EmptyRoomImage);
                        CurrentViewingRoom = RoomManager.Instance.allRooms[1];
                        MonsterManager.Instance.HideAllMonsters();

                        CameraFeedEffectManager.Instance.ScreenStatic.color = new Color(CameraFeedEffectManager.Instance.ScreenStatic.color.r,
                                                                                        CameraFeedEffectManager.Instance.ScreenStatic.color.g,
                                                                                        CameraFeedEffectManager.Instance.ScreenStatic.color.b,
                                                                                        CameraFeedEffectManager.Instance.startingStaticAlpha);
                        GetComponent<AudioSource>().Play();
                    }
                    else if (hit.transform.name == "Button3")
                    {
                        CameraScreenBGObject.sprite = CameraImages[2];
                        CameraFeedEffectManager.Instance.ChangeSprite(RoomManager.Instance.allRooms[2].EmptyRoomImage);
                        CurrentViewingRoom = RoomManager.Instance.allRooms[2];
                        MonsterManager.Instance.HideAllMonsters();

                        CameraFeedEffectManager.Instance.ScreenStatic.color = new Color(CameraFeedEffectManager.Instance.ScreenStatic.color.r, 
                                                                                        CameraFeedEffectManager.Instance.ScreenStatic.color.g, 
                                                                                        CameraFeedEffectManager.Instance.ScreenStatic.color.b, 
                                                                                        .95f);
                        GetComponent<AudioSource>().Play();
                    }
                    else if (hit.transform.name == "Button4")
                    {
                        CameraScreenBGObject.sprite = CameraImages[3];
                        CameraFeedEffectManager.Instance.ChangeSprite(RoomManager.Instance.allRooms[3].EmptyRoomImage);
                        CurrentViewingRoom = RoomManager.Instance.allRooms[3];
                        MonsterManager.Instance.HideAllMonsters();

                        CameraFeedEffectManager.Instance.ScreenStatic.color = new Color(CameraFeedEffectManager.Instance.ScreenStatic.color.r,
                                                                                        CameraFeedEffectManager.Instance.ScreenStatic.color.g,
                                                                                        CameraFeedEffectManager.Instance.ScreenStatic.color.b,
                                                                                        CameraFeedEffectManager.Instance.startingStaticAlpha);
                        GetComponent<AudioSource>().Play();    
                    }
                    else if (hit.transform.name == "Button5")
                    {
                        CameraScreenBGObject.sprite = CameraImages[4];
                        CameraFeedEffectManager.Instance.ChangeSprite(RoomManager.Instance.allRooms[4].EmptyRoomImage);
                        CurrentViewingRoom = RoomManager.Instance.allRooms[4];
                        MonsterManager.Instance.HideAllMonsters();

                        CameraFeedEffectManager.Instance.ScreenStatic.color = new Color(CameraFeedEffectManager.Instance.ScreenStatic.color.r,
                                                                                        CameraFeedEffectManager.Instance.ScreenStatic.color.g,
                                                                                        CameraFeedEffectManager.Instance.ScreenStatic.color.b,
                                                                                        CameraFeedEffectManager.Instance.startingStaticAlpha);
                        GetComponent<AudioSource>().Play();
                    }
                    else if (hit.transform.name == "Button6")
                    {
                        CameraScreenBGObject.sprite = CameraImages[5];
                        CameraFeedEffectManager.Instance.ChangeSprite(RoomManager.Instance.allRooms[5].EmptyRoomImage);
                        CurrentViewingRoom = RoomManager.Instance.allRooms[5];
                        MonsterManager.Instance.HideAllMonsters();

                        CameraFeedEffectManager.Instance.ScreenStatic.color = new Color(CameraFeedEffectManager.Instance.ScreenStatic.color.r,
                                                                                        CameraFeedEffectManager.Instance.ScreenStatic.color.g,
                                                                                        CameraFeedEffectManager.Instance.ScreenStatic.color.b,
                                                                                        .95f);
                        GetComponent<AudioSource>().Play();
                    }
                }

                if (CurrentViewingRoom.MonstersInMe.Count > 0)
                {
                    foreach (var item in CurrentViewingRoom.MonstersInMe)
                    {
                        item.ShowCurrentImage();
                    }
                }
            }
        }
  
    }

    public void ChangeButtonsActive(bool val)
    {
        foreach (var item in ButtonObjects)
        {
            item.SetActive(val);
        }
    } 
}
