using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class UIGlobals : MonoBehaviour
{
    [SerializeField]
    double red_flash_time = 1.0f;
    double red_flash_start = -10.0f;

    float fade_amount;

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
    private ProgressBar depth;
    [SerializeField]
    TMP_Text depth_label;

    [SerializeField]
    private UnityEngine.UI.Image fade;

    [SerializeField]
    private InventoryCounter echolocation_level;

    [SerializeField]
    private InventoryCounter oxygen_level;

    [SerializeField]
    private InventoryCounter speed_level;

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

    public void SetEcholocationUpgradeLevel(int level)
    {
        echolocation_level.SetValue(level + 1);
    }

    public void SetOxygenUpgradeLevel(int level)
    {
        oxygen_level.SetValue(level + 1);
    }
    public void SetSpeedUpgradeLevel(int level)
    {
        speed_level.SetValue(level + 1);
    }

    public void SetDepth(float current_depth, float max_depth)
    {
        if (current_depth < 0.0f)
        {
            current_depth = 0.0f;
        }

        float t = current_depth / max_depth;

        depth.SetValue(t);
        depth_label.text = "Depth: " + Mathf.Round(current_depth).ToString() + "m";
    }


    public static UIGlobals Get()
    {
        return GameObject.Find("/UI").GetComponent<UIGlobals>();
    }

    public void SetFade(float value)
    {
        fade_amount = value;
    }

    public void DoRedFlash()
    {
        red_flash_start = Time.timeAsDouble;
    }

    void Update()
    {
        double red_flash_t = (Time.timeAsDouble - red_flash_start) / red_flash_time;

        // preserve fade alpha
        Color flash_color = new Color(0.4f, 0.0f, 0.0f, 0.5f);
        Color fade_color = new Color(0.0f, 0.0f, 0.0f, fade_amount);
        fade.color = Color.Lerp(flash_color, fade_color, (float)red_flash_t);
    }
}
