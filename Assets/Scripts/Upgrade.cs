using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Upgrade : MonoBehaviour
{
    [SerializeField]
    TMP_Text text;

    public void SetText(string new_text)
    {
        text.text = new_text;
    }
}
