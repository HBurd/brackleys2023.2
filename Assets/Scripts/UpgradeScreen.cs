using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeScreen : MonoBehaviour
{
    [SerializeField]
    GameObject upgrade_field;

    Upgrade echolocation;
    Upgrade oxygen;
    Upgrade speed;

    // Start is called before the first frame update
    void Start()
    {
        echolocation = Instantiate(upgrade_field, transform.GetChild(0)).GetComponent<Upgrade>();
        echolocation.SetType(UpgradeType.Echolocation);
        echolocation.SetCost(Player.Get().GetComponent<Echolocation>().GetNextUpgradeCost());
    }

    public void EnableOxygenUpgrades()
    {
        if (oxygen != null)
        {
            return;
        }

        oxygen = Instantiate(upgrade_field, transform.GetChild(0)).GetComponent<Upgrade>();
        oxygen.SetType(UpgradeType.Oxygen);

        PlayerMovement player = PlayerMovement.Get();
        oxygen.SetCost(player.GetNextOxygenUpgradeCost());
    }

    public void EnableSpeedUpgrades()
    {
        if (speed != null)
        {
            return;
        }

        speed = Instantiate(upgrade_field, transform.GetChild(0)).GetComponent<Upgrade>();
        speed.SetType(UpgradeType.Speed);

        PlayerMovement player = PlayerMovement.Get();
        speed.SetCost(player.GetNextSpeedUpgradeCost());
    }

    public void HandleUpgrade(UpgradeType type)
    {
        Player player = Player.Get();
        PlayerMovement player_movement = player.GetComponent<PlayerMovement>();
        if (type == UpgradeType.Echolocation)
        {
            Echolocation echo = player.GetComponent<Echolocation>();
            int cost = echo.GetNextUpgradeCost();
            if (player.GetFish() >= cost)
            {
                player.GiveItem(ItemType.Fish, -cost);
                echo.Upgrade();
                echolocation.SetCost(echo.GetNextUpgradeCost());
            }
        }
        else if (type == UpgradeType.Oxygen)
        {
            int cost = player_movement.GetNextOxygenUpgradeCost();
            if (player.GetFish() >= cost)
            {
                player.GiveItem(ItemType.Fish, -cost);
                player_movement.UpgradeOxygen();
                oxygen.SetCost(player_movement.GetNextOxygenUpgradeCost());
            }
        }
        else if (type == UpgradeType.Speed)
        {
            int cost = player_movement.GetNextSpeedUpgradeCost();
            if (player.GetFish() >= cost)
            {
                player.GiveItem(ItemType.Fish, -cost);
                player_movement.UpgradeSpeed();
                speed.SetCost(player_movement.GetNextSpeedUpgradeCost());
            }
        }
    }
}

