using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGlobals : MonoBehaviour
{
    [SerializeField]
    private Tooltip tooltip;

    [SerializeField]
    private DialogueSystem dialogue;

    [SerializeField]
    private Tooltip treasure;

    [SerializeField]
    private UpgradeScreen upgrades;

    public Tooltip GetTooltip()
    {
        return tooltip;
    }

    public DialogueSystem GetDialogue()
    {
        return dialogue;
    }

    public Tooltip GetTreasure()
    {
        return treasure;
    }

    public UpgradeScreen GetUpgradeScreen()
    {
        return upgrades;
    }

    public static UIGlobals Get()
    {
        return GameObject.Find("/UI").GetComponent<UIGlobals>();
    }
}
