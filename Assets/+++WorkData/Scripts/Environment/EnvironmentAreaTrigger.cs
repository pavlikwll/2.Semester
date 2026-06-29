using UnityEngine;

public class EnvironmentAreaTrigger : MonoBehaviour
{
    public FootstepSoundArea footstepSoundArea;
    
    [Min(0)]
    public int priority = 0;
}
