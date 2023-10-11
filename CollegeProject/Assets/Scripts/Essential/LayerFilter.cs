using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 하나의 필터를 보여주는 컴포넌트
// 해당 레이어는 생성시 게임 화면 최상단에 표시됨
public class LayerFilter : MonoBehaviour
{
    // LayerFilter가 사용하는 필터 정보
    FilterSet filter;

    // UI에 보여지는 이미지 컴포넌트
    public Image screen;

    // 배경 이미지 모음
    Sprite[] sprites;

    // 사운드매니저에서 할당받는 소리 인덱스
    int soundIndex;

    // 자신 오브젝트의 트랜스폼 값
    public RectTransform rectTrans;

    // 저장된 인덱스 값
    int index;

    // 애니메이션 코루틴
    IEnumerator _animation;

    // 이 필터가 루프인지를 반환
    public bool isLoop
    {
        get { return filter.isLoop; }
    }

    // 현재 필터가 입력받은 인덱스랑 같은 종류인지 반환
    public bool CheckIndex(int _index)
    {
        return index == _index;
    }

    // 해당 인덱스 배경 재생 
    // 만약 해당 인덱스의 배경이 없다면 배경 기본컬러 표시
    public void Play(int _index)
    {
        gameObject.SetActive(true);
        index = _index;

        // screen에 출력되는 이미지 컴포넌트의 RectTransform을 strech로 지정하기
        rectTrans.offsetMin = new Vector2(0, 0);
        rectTrans.offsetMax = new Vector2(0, 0);

        // filterlist에서 해당 index의 값을 가져와서 이미지 사운드 적용.
        filter = FilterManager.GetFilter(index);
        sprites = filter.backgroundImage.ToArray();

        // 필터의 이미지 반복배치 / 화면맞춤 여부
        if (filter.isTiled)
            screen.type = Image.Type.Tiled;
        else
            screen.type = Image.Type.Simple;

        // 오디오매니저에 소리 재생기능 위임
        soundIndex = AudioManager.Play(filter.sound, filter.isLoop, filter.volume);

        screen.enabled = true;
        _animation = Animation();
        StartCoroutine(_animation);
    }

    // 애니메이션을 재생하기 위한 코루틴
    IEnumerator Animation()
    {
        while (true)
        {
            yield return null;
            do
            {
                if(sprites.Length < 1)
                {
                    Debug.Log("sprites.Length error");
                    break;
                }

                for (int i = 0; i < sprites.Length; i++)
                {
                    screen.sprite = sprites[i];
                    // time/speed만큼 기다리다
                    yield return new WaitForSeconds(ScreenSettings.animationSpeed / filter.speed);
                }
            } while (filter.isLoop);
            Off();
        }
    }

    // 자신 레이어 비활성화
    public void Off()
    {
        AudioManager.Off(soundIndex);
        StopCoroutine(_animation);
        gameObject.SetActive(false);
        if (!isLoop)
            FilterManager.CheckOff();
    }
}
