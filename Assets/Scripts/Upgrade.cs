using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    [SerializeField]
    TMP_Text text;

    [SerializeField]
    TMP_Text cost_text;

    [SerializeField]
    UpgradeType type;

    int cost = 1;

    void Start()
    {
        text.text = type.ToString();
        cost_text.text = cost.ToString();
    }

    public void SetType(UpgradeType new_type)
    {
        type = new_type;
    }

    public void HandleUpgrade()
    {
        UIGlobals.Get().GetUpgradeScreen().HandleUpgrade(type);
    }

    public int GetCost()
    {
        return cost;
    }

    public void SetCost(int new_cost)
    {
        if (new_cost < 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            cost = new_cost;
            cost_text.text = cost.ToString();
        }
    }
}

public enum UpgradeType
{
    Echolocation,
    Oxygen,
    Speed
}