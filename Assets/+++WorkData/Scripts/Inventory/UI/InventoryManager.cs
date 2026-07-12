//Volodymyr Pavlik
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Slots")]
    [SerializeField] private List<InventorySlot> inventorySlots;
    [SerializeField] private List<InventorySlot> hotbarSlots;

    [Header("Items")]
    [SerializeField] private List<ItemDefinition> allItemsInGame;

    [Header("Item Information")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;

    private void Awake()
    {
        Instance = this;
    }

    public void SetInventoryItems(List<Item> items)
    {
        ResetInventorySlots();

        int slotIndex = 0;

        foreach (Item inventoryItem in items)
        {
            if (slotIndex >= inventorySlots.Count)
                break;

            ItemDefinition definition = allItemsInGame.Find(
                itemDefinition => itemDefinition.id == inventoryItem.id
            );

            if (definition == null)
            {
                Debug.LogWarning(
                    $"No ItemDefinition found for ID: {inventoryItem.id}"
                );

                continue;
            }

            inventorySlots[slotIndex].FillInventorySlot(
                definition,
                inventoryItem.amount
            );

            slotIndex++;
        }

        SetHotbarItems(items);
        SelectFirstInventorySlot(items);
    }

    private void ResetInventorySlots()
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            slot.ResetInventorySlot();
        }
    }

    private void SelectFirstInventorySlot(List<Item> items)
    {
        if (items.Count == 0 || inventorySlots.Count == 0)
            return;

        inventorySlots[0].OnClickSlot();

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(
                inventorySlots[0].gameObject
            );
        }
    }

    private void SetHotbarItems(List<Item> items)
    {
        foreach (InventorySlot slot in hotbarSlots)
        {
            slot.ResetInventorySlot();
        }

        int hotbarIndex = 0;

        foreach (Item inventoryItem in items)
        {
            if (hotbarIndex >= hotbarSlots.Count)
                break;

            ItemDefinition definition = allItemsInGame.Find(
                itemDefinition =>
                    itemDefinition.id == inventoryItem.id &&
                    itemDefinition.itemType == ItemType.Special
            );

            if (definition == null)
                continue;

            hotbarSlots[hotbarIndex].FillInventorySlot(
                definition,
                inventoryItem.amount
            );

            hotbarIndex++;
        }
    }

    public void ShowItemInfo(ItemDefinition item)
    {
        if (item == null)
            return;

        itemIcon.sprite = item.sprite;
        itemName.SetText(item.displayName);
        itemDescription.SetText(item.description);
    }
}