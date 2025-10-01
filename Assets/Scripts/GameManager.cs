using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("웨이브 설정")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject wave1Monster;
    [SerializeField] private GameObject wave2Monster;
    [SerializeField] private GameObject wave3Monster;

    [Header("컷씬 설정")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Image panelImage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))  //웨이브 시작 테스트용
        {
            StartWave();
        }
    }

    public void StartWave()  //게임 시작 실행 함수
    {
        StartCoroutine(Wave());
    }

    public void StartFadeUIPanel(float fadeDuration)
    {
        StartCoroutine(FadeUIPanel(fadeDuration));
    }

    IEnumerator Wave()
    {
        GameObject monster1 = Instantiate(wave1Monster, spawnPoint);

        yield return new WaitUntil(() => monster1 == null);

        GameObject monster2 = Instantiate(wave2Monster, spawnPoint);

        yield return new WaitUntil(() => monster2 == null);

        GameObject monster3 = Instantiate(wave3Monster, spawnPoint);

        yield return new WaitUntil(() => monster3 == null);

        //게임 종료 사운드 등

    }

    IEnumerator FadeUIPanel(float fadeDuration)
    {
        float t = 0;
        while (true)
        {

            yield return null;
        }
    }
}
