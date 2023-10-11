using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 로딩창을 표시하는 컴포넌트
public abstract class LoadingScreen : MonoBehaviour
{
    // 트랜스폼 변환용
    public RectTransform _transform;

    //로딩창의 시작, 종료
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
