using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talkable : MonoBehaviour
{
    DialogueSystem dialogue;
    Tooltip tooltip = null;
    bool active = false;

    public delegate void InteractHandler();

    public event InteractHandler Interact;

    // Start is called before the first frame update
    void Start()
    {
        tooltip = UIGlobals.Get().GetTooltip();
        dialogue = UIGlobals.Get().GetDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (active && Input.GetMouseButtonDown(1))
        {
            dialogue.Open();
            Interact?.Invoke();
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
}
