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
            if (rb.velocity.magnitude > 10f)  //무기 속도 체크
            {
                //무기 휘두르는 사운드
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            Debug.Log("작동한다");

            Monster hitedMonster = other.GetComponent<Skeleton>();
            if (hitedMonster != null)
            {
                Debug.Log("일반 데미지");
                hitedMonster.Hit(attackValue);
            }
            else
            {
                Debug.Log("없다");
            }
        }
        else if (other.gameObject.CompareTag("MonsterHead"))
        {
            Monster hitedMonster = other.transform.parent.GetComponent<Skeleton>();
            if (hitedMonster != null)
            {
                Debug.Log("크리트컬 데미지");
                hitedMonster.Hit(attackValue * criticalRat);
            }
        }
    }

    //private void OnCollisionEnter(Collision collision)  //몬스터 충돌시, 몬스터 공격 판정
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
