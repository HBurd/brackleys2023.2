using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField]
    TMP_Text text;

    [SerializeField]
    Animator animator;

    [SerializeField]
    TMP_Text character_name;

    [SerializeField]
    double slide_time = 1.0f;

    [SerializeField]
    RuntimeAnimatorController wally;

    [SerializeField]
    RuntimeAnimatorController parrot;

    [SerializeField]
    RuntimeAnimatorController pirate;

    double slide_end_time = 0.0f;


    RectTransform image_tf;
    RectTransform panel_tf;

    Vector2 image_offscreen;
    Vector2 panel_offscreen;

    bool activated = false;

    void Update()
    {
        float t = 1.0f - (float)(slide_end_time - Time.timeAsDouble) / (float)slide_time;

        if (activated)
        {
            image_tf.anchoredPosition = Vector2.Lerp(image_offscreen, Vector2.zero, t);
            panel_tf.anchoredPosition = Vector2.Lerp(panel_offscreen, Vector2.zero, t);
        }
        else
        {
            image_tf.anchoredPosition = Vector2.Lerp(Vector2.zero, image_offscreen, t);
            panel_tf.anchoredPosition = Vector2.Lerp(Vector2.zero, panel_offscreen, t);

            if (t > 1.0f)
            {
                gameObject.SetActive(false);
                animator.gameObject.SetActive(false);
            }
        }
    }

    public void Open()
    {
        if (activated)
        {
            return;
        }

        gameObject.SetActive(true);
        animator.gameObject.SetActive(true);


        slide_end_time = Time.timeAsDouble + slide_time;

        image_tf = animator.GetComponent<RectTransform>();
        image_offscreen = 500.0f * Vector2.left;

        panel_tf = GetComponent<RectTransform>();
        panel_offscreen = 300.0f * Vector2.down;

        image_tf.anchoredPosition = image_offscreen;
        panel_tf.anchoredPosition = panel_offscreen;

        activated = true;
    }

    public void Set(DialogueEntry dialogue)
    {
        text.text = dialogue.text;
        RuntimeAnimatorController anim = LookupAnimation(dialogue.name);
        if (anim != null)
        {
            animator.runtimeAnimatorController = anim;
        }
        character_name.text = dialogue.name;
    }

    public void Close()
    {
        activated = false;
        slide_end_time = Time.timeAsDouble + slide_time;
    }

    public bool IsOpen()
    {
        return activated;
    }

    RuntimeAnimatorController LookupAnimation(string name)
    {
        if (name == "Scook")
        {
            return parrot;
        }
        else if (name == "Carl")
        {
            return pirate;
        }
        else if (name == "Wally")
        {
            return wally;
        }

        return null;
    }
}

[System.Serializable]
public struct DialogueEntry
{
    public string name;
    public string text;
}

[System.Serializable]
public struct Dialogue
{
    public string name;
    public string next;
    public DialogueEntry[] dialogue;
}

[System.Serializable]
public struct LoadedText
{
    public string start;
    public Dialogue[] dialogue;
}