using System;
using UnityEngine;

public class PlayerAreaBehaviour : MonoBehaviour
{
    [Header("Footstep Timer")] [SerializeField]
    private float foostepTime;
    private float _footstepTimer;

    private void Update()
    {
        CalculateFootstepTimer();
    }

    private void CalculateFootstepTimer()
    {
        if (PlayerState.Instance.PlayerMovement == PlayerMovement.Idle) return;

        _footstepTimer += Time.deltaTime;

        if (_footstepTimer > foostepTime)
        {
            _footstepTimer = 0;
            PlayTileSound();
        }
    }

    private void PlayTileSound()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.02f);
        int highestPriority = -1;
        EnvironmentAreaTrigger selectedArea = null;  
        
        
        foreach (var hit in hits)
        {
            EnvironmentAreaTrigger area = hit.GetComponent<EnvironmentAreaTrigger>();  

            if (area != null && area.priority > highestPriority)
            {
                highestPriority = area.priority;
                selectedArea = area;
            }
        }

        //emitter.Play();
        
        if (selectedArea == null)
        {
            //fallback sound?
            //FMOD.RESULT defaulResult = emitter.EventInstance.setParameterByNameWithLabel("surface", "Default");
            return;
        }
        
        print(selectedArea.footstepSoundArea.fmodFootstepEvent);
        //PLAY FMOD DEFAULT SOUND 
        return;
    }
    //FMOD.RESULT result = emitter.EventInstance.setParameterByNameWithLabel("surface", selectedArea.footstepSoundArea.fmodFootstepEvent);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.06f); 
    }
}
    

