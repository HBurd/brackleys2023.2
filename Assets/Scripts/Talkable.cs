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

    public delegate void StateChangeEventHandler(string new_state);
    public event StateChangeEventHandler StateChangeEvent;

    string previous = "intro";

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
        }
    }

    void SetState(string new_state)
    {
        string old_previous = previous;
        previous = current_dialogue;
        current_dialogue = new_state == "previous" ? old_previous : new_state;
    }

    bool PopQueuedText()
    {
        if (text_index > 0)
        {
            return false;
        }

        bool should_pop = false;
        QueuedText say_next = new QueuedText { name = "", after = "", priority = -1000 };

        foreach (QueuedText text in queued_text)
        {
            if (past_states.Contains(text.after) && text.priority > say_next.priority)
            {
                should_pop = true;
                say_next = text;
            }
        }

        if (should_pop)
        {
            SetState(say_next.name);
            queued_text.Remove(say_next);
            return true;
        }

        return false;
    }

    public void Interact()
    {
        dialogue.Open();

        bool dont_pop = false;
        if (text_index >= loaded_text[current_dialogue].dialogue.Length)
        {
            past_states.Add(current_dialogue);
            SetState(loaded_text[current_dialogue].next);
            text_index = 0;
            if (!PopQueuedText())
            {
                dialogue.Close();
                StateChangeEvent?.Invoke("");
                return;
            }
            // successfully popped, don't pop again
            dont_pop = true;
        }

        if (text_index == 0)
        {
            if (!dont_pop)
            {
                PopQueuedText();
            }
            StateChangeEvent?.Invoke(current_dialogue);
        }

        dialogue.Set(loaded_text[current_dialogue].dialogue[text_index]);
        text_index += 1;
    }

    public void SayAfter(string name, string after, int priority)
    {
        // make sure not to add a duplicate
        foreach (QueuedText text in queued_text)
        {
            if (text.name == name)
            {
                return;
            }
        }
        queued_text.Add(new QueuedText { name = name, after = after, priority = priority });
    }
}

struct QueuedText
{
    public string name;
    public string after;
    public int priority;
}