using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishWander : MonoBehaviour
{
    SpriteRenderer sprite;
    Rigidbody2D rb;

    Vector3 initial_position;

    float t = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.x > 0.0f)
        {
            sprite.flipX = false;
        }
        else if (rb.velocity.x < 0.0f)
        {
            sprite.flipX = true;
        }

        t += 0.2f * Time.deltaTime;

        float random1 = Mathf.PerlinNoise1D(t) - 0.5f;
        float random2 = Mathf.PerlinNoise1D(t + 20.0f) - 0.5f;

        rb.velocity = 2.0f * random1 * Vector2.right + 0.8f * random2 * Vector2.up;
    }
}
