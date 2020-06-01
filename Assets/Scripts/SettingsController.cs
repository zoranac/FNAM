using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : Singleton<SettingsController>
{
    public float AudioVolume;
    public int Difficulty;

    public Slider DifficultySlider;
    public Slider AudioVolumeSlider;
    public Text DifficultyText;

    private void Update()
    {
        if (TimeController.Instance.Day < 6)
        {
            DifficultyText.enabled = true;
            DifficultySlider.interactable = false;
        }
        else
        {
            DifficultyText.enabled = false;
            DifficultySlider.interactable = true;
            Difficulty = Mathf.RoundToInt(DifficultySlider.value);
        }

        AudioVolume = AudioVolumeSlider.value;

    }
}
