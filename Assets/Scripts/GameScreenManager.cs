using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameScreenManager : Singleton<GameScreenManager>
{
    private char[] Letters = new char[26];

    public int FailureReductionAmount = 250;
    public int SuccessAdditionAmount = 150;
    public int VarianceAmount = 0;
    public float ShowingInterval = 2;
    public int MinLetters = 5;
    public int MaxLetters = 8;
    public float TimerTickRate = .0005f;
    public Image timerImage;

    public bool RecallMode = false;

    private List<char> LettersShown = new List<char>();

    public Text ShowLetterText;
    private float showLetterTime = 0;
    private int lettersToShow = 0;

    public Vector3[] nodePoints = new Vector3[8];
    public GameObject RecallNodePrefab;

    private List<GameObject> recallNodes = new List<GameObject>();

    public List<char> LettersClicked = new List<char>();

    public AudioClip ButtonHover;
    public AudioClip ButtonPress;
    public AudioClip TimerTick;
    public AudioClip LetterChangeReveal;
    public AudioClip Success;
    public AudioClip Failure;



    // Start is called before the first frame update
    void Start()
    {
        Letters[0] = 'A';
        Letters[1] = 'B';
        Letters[2] = 'C';
        Letters[3] = 'D';
        Letters[4] = 'E';
        Letters[5] = 'F';
        Letters[6] = 'G';
        Letters[7] = 'H';
        Letters[8] = 'I';
        Letters[9] = 'J';
        Letters[10] = 'K';
        Letters[11] = 'L';
        Letters[12] = 'M';
        Letters[13] = 'N';
        Letters[14] = 'O';
        Letters[15] = 'P';
        Letters[16] = 'Q';
        Letters[17] = 'R';
        Letters[18] = 'S';
        Letters[19] = 'T';
        Letters[20] = 'U';
        Letters[21] = 'V';
        Letters[22] = 'W';
        Letters[23] = 'X';
        Letters[24] = 'Y';
        Letters[25] = 'Z';


        TimeController.Instance.RestartDayEvent.AddListener(RestartDay);

        RestartDay();
        

    }

    private void RestartDay()
    {
        StartRound();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (TimeController.Instance.TimeStarted && !ScareController.Instance.DoingScare)
        {
            if (RecallMode)
            {
                TickTimer();
                if (LettersClicked.Count >= LettersShown.Count && LettersClicked.Count > 0)
                {
                    CheckRecall();
                }
            }
            else
            {
                if (Time.time > showLetterTime + ShowingInterval)
                {
                    if (LettersShown.Count >= lettersToShow)
                    {
                        StartRecall();
                    }
                    else
                    {
                        showLetterTime = Time.time;
                        ShowNewLetter();
                    }
                }
            }
        }
    }

    private void CheckRecall()
    {
        for (int i = 0; i < LettersClicked.Count; i++)
        {
            if (LettersClicked[i] != LettersShown[i])
            {
                IncorrectRecall();
                return;
            }
        }

        CorrectRecall();
    }

    private void IncorrectRecall()
    {
        if (!ScareController.Instance.PowerOutage)
        {
            GetComponent<AudioSource>().clip = Failure;
            GetComponent<AudioSource>().Play();
        }
        ViewershipManager.Instance.ReduceViewership(UnityEngine.Random.Range(FailureReductionAmount-VarianceAmount,FailureReductionAmount+VarianceAmount));
        StartRound();
    }

    private void CorrectRecall()
    {
        if (!ScareController.Instance.PowerOutage)
        {
            GetComponent<AudioSource>().clip = Success;
            GetComponent<AudioSource>().Play();
        }
        ViewershipManager.Instance.ReduceViewership(-(UnityEngine.Random.Range(SuccessAdditionAmount - VarianceAmount, SuccessAdditionAmount + VarianceAmount)));
        StartRound();
    }

    public void StartRound()
    {
        foreach (var item in recallNodes)
        {
            Destroy(item);
        }
        LettersClicked.Clear();
        recallNodes.Clear();
        timerImage.fillAmount = 1;
        RecallMode = false;
        lettersToShow = UnityEngine.Random.Range(MinLetters, MaxLetters+1);
        showLetterTime = Time.time;
        LettersShown.Clear();
    }

    private void StartRecall()
    {
        if (ShowLetterText != null)
        {
            ShowLetterText.text = "";
        }
        RecallMode = true;

        int n = LettersShown.Count;

        System.Random rng = new System.Random();

        List<char> temp = new List<char>();
        foreach (var item in LettersShown)
        {
            temp.Add(item);
        }

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            char value = temp[k];
            temp[k] = temp[n];
            temp[n] = value;
        }

        for (int i = 0; i < temp.Count; i++)
        {
            GameObject g = (GameObject)Instantiate(RecallNodePrefab);
            g.transform.position = nodePoints[i];
            g.GetComponent<RecallNode>().LetterText.text = temp[i].ToString();
            recallNodes.Add(g);
        }
    }

    private void TickTimer()
    {
        timerImage.fillAmount -= TimerTickRate;
        
        if (Math.Round(timerImage.fillAmount,3) == .75f || Math.Round(timerImage.fillAmount, 3) == .5f || Math.Round(timerImage.fillAmount, 3) == .25f 
            || Math.Round(timerImage.fillAmount, 3) == .15f || Math.Round(timerImage.fillAmount, 3) == .10f || Math.Round(timerImage.fillAmount, 3) == .05f)
        {
            if (!ScareController.Instance.PowerOutage)
            {
                timerImage.GetComponent<AudioSource>().clip = TimerTick;
                if (!timerImage.GetComponent<AudioSource>().isPlaying)
                    timerImage.GetComponent<AudioSource>().Play();
            }
        }

        if (timerImage.fillAmount <= 0)
        {
            IncorrectRecall();
        }
    }

    private void ShowNewLetter()
    {
        var lettersLeft = Letters.Where(x => !LettersShown.Contains(x)).ToList();
        char c = lettersLeft[UnityEngine.Random.Range(0, lettersLeft.Count)];
        LettersShown.Add(c);
        ShowLetterText.text = c.ToString();
        if (!ScareController.Instance.PowerOutage)
        {
            GetComponent<AudioSource>().clip = LetterChangeReveal;
            GetComponent<AudioSource>().Play();
        }
    }


}
