//Volodymyr Pavlik
using UnityEngine;

public class SingleInteractable : BaseInteractable
{
    public override void Interact()
    {
        if (!canInteract) return;
        canInteract = false;
        base.Interact();
        base.Unselected();
    }

    public override void Selected()
    {
        if (!canInteract) return;
        base.Selected();
    }
}
