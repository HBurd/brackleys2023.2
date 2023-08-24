using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Echolocation : MonoBehaviour
{

    [SerializeField]
    float sonar_angle = 20.0f;

    double next_ping_time = 0.0;

    [SerializeField]
    GameObject sonar_light;

    [SerializeField]
    float max_sonar_intensity = 0.1f;

    [SerializeField]
    float sonar_range = 10.0f;

    [SerializeField]
    EcholocationLevel[] echolocation_levels;

    int echolocation_level = 0;

    // Update is called once per frame
    void Update()
    {
        if (Time.timeAsDouble > next_ping_time)
        {
            float random_angle = Random.Range(-sonar_angle, sonar_angle) * Mathf.PI / 180.0f;

            Vector2 dir = (transform.right.x * Mathf.Cos(random_angle) - transform.right.y * Mathf.Sin(random_angle)) * Vector2.right
                + (transform.right.x * Mathf.Sin(random_angle) + transform.right.y * Mathf.Cos(random_angle)) * Vector2.up;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, sonar_range, LayerMask.GetMask("Environment"));
            if (hit.collider != null)
            {
                GameObject light = Instantiate(sonar_light, hit.point, Quaternion.identity);
                light.GetComponent<EchoLight>().initial_intensity = max_sonar_intensity * (1.0f - hit.distance / sonar_range);
                light.GetComponent<EchoLight>().timeout = echolocation_levels[echolocation_level].time;
            }

            next_ping_time = Time.timeAsDouble + 1.0f / echolocation_levels[echolocation_level].rate;
        }
    }

    public int GetNextUpgradeCost()
    {
        if (echolocation_level + 1 >= echolocation_levels.Length)
        {
            return -1;
        }
        return echolocation_levels[echolocation_level + 1].cost;
    }

    public void Upgrade()
    {
        if (echolocation_level == echolocation_levels.Length - 1)
        {
            return;
        }
        echolocation_level += 1;
    }
}

[System.Serializable]
struct EcholocationLevel
{
    public int cost;
    public float time;
    public float rate;
}