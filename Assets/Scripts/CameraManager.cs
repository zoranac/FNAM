using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
     public int Boundary = 50; // distance from edge scrolling starts
     public int speed = 5;
     private int theScreenWidth;
     private int theScreenHeight;

    public Vector3 MainPos;
    public Vector3 Screen1Pos;
    public Vector3 Screen2Pos;
    public bool ZoomedOnScreen = false;
    public GameObject Screen1Hidden;
    public GameObject Screen2Hidden;
    public int ViewingScreen = 0;
    public bool CameraMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        theScreenWidth = Screen.width;
        theScreenHeight = Screen.height;
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        TimeController.Instance.RestartDayEvent.AddListener(RestartDay);
        RestartDay();
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.targetFrameRate != 60)
            Application.targetFrameRate = 60;

        if (!ScareController.Instance.Silenced)
        {
            //Panning Movement
            if (!ZoomedOnScreen)
            {
                if (Application.isFocused)
                {
                    if (Input.mousePosition.x > theScreenWidth - Boundary && transform.position.x <= 7f)
                    {
                        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z); // move on +X axis
                    }
                    if (Input.mousePosition.x < 0 + Boundary && transform.position.x >= -7f)
                    {
                        transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z); // move on +X axis
                    }
                }

                //Screen Clicked
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.name == "Screen Area")
                        {
                            ZoomedOnScreen = true;
                            ViewingScreen = 1;
                            StartCoroutine(MoveToSpot(Screen1Pos));
                        }
                        else if (hit.transform.name == "Screen Area 2")
                        {
                            ZoomedOnScreen = true;
                            ViewingScreen = 2;
                            StartCoroutine(MoveToSpot(Screen2Pos));
                        }
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.name == "BackArrow")
                        {
                            ZoomedOnScreen = false;
                            Screen1Hidden.SetActive(false);
                            Screen2Hidden.SetActive(false);
                            ViewingScreen = 0;
                            StartCoroutine(MoveToSpot(MainPos));
                        }
                        else if (hit.transform.name == "LeftArrow")
                        {
                            ZoomedOnScreen = true;
                            Screen1Hidden.SetActive(false);
                            Screen2Hidden.SetActive(false);
                            ViewingScreen = 1;
                            StartCoroutine(MoveToSpot(Screen1Pos));
                        }
                        else if (hit.transform.name == "RightArrow")
                        {
                            ZoomedOnScreen = true;
                            Screen1Hidden.SetActive(false);
                            Screen2Hidden.SetActive(false);
                            ViewingScreen = 2;
                            StartCoroutine(MoveToSpot(Screen2Pos));
                        }
                    }
                }
            }
        }
    }


    public IEnumerator MoveToSpot(Vector3 pos, float waitTime = .5f)
    {
        var Gotoposition = pos;
        float elapsedTime = 0;
        var currentPos = transform.position;
        CameraMoving = true;

        while (elapsedTime < waitTime)
        {
            transform.position = Vector3.Lerp(currentPos, Gotoposition, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        if (pos == Screen1Pos)
        {
            Screen1Hidden.SetActive(true);
            CameraScreenManager.Instance.ChangeButtonsActive(false);
        }
        else if (pos == Screen2Pos)
        {
            Screen2Hidden.SetActive(true);
            CameraScreenManager.Instance.ChangeButtonsActive(true);
        }
        else
        {
            CameraScreenManager.Instance.ChangeButtonsActive(false);
        }

        CameraMoving = false;

        yield break;
    }

    public void RestartDay()
    {
        Debug.Log("restart day");
        Screen1Hidden.SetActive(false);
        Screen2Hidden.SetActive(false);

        transform.position = MainPos;
        ZoomedOnScreen = false;
        ViewingScreen = 0;
        CameraMoving = false;
    }
}
