using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    Transform player;

    Rigidbody2D rb;

    float magnet_radius = 5.0f;

    [SerializeField]
    int treasure_value = 1;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 delta = player.position - transform.position;
        if (delta.sqrMagnitude < magnet_radius * magnet_radius)
        {
            rb.velocity = 10.0f * delta.normalized;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

        if (delta.sqrMagnitude < 0.1f)
        {
            Destroy(gameObject);
            player.GetComponent<Player>().GiveTreasure(treasure_value);
        }
    }
}
