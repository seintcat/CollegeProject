using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �ε�â�� ǥ���ϴ� ������Ʈ
public abstract class LoadingScreen : MonoBehaviour
{
    // Ʈ������ ��ȯ��
    public RectTransform _transform;

    //�ε�â�� ����, ����
    public virtual void On()
    {
        _transform.offsetMin = new Vector2(0, 0);
        _transform.offsetMax = new Vector2(0, 0);
        _transform.localScale = new Vector3(1f, 1f, 1f);

        gameObject.SetActive(true);
    }
    public virtual void Off()
    {
        gameObject.SetActive(false);
    }
}
