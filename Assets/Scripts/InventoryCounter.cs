using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryCounter : MonoBehaviour
{
    [SerializeField]
    TMP_Text text;    

    // Start is called before the first frame update
    void Start()
    {
        SetValue(0);
    }

    public void SetValue(int value)
    {
        text.text = value.ToString();
        gameObject.SetActive(value != 0);
    }
}
