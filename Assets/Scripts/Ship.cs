using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    Talkable talkable;

    [SerializeField]
    GameObject fish;

    [SerializeField]
    Transform fish_spawn;

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

    public void SpawnFish(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            GameObject new_fish = Instantiate(fish);
            new_fish.transform.position = fish_spawn.position + (Random.Range(-1.0f, 1.0f) * Vector3.right);
            new_fish.GetComponent<Rigidbody2D>().velocity = Vector2.down * 1.0f;
        }
    }
}
