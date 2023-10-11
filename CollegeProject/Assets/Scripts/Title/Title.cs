using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

// Ÿ��Ʋ ȭ���� �����ϴ� �߻� Ŭ����
public abstract class Title : MonoBehaviour
{
    //��Ƽ��, �� ����, �ɼ�, ������, ���� ��ư ������ �����
    [SerializeField]
    List<Button> buttons;

    //Ȯ��â �������
    [SerializeField]
    Confirm confirmWindow;

    // ��ư���� �����ϴ� ��ġ�� ����ϴ� ������Ʈ
    [SerializeField]
    VerticalLayoutGroup buttonField;

    // �ʱ�ȭ �ڷ�ƾ
    static IEnumerator init;

    //������ �̾��Ѵ�
    public virtual void Continue()
    {
        EssentialManager.SceneSelect(2);
    }

    //�� ����
    public virtual void NewGame()
    {
        Save.NewSave();
        EssentialManager.SceneSelect(2);
    }

    //�ַ��� ȭ��
    public virtual void Gallery()
    {
        EssentialManager.SceneSelect(4);
    }

    //ȯ�漳��
    public virtual void Option()
    {
        TitleManager.Open(2);
        ButtonLock(true);
    }

    //���� ����
    public virtual void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ���ø����̼� ����
#endif
    }

    //��ư�� Ȱ��ȭ / ��Ȱ��ȭ
    public virtual void ButtonLock(bool value)
    {
        foreach (Button btn in buttons)
            btn.interactable = !value;
    }

    // Ȯ��â üũ
    public virtual void CheckDialog(int index)
    {
        ButtonLock(true);

        switch (index)
        {
            case 0:
                if (!Save.exist)
                    NewGame();
                else
                    SetDialog(index);
                break;
            case 1:
                SetDialog(index);
                break;
            default:
                break;
        }
    }

    // Ȯ��â�� �̺�Ʈ �ٿ� ����
    public virtual void SetDialog(int index)
    {
        ButtonLock(true);

        confirmWindow.GetComponent<Confirm>().SetDialog(index);
    }

    // Ÿ��Ʋ �ʱ�ȭ
    public virtual void Init(RectTransform parent = null)
    {
        RectTransform rectTrans = gameObject.GetComponent<RectTransform>();
        if (parent != null)
            rectTrans.SetParent(parent);
        rectTrans.offsetMin = new Vector2(0, 0);
        rectTrans.offsetMax = new Vector2(0, 0);
        rectTrans.localScale = new Vector3(1f, 1f, 1f);

        // UI�� ��ưũ�� ����
        buttonField.childControlHeight = true;
        buttonField.childForceExpandHeight = true;

        init = Init_();
        StartCoroutine(init);
    }
    public virtual IEnumerator Init_()
    {
        yield return new WaitForEndOfFrame();

        buttonField.childControlHeight = false;

        if (!Save.exist)
            buttons[0].gameObject.SetActive(false);

        StopCoroutine(init);
    }

}
