// Volodymyr Pavlik
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [Min(1)]
    [SerializeField] private int damage = 1;
    
    [SerializeField] private LayerMask targetLayers;
    
    [Min(0f)]
    [SerializeField] private float damageCooldown = 1f;
    
    [SerializeField] private Transform ownerRoot;

    private readonly Dictionary<Health, float> _lastDamageTimes = new();

    private void Awake()
    {
        if (ownerRoot == null)
        {
            ownerRoot = transform.root;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryDealDamage(other);
    }

    private void TryDealDamage(Collider2D other)
    {
        if (other.transform.root == ownerRoot)
            return;

        int layerMask = 1 << other.gameObject.layer;

        if ((targetLayers.value & layerMask) == 0)
            return;

        Health health = other.GetComponentInParent<Health>();

        if (health == null || health.IsDead)
            return;

        if (_lastDamageTimes.TryGetValue(health, out float lastDamageTime))
        {
            if (Time.time < lastDamageTime + damageCooldown)
                return;
        }

        _lastDamageTimes[health] = Time.time;
        health.TakeDamage(damage);
    }
}