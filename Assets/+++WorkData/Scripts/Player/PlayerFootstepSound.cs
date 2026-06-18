using System;
using UnityEngine;

public class PlayerFootstepSound : MonoBehaviour
{
    public static Action<FootstepSoundArea, int> OnAreaChange;
    public static Action OnPriorityExit;
    
    [SerializeField] private float foostepTime;
    private float _footstepTimer;

    public FootstepSoundArea[] footstepSoundAreas;
    private int _currentPriority;
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
            //TODO: PLAY SOUND
            //TODO: enum ? _footstepSoundArea.area..... with switch or other?
            //FMOD.RESULT result = emitter.EventInstance.setParameterByNameWithLabel("surface", label); //Label = Parameter Type in FMOD
            print($"Play Sound with event: {footstepSoundAreas[_currentPriority].fmodFootstepEvent}");
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
