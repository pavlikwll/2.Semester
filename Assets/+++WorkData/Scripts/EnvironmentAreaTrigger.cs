using UnityEngine;

public class EnvironmentAreaTrigger : MonoBehaviour
{
    [Min(0)]
    public int priority = 0;
    
    [SerializeField] private FootstepSoundArea footstepSoundArea;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerFootstepSound.OnAreaChange?.Invoke(footstepSoundArea, priority);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && priority > 0)
        {
            PlayerFootstepSound.OnPriorityExit?.Invoke();
        }
    }
}
