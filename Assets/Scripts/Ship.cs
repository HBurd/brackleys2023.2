using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    Talkable talkable;

    // Start is called before the first frame update
    void Start()
    {
        Player.Get().DieEvent += HandleDeath;
        talkable = GetComponent<Talkable>();
    }

    void HandleDeath()
    {
        talkable.SayAfter("died", "intro", 1);
        talkable.Interact();
    }
}
