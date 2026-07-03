using FMODUnity;
using UnityEngine;

public class UIAudio : MonoBehaviour
{
    [SerializeField] private EventReference buttonClick;
    [SerializeField] private EventReference buttonHover;
    [SerializeField] private EventReference inventoryOpen;
    [SerializeField] private EventReference inventoryClose;

    public void PlayButtonClick()
    {
        RuntimeManager.PlayOneShot(buttonClick);
    }

    public void PlayButtonHover()
    {
        RuntimeManager.PlayOneShot(buttonHover);
    }

    public void PlayInventoryOpen()
    {
        RuntimeManager.PlayOneShot(inventoryOpen);
    }

    public void PlayInventoryClose()
    {
        RuntimeManager.PlayOneShot(inventoryClose);
    }
}