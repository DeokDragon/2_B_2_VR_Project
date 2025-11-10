using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedWall : MonoBehaviour
{
    private Rigidbody[] partRbs;
    private RigidbodyConstraints defualtConstraint;
    void Start()
    {
        partRbs = transform.GetComponentsInChildren<Rigidbody>();     //모든 파츠의 Rigidbody 받기
        foreach (Rigidbody rb in partRbs)                             //각 Rigidbody에 제약 걸기
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;

        defualtConstraint = RigidbodyConstraints.None;                //제약 해제 변수 할당
    }

    public void DestroyWall()
    {
        foreach (Rigidbody rb in partRbs)
            rb.constraints = defualtConstraint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null)
        {
            if (other.gameObject.CompareTag("Spon"))
            {
                DestroyWall();
            }
        }
    }
}
