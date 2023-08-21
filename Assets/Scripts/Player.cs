using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int treasure = 0;

    Tooltip tooltip = null;

    void Start()
    {
        tooltip = GameObject.Find("UI/TreasureDisplay").GetComponent<Tooltip>();
        GiveTreasure(0);
    }

    public void GiveTreasure(int value)
    {
        treasure += value;
        tooltip.SetText(treasure.ToString());
    }
}
