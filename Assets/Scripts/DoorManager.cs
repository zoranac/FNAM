using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : Singleton<DoorManager>
{
    public bool HoldingDoor = false;
    public Sprite DoorOpen;
    public Sprite DoorClosed;
    public AudioClip OpenClip;
    public AudioClip CloseClip;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && HoldingDoor)
        {
            GetComponent<AudioSource>().clip = OpenClip;
            GetComponent<AudioSource>().Play();
        }

        ReleaseDoor();
        if (!CameraManager.Instance.ZoomedOnScreen && !ScareController.Instance.Silenced)
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.name == "Door")
                    {
                        HoldDoor();
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.name == "Door")
                    {
                        GetComponent<AudioSource>().clip = CloseClip;
                        GetComponent<AudioSource>().Play();
                    }
                }
            }         
        }
    }

    public void HoldDoor()
    {
        HoldingDoor = true;
        GetComponent<SpriteRenderer>().sprite = DoorClosed;
    }

    public void ReleaseDoor()
    {
        HoldingDoor = false;
        GetComponent<SpriteRenderer>().sprite = DoorOpen;
    }
}
