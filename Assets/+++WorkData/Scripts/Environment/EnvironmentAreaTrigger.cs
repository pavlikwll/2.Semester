using UnityEngine;

public class EnvironmentAreaTrigger : MonoBehaviour
{
    [SerializeField] private FootstepSoundArea footstepSoundArea;
    
    [Min(0)]
    [SerializeField] private int priority = 0;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerFootstepSound.OnAreaChange?.Invoke(footstepSoundArea, priority);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerFootstepSound.OnPriorityExit?.Invoke();
        }
    }
}
