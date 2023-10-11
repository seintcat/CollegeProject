using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// ������ �����Ű�� ������Ʈ
public class TextManager : MonoBehaviour
{
    // ���������� ������ �ε���
    int lastIndex = 0;

    // ���� ������
    [SerializeField]
    List<ContextList> contexts;

    //�ؽ�Ʈ
    [SerializeField]
    List<TMP_Text> text;

    // �ؽ�Ʈ ���븸 �ٲٴ� ������
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

    // �ؽ�Ʈ ���۳�Ʈ�� ���� ����� �ؽ�Ʈ�� ��Ÿ�� ����
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

    // �ؽ�Ʈ ���� ���� ���� ��������
    public Context GetContext(int index = -1)
    {
        CheckIndex(index);
        if (lastIndex < 0)
            return null;

        if (EssentialManager.settings.languageIndex < contexts[lastIndex].lang.Count)
            return contexts[lastIndex].lang[EssentialManager.settings.languageIndex];

        return contexts[lastIndex].lang[0];
    }

    // �ؽ�Ʈ ���۳�Ʈ�� ���� ��� ��Ÿ�ϸ� ����
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

    // �ʱ�ȭ �޼���
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
