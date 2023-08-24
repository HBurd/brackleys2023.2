using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIGlobals : MonoBehaviour
{
    [SerializeField]
    private Tooltip tooltip;

    [SerializeField]
    private DialogueSystem dialogue;

    [SerializeField]
    private InventoryCounter treasure;

    [SerializeField]
    private InventoryCounter fish;

    [SerializeField]
    private UpgradeScreen upgrades;

    [SerializeField]
    private ProgressBar oxygen;

    [SerializeField]
    private ProgressBar boost;

    [SerializeField]
    private UnityEngine.UI.Image fade;

    public Tooltip GetTooltip()
    {
        return tooltip;
    }

    public DialogueSystem GetDialogue()
    {
        return dialogue;
    }

    public InventoryCounter GetTreasure()
    {
        return treasure;
    }

    public InventoryCounter GetFish()
    {
        return fish;
    }

    public UpgradeScreen GetUpgradeScreen()
    {
        return upgrades;
    }

    public ProgressBar GetOxygenBar()
    {
        return oxygen;
    }

    public ProgressBar GetBoostBar()
    {
        return boost;
    }


    public static UIGlobals Get()
    {
        return GameObject.Find("/UI").GetComponent<UIGlobals>();
    }

    public void SetFade(float value)
    {
        fade.color = new Color(0.0f, 0.0f, 0.0f, value);
    }
}
