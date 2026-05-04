using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public static Action OnInteract;
    private BaseInteractable _currentInteractable;

    private void OnEnable()
    {
        OnInteract += Interact;
    }

    private void OnDisable()
    {
        OnInteract -= Interact;
    }

    private void Interact()
    {
        if (!_currentInteractable) return;
        
        _currentInteractable.Interact();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        BaseInteractable currentBaseInteractable = other.GetComponent<BaseInteractable>();
        if (currentBaseInteractable)
        {
            _currentInteractable = currentBaseInteractable;
            _currentInteractable.Selected();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        BaseInteractable currentBaseInteractable = other.GetComponent<BaseInteractable>();
        if (currentBaseInteractable)
        {
            _currentInteractable.Unselected();
            _currentInteractable = null;
        }
    }
}
