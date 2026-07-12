//Volodymyr Pavlik
using System;
using UnityEngine;
using UnityEngine.Events;

public class Interaction : MonoBehaviour
{
    #region Inspector
    [Header("References")]
    public string interactableId;
    [SerializeField] private InteractionGroup[] interactionGroups;
    [SerializeField] private int interactionIndex;
    #endregion
    
    #region Interaction Functions
    public void Execute()
    {
        //Fallback in case the index is out of range
        if(interactionGroups.Length < interactionIndex)
        {
            Debug.LogError($"Interaction Index was {interactionIndex} out of range");
            return;
        }
        
        InteractionGroup currentInteraction = interactionGroups[interactionIndex];
        currentInteraction.onInteracted?.Invoke();
        if (currentInteraction.nextInteraction != -1)
        {
            interactionIndex = currentInteraction.nextInteraction;
        }
    }

    /// <summary>
    /// Set the interaction index to a specific interaction 
    /// </summary>
    /// <param name="interactIndex">the new interaction index</param>
    public void SetInteractionIndex(int interactIndex)
    {
        interactionIndex = interactIndex;
    }
    
    /// <summary>
    /// Set the interaction index to a specific interaction and executes the interaction
    /// </summary>
    /// <param name="interactIndex">the new interaction index</param>
    public void SetInteractionIndexExecute(int interactIndex)
    {
        interactionIndex = interactIndex;
        Execute();
    }
    
    
    #endregion
}
[Serializable]
public class InteractionGroup
{
    public string interactionName;
    public UnityEvent onInteracted;
    public int nextInteraction = -1;
}

