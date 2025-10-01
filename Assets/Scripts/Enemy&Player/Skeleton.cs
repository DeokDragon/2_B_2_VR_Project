using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Monster
{
    public Material hitMaterial;
    public Material deadMaterial;

    private Material defaultMaterial;
    private MeshRenderer currentRenderer;

    protected override void OnEnable()
    {
        //테스트용
        defaultMaterial = GetComponent<MeshRenderer>().material;
        currentRenderer = GetComponent<MeshRenderer>();


        base.OnEnable();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Move()
    {
        base.Move();
    }

    protected override void DetectPlayer()
    {
        base.DetectPlayer();

    }

    protected override void ChasePlayer()
    {
        base.ChasePlayer();
    }

    protected override void CheckAttack()
    {
        base.CheckAttack();

        Vector3 origin = transform.position + transform.up * a_DetectUp + transform.forward * a_DetectFront;
        if (Physics.CheckBox(origin, a_DetectBox, transform.rotation, playerMask))
        {
            if (timer >= attackDurration)
            {
                //공격 사운드
                //공격 애니메이션

                timer = 0f;
                player.Hit(damage);
            }
        }
    }

    public override void Hit(float _damage)
    {
        //피격사운드 재생
        //피격 애니메이션 재생

        currentRenderer.material = hitMaterial;
        Invoke("ResetHit", 0.6f);

        base.Hit(_damage);

        Debug.Log($"현재 체력 : {hp}");
    }

    protected override void Dead()
    {
        //사망 사운드 재생
        //사망 애니메이션 재생
        currentRenderer.material = deadMaterial;

        base.Dead();
    }

    private void ResetHit()
    {
        currentRenderer.material = defaultMaterial;
    }
}
