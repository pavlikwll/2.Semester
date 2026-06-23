using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    [SerializeField] private List<InventorySlot> inventorySlots;
    [SerializeField] private List<InventorySlot> hotbarSlots;
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
                if (currentItemIndex >= inventorySlots.Count)
                    break;

                ItemDefinition definition = allItemsInGame.Find(item => item.id == currentItemInInventory.id);

                if (definition == null)
                {
                    Debug.LogWarning($"No ItemDefinition found for ID: {currentItemInInventory.id}");
                    continue;
                }

                inventorySlots[currentItemIndex].FillInventorySlot(definition, currentItemInInventory.amount);

                currentItemIndex++;
            }

            if (items.Count > 0)
            {
                inventorySlots[0].OnClickSlot();
                EventSystem.current.SetSelectedGameObject(inventorySlots[0].gameObject);
            }

            SetHotbarItems(items);
        }
    }

    private void SetHotbarItems(List<Item> items)
    {
        foreach (var slot in hotbarSlots)
        {
            slot.ResetInventorySlot();
        }

        int hotbarIndex = 0;
        foreach (var currentItemInInventory in items)
        {
            foreach (var currentItemInGame in allItemsInGame)
            {
                if (currentItemInGame.id == currentItemInInventory.id &&
                    currentItemInGame.itemType == ItemType.Special)
                {
                    hotbarSlots[hotbarIndex].FillInventorySlot(currentItemInGame, currentItemInInventory.amount);
                    hotbarIndex++;
                    break;
                }
            }
        }

    }

    public void ShowItemInfo(ItemDefinition item)
    {
        itemIcon.sprite = item.sprite;
        itemName.SetText(item.displayName);
        itemDescription.SetText(item.description);
    }
}