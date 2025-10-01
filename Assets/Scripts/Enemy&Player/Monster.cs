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
    [Header("�⺻ ����")]
    [SerializeField] protected float maxHp;
    [SerializeField] protected float speed;
    [SerializeField] protected float damage;
    [SerializeField] protected float attackDurration;
    [SerializeField] protected LayerMask playerMask;
    [Header("�̵� ����")]
    [SerializeField] private float moveRadius1;
    [SerializeField] private float moveRadius2;
    [SerializeField] private float maxMoveDuration;
    [SerializeField] private float minMoveDuration;
    [Header("���� ����")]
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
        if (currentEnemyState == EnemyState.PlayerNotExist)  //�÷��̾ ���� ��
        {
            Move();
            DetectPlayer();
        }
        else if (currentEnemyState == EnemyState.PlayerExist)  //�÷��̾� �߰߉��� ��
        {
            ChasePlayer();
            CheckAttack();
        }
    }

    protected virtual void Move()  //�̵� (���� ���� ��, �̵�����)
    {
        if(currentCoroutine == null) timer += Time.deltaTime;

        if (timer > currentMoveDuration)  //���ð� ���
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

            if (distance < 0.2f)  //������ ������, �ʱ�ȭ
            {
                currentMoveDuration = UnityEngine.Random.Range(minMoveDuration, maxMoveDuration);  //���� �̵� �Ÿ� ���� ����
                break;
            }

            yield return null;
        }
        currentCoroutine = null;
    }

    protected virtual void  DetectPlayer()  //�÷��̾� ����
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < detectRadius)  //�÷��̾� ����
        {
            currentEnemyState = EnemyState.PlayerExist;

            timer = attackDurration; //ùŸ�� ���ӽð� ���� (����ó��)
        }
    }

    protected virtual void ChasePlayer()  //�÷��̾� ����
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
            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);  //�÷��̾� ����
        }

        transform.LookAt(chasePlayerPos);    //�÷��̾� ���� ����
    }

    protected virtual void CheckAttack()  //���� (���� ���� ����)
    {
        timer += Time.deltaTime;
        if (timer > attackDurration)
        {
            Vector3 origin = transform.position + transform.up * a_DetectUp + transform.forward * a_DetectFront;

            if (Physics.CheckBox(origin, a_DetectBox, transform.rotation, playerMask))
            {
                player.Hit(damage);
                Debug.Log("�÷��̾� �ǰ�");
            }

            timer = 0f;
        }
    }

    public virtual void Hit(float _damage)   //�ǰ� (���ظ� ���� ����)
    {
        hp -= _damage;

        if (hp <= 0)
        {
            currentEnemyState = EnemyState.Dead;
            Dead();
        }
    }

    protected virtual void Dead()   //���
    {
        Destroy(gameObject, 2f);
    }

    private void OnDrawGizmos()
    {
        //���ݹ��� ����
        Vector3 origin = transform.position + transform.up * a_DetectUp + transform.forward * a_DetectFront;

        Gizmos.matrix = Matrix4x4.TRS(origin, transform.rotation, Vector3.one);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, a_DetectBox * 2f);

        //�̵� ���� ǥ��
        Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, moveRadius1);
        Gizmos.DrawWireSphere(transform.position, moveRadius2);

        //�÷��̾� ���� ���� ǥ��
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);

        //�̵��� ��ġ ǥ��
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(movePos, 0.3f);
    }
}
