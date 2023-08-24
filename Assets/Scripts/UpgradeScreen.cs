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
        if (type == UpgradeType.Echolocation)
        {
            int cost = echolocation.GetCost();
            echolocation.SetCost(cost + 2);
        }
        else if (type == UpgradeType.Oxygen)
        {
            PlayerMovement player = PlayerMovement.Get();
            player.UpgradeOxygen();
            oxygen.SetCost(player.GetNextOxygenUpgradeCost());
        }
        else if (type == UpgradeType.Speed)
        {
            PlayerMovement player = PlayerMovement.Get();
            player.UpgradeSpeed();
            speed.SetCost(player.GetNextSpeedUpgradeCost());
        }
    }
}

