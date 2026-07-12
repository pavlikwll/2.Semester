//Volodymyr Pavlik
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Slider slider;

    private void Awake()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }
    }

    private void OnEnable()
    {
        if (health == null)
            return;

        health.HealthChanged += UpdateHealthBar;

        UpdateHealthBar(
            health.CurrentHealth,
            health.MaxHealth
        );
    }

    private void OnDisable()
    {
        if (health != null)
        {
            health.HealthChanged -= UpdateHealthBar;
        }
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = currentHealth;
    }
}