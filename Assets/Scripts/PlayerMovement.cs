using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb = null;
    bool in_water = true;
    
    Animator animator = null;
    DialogueSystem dialogue = null;
    DialogueSource dialogue_source = null;
    Tooltip tooltip = null;
    SpriteRenderer sprite = null;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        dialogue = GameObject.FindFirstObjectByType<DialogueSystem>(FindObjectsInactive.Include);
        tooltip = GameObject.Find("UI/Tooltip").GetComponent<Tooltip>();
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

        if (Input.GetMouseButton(0))
        {
            rb.AddForce(mouse_delta);
            animator.SetBool("isSwimming", true);
        } else 
        {
            animator.SetBool("isSwimming", false);
        }

        Vector2 velocity_forward = Vector2.Dot(rb.velocity, mouse_delta_normalized) * mouse_delta_normalized;
        Vector2 velocity_normal = rb.velocity - velocity_forward;

        rb.AddForce(-0.1f * velocity_forward);
        rb.AddForce(-0.5f * velocity_normal);


        if (dialogue_source != null && Input.GetMouseButton(1))
        {
            dialogue.Open(dialogue_source);
        }
        else if(dialogue_source == null)
        {
            dialogue.Close();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Sky")
        {
            in_water = false;
            rb.gravityScale = 1.0f;
        }
        else if (other.tag == "Dialogue")
        {
            dialogue_source = other.GetComponent<DialogueSource>();
            tooltip.SetText("RMB to talk");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Sky")
        {
            in_water = true;
            rb.gravityScale = 0.0f;
        }
        else if (other.tag == "Dialogue")
        {
            dialogue_source = null;
            tooltip.SetText("");
        }
    }
}
