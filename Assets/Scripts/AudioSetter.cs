using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSetter : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        GetComponent<AudioSource>().volume = SettingsController.Instance.AudioVolume;
    }
}
