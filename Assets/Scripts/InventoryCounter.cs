using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryCounter : MonoBehaviour
{
    [SerializeField]
    TMP_Text text;

    [SerializeField]
    bool hide_at_zero;

    // Start is called before the first frame update
    void Start()
    {
        //SetValue(0);
    }

    public void SetValue(int value)
    {
        text.text = value.ToString();

        if (value == 0 && hide_at_zero)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
