using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("기본 설정")]
    [SerializeField] private float maxHp;

    private float hp;

    [Header("테스트용 설정")]
    [SerializeField] private float speed;
    [SerializeField] private Camera firstPersonCamera;
    [SerializeField] private float mouseSenesitivity = 2f;
    [SerializeField] private float yMinLimit = 30;         //카메라 수직 회전 최소각
    [SerializeField] private float yMaxLimit = 90;         //카메라 수직 회전 최대각
    [SerializeField] private GameObject head;              //머리오브젝트
    [SerializeField] private GameObject weapon;              //머리오브젝트
    [SerializeField] private float weaponUp;              //머리오브젝트

    private Rigidbody rb;     //테스트용        
    private float theta = 0.0f;                  //카메라 수평회전 각도
    private float phi = 0.0f;                    //카메라의 수직회전 각도
    private float targetVerticalRotation;         //목표 수직 회전 각도
    private float RotationSpeed = 240f;           //수직 회전 속도

    private bool isAttacking;
    private bool isWeaponUp;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hp = maxHp;
        SetupCameras();
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("공격한다");
            weapon.SetActive(!isAttacking);

            isAttacking = !isAttacking;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!isWeaponUp)
            {
                weapon.transform.position += Vector3.up * weaponUp;
            }
            else
            {
                weapon.transform.position -= Vector3.up * weaponUp;
            }

            isWeaponUp = !isWeaponUp;
        }
    }

    void SetupCameras()
    {
        firstPersonCamera.transform.localPosition = new Vector3(0f, 0f, -0.3f);
        firstPersonCamera.transform.localRotation = Quaternion.identity;
    }

    public void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");         //좌우 입력(1, -1)
        float moveVertical = Input.GetAxis("Vertical");             //앞뒤 입력(1, -1)

        //이동 백터 계산
        Vector3 movement = transform.forward * moveVertical + transform.right * moveHorizontal;

        //이동방향으로 캐릭터 회전
        if (movement.magnitude > 0.1f)
        {
            Quaternion toRoation = Quaternion.LookRotation(movement, Vector2.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRoation, 10f * Time.deltaTime);
        }
        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
    }

    //카메라 및 캐릭터 회전처리하는 함수
    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSenesitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSenesitivity;

        //수평 회전(theta 값)
        theta += mouseX;
        theta = Mathf.Repeat(theta, 360f);

        //수직 회전 처리
        targetVerticalRotation -= mouseY;
        targetVerticalRotation = Mathf.Clamp(targetVerticalRotation, yMinLimit, yMaxLimit);

        phi = Mathf.MoveTowards(phi, targetVerticalRotation, RotationSpeed * Time.deltaTime);

        //플레이어, 머리회전 처리
        head.gameObject.transform.localRotation = Quaternion.Euler(phi, 0.0f, 0.0f);
        gameObject.transform.rotation = Quaternion.Euler(0.0f, theta, 0.0f);
    }


    public virtual void Hit(float _damage)   //피격 (피해를 입은 상태)
    {
        hp -= _damage;

        if (hp <= 0)
        {
            Dead();
        }
    }

    private void Dead()   //사망
    {
        SceneManager.LoadScene(0);   //씬 재시작
        gameObject.SetActive(false);
    }
}
