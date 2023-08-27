using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    int treasure = 0;
    int fish = 0;

    InventoryCounter treasure_display = null;
    InventoryCounter fish_display = null;

    Ship ship;

    UIGlobals ui;

    [SerializeField]
    TMP_Text treasure_counter;


    int deposited_treasure = 0;

    [SerializeField]
    int total_treasure = 20;


    public delegate void ItemEventHandler(ItemType type, int count);
    public event ItemEventHandler ItemEvent;

    public delegate void DieEventHandler();
    public event DieEventHandler DieEvent;

    public delegate void MaxTreasureEventHandler();
    public event MaxTreasureEventHandler MaxTreasureEvent;

    void Start()
    {
        ui = UIGlobals.Get();
        treasure_display = ui.GetTreasure();
        fish_display = ui.GetFish();

        ship = GameObject.Find("/Boat").GetComponent<Ship>();

        // setting negative just hides fully
        ui.SetOxygenUpgradeLevel(-1);
        ui.SetSpeedUpgradeLevel(-1);

        treasure_counter.text = "0 / " + total_treasure;
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
                ui.SetOxygenUpgradeLevel(0);
                break;
            case ItemType.Flippers:
                ui.SetSpeedUpgradeLevel(0);
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
            deposited_treasure += treasure;
            ship.SpawnFish(10 * treasure);
            GiveItem(ItemType.Treasure, -treasure);
            treasure_counter.text = deposited_treasure.ToString() + " / " + total_treasure;

            if (deposited_treasure >= total_treasure)
            {
                MaxTreasureEvent?.Invoke();
            }
        }
    }

    public int GetFish()
    {
        return fish;
    }
}
