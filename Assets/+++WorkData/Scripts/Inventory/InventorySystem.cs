using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static Action<ItemDefinition, int> OnAddItemDefinition;
    public static Action<string, int> OnAddItemId;
    public static Action OnChangeInventory;
    
    [SerializeField] private List<Item> items;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] public GameObject inventoryContainer;

    private void OnEnable()
    {
        OnAddItemDefinition += Add;
        OnAddItemId += Add;
        OnChangeInventory += UpdateInventoryUI;
    }

    private void OnDisable()
    {
        OnAddItemDefinition -= Add;
        OnAddItemId -= Add;
        OnChangeInventory -= UpdateInventoryUI;
    }

    private void UpdateInventoryUI()
    {
        inventoryContainer.SetActive(!inventoryContainer.activeSelf);
        inventoryManager.SetInventoryItems(items);
    }

    public void ChangeInventoryState()
    {
        inventoryContainer.SetActive(!inventoryContainer.activeSelf);
        InventoryManager.Instance.SetInventoryItems(items);
    }
    
    public Item GetItem(string id)
    {
        foreach (var item in items)
        {
            if (item.id == id)
            {
                return item;
            }
        }

        return null;
    }
    
    public void Add(ItemDefinition itemDefenition, int amount)
    {
        Add(itemDefenition.id, amount);
    }

    public void Add(string id, int amount)
    {
        if(!ValidateItem(id)) return;

        Item item = GetItem(id);

        if (item == null)
        {
            items.Add(new Item(id, amount));
        }
        else
        {
            item.amount += amount;
            //TODO Check for error
        }

        OnChangeInventory?.Invoke();
    }

    private bool ValidateItem(string itemId)
    {
        if (string.IsNullOrWhiteSpace(itemId) || string.IsNullOrEmpty(itemId))
        {
            Debug.LogError("Item id is null or empty.");
            return false;
        }
        
        //TODO: Check if item exists

        return true;

    }
    
    public bool HasItem(string id, int amount)
    {
        Item item = GetItem(id);

        if (item == null)
            return false;

        return item.amount >= amount;
    }

    public void RemoveItem(string id, int amount)
    {
        Item item = GetItem(id);

        if (item == null)
            return;

        item.amount -= amount;

        if (item.amount <= 0)
        {
            items.Remove(item);
        }
        
        OnChangeInventory?.Invoke();
    }
}
