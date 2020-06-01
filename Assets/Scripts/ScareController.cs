using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ScareController : Singleton<ScareController>
{
    public Volume PPVolume;
    public GameObject GameOverCanvas;
    public Text CommentText;
    public bool DoingScare = false;
    public int ScareType = -1;
    public bool PowerOutage = false;

    public AudioSource SoundEffectSource;

    public AudioClip StartupSound;
    public AudioClip Breathing1;
    public AudioClip Breathing2;
    public AudioClip Static;
    public GameObject Monster1Jumpscare;

    public GameObject DarkScreenLeft;
    public GameObject DarkScreenRight;
    public List<Sprite> DarkScreenScareSprites = new List<Sprite>();
    public Sprite FinalDarkScreenScareSprite;
    private Vector3 lastCameraPos;
    float elapsed = 0f;
    float elapsed2 = 0f;
    Monster monsterScare;

    bool Kill1 = false;
    bool Kill2 = false;
    bool Kill3 = false;


    int KillState = 0;
    float Rand = 0;

    public bool Silenced = false;

    public GameObject Hand1;
    public GameObject Hand2;
    public Sprite BlackScreen;

    private bool scaleUp = true;
    private Vector3 shakepos;
    private float shakeTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        TimeController.Instance.RestartDayEvent.AddListener(RestartDay);
        RestartDay();
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeController.Instance.TimeStarted)
        {
            if (Kill3)
            {
                if (KillState == 0)
                {
                    //activate hands
                    Hand1.SetActive(true);
                    Hand2.SetActive(true);
                    DarkScreenLeft.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    DarkScreenRight.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    DarkScreenLeft.GetComponent<Animator>().Play("BlankScreen", -1, 0f);
                    DarkScreenRight.GetComponent<Animator>().Play("BlankScreen", -1, 0f);
                    CameraManager.Instance.Screen1Hidden.SetActive(false);
                    CameraManager.Instance.Screen2Hidden.SetActive(false);
                    Silenced = true;

                    ChromaticAberration ca;
                    if (PPVolume.profile.TryGet(out ca))
                    {
                        ca.intensity.value = 1;
                    }

                    FilmGrain fg;
                    if (PPVolume.profile.TryGet(out fg))
                    {
                        fg.intensity.value = 1;
                    }

                    KillState = 1;
                    shakepos = Camera.main.transform.position;
                    SoundEffectSource.clip = Static;
                }
                else if (KillState == 1)
                {
                    if (Hand1.GetComponent<HandMovement>().Grabbed)
                    {
                        if (shakeTime == 0)
                        {
                            SoundEffectSource.Play();
                            Camera.main.GetComponent<CameraShake>().StartShake(1, shakepos);
                            shakeTime = 1;
                        }

                        if (Camera.main.GetComponent<CameraShake>().shakeDuration <= 0)
                        {
                            SoundEffectSource.Pause();
                        }

                        elapsed += Time.deltaTime;
                        //Fade In Vignette
                        if (elapsed >= 2)
                        {
                            elapsed = elapsed % 2f;
                            Camera.main.GetComponent<CameraShake>().StartShake(1, shakepos);
                            SoundEffectSource.Play();
                            KillState = 2;
                        }

                        Vignette v;
                        if (PPVolume.profile.TryGet(out v))
                        {
                            if (v.intensity.value >= .75f && scaleUp)
                            {
                                scaleUp = false;
                            }
                            else if (v.intensity.value <= .1f && !scaleUp)
                            {
                                scaleUp = true;
                            }

                            if (!scaleUp && v.intensity.value > .1f)
                            {
                                v.intensity.value = v.intensity.value - .005f;
                            }
                            else if (scaleUp && v.intensity.value < .75f)
                            {
                                v.intensity.value = v.intensity.value + .005f;
                            }
                        }
                    }
                }
                else if (KillState == 2)
                {
                    Vignette v;
                    if (PPVolume.profile.TryGet(out v))
                    {
                        if (v.intensity.value >= .75f && scaleUp)
                        {
                            scaleUp = false;
                        }
                        else if (v.intensity.value <= .1f && !scaleUp)
                        {
                            scaleUp = true;
                        }

                        if (!scaleUp && v.intensity.value > .1f)
                        {
                            v.intensity.value = v.intensity.value - .005f;
                        }
                        else if (scaleUp && v.intensity.value < .75f)
                        {
                            v.intensity.value = v.intensity.value + .005f;
                        }
                    }

                    if (Camera.main.GetComponent<CameraShake>().shakeDuration <= 0)
                    {
                        SoundEffectSource.Pause();
                    }

                    elapsed += Time.deltaTime;

                    if (elapsed >= 2.5f)
                    {
                        elapsed = elapsed % 2f;
                        Camera.main.GetComponent<CameraShake>().StartShake(3, shakepos);
                        KillState = 3;
                        SoundEffectSource.Play();
                        SoundEffectSource.loop = true;
                    }
                }
                else if (KillState == 3)
                {
                    Vignette v;
                    if (PPVolume.profile.TryGet(out v))
                    {
                        v.intensity.value = v.intensity.value + .001f;
                    }

                    if (Camera.main.GetComponent<CameraShake>().shakeDuration <= 0)
                    {
                        int rand = Random.Range(0, 2);

                        if (rand == 0)
                        {
                            GameOver("Don't Stream Alone");
                        }
                        else
                        {
                            GameOver("Losing viewers is the death of a streamer");
                        }
                    }
                }
            }
            else if (Kill1)
            {
                if (KillState == 0)
                {
                    CameraManager.Instance.Screen1Hidden.SetActive(false);
                    CameraManager.Instance.Screen2Hidden.SetActive(false);

                    Monster1Jumpscare.SetActive(true);
                    StartCoroutine(CameraManager.Instance.MoveToSpot(new Vector3(-13, -0.28f, -9), .25f));
                    KillState = 1;
                    monsterScare.GetComponent<AudioSource>().Play();
                }
                else if (KillState == 1)
                {

                    if (Vector3.Distance(Camera.main.transform.position, new Vector3(-13, -0.28f, -9)) < 1f)
                    {
                        Camera.main.GetComponent<CameraShake>().StartShake(1.75f, new Vector3(-13, -0.28f, -9));
                   
                        KillState = 2;
                    }

                }
                else if (KillState == 2)
                {
                    elapsed += Time.deltaTime;
                    //Fade In Vignette
                    if (elapsed >= .01f)
                    {
                        AddVignette(.001f);
                        elapsed = elapsed % .01f;
                    }

                    if (Camera.main.GetComponent<CameraShake>().shakeDuration <= 0)
                    {
                        int rand = Random.Range(0, 2);

                        if (rand == 0)
                        {
                            GameOver("Don't let strangers in");
                        }
                        else
                        {
                            GameOver("Keep the door closed");
                        }

                    }
                }
            }
            else if (Kill2)
            {
                //Do Wife Transition Animation
                if (KillState == 0)
                {
                    DarkScreenLeft.GetComponent<Animator>().SetFloat("SpeedMultiplier", 3f);
                    DarkScreenRight.GetComponent<Animator>().SetFloat("SpeedMultiplier", 3f);



                    if (DarkScreenLeft.GetComponent<SpriteRenderer>().sprite == FinalDarkScreenScareSprite)
                    {
                        KillState = 1;
                        elapsed = 0;
                        Silenced = true;
                        SoundEffectSource.Stop();
                        CameraManager.Instance.Screen1Hidden.SetActive(false);
                        CameraManager.Instance.Screen2Hidden.SetActive(false);
                    }

                }
                else if (KillState == 1)
                {
                    ColorAdjustments ca;
                    if (PPVolume.profile.TryGet(out ca))
                    {
                        if (ca.postExposure.value < 60)
                        {
                            ca.postExposure.value = ca.postExposure.value + .75f;
                        }
                        ca.colorFilter.value = Color.red;
                    }

                    ChangeVignette(.4f);

                    elapsed += Time.deltaTime;

                    if (elapsed >= 1.5f)
                    {
                        //  DarkScreenLeft.GetComponent<SpriteRenderer>().sprite = DarkScreenScareSprites[2];
                        //   DarkScreenRight.GetComponent<SpriteRenderer>().sprite = DarkScreenScareSprites[2];

                        DarkScreenLeft.GetComponent<Animator>().Play("Bloody", -1, 0f);
                        DarkScreenRight.GetComponent<Animator>().Play("Bloody", -1, 0f);
                        ca.postExposure.value = 2;
                        elapsed = 0;
                        KillState = 2;
                    }
                }
                else if (KillState == 2)
                {
                    elapsed += Time.deltaTime;
                    Silenced = true;
                    if (elapsed >= 4f)
                    {
                        elapsed = 0;
                        KillState = 3;
                    }
                }
                else if (KillState == 3)
                {
                    //GameOver
                    int rand = Random.Range(0, 2);

                    if (rand == 0)
                    {
                        GameOver("Your wife needs more attention, watch over her closely");
                    }
                    else
                    {
                        GameOver("If your wife is near you, don't do anything that could startle her");
                    }
                }
            }
            else if (DoingScare && ScareType == 1)
            {
                ChromaticAberration ca;
                if (PPVolume.profile.TryGet(out ca))
                {
                    ca.intensity.value = Random.Range(0f, .25f);
                }

                elapsed2 += Time.deltaTime;
                if (elapsed2 >= 1)
                {
                    if (Input.GetMouseButtonDown(0) || Vector3.Distance(Camera.main.transform.position, lastCameraPos) > 2.5f)
                    {
                        Kill2 = true;
                        elapsed = 0;
                        DarkScreenLeft.GetComponent<SpriteRenderer>().sprite = DarkScreenScareSprites[1];
                        DarkScreenRight.GetComponent<SpriteRenderer>().sprite = DarkScreenScareSprites[1];
                        SoundEffectSource.clip = Breathing2;
                        SoundEffectSource.Play();
                    }
                    else
                    {
                        elapsed += Time.deltaTime;
                        if (elapsed >= Rand)
                        {
                            elapsed = 0;
                            elapsed2 = 0;
                            //End Scare
                            DarkScreenLeft.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .2f);
                            DarkScreenRight.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .2f);
                            DoingScare = false;
                            monsterScare.RestartDay();
                            PowerOutage = false;
                            GetComponent<AudioSource>().UnPause();
                            SoundEffectSource.clip = StartupSound;
                            SoundEffectSource.Play();
                            resetPP();
                        }
                    }
                }
            }
        }
    }

    void resetPP()
    {
        ColorAdjustments ca;
        if (PPVolume.profile.TryGet(out ca))
        {
            ca.postExposure.value = 0;
            ca.colorFilter.value = Color.white;
            ca.contrast.value = 10;
        }

        ChromaticAberration abr;
        if (PPVolume.profile.TryGet(out abr))
        {
            abr.intensity.value = 0;
        }

        FilmGrain fg;
        if (PPVolume.profile.TryGet(out fg))
        {
            fg.intensity.value = .55f;
        }
       
        ChangeBloomIntensity(5);

        Vignette v;
        if (PPVolume.profile.TryGet(out v))
        {
            v.intensity.value = 0;
            v.color.value = Color.black;
        }
    }

    void ChangeBloomIntensity(float val)
    {
        Bloom b;

        if (PPVolume.profile.TryGet(out b))
        {
            b.intensity.value = val;
        }
    }

    void ChangeVignette(float val)
    {
        Vignette v;
        if (PPVolume.profile.TryGet(out v))
        {
            v.intensity.value = val;
        }
    }

    void AddVignette(float val)
    {
        Vignette v;
        if (PPVolume.profile.TryGet(out v))
        {
            v.intensity.value += val;
         
        }
    }

    public void DoScare(Monster monster)
    {
        DoingScare = true;
        monsterScare = monster;
        ScareType = monster.MoveType;
        //Do Scare
        if (ScareType == 0)
        {
            Kill1 = true;
        }
        else if (ScareType == 1)
        {
            DarkScreenLeft.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            DarkScreenRight.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

            lastCameraPos = Camera.main.transform.position;
            Rand = Random.Range(5f, 10f);

            DarkScreenLeft.GetComponent<Animator>().Play("OpeningMouth", -1, 0f);
            DarkScreenRight.GetComponent<Animator>().Play("OpeningMouth", -1, 0f);

            DarkScreenLeft.GetComponent<SpriteRenderer>().sprite = DarkScreenScareSprites[0];
            DarkScreenRight.GetComponent<SpriteRenderer>().sprite = DarkScreenScareSprites[0];

            SoundEffectSource.clip = Breathing1;
            SoundEffectSource.Play();

            PowerOutage = true;
            GetComponent<AudioSource>().Pause();

            ColorAdjustments ca;
            if (PPVolume.profile.TryGet(out ca))
            {
                ca.colorFilter.value = Color.gray;
                ca.postExposure.value = 1.15f;
            }

            Vignette v;
            if (PPVolume.profile.TryGet(out v))
            {
                v.intensity.value = .25f;
                v.color.value = Color.black;
            }
        }


    }
    //Scare Because No Viewers
    public void DoScare()
    {
        DoingScare = true;
        Kill3 = true;
    }

    public void GameOver(string comment = "")
    {
        //game over
        GameOverCanvas.SetActive(true);
        TimeController.Instance.PauseTime(true);
        resetPP();
        if (comment != "")
        {
            CommentText.text = comment;
        }
    }

    public void RestartDay()
    {
        SoundEffectSource.loop = false;
        SoundEffectSource.Stop();
        scaleUp = true;
        shakeTime = 0;
        GetComponent<AudioSource>().UnPause();
        Silenced = false;
        Monster1Jumpscare.SetActive(false);
        DoingScare = false;
        ScareType = -1;
        Kill1 = false;
        Kill2 = false;
        Kill3 = false;
        Rand = 0;
        KillState = 0;
        GameOverCanvas.SetActive(false);
        PowerOutage = false;
        elapsed = 0;
        elapsed2 = 0;
        DarkScreenLeft.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .2f);
        DarkScreenRight.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .2f);
        DarkScreenLeft.GetComponent<Animator>().SetFloat("SpeedMultiplier", 1f);
        DarkScreenRight.GetComponent<Animator>().SetFloat("SpeedMultiplier", 1f);
        DarkScreenLeft.GetComponent<Animator>().Play("Default", -1, 0f);
        DarkScreenRight.GetComponent<Animator>().Play("Default", -1, 0f);
        resetPP();
    }
}
