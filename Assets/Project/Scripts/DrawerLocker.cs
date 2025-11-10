using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DrawerLocker : MonoBehaviour
{
    [SerializeField] XRGrabInteractable drawer;

    bool isLocked = true;
    
    private void Start()
    {
        Lock();    //잠금 처리
    }
    public void unLock()
    {
        isLocked = false;
        UpdateLockState();
    }

    public void Lock()
    {
        isLocked = true;
        UpdateLockState();
    }

    private void UpdateLockState()
    {
        if (isLocked) drawer.interactionLayers = InteractionLayerMask.GetMask("Nothing");
        else drawer.interactionLayers = InteractionLayerMask.GetMask("Default");
    }
}
