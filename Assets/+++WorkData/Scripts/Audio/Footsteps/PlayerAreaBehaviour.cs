//Volodymyr Pavlik
using FMODUnity;
using UnityEngine;

public class PlayerAreaBehaviour : MonoBehaviour
{
    [Header("Footstep Timer")]
    [SerializeField] private float footstepTime = 0.35f;
    private float _footstepTimer;

    [Header("FMOD")]
    [SerializeField] private StudioEventEmitter footstepEmitter;

    private void Awake()
    {
        if (footstepEmitter == null)
        {
            footstepEmitter = GetComponent<StudioEventEmitter>();
        }
    }

    private void Update()
    {
        CalculateFootstepTimer();
    }

    private void CalculateFootstepTimer()
    {
        if (PlayerState.Instance.playerMovement == PlayerMovement.Idle)
            return;

        _footstepTimer += Time.deltaTime;

        if (_footstepTimer > footstepTime)
        {
            _footstepTimer = 0;
            PlayTileSound();
        }
    }

    private void PlayTileSound()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.06f);

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

        if (selectedArea == null)
            return;

        if (footstepEmitter == null)
        {
            Debug.LogError("No StudioEventEmitter found on Player.");
            return;
        }

        string surfaceLabel = selectedArea.footstepSoundArea.fmodFootstepEvent;

        footstepEmitter.EventInstance.setParameterByNameWithLabel("surface", surfaceLabel);
        footstepEmitter.Play();

        Debug.Log("Footstep surface: " + surfaceLabel);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.06f);
    }
}