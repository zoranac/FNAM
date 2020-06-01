using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WifeHallway : MonoBehaviour
{
    public float Delay = .25f;
    float elapsed = 0f;
    public SpriteRenderer spriteRenderer;
    public Sprite WH1;
    public Sprite WH2;
    public Vector3 StartingPos;
    public Vector3 StartingScale;
    public bool StartAnim = false;
    private int currentImage = 0;
    int counter = 0;

    private void OnEnable()
    {
        StartingPos = transform.position;
        StartingScale = transform.localScale;
        TimeController.Instance.RestartDayEvent.AddListener(RestartDay);
        RestartDay();
    }

    private void RestartDay()
    {
        spriteRenderer.sprite = WH1;
        currentImage = 0; 
        transform.position = StartingPos;
        transform.localScale = StartingScale;
        StartAnim = false;
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (StartAnim)
        {
            Debug.Log("test1");
            elapsed += Time.deltaTime;
            if (elapsed >= Delay)
            {
                Debug.Log("test2");

                elapsed = elapsed % Delay;
                if (currentImage == 0)
                {
                    spriteRenderer.sprite = WH2;
                    currentImage = 1;
                    transform.position = new Vector3(transform.position.x, transform.position.y - .1f, transform.position.z);
                    transform.localScale = new Vector3(transform.localScale.x + .007f, transform.localScale.y + .007f, transform.localScale.z + .007f);
                }
                else
                {
                    spriteRenderer.sprite = WH1;
                    currentImage = 0;
                    transform.position = new Vector3(transform.position.x, transform.position.y - .1f, transform.position.z);
                    transform.localScale = new Vector3(transform.localScale.x + .0035f, transform.localScale.y + .0035f, transform.localScale.z + .0035f);
                }
                counter++;

                if (counter >= 20)
                {
                    Debug.Log("test3");


                    CameraFeedEffectManager.Instance.StartTransition = true;
                    RestartDay();
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
