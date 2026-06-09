using System.Collections.Generic;
using UnityEngine;


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    [SerializeField] private List<InventorySlot> inventorySlots;
    [SerializeField] private List<ItemDefinition> allItemsInGame;

    private void Awake()
    {
        Instance = this;
    }
    public void SetInventoryItems(List<Item> items)
    {
        foreach (var currentSlot in inventorySlots)
        {
            currentSlot.ResetInventorySlot();
        }

        int currentItemIndex = 0;
        foreach (var currentItemInInventory in items)
        {
            //check slot, found Item ->stop
            foreach (var currentItemInGame in allItemsInGame)
            {
                if (currentItemInGame.id == currentItemInInventory.id)
                {
                    inventorySlots[currentItemIndex].FillInventorySlot(currentItemInGame, currentItemInInventory.amount);
                    break;
                }
            }

            currentItemIndex++;

        }
        
    }
}
