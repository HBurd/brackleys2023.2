using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    Talkable talkable;
    DialogueSystem dialogue;

    [SerializeField]
    Sprite parrot;

    [SerializeField]
    Sprite pirate;

    [SerializeField]
    TextAsset text_json;

    BoatText loaded_text;
    int text_index = 0;

    // Start is called before the first frame update
    void Start()
    {
        talkable = GetComponent<Talkable>();
        talkable.Interact += Interact;

        dialogue = UIGlobals.Get().GetDialogue();

        loaded_text = JsonUtility.FromJson<BoatText>(text_json.text);
    }

    void Interact()
    {
        if (text_index >= loaded_text.dialogue.Length)
        {
            text_index = 0;
            dialogue.Close();
            return;
        }

        dialogue.Set(loaded_text.dialogue[text_index]);
        text_index += 1;
    }
}

[System.Serializable]
public struct BoatText
{
    public Dialogue[] dialogue;
}