using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeController : Singleton<TimeController>
{
    public Text DesktopTime;
    public Text CameraScreenTime;
    public Text DesktopDay;
    public GameObject DayEndCanvas;
    public UnityEvent RestartDayEvent = new UnityEvent();

    public string CurrentTime {
        get {
            string minStr = "";

            if (Minute < 10)
            {
                minStr = "0" + Minute.ToString();
            }
            else
            {
                minStr = Minute.ToString();
            }

            return Hour.ToString() + ":" + minStr + " PM";
        }
    }
    public string CurrentTimeHour
    {
        get
        {
            string minStr = "";

            if (Minute < 10)
            {
                minStr = "0" + Minute.ToString();
            }
            else
            {
                minStr = Minute.ToString();
            }

            return Hour.ToString() + " PM";
        }
    }
    public bool TimeStarted { get { return timeStarted; } }
    private bool timeStarted = false;
    public bool TimePaused = false;

    public string DateString { get { return "3/" + DateInt.ToString() + "/2021"; } }
    public int DateInt = 14;
    public int Day = 0;
    public int Hour = 0;
    public int Minute = 0;

    float elapsed = 0f;

    public bool EndDay = false;

    private void Start()
    {
        RestartAll();
    }

    private void Update()
    {
        if (EndDay)
        {
            elapsed += Time.deltaTime;
            if (elapsed >= 7f)
            {
                elapsed = elapsed % 5f;
                StartDay();
            }
        }
        else if (TimeStarted)
        {
            if (!ScareController.Instance.DoingScare)
            {
                elapsed += Time.deltaTime;
                if (elapsed >= .4f)
                {
                    elapsed = elapsed % .4f;
                    TickTime();
                }
            }
        }
    }

    private void TickTime()
    {
        Minute++;
        if (Minute >= 60)
        {
            Hour++;
            Minute = 0;
            if (Hour >= 11)
            {
                EndTime();
            }
        }

        DesktopTime.text = CurrentTime;
        CameraScreenTime.text = CurrentTimeHour;
    }

    public void StartTime()
    {
        timeStarted = true;
        resetTime();
    }

    public void EndTime()
    {
        timeStarted = false;
        EndDay = true;
        
        //reset time for next day
        resetTime();
        Day++;
        DateInt++;
        DayEndCanvas.SetActive(true);
        DayEndCanvas.GetComponent<AudioSource>().Play();
        DayEndCanvas.GetComponentInChildren<Animator>().Play("EndTimeAnim", -1, 0f);

        DesktopDay.text = DateString;
    }

    private void resetTime()
    {
        Hour = 3;
        Minute = 0;
        DesktopDay.text = DateString;
        DesktopTime.text = CurrentTime;
        CameraScreenTime.text = CurrentTimeHour;
    }

    public void PauseTime(bool val)
    {
        TimePaused = val;
        if (val)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void StartDay() {
        resetTime();
        DayEndCanvas.SetActive(false);
        timeStarted = false;
        EndDay = false;
        PauseTime(false);
        RestartDayEvent.Invoke();
    }

    public void RestartAll()
    {
        Day = 1;
        DateInt = 14;
        StartDay();
    }
}
