using System;
using UnityEngine;
using UnityEngine.Events;

public class BaseInteractable : MonoBehaviour
{
    public UnityEvent OnInteracted;
    public UnityEvent OnSelected;
    public UnityEvent OnUnselected;

    public bool canInteract = true;

    protected bool isSelected = false;

    protected void OnDisable()
    {
        Unselected();
    }

    public virtual void Interact()
    {
        OnInteracted?.Invoke();
    }
    
    
    public virtual void Selected()
    {
        if (isSelected) return;

        isSelected = true;
        OnSelected?.Invoke();
    }
    
    
    public virtual void Unselected()
    {
        if (!isSelected) return;
        isSelected = false;
        OnUnselected?.Invoke();
    }
    
}
