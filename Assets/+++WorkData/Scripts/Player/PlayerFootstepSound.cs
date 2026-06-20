using System;
using UnityEngine;
using FMODUnity;

public class PlayerFootstepSound : MonoBehaviour
{
    public static Action<FootstepSoundArea, int> OnAreaChange;
    public static Action OnPriorityExit;
    
    [SerializeField] private float foostepTime;
    private float _footstepTimer;

    public FootstepSoundArea[] footstepSoundAreas;
    private int _currentPriority;
    
    [SerializeField] private StudioEventEmitter footstepEmitter;
    private void OnEnable()
    {
        OnAreaChange += AreaChange;
        OnPriorityExit += PriorityExit;
    }

    private void OnDisable()
    {
        OnAreaChange -= AreaChange;
        OnPriorityExit -= PriorityExit;

    }
    
    private void Update()
    {
        CalculateFootstepTimer();
    }

    private void CalculateFootstepTimer()
    {
        if (PlayerState.Instance.GetPlayerMovement() == PlayerMovement.Idle) return;
        
        _footstepTimer += Time.deltaTime;

        if (_footstepTimer > foostepTime)
        {
            _footstepTimer = 0;

            footstepEmitter.Play();

            footstepEmitter.EventInstance.setParameterByNameWithLabel(
                "surface",
                footstepSoundAreas[_currentPriority].area.ToString()
            );
        }
    }

    private void AreaChange(FootstepSoundArea footstepSoundArea, int newPriority)
    {
        _currentPriority = newPriority;
        
        footstepSoundAreas[_currentPriority] = footstepSoundArea;
    }

    private void PriorityExit()
    {
        _currentPriority--;
    }
}
