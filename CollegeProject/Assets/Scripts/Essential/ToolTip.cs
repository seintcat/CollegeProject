using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 팁 표시 및 스토리 진행용 컴포넌트
public class ToolTip : MonoBehaviour
{
    //각종 이미지 컴포넌트
    [SerializeField]
    List<Image> images;

    //텍스트
    [SerializeField]
    TextManager text;

    //이미지 비율 설정용
    [SerializeField]
    List<AspectRatioFitter> fitters;

    //툴팁 표시 내용
    ToolTipData data;

    // 자동종료 코루틴 재생용
    IEnumerator enumerator;

    //툴팁 재생
    public void Make(ToolTipData _data)
    {
        gameObject.SetActive(true);
        Color color;

        // 초기화
        foreach (Image image in images)
        {
            image.sprite = null;
            color = image.color;
            color.a = 0f;
            image.color = color;
        }

        data = _data;

        if(data.context != null)
            text.ApplyText(data.context);

        if(data.filterIndex > -1)
            FilterManager.Play(data.filterIndex);

        // 비율 적용
        if(data.raitios.Count > fitters.Count)
        {
            for (int i = 1; i < fitters.Count; i++)
                if (fitters[i] != null)
                    fitters[i].aspectRatio = data.raitios[i];
        }
        else
        {
            for (int i = 1; i < data.raitios.Count; i++)
                if (fitters[i] != null)
                    fitters[i].aspectRatio = data.raitios[i];
        }

        // 이미지 소스 적용
        if (data.sprites.Count > images.Count)
        {
            for (int i = 0; i < images.Count; i++)
                if (images[i] != null)
                {
                    images[i].sprite = data.sprites[i];
                    color = images[i].color;
                    color.a = 255f;
                    images[i].color = color;
                    images[i].gameObject.GetComponent<RectTransform>().localScale = new Vector3(data.scale[i].x, data.scale[i].y, 1f);
                }
        }
        else
        {
            for (int i = 0; i < data.sprites.Count; i++)
                if (images[i] != null)
                {
                    images[i].sprite = data.sprites[i];
                    color = images[i].color;
                    color.a = 255f;
                    images[i].color = color;
                    images[i].gameObject.GetComponent<RectTransform>().localScale = new Vector3(data.scale[i].x, data.scale[i].y, 1f);
                }
        }

        if (data.time > 0)
        {
            enumerator = Make_(data.time);
            StartCoroutine(enumerator);
        }
    }
    IEnumerator Make_(float Time)
    {
        while (true)
        {
            yield return new WaitForSeconds(Time);
            Off(true);
        }
    }

    //재생 종료, 이어지는 툴팁 재생여부
    public void Off(bool playChain)
    {
        if (data.time > 0)
            StopCoroutine(enumerator);

        if (data.filterIndex > -1)
            FilterManager.Off(data.filterIndex);

        gameObject.SetActive(false);

        if (playChain && data.chainIndex > -1)
            ToolTipManager.Show(data.chainIndex);
    }

    // 초기화 메서드
    public void Init()
    {
        RectTransform rectTrans = gameObject.GetComponent<RectTransform>();
        rectTrans.anchorMin = new Vector2(0, 0);
        rectTrans.anchorMax = new Vector2(1, 1);
        rectTrans.offsetMin = new Vector2(0, 0);
        rectTrans.offsetMax = new Vector2(0, 0);
        rectTrans.localScale = new Vector3(1f, 1f, 1f);
    }

    public void Resume()
    {
        enumerator = Make_(data.time);
        StartCoroutine(enumerator);
    }
}
