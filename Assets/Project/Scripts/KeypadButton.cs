using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeypadButton : MonoBehaviour
{
    [SerializeField] KeypadButtonType type = KeypadButtonType.None;
    [SerializeField] TextMeshPro buttonText;
    [SerializeField] KeypadSystem parent;

    void Start()
    {
        if (KeypadButtonType.None == type)
        {
            Debug.Log("키패드에 KeypadButtonType이 설정되지 않았습니다.");
            return;
        }

        switch (type)
        {
            case KeypadButtonType.One:SetButtonText("1");
                break;
            case KeypadButtonType.Two:SetButtonText("2");
                break;
            case KeypadButtonType.Three:SetButtonText("3");
                break;
            case KeypadButtonType.Four:SetButtonText("4");
                break;
            case KeypadButtonType.Five:SetButtonText("5");
                break;
            case KeypadButtonType.Six:SetButtonText("6");
                break;
            case KeypadButtonType.Seven:SetButtonText("7");
                break;
            case KeypadButtonType.Eight:SetButtonText("8");
                break;
            case KeypadButtonType.Nine:SetButtonText("9");
                break;
            case KeypadButtonType.Zero:SetButtonText("0");
                break;
            case KeypadButtonType.Delete:SetButtonText("D");
                break;
            case KeypadButtonType.Enter:SetButtonText("E");
                break;
        }
    }

    private void SetButtonText(string text)
    {
        if(buttonText == null)
        {
            Debug.LogError("버튼 TMPro가 설정되지 않았습니다.");
            return;
        }
        buttonText.text = text;
    }

    public void InputButton()
    {
        switch (type)
        {
            case KeypadButtonType.One:parent.InputNumber(1); break;
            case KeypadButtonType.Two:parent.InputNumber(2); break;
            case KeypadButtonType.Three: parent.InputNumber(3); break;
            case KeypadButtonType.Four: parent.InputNumber(4); break;
            case KeypadButtonType.Five:parent.InputNumber(5); break;
            case KeypadButtonType.Six: parent.InputNumber(6); break;
            case KeypadButtonType.Seven:parent.InputNumber(7); break;
            case KeypadButtonType.Eight: parent.InputNumber(8); break;
            case KeypadButtonType.Nine: parent.InputNumber(9); break;
            case KeypadButtonType.Zero: parent.InputNumber(0); break;
            case KeypadButtonType.Delete: parent.DeleteNumber(); break;
            case KeypadButtonType.Enter: parent.EnterResult(); break;
        }
    }

    private enum KeypadButtonType
    {
        One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Zero,
        Delete, Enter,
        None
    }
}
