using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wally : MonoBehaviour
{
    Talkable talkable;
    UpgradeScreen upgrades;

    bool player_found_oxygen = false;
    bool player_found_flippers = false;
    bool new_upgrades = false;
    bool said_intro = false;

    // Start is called before the first frame update
    void Start()
    {
        talkable = GetComponent<Talkable>();
        upgrades = UIGlobals.Get().GetUpgradeScreen();

        Player player = Player.Get();
        player.ItemEvent += PlayerItemHandler;
        talkable.StateChangeEvent += StateChangeHandler;
        talkable.OpenEvent += OpenEventHandler;
    }

    void PlayerItemHandler(ItemType item, int count)
    {
        if (item == ItemType.OxygenTank)
        {
            //talkable.SayAfter("find_thing", "intro", 1);
            //talkable.SayAfter("upgrades", "find_oxygen", 0);
            new_upgrades = true;
            player_found_oxygen = true;
            upgrades.EnableOxygenUpgrades();
        }
        else if (item == ItemType.Flippers)
        {
            //talkable.SayAfter("find_thing", "intro", 1);
            //talkable.SayAfter("upgrades", "find_flippers", 0);
            new_upgrades = true;
            player_found_flippers = true;
            upgrades.EnableSpeedUpgrades();
        }
    }

    void StateChangeHandler(string new_state)
    {
        if (new_state == "upgrades" || new_state == "find_more_items")
        {
            upgrades.gameObject.SetActive(true);
        }
        else
        {
            upgrades.gameObject.SetActive(false);

            if (new_state == "found_item")
            {
                talkable.SayAfter("upgrades", "intro", 1);
            }
            else if (new_state == "found_all_items")
            {
                talkable.SayAfter("upgrades", "intro", 1);
            }
        }
    }

    void OpenEventHandler(string last_state)
    {
        Debug.Log("Open");
        if (!said_intro)
        {
            said_intro = true;
            return;
        }


        if (new_upgrades)
        {
            new_upgrades = false;
            talkable.SayAfter("found_item", "intro", 1);
        }
        else if (player_found_oxygen && player_found_flippers)
        {
            talkable.SayAfter("found_all_items", "intro", 1);
        }
        else
        {
            talkable.SayAfter("find_more_items", "intro", 1);
        }
    }
}
