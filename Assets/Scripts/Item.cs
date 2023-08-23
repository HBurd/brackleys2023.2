using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    Transform player;

    [SerializeField]
    ItemType type;

    [SerializeField]
    int count = 1;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 delta = player.position - transform.position;

        if (delta.sqrMagnitude < 1.0f)
        {
            player.GetComponent<Player>().GiveItem(type, count);
            Destroy(gameObject);
        }
    }
}

public enum ItemType
{
    Treasure,
    OxygenTank,
    Flippers,
    Fish,
}