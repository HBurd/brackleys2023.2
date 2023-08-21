using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb = null;
    bool in_water = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!in_water)
        {
            return;
        }

        Vector3 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition + 10.0f * Vector3.forward);
        Vector3 mouse_delta = mouse_pos - transform.position;
        float angle = Mathf.Atan2(mouse_delta.y, mouse_delta.x);

        Vector3 mouse_delta_normalized = mouse_delta.normalized;

        transform.rotation = Quaternion.AngleAxis(angle * 180.0f / Mathf.PI, Vector3.forward);

        if (Input.GetMouseButton(0))
        {
            rb.AddForce(mouse_delta);
        }

        Vector2 velocity_forward = Vector2.Dot(rb.velocity, mouse_delta_normalized) * mouse_delta_normalized;
        Vector2 velocity_normal = rb.velocity - velocity_forward;

        rb.AddForce(-0.1f * velocity_forward);
        rb.AddForce(-0.5f * velocity_normal);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Sky")
        {
            return;
        }
        in_water = false;
        rb.gravityScale = 1.0f;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "Sky")
        {
            return;
        }
        in_water = true;
        rb.gravityScale = 0.0f;
    }
}
