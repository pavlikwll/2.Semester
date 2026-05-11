using UnityEngine;

public class DialogueInteractable : BaseInteractable
{
    [SerializeField] bool singleInteraction;
    
    private bool _isStillSelected;
    private bool _dialogueActive;
    
    public override void Interact()
    {
        if (!canInteract) return;
        if (singleInteraction) canInteract = false;
        
        DialogueController.OnDialogueStarted?.Invoke(this);
        _dialogueActive = true;
        base.Interact();
        base.Unselected();
    }
    
    public override void Selected()
    {
        _isStillSelected = true;
        
        if (_dialogueActive) return;
        if (!canInteract) return;
        
        base.Selected();
    }

    public override void Unselected()
    {
        _isStillSelected = false;
        
        if (_dialogueActive) return;
        if (!canInteract) return;
        
        base.Unselected();
    }

    public void TrySelected()
    {
        _dialogueActive = false;

        if (!_isStillSelected) return;
        if (!canInteract) return;

        base.Selected();
    }
}
