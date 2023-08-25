using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar2 : ProgressBar
{
    UnityEngine.UI.Image bar_2;

    override protected void Start()
    {
        base.Start();
        bar_2 = transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Image>();
    }
    public override void SetValue(float value)
    {
        base.SetValue(value);

        if (value > 1.0f)
        {
            value -= 1.0f;
            bar_2.fillAmount = value;
        }
        else
        {
            bar_2.fillAmount = 0.0f;
        }
    }
}
