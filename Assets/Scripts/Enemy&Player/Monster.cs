using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyState
{
    PlayerNotExist,
    PlayerExist,
    Dead
}

public class Monster : MonoBehaviour
{
    [Header("기본 설정")]
    [SerializeField] protected float maxHp;
    [SerializeField] protected float speed;
    [SerializeField] protected float damage;
    [SerializeField] protected float attackDurration;
    [SerializeField] protected LayerMask playerMask;
    [Header("이동 설정")]
    [SerializeField] private float moveRadius1;
    [SerializeField] private float moveRadius2;
    [SerializeField] private float maxMoveDuration;
    [SerializeField] private float minMoveDuration;
    [Header("감지 설정")]
    [SerializeField] private float detectRadius;
    [SerializeField] protected Vector3 a_DetectBox;
    [SerializeField] protected float a_DetectUp;
    [SerializeField] protected float a_DetectFront;

    protected float hp;
    protected Rigidbody rb;
    protected EnemyState currentEnemyState;
    protected float timer;
    protected PlayerController player;

    private float currentMoveDuration;
    private float currentMoveRadius;
    private Vector3 movePos;
    private Coroutine currentCoroutine;


    protected virtual void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerController>();
        currentEnemyState = EnemyState.PlayerNotExist;

        hp = maxHp;
        currentMoveDuration = maxMoveDuration;

    }

    protected virtual void Update()
    {
        if (currentEnemyState == EnemyState.PlayerNotExist)  //플레이어가 없을 때
        {
            Move();
            DetectPlayer();
        }
        else if (currentEnemyState == EnemyState.PlayerExist)  //플레이어 발견됬을 때
        {
            ChasePlayer();
            CheckAttack();
        }
    }

    protected virtual void Move()  //이동 (적이 없을 때, 이동상태)
    {
        if(currentCoroutine == null) timer += Time.deltaTime;

        if (timer > currentMoveDuration)  //대기시간 경과
        {
            timer = 0f;

            int radiusId = UnityEngine.Random.Range(0, 2);

            switch (radiusId)
            {
                case 0: currentMoveRadius = moveRadius1; break;
                case 1: currentMoveRadius = moveRadius2; break;
            }
            Vector2 randomPos = Random.insideUnitCircle * currentMoveRadius;
            movePos = new Vector3(randomPos.x, transform.position.y, randomPos.y);

            if (currentCoroutine == null)
            {
                currentCoroutine = StartCoroutine(RotateAndMove(movePos));
            }
        }
    }

    private IEnumerator RotateAndMove(Vector3 willMovePos)
    {
        Vector3 direction = (willMovePos - transform.position).normalized;
        Quaternion rotateQuaternion = Quaternion.LookRotation(direction, Vector2.up);
        float t = 0;
        while (true)
        {
            t += Time.deltaTime / 1.5f;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateQuaternion, t);

            if (t > 1f) break;
            yield return null;
        }
        while (true)
        {
            float distance = Vector3.Distance(willMovePos, transform.position);

            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);

            if (distance < 0.2f)  //목적지 도착시, 초기화
            {
                currentMoveDuration = UnityEngine.Random.Range(minMoveDuration, maxMoveDuration);  //다음 이동 거리 랜덤 지정
                break;
            }

            yield return null;
        }
        currentCoroutine = null;
    }

    protected virtual void  DetectPlayer()  //플레이어 감지
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < detectRadius)  //플레이어 감지
        {
            currentEnemyState = EnemyState.PlayerExist;

            timer = attackDurration; //첫타는 지속시간 없음 (예외처리)
        }
    }

    protected virtual void ChasePlayer()  //플레이어 추적
    {
        if (player == null) return;

        Vector3 chasePlayerPos = player.transform.position;
        chasePlayerPos.y = transform.position.y;

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        Vector3 direction = (chasePlayerPos - transform.position).normalized;
        float distance = Vector3.Distance(chasePlayerPos, transform.position);

        if (distance > 0.2f)
        {
            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);  //플레이어 추적
        }

        transform.LookAt(chasePlayerPos);    //플레이어 방향 보기
    }

    protected virtual void CheckAttack()  //공격 (적이 공격 상태)
    {
        timer += Time.deltaTime;
        if (timer > attackDurration)
        {
            Vector3 origin = transform.position + transform.up * a_DetectUp + transform.forward * a_DetectFront;

            if (Physics.CheckBox(origin, a_DetectBox, transform.rotation, playerMask))
            {
                player.Hit(damage);
                Debug.Log("플레이어 피격");
            }

            timer = 0f;
        }
    }

    public virtual void Hit(float _damage)   //피격 (피해를 입은 상태)
    {
        hp -= _damage;

        if (hp <= 0)
        {
            currentEnemyState = EnemyState.Dead;
            Dead();
        }
    }

    protected virtual void Dead()   //사망
    {
        Destroy(gameObject, 2f);
    }

    private void OnDrawGizmos()
    {
        //공격범위 감지
        Vector3 origin = transform.position + transform.up * a_DetectUp + transform.forward * a_DetectFront;

        Gizmos.matrix = Matrix4x4.TRS(origin, transform.rotation, Vector3.one);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, a_DetectBox * 2f);

        //이동 범위 표시
        Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, moveRadius1);
        Gizmos.DrawWireSphere(transform.position, moveRadius2);

        //플레이어 감지 범위 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);

        //이동할 위치 표시
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(movePos, 0.3f);
    }
}
