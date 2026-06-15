using UnityEngine;

[CreateAssetMenu(fileName = "New Footstep Sound", menuName = "MotionBrain/Footstep/SoundArea")]
public class FootstepSoundArea : ScriptableObject
{
    public Area area;
    public string fmodFootstepEvent = "defaultEvent";
}
