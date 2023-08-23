using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wally : MonoBehaviour
{
    Talkable talkable;


    // Start is called before the first frame update
    void Start()
    {
        talkable = GetComponent<Talkable>();

        Player player = Player.Get();
        player.ItemHandler += PlayerItemHandler;
    }

    void PlayerItemHandler(ItemType item, int count)
    {
        if (item == ItemType.OxygenTank)
        {
            talkable.SayAfter("find_oxygen", "intro");
        }
        else if (item == ItemType.Flippers)
        {
            talkable.SayAfter("find_flippers", "intro");
        }
    }
}
