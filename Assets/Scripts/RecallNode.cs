using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RecallNode : MonoBehaviour
{
    public Text LetterText;
    public Sprite Clicked;
    public Sprite Hover;
    public Sprite Default;
    public bool ClickedBool = false;
    public bool PlayedHoverSound = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CameraManager.Instance.ViewingScreen == 1 && !ScareController.Instance.PowerOutage && TimeController.Instance.TimeStarted)
        {
            if (!ClickedBool)
            {
                if (!CameraManager.Instance.CameraMoving)
                {
                    GetComponent<BoxCollider>().enabled = true;
                }

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hit = Physics.RaycastAll(ray);

                if (hit.Length > 0)
                {
                    if (hit.Where(x => x.transform == transform).Count() <= 0)
                    {
                        PlayedHoverSound = false;
                        GetComponent<SpriteRenderer>().sprite = Default;
                    }
                    else
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            Debug.Log(LetterText.text + " Was clicked");
                            GameScreenManager.Instance.LettersClicked.Add(LetterText.text.ToCharArray()[0]);
                            GetComponent<SpriteRenderer>().sprite = Clicked;
                            GetComponent<AudioSource>().clip = GameScreenManager.Instance.ButtonPress;
                            GetComponent<AudioSource>().Play();
                            ClickedBool = true;
                        }
                        else
                        {
                            if (!PlayedHoverSound)
                            {
                                GetComponent<SpriteRenderer>().sprite = Hover;
                                GetComponent<AudioSource>().clip = GameScreenManager.Instance.ButtonHover;

                                GetComponent<AudioSource>().Play();

                                PlayedHoverSound = true;
                            }
                        }
                    }

                    
                }
                else
                {
                    GetComponent<SpriteRenderer>().sprite = Default;
                    PlayedHoverSound = false;
                }
            }
        }
        else
        {
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
