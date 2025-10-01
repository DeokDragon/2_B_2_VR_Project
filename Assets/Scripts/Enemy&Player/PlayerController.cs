using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("�⺻ ����")]
    [SerializeField] private float maxHp;

    private float hp;

    [Header("�׽�Ʈ�� ����")]
    [SerializeField] private float speed;
    [SerializeField] private Camera firstPersonCamera;
    [SerializeField] private float mouseSenesitivity = 2f;
    [SerializeField] private float yMinLimit = 30;         //ī�޶� ���� ȸ�� �ּҰ�
    [SerializeField] private float yMaxLimit = 90;         //ī�޶� ���� ȸ�� �ִ밢
    [SerializeField] private GameObject head;              //�Ӹ�������Ʈ
    [SerializeField] private GameObject weapon;              //�Ӹ�������Ʈ
    [SerializeField] private float weaponUp;              //�Ӹ�������Ʈ

    private Rigidbody rb;     //�׽�Ʈ��        
    private float theta = 0.0f;                  //ī�޶� ����ȸ�� ����
    private float phi = 0.0f;                    //ī�޶��� ����ȸ�� ����
    private float targetVerticalRotation;         //��ǥ ���� ȸ�� ����
    private float RotationSpeed = 240f;           //���� ȸ�� �ӵ�

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
            Debug.Log("�����Ѵ�");
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
        float moveHorizontal = Input.GetAxis("Horizontal");         //�¿� �Է�(1, -1)
        float moveVertical = Input.GetAxis("Vertical");             //�յ� �Է�(1, -1)

        //�̵� ���� ���
        Vector3 movement = transform.forward * moveVertical + transform.right * moveHorizontal;

        //�̵��������� ĳ���� ȸ��
        if (movement.magnitude > 0.1f)
        {
            Quaternion toRoation = Quaternion.LookRotation(movement, Vector2.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRoation, 10f * Time.deltaTime);
        }
        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
    }

    //ī�޶� �� ĳ���� ȸ��ó���ϴ� �Լ�
    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSenesitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSenesitivity;

        //���� ȸ��(theta ��)
        theta += mouseX;
        theta = Mathf.Repeat(theta, 360f);

        //���� ȸ�� ó��
        targetVerticalRotation -= mouseY;
        targetVerticalRotation = Mathf.Clamp(targetVerticalRotation, yMinLimit, yMaxLimit);

        phi = Mathf.MoveTowards(phi, targetVerticalRotation, RotationSpeed * Time.deltaTime);

        //�÷��̾�, �Ӹ�ȸ�� ó��
        head.gameObject.transform.localRotation = Quaternion.Euler(phi, 0.0f, 0.0f);
        gameObject.transform.rotation = Quaternion.Euler(0.0f, theta, 0.0f);
    }


    public virtual void Hit(float _damage)   //�ǰ� (���ظ� ���� ����)
    {
        hp -= _damage;

        if (hp <= 0)
        {
            Dead();
        }
    }

    private void Dead()   //���
    {
        SceneManager.LoadScene(0);   //�� �����
        gameObject.SetActive(false);
    }
}
