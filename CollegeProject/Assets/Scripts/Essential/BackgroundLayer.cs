using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 배경을 보여주는 컴포넌트
public class BackgroundLayer : MonoBehaviour
{
    // LayerFilter가 사용하는 필터 정보
    FilterSet filter;

    // UI에 보여지는 이미지 컴포넌트
    public Image screen;

    // 배경 이미지 모음
    Sprite[] sprites;

    // 사운드매니저에서 할당받는 소리 인덱스
    int soundIndex;

    // 저장된 인덱스 값
    int index;

    // 자기 자신 컴포넌트 전역 설정값
    static BackgroundLayer layer;

    // 애니메이션 코루틴
    static IEnumerator backgroundAnimation;

    // 캔버스 크기 조절
    [SerializeField]
    CanvasScaler canvas;

    // 이미지 사이즈 조절 컴포넌트
    [SerializeField]
    ContentSizeFitter fitter;
    [SerializeField]
    RectTransform imgTransform;

    // 마지막으로 맞춘 캔버스 크기를 저장
    static Vector2 canvasSize = new Vector2(100, 100);
    // 마지막으로 맞춘 캔버스 적용비율 저장
    static float matchWidthOrHeight = 0f;

    // 초기화 메서드
    public void Init()
    {
        // 중복검사
        var objs = FindObjectsOfType<BackgroundLayer>();
        if (objs.Length != 1)
        {
            Debug.Log("error: you can use only one BackgroundLayer");
            Destroy(gameObject);
            return;
        }

        // 전역변수에 인스턴스 대입.
        layer = this;
        Off_();
    }

    //해당 인덱스 배경 재생 
    //만약 해당배경이 없다면 오류 발생
    public static void Play(int index = -1)
    {
        layer.Play_(index);
    }
    public void Play_(int _index)
    {
        if (_index < 0) // || !FilterManager.GetFilter(index).isLoop)
            Off_();
        else
        {
            Off_();
            gameObject.SetActive(true);
            screen.color = new Color(255, 255, 255);
            index = _index;

            // filterlist에서 해당 index의 값을 가져와서 이미지 사운드 적용.
            filter = FilterManager.GetFilter(index);
            sprites = filter.backgroundImage.ToArray();

            // 필터의 이미지 반복배치 / 화면맞춤 여부
            if (filter.isTiled)
            {
                screen.type = Image.Type.Tiled;
                canvasSize = EssentialManager.display.ratio[SettingsToJson.GetSettings().ratioIndex].pixel;
                matchWidthOrHeight = 0f;

                ApplyCanvasSize_();
            }
            else
            {
                screen.type = Image.Type.Simple;
                screen.sprite = sprites[0];

                imgTransform.anchorMin = new Vector2(0.5f, 0.5f);
                imgTransform.anchorMax = new Vector2(0.5f, 0.5f);

                fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

                Vector2Int canvasRatio = EssentialManager.display.ratio[SettingsToJson.GetSettings().ratioIndex].value;
                Vector2 imgSize = imgTransform.sizeDelta;
                float canvasRatioFloat = (float)canvasRatio.x / canvasRatio.y;
                float imgRatioFloat = imgSize.x / imgSize.y;
                if (canvasRatioFloat <= imgRatioFloat)
                {
                    // 배경이미지의 가로가 더 큼
                    matchWidthOrHeight = 1f;
                    canvasSize.y = imgSize.y;
                    canvasSize.x = (imgSize.y / canvasRatio.y) * canvasRatio.x;
                }
                else
                {
                    // 배경이미지의 세로가 더 큼
                    matchWidthOrHeight = 0f;
                    canvasSize.x = imgSize.x;
                    canvasSize.y = (imgSize.x / canvasRatio.x) * canvasRatio.y;
                }

                ApplyCanvasSize_();
            }

            // 오디오매니저에 소리 재생기능 위임
            soundIndex = AudioManager.Play(filter.sound, filter.isLoop, filter.volume);

            backgroundAnimation = BackgroundAnimation();
            StartCoroutine(backgroundAnimation);
        }
    }

    // 애니메이션을 재생하기 위한 코루틴
    IEnumerator BackgroundAnimation()
    {
        do
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                screen.sprite = sprites[i];
                // time/speed만큼 기다리다
                yield return new WaitForSeconds(ScreenSettings.animationSpeed / filter.speed); 
            }
        } while (filter.isLoop);
    }

    // 자신 레이어 비활성화
    public static void Off()
    {
        if (layer == null)
            return;

        layer.Off_();
    }
    public void Off_()
    {
        if(gameObject.activeSelf)
            AudioManager.Off(soundIndex);

        if(backgroundAnimation != null)
            StopCoroutine(backgroundAnimation);

        if (Map.mapData == null)
            screen.color = new Color(0, 0, 0);
        else
            screen.color = Map.mapData.backgroundColor;

        // 이미지 초기화
        screen.sprite = null;
        fitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        imgTransform.offsetMin = new Vector2(0, 0);
        imgTransform.offsetMax = new Vector2(0, 0);
        imgTransform.localScale = new Vector3(1f, 1f, 1f);
    }

    // 배경이미지 크기에 따른 화면비 조정
    public static void ApplyCanvasSize()
    {
        if (layer == null)
            return;

        layer.ApplyCanvasSize_();
    }
    public void ApplyCanvasSize_()
    {
        canvas.matchWidthOrHeight = matchWidthOrHeight;
        canvas.referenceResolution = canvasSize;
    }

    private void Awake()
    {
        Init();
    }
}
