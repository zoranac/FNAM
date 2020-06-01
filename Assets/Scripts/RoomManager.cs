using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : Singleton<RoomManager>
{
    public List<Room> allRooms = new List<Room>();
    public AudioClip mousedown;
    public AudioClip mouseup;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!ScareController.Instance.Silenced)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GetComponent<AudioSource>().clip = mousedown;
                GetComponent<AudioSource>().Play();
            }
            if (Input.GetMouseButtonUp(0))
            {
                GetComponent<AudioSource>().clip = mouseup;
                GetComponent<AudioSource>().Play();
            }
        }
    }
}
