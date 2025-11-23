using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private Slider mHealthSlider;
    public void UpdateHealthSlider(float currentHealth)
    {
        mHealthSlider.value = currentHealth / 100;
        Debug.Log($"Slider{mHealthSlider.value}, sended value {currentHealth}");
    }
}
