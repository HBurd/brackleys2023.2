using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int treasure = 0;

    Tooltip tooltip = null;

    [SerializeField]
    float sonar_rate = 1.0f;

    [SerializeField]
    float sonar_angle = 20.0f;

    double next_ping_time = 0.0;

    [SerializeField]
    GameObject sonar_light;

    void Start()
    {
        tooltip = GameObject.Find("UI/TreasureDisplay").GetComponent<Tooltip>();
        GiveTreasure(0);
    }

    public void GiveTreasure(int value)
    {
        treasure += value;
        tooltip.SetText(treasure.ToString());
    }

    void Update()
    {
        if (Time.timeAsDouble > next_ping_time)
        {
            float random_angle = Random.Range(-sonar_angle, sonar_angle) * Mathf.PI / 180.0f;

            Vector2 dir = (transform.right.x * Mathf.Cos(random_angle) - transform.right.y * Mathf.Sin(random_angle)) * Vector2.right
                + (transform.right.x * Mathf.Sin(random_angle) + transform.right.y * Mathf.Cos(random_angle)) * Vector2.up;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 20.0f, LayerMask.GetMask("Environment"));
            if (hit.collider != null)
            {
                Instantiate(sonar_light, hit.point, Quaternion.identity);
            }

            next_ping_time = Time.timeAsDouble + 1.0f / sonar_rate;
        }
    }
}
