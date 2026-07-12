//Volodymyr Pavlik
using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public event Action<int, int> HealthChanged;

    [Header("Health")]
    [SerializeField] private int maxHealth = 5;

    [Header("Events")]
    [SerializeField] private UnityEvent onDamage;
    [SerializeField] private UnityEvent onDeath;

    [SerializeField] private int currentHealth;
    private bool isDead;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public bool IsDead => isDead;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        HealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isDead || damage <= 0)
        {
            return;
        }

        currentHealth = Mathf.Max(0, currentHealth - damage);

        HealthChanged?.Invoke(currentHealth, maxHealth);
        onDamage?.Invoke();

        if (currentHealth == 0)
        {
            isDead = true;
            onDeath?.Invoke();
        }
    }
/*
    public void Heal(int amount)
    {
        if (isDead || amount <= 0)
            return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        HealthChanged?.Invoke(currentHealth, maxHealth);
    }
*/
}