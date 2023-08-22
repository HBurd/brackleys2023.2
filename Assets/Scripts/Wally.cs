using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wally : MonoBehaviour
{
    Talkable talkable;
    DialogueSystem dialogue;

    [SerializeField]
    Sprite portrait;

    [SerializeField]
    TextAsset text_json;

    WhaleText loaded_text;
    int text_index = 0;

    // Start is called before the first frame update
    void Start()
    {
        talkable = GetComponent<Talkable>();
        talkable.Interact += Interact;

        dialogue = UIGlobals.Get().GetDialogue();

        loaded_text = JsonUtility.FromJson<WhaleText>(text_json.text);
    }

    void Interact()
    {
        if (text_index >= loaded_text.intro.Length)
        {
            text_index = 0;
            dialogue.Close();
            return;
        }

        dialogue.Set(loaded_text.intro[text_index]);
        text_index += 1;
    }
}

[System.Serializable]
public struct WhaleText
{
    public Dialogue[] intro;
}