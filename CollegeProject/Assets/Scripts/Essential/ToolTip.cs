using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �� ǥ�� �� ���丮 ����� ������Ʈ
public class ToolTip : MonoBehaviour
{
    //���� �̹��� ������Ʈ
    [SerializeField]
    List<Image> images;

    //�ؽ�Ʈ
    [SerializeField]
    TextManager text;

    //�̹��� ���� ������
    [SerializeField]
    List<AspectRatioFitter> fitters;

    //���� ǥ�� ����
    ToolTipData data;

    // �ڵ����� �ڷ�ƾ �����
    IEnumerator enumerator;

    //���� ���
    public void Make(ToolTipData _data)
    {
        gameObject.SetActive(true);
        Color color;

        // �ʱ�ȭ
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

        // ���� ����
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

        // �̹��� �ҽ� ����
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

    //��� ����, �̾����� ���� �������
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

    // �ʱ�ȭ �޼���
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
