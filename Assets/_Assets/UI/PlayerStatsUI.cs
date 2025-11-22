using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private Slider mHealthSlider;
    public void UpdateHealthSlider(float currentHealth)
    {
        mHealthSlider.value = currentHealth;
        Debug.Log(mHealthSlider.value);
    }
}
