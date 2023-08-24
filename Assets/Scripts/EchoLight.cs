using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EchoLight: MonoBehaviour
{
    public double timeout = 1.0;

    double time_of_death = 0.0;

    public float initial_intensity = 0.2f;


    Light2D light_2d;

    // Start is called before the first frame update
    void Start()
    {
        time_of_death = Time.timeAsDouble + timeout;
        light_2d = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        double time_remaining = time_of_death - Time.timeAsDouble;
        double time_alive = timeout - time_remaining;
        double intensity = time_alive < 0.1 ? (initial_intensity * time_alive / 0.1)
            : (-initial_intensity * (time_alive - timeout) / (timeout - 0.1));

        light_2d.intensity = (float)intensity;
        if (time_remaining <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
