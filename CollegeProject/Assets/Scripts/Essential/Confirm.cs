using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

// 확인창을 구성하는 클래스
public class Confirm : MonoBehaviour
{
    //확인버튼
    [SerializeField]
    Button button;

    //텍스트
    [SerializeField]
    TextManager text;

    // 확인창 버튼에 할당할 이벤트 목록
    [SerializeField]
    List<ButtonClickedEvent> events = new List<ButtonClickedEvent>();

    //확인창에 이벤트 붙여 실행
    public void SetDialog(int index)
    {
        button.onClick = events[index];
        text.ApplyText(index);
        gameObject.SetActive(true);
    }
}
