using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb = null;
    bool in_water = true;
    
    Animator animator = null;

    SpriteRenderer sprite = null;

    DialogueSystem dialogue;

    //[SerializeField]
    //float max_speed = 1.0f;

    [SerializeField]
    float max_power = 1.0f;

    [SerializeField]
    float control_radius = 5.0f;

    [SerializeField]
    float forward_drag = 0.1f;

    [SerializeField]
    float normal_drag_factor = 10.0f;

    [SerializeField]
    SpeedLevel[] speed_levels;

    int speed_level = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        dialogue = UIGlobals.Get().GetDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (!in_water)
        {
            return;
        }

        if (dialogue.IsOpen())
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector3 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition + 10.0f * Vector3.forward);
        Vector3 mouse_delta = mouse_pos - transform.position;

        float angle = Mathf.Atan2(mouse_delta.y, mouse_delta.x);

        if (angle > Mathf.PI)
        {
            angle -= 2.0f * Mathf.PI;
        }
        else if (angle <= -Mathf.PI)
        {
            angle += 2.0f * Mathf.PI;
        }

        sprite.flipY = angle > 0.5f * Mathf.PI || angle < -0.5f * Mathf.PI;

        Vector3 mouse_delta_normalized = mouse_delta.normalized;

        transform.rotation = Quaternion.AngleAxis(angle * 180.0f / Mathf.PI, Vector3.forward);

        Vector2 velocity_forward = Vector2.Dot(rb.velocity, mouse_delta_normalized) * mouse_delta_normalized;
        Vector2 velocity_normal = rb.velocity - velocity_forward;

        Debug.Log(velocity_forward.magnitude);

        if (Input.GetMouseButton(0))
        {
            float target_speed = Mathf.Lerp(0.0f, GetMaxSpeed(), mouse_delta.magnitude / control_radius);
            float power = Mathf.Lerp(max_power, forward_drag * target_speed, velocity_forward.magnitude / target_speed);
            rb.AddForce(power * mouse_delta_normalized);
            animator.SetBool("isSwimming", true);
        } else 
        {
            animator.SetBool("isSwimming", false);
        }

        rb.AddForce(-forward_drag * velocity_forward);
        rb.AddForce(-forward_drag * normal_drag_factor * velocity_normal);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Sky")
        {
            in_water = false;
            rb.gravityScale = 1.0f;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Sky")
        {
            in_water = true;
            rb.gravityScale = 0.0f;
        }
    }

    float GetMaxSpeed()
    {
        return speed_levels[speed_level].max_speed;
    }

    public int GetNextUpgradeCost()
    {
        if (speed_level+ 1 >= speed_levels.Length)
        {
            return -1;
        }
        return speed_levels[speed_level + 1].cost;
    }

    public void Upgrade()
    {
        speed_level += 1;
    }

    public static PlayerMovement Get()
    {
        return GameObject.Find("/Player").GetComponent<PlayerMovement>();
    }
}

[System.Serializable]
struct SpeedLevel
{
    public int cost;
    public float max_speed;
}