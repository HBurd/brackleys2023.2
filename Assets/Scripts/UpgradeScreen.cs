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
        echolocation.SetText("Echolocation");
    }

    public void EnableOxygenUpgrades()
    {
        if (oxygen != null)
        {
            return;
        }

        oxygen = Instantiate(upgrade_field, transform.GetChild(0)).GetComponent<Upgrade>();
        oxygen.SetText("Oxygen");
    }

    public void EnableSpeedUpgrades()
    {
        if (speed != null)
        {
            return;
        }

        speed = Instantiate(upgrade_field, transform.GetChild(0)).GetComponent<Upgrade>();
        speed.SetText("Speed");
    }
}
