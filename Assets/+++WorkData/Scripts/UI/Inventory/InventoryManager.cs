using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    [SerializeField] private List<InventorySlot> inventorySlots;
    [SerializeField] private List<ItemDefinition> allItemsInGame;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;

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
        
        if (items.Count > 0)
        {
            inventorySlots[0].OnClickSlot();
            EventSystem.current.SetSelectedGameObject(inventorySlots[0].gameObject);
        }
    }
    
    public void ShowItemInfo(ItemDefinition item)
    {
        itemIcon.sprite = item.sprite;
        itemName.SetText(item.displayName);
        itemDescription.SetText(item.description);
    }
}
