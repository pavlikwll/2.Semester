//Volodymyr Pavlik
using System;
using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour
{
    [SerializeField] private ItemDefinition itemDefenition;

    [Min(1)]
    [SerializeField] private int amount = 1;

    [SerializeField] private UnityEvent onCollected;

    [SerializeField] private bool destroyedObj = true;

    [SerializeField] private float destroyTimer = 0;

    private void OnValidate()
    {
        if (amount > 10)
        {
            amount = 10;
        }
    }

    public void Collect()
    {
        if (itemDefenition == null)
        {
            Debug.LogError("No Item Definition assigned!");
            return;
        }

        InventorySystem.OnAddItemDefinition?.Invoke(itemDefenition, amount);
        
        onCollected?.Invoke();
        
        if(destroyedObj)
            Invoke("DestroyObj", destroyTimer);
    }

    private void DestroyObj()
    {
        Destroy(gameObject);
    }

}
 