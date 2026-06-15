using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepSound : MonoBehaviour
{
    public static Action<FootstepSoundArea, int> OnAreaChange;
    public static Action OnPriorityExit;
    
    [SerializeField] private float footstepTime;
    private float _footstepTimer;

    public List<FootstepSoundArea> _footstepSoundAreas;
    public int _currentPriority = -1;

    private void Awake()
    {
        _currentPriority = -1;
    }

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
        if (PlayerState.Instance.GetPlayerAction() == PlayerAction.Default) return;
        if (_footstepSoundAreas.Count < 1) return;
        
        _footstepTimer += Time.deltaTime;
        
        if (_footstepTimer > footstepTime)
        {
            _footstepTimer = 0;
            //TODO: PLAY SOUND
            //FMOD.RESULT result = emitter.EventInstance.setParameterByNameWithLabel("surface", label); //Label = Parameter Type in FMOD
           print($"Play Sound with event: {_footstepSoundAreas[_currentPriority].fmodFootstepEvent}");
        }
    }

    private void AreaChange(FootstepSoundArea footstepSoundArea, int priority)
    {
        if (priority == _currentPriority)
        {
            _footstepSoundAreas[priority] = footstepSoundArea;
            return;
        }
        _currentPriority = priority;
        _footstepSoundAreas.Add(footstepSoundArea);
    }

    private void PriorityExit()
    {
        _footstepSoundAreas.RemoveAt(_currentPriority);
        _currentPriority --;
    }
}
 