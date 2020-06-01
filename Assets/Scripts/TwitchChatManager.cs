using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchChatManager : Singleton<TwitchChatManager>
{
    public string[] userNameList = new string[50];
    public float delay = 1f;
    public Queue<string> commentQueue = new Queue<string>();
    public Queue<AudioClip> clipQueue = new Queue<AudioClip>();

    public bool ReadingMessage;

    public List<string> RandomComments = new List<string>();
    public List<AudioClip> RandomClips = new List<AudioClip>();
    public int RandomFrequency = 1000;

    float elapsed = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeController.Instance.TimeStarted && !ScareController.Instance.PowerOutage)
        {
            if(Random.Range(0, RandomFrequency) == 0)
            {
                int r = Random.Range(0, RandomComments.Count);
                ShowTwitchChatComment(RandomComments[r], RandomClips[r]);
                RandomComments.RemoveAt(r);
                RandomClips.RemoveAt(r);
            }
        }


        if (!TimeController.Instance.EndDay && !ReadingMessage && !ScareController.Instance.PowerOutage)
        {
            elapsed += Time.deltaTime;
            if (elapsed >= delay)
            {
                elapsed = elapsed % delay;

                if (clipQueue.Count > 0)
                {
                    readMessage();
                }   
            }
        }
        else if (ReadingMessage)
        {
            if (ChatMessage.Instance.ReadyForNext)
            {
                ReadingMessage = false;
            }
        }
    }

    private void readMessage()
    {
        ReadingMessage = true;
        ChatMessage.Instance.SetTexts(userNameList[Random.Range(0, userNameList.Length)], commentQueue.Dequeue(), clipQueue.Dequeue());
    }

    public void ShowTwitchChatComment(string comment, AudioClip clip)
    {
        commentQueue.Enqueue(comment);
        clipQueue.Enqueue(clip);
    }
}
