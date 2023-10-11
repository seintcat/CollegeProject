using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KnightUI : MonoBehaviour
{
    [SerializeField]
    Image imgM, hpBar;
    [SerializeField]
    GameObject objP, objB, objM, objC;
    [SerializeField]
    TextMeshProUGUI text;
    [SerializeField]
    Sprite one, two;
    [HideInInspector]
    public float maxHp = 1;

    public int potion
    {
        set
        {
            if(value > 0)
            {
                objP.SetActive(true);
                text.text = value.ToString();
            }
            else
                objP.SetActive(false);
        }
    }

    public bool bone
    {
        set { objB.SetActive(value); }
    }

    public int minotaur
    {
        set
        {
            objM.SetActive(true);

            if (value == 1)
                imgM.sprite = one;
            else if (value == 2)
                imgM.sprite = two;
            else
                objM.SetActive(false);
        }
    }

    public float hp
    {
        set
        {
            float ratio = value / maxHp;
            if (ratio > 1) ratio = 1;
            else if (ratio < 0) ratio = 0;

            hpBar.fillAmount = ratio;
        }
    }

    public bool cannonBall
    {
        set { objC.SetActive(value); }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
