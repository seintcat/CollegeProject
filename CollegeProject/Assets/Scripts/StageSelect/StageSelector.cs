using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �������� ���� â
public class StageSelector : MonoBehaviour
{
    //�� ���� ����� �ؽ�Ʈ�Ŵ���
    [SerializeField]
    TextManager storyText;
    [SerializeField]
    TextManager gameplayText;

    //�����Ȳ ǥ������
    [SerializeField]
    GameObject progressPointer;

    //�������� ����â ���� �̹���
    [SerializeField]
    Image mainImage;

    //��Ģ�� �̹���
    [SerializeField]
    Image rule1, rule2, rule3;

    //����â ��� �̹���
    [SerializeField]
    Image progressBackground;

    //�ʱ�ȭ �Լ�
    public void Init()
    {
        StageSelectData data = StageSelectManager.data.list[Save.GetSave().stageSelectorDataIndex];

        // �ؽ�Ʈ ����
        storyText.ApplyText(data.storyText);
        gameplayText.ApplyText(data.tipText);

        // ������ ����
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

        // ��Ÿ �̹��� ����
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

    // Ÿ��Ʋ ȭ������ ���ư�
    public void BackToMainTitle()
    {
        EssentialManager.SceneSelect(1);
    }

    // ���������� �����ϴ� �޼���
    public void GameStart()
    {
        EssentialManager.SceneSelect(3);
    }
}
