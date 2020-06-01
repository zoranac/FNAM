using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DesktopManager : Singleton<DesktopManager>
{
    public List<GameObject> DesktopButtonObjects = new List<GameObject>();
    public GameObject CloseWindowsButton;
    public GameObject QuitButton;
    public GameObject QuitCanvas;
    public GameObject OptionsCanvas;
    public GameObject Options;
    public GameObject MailScreen;
    public GameObject GameScreenCanvas;
    public GameObject GameScreen;

    public GameObject MailNotif;
    public Text MailSubject;
    public Text MailFrom;
    public Text MailBody;

    public List<Sprite> MailSprites = new List<Sprite>();

    private int previousDay = 0;

    // Start is called before the first frame update
    void Start()
    {
        TimeController.Instance.RestartDayEvent.AddListener(RestartDay);
        RestartDay();
        MailNotif.SetActive(true);
    }

    public void RestartDay()
    {
        QuitCanvas.SetActive(false);
        OptionsCanvas.SetActive(false);
        Options.SetActive(false);
        MailScreen.SetActive(false);
        GameScreenCanvas.SetActive(false);
        GameScreen.SetActive(false);
        ChangeButtonsActive(true);
        QuitButton.SetActive(true);
        CloseWindowsButton.SetActive(false);
        if (previousDay != TimeController.Instance.Day)
        {
            MailNotif.SetActive(true);
            previousDay = TimeController.Instance.Day;
            SetDailyMail();
        }
        else
        {
            MailNotif.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CameraManager.Instance.ZoomedOnScreen)
        {

            if (Input.GetMouseButtonDown(0))
            {

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.name == "StartStream")
                    {
                        TimeController.Instance.StartTime();
                        GameScreenCanvas.SetActive(true);
                        GameScreen.SetActive(true);
                        GameScreenManager.Instance.StartRound();
                        ChangeButtonsActive(false);
                        CloseWindowsButton.SetActive(false);
                        QuitButton.SetActive(false);
                        QuitCanvas.SetActive(false);
                    }
                    else if (hit.transform.name == "Settings")
                    {
                        OptionsCanvas.SetActive(true);
                        Options.SetActive(true);
                        ChangeButtonsActive(false);
                        CloseWindowsButton.SetActive(true);
                        QuitCanvas.SetActive(false);
                    }
                    else if (hit.transform.name == "Mail")
                    {
                        MailScreen.SetActive(true);
                        ChangeButtonsActive(false);
                        CloseWindowsButton.SetActive(true);
                        MailNotif.SetActive(false);
                        QuitCanvas.SetActive(false);
                    }
                    else if (hit.transform.name == "CloseWindow")
                    {
                        OptionsCanvas.SetActive(false);
                        Options.SetActive(false);
                        MailScreen.SetActive(false);
                        ChangeButtonsActive(true);
                        CloseWindowsButton.SetActive(false);
                        QuitCanvas.SetActive(false);
                    }
                    else if (hit.transform.name == "Quit")
                    {
                        QuitCanvas.SetActive(!QuitCanvas.activeSelf);
                    }
                }
            }
        }
    }

    public void ChangeButtonsActive(bool val)
    {
        foreach (var item in DesktopButtonObjects)
        {
           // item.GetComponent<BoxCollider>().enabled = val;
            item.SetActive(val);
        }
    }

    private void SetDailyMail()
    {
        if (TimeController.Instance.Day <= 0)
        {
            MailScreen.GetComponent<SpriteRenderer>().sprite = MailSprites[TimeController.Instance.Day];
        }
        else
        {
            MailScreen.GetComponent<SpriteRenderer>().sprite = MailSprites[TimeController.Instance.Day-1];
        }
       
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void CancelQuit()
    {
        QuitCanvas.SetActive(false);
    }
}
