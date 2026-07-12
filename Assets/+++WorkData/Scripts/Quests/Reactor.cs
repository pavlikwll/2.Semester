//Volodymyr Pavlik
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Reactor : MonoBehaviour
{
    [Header("Conditions")]
    [Tooltip("Усі умови мають бути виконані.")]
    [SerializeField] private List<Condition> conditions = new();

    [Header("Events")]
    [SerializeField] private UnityEvent onFulfilled;
    [SerializeField] private UnityEvent onUnfulfilled;

    [Header("Quest")]
    [SerializeField] private QuestEntry questEntry;

    private bool _fulfilled;
    private bool _hasCheckedConditions;
    private InventorySystem _inventorySystem;

    private void Awake()
    {
        _inventorySystem = FindFirstObjectByType<InventorySystem>();

        if (_inventorySystem == null)
        {
            Debug.LogError("InventorySystem не знайдено на сцені.", this);
        }
    }

    private void OnEnable()
    {
        InventorySystem.OnInventoryChanged += CheckConditions;

        if (questEntry != null)
        {
            questEntry.gameObject.SetActive(true);
        }

        CheckConditions();
    }

    private void OnDisable()
    {
        InventorySystem.OnInventoryChanged -= CheckConditions;

        if (questEntry != null)
        {
            questEntry.gameObject.SetActive(false);
        }
    }

    private void CheckConditions()
    {
        if (_inventorySystem == null)
        {
            return;
        }

        bool newFulfilled = AreAllConditionsFulfilled();

        // Під час першої перевірки встановлюємо початковий стан.
        if (!_hasCheckedConditions)
        {
            _hasCheckedConditions = true;
            _fulfilled = newFulfilled;

            UpdateQuestStatus(newFulfilled);

            if (newFulfilled)
            {
                onFulfilled?.Invoke();
            }
            else
            {
                onUnfulfilled?.Invoke();
            }

            return;
        }

        // Нічого не змінилося - події повторно не запускаємо.
        if (_fulfilled == newFulfilled)
        {
            return;
        }

        _fulfilled = newFulfilled;
        UpdateQuestStatus(newFulfilled);

        if (newFulfilled)
        {
            onFulfilled?.Invoke();
        }
        else
        {
            onUnfulfilled?.Invoke();
        }
    }

    private bool AreAllConditionsFulfilled()
    {
        if (conditions == null || conditions.Count == 0)
        {
            return true;
        }

        foreach (Condition condition in conditions)
        {
            if (condition == null || condition.itemDefinition == null)
            {
                return false;
            }

            if (!_inventorySystem.HasItem(
                    condition.itemDefinition.id,
                    condition.amount))
            {
                return false;
            }
        }

        return true;
    }

    private void UpdateQuestStatus(bool isFulfilled)
    {
        if (questEntry != null)
        {
            questEntry.SetQuestStatus(isFulfilled);
        }
    }
}

[Serializable]
public class Condition
{
    public ItemDefinition itemDefinition;

    [Min(1)]
    public int amount = 1;
}