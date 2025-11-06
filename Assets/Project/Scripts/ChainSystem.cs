using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

public class ChainSystem : MonoBehaviour
{
    //이동 가능 범위 (반지름)
    //이동 제한 기준 위치
    //이동 가능 여부 불값
    //로코 모션 컴포넌트

    [SerializeField] Transform centerTrans;
    [SerializeField] float radius = 3f;
    [SerializeField] LocomotionMediator mediator;
    
    private bool puzzleSolved = false;

    // Update is called once per frame
    void Update()
    {
        if (puzzleSolved)
            return;

        var origin = mediator.xrOrigin;

        if (origin == null) return;

        Vector3 playerPos = origin.Camera.transform.position;
        Vector3 centerPos = centerTrans.position;

        float distance = Vector3.Distance(playerPos, centerPos);

        if (distance > radius)
        {
            Vector3 direction = (playerPos - centerPos).normalized;
            Vector3 outlinePos = centerPos + direction * radius;

            Vector3 offset = playerPos - outlinePos;

            origin.transform.position -= offset;

            Debug.Log("범위를 벗어나서 이동이 제한됨");
        }
    }

    public void UnLockChain()
    {
        puzzleSolved = true;
        Debug.Log("쇠사슬의 잠금이 해제되었습니다.");
    }
}
