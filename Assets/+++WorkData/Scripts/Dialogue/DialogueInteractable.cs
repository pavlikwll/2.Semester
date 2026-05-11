using UnityEngine;

public class DialogueInteractable : BaseInteractable
{
    [SerializeField] bool singleInteraction;
    
    private bool _isStillSelected;
    private bool _DialogueActive;
    
    public override void Interact()
    {
        if (!canInteract) return;
        if (singleInteraction) canInteract = false;
        
        DialogueController.OnDialogueStarted?.Invoke(this);
        _DialogueActive = true;
        base.Interact();
        base.Unselected();
    }
    
    public override void Selected()
    {
        _isStillSelected = true;
        
        if (_DialogueActive) return;
        if (!canInteract) return;
        
        base.Selected();
    }

    public override void Unselected()
    {
        _isStillSelected = false;
        
        if (_DialogueActive) return;
        if (!canInteract) return;
        
        base.Unselected();
    }

    public void TrySelected()
    {
        if (!_isStillSelected) return;
        _DialogueActive = false;
        base.Selected();
    }
}
