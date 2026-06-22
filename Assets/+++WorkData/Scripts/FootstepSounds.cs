using System;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(FootstepSounds), menuName = "MotionBrain/Footstep/Sounds")]
public class FootstepSounds : ScriptableObject
{
    public TilesToFootstepSound[] tilesToFootstepSounds;
}

[Serializable]
public class TilesToFootstepSound
{
    public string tileName;
    public string fmodFootstepLabel;
}
