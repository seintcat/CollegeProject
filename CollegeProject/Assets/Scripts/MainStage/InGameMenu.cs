using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Button;
using UnityEngine.UI;
using TMPro;
using System;

// �������� ���� ���� �߻��ϴ� ��Ȳ ���� ������Ʈ
public abstract class InGameMenu : MonoBehaviour
{
    //Ȯ��â �������
    [SerializeField]
    Confirm confirmWindow;

    //���� ���� �̹���
    [SerializeField]
    Image gameoverImage;
    public Sprite gameover
    {
        set
        {
            if(gameoverImage != null) 
                gameoverImage.sprite = value; 
        }
    }

    //�������� ��� �̹���
    [SerializeField]
    Image progressBarImage;
    public Sprite progressBar
    {
        set
        {
            if (progressBarImage != null)
                progressBarImage.sprite = value;
        }
    }

    //�������� ǥ�� �̹���
    [SerializeField]
    Image progressIconImage;
    public Sprite progressIcon
    {
        set
        {
            if (progressIconImage != null)
                progressIconImage.sprite = value;
        }
    }

    //��������
    public float progress
    {
        set
        {
            if (progressIconImage != null)
            {
                RectTransform rect = progressIconImage.gameObject.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(value, 0f);
                rect.anchorMax = new Vector2(value, 0f);
                rect.pivot = new Vector2(0.5f, 0f);
            }
        }
    }

    //�¸� �Ϲݴ��, �й� ��� ����
    [SerializeField]
    TextManager nomalText;

    //�¸� �б��� ��� ����
    [SerializeField]
    TextManager hintText;

    //��� �� ����������ۿ� ���̴� ��ư�� ���
    [SerializeField]
    List<Button> buttons;

    // �ش� �������� �ð� ��� ǥ�ÿ�
    [SerializeField]
    ContextList thisStageTime;

    // ��ü ���� ����ð� ǥ���
    [SerializeField]
    ContextList totalTime;

    //�ʱ�ȭ �Լ�
    public virtual void Init(RectTransform parent = null)
    {
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        if (parent != null)
            rect.SetParent(parent);
        rect.offsetMin = new Vector2(0, 0);
        rect.offsetMax = new Vector2(0, 0);
        rect.localScale = new Vector3(1f, 1f, 1f);
        Exit();
    }

    //�������� �������� �����
    public virtual void Restart()
    {
        Time.timeScale = 1;
        ToolTipManager.Clear();
        EssentialManager.SceneSelect(3);
    }

    //���� ��� ����
    public virtual void Resume()
    {
        Exit();
    }

    //ȯ�漳��
    public virtual void Option()
    {
        ButtonLock(true);
        InGameUIManager.Option();
    }

    //�������� ���� ȭ������ �̵�
    public virtual void GoBack()
    {
        Time.timeScale = 1;
        ToolTipManager.Clear();
        EssentialManager.SceneSelect(2);
    }

    //�¸� ���� ó��
    public virtual void StageClear(ContextList hint = null, int stageSelectDataIndex = -1)
    {
        gameObject.SetActive(true);

        // ���� �ð� ǥ��
        long time = EssentialManager.playTime;
        Save save = Save.GetSave();
        save.playTime = time;
        Save.ApplySave(save);
        string gameTime = thisStageTime.lang[EssentialManager.settings.languageIndex].text;
        gameTime += DateTime.FromBinary(time).ToString("HH.mm.ss.ff");
        gameTime += "\n\n";
        gameTime += totalTime.lang[EssentialManager.settings.languageIndex].text;
        gameTime += DateTime.FromBinary(save.playTime).ToString("HH.mm.ss.ff");

        nomalText.onlyText = gameTime;

        // �б� ǥ��
        if (hint == null)
            hintText.gameObject.SetActive(false);
        else
        {
            hintText.gameObject.SetActive(true);
            hintText.ApplyText(hint);
        }

        // ����� ǥ��
        int indexReal;
        if (stageSelectDataIndex < 0)
            indexReal = save.stageSelectorDataIndex;
        else
            indexReal = stageSelectDataIndex;
        StageSelectData data = InGameUIManager.stageSelectDatas.list[indexReal];

        if (data.progressBackground != null)
        {
            progressBarImage.sprite = data.progressBackground;
            progressBarImage.type = Image.Type.Sliced;
        }
        else
            progressBarImage.color = new Color(0, 0, 0, 0);

        if (data.progressPointer != null)
        {
            progressIconImage.sprite = data.progressPointer;
            progress = data.progress;
        }
        else
            progressIconImage.color = new Color(0, 0, 0, 0);
    }

    //�й� ���� ó��
    public virtual void GameOver(Sprite sprite, ContextList context)
    {
        gameObject.SetActive(true);
        gameover = sprite;
        nomalText.ApplyText(context);
        FilterManager.Off();
        FilterManager.Play(14);
    }

    //��ư�� Ȱ��ȭ / ��Ȱ��ȭ
    public virtual void ButtonLock(bool btnLock)
    {
        if (buttons != null && buttons.Count > 0)
            foreach (Button btn in buttons)
                btn.interactable = !btnLock;
    }

    //ȭ�� ��Ȱ��ȭ
    public virtual void Exit()
    {
        Time.timeScale = 1;
        ButtonLock(false);
        gameObject.SetActive(false);
    }

    // Ȯ��â�� �̺�Ʈ �ٿ� ����
    public virtual void SetDialog(int index)
    {
        ButtonLock(true);
        confirmWindow.SetDialog(index);
    }
}
