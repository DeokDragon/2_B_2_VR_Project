using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System;

public class KeypadSystem : MonoBehaviour
{
    [SerializeField] string correctResultString;      //비번 정답
    [SerializeField] TextMeshPro resultText;          //입력 결과 텍스트
     
    const int resultNumberCount = 4;                  //비밀번호 숫자 개수

    int[] correctResult = new int[resultNumberCount]; //정답인 비밀번호 숫자 배열
    int[] inputResult = new int[resultNumberCount];   //입력되는 비밀번호 숫자 배열
    int currentInputIndex = 0;

    void Start()
    {
        for (int i = 0; i < resultNumberCount; i++)
            inputResult[i] = -1;

        int count = 0;
        foreach (char c in correctResultString)
        {
            if (int.TryParse(c.ToString(), out int result))
            {
                correctResult[count++] = result;
            }
            else
            {
                Debug.LogError("키패드 비밀번호가 제대로 설정되지 않았습니다. 오류예시) 숫자만 입력할 것");
            }
        }
    }

    public void InputNumber(int number)     //키패드 숫자 입력
    {
        if (currentInputIndex >= resultNumberCount)
        {
            Debug.Log("모든 번호가 이미 입력되었습니다. 더 이상 입력이 불가합니다");
            return;
        }
        Debug.Log($"{number}가 추가되었습니다.");

        resultText.text += number.ToString();
        inputResult[currentInputIndex] = number;
        //입력 사운드 재생
        currentInputIndex++;
    }

    public void DeleteNumber()    //키패드 숫자 삭제
    {
        if (currentInputIndex <= 0)
        {
            Debug.Log("더 이상의 숫자는 삭제할 수 없습니다.");
            return;
        }

        string temp = resultText.text;
        if (temp.Length > 0) resultText.text = temp.Substring(0, temp.Length - 1);

        Debug.Log($"{inputResult[currentInputIndex - 1]}가 삭제되었습니다.");
        inputResult[currentInputIndex - 1] = -1;
        currentInputIndex--;
    }

    public void EnterResult()     //키패드 정답 입력
    {
        if (inputResult.Contains(-1)) //아직 입력하지 않은 부분이 있을 경우, 반환처리
        {
            Debug.Log("아직 값이 다 채워지지 않았습니다. 다 채운후, 입력해주세요");
            return;
        }

        bool isCorrect = true;
        for (int i = 0; i < resultNumberCount; i++)
            if (inputResult[i] != correctResult[i]) isCorrect = false;

        if (isCorrect)    //비번 입력을 맞출 경우
        {
            Debug.Log("비밀번호를 맞추셨습니다. 서랍 잠금이 해제됩니다.");
            DrawerLocker locker = FindObjectOfType<DrawerLocker>();
            if (locker != null)
            {
                locker.unLock();     //잠금 해제 함수 (서랍 잠금 해제)
            }
        }
        else
        {
            Debug.Log("비밀번호를 틀리셨습니다. 다시 입력해주세요");
            //서랍 잠금 해제 실패 사운드 재생
        }
    }

}
