using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talkable : MonoBehaviour
{
    DialogueSystem dialogue;
    Tooltip tooltip = null;
    bool active = false;

    [SerializeField]
    TextAsset text_json;

    Dictionary<string, Dialogue> loaded_text = new Dictionary<string, Dialogue>();
    string current_dialogue;
    int text_index = 0;

    // Start is called before the first frame update
    void Start()
    {
        tooltip = UIGlobals.Get().GetTooltip();
        dialogue = UIGlobals.Get().GetDialogue();

        LoadedText dialogues = JsonUtility.FromJson<LoadedText>(text_json.text);

        current_dialogue = dialogues.start;
        foreach (Dialogue dialogue in dialogues.dialogue)
        {
            loaded_text.Add(dialogue.name, dialogue);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (active && Input.GetMouseButtonDown(1))
        {
            dialogue.Open();
            Interact();
            tooltip.SetText("RMB to advance");
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            active = true;
            tooltip.SetText("RMB to talk");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            active = false;
            tooltip.SetText("");
            dialogue.Close();
        }
    }

    void Interact()
    {
        if (text_index >= loaded_text[current_dialogue].dialogue.Length)
        {
            current_dialogue = loaded_text[current_dialogue].next;
            text_index = 0;
            dialogue.Close();
            return;
        }

        dialogue.Set(loaded_text[current_dialogue].dialogue[text_index]);
        text_index += 1;
    }
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