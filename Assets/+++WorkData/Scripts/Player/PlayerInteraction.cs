//Volodymyr Pavlik
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public static Action OnInteract;
    public List<BaseInteractable> interactables = new();

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
        if (interactables.Count == 0)
        {
            return;
        }

        interactables[0].Interact();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        BaseInteractable interactable = other.GetComponent<BaseInteractable>();
        if (interactable != null)
        {
            interactables.Add(interactable);
            interactable.Selected();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        BaseInteractable interactable = other.GetComponent<BaseInteractable>();
        if (interactable != null)
        {
            interactable.Unselected();
            interactables.Remove(interactable);
        }
    }
}
