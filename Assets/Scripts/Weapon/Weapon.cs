using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float attackValue;
    public float criticalRat = 1.5f;
    
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            if (rb.velocity.magnitude > 10f)  //���� �ӵ� üũ
            {
                //���� �ֵθ��� ����
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            Debug.Log("�۵��Ѵ�");

            Monster hitedMonster = other.GetComponent<Skeleton>();
            if (hitedMonster != null)
            {
                Debug.Log("�Ϲ� ������");
                hitedMonster.Hit(attackValue);
            }
            else
            {
                Debug.Log("����");
            }
        }
        else if (other.gameObject.CompareTag("MonsterHead"))
        {
            Monster hitedMonster = other.transform.parent.GetComponent<Skeleton>();
            if (hitedMonster != null)
            {
                Debug.Log("ũ��Ʈ�� ������");
                hitedMonster.Hit(attackValue * criticalRat);
            }
        }
    }

    //private void OnCollisionEnter(Collision collision)  //���� �浹��, ���� ���� ����
    //{
    //    if (collision.collider.CompareTag("Monster"))
    //    {
    //        Monster hitedMonster = collision.collider.GetComponent<Skeleton>();
    //        if (hitedMonster != null)
    //        {
    //            hitedMonster.Hit(attackValue);
    //        }
    //    }
    //}
}
