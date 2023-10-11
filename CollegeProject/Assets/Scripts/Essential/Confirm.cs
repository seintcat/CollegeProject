using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

// Ȯ��â�� �����ϴ� Ŭ����
public class Confirm : MonoBehaviour
{
    //Ȯ�ι�ư
    [SerializeField]
    Button button;

    //�ؽ�Ʈ
    [SerializeField]
    TextManager text;

    // Ȯ��â ��ư�� �Ҵ��� �̺�Ʈ ���
    [SerializeField]
    List<ButtonClickedEvent> events = new List<ButtonClickedEvent>();

    //Ȯ��â�� �̺�Ʈ �ٿ� ����
    public void SetDialog(int index)
    {
        button.onClick = events[index];
        text.ApplyText(index);
        gameObject.SetActive(true);
    }
}
