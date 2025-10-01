using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireBulletOnActivate : MonoBehaviour
{
    public GameObject fireBullet;
    public Transform firePoint;
    public float fireSpeed;

    void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(FireBullet);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FireBullet(ActivateEventArgs args)
    {
        GameObject spawnBullet = Instantiate(fireBullet, firePoint);
        spawnBullet.GetComponent<Rigidbody>().velocity = firePoint.forward * fireSpeed;
        Destroy(spawnBullet, 5f);
    }
}
