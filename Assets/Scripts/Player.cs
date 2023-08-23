using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int treasure = 0;
    int fish = 0;
    bool have_oxygen = false;
    bool have_flippers = false;

    Tooltip tooltip = null;

    public delegate void ItemEventHandler(ItemType type, int count);
    public event ItemEventHandler ItemHandler;

    void Start()
    {
        tooltip = UIGlobals.Get().GetTreasure();
    }

    public void GiveItem(ItemType type, int count)
    {
        switch (type)
        {
            case ItemType.Treasure:
                treasure += count;
                break;
            case ItemType.Fish:
                fish += count;
                break;
            case ItemType.OxygenTank:
                have_oxygen = true;
                break;
            case ItemType.Flippers:
                have_flippers = true;
                break;
        }
        tooltip.SetText(treasure.ToString());

        ItemHandler?.Invoke(type, count);
    }

    public static Player Get()
    {
        return GameObject.Find("/Player").GetComponent<Player>();
    }
}
