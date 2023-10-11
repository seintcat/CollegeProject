using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestLoading : LoadingScreen
{
    // 애니메이션 코루틴
    IEnumerator _animation;

    // UI에 보여지는 이미지 컴포넌트
    public Image screen;

    // 이미지 리스트
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

    // 애니메이션을 재생하기 위한 코루틴
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
                // time/speed만큼 기다리다
                yield return new WaitForSeconds(ScreenSettings.animationSpeed * 2);
            }
        }
    }
}
