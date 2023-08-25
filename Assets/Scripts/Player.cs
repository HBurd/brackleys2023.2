using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int treasure = 0;
    int fish = 0;
    bool have_oxygen = false;
    bool have_flippers = false;

    InventoryCounter treasure_display = null;
    InventoryCounter fish_display = null;


    public delegate void ItemEventHandler(ItemType type, int count);
    public event ItemEventHandler ItemEvent;

    public delegate void DieEventHandler();
    public event DieEventHandler DieEvent;

    void Start()
    {
        UIGlobals ui = UIGlobals.Get();
        treasure_display = ui.GetTreasure();
        fish_display = ui.GetFish();
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
        treasure_display.SetValue(treasure);
        fish_display.SetValue(fish);


        ItemEvent?.Invoke(type, count);
    }

    public void Die()
    {
        DieEvent?.Invoke();
    }

    public static Player Get()
    {
        return GameObject.Find("/Player").GetComponent<Player>();
    }

    public void Upgrade(UpgradeType type)
    {
        Debug.Log("Upgrade" + type.ToString());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Barrel")
        {
            GiveItem(ItemType.Treasure, -treasure);
        }
    }

    public int GetFish()
    {
        return fish;
    }
}
