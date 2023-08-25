using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    UnityEngine.UI.Image bar;

    virtual protected void Start()
    {
        bar = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
    }

    public virtual void SetValue(float value)
    {
        bar.fillAmount = value;
    }
}
