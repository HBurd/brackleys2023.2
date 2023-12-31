using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb = null;
    bool in_water = true;
    bool in_air = false;

    double damage_timeout = 0.0f;

    Animator animator = null;

    SpriteRenderer sprite = null;

    UIGlobals ui;
    DialogueSystem dialogue;

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
            current_boost += 0.5f * Time.deltaTime;
            current_boost = Mathf.Clamp(current_boost, 0.0f, boost_amount);
            boost_bar.SetValue(current_boost / boost_amount);
        }

        if (dialogue.IsOpen())
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0.0f;
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
            float current_boost_factor = 0.0f;
            if (Input.GetButton("Boost") && current_boost > 0.0f)
            {
                current_boost_factor = boost_factor;
                current_boost -= Time.deltaTime;
                current_boost = Mathf.Clamp(current_boost, 0.0f, boost_amount);
                boost_bar.SetValue(current_boost / boost_amount);
            }

            float target_speed = Mathf.Lerp(0.0f, GetMaxSpeed(), mouse_delta.magnitude / control_radius);
            float power = current_boost_factor + Mathf.Lerp(GetMaxPower(), forward_drag * target_speed, velocity_forward.magnitude / target_speed);
            rb.AddForce(power * mouse_delta_normalized);
            animator.SetBool("isSwimming", true);
        }
        else
        {
            animator.SetBool("isSwimming", false);
        }

        //Debug.Log(velocity_forward.magnitude);

        rb.AddForce(-forward_drag * velocity_forward);
        rb.AddForce(-forward_drag * normal_drag_factor * velocity_normal);
    }

    void UpdateOxygen()
    {
        ui.SetDepth(-transform.position.y, oxygen_levels[oxygen_level].max_depth);

        if (in_air && !in_water)
        {
            current_oxygen = GetMaxOxygen();
        }

        if (!dialogue.IsOpen())
        {
            if (-transform.position.y > oxygen_levels[oxygen_level].max_depth)
            {
                TakeDamage(5);
            }

            float oxygen_deplete_factor = 1.0f;
            current_oxygen -= oxygen_deplete_factor * Time.deltaTime;
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

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Sky")
        {
            in_air = true;
        }
        else if (other.tag == "Water")
        {
            in_water = true;
        }
        else if (other.tag == "OxygenRefill")
        {
            current_oxygen = GetMaxOxygen();
        }
        else if (other.tag == "Spike")
        {
            TakeDamage(10);
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

    float GetMaxPower()
    {
        return in_water ? speed_levels[speed_level].power : 0.1f;
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
        if (speed_level == speed_levels.Length - 1)
        {
            return;
        }

        speed_level += 1;
        ui.SetSpeedUpgradeLevel(speed_level);
    }

    public void UpgradeOxygen()
    {
        if (oxygen_level == oxygen_levels.Length - 1)
        {
            return;
        }
        oxygen_level += 1;
        ui.SetOxygenUpgradeLevel(oxygen_level);
        current_oxygen = GetMaxOxygen();
    }

    void TakeDamage(int amount)
    {
        if (Time.timeAsDouble < damage_timeout)
        {
            return;
        }

        damage_timeout += Time.timeAsDouble + 3.0;


        current_oxygen -= amount;
        ui.DoRedFlash();
    }

    public static PlayerMovement Get()
    {
        return GameObject.Find("/Player").GetComponent<PlayerMovement>();
    }

    void Die()
    {
        // Dolphin Die :(
        Debug.Log("Dolphin mans has died!");
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
    public float power;
}

[System.Serializable]
struct OxygenLevel
{
    public int cost;
    public float time;
    public float max_depth;
    public float threshold_deplete_factor;
}