//Volodymyr Pavlik
using UnityEngine;
using FMODUnity;

public class FMODAnimationEvent : MonoBehaviour
{
    public void Play(string eventPath)
    {
        if (string.IsNullOrEmpty(eventPath))
        {
            Debug.LogWarning("FMOD Event Path ist leer!");
            return;
        }

        RuntimeManager.PlayOneShot(eventPath, transform.position);
    }
}
