using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 스테이지 선택 창
public class StageSelector : MonoBehaviour
{
    //각 지문 제어용 텍스트매니저
    [SerializeField]
    TextManager storyText;
    [SerializeField]
    TextManager gameplayText;

    //진행상황 표시지점
    [SerializeField]
    GameObject progressPointer;

    //스테이지 선택창 메인 이미지
    [SerializeField]
    Image mainImage;

    //규칙들 이미지
    [SerializeField]
    Image rule1, rule2, rule3;

    //진행창 배경 이미지
    [SerializeField]
    Image progressBackground;

    //초기화 함수
    public void Init()
    {
        StageSelectData data = StageSelectManager.data.list[Save.GetSave().stageSelectorDataIndex];

        // 텍스트 적용
        storyText.ApplyText(data.storyText);
        gameplayText.ApplyText(data.tipText);

        // 포인터 적용
        if (data.progressPointer != null)
        {
            progressPointer.GetComponentInChildren<Image>().sprite = data.progressPointer;
            
            RectTransform progressRect = progressPointer.GetComponent<RectTransform>();
            progressRect.anchorMin = new Vector2(data.progress, 0f);
            progressRect.anchorMax = new Vector2(data.progress, 1f);
            progressRect.pivot = new Vector2(0.5f, 0f);
        }
        else
            progressPointer.GetComponent<Image>().color = new Color(0, 0, 0, 0);

        // 기타 이미지 적용
        if (data.mainImage != null)
            mainImage.sprite = data.mainImage;
        else
            mainImage.color = new Color(0, 0, 0, 0);
        if (data.rule1 != null)
            rule1.sprite = data.rule1;
        else
            rule1.color = new Color(0, 0, 0, 0);
        if (data.rule2 != null)
            rule2.sprite = data.rule2;
        else
            rule2.color = new Color(0, 0, 0, 0);
        if (data.rule3 != null)
            rule3.sprite = data.rule3;
        else
            rule3.color = new Color(0, 0, 0, 0);
        if (data.progressBackground != null)
        {
            progressBackground.sprite = data.progressBackground;
            progressBackground.type = Image.Type.Sliced;
        }
        else
            progressBackground.color = new Color(0, 0, 0, 0);
    }

    // 타이틀 화면으로 돌아감
    public void BackToMainTitle()
    {
        EssentialManager.SceneSelect(1);
    }

    // 스테이지를 시작하는 메서드
    public void GameStart()
    {
        EssentialManager.SceneSelect(3);
    }
}
