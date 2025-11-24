using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    Player mPlayer;
    [Header("Health")]
    [SerializeField] private Slider mHealthSlider;
    [SerializeField] private Image mHealthTint;

    [Header("Death")]
    [SerializeField] GameObject mDeathScreen;

    private void Awake()
    {
        mPlayer = FindAnyObjectByType<Player>();
        mDeathScreen.SetActive(false);
    }
    public void UpdateHealthSlider(float currentHealth)
    {
        mHealthSlider.value = currentHealth / 100;
        Color c = mHealthTint.color;
        c.a = Mathf.Clamp01(currentHealth / 200f);
        mHealthTint.color = c;
        //Debug.Log($"Slider{mHealthSlider.value}, sended value {currentHealth}");
    }

    
}
