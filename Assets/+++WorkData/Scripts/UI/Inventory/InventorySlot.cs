using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image itemDisplay;
    [SerializeField] private TextMeshProUGUI itemAmountText;

    private Button _inventorySlotButton;
    private ItemDefinition _itemDefenition;

    private void Awake()
    {
        _inventorySlotButton = GetComponent<Button>();
    }

    public void ResetInventorySlot()
    {
        itemDisplay.sprite = null;
        itemAmountText.SetText("");
        _inventorySlotButton.interactable = false;
    }

    public void FillInventorySlot(ItemDefinition itemDefenition, int amount)
    {
        itemDisplay.sprite = itemDefenition.sprite;
        itemAmountText.SetText(amount.ToString());
        _inventorySlotButton.interactable = true;
    }
}