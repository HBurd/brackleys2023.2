using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talkable : MonoBehaviour
{
    DialogueSystem dialogue;
    Tooltip tooltip = null;

    [SerializeField]
    TextAsset text_json;

    Dictionary<string, Dialogue> loaded_text = new Dictionary<string, Dialogue>();
    string current_dialogue;
    int text_index = 0;

    HashSet<string> past_states = new HashSet<string>();
    List<QueuedText> queued_text = new List<QueuedText>();

    Player player;

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
        if (player != null && Input.GetMouseButtonDown(1))
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
            player = other.GetComponent<Player>();
            tooltip.SetText("RMB to talk");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player = null;
            tooltip.SetText("");
            dialogue.Close();
        }
    }

    bool PopQueuedText()
    {
        if (text_index > 0)
        {
            return false;
        }

        foreach (QueuedText text in queued_text)
        {
            if (past_states.Contains(text.after))
            {
                current_dialogue = text.name;
                queued_text.Remove(text);
                return true;
            }
        }
        return false;
    }

    void Interact()
    {
        if (text_index >= loaded_text[current_dialogue].dialogue.Length)
        {
            past_states.Add(current_dialogue);
            current_dialogue = loaded_text[current_dialogue].next;
            text_index = 0;
            if (!PopQueuedText())
            {
                dialogue.Close();
            }
            return;
        }

        dialogue.Set(loaded_text[current_dialogue].dialogue[text_index]);
        text_index += 1;
    }

    public void SayAfter(string name, string after)
    {
        queued_text.Add(new QueuedText { name = name, after = after });
        PopQueuedText();
    }
}

struct QueuedText
{
    public string name;
    public string after;
}