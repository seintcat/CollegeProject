using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestLoading : LoadingScreen
{
    // �ִϸ��̼� �ڷ�ƾ
    IEnumerator _animation;

    // UI�� �������� �̹��� ������Ʈ
    public Image screen;

    // �̹��� ����Ʈ
    [SerializeField]
    List<Sprite> sprites;

    private void Awake()
    {
        screen.type = Image.Type.Simple;
    }
    private void OnEnable()
    {
        _animation = Animation();
        StartCoroutine(_animation);
    }
    private void OnDisable()
    {
        StopCoroutine(_animation);
    }

    // �ִϸ��̼��� ����ϱ� ���� �ڷ�ƾ
    IEnumerator Animation()
    {
        while (true)
        {
            if (sprites.Count < 1)
            {
                Debug.Log("sprites.Length error");
                break;
            }

            for (int i = 0; i < sprites.Count; i++)
            {
                screen.sprite = sprites[i];
                // time/speed��ŭ ��ٸ���
                yield return new WaitForSeconds(ScreenSettings.animationSpeed * 2);
            }
        }
    }
}
