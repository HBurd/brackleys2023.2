using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public UnityEngine.UI.Image damageFlashImage; // Reference to the UI Image for the damage flash effect
    public float flashSpeed = 5f; // How quickly the damaage flash effect fades away
    private bool isDamaged = false; // controls damage flash

    Rigidbody2D rb = null;
    bool in_water = true;
    bool in_air = false;

    Animator animator = null;

    SpriteRenderer sprite = null;

    UIGlobals ui;
    DialogueSystem dialogue;

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

    [SerializeField]
    OxygenLevel[] oxygen_levels;

    int speed_level = 0;
    int oxygen_level = 0;

    float current_oxygen = 0.0f;

    ProgressBar oxygen_bar = null;
    ProgressBar boost_bar = null;

    bool can_blow = true;

    [SerializeField]
    float boost_amount = 1.0f;

    [SerializeField]
    float boost_factor = 2.0f;

    float current_boost = 1.0f;

    [SerializeField]
    Transform particles;

    Vector3 initial_position;

    // Start is called before the first frame update
    void Start()
    {
        ui = UIGlobals.Get();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        dialogue = ui.GetDialogue();

        oxygen_bar = ui.GetOxygenBar();
        boost_bar = ui.GetBoostBar();

        current_oxygen = GetMaxOxygen();

        initial_position = transform.position;

        current_boost = boost_amount;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSwimming();
        UpdateOxygen();
        FlashScreenOnHit();
    }

    void UpdateSwimming()
    {
        if (in_air && !in_water)
        {
            rb.gravityScale = 1.0f;
        }
        else if (in_air)
        {
            rb.gravityScale = 0.5f;
        }
        else
        {
            can_blow = true;
            rb.gravityScale = 0.0f;
        }

        if (!Input.GetButton("Boost"))
        {
            current_boost += 0.25f * Time.deltaTime;
            current_boost = Mathf.Clamp(current_boost, 0.0f, 1.0f);
            boost_bar.SetValue(current_boost / boost_amount);
        }

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

        if (sprite.flipY)
        {
            particles.localRotation = Quaternion.AngleAxis(90.0f, Vector3.right);
        }
        else
        {
            particles.localRotation = Quaternion.AngleAxis(-90.0f, Vector3.right);
        }

        Vector3 mouse_delta_normalized = mouse_delta.normalized;

        transform.rotation = Quaternion.AngleAxis(angle * 180.0f / Mathf.PI, Vector3.forward);

        Vector2 velocity_forward = Vector2.Dot(rb.velocity, mouse_delta_normalized) * mouse_delta_normalized;
        Vector2 velocity_normal = rb.velocity - velocity_forward;

        if (Input.GetMouseButton(0))
        {
            float current_boost_factor = 1.0f;
            if (Input.GetButton("Boost") && current_boost > 0.0f)
            {
                current_boost_factor = boost_factor;
                current_boost -= Time.deltaTime;
                current_boost = Mathf.Clamp(current_boost, 0.0f, 1.0f);
                boost_bar.SetValue(current_boost / boost_amount);
            }

            float target_speed = Mathf.Lerp(0.0f, current_boost_factor * GetMaxSpeed(), mouse_delta.magnitude / control_radius);
            float power = Mathf.Lerp(current_boost_factor * max_power, forward_drag * target_speed, velocity_forward.magnitude / target_speed);
            rb.AddForce(power * mouse_delta_normalized);
            animator.SetBool("isSwimming", true);
        }
        else
        {
            animator.SetBool("isSwimming", false);
        }

        rb.AddForce(-forward_drag * velocity_forward);
        rb.AddForce(-forward_drag * normal_drag_factor * velocity_normal);
    }

    void UpdateOxygen()
    {
        if (!dialogue.IsOpen())
        {
            current_oxygen -= Time.deltaTime;
        }

        if (current_oxygen < 10.0f)
        {
            ui.SetFade(Mathf.Clamp(1.0f - current_oxygen / 10.0f, 0.0f, 1.0f));
        }
        else
        {
            ui.SetFade(0.0f);
        }

        if (current_oxygen <= 0.0f)
        {
            Die();
        }

        oxygen_bar.SetValue(current_oxygen / GetMaxOxygen());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Sky")
        {
            in_air = true;
        }
        else if (other.tag == "Water")
        {
            in_water = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Sky")
        {
            in_air = false;
        }
        else if (other.tag == "Water")
        {
            in_water = false;

            current_oxygen = GetMaxOxygen();

            if (can_blow)
            {
                can_blow = false;
                ParticleSystem particle_system = particles.gameObject.GetComponent<ParticleSystem>();
                particle_system.time = 0.0f;
                particle_system.Play();
            }
        }
    }

    float GetMaxSpeed()
    {
        return speed_levels[speed_level].max_speed;
    }

    float GetMaxOxygen()
    {
        return oxygen_levels[oxygen_level].time;
    }

    public int GetNextSpeedUpgradeCost()
    {
        if (speed_level + 1 >= speed_levels.Length)
        {
            return -1;
        }
        return speed_levels[speed_level + 1].cost;
    }

    public int GetNextOxygenUpgradeCost()
    {
        if (oxygen_level + 1 >= oxygen_levels.Length)
        {
            return -1;
        }
        return oxygen_levels[oxygen_level + 1].cost;
    }

    public void UpgradeSpeed()
    {
        speed_level += 1;
    }

    public void UpgradeOxygen()
    {
        oxygen_level += 1;

        current_oxygen = GetMaxOxygen();
    }

    public void TakeDamage(int amount)
    {
        current_oxygen -= amount;
        isDamaged = true; // Set flag to trigger damage flash

        // Check for death
        if (current_oxygen <= 0)
        {
            // Dolphin Die :(
            Debug.Log("Dolphin mans has died!");
        }
    }

    private void FlashScreenOnHit()
    {
        if (isDamaged)
        {
            // Set the flash image's color to fully opaque
            damageFlashImage.color = new Color(1, 0, 0, 0.2f);
        }
        else
        {
            // Otherwise, fade the flash image
            damageFlashImage.color = Color.Lerp(damageFlashImage.color, new Color(1, 0, 0, 0), flashSpeed * Time.deltaTime);
        }

        isDamaged = false;
    }

    public static PlayerMovement Get()
    {
        return GameObject.Find("/Player").GetComponent<PlayerMovement>();
    }

    void Die()
    {
        GetComponent<Player>().Die();
        transform.position = initial_position;
        current_oxygen = GetMaxOxygen();
        current_boost = boost_amount;
    }
}

[System.Serializable]
struct SpeedLevel
{
    public int cost;
    public float max_speed;
}

[System.Serializable]
struct OxygenLevel
{
    public int cost;
    public float time;
    public float depth1;
    public float depth2;
}