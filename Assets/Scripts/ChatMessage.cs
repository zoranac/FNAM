using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatMessage : Singleton<ChatMessage>
{
    public AudioSource audioSource;
    public bool isPlayingAudio = false;
    public Text Name;
    public Text Comment;
    public AudioClip Clip;
    public bool ReadyForNext = true;
    bool fadingIn = false;
    bool fadingOut = false;
    private float elapsed = 0;

    // Start is called before the first frame update
    void Start()
    {
        TimeController.Instance.RestartDayEvent.AddListener(RestartDay);
        RestartDay();
    }

    private void RestartDay()
    {
        ReadyForNext = true;
        fadingIn = false;
        fadingOut = false;
        elapsed = 0;
        isPlayingAudio = false;
        Name.color = new Color(Name.color.r, Name.color.g, Name.color.b, 0);
        Comment.color = new Color(Name.color.r, Name.color.g, Name.color.b, 0);
        audioSource.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeController.Instance.TimeStarted && !ScareController.Instance.PowerOutage && !ScareController.Instance.DoingScare)
        {
            audioSource.volume = SettingsController.Instance.AudioVolume;

            if (fadingIn)
            {
                elapsed += Time.deltaTime;
                if (elapsed >= .1f)
                {
                    elapsed = elapsed % .1f;
                    float alpha = Name.color.a + .1f;
                    if (alpha >= 1)
                    {
                        Name.color = new Color(Name.color.r, Name.color.g, Name.color.b, 1);
                        Comment.color = new Color(Name.color.r, Name.color.g, Name.color.b, 1);
                        fadingIn = false;
                        PlayTextAudio();
                    }
                    else
                    {
                        Name.color = new Color(Name.color.r, Name.color.g, Name.color.b, alpha);
                        Comment.color = new Color(Name.color.r, Name.color.g, Name.color.b, alpha);
                    }
                }
            }
            else if (fadingOut)
            {
                elapsed += Time.deltaTime;
                if (elapsed >= .1f)
                {
                    elapsed = elapsed % .1f;
                    float alpha = Name.color.a - .1f;
                    if (alpha <= 0)
                    {
                        Name.color = new Color(Name.color.r, Name.color.g, Name.color.b, 0);
                        Comment.color = new Color(Name.color.r, Name.color.g, Name.color.b, 0);
                        fadingOut = false;
                        ReadyForNext = true;
                    }
                    else
                    {
                        Name.color = new Color(Name.color.r, Name.color.g, Name.color.b, alpha);
                        Comment.color = new Color(Name.color.r, Name.color.g, Name.color.b, alpha);
                    }
                }
            }
            else if (isPlayingAudio)
            {
                if (!audioSource.isPlaying)
                {
                    fadingOut = true;
                    isPlayingAudio = false;
                }
            }
        }
        else
        {
            audioSource.volume = 0;
        }
    }

    public bool SetTexts(string name, string comment, AudioClip clip)
    {
        if (ReadyForNext)
        {
            Name.text = name;
            Comment.text = comment;
            Clip = clip;
            fadingIn = true;
            ReadyForNext = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PlayTextAudio()
    {
        audioSource.clip = Clip;
        audioSource.Play();
        isPlayingAudio = true;
    }

}
