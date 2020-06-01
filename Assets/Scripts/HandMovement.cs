using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMovement : MonoBehaviour
{
    private float elapsed;
    public Vector3 startingPoint;
    public float MoveDuration;
    public bool Grabbed = false;
    public GameObject ScreenHole;
    // Start is called before the first frame update
    void Start()
    {
        TimeController.Instance.RestartDayEvent.AddListener(RestartDay);
        transform.position = startingPoint;
        GetComponent<LineRenderer>().SetPosition(1, startingPoint);
        Grabbed = false;
        elapsed = 0;
    }

    private void RestartDay()
    {
        transform.position = startingPoint;
        GetComponent<LineRenderer>().SetPosition(1, startingPoint);
        Grabbed = false;
        elapsed = 0;
        gameObject.SetActive(false);
        ScreenHole.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        ScreenHole.SetActive(true);
        ScreenHole.transform.position = startingPoint;
        if (elapsed < MoveDuration)
        {
            elapsed += Time.deltaTime;
            //Fade In Vignette

            transform.position = Vector3.Lerp(startingPoint, Camera.main.transform.position + (Vector3.down*1.6f) + Vector3.back, (elapsed / MoveDuration));
            GetComponent<LineRenderer>().SetPosition(1, transform.position);
            GetComponent<LineRenderer>().SetWidth(1.5f, 1f);
            elapsed += Time.deltaTime;
        }
        else
        {
            Grabbed = true;
        }
    }
}
