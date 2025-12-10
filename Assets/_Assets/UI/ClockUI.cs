using TMPro;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mClockText;

    public void ShowTime(float time) 
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f);
        string formattedTime = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        mClockText.text = formattedTime;
    }
}
