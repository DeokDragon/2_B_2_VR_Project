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
        //�׽�Ʈ��
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
                //���� ����
                //���� �ִϸ��̼�

                timer = 0f;
                player.Hit(damage);
            }
        }
    }

    public override void Hit(float _damage)
    {
        //�ǰݻ��� ���
        //�ǰ� �ִϸ��̼� ���

        currentRenderer.material = hitMaterial;
        Invoke("ResetHit", 0.6f);

        base.Hit(_damage);

        Debug.Log($"���� ü�� : {hp}");
    }

    protected override void Dead()
    {
        //��� ���� ���
        //��� �ִϸ��̼� ���
        currentRenderer.material = deadMaterial;

        base.Dead();
    }

    private void ResetHit()
    {
        currentRenderer.material = defaultMaterial;
    }
}
