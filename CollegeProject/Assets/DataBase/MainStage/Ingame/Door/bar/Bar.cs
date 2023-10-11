using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    public RectTransform rect;
    public float value
    {
        set
        {
            rect.anchorMin = new Vector2(1f - value, rect.anchorMin.y);
            rect.offsetMin = new Vector2(0f, rect.offsetMin.y);
        }
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
