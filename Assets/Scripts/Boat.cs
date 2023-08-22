using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    Talkable talkable;
    DialogueSystem dialogue;

    [SerializeField]
    Sprite parrot;

    // Start is called before the first frame update
    void Start()
    {
        talkable = GetComponent<Talkable>();
        talkable.Interact += Interact;

        dialogue = UIGlobals.Get().GetDialogue();
    }

    void Interact()
    {
        dialogue.Open("I'm a boat", parrot);
    }
}
