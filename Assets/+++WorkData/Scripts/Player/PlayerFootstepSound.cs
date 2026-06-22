using System;
using System.Linq;
using FMODUnity;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerFootstepSound : MonoBehaviour
{
    public static Action<FootstepSoundArea, int> OnAreaChange;
    public static Action OnPriorityExit;

    [Header("Tilemap References")] 
    [SerializeField] private Tilemap[] tilemaps;
    [SerializeField] private FootstepSounds footstepSounds;
    
    [Header("Footstep Timer")]
    [SerializeField] private float foostepTime;
    
    
    private Tilemap[] _sortingTilemaps;
    private float _footstepTimer;
    private FootstepSoundArea[] _footstepSoundAreas;
    private int _currentPriority;

    private StudioEventEmitter _footStepEmitter;


    private void Awake()
    {
        _footStepEmitter = GetComponent<StudioEventEmitter>();
        
        _sortingTilemaps = tilemaps
            .Where(tm => tm != null)
            .OrderByDescending(tm =>
            {
                var r = tm.GetComponent<TilemapRenderer>();
                return SortingLayer.GetLayerValueFromID(r.sortingLayerID);
            })
            .ThenByDescending(tm => tm.GetComponent<TilemapRenderer>().sortingOrder).ToArray();
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
        CalculateFootstepTimer();
    }

    private void CalculateFootstepTimer()
    {
        if (PlayerState.Instance.playerMovement == PlayerMovement.Idle) return;
        
        _footstepTimer += Time.deltaTime;

        if (_footstepTimer > foostepTime)
        {
            _footstepTimer = 0;
            TileBase currentTile = GetTileAtPlayerPosition();
            PlayTileSound(currentTile);
            _footStepEmitter.Play();
        }
    }


    private void PlayTileSound(TileBase currentTile)
    {
        //emitter.Play();
        TilesToFootstepSound resultTile =
            footstepSounds.tilesToFootstepSounds.FirstOrDefault(ttfs => currentTile.name.Contains(ttfs.tileName));

        
        //resultTile = null;
        for (int i = 0; i < footstepSounds.tilesToFootstepSounds.Length; i++)
        {
            if (currentTile.name.Contains(footstepSounds.tilesToFootstepSounds[i].tileName))
            {
                resultTile = footstepSounds.tilesToFootstepSounds[i];
                break;
            }
        }//*/

        if (resultTile == null)
        {
            print(currentTile.name);
            //FMOD.RESULT defaulResult = emitter.EventInstance.setParameterByNameWithLabel("surface", "Default");
            //PLAY FMOD DEFAULT SOUND 
            return;
        }
        //FMOD.RESULT result = emitter.EventInstance.setParameterByNameWithLabel("surface", resultTile.fmodFootstepLabel);
        print(resultTile.fmodFootstepLabel);
    }

    private TileBase GetTileAtPlayerPosition()
    {
        foreach (var tilemap in _sortingTilemaps)
        {
            Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
            TileBase tile = tilemap.GetTile(cellPosition);

            if (tile != null)
                return tile;
        }

        return null;
    }
    
    
    
    
    private void AreaChange(FootstepSoundArea footstepSoundArea, int newPriority)
    {
//        _currentPriority = newPriority;
        
//        _footstepSoundAreas[_currentPriority] = footstepSoundArea;
    }

    private void PriorityExit()
    {
        _currentPriority--;
    }
}