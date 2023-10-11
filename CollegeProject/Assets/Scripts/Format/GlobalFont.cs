using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// �� ������ �Ǵ� ��Ʈ Ŭ����
[System.Serializable]
public class GlobalFont
{
    // ��Ʈ ������
    [SerializeField]
    TMP_FontAsset _fontAsset;
    public TMP_FontAsset fontAsset
    {
        get { return _fontAsset; }
    }

    // ��Ʈ�� �����ϴ� ��� ���
    [SerializeField]
    List<bool> _usableLanguage;
    public List<bool> usableLanguage
    {
        get { return _usableLanguage; }
    }
}
