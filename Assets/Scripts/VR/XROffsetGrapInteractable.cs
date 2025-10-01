using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XROffsetGrapInteractable : XRGrabInteractable
{
    private Vector3 initialLocalPos;
    private Quaternion initialLocalRot;

    void Start()
    {
        if (!attachTransform)
        {
            GameObject attachPoint = new GameObject("Offset Grap pivot");
            attachPoint.transform.SetParent(transform, false);
            attachTransform = attachPoint.transform;
        }
        else
        {
            initialLocalPos = attachTransform.position;
            initialLocalRot = attachTransform.rotation;
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactorObject is XRDirectInteractor)
        {
            attachTransform.position = args.interactorObject.transform.position;
            attachTransform.rotation = args.interactorObject.transform.rotation;
        }
        else
        {
            attachTransform.position = initialLocalPos;
            attachTransform.rotation = initialLocalRot;
        }
        base.OnSelectEntered(args);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
