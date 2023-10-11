using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTilte2 : Title
{
    public TextManager yesBTN, noBTN;

    public override void Init(RectTransform parent)
    {
        base.Init(parent);
        yesBTN.ApplyText(0);
        noBTN.ApplyText(0);
    }
}
