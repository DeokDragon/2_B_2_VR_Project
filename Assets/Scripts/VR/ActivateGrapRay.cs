using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ActivateGrapRay : MonoBehaviour
{
    public GameObject rightRay;
    public GameObject leftRay;

    public XRDirectInteractor rightDirectGrab;
    public XRDirectInteractor LeftDirectGrab;
    void Update()
    {
        if (rightRay != null)
        {
            rightRay.SetActive(rightDirectGrab.interactablesSelected.Count == 0);
        }
        if (leftRay != null)
        {
            leftRay.SetActive(LeftDirectGrab.interactablesSelected.Count == 0);
        }
    }
}
