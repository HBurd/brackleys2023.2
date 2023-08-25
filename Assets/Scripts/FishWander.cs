using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishWander : MonoBehaviour
{
    SpriteRenderer sprite;
    Rigidbody2D rb;

    Vector3 initial_position;

    float t = 0.0f;
    float perlin_x = 0.0f;
    float perlin_y = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        perlin_x = Random.Range(-1000.0f, 1000.0f);
        perlin_y = Random.Range(-1000.0f, 1000.0f);
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

        t += 0.1f * Time.deltaTime;

        if (t > 2.0f * Mathf.PI)
        {
            t -= 2.0f * Mathf.PI;
        }

        float sample_x = perlin_x + 5.0f * Mathf.Cos(t);
        float sample_y = perlin_y + 5.0f * Mathf.Sin(t);

        float random1 = Mathf.PerlinNoise(sample_x, sample_y) - 0.5f;
        float random2 = Mathf.PerlinNoise(sample_x + 10.0f, sample_y) - 0.5f;

        rb.AddForce(2.0f * random1 * Vector2.right + 0.5f * random2 * Vector2.up);
    }
}
