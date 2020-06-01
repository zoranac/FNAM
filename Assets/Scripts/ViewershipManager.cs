using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewershipManager : Singleton<ViewershipManager>
{
    public int StartingViewership;
    public int Viewership = 0;
    public Text ViewershipCounter;
    public float lossRate = .03f;

    float elapsed = 0f;

    private void Start()
    {
        TimeController.Instance.RestartDayEvent.AddListener(RestartDay);
        RestartDay();
    }

    private void Update()
    {
        ViewershipCounter.text = string.Format("{0:n0}", Viewership.ToString());

        if (TimeController.Instance.TimeStarted)
        {
            elapsed += Time.deltaTime;
            if (elapsed >= lossRate)
            {
                elapsed = elapsed % lossRate;
                ReduceViewership(1);
            }


            if (Viewership <= 0)
            {
                Viewership = 0;
                ScareController.Instance.DoScare();
            }
        }
    }

    public void ReduceViewership(int amount)
    {
        if (DoorManager.Instance.HoldingDoor)
        {

        }
        else
        {
            Viewership -= amount;
        }
    }

    public void RestartDay()
    {
        Viewership = StartingViewership;
    }

}
