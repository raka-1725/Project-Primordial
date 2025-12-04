using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private Slider mVolumeSlider;
    [SerializeField] private TextMeshProUGUI mVolumeValueText;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume")) 
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }

    public void ChangeVolume() 
    {
        AudioListener.volume = mVolumeSlider.value;
        //Debug.Log($"volume : {mVolumeSlider.value}");
        Save();
    }

    private void Save()
    {
        UpdateVolumeValueText();
        PlayerPrefs.SetFloat("musicVolume", mVolumeSlider.value);
    }

    private void Load()
    {
        mVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        UpdateVolumeValueText();
    }

    public void ChangeVolumeInput() 
    {
        string input = mVolumeValueText.text;
        if (int.TryParse(input, out int value)) 
        {
            float convertedval = Mathf.Clamp01(value / 100f);
            mVolumeSlider.value = convertedval;
            ChangeVolume();
        }
    }

    private void UpdateVolumeValueText() 
    {
        mVolumeValueText.SetText($"{Mathf.RoundToInt(mVolumeSlider.value * 100)}%");
        //Debug.Log("Update text");
    }
}
