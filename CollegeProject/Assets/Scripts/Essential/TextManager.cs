using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// 지문을 재생시키는 컴포넌트
public class TextManager : MonoBehaviour
{
    // 마지막으로 참조한 인덱스
    int lastIndex = 0;

    // 지문 데이터
    [SerializeField]
    List<ContextList> contexts;

    //텍스트
    [SerializeField]
    List<TMP_Text> text;

    // 텍스트 내용만 바꾸는 접근자
    public string onlyText
    {
        set
        {
            if(text != null && text.Count > 0)
            {
                ApplyStyle(lastIndex);
                foreach (TMP_Text _text in text)
                {
                    _text.text = value;
                    _text.ForceMeshUpdate();
                }
            }
        }
    }

    // 텍스트 컴퍼넌트에 현재 언어의 텍스트와 스타일 적용
    public void ApplyText(int index = -1)
    {
        CheckIndex(index);
        if (lastIndex < 0)
            return;

        ApplyStyle(lastIndex);
        if (text != null && text.Count > 0)
            foreach (TMP_Text _text in text)
            {
                _text.text = GetContext(lastIndex).text;
                _text.ForceMeshUpdate();
            }
    }
    public void ApplyText(ContextList context)
    {
        contexts = new List<ContextList>();
        contexts.Add(context);
        ApplyText(0);
    }

    // 텍스트 값만 현재 언어로 가져오기
    public Context GetContext(int index = -1)
    {
        CheckIndex(index);
        if (lastIndex < 0)
            return null;

        if (EssentialManager.settings.languageIndex < contexts[lastIndex].lang.Count)
            return contexts[lastIndex].lang[EssentialManager.settings.languageIndex];

        return contexts[lastIndex].lang[0];
    }

    // 텍스트 컴퍼넌트에 현재 언어 스타일만 적용
    public void ApplyStyle(int index = -1)
    {
        CheckIndex(index);
        if (lastIndex < 0)
            return;

        TMP_FontAsset font;
        float fontSize;
        if (EssentialManager.settings.languageIndex < contexts[lastIndex].lang.Count)
        {
            font = EssentialManager.languageData._fonts[contexts[lastIndex].lang[EssentialManager.settings.languageIndex].fontIndex].fontAsset;
            fontSize = contexts[lastIndex].lang[EssentialManager.settings.languageIndex].fontSize;
        }
        else
        {
            font = EssentialManager.languageData._fonts[contexts[lastIndex].lang[0].fontIndex].fontAsset;
            fontSize = contexts[lastIndex].lang[0].fontSize;
        }
        
        if (text.Count > 0)
            foreach (TMP_Text _text in text)
            {
                _text.font = font;
                _text.fontSize = fontSize;
                _text.ForceMeshUpdate();
            }
    }

    void CheckIndex(int index)
    {
        if (index >= 0)
            lastIndex = index;

        if (lastIndex >= contexts.Count)
            lastIndex = 0;
    }

    // 초기화 메서드
    public void Init()
    {
        if (text.Count > 0)
            foreach (TMP_Text _text in text)
                _text.styleSheet = EssentialManager.textStyleSheet;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if (text.Count == 1 && contexts.Count == 1)
            ApplyText(0);
    }
}
