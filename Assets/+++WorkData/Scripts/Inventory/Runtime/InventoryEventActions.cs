//Volodymyr Pavlik
using FMODUnity;
using UnityEngine;

public class InventoryEventActions : MonoBehaviour
{
    [Header("Item")]
    [SerializeField] private ItemDefinition itemDefinition;

    [Min(1)]
    [SerializeField] private int amount = 1;

    [Header("Audio")]
    [SerializeField] private EventReference addItemSound;
    [SerializeField] private EventReference removeItemSound;

    public void AddItem()
    {
        if (itemDefinition == null)
        {
            Debug.LogError("ItemDefinition is missing.", this);
            return;
        }

        InventorySystem.Instance.Add(itemDefinition, amount);

        if (!addItemSound.IsNull)
        {
            RuntimeManager.PlayOneShot(addItemSound);
        }
    }

    public void RemoveItem()
    {
        if (itemDefinition == null)
        {
            Debug.LogError("ItemDefinition is missing.", this);
            return;
        }

        InventorySystem.Instance.RemoveItem(
            itemDefinition.id,
            amount
        );

        if (!removeItemSound.IsNull)
        {
            RuntimeManager.PlayOneShot(removeItemSound);
        }
    }
}