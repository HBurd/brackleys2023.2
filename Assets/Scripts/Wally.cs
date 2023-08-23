using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wally : MonoBehaviour
{
    Talkable talkable;
    UpgradeScreen upgrades;

    // Start is called before the first frame update
    void Start()
    {
        talkable = GetComponent<Talkable>();
        upgrades = UIGlobals.Get().GetUpgradeScreen();

        Player player = Player.Get();
        player.ItemEvent += PlayerItemHandler;
        talkable.StateChangeEvent += StateChangeHandler;
    }

    void PlayerItemHandler(ItemType item, int count)
    {
        if (item == ItemType.OxygenTank)
        {
            talkable.SayAfter("find_oxygen", "intro", 1);
            talkable.SayAfter("upgrades", "find_oxygen", 0);
            upgrades.EnableOxygenUpgrades();
        }
        else if (item == ItemType.Flippers)
        {
            talkable.SayAfter("find_flippers", "intro", 1);
            talkable.SayAfter("upgrades", "find_flippers", 0);
            upgrades.EnableSpeedUpgrades();
        }
    }

    void StateChangeHandler(string new_state)
    {
        if (new_state == "upgrades")
        {
            upgrades.gameObject.SetActive(true);
        }
        else
        {
            upgrades.gameObject.SetActive(false);
        }
    }
}
