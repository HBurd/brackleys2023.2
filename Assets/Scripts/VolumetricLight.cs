using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class VolumetricLight : MonoBehaviour
{
    Vector3 start_pos;

    [SerializeField]
    float jitter_radius = 1.0f;

    [SerializeField]
    float jitter_time = 2.0f;

    public bool root = false;

    float t = 0.0f;

    Light2D light_2d;

    [System.NonSerialized]
    public float intensity = 0.0f;

    [System.NonSerialized]
    public float last_pass = 0.0f;

    [System.NonSerialized]
    public float next_pass = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        start_pos = transform.position;
        light_2d = GetComponent<Light2D>();
        light_2d.intensity = intensity;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (t >= jitter_time)
        {
            t -= jitter_time;
        }

        float angle = 2.0f * Mathf.PI * t / jitter_time;

        Vector2 sample_point = (Vector2)start_pos + jitter_radius * (Mathf.Cos(angle) * Vector2.right + Mathf.Sin(angle) * Vector2.up);

        float dx = Mathf.PerlinNoise(sample_point.x, sample_point.y);
        float dy = Mathf.PerlinNoise(sample_point.x + 1.0f, sample_point.y);

        Vector3 position = start_pos + 5.0f * (Vector3.right * dx + Vector3.up * dy);
        transform.position = position;
    }
}
