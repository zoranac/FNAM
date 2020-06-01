using Kino;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraFeedEffectManager : Singleton<CameraFeedEffectManager>
{
    public SpriteRenderer ScreenStatic;
    public AnalogGlitchImage Analog;

    public bool StartTransition = false;
    private bool transitionBack = false;

    private float startingHShake = 0;
    private float startingColorDrift = 0;
    public float startingStaticAlpha = 0;

    float elapsed = 0f;
    private int counter;

    // Start is called before the first frame update
    void Start()
    {
        startingHShake = Analog.horizontalShake;
        startingColorDrift = Analog.colorDrift;
        startingStaticAlpha = ScreenStatic.color.a;
        counter = 0;
        TimeController.Instance.RestartDayEvent.AddListener(RestartDay);
    }

    private void RestartDay()
    {
        counter = 0;
        Analog.colorDrift = startingColorDrift;
        Analog.horizontalShake = startingHShake;
        ScreenStatic.color = new Color(ScreenStatic.color.r, ScreenStatic.color.g, ScreenStatic.color.b, startingStaticAlpha);
    }


    // Update is called once per frame
    void Update()
    {
        if (StartTransition)
        {
            elapsed += Time.deltaTime;
            if (elapsed >= .01f)
            {
                ScreenStatic.color = new Color(ScreenStatic.color.r, ScreenStatic.color.g, ScreenStatic.color.b, ScreenStatic.color.a + 1f);
                Analog.colorDrift += .5f;
                Analog.horizontalShake += .5f;

                elapsed = elapsed % .02f;
                
                if (ScreenStatic.color.a >= 1)
                {
                    StartTransition = false;
                    transitionBack = true;
                }
            }
        }
        if (transitionBack)
        {
            elapsed += Time.deltaTime;
            if (elapsed >= .01f)
            {
                counter++;
                if (CameraScreenManager.Instance.CurrentViewingRoom != RoomManager.Instance.allRooms[2] && CameraScreenManager.Instance.CurrentViewingRoom != RoomManager.Instance.allRooms[5])
                {
                    ScreenStatic.color = new Color(ScreenStatic.color.r, ScreenStatic.color.g, ScreenStatic.color.b, .95f);
                }
                else
                {
                    ScreenStatic.color = new Color(ScreenStatic.color.r, ScreenStatic.color.g, ScreenStatic.color.b, ScreenStatic.color.a - .015f);
                }
                Analog.colorDrift += .01f;
                Analog.horizontalShake += .01f;
                elapsed = elapsed % .01f;

                if (ScreenStatic.color.a <= startingStaticAlpha || counter > 60)
                {
                    if (CameraScreenManager.Instance.CurrentViewingRoom != RoomManager.Instance.allRooms[2] && CameraScreenManager.Instance.CurrentViewingRoom != RoomManager.Instance.allRooms[5])
                    {
                        ScreenStatic.color = new Color(ScreenStatic.color.r, ScreenStatic.color.g, ScreenStatic.color.b, startingStaticAlpha);
                    }
                    Analog.colorDrift = startingColorDrift;
                    Analog.horizontalShake = startingHShake;
                    counter = 0;
                    transitionBack = false;
                }
            }
        }


        if (CameraScreenManager.Instance.CurrentViewingRoom == RoomManager.Instance.allRooms[2] && CameraScreenManager.Instance.CurrentViewingRoom == RoomManager.Instance.allRooms[5])
        {
            ScreenStatic.color = new Color(ScreenStatic.color.r, ScreenStatic.color.g, ScreenStatic.color.b, .95f);
        }
    }
    public void ChangeSprite(Sprite s)
    {
        GetComponent<SpriteRenderer>().sprite = s;
    }

}
