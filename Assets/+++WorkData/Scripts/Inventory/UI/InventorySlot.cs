//Volodymyr Pavlik
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image itemDisplay;
    [SerializeField] private TextMeshProUGUI itemAmountText;
    [SerializeField] private Button inventorySlotButton;
    
    private ItemDefinition _itemDefinition;

    private void Awake()
    {
        if (inventorySlotButton == null)
        {
            inventorySlotButton = GetComponent<Button>();
        }
    }

    public void ResetInventorySlot()
    {
        _itemDefinition = null;

        itemDisplay.sprite = null;
        itemDisplay.enabled = false;

        itemAmountText.SetText("");

        if (inventorySlotButton != null)
        {
            inventorySlotButton.interactable = false;
        }
    }

    public void FillInventorySlot(ItemDefinition itemDefinition, int amount)
    {
        _itemDefinition = itemDefinition;

        itemDisplay.sprite = itemDefinition.sprite;
        itemDisplay.enabled = true;

        itemAmountText.SetText(amount.ToString());

        if (inventorySlotButton != null)
        {
            inventorySlotButton.interactable = true;
        }
    }
    
    public void OnClickSlot()
    {
        if (_itemDefinition == null)
        {
            return;
        }
        InventoryManager.Instance.ShowItemInfo(_itemDefinition);
    }
}